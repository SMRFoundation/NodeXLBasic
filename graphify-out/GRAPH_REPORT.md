# Graph Report - NodeXLBasic  (2026-06-30)

## Corpus Check
- 16 files · ~219,768 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 236 nodes · 389 edges · 21 communities (13 shown, 8 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 1 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `0df3d9a1`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]
- [[_COMMUNITY_Community 6|Community 6]]
- [[_COMMUNITY_Community 7|Community 7]]
- [[_COMMUNITY_Community 8|Community 8]]
- [[_COMMUNITY_Community 9|Community 9]]
- [[_COMMUNITY_Community 10|Community 10]]
- [[_COMMUNITY_Community 11|Community 11]]
- [[_COMMUNITY_Community 12|Community 12]]
- [[_COMMUNITY_Community 13|Community 13]]
- [[_COMMUNITY_Community 14|Community 14]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]
- [[_COMMUNITY_Community 20|Community 20]]

## God Nodes (most connected - your core abstractions)
1. `Activity` - 62 edges
2. `Activity` - 54 edges
3. `Variable` - 31 edges
4. `Variable` - 25 edges
5. `Activity` - 25 edges
6. `Property` - 22 edges
7. `Property` - 20 edges
8. `Activity` - 17 edges
9. `Variable` - 15 edges
10. `Property` - 13 edges

## Surprising Connections (you probably didn't know these)
- `oAuthTwitter` --inherits--> `OAuthBase`  [EXTRACTED]
  Common/SocialNetwork/Twitter/Authorization/oAuthTwitter.cs → Common/SocialNetwork/Twitter/Authorization/oAuth.cs

## Import Cycles
- None detected.

## Communities (21 total, 8 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (29): agileTestPlatformAssemblies, agileTestPlatformAssembly, assemblies, associatedChangesets, BinariesDirectory, BuildAgent, BuildDetail, BuildDirectory (+21 more)

### Community 1 - "Community 1"
Cohesion: 0.12
Nodes (28): Activity, BuildDetail, BuildLocation, BuildNumber, BuildNumberFormat, BuildProcessVersion, BuildStatus, ChildBuildDetail (+20 more)

### Community 2 - "Community 2"
Cohesion: 0.14
Nodes (25): Activity, AgentSettings, AssociateChangesetsAndWorkItems, BuildNumberFormat, BuildProcessVersion, BuildSettings, CleanWorkspace, CreateLabel (+17 more)

### Community 3 - "Community 3"
Cohesion: 0.15
Nodes (23): Activity, AgentSettings, AssociateChangesetsAndWorkItems, BuildNumberFormat, BuildSettings, CleanWorkspace, CreateLabel, CreateWorkItem (+15 more)

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (24): assemblies, associatedChangesets, BinariesDirectory, BuildAgent, BuildDetail, BuildDirectory, compilationException, localBuildProjectItem (+16 more)

### Community 5 - "Community 5"
Cohesion: 0.15
Nodes (12): string, OAuthBase, QueryParameter, QueryParameterComparer, SignatureTypes, Smrf.SocialNetworkLib.Twitter, HashAlgorithm, IComparer (+4 more)

### Community 6 - "Community 6"
Cohesion: 0.17
Nodes (19): Activity, AgentSettings, BinariesSubdirectory, BuildDetail, buildDirectory, ConfigurationFolderPath, DoNotDownloadBuildType, LogFilePerProject (+11 more)

### Community 7 - "Community 7"
Cohesion: 0.27
Nodes (6): string, Method, oAuthTwitter, Smrf.SocialNetworkLib.Twitter, HttpWebRequest, Int32

### Community 8 - "Community 8"
Cohesion: 0.22
Nodes (9): compilationExceptionArgument, ex, exception, failedRequests, platformConfiguration, serverBuildProjectItem, spec, testException (+1 more)

### Community 9 - "Community 9"
Cohesion: 0.25
Nodes (8): compilationExceptionArgument, ex, exception, platformConfiguration, serverBuildProjectItem, spec, testException, DelegateInArgument

### Community 10 - "Community 10"
Cohesion: 0.33
Nodes (3): Complete NodeXL Release History, Complete NodeXL Release History, Page 2, NodeXL Redesign

### Community 11 - "Community 11"
Cohesion: 0.67
Nodes (3): WorkspaceName, CreateWorkspace, DeleteWorkspace

### Community 12 - "Community 12"
Cohesion: 0.67
Nodes (3): WorkspaceName, CreateWorkspace, DeleteWorkspace

## Knowledge Gaps
- **22 isolated node(s):** `codegraph`, `Process`, `DeleteWorkspace`, `CreateWorkspace`, `LabelWorkspace` (+17 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **8 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Activity` connect `Community 2` to `Community 0`, `Community 8`, `Community 11`, `Community 16`?**
  _High betweenness centrality (0.065) - this node is a cross-community bridge._
- **Why does `Activity` connect `Community 3` to `Community 9`, `Community 4`, `Community 12`, `Community 17`?**
  _High betweenness centrality (0.052) - this node is a cross-community bridge._
- **Why does `OAuthBase` connect `Community 5` to `Community 7`?**
  _High betweenness centrality (0.014) - this node is a cross-community bridge._
- **What connects `codegraph`, `Process`, `DeleteWorkspace` to the rest of the system?**
  _22 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.06896551724137931 - nodes in this community are weakly interconnected._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.11822660098522167 - nodes in this community are weakly interconnected._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.14153846153846153 - nodes in this community are weakly interconnected._