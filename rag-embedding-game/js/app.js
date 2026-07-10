/* =============================================================================
 * app.js  —  UI wiring: input, mode switching, turn flow, rendering
 * ===========================================================================*/

(function () {
  "use strict";
  const E = window.RAG_ENGINE;
  const { Game } = window.RAG_GAME;
  const { Viz } = window.RAG_VIZ;

  const $ = (id) => document.getElementById(id);
  const svg = $("board"), tooltip = $("tooltip");
  const viz = new Viz(svg, tooltip);
  let game = new Game();

  // ---- populate vocabulary autocomplete + legend --------------------------
  const dl = $("vocab");
  E.WORDS.slice().sort((a, b) => a.word.localeCompare(b.word)).forEach((w) => {
    const o = document.createElement("option"); o.value = w.word; dl.appendChild(o);
  });
  const legend = $("legendItems");
  E.clusterKeys.forEach((k) => {
    const d = document.createElement("div");
    d.innerHTML = `<span class="dot" style="background:${E.CLUSTERS[k].color}"></span>${E.CLUSTERS[k].name}`;
    d.title = E.CLUSTERS[k].blurb; legend.appendChild(d);
  });

  // ---- region selection (LEARN targets) via board clicks OR panel chips ---
  function toggleTarget(key) {
    if (game.mode !== "learn" || game.finished) return;
    let t = game.targets.slice();
    if (t.includes(key)) t = t.filter((x) => x !== key);
    else { t.push(key); if (t.length > 2) t.shift(); }
    game.targets = t;
    render(); renderStatus();
  }
  E.clusterKeys.forEach((key) => {
    viz.regionPaths[key].path.addEventListener("click", () => toggleTarget(key));
  });

  // build the reliable panel picker (overlapping blobs make board-clicks fiddly)
  const picker = $("regionPicker");
  E.clusterKeys.forEach((key) => {
    const b = document.createElement("button");
    b.dataset.key = key;
    b.innerHTML = `<span class="rp-dot" style="background:${E.CLUSTERS[key].color}"></span>${E.CLUSTERS[key].name}`;
    b.addEventListener("click", () => toggleTarget(key));
    picker.appendChild(b);
  });
  function renderPicker() {
    picker.classList.toggle("show", game.mode === "learn" && !game.finished);
    picker.querySelectorAll("button").forEach((b) => {
      const on = game.targets.includes(b.dataset.key);
      b.classList.toggle("sel", on);
      b.style.background = on ? E.CLUSTERS[b.dataset.key].color : "";
      b.style.borderColor = on ? E.CLUSTERS[b.dataset.key].color : "";
    });
  }

  // ---- rendering ----------------------------------------------------------
  function render() {
    const st = game.state();
    st.showRoadmap = $("tglRoadmap").checked;
    viz.render(st);
  }
  function setGhost() { viz.setGhost($("tglGhost").checked); }
  $("tglGhost").addEventListener("change", setGhost);
  $("tglRoadmap").addEventListener("change", render);

  function feedback(msg, cls, extra) {
    const f = $("feedback");
    f.className = "feedback" + (cls ? " " + cls : "");
    f.innerHTML = msg + (extra ? `<div class="probe-nn">${extra}</div>` : "");
  }

  function pushLog(msg) {
    if (!msg) return;
    const li = document.createElement("li"); li.textContent = msg;
    $("log").prepend(li);
  }

  function chip(key) {
    return `<span class="pair-chip" style="border-color:${E.CLUSTERS[key].color};color:${E.CLUSTERS[key].color}">${E.CLUSTERS[key].name}</span>`;
  }

  // ---- mode-specific status + scoreboard ----------------------------------
  function renderStatus() {
    renderPicker();
    const s = $("modeStatus");
    if (game.mode === "learn") {
      const sc = game.learnScore();
      if (game.finished) {
        s.innerHTML = `<div class="headline">🎉 Fully connected!</div>
          One conceptual graph from ${sc.regions} regions in ${sc.bridges} bridges.`;
      } else if (game.targets.length === 2) {
        s.innerHTML = `<div class="headline">Bridge these two regions</div>
          ${chip(game.targets[0])} &nbsp;↔&nbsp; ${chip(game.targets[1])}
          <div style="color:var(--muted);font-size:12px;margin-top:6px">Type a word close to BOTH — or click regions to choose a different pair. <b>Probe</b> to test without committing.</div>`;
      } else {
        s.innerHTML = `<div class="headline">Pick two regions to bridge</div>
          <div style="color:var(--muted);font-size:12px">Click two shaded regions on the board (${game.targets.length}/2 selected).</div>`;
      }
      renderLearnScore(sc);
    } else {
      const turnP = game.players[game.turn];
      s.innerHTML = game.finished
        ? `<div class="headline">Game over</div>` + winnerLine()
        : `<div class="headline">${game.mode === "duel" ? "Encirclement" : "Team structures"}</div>
           <span class="turn-chip" style="background:${turnP.color}22;color:${turnP.color}">
             <span class="dot" style="background:${turnP.color}"></span>${turnP.name}'s move</span>
           <div style="color:var(--muted);font-size:12px;margin-top:8px">
             <b>Claim</b> a neutral region (name a word inside it) · <b>Bridge</b> two regions you own · surround an enemy group on all sides to <b>capture</b> it.</div>`;
      renderDuelScore();
    }
  }

  function renderLearnScore(sc) {
    $("scoreboard").innerHTML = `
      <div class="score-card">
        <div class="who">Progress</div>
        <div class="score-nums">
          <span><b>${sc.regions - sc.components}</b>/${sc.regions - 1} links</span>
          <span>guesses <b>${sc.guesses}</b></span>
          <span>avg str <b>${sc.avgStrength.toFixed(2)}</b></span>
        </div>
      </div>`;
  }

  function teamName(tm) {
    if (game.mode === "duel") return game.players[tm] ? game.players[tm].name : tm;
    return tm === "team1" ? "Team 1" : "Team 2";
  }
  function teamColor(tm) {
    const anyId = Object.keys(game.players).find((id) => game._teamId(id) === tm);
    return game.players[anyId].color;
  }

  function renderDuelScore() {
    const sc = game.duelScore();
    $("scoreboard").innerHTML = Object.keys(sc).map((tm) => {
      const active = game._teamId(game.turn) === tm && !game.finished;
      return `<div class="score-card ${active ? "active" : ""}">
        <div class="who"><span class="dot" style="background:${teamColor(tm)}"></span>${teamName(tm)}</div>
        <div class="score-nums">
          <span>regions <b>${sc[tm].regions}</b></span>
          <span>structure <b>${sc[tm].largest}</b></span>
          <span>walls <b>${sc[tm].bridges}</b></span>
        </div></div>`;
    }).join("");
  }

  function winnerLine() {
    const sc = game.duelScore();
    const teams = Object.keys(sc);
    teams.sort((a, b) => sc[b].regions - sc[a].regions || sc[b].largest - sc[a].largest);
    if (sc[teams[0]].regions === sc[teams[1]].regions && sc[teams[0]].largest === sc[teams[1]].largest)
      return `<div>It's a draw.</div>`;
    return `<div><b style="color:${teamColor(teams[0])}">${teamName(teams[0])}</b> wins — ${sc[teams[0]].regions} regions, largest structure ${sc[teams[0]].largest}.</div>`;
  }

  // ---- input handling -----------------------------------------------------
  function doProbe() {
    const word = $("wordInput").value;
    if (!word.trim()) return;
    const p = game.mode === "learn" ? game.probe(word) : probeGeneric(word);
    if (!p.ok) { feedback(`"${word}" isn't in the vocabulary — try a related word.`, "bad"); return; }
    viz.flashProbe(p.word.id, "#fff");
    const nn = p.neighbors.map((n) => `${n.word.word} ${n.sim.toFixed(2)}`).join(" · ");
    let head = `Probe <b>${p.word.word}</b> → nearest ${E.CLUSTERS[p.classify.best].name} (${p.classify.bestSim.toFixed(2)})`;
    if (p.bridge) head += `<br>bridge strength to selected pair: <b>${p.bridge.strength.toFixed(2)}</b> (${p.bridge.simA.toFixed(2)} / ${p.bridge.simB.toFixed(2)})`;
    feedback(head, "", "kNN: " + nn);
  }
  function probeGeneric(word) {
    const w = E.lookup(word); if (!w) return { ok: false };
    return { ok: true, word: w, classify: E.classify(w), neighbors: E.nearest(w.vec16, 4, [w.id]) };
  }

  function doSubmit() {
    if (game.finished) return;
    const word = $("wordInput").value;
    if (!word.trim()) return;
    $("wordInput").value = "";

    if (game.mode === "learn") {
      const r = game.tryBridge(word);
      feedback(r.msg, r.ok ? "good" : "bad");
      if (r.ok) { viz.flashProbe(r.wordId, "#fff"); pushLog(game.log[game.log.length - 1]); }
      render(); renderStatus();
      return;
    }

    // duel / team
    const r = game.play(word, game.turn);
    feedback(r.msg, r.ok ? "good" : "bad");
    pushLog(r.ok ? r.msg : null);
    render(); renderStatus();
    afterTurn(r);
  }

  function afterTurn(r) {
    if (!r.then) return;
    if (r.then.finished) { renderStatus(); return; }
    // AI takes over if it's now the computer's seat
    if (game.vsAI && game.turn === "p2" && !game.finished) {
      setTimeout(aiTurn, 650);
    }
  }
  function aiTurn() {
    const r = game.aiMove("p2");
    feedback(r.msg, r.ok ? "good" : "bad");
    pushLog(r.ok ? r.msg : null);
    render(); renderStatus();
    if (r.captured && r.captured.length) render();
  }

  $("submitBtn").addEventListener("click", doSubmit);
  $("probeBtn").addEventListener("click", doProbe);
  $("wordInput").addEventListener("keydown", (e) => {
    if (e.key === "Enter") doSubmit();
    if (e.key === "Tab" && e.shiftKey) { e.preventDefault(); doProbe(); }
  });

  viz.onPointClick = (wd) => { $("wordInput").value = wd.word; };

  // ---- mode switching -----------------------------------------------------
  function startMode(mode) {
    let opts = {};
    if (mode === "duel") {
      opts.vsAI = confirm("Play against the computer?\n\nOK = vs Computer   ·   Cancel = 2-player hot-seat");
    } else if (mode === "team") {
      opts.perTeam = 2;
    }
    game.reset(mode, opts);
    $("log").innerHTML = "";
    feedback(mode === "learn"
      ? "Find a word that lives between the two highlighted regions."
      : "Name a word that lands inside a region to claim it.", "");
    setGhost(); render(); renderStatus();
  }

  document.querySelectorAll("#modeTabs button").forEach((b) => {
    b.addEventListener("click", () => {
      document.querySelectorAll("#modeTabs button").forEach((x) => x.classList.remove("active"));
      b.classList.add("active");
      startMode(b.dataset.mode);
    });
  });

  // ---- help modal ---------------------------------------------------------
  $("helpBtn").addEventListener("click", () => $("helpModal").classList.remove("hidden"));
  $("helpClose").addEventListener("click", () => $("helpModal").classList.add("hidden"));
  $("helpModal").addEventListener("click", (e) => { if (e.target.id === "helpModal") $("helpModal").classList.add("hidden"); });

  // ---- boot ---------------------------------------------------------------
  render(); renderStatus();
  feedback("Find a word that lives between the two highlighted regions. Hover any dot to see its embedding.", "");
})();
