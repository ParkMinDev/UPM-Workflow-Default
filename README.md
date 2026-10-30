# ParkMinPackages Workflow Default

The default Unity project workflow, structure, and reusable setup conventions maintained by ParkMinPackages.

## UI Editor

Add `UIEditor` to a RectTransform to arrange its direct children in Edit Mode. Choose Horizontal, Vertical, or Grid and configure placement directions and Spacing. Grid supports a column count and uses the same width/height setting as child sizing. Inactive children can be included and individual children can be excluded.

New components start with Child Arrangement and Child Layout Control disabled. Settings and buttons are hidden until the corresponding feature is enabled. Automatic updates, child resizing, inactive-child inclusion and Awake restoration also default to disabled. Spacing starts at zero and the shared child/cell size is 1920 by 1080.

The first child's top-left bounds align exactly with the parent's actual top-left rectangle corner, regardless of either pivot or the child's anchors, preserving its Z position. Grid children align with each cell's top-left corner; there is no cell-alignment setting. Right extends to positive X and left to negative X; down extends to negative Y and up to positive Y. Grid applies these directions to columns and rows, returning to the starting column for each row. Sibling order is preserved. There is no start-point selector or padding. Horizontal rows align child top edges and vertical columns align child left edges. Parent resizing updates the top-left origin automatically.

The first child, or the first grid cell, stays at the same starting position for either direction. Direction changes affect progression from the second child/cell onward. Horizontal and vertical layouts account for different child sizes to preserve edge-to-edge spacing when moving left or up.

The automatic layout option (`자동 배치`) updates after changes to settings, hierarchy or child rectangles. With Child Arrangement enabled, turn automatic updates off to use the Arrange Now (`지금 배치`) inspector button. Position changes support Undo and prefab overrides. Anchors, pivots and local Z positions are preserved, and no driven properties are registered. All arrangement code lives in the Editor assembly; Play Mode and builds use the recorded child transforms without running arrangement code. Child Layout Control can optionally enable descendant layout components in Awake. Remove the standard Layout Group from the same parent before using this component.

Enable child sizing to apply the shared width and height to children in Horizontal, Vertical, or Grid mode. Disable it to retain current sizes; disabling does not restore sizes from before a previous arrangement. Grid always uses that same width/height to calculate cell spacing, including when child resizing is off. Excluded children keep their positions and sizes. Odin attributes provide the grouped Korean inspector, conditional fields, direction buttons and Arrange Now button. No custom inspector is used; the Editor-only controller performs arrangement and automatic updates.

Enable Child Layout Control to show Enable Layouts, Disable Layouts and Enable Layouts In Awake. The controls include ContentSizeFitter, LayoutGroup, AspectRatioFitter, ScrollRect and Scrollbar on the same object and inactive descendants. Enable/Disable only toggle the layout components and record Undo/prefab overrides. Enable the independent Prefab Override Control toggle (disabled by default) to reveal the Prefab Overrides section. This section is hidden when the toggle is off and does not depend on Child Layout Control. Use Revert Property Changes to restore overridden properties on the owner and every descendant GameObject and component, including inactive children, to their prefab source values. This includes differing values rather than only redundant equal-value overrides. Include Object Names is disabled by default and appears directly above the revert button. Leave it disabled to preserve the current names of the owner and descendants, or enable it to revert GameObject names as well. Added/removed objects or components and hierarchy/component-list records are preserved; missing scripts and objects without a prefab source are skipped. RectTransforms on prefab instance roots are excluded, including nested prefab roots, preserving their placement, size, anchors, pivot, rotation and scale. Non-root RectTransforms and other components remain eligible for property restoration. Reverting is Editor-only and grouped into one Undo operation. Awake restoration runs at execution order -2 when explicitly enabled.

## Samples

Install the package, select `ParkMinDev.UPM.Workflow.Default` in Package Manager, and import the samples you need.

Sample assets are stored under `Samples/Project Structure` and `Samples/UI Prefabs`; package sample entries point to these paths.

- `Project Structure` provides a starter scene, folder structure, and common prefabs.
- `UI Prefabs` provides reusable UI prefabs.

Unity imports samples to `Assets/Samples/ParkMinDev.UPM.Workflow.Default/<version>/<sample name>`.

To use the project structure at the project root, move the imported `UnnamedTeam` folder to the desired location under `Assets`.
