# Branch strategy

- `main`: stable milestone snapshots and game jam submissions.
- `dev`: integration branch; review and merge feature work here.
- `feat/*` and `fix/*`: short-lived branches per task, branched from `dev`.
- Work is committed with corresponding `.meta` files. Keep local Unity cache/build outputs out of Git.
- Do not enable automatic full test runs for every commit. Run targeted checks when useful and full platform smoke checks at milestones.

Suggested workflow:

```bash
git switch dev
git pull --ff-only origin dev
git switch -c feat/example
# Make changes and commit
git push -u origin feat/example
```

Open a pull request into `dev`. When the prototype is ready for a milestone or release, open a pull request from `dev` to `main`.
