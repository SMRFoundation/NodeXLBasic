/* =============================================================================
 * viz.js  —  render the 2-D projection of the embedding space as live SVG
 * -----------------------------------------------------------------------------
 * Draws: shaded concept regions (convex hulls of each cluster), centroid labels,
 * word points (revealed as they are guessed), and the bridges/edges players
 * build between regions. Ownership tinting + hover tooltips included.
 * ===========================================================================*/

(function (global) {
  "use strict";
  const E = global.RAG_ENGINE;
  const SVGNS = "http://www.w3.org/2000/svg";
  const el = (n, a) => {
    const e = document.createElementNS(SVGNS, n);
    for (const k in (a || {})) e.setAttribute(k, a[k]);
    return e;
  };

  const SIZE = 1000, PAD = 80;
  const X = (x) => PAD + x * (SIZE - 2 * PAD);
  const Y = (y) => PAD + (1 - y) * (SIZE - 2 * PAD); // flip so up = +y
  const clamp = (v, lo, hi) => Math.max(lo, Math.min(hi, v));

  // smooth-ish blob path from hull points, padded outward from centroid
  function blobPath(hull, cx, cy, pad) {
    if (!hull.length) return "";
    const pts = hull.map((p) => {
      const dx = p.x - cx, dy = p.y - cy;
      const len = Math.hypot(dx, dy) || 1;
      return { x: p.x + (dx / len) * pad, y: p.y + (dy / len) * pad };
    });
    // Catmull-Rom → cubic bezier, closed
    const n = pts.length;
    let d = `M ${pts[0].x.toFixed(1)} ${pts[0].y.toFixed(1)} `;
    for (let i = 0; i < n; i++) {
      const p0 = pts[(i - 1 + n) % n], p1 = pts[i], p2 = pts[(i + 1) % n], p3 = pts[(i + 2) % n];
      const c1x = p1.x + (p2.x - p0.x) / 6, c1y = p1.y + (p2.y - p0.y) / 6;
      const c2x = p2.x - (p3.x - p1.x) / 6, c2y = p2.y - (p3.y - p1.y) / 6;
      d += `C ${c1x.toFixed(1)} ${c1y.toFixed(1)}, ${c2x.toFixed(1)} ${c2y.toFixed(1)}, ${p2.x.toFixed(1)} ${p2.y.toFixed(1)} `;
    }
    return d + "Z";
  }

  function Viz(svg, tooltip) {
    this.svg = svg;
    this.tooltip = tooltip;
    this.proj = E.projectPCA();
    this.ptById = new Map(this.proj.points.map((p) => [p.id, p]));
    this.onPointClick = null;

    // static layers (order matters)
    this.gEdgesBase = el("g", { class: "layer-edges-base" });
    this.gRegions = el("g", { class: "layer-regions" });
    this.gEdges = el("g", { class: "layer-edges" });
    this.gGhost = el("g", { class: "layer-ghost" });
    this.gPoints = el("g", { class: "layer-points" });
    this.gLabels = el("g", { class: "layer-labels" });
    svg.setAttribute("viewBox", `0 0 ${SIZE} ${SIZE}`);
    [this.gEdgesBase, this.gRegions, this.gEdges, this.gGhost, this.gPoints, this.gLabels]
      .forEach((g) => svg.appendChild(g));

    this._buildRegions();
  }

  Viz.prototype._buildRegions = function () {
    this.regionPaths = {};
    E.clusterKeys.forEach((key) => {
      const members = E.WORDS.filter((w) => w.cluster === key).map((w) => this.ptById.get(w.id));
      const scr = members.map((p) => ({ x: X(p.x), y: Y(p.y) }));
      const hull = E.convexHull(scr);
      const cx = scr.reduce((s, p) => s + p.x, 0) / scr.length;
      const cy = scr.reduce((s, p) => s + p.y, 0) / scr.length;
      const path = el("path", {
        d: blobPath(hull, cx, cy, 24),
        class: "region",
        fill: E.CLUSTERS[key].color,
        "data-key": key,
      });
      this.gRegions.appendChild(path);
      this.regionPaths[key] = { path, cx, cy };
    });
  };

  // faint dots for every embedding (teaching "show the whole space")
  Viz.prototype.setGhost = function (show) {
    this.gGhost.innerHTML = "";
    if (!show) return;
    this.proj.points.forEach((p) => {
      this.gGhost.appendChild(el("circle", {
        cx: X(p.x), cy: Y(p.y), r: 4, class: "ghost-dot",
        fill: E.CLUSTERS[E.WORDS[p.id].cluster].color,
      }));
    });
  };

  Viz.prototype._tip = function (evt, html) {
    if (!html) { this.tooltip.style.display = "none"; return; }
    this.tooltip.innerHTML = html;
    this.tooltip.style.display = "block";
    const r = this.svg.getBoundingClientRect();
    this.tooltip.style.left = evt.clientX - r.left + 14 + "px";
    this.tooltip.style.top = evt.clientY - r.top + 14 + "px";
  };

  // full re-render from game state
  Viz.prototype.render = function (state) {
    const PLAYERS = state.players || {};
    // 1) region ownership tint
    E.clusterKeys.forEach((key) => {
      const rp = this.regionPaths[key];
      const owner = (state.control || {})[key];
      rp.path.setAttribute("class", "region" + (owner ? " owned" : ""));
      rp.path.setAttribute("stroke", owner ? PLAYERS[owner].color : E.CLUSTERS[key].color);
      rp.path.setAttribute("fill", owner ? PLAYERS[owner].color : E.CLUSTERS[key].color);
      rp.path.style.opacity = owner ? 0.9 : 1;
      const captured = (state.captured || {})[key];
      rp.path.classList.toggle("captured", !!captured);
      rp.path.classList.toggle("atari", !!(state.atari && state.atari.has(key)));
    });

    // 2) base adjacency (faint roadmap of bridgeable regions)
    this.gEdgesBase.innerHTML = "";
    if (state.showRoadmap) {
      (state.roadmap || []).forEach((e) => {
        const a = this.proj.centroids2d[e.a], b = this.proj.centroids2d[e.b];
        this.gEdgesBase.appendChild(el("line", {
          x1: X(a.x), y1: Y(a.y), x2: X(b.x), y2: Y(b.y), class: "edge-base",
        }));
      });
    }

    // 3) built bridges
    this.gEdges.innerHTML = "";
    (state.bridges || []).forEach((br) => {
      const a = this.proj.centroids2d[br.a], b = this.proj.centroids2d[br.b];
      const color = br.owner ? PLAYERS[br.owner].color : "#9aa4b2";
      this.gEdges.appendChild(el("line", {
        x1: X(a.x), y1: Y(a.y), x2: X(b.x), y2: Y(b.y),
        class: "edge-built", stroke: color, "stroke-width": 4 + 6 * (br.strength || 0),
      }));
      // the bridge word sits on the line
      const wp = this.ptById.get(br.wordId);
      if (wp) {
        this.gEdges.appendChild(el("circle", {
          cx: X(wp.x), cy: Y(wp.y), r: 9, class: "bridge-node", fill: color,
        }));
      }
    });

    // 4) revealed word points
    this.gPoints.innerHTML = "";
    this.gLabels.innerHTML = "";
    const revealed = state.revealed || new Set();
    const self = this;
    this.proj.points.forEach((p) => {
      if (!revealed.has(p.id)) return;
      const wd = E.WORDS[p.id];
      const owner = (state.wordOwner || {})[p.id];
      const color = owner ? PLAYERS[owner].color : E.CLUSTERS[wd.cluster].color;
      const c = el("circle", { cx: X(p.x), cy: Y(p.y), r: 8, class: "wordpt", fill: color });
      c.addEventListener("mousemove", (ev) => self._tip(ev, self._pointHtml(wd, state)));
      c.addEventListener("mouseleave", () => self._tip(null));
      c.addEventListener("click", () => self.onPointClick && self.onPointClick(wd));
      this.gPoints.appendChild(c);
      const t = el("text", { x: X(p.x) + 12, y: Y(p.y) + 4, class: "wordlbl" });
      t.textContent = wd.word;
      this.gLabels.appendChild(t);
    });

    // 5) highlight target regions (single-player challenge)
    E.clusterKeys.forEach((key) => {
      const rp = this.regionPaths[key];
      rp.path.classList.toggle("target", (state.targets || []).includes(key));
    });

    // 6) centroid labels
    this._labelCentroids(state);
  };

  Viz.prototype._labelCentroids = function (state) {
    E.clusterKeys.forEach((key) => {
      const c = this.proj.centroids2d[key];
      const owner = (state.control || {})[key];
      const g = el("g", { class: "centroid-label" });
      const label = E.CLUSTERS[key].name + (owner ? " ●" : "");
      const lx = clamp(X(c.x), 60, SIZE - 60), ly = clamp(Y(c.y), 34, SIZE - 20);
      const t = el("text", { x: lx, y: ly, class: "cl-name", fill: owner ? state.players[owner].color : "#e8edf4" });
      t.textContent = label;
      g.appendChild(t);
      this.gLabels.appendChild(g);
    });
  };

  Viz.prototype._pointHtml = function (wd, state) {
    const top = Object.entries(wd.vec).sort((a, b) => b[1] - a[1]).slice(0, 3)
      .map(([k, v]) => `<b>${k}</b> ${v.toFixed(2)}`).join(" · ");
    let extra = "";
    if (state.targets && state.targets.length === 2) {
      const bs = E.bridgeScore(wd, state.targets[0], state.targets[1]);
      extra = `<div class="tt-bridge">→ ${E.CLUSTERS[state.targets[0]].name} ${bs.simA.toFixed(2)} · ${E.CLUSTERS[state.targets[1]].name} ${bs.simB.toFixed(2)}<br>bridge strength <b>${bs.strength.toFixed(2)}</b></div>`;
    }
    return `<div class="tt-word">${wd.word}</div><div class="tt-dims">${top}</div>${extra}`;
  };

  // Briefly flash a probe point that isn't a permanent placement.
  Viz.prototype.flashProbe = function (wordId, color) {
    const p = this.ptById.get(wordId);
    if (!p) return;
    const c = el("circle", { cx: X(p.x), cy: Y(p.y), r: 6, class: "probe-flash",
      fill: "none", stroke: color || "#fff", "stroke-width": 3 });
    this.gGhost.appendChild(c);
    let r = 6;
    const iv = setInterval(() => {
      r += 4; c.setAttribute("r", r); c.style.opacity = Math.max(0, 1 - (r - 6) / 60);
      if (r > 66) { clearInterval(iv); c.remove(); }
    }, 24);
  };

  global.RAG_VIZ = { Viz };
})(typeof window !== "undefined" ? window : globalThis);
