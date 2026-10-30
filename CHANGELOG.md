# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [15.0.1] - 2026-10-30

### Fixed
- Block automatic and manual UIEditor arrangement while the Editor is playing, including play-mode exit transitions, before modifying objects or marking scenes dirty.

## [15.0.0] - 2026-10-30

### Changed
- Remove arrangement and child-sizing feature toggles. Always display their settings and apply both operations through the shared Targets section, with separate grid cell dimensions.

### Removed
- Remove UIEditor child layout enable/disable controls and optional Awake restoration. Manage standard layout components directly.
- Remove prefab property reversion, its independent feature toggle and object-name option. Use Unity prefab override controls instead. Child arrangement is unchanged.

## [14.0.0] - 2026-10-30

### Changed
- Rename UILayoutEditor and its editor controller to UIEditor and UIEditorController, preserving both script GUIDs and recording the previous component name with MovedFrom.
- Move object-name selection and property reversion into a dedicated Prefab Overrides section controlled by an independent, default-off Prefab Override Control toggle.
- Exclude RectTransforms on all prefab instance roots, including nested prefab roots, from property reversion. Other components and non-root RectTransforms retain their existing behavior.

## [13.1.1] - 2026-10-30

### Fixed
- Preserve GameObject names by default when reverting property changes. Add an Include Object Names option immediately above the revert button to opt into restoring names.

## [13.1.0] - 2026-10-30

### Changed
- Replace automatic redundant-override cleanup during layout toggles with a separate Revert Property Changes button below Disable Layouts.
- Restore differing property overrides on the owner and every descendant GameObject and component, including inactive children, to their prefab source values. Preserve added/removed objects and components and structural hierarchy/component-list records.
- Group the Editor-only property restoration into one Undo operation and update the usage documentation.

## [13.0.1] - 2026-10-30

### Fixed
- Remove redundant RectTransform geometry overrides after toggling descendant layouts when their serialized values match the prefab source. Preserve actual differences and Unity default overrides, deduplicate targets and include RectTransforms in Undo recording.

## [13.0.0] - 2026-10-30

### Changed
- Rename EditorUILayout to UILayoutEditor and its editor controller while preserving script GUIDs and the previous component identity through MovedFrom.
- Default all feature switches to disabled and hide inactive feature settings and buttons. Existing sample placement settings remain enabled explicitly.

### Added
- Integrate descendant layout enable/disable buttons, Undo and prefab override handling, and optional Awake restoration with execution order -2.
- Separate child arrangement from automatic updates so manual arrangement remains available when automatic updates are disabled.

## [12.1.0] - 2026-10-27

### Added
- Add EditorUILayout with edit-only horizontal, vertical and configurable-column grid arrangement, independent fill directions, Spacing, inactive/excluded children, optional child resizing, automatic updates, manual arrangement and Undo/prefab override support. Child anchors, pivots and Z positions are preserved; player builds contain settings only.
- Share one width/height setting between grid cells and child sizing, remove cell-internal alignment, and present grouped Korean settings, direction controls and the manual button using Odin attributes without a custom inspector.
- Default new components to zero Spacing and a shared child/cell size of 1920 by 1080.
- Align the first child's bounds with the parent's actual top-left corner without changing anchors or pivots. Extend placement right/left for horizontal layouts, down/up for vertical layouts, and in signed column/row directions for grids while preserving sibling order. Update placement when the parent resizes.
- Keep the first child/grid cell at the same start position in either direction; correct the initial left/up offset while retaining spacing between differently sized children.

### Changed
- Move sample assets from Samples~ to Samples while preserving asset GUIDs, update package sample paths, and use EditorUILayout in the standard and dual-monitor UI prefabs.

## [12.0.2] - 2026-10-27

### Changed
- Align exact dependencies to foundation 12.0.1, ugui 16.1.2. No runtime code changes.

## [12.0.1] - 2026-10-26

### Fixed
- Limit UIRoot.SetChildsSize to immediate RectTransform children; preserve sizes of grandchildren and deeper descendants.

## [12.0.0] - 2026-10-26

### Added
- Add UIRoot child-size authoring action and canvas-camera, single-display UI and dual-display UI sample prefabs.

### Changed
- Breaking: replace the Main Canvas sample path with Canvas while preserving its asset GUID.
- Align exact dependencies to Foundation 12.0.0 and UGUI 16.1.1; UI sample cleanup uses Foundation DestroyOnAwake.

## [11.0.2] - 2026-10-19

### Changed
- Aligned the exact UGUI dependency to 16.1.0 for the angle-graphics release. No runtime code changes.

## [11.0.1] - 2026-10-04

### Changed
- Aligned the UGUI dependency to the current menu release.

## [11.0.0] - 2026-10-04

### Changed
- Breaking: moved namespaces and assembly names from `ParkMinPackages.Workflow.Default` to `ParkMinDev.UPM.Workflow.Default` while preserving namespace suffixes and asset GUIDs.
- Updated package references, assembly friend declarations, serialized type identifiers, and final dependency versions.
- Added previous type-location metadata where applicable and preserved existing MovedFrom history.

## [10.0.7] - 2026-10-04

### Changed
- Standardized package identity and display name as `com.parkmindev.upm.workflow.default` / `ParkMinDev.UPM.Workflow.Default`.
- Synchronized own-package dependency versions for this release; C# namespaces and assembly names remain unchanged.

## [10.0.6] - 2026-10-04

### Changed
- Renamed the repository to UPM-Workflow-Default and updated repository links without changing the Unity package identity, namespaces, assemblies, or asset GUIDs.
- Updated ParkMin dependency repository URLs and synchronized their final release versions in parkmin-upm.json.

## [10.0.5] - 2026-10-04

### Changed
- Moved repository links and dependency URLs to ParkMinDev while preserving the package identity.
- Replaced repository dependency metadata with parkmin-upm.json and aligned ParkMin dependency release versions.

## [10.0.4] - 2026-10-01

### Changed
- Standardized the Project Structure sample to the requested 15 top-level folders, replacing the empty Audio and Models folders with the new content categories.
- Added folder metadata and .gitkeep files to preserve the new empty sample directories in Git.

## [10.0.3] - 2026-10-01

### Changed
- Reorganized the Project Structure sample: renamed 01_Scenes to Scenes, moved shared asset and script folders out of General, and grouped reusable prefabs under Prefabs/General while preserving asset contents and GUIDs.
- Added folder metadata for the Project Structure and UI Prefabs samples.

## [10.0.2] - 2026-10-01

### Fixed
- Aligned ParkMin dependency declarations with the release versions: com.parkminpackages.foundation 10.1.2, com.parkminpackages.ugui 15.0.2.

## [10.0.1] - 2026-09-15

### Fixed
- Immediately deactivated timed UI when its display duration is cancelled.

## [10.0.0] - 2026-08-30

### Breaking Changes
- Replaced the reusable UI flow interfaces, `UIFlowExtensions`, and `ShowHideUI` with owner-based feature components.
- Moved feature attachment helpers onto `Actor` and changed actor feature initialization to the shared Foundation feature lifecycle.

### Added
- Added features for show and hide transitions, timed display, confirmation, yes-or-no input, text messages, and bottom-up RectTransform layout rebuilding.
- Added `Project Structure` and `UI Prefabs` as importable Package Manager samples.

### Changed
- Updated Foundation to `10.0.0` and UGUI to `13.0.0`.

### Removed
- Removed the legacy `.unitypackage` presets and their export profile utility.

## [9.1.4] - 2026-08-20

### Changed
- Assigned the shared editor-only icon to `CanvasResolutionHelper`.

## [9.1.3] - 2026-08-19

### Added
- Added `IShowHideUI` for synchronous show and hide state commands backed by `UIActivator` transitions.
- Added `ShowHideUI` as a reusable `BasicUI` implementation that cancels the previous transition before starting the next one.

## [9.1.2] - 2026-08-16

### Changed
- Added explicit versions to Git dependency metadata for PackageManager display and validation.

## [9.1.1] - 2026-08-16

### Changed
- Moved Git package dependency metadata from `package.json` to `parkmin-dependencies.json` while retaining Unity Registry dependencies.

## [9.1.0] - 2026-08-12

### Added
- Added the `IInputView` contract for cancellable input flows that return a yes-or-no answer with entered text.
- Added the `IInputFieldButtonYesOrNoView` implementation-template interface for combining an `InputField` with button-based yes-or-no selection.

## [9.0.0] - 2026-08-12

### Breaking Changes
- Moved button-based confirmation and yes-or-no implementations from the base contracts to specialized implementation-template interfaces.
- Moved RectTransform layout data and bottom-up rebuilding from ILayoutRebuildable to IRectTransformLayoutRebuildable.

### Added
- Added IButtonConfirmView, IButtonYesOrNoView, and IRectTransformLayoutRebuildable implementation interfaces.

## [8.0.0] - 2026-08-12

### Breaking Changes
- Reworked message, icon, confirmation, yes-or-no, and layout contracts around interface implementation templates with explicit forwarding methods.
- Generalized icon views by icon asset type so Image views use Sprite and RawImage views use Texture without runtime Sprite creation.
- Renamed confirmation and yes-or-no operations and consolidated their presentation flows into UIFlowExtensions.

### Changed
- Moved the duration-based display flow from BasicUI to UIFlowExtensions.
- Added message and icon value accessors and fluent configuration extensions.
- Updated the UGUI dependency to 6.0.0.

## [7.0.4] - 2026-08-10

### Changed
- Renamed `RebuildLayoutByRectTransformTwice` to `RebuildLayoutBottomUp` and rebuild `ContentSizeFitter` layouts from the deepest hierarchy level upward.

## [7.0.3] - 2026-08-10

### Changed
- Rebuild child `ContentSizeFitter` layouts before rebuilding the root layout target.

## [7.0.2] - 2026-08-10

### Added
- Added metadata for the required empty `Actors` folder.

### Changed
- Updated the reusable UIs preset package after removing the default message UI implementations.

## [7.0.1] - 2026-08-10

### Changed
- Renamed the button-based confirmation implementation method from `ConfirmByButtonAsync` to `ConfirmAsyncByButton` for implementation-name consistency.

## [7.0.0] - 2026-08-10

### Breaking Changes
- Removed the default `MessageUI` and `YesOrNoUI` components so applications can provide UI implementations suited to their own presentation requirements.

### Added
- Added composable interfaces and shared implementations for messages, icons, confirmation, yes-or-no answers, and layout rebuilding.
- Added `WithMessage` and `WithIcon` fluent extensions that preserve the concrete receiver type regardless of chaining order.
- Added reusable message and yes-or-no UI flow extensions.
- Added the standalone `YesOrNo` result enum.

### Changed
- Added the direct TextMesh Pro assembly reference required by TMP-based message views.

## [6.1.0] - 2026-07-29

### Added
- Added the reusable `UIRoot` actor component.
- Added `CanvasResolutionHelper` for configuring CanvasScaler and UI root dimensions.

### Changed
- Updated the UIs and WorkSpace preset packages with current namespaces, Foundation components, and Main Canvas structure.
- Updated the Foundation dependency to `5.2.0`.
## [6.0.0] - 2026-07-28

### Breaking Changes
- Moved `EditorPlayBehaviour`, `InstantiateOnceOnRuntime`, and `TargetFrameSetting` to Foundation and changed their namespaces to `ParkMinPackages.Foundation.Components`.

### Changed
- Updated the Foundation dependency to `5.1.0`.

## [5.1.0] - 2026-07-25

### Changed
- Repackaged the reusable UI and workspace presets as importable `UIs.unitypackage` and `WorkSpace.unitypackage` assets.
- Removed the unpacked preset source assets now distributed through the Unity packages.

## [5.0.0] - 2026-07-25

### Breaking Changes
- Changed runtime and editor namespaces to the `ParkMinPackages.Workflow.Default` convention.

### Added
- Added the Actor, binding, UI, editor-play, bootstrap, and target-frame workflow components.
- Added reusable UI and general project presets.
- Added project context menus for Scripts and Domain folder structures.
- Added Build Settings Profile creation, capture, and safe application workflows.

### Changed
- Added explicit dependencies on Foundation, UGUI, UniTask, R3, Input System, and Unity UGUI.
## [1.0.0] - 2026-07-25

### Added
- Added the initial Unity package and assembly structure.
