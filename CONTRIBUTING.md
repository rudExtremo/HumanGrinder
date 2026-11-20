# Contributing to Human Grinder

- Unity: use **6000.3.6f1** on both machines to avoid scene/prefab rewrites.
- The repository is public; do not commit credentials, signing keys, store configs, unlicensed graphics, or proprietary BannerMatch assets.
- Keep Unity `.meta` files under version control. Ignore `Library`, `Temp`, `Builds`, and other generated folders.
- Work in short-lived branches (`feat/*`, `fix/*`) and merge through reviewed PRs.
- Prefer single-responsibility components and data-driven balancing. Avoid large simultaneous scene/prefab edits.
- For gameplay changes: run the relevant Editor smoke test. Do not execute the complete test suite on every commit; use full regression checks before releases and platform builds.
- Add repro steps and platform details when opening bugs.
- Agree on license/asset rights before publishing third-party contributions.
