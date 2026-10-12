# Development Workflow

Guidelines for collaborating on Graveyard Shift while keeping the repository clean and stable.

## 1. Main Branch

- `main` must always remain in a working state.
- Never work directly on `main`.
- Merge changes only after testing and review.

## 2. Branch Workflow

- Create a separate branch for each task or feature.
- Keep changes focused on that task.
- Push your branch to GitHub when you want to back it up or share your work.
- Merge completed work into `main`.

## 3. Daily Workflow

1. Pull the latest changes from `main`.
2. Create a branch for your task.
3. Make changes and test them in Godot.
4. Commit and push your work.
5. Review and merge the changes into `main`.
6. Update your local `main` before starting the next task.

## 4. Avoiding Conflicts

- Communicate before modifying shared systems or project settings.
- Avoid editing the same scenes or scripts simultaneously.
- Pull recent changes before starting work.
- Never force-push to `main`.
- If a conflict occurs, coordinate before resolving it.

## 5. Godot Project Rules

- Commit required scenes, scripts, assets, and resources.
- Keep generated files and temporary build outputs out of Git.
- Coordinate changes to `project.godot` and shared resources.
- Do not rename or move shared files without informing each other.

## Golden Rule

**Work separately, communicate clearly, test changes, and keep `main` stable.**
