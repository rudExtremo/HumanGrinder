# Smoke checklist (milestones and release candidates)

The full checklist is intended for milestones/builds, not for every commit.

## Runtime

- [ ] Unity 6000.3.6f1 opens the project without package or compile errors.
- [ ] Create the prototype scene through the Tools menu, save, and enter Play mode.
- [ ] Menu offers exactly 3 characters and 3 tower-length options.
- [ ] P1 can move, jump, and slash; only enemies in range and in the facing direction take damage.
- [ ] P2 instantly joins when P2's movement/attack keys are pressed, including during a running wave.
- [ ] P1 and P2 can move/attack simultaneously, with one shared camera and separate HP.
- [ ] Monsters spawn from both sides, approach living players, and attack within range.
- [ ] Defeating all enemies triggers the next floor; final floor completion shows victory.
- [ ] All players dead shows defeat; Enter returns to menu, no surviving enemies persist.
- [ ] Input remains usable after restarting a run.

## Platform

- [ ] Windows: clean build runs without Editor; verify window size and frame pacing.
- [ ] Web: loads in target browser, canvas focus captures keyboard, P2 keys do not scroll page unexpectedly.
- [ ] Web: acceptable memory use, reasonable initial download, no browser console errors.
- [ ] Android (stretch): controls are designed and tested for touch before calling it supported.
