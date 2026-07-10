/* =============================================================================
 * engine.js  —  the vector-space machinery
 * -----------------------------------------------------------------------------
 * Everything a real RAG stack does at retrieval time, in miniature:
 *   • embed()            build a unit vector from the interpretable axes
 *   • cosine()           similarity = the workhorse of semantic search
 *   • nearest()          k-NN retrieval (what RAG runs against your query)
 *   • projectPCA()       high-D → 2-D so we can DRAW the space (t-SNE/UMAP idea)
 *   • cluster centroids  a "region" is summarized by its mean vector
 *   • clusterGraph()     which regions are close enough to be bridgeable
 * ===========================================================================*/

(function (global) {
  "use strict";

  const { DIMS, CLUSTERS, WORDS } = global.RAG_DATA;
  const D = DIMS.length;

  // ---- build normalized vectors -------------------------------------------
  function embed(vecObj) {
    const v = new Float64Array(D);
    for (let i = 0; i < D; i++) v[i] = vecObj[DIMS[i]] || 0;
    let n = 0;
    for (let i = 0; i < D; i++) n += v[i] * v[i];
    n = Math.sqrt(n) || 1;
    for (let i = 0; i < D; i++) v[i] /= n; // unit length → dot product == cosine
    return v;
  }
  WORDS.forEach((w) => (w.vec16 = embed(w.vec)));

  function cosine(a, b) {
    let s = 0;
    for (let i = 0; i < D; i++) s += a[i] * b[i];
    return s; // both are unit vectors
  }

  const byWord = new Map(WORDS.map((w) => [w.word.toLowerCase(), w]));
  const lookup = (word) => byWord.get(String(word || "").trim().toLowerCase()) || null;

  // ---- k nearest neighbours (semantic retrieval) --------------------------
  function nearest(vec, k, exclude) {
    const skip = new Set(exclude || []);
    return WORDS.filter((w) => !skip.has(w.id))
      .map((w) => ({ word: w, sim: cosine(vec, w.vec16) }))
      .sort((a, b) => b.sim - a.sim)
      .slice(0, k);
  }

  // ---- cluster centroids (a "region" summarized by its mean) --------------
  const clusterKeys = Object.keys(CLUSTERS);
  const centroids = {};
  clusterKeys.forEach((key) => {
    const members = WORDS.filter((w) => w.cluster === key);
    const c = new Float64Array(D);
    members.forEach((m) => {
      for (let i = 0; i < D; i++) c[i] += m.vec16[i];
    });
    let n = 0;
    for (let i = 0; i < D; i++) n += c[i] * c[i];
    n = Math.sqrt(n) || 1;
    for (let i = 0; i < D; i++) c[i] /= n;
    centroids[key] = { key, vec: c, members };
  });

  // ---- PCA: high-D → 2-D via power iteration on the covariance matrix ------
  // (Deterministic; this is the "make it visible" step every embedding demo does.)
  function projectPCA() {
    const X = WORDS.map((w) => Array.from(w.vec16));
    const mean = new Float64Array(D);
    X.forEach((row) => row.forEach((v, i) => (mean[i] += v)));
    for (let i = 0; i < D; i++) mean[i] /= X.length;
    const Xc = X.map((row) => row.map((v, i) => v - mean[i]));

    // covariance matrix C = Xcᵀ·Xc
    const C = Array.from({ length: D }, () => new Float64Array(D));
    Xc.forEach((row) => {
      for (let i = 0; i < D; i++)
        for (let j = 0; j < D; j++) C[i][j] += row[i] * row[j];
    });

    const matVec = (M, v) => {
      const out = new Float64Array(D);
      for (let i = 0; i < D; i++) {
        let s = 0;
        for (let j = 0; j < D; j++) s += M[i][j] * v[j];
        out[i] = s;
      }
      return out;
    };
    const norm = (v) => Math.sqrt(v.reduce((s, x) => s + x * x, 0)) || 1;

    // Deterministic seed vector (no Math.random — must be reproducible).
    const powerIter = (M, avoid) => {
      let v = new Float64Array(D);
      for (let i = 0; i < D; i++) v[i] = Math.sin(i + 1) + 0.3 * Math.cos(2 * i + 1);
      for (let it = 0; it < 200; it++) {
        let nv = matVec(M, v);
        // deflate against already-found components (Gram-Schmidt)
        (avoid || []).forEach((u) => {
          let d = 0;
          for (let i = 0; i < D; i++) d += nv[i] * u[i];
          for (let i = 0; i < D; i++) nv[i] -= d * u[i];
        });
        const n = norm(nv);
        for (let i = 0; i < D; i++) nv[i] /= n;
        v = nv;
      }
      return v;
    };

    const pc1 = powerIter(C, []);
    const pc2 = powerIter(C, [pc1]);

    const proj = (vec) => {
      let x = 0, y = 0;
      for (let i = 0; i < D; i++) {
        const cd = vec[i] - mean[i];
        x += cd * pc1[i];
        y += cd * pc2[i];
      }
      return { x, y };
    };

    // project words + centroids, then rescale to [0,1]²
    const pts = WORDS.map((w) => ({ id: w.id, ...proj(w.vec16) }));
    const cen = {};
    clusterKeys.forEach((k) => (cen[k] = proj(centroids[k].vec)));

    const xs = pts.map((p) => p.x), ys = pts.map((p) => p.y);
    const minX = Math.min(...xs), maxX = Math.max(...xs);
    const minY = Math.min(...ys), maxY = Math.max(...ys);
    const sx = (v) => (v - minX) / (maxX - minX || 1);
    const sy = (v) => (v - minY) / (maxY - minY || 1);

    pts.forEach((p) => { p.x = sx(p.x); p.y = sy(p.y); });
    clusterKeys.forEach((k) => { cen[k] = { x: sx(cen[k].x), y: sy(cen[k].y) }; });

    return { points: pts, centroids2d: cen, pc1, pc2 };
  }

  // ---- convex hull (Andrew's monotone chain) for drawing region blobs -----
  function convexHull(points) {
    if (points.length < 3) return points.slice();
    const pts = points.slice().sort((a, b) => a.x - b.x || a.y - b.y);
    const cross = (o, a, b) => (a.x - o.x) * (b.y - o.y) - (a.y - o.y) * (b.x - o.x);
    const lower = [];
    for (const p of pts) {
      while (lower.length >= 2 && cross(lower[lower.length - 2], lower[lower.length - 1], p) <= 0) lower.pop();
      lower.push(p);
    }
    const upper = [];
    for (let i = pts.length - 1; i >= 0; i--) {
      const p = pts[i];
      while (upper.length >= 2 && cross(upper[upper.length - 2], upper[upper.length - 1], p) <= 0) upper.pop();
      upper.push(p);
    }
    lower.pop(); upper.pop();
    return lower.concat(upper);
  }

  // ---- cluster adjacency: which regions can be bridged? -------------------
  // Two regions are "adjacent" if their centroids are similar enough that a
  // real vocabulary word could plausibly straddle them. This is the board's
  // road-map: bridges may only be built along these edges.
  function clusterGraph(threshold) {
    const t = threshold == null ? 0.18 : threshold;
    const edges = [];
    for (let i = 0; i < clusterKeys.length; i++) {
      for (let j = i + 1; j < clusterKeys.length; j++) {
        const a = clusterKeys[i], b = clusterKeys[j];
        const sim = cosine(centroids[a].vec, centroids[b].vec);
        if (sim >= t) edges.push({ a, b, sim });
      }
    }
    return edges;
  }

  // ---- evaluate a candidate bridge between two regions --------------------
  // The heart of the "bridge" idea: a word bridges A and B when it is close to
  // BOTH centroids. Strength = harmonic mean (punishes lopsided words).
  function bridgeScore(word, aKey, bKey) {
    const simA = cosine(word.vec16, centroids[aKey].vec);
    const simB = cosine(word.vec16, centroids[bKey].vec);
    const strength = simA + simB > 0 ? (2 * simA * simB) / (simA + simB) : 0;
    return { simA, simB, strength };
  }

  // Which region does a word most belong to? (nearest centroid = "retrieval")
  function classify(word) {
    let best = null, bestSim = -1, second = null, secondSim = -1;
    clusterKeys.forEach((k) => {
      const s = cosine(word.vec16, centroids[k].vec);
      if (s > bestSim) { second = best; secondSim = bestSim; best = k; bestSim = s; }
      else if (s > secondSim) { second = k; secondSim = s; }
    });
    return { best, bestSim, second, secondSim };
  }

  global.RAG_ENGINE = {
    DIMS, CLUSTERS, WORDS, clusterKeys, centroids,
    embed, cosine, lookup, nearest, projectPCA, convexHull,
    clusterGraph, bridgeScore, classify,
  };
})(typeof window !== "undefined" ? window : globalThis);
