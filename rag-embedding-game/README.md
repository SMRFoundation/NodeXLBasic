# Latent — an embedding & RAG concept game

**Latent** is a browser game for *feeling* what an embedding is and how
Retrieval-Augmented Generation (RAG) uses one. You play inside a vector space:
words are points, meaning is geometry, and the moves you make are exactly the
operations a RAG pipeline runs — similarity search, nearest-neighbour
retrieval, and stitching distant context together with "bridge" concepts.

It runs entirely client-side. Open `index.html` and play — no build step, no
server, no dependencies.

```
rag-embedding-game/
├── index.html         # app shell + teaching modal
├── css/styles.css
└── js/
    ├── data.js        # the toy "embedding model": words as interpretable vectors
    ├── engine.js      # cosine sim · kNN retrieval · PCA projection · cluster graph
    ├── viz.js         # SVG rendering of the projected space
    ├── game.js        # rules & state for the three modes
    └── app.js         # UI wiring
```

---

## Why this teaches RAG

A RAG system does four things, and each maps onto something you can see and do
in the game:

| RAG concept | In the game |
|---|---|
| **Embedding** — text → a vector of numbers capturing meaning | Every word is a 16-number vector over *interpretable* axes (`water`, `tech`, `affect`, `cosmos`, …). Hover any point to read its axes. |
| **Cosine similarity** — the metric that says two things "mean" the same | Distance/direction on the board. Related words point the same way. |
| **Retrieval (k-NN)** — embed the query, grab the nearest chunks | The **Probe** button: type a word, get its nearest neighbours and their scores back — a live similarity query. |
| **Dimensionality reduction** — you can't see 1,536 dims, so project | The board itself is a **PCA** projection of the 16-D space down to 2-D, the same move (t-SNE/UMAP) people use to *look at* real embeddings. |
| **Chunk / region** — a neighbourhood of related meaning | Each shaded blob is a **concept region** (a cluster). Regions overlap, because meaning does. |
| **Context stitching** — connecting distant facts through a shared pivot | A **bridge word** (e.g. `submarine` between *Ocean* and *Technology*) scores high on *both* regions. Finding bridges is how RAG links context that isn't lexically similar. |

The pedagogical trick is that our toy embedding uses **human-readable axes**.
Real embeddings hide meaning in hundreds of opaque dimensions; here the numbers
are legible, so you can see *why* `submarine` sits between water and machinery,
then trust that a real model is doing the same thing with numbers you can't read.

> Swapping the toy model for a real one is a documented one-file change — see
> **Using real embeddings** below.

---

## The three ways to play

The board is a graph of eight concept regions. The **roadmap** (faint dashed
lines) shows which regions are close enough in the embedding to be *bridgeable*.

### 1 · Solo — *Probe & Bridge*  (single user, learning mode)
The eight regions start disconnected. Your goal: connect them all into **one
graph** by finding words that straddle two regions at once.

- Pick two regions (chips in the panel, or click the blobs).
- **Probe** words to see where they land and how strongly they pull toward each
  region — this is literally a retrieval query.
- **Play** a word that scores above threshold on *both* regions to build a
  bridge. Connect all eight to win. Fewer guesses and stronger bridges score
  higher.

### 2 · 1v1 — *Encirclement*  (competitive, hot-seat or vs. AI)
**Go, played on the concept graph.** This is the "variation of Go" where a tile
is a region of vector space instead of a board intersection.

- **Claim** a neutral region by naming a word that embeds *inside* it.
- **Bridge** two regions you already own to build a wall between them.
- **Liberties** are a region-group's neutral neighbours in the roadmap graph.
  When a group's every neighbour is controlled by your opponent, it has no
  liberties and is **captured** — it flips to them. Groups one move from capture
  pulse red (*atari*), exactly like Go.
- When every region is claimed, the most territory (with largest connected
  structure as tiebreak) wins.

The strategy is semantic: to encircle, you must *know which words live where*.
The AI opponent plays a heuristic game — claiming to build walls and taking
captures when they open up.

### 3 · Teams — *Structures*  (collaborative, N-per-side)
Several players per side take turns contributing to one shared board. Same
capture rules, but scored by each team's **largest linked conceptual
structure** — the biggest connected territory you can grow to *encircle* the
other team. This is the local prototype of the real-time team mode described
below.

---

## Running it

```bash
# just open it
open rag-embedding-game/index.html          # macOS
xdg-open rag-embedding-game/index.html      # Linux

# or serve it (any static server works)
cd rag-embedding-game && python3 -m http.server 8000
# → http://localhost:8000
```

Because it's pure static files it deploys to GitHub Pages, Netlify, Cloudflare
Pages, S3, or any CDN with zero configuration.

---

## Deploying as a real web application

The prototype is deliberately client-only so it can ship as a static page. Here
is how the same game scales from single-user to competitive and team play.

### Single user (today)
Everything — the embedding model, PCA, retrieval, scoring — runs in the
browser. Nothing leaves the device. This is the ideal shape for the learning
mode and for embedding the game in a blog post or course page.

### Competitive 1v1 (real-time, two devices)
Once two people play from different machines you need an **authoritative game
server** so neither client can cheat the rules (claim validity, capture
resolution, turn order).

```
Browser A ─┐                         ┌─ Redis (session / presence / matchmaking)
           ├─ WebSocket ─ Game Server ┤
Browser B ─┘             (Node/Go)    └─ Embedding service ─ Vector DB
```

- **Transport:** WebSocket (or WebRTC data channel for lower latency). Clients
  send *intents* (`{type:"play", word:"submarine"}`); the server validates
  against the same `engine.js` rules (share the module across client/server —
  it already runs under Node) and broadcasts authoritative state diffs.
- **State model** — small and diff-friendly:
  ```jsonc
  {
    "matchId": "…", "turn": "p2", "mode": "duel",
    "control":  { "ocean": "p1", "tech": "p2" },   // region → owner
    "bridges":  [ { "a":"ocean","b":"tech","word":"submarine","owner":"p1" } ],
    "revealed": [ 4, 17, 88 ]                       // word ids on the board
  }
  ```
- **Matchmaking / lobbies:** Redis pub/sub for presence and a simple queue;
  rooms keyed by `matchId`.
- **Anti-cheat:** the server owns the embedding lookup and the similarity
  thresholds. Clients never decide whether a move is legal.
- **Reconnect:** state is a small JSON blob — persist per match so a dropped
  player rejoins mid-game.

### Teams building linked structures (many users, one board)
The team mode generalises the 1v1 server: multiple seats per side, a shared
board, and a scoreboard driven by connected-component size ("largest structure
that encircles the opponent").

- **Concurrency:** keep the authoritative server as the single writer. Use
  optimistic UI on clients (show my move immediately, reconcile on the
  server's diff). CRDTs are overkill because moves are already serialised
  through turn order; a per-team move queue is enough.
- **Presence & collaboration:** live cursors, "player X is probing…" hints, and
  a team chat channel over the same socket.
- **Scale-out:** shard by `matchId`; a stateless gateway routes sockets to the
  server instance holding that match (consistent hashing). Redis for cross-node
  presence and matchmaking.
- **Spectators & replays:** every match is a list of intents, so replays and
  live spectating are just re-applying the log.

### Using real embeddings (make it a true RAG demo)
`data.js` is the only thing that has to change. Replace the hand-authored
vectors with a real embedding model and the whole game becomes a live view of a
production RAG index:

1. **Embed a vocabulary** (or arbitrary documents) with an embedding model —
   e.g. an Anthropic-recommended provider, OpenAI `text-embedding-3`, Cohere,
   or a local `sentence-transformers` model. Store the vectors in a **vector
   database** (pgvector, Pinecone, Qdrant, Weaviate).
2. **Retrieval** (`engine.nearest`) becomes a real ANN query against that DB —
   identical semantics, production data.
3. **Projection:** precompute a 2-D UMAP/PCA layout server-side and cache it, or
   project on the client for small sets. Clusters can come from k-means / HDBSCAN
   instead of hand-labelled themes.
4. **Bridges** now surface genuine cross-domain connectors in *your* corpus —
   turning the game into an exploratory tool for auditing what a RAG system will
   and won't be able to retrieve across.

Everything above the data layer — rules, capture logic, visualisation — is
unchanged. The game is, in effect, a playable front-end for a vector index.

---

## Design notes & limitations

- **Small board, sharp game.** Eight regions on a dense graph means
  encirclement happens fast — overextending into a pocket gets punished
  immediately, which is an honest lesson about liberties. A larger corpus (see
  *real embeddings*) yields a deeper strategic board.
- **Regions overlap on purpose.** In a projected embedding, neighbourhoods
  genuinely bleed into each other; the blobs reflect that rather than hiding it.
- **Deterministic PCA.** The projection uses power iteration with a fixed seed
  so the board is identical on every load (no `Math.random`), which matters for
  fair competitive play.
- The toy vocabulary is ~125 words across 8 themes, authored so that bridge
  words (`submarine`, `bitcoin`, `melancholy`, `satellite`, `sonar`, …) sit
  measurably between two regions.

---

*Built as a standalone illustration of embeddings and RAG. Lives alongside — but
is independent of — the NodeXL codebase in this repository.*
