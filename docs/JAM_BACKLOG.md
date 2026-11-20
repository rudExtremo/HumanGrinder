# Jam backlog

## P0 — playable vertical slice (Windows + Web)

- [x] Pin the Unity editor version to match BannerMatch (`6000.3.6f1`)
- [x] Document scope and controls
- [x] Add a prototype scene generator with placeholder sprites
- [x] Add a simple wave/ascend loop with 1–2 players
- [ ] **Open and compile project in Unity Editor** (not yet verified)
- [ ] Playtest two-player input on one physical keyboard
- [ ] Make melee hit feedback satisfying (hit stop, animation, particles, sounds)
- [ ] Make enemy movement, melee reach, and player damage feel fair
- [ ] Produce a Web build and verify browser focus/input
- [ ] Produce a Windows build and smoke test clean machine

## P1 — roguelite identity and polish

- [ ] 3 distinct character silhouettes / move sets
- [ ] Between-floor upgrade draft (choose 1 of 3)
- [ ] 2–3 enemy archetypes with contrasting patterns
- [ ] Elevator ascent VFX, tower backgrounds, and progression presentation
- [ ] Local co-op balance, join feedback, dead-player handling
- [ ] Main menu UX and persistent settings
- [ ] Audio implementation
- [ ] Testing matrix and bug triage

## P2 — Android (if time remains)

- [ ] Touch controls for two-player? Decide whether Android supports one player only
- [ ] Safe area, multiple aspect ratios, performance, input latency
- [ ] Build, install, and smoke test on Android device

## Suggested first collaboration split

**Developer A:** input, characters, combat, local co-op.

**Developer B:** enemies, floor director, elevator, level art/VFX.

Keep small vertical PRs; review changes before merging into `dev`. Merge tested milestones from `dev` into `main`.
