# ParkMinPackages Workflow Default

The default Unity project workflow, structure, and reusable setup conventions maintained by ParkMinPackages.

## UI Editor

Add `UIEditor` to a RectTransform to size and arrange its direct children in Edit Mode. Arrangement, child sizing and target settings are displayed without feature-enable toggles. Choose Horizontal, Vertical or Grid; only mode-specific fields are conditionally hidden. Grid uses separate cell dimensions for placement.

The Targets section contains Auto Apply, Include Inactive Children, Excluded Children and Apply Now (`지금 적용`). Auto Apply is disabled by default. Apply Now or automatic updates apply both child sizing and arrangement, preserving excluded children. Disabling automatic updates leaves the recorded transforms unchanged. Spacing defaults to zero; child size and grid cell size default to 1920 by 1080.

Children align from the parent's top-left rectangle corner in sibling order. Directions control horizontal and vertical progression. Rotation and scale are accounted for in bounds, and anchors, pivots and local Z positions are preserved. Changes support Undo and prefab overrides. Arrangement runs only in the Editor; Play Mode and builds use the recorded transforms. Manage standard layout components and prefab overrides through Unity's own controls.

## Samples

Install the package, select `ParkMinDev.UPM.Workflow.Default` in Package Manager, and import the samples you need.

Sample assets are stored under `Samples/Project Structure` and `Samples/UI Prefabs`; package sample entries point to these paths.

- `Project Structure` provides a starter scene, folder structure, and common prefabs.
- `UI Prefabs` provides reusable UI prefabs.

Unity imports samples to `Assets/Samples/ParkMinDev.UPM.Workflow.Default/<version>/<sample name>`.

To use the project structure at the project root, move the imported `UnnamedTeam` folder to the desired location under `Assets`.
