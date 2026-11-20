# Agent guidance

Project: Human Grinder, Unity 6000.3.6f1, 2D side-view local co-op roguelite slasher.

- Maintain Windows and Web as primary targets; Android is stretch-only.
- Preserve second-player instant join on P2 input and shared-screen co-op.
- Prefer minimal package dependencies; do not copy BannerMatch monetization/telemetry or assets.
- Code, comments and in-game UI must be English.
- Avoid regenerating scene assets unless the changes require it.
- Do not run all tests on every commit. Prefer static checks and targeted smoke tests; perform full platform smoke/release checks at milestones.
- Do not claim Unity compilation or builds passed unless actually run in a Unity editor.
