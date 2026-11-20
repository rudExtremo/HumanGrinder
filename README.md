# Human Grinder

A 2D side-view, single-screen local co-op roguelite slasher set on an elevator climbing a monster-filled tower.

**Status:** game-jam prototype scaffold. Placeholder visuals, keyboard controls, and a minimal playable combat loop. Art, sound, upgrades, balancing, and platform builds are not yet complete.

## Concept

Choose one of three fighters and a tower length. Survive increasingly hostile floors while an elevator carries you upward. A second player can join by pressing their controls, without a separate menu or network session. One shared camera; a compact, arcade-style HP HUD across the top.

## Toolchain

- **Unity Editor:** `6000.3.6f1` (same editor version as `morozhenko-apps/Bannermatch` at bootstrap time).
- **Input:** Unity Input System `1.18.0`.
- **Target platforms:** Web (Unity Web), Windows desktop; Android is stretch scope and requires touch/gamepad UI.
- **Rendering:** built-in SpriteRenderer placeholders for the jam prototype; no URP dependency yet.

## Run in Unity

1. Open this folder through Unity Hub using **6000.3.6f1**. Let Unity import packages.
2. Choose **Tools > Human Grinder > Create Prototype Scene**. This writes `Assets/Scenes/Prototype.unity` and adds it to Build Settings.
3. Open the generated scene and press Play.
4. Choose a character and one of three tower lengths. Press Start.

The repo intentionally has no Unity Library/Temp/build outputs and does not include code or art from BannerMatch.

## Controls

| | Movement | Jump | Slash | Join |
| --- | --- | --- | --- | --- |
| Player 1 | A / D | Space | F | Present at start |
| Player 2 | Left / Right arrow | Up arrow or `/` | `.` | Press any Player 2 movement/jump/slash key |

Keyboard input and shared-screen local co-op are the **jam MVP**. Android touch controls are deferred. Some keyboards do not register every key combination simultaneously (ghosting); check with two players on the target hardware.

## Planned gameplay

- 3 selectable character archetypes, each with different HP, speed, and attack power (temporary names/stats in prototype).
- 3 tower-length presets (currently provisional **5 / 10 / 15** floors; adjust during balancing).
- Side-view arena on an automatically ascending elevator, with floor-by-floor monster waves.
- One-screen drop-in local co-op and top HUD with separate HP pools.
- Roguelite run structure: escalating threat, death/retry, later add temporary upgrade choices and varied enemy patterns.

See [`docs/GAME_DESIGN.md`](docs/GAME_DESIGN.md), [`docs/JAM_BACKLOG.md`](docs/JAM_BACKLOG.md), and [`CONTRIBUTING.md`](CONTRIBUTING.md).

## License

No license has been selected yet. Public repository visibility does **not** grant redistribution or reuse rights. Before accepting outside contributions, agree on a license and art/audio ownership with the collaborator.
