---
name: git-conventional-commits
description: "Create Git commits and draft commit messages using Conventional Commits with repository conventions for AI tooling. Use whenever creating a Git commit or writing its message."
---

# Git Conventional Commits

Apply the Conventional Commits 1.0.0 rules and repository conventions contained
in this skill. All guidance needed for this workflow is included here; use it
directly without fetching external documentation.

## Select the changes

Inspect Git status and the intended diff before choosing a message. Inspect
`git diff --cached` for staged changes and unstaged or untracked content as needed
to understand the requested commit. Describe the changes actually included.

Respect existing staged changes and the user's requested scope. Do not stage
unrelated files or silently include unrelated staged changes. If the intended
commit scope cannot be determined, ask before committing.

Prioritize separate commits by concern: the working tree may contain several
unrelated tasks. Group changes into coherent concerns before staging, and create
a separate commit for each concern covered by the user's authorization when
straightforward separation is possible. Prefer staging whole files; use simple
hunk staging when a file contains clearly separable concerns. Avoid elaborate
index manipulation, temporary rewrites of working files, or history rewriting
just to split commits.

For changes that cannot be separated without substantial Git gymnastics, keep
them together, select the primary purpose, and explain secondary concerns in the
body. Briefly report which concerns were combined and why. Respect existing staged
changes; do not silently unstage or rearrange unrelated work.

This skill does not independently authorize creating commits, pushing, or
rewriting history. When asked only for a message, return it without changing Git
state.

## Format the message

```text
<type>[optional scope][optional !]: <description>

[optional body]

[optional footer(s)]
```

Place a scope in parentheses, such as `fix(api): handle missing input`. Include
the colon and following space. Separate the optional body and footer section
from preceding content with a blank line. Use trailer-style footers, such as
`Refs: #123`, only when supported by the task or changes.

Prefer lowercase types and scopes and a concise imperative description, such as
"add" or "correct". These are local conventions, not specification requirements.
Ordinary changes may omit the scope.

Keep the entire first line (the commit header) to at most 50 characters, including
the type, scope, breaking-change marker, punctuation, spaces, and description.
Shorten the wording or omit an optional ordinary scope to fit; retain the required
AI tooling scope and any breaking-change marker. Put additional detail in the
body. Count the header characters before committing.

## Choose the type

The specification requires `feat` for new application or library functionality
and `fix` for application bug fixes. It permits other types; use these local
conventions according to the purpose of the changes:

| Type       | Use                                                       |
| ---------- | --------------------------------------------------------- |
| `feat`     | New application or library functionality                  |
| `fix`      | Bug fixes                                                 |
| `build`    | Build tooling, packaging, or dependency changes           |
| `chore`    | Maintenance that does not fit another type                |
| `ci`       | CI pipelines and automation                               |
| `docs`     | Documentation, AI instructions, and AI tooling setup      |
| `style`    | Formatting changes without behavioral changes             |
| `refactor` | Code restructuring without adding features or fixing bugs |
| `perf`     | Performance improvements                                  |
| `test`     | Adding or correcting tests                                |

Classify by intent: a bug fix that also adds regression tests is usually `fix`,
while a test-only change is `test`. `style` refers to code formatting, not new
visual behavior. Use `chore` only when a more specific type does not fit.

## Classify AI tooling work

For AI instructions and tooling setup, use `docs` with a scope identifying the
kind of work, even when adding or correcting a workflow:

| Scope      | Work                                                 | Example                                           |
| ---------- | ---------------------------------------------------- | ------------------------------------------------- |
| `skills`   | Skill instructions and supporting workflow resources | `docs(skills): add conventional commits workflow` |
| `agents`   | `AGENTS.md` instructions                             | `docs(agents): require the commit skill`          |
| `guidance` | Other AI guidance                                    | `docs(guidance): clarify review expectations`     |
| `config`   | AI tooling configuration                             | `docs(config): configure Codex defaults`          |
| `mcp`      | MCP connection setup                                 | `docs(mcp): configure the documentation server`   |

This convention applies to AI instructions and setup. Application behavior,
executable MCP implementations, infrastructure, and ordinary configuration retain
their appropriate types. For example, new MCP server functionality is `feat`,
and changing application packaging is `build`.

For inseparable work spanning multiple AI scopes, choose the primary scope and
describe the remaining work in the body.

## Identify breaking changes

Any type can introduce a breaking API change. Mark it with `!` immediately before
the colon, an uppercase `BREAKING CHANGE: <description>` footer, or both. Explain
the incompatibility in the description when using `!` alone; use a body or footer
when migration context would help.

```text
feat(api)!: require explicit authentication

BREAKING CHANGE: API clients must supply an authentication token.
```

`feat` corresponds to a SemVer minor change, `fix` to a patch, and breaking
changes to a major change. Other types do not imply a version bump unless they
introduce a breaking change. Do not change package versions merely to write a
commit message.

Before an authorized commit, check that the selected diff and message agree,
the type and scope fit the purpose, the header is at most 50 characters, and any
breaking change is marked.
