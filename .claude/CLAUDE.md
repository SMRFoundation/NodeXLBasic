<!-- CODEGRAPH_START -->
## CodeGraph

In repositories indexed by CodeGraph (a `.codegraph/` directory exists at the repo root), reach for it BEFORE grep/find or reading files when you need to understand or locate code:

- **MCP tool** (when available): `codegraph_explore` answers most code questions in one call — the relevant symbols' verbatim source plus the call paths between them, including dynamic-dispatch hops grep can't follow. Name a file or symbol in the query to read its current line-numbered source. If it's listed but deferred, load it by name via tool search.
- **Shell** (always works): `codegraph explore "<symbol names or question>"` prints the same output.

If there is no `.codegraph/` directory, skip CodeGraph entirely — indexing is the user's decision.
<!-- CODEGRAPH_END -->

<!-- GRAPHIFY_START -->
## Graphify

This repo ships a graphify knowledge graph in `graphify-out/`. When answering
questions about architecture, file relationships, or "where does X happen",
treat the question as a graphify query first instead of grepping blindly:

- `graphify explain "<node>"` — plain-language description of a node and its neighbors.
- `graphify path "<A>" "<B>"` — shortest path between two nodes in the graph.
- `graphify-out/GRAPH_REPORT.md` — god nodes, communities, and surprising connections.
- `graphify-out/graph.html` — interactive visualization of the graph.

Rebuild after code changes with `graphify update .` (no LLM needed). If
`graphify-out/` is absent, skip graphify — building the graph is the user's decision.
<!-- GRAPHIFY_END -->

