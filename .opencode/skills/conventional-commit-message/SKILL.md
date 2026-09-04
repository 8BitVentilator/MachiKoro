---
name: conventional-commit-message
description: Use when asked to generate, formulate, or suggest a git commit message. Analyzes repository changes and summarizes them as a Conventional Commits message without creating the commit.
---

# Conventional Commit Message

Generate an accurate commit message from the actual repository changes. Do not
create, stage, amend, or push a commit unless the user explicitly asks for it.

## Analyze The Changes

1. Run `git status --short` to identify staged, unstaged, and untracked files.
2. If staged changes exist, treat only those changes as the commit contents.
   Mention briefly when relevant unstaged or untracked changes were excluded.
3. Inspect the selected changes with `git diff --cached --stat` and
   `git diff --cached` for staged changes, or `git diff --stat` and `git diff`
   when nothing is staged.
4. When the selected set includes untracked files, inspect their relevant
   contents because they do not appear in `git diff`.
5. Run `git log --oneline -10` and follow an established repository convention
   for language and scopes when it is compatible with Conventional Commits.
6. Base the message on behavior and intent visible in the diff. Do not infer
   ticket numbers, breaking changes, or motivations that the changes do not
   support.

If no changes exist, state that no commit message can be generated. If the
selected changes contain unrelated units of work, recommend splitting them and
provide one message per cohesive commit rather than hiding them under a vague
summary.

## Compose The Message

Follow Conventional Commits 1.0.0:

```text
<type>[optional scope][!]: <description>

[optional body]

[optional footer(s)]
```

Choose the narrowest fitting type:

- `feat`: adds or materially extends user-visible behavior
- `fix`: corrects faulty behavior
- `refactor`: restructures code without changing behavior
- `test`: changes tests only
- `docs`: changes documentation only
- `build`: changes build tooling or dependencies
- `ci`: changes continuous integration configuration
- `perf`: improves performance
- `style`: changes formatting without changing behavior
- `chore`: maintenance not covered by a more specific type
- `revert`: reverts an earlier commit

Use a short, concrete scope only when it adds useful context. Write the
description in the imperative mood, without a trailing period, and keep the
entire header at 72 characters or fewer when practical. Prefer the language
used by recent well-formed commits; if there is no reliable precedent, use
English.

Add a body only when the header cannot explain important context. Describe
what changed and why, not an inventory of files. Wrap body lines at about 72
characters.

For an actual breaking change, add `!` before the colon and include a
`BREAKING CHANGE: <description>` footer. Include issue references only when
they are present in the changes or supplied by the user.

## Output

Return the best message in a fenced `text` block so it can be used directly.
Do not add alternative phrasings unless the changes are ambiguous or need to
be split. Keep any explanation outside the block brief.
