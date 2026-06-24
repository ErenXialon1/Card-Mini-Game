# Revolver Mini Game - Unity Case Study

This project is a Unity case study developed for the Vertigo Games Game Developer Demo.

The assignment was to create a Wheel of Fortune-style gambling mini-game inspired by the card game structure in Critical Strike.

The project focuses on:

- Gameplay flow implementation
- Configurable wheel content
- Responsive Unity UI
- Maintainable C# architecture

## Gameplay Summary

The player progresses through zones by spinning a wheel.

Each wheel contains multiple reward slices and, depending on the zone type, may also contain a bomb slice. Rewards become more valuable as the player advances through zones.

If the player hits a bomb, all collected rewards are lost and the game can be restarted.  
On safe and super zones, the player can leave with their collected rewards without risk.

## Zone Rules

- Standard zones contain rewards and a bomb.
- Every 5th zone is a safe zone.
- Every 30th zone is a super zone.
- Safe zones and super zones do not contain a bomb.
- The player can leave only when the wheel is not spinning and the current zone allows leaving.

## Technical Highlights

- Modular gameplay logic
- Configurable wheel slice data
- Player feedback with UI and Sound Effects
- TextMeshPro
- Responsive UI layout for different aspect ratios

## Architecture Focus

The project was structured with maintainability and scalability in mind.

The implementation prioritizes:

- Separation of responsibilities
- Reusable gameplay components
- Clear data flow
- Editor-configurable content
- Testable core logic where possible


## Project Status

Completed as a case study.
