# AI Coding-Assistant Extensions

This project is wired for three Claude Code extensions that give the assistant
persistent memory and a structural map of the codebase. The **project-level
configuration is committed** (`.claude/`, `.mcp.json`, `graphify-out/`), but the
CLIs themselves are per-machine — each developer installs them once locally.

| Tool | What it does | Scope of committed config |
|------|--------------|---------------------------|
| [codegraph](https://github.com/colbymchenry/codegraph) | Pre-indexed code knowledge graph, auto-syncs on change. Exposes `codegraph_explore` MCP tool. | `.mcp.json`, `.claude/settings.json`, `.claude/CLAUDE.md` |
| [graphify](https://github.com/safishamsi/graphify) | Turns the repo into a queryable knowledge graph (god nodes, communities). | `graphify-out/` (graph.json, report, html) |
| [claude-mem](https://github.com/thedotmack/claude-mem) | Persistent memory across sessions. Data lives in `~/.claude-mem` (per-machine, not committed). | — |

## One-time local install

### Prerequisites
- Node.js 18+ and npm
- [uv](https://docs.astral.sh/uv/) and Python 3.10+ (for graphify)
- Bun is auto-installed by claude-mem if missing

### codegraph
```bash
npm i -g @colbymchenry/codegraph
codegraph install --target claude --location local   # wires the MCP server (config already committed)
codegraph init                                        # builds the local .codegraph/ index
```
The `.codegraph/` index is machine-local (self-gitignored). Run `codegraph init`
once after cloning; it auto-syncs on file changes afterward.

### graphify
```bash
uv tool install graphifyy            # PyPI package is "graphifyy"; CLI is "graphify"
graphify install --platform claude   # registers the /graphify skill
graphify update .                    # rebuild graphify-out/ (no LLM needed)
```

### claude-mem
```bash
npx claude-mem install --provider claude   # interactive; sets up Bun/uv + plugin
npx claude-mem start                       # start the memory worker (http://localhost:37700)
```
Optional: `/learn-codebase` inside Claude Code to front-load the whole repo into memory.

## Using them

- **Code questions** → ask normally; `codegraph_explore` and the graphify graph are
  consulted automatically (see `.claude/CLAUDE.md` directives).
- **Architecture / "where does X live"** → `graphify explain "<node>"`,
  `graphify path "<A>" "<B>"`, or open `graphify-out/graph.html`.
- **Memory** → claude-mem injects relevant prior-session context starting from your
  second session in the project.

## Maintenance
- Rebuild the graphify graph after large refactors: `graphify update .`
- codegraph auto-syncs; force a full rebuild with `codegraph index`.
- Telemetry: both codegraph and claude-mem collect anonymous usage stats by default.
  Disable with `codegraph telemetry off` and `npx claude-mem telemetry disable`.
