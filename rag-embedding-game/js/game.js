/* =============================================================================
 * game.js  —  rules & state for the three modes
 * -----------------------------------------------------------------------------
 *  LEARN  "Probe & Bridge"   : connect every concept region into one graph by
 *                              finding words that straddle two regions.
 *  DUEL   "Encirclement"     : Go on the concept graph. Claim regions, build
 *                              bridges (walls), surround to capture. (vs human
 *                              hot-seat or vs a heuristic AI.)
 *  TEAM   "Structures"       : same rules, N seats per side, scored by the
 *                              largest linked conceptual structure each team owns.
 *
 * Every legal move requires producing a REAL word whose embedding actually lands
 * where the rules demand — that is the pedagogical core.
 * ===========================================================================*/

(function (global) {
  "use strict";
  const E = global.RAG_ENGINE;

  const CLAIM_SIM = 0.34;   // min similarity to a centroid to "own" its region
  const BRIDGE_MIN = 0.30;  // min similarity to EACH region for a valid bridge

  const PALETTE = {
    p1: { color: "#2f9bd6" },
    p2: { color: "#e0518e" },
    p3: { color: "#54b04a" },
    p4: { color: "#f0932b" },
  };

  function Game() {
    this.reset("learn", {});
  }

  Game.prototype.reset = function (mode, opts) {
    opts = opts || {};
    this.mode = mode;
    this.roadmap = E.clusterGraph();
    this.adj = {};
    E.clusterKeys.forEach((k) => (this.adj[k] = []));
    this.roadmap.forEach((e) => { this.adj[e.a].push(e.b); this.adj[e.b].push(e.a); });

    this.control = {};      // clusterKey -> playerId
    this.captured = {};     // clusterKey -> true (flash flag)
    this.wordOwner = {};    // wordId -> playerId
    this.usedWords = new Set();
    this.revealed = new Set();
    this.bridges = [];      // {a,b,wordId,owner,strength}
    this.targets = [];      // learn-mode current pair
    this.log = [];
    this.turn = "p1";
    this.guesses = 0;
    this.strengthSum = 0;
    this.finished = false;

    if (mode === "learn") {
      this.players = { p1: { name: "You", color: PALETTE.p1.color, team: "p1" } };
      this.vsAI = false;
      this._pickTargets();
    } else if (mode === "duel") {
      this.players = {
        p1: { name: opts.p1 || "Player 1", color: PALETTE.p1.color, team: "p1" },
        p2: { name: opts.p2 || (opts.vsAI ? "Computer" : "Player 2"), color: PALETTE.p2.color, team: "p2" },
      };
      this.vsAI = !!opts.vsAI;
      this.seats = ["p1", "p2"];
      this.teamOf = { p1: "p1", p2: "p2" };
    } else if (mode === "team") {
      const perTeam = opts.perTeam || 2;
      this.players = {}; this.seats = []; this.teamOf = {};
      for (let t = 1; t <= 2; t++) {
        for (let s = 0; s < perTeam; s++) {
          const id = `t${t}s${s}`;
          this.players[id] = { name: `Team ${t} · P${s + 1}`, color: PALETTE[t === 1 ? "p1" : "p2"].color, team: `team${t}` };
          this.teamOf[id] = `team${t}`;
          this.seats.push(id);
        }
      }
      this.vsAI = false;
      this.turn = this.seats[0];
    }
    return this;
  };

  // ---- state snapshot for the renderer ------------------------------------
  Game.prototype.state = function () {
    // For team mode, tint regions by TEAM color via a synthetic player map.
    return {
      mode: this.mode,
      players: this.players,
      control: this.control,
      captured: this.captured,
      wordOwner: this.wordOwner,
      bridges: this.bridges,
      revealed: this.revealed,
      roadmap: this.roadmap,
      showRoadmap: true,
      targets: this.targets,
      atari: this.atariSet(),
    };
  };

  // =====================  LEARN MODE  ======================================
  Game.prototype._components = function () {
    // connected components over regions that have been bridged together
    const parent = {}; E.clusterKeys.forEach((k) => (parent[k] = k));
    const find = (x) => (parent[x] === x ? x : (parent[x] = find(parent[x])));
    const union = (a, b) => (parent[find(a)] = find(b));
    this.bridges.forEach((br) => union(br.a, br.b));
    const groups = {};
    E.clusterKeys.forEach((k) => { const r = find(k); (groups[r] = groups[r] || []).push(k); });
    return Object.values(groups);
  };

  Game.prototype._pickTargets = function () {
    // suggest two regions in different components that ARE roadmap-adjacent
    const comps = this._components();
    const compOf = {};
    comps.forEach((g, i) => g.forEach((k) => (compOf[k] = i)));
    const options = this.roadmap.filter((e) => compOf[e.a] !== compOf[e.b]);
    if (!options.length) { this.finished = true; this.targets = []; return; }
    options.sort((a, b) => b.sim - a.sim);
    this.targets = [options[0].a, options[0].b];
  };

  Game.prototype.setTargets = function (a, b) { this.targets = [a, b]; };

  // Probe: evaluate a word without committing (a RAG "query" returning scores).
  Game.prototype.probe = function (word) {
    const w = E.lookup(word);
    if (!w) return { ok: false, reason: "not-in-vocabulary" };
    const cls = E.classify(w);
    const res = { ok: true, word: w, classify: cls, neighbors: E.nearest(w.vec16, 4, [w.id]) };
    if (this.targets.length === 2) res.bridge = E.bridgeScore(w, this.targets[0], this.targets[1]);
    return res;
  };

  // Commit a bridge attempt in LEARN mode.
  Game.prototype.tryBridge = function (word) {
    const w = E.lookup(word);
    if (!w) return { ok: false, msg: `"${word}" isn't in the vocabulary. Try a related word.` };
    if (this.usedWords.has(w.id)) return { ok: false, msg: `"${w.word}" is already on the board.` };
    if (this.targets.length !== 2) return { ok: false, msg: "Pick two regions to bridge first." };
    const [a, b] = this.targets;
    const bs = E.bridgeScore(w, a, b);
    this.guesses++;
    this.revealed.add(w.id); this.usedWords.add(w.id);
    if (bs.simA < BRIDGE_MIN || bs.simB < BRIDGE_MIN) {
      const weak = bs.simA < bs.simB ? a : b;
      return {
        ok: false, placed: true,
        msg: `"${w.word}" lands mostly in ${nameOf(w)}. Too far from ${E.CLUSTERS[weak].name} (${Math.min(bs.simA, bs.simB).toFixed(2)} < ${BRIDGE_MIN}). Need a word close to BOTH.`,
        bridge: bs,
      };
    }
    this.bridges.push({ a, b, wordId: w.id, owner: "p1", strength: bs.strength });
    this.strengthSum += bs.strength;
    this.log.push(`Bridged ${E.CLUSTERS[a].name} ↔ ${E.CLUSTERS[b].name} with "${w.word}" (${bs.strength.toFixed(2)})`);
    const comps = this._components();
    const done = comps.length === 1;
    if (done) this.finished = true; else this._pickTargets();
    return {
      ok: true, done, bridge: bs, wordId: w.id,
      msg: `✓ "${w.word}" bridges ${E.CLUSTERS[a].name} ↔ ${E.CLUSTERS[b].name}  ·  strength ${bs.strength.toFixed(2)}`,
      components: comps.length,
    };
  };

  Game.prototype.learnScore = function () {
    const comps = this._components().length;
    const connected = E.clusterKeys.length - comps; // bridges effectively linking
    const efficiency = this.guesses ? this.bridges.length / this.guesses : 0;
    return {
      regions: E.clusterKeys.length, components: comps,
      bridges: this.bridges.length, guesses: this.guesses,
      avgStrength: this.bridges.length ? this.strengthSum / this.bridges.length : 0,
      efficiency,
    };
  };

  // =====================  DUEL / TEAM (Go on the graph)  ===================
  Game.prototype.groupsOf = function (teamPredicate) {
    // connected components of regions owned by a given team, via roadmap adjacency
    const owned = E.clusterKeys.filter((k) => this.control[k] && teamPredicate(this.control[k]));
    const set = new Set(owned);
    const seen = new Set(); const groups = [];
    owned.forEach((start) => {
      if (seen.has(start)) return;
      const stack = [start], g = [];
      seen.add(start);
      while (stack.length) {
        const k = stack.pop(); g.push(k);
        this.adj[k].forEach((n) => { if (set.has(n) && !seen.has(n)) { seen.add(n); stack.push(n); } });
      }
      groups.push(g);
    });
    return groups;
  };

  Game.prototype._teamId = function (playerId) { return this.teamOf ? this.teamOf[playerId] : playerId; };

  // Regions belonging to a group that is one move from capture ("atari"):
  // a same-team group whose only remaining liberty is a single neutral region.
  Game.prototype.atariSet = function () {
    const out = new Set();
    if (this.mode === "learn") return out;
    const teams = new Set((this.seats || []).map((id) => this._teamId(id)));
    teams.forEach((tm) => {
      this.groupsOf((owner) => this._teamId(owner) === tm).forEach((g) => {
        const gset = new Set(g); const nb = new Set();
        g.forEach((k) => this.adj[k].forEach((n) => { if (!gset.has(n)) nb.add(n); }));
        let liberties = 0;
        nb.forEach((n) => { if (!this.control[n]) liberties++; });
        if (liberties <= 1 && nb.size > 0) g.forEach((k) => out.add(k));
      });
    });
    return out;
  };

  // After a move by `byPlayer`, capture any enemy group with no neutral liberties
  // that is surrounded entirely by the mover's team. (Simplified Go capture.)
  Game.prototype._resolveCaptures = function (byPlayer) {
    this.captured = {};
    const myTeam = this._teamId(byPlayer);
    const enemyGroups = this.groupsOf((owner) => this._teamId(owner) !== myTeam);
    let flipped = [];
    enemyGroups.forEach((g) => {
      const gset = new Set(g);
      const neighbors = new Set();
      g.forEach((k) => this.adj[k].forEach((n) => { if (!gset.has(n)) neighbors.add(n); }));
      let surrounded = neighbors.size > 0;
      neighbors.forEach((n) => {
        const owner = this.control[n];
        if (!owner || this._teamId(owner) !== myTeam) surrounded = false; // neutral or ally-of-enemy = liberty
      });
      if (surrounded) g.forEach((k) => { this.control[k] = byPlayer; this.captured[k] = true; flipped.push(k); });
    });
    return flipped;
  };

  Game.prototype._advanceTurn = function () {
    if (this.mode === "learn") return;
    const i = this.seats.indexOf(this.turn);
    this.turn = this.seats[(i + 1) % this.seats.length];
  };

  Game.prototype._allClaimed = function () { return E.clusterKeys.every((k) => this.control[k]); };

  // A move: play `word` as a CLAIM or a BRIDGE (auto-detected).
  Game.prototype.play = function (word, playerId) {
    playerId = playerId || this.turn;
    const w = E.lookup(word);
    if (!w) return { ok: false, msg: `"${word}" isn't in the vocabulary.` };
    if (this.usedWords.has(w.id)) return { ok: false, msg: `"${w.word}" is already on the board.` };
    const myTeam = this._teamId(playerId);
    const cls = E.classify(w);

    // BRIDGE if the word straddles two regions this team already controls.
    const ownedByTeam = (k) => this.control[k] && this._teamId(this.control[k]) === myTeam;
    let bridgePair = null, bestStrength = 0;
    this.roadmap.forEach((e) => {
      if (ownedByTeam(e.a) && ownedByTeam(e.b)) {
        const bs = E.bridgeScore(w, e.a, e.b);
        if (bs.simA >= BRIDGE_MIN && bs.simB >= BRIDGE_MIN && bs.strength > bestStrength) {
          bestStrength = bs.strength; bridgePair = { e, bs };
        }
      }
    });

    // CLAIM: word lands strongly in a neutral region.
    const claimSim = cls.bestSim;
    const canClaim = !this.control[cls.best] && claimSim >= CLAIM_SIM;

    let result;
    if (bridgePair && (!canClaim || bridgePair.bs.strength >= claimSim)) {
      const { e, bs } = bridgePair;
      this.bridges.push({ a: e.a, b: e.b, wordId: w.id, owner: playerId, strength: bs.strength });
      result = { ok: true, kind: "bridge",
        msg: `${this.players[playerId].name} walled ${E.CLUSTERS[e.a].name} ↔ ${E.CLUSTERS[e.b].name} with "${w.word}" (${bs.strength.toFixed(2)})` };
    } else if (canClaim) {
      this.control[cls.best] = playerId;
      result = { ok: true, kind: "claim", region: cls.best,
        msg: `${this.players[playerId].name} claimed ${E.CLUSTERS[cls.best].name} with "${w.word}" (${claimSim.toFixed(2)})` };
    } else {
      // illegal — but still reveal so the player learns where it landed
      this.revealed.add(w.id); this.usedWords.add(w.id); this.wordOwner[w.id] = playerId; this.guesses++;
      const target = this.control[cls.best] ? "already owned" : `only ${claimSim.toFixed(2)} < ${CLAIM_SIM}`;
      return { ok: false, placed: true,
        msg: `"${w.word}" landed in ${E.CLUSTERS[cls.best].name} (${target}). No claim. Turn passes.`,
        classify: cls, endsTurn: true, then: this._afterMove(playerId) };
    }

    this.revealed.add(w.id); this.usedWords.add(w.id); this.wordOwner[w.id] = playerId; this.guesses++;
    const flipped = this._resolveCaptures(playerId);
    if (flipped.length) result.msg += `  —  captured ${flipped.map((k) => E.CLUSTERS[k].name).join(", ")}!`;
    result.captured = flipped;
    result.then = this._afterMove(playerId);
    return result;
  };

  Game.prototype._afterMove = function (playerId) {
    if (this._allClaimed()) { this.finished = true; return { finished: true, score: this.duelScore() }; }
    this._advanceTurn();
    return { finished: false, next: this.turn };
  };

  Game.prototype.duelScore = function () {
    const teams = {};
    (this.seats || ["p1", "p2"]).forEach((id) => { teams[this._teamId(id)] = teams[this._teamId(id)] || { regions: 0, largest: 0, bridges: 0 }; });
    E.clusterKeys.forEach((k) => { if (this.control[k]) teams[this._teamId(this.control[k])].regions++; });
    Object.keys(teams).forEach((tm) => {
      const groups = this.groupsOf((owner) => this._teamId(owner) === tm);
      teams[tm].largest = groups.reduce((m, g) => Math.max(m, g.length), 0);
    });
    this.bridges.forEach((br) => { teams[this._teamId(br.owner)].bridges++; });
    return teams;
  };

  // ---- simple heuristic AI (single-player duel) ---------------------------
  Game.prototype.aiMove = function (playerId) {
    const myTeam = this._teamId(playerId);
    const ownedByTeam = (k) => this.control[k] && this._teamId(this.control[k]) === myTeam;

    // 1) capture chance: neutral region whose claim would surround an enemy group
    // 2) else claim the neutral region that most threatens the opponent / is strongest
    // 3) else bridge two owned regions
    const neutral = E.clusterKeys.filter((k) => !this.control[k]);

    // try to find a capturing claim
    for (const k of neutral) {
      const backup = this.control[k]; this.control[k] = playerId;
      const enemyGroups = this.groupsOf((o) => this._teamId(o) !== myTeam);
      let captures = false;
      enemyGroups.forEach((g) => {
        const gset = new Set(g); const nb = new Set();
        g.forEach((x) => this.adj[x].forEach((n) => { if (!gset.has(n)) nb.add(n); }));
        let surrounded = nb.size > 0;
        nb.forEach((n) => { const o = this.control[n]; if (!o || this._teamId(o) !== myTeam) surrounded = false; });
        if (surrounded) captures = true;
      });
      this.control[k] = backup;
      if (captures) { const wrd = this._bestWordFor(k); if (wrd) return this.play(wrd, playerId); }
    }

    // strongest available claim (prefer regions adjacent to my territory to build walls)
    const scored = neutral.map((k) => {
      const adjMine = this.adj[k].filter(ownedByTeam).length;
      return { k, s: adjMine * 2 + this.adj[k].length };
    }).sort((a, b) => b.s - a.s);
    for (const c of scored) { const wrd = this._bestWordFor(c.k); if (wrd) return this.play(wrd, playerId); }

    // fallback: bridge
    for (const e of this.roadmap) {
      if (ownedByTeam(e.a) && ownedByTeam(e.b)) {
        const wrd = this._bestBridgeWord(e.a, e.b);
        if (wrd) return this.play(wrd, playerId);
      }
    }
    // truly stuck: pass by advancing
    this._advanceTurn();
    return { ok: false, msg: `${this.players[playerId].name} passes.`, then: { finished: this.finished, next: this.turn } };
  };

  Game.prototype._bestWordFor = function (clusterKey) {
    const cand = E.WORDS
      .filter((w) => !this.usedWords.has(w.id) && w.cluster === clusterKey)
      .map((w) => ({ w, s: E.cosine(w.vec16, E.centroids[clusterKey].vec) }))
      .filter((c) => c.s >= CLAIM_SIM)
      .sort((a, b) => b.s - a.s);
    return cand.length ? cand[0].w.word : null;
  };

  Game.prototype._bestBridgeWord = function (a, b) {
    const cand = E.WORDS
      .filter((w) => !this.usedWords.has(w.id))
      .map((w) => ({ w, bs: E.bridgeScore(w, a, b) }))
      .filter((c) => c.bs.simA >= BRIDGE_MIN && c.bs.simB >= BRIDGE_MIN)
      .sort((x, y) => y.bs.strength - x.bs.strength);
    return cand.length ? cand[0].w.word : null;
  };

  function nameOf(w) { return E.CLUSTERS[E.classify(w).best].name; }

  global.RAG_GAME = { Game, CLAIM_SIM, BRIDGE_MIN };
})(typeof window !== "undefined" ? window : globalThis);
