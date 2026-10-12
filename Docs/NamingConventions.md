# Graveyard Shift — Naming Conventions

## Project Naming

Use `PascalCase` for all project folders, scenes, C# files, classes, and other in-project identifiers.

Examples:
- Folders: `Scenes`, `Scripts`, `Player`
- Scenes: `Main.tscn`, `Player.tscn`
- C# files: `PlayerController.cs`, `EnemyController.cs`
- Classes: `PlayerController`, `EnemyController`

## Branch Naming

Use lowercase `kebab-case` with conventional prefixes.

- `main` — stable version
- `chore/project-setup` — initial setup
- `feat/player-foundation` — player system
- `feat/enemy-ai` — enemy behavior
- `feat/combat-system` — combat and targeting
- `fix/player-collision` — bug fixes
- `refactor/enemy-controller` — code restructuring
- `docs/development-plan` — documentation

## Commit Naming

Follow Conventional Commits: `type: short description`

- `chore: initialize godot project structure`
- `feat: implement player movement`
- `fix: prevent player from leaving arena`
- `refactor: separate enemy movement logic`
- `docs: add development roadmap`
- `test: add player health tests`
- `assets: import tiny swords environment`

**Rules:** Keep branches and commits lowercase, concise, and descriptive. No trailing periods.
