# Human Grinder — game design draft (v0.1)

## High concept

A fast 2D side-scrolling *arena* slasher with roguelite runs. One or two characters ride an elevator up a tower and fend off monster hordes at each landing. The screen is shared, with punchy melee combat and deliberately simple arcade UI.

## Player fantasy

Hold the elevator, cut through crowds, survive a longer tower, experiment with a different fighter, and bring a friend in without interrupting play.

## Session flow

1. Main menu: select **one of 3 playable characters** and **one of 3 tower lengths**.
2. Start on elevator floor 1; player 1 enters automatically.
3. At any time during play, pressing a mapped player 2 action **activates player 2** on the same screen. P2 starts with a separately selected/default character.
4. Fight the floor's enemy wave. If the wave is cleared, elevator ascends to the next floor.
5. In a later milestone, present one of several temporary roguelite upgrades between floors.
6. Survive the final floor to win. If all active players die, run ends; restart from floor 1.

## Combat rules (provisional)

- Horizontal movement + jump + one quick melee slash, no aiming or independent camera.
- Damage/death are deterministic at the prototype level; no production animation or audio assets yet.
- Enemy AI: approach nearest living character, attack in close range, with independent cooldown.
- HP at top of screen in the arcade tradition (separate bar per player).
- Character types **Vanguard**, **Duelist**, **Crusher** are placeholders, not final identities.

## Tower presets (provisional)

- Short: 5 floors
- Standard: 10 floors
- Endless-ish: 15 floors (not technically endless)

These numbers are implementation placeholders, **not confirmed design decisions**. The intended UX is three choices, inspired by classic fighting-game tower selection.

## Visual direction

- Pixel/hand-drawn 2D, strong silhouette readability; lateral camera, elevator visible under feet.
- No scrolling split-screen: two players must fit in the single arena.
- Top-anchored HP panels, large wave/floor indicator, limited HUD clutter.
- Public repo should contain only original assets or assets with redistribution permission. References to existing games describe mechanics, not copyrighted artwork or music.

## Platform targets

**MVP: Windows and Web.** Android is a stretch target and needs a separate touch-control pass, layout validation, performance checks, and build setup. Web requires keyboard capture/focus testing and memory profiling.

## Open design questions

- Does character selection apply individually to P1 and P2 or does P2 select on join?
- Should P2 join when P1 is dead, and does that revive the run?
- Does each player have independent upgrades, or are upgrades shared?
- Are towers 5 / 10 / 15, or different floor counts?
- Which input combinations are acceptable for the jam keyboard layout?
- What is the jam deadline, art style, and theme constraint?
