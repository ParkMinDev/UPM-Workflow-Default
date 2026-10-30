using System;
using System.Collections.Generic;
using System.Linq;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Editor
{
    [InitializeOnLoad]
    public static class UIEditorController
    {
        // - Public Statics -
        public static void Arrange(UIEditor layout) {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || layout == null || EditorUtility.IsPersistent(layout) || !layout.gameObject.scene.IsValid())
                return;

            RectTransform[] children = GetChildren(layout);
            if (children.Length == 0) {
                _layoutHashes[layout] = GetLayoutHash(layout);
                return;
            }

            // 자식 크기 적용
            bool changed = false;
            foreach (RectTransform child in children) {
                if ((child.rect.size - layout.ChildSize).sqrMagnitude <= 0.000001f)
                    continue;
                Undo.RecordObject(child, "Resize UI Children");
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, layout.ChildSize.x);
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, layout.ChildSize.y);
                if (PrefabUtility.IsPartOfPrefabInstance(child))
                    PrefabUtility.RecordPrefabInstancePropertyModifications(child);
                EditorUtility.SetDirty(child);
                changed = true;
            }

            // 자식 영역 계산
            Rect[] bounds = new Rect[children.Length];
            Vector3[] corners = new Vector3[4];
            for (int i = 0; i < children.Length; i++) {
                RectTransform child = children[i];
                Rect rectangle = child.rect;
                corners[0] = new Vector3(rectangle.xMin, rectangle.yMin);
                corners[1] = new Vector3(rectangle.xMin, rectangle.yMax);
                corners[2] = new Vector3(rectangle.xMax, rectangle.yMax);
                corners[3] = new Vector3(rectangle.xMax, rectangle.yMin);
                Vector2 minimum = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
                Vector2 maximum = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
                foreach (Vector3 corner in corners) {
                    Vector3 offset = child.localRotation * Vector3.Scale(corner, child.localScale);
                    minimum = Vector2.Min(minimum, offset);
                    maximum = Vector2.Max(maximum, offset);
                }
                bounds[i] = Rect.MinMaxRect(minimum.x, minimum.y, maximum.x, maximum.y);
            }

            // 배치 시작점과 진행 방향
            int columns = Mathf.Min(layout.Columns, children.Length);
            Rect parentRect = ((RectTransform)layout.transform).rect;
            Vector2 origin = new Vector2(parentRect.xMin, parentRect.yMax);
            float horizontalSign = layout.HorizontalOrder == UIEditor.HorizontalDirection.LeftToRight ? 1f : -1f;
            float verticalSign = layout.VerticalOrder == UIEditor.VerticalDirection.TopToBottom ? -1f : 1f;
            float cursor = 0f;

            // 자식 위치 기록
            for (int i = 0; i < children.Length; i++) {
                RectTransform child = children[i];
                Vector2 topLeft;
                if (layout.Mode == UIEditor.LayoutMode.Horizontal) {
                    topLeft = new Vector2(origin.x + cursor + (horizontalSign < 0f ? bounds[0].width - bounds[i].width : 0f), origin.y);
                    cursor += horizontalSign * (bounds[i].width + layout.Spacing.x);
                }
                else if (layout.Mode == UIEditor.LayoutMode.Vertical) {
                    topLeft = new Vector2(origin.x, origin.y + cursor + (verticalSign > 0f ? bounds[i].height - bounds[0].height : 0f));
                    cursor += verticalSign * (bounds[i].height + layout.Spacing.y);
                }
                else {
                    int column = i % columns;
                    int row = i / columns;
                    topLeft = new Vector2(origin.x + horizontalSign * column * (layout.CellSize.x + layout.Spacing.x), origin.y + verticalSign * row * (layout.CellSize.y + layout.Spacing.y));
                }

                Vector3 position = new Vector3(topLeft.x - bounds[i].xMin, topLeft.y - bounds[i].yMax, child.localPosition.z);
                if ((child.localPosition - position).sqrMagnitude <= 0.000001f)
                    continue;

                Undo.RecordObject(child, "Arrange UI Children");
                child.localPosition = position;
                if (PrefabUtility.IsPartOfPrefabInstance(child))
                    PrefabUtility.RecordPrefabInstancePropertyModifications(child);
                EditorUtility.SetDirty(child);
                changed = true;
            }

            if (changed)
                EditorSceneManager.MarkSceneDirty(layout.gameObject.scene);
            _layoutHashes[layout] = GetLayoutHash(layout);
        }

        // - Construct -
        static UIEditorController() {
            UIEditor.ArrangeRequested += Arrange;
            EditorApplication.update += Update;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        // - Handler -
        static void Update() {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.timeSinceStartup < _nextCheckTime)
                return;
            _nextCheckTime = EditorApplication.timeSinceStartup + 0.15d;

            UIEditor[] layouts = Resources.FindObjectsOfTypeAll<UIEditor>().Where(layout => layout.isActiveAndEnabled && layout.AutoLayout && !EditorUtility.IsPersistent(layout) && layout.gameObject.scene.IsValid()).ToArray();
            foreach (UIEditor removed in _layoutHashes.Keys.Except(layouts).ToArray())
                _layoutHashes.Remove(removed);
            foreach (UIEditor layout in layouts) {
                int hash = GetLayoutHash(layout);
                if (_layoutHashes.TryGetValue(layout, out int previousHash) && previousHash == hash)
                    continue;
                Arrange(layout);
            }
        }

        static void OnUndoRedo() {
            foreach (UIEditor layout in _layoutHashes.Keys.ToArray()) {
                if (layout != null)
                    _layoutHashes[layout] = GetLayoutHash(layout);
            }
        }

        // - Private Statics -
        static readonly Dictionary<UIEditor, int> _layoutHashes = new Dictionary<UIEditor, int>();
        static double _nextCheckTime;

        static RectTransform[] GetChildren(UIEditor layout) {
            return layout.transform.Cast<Transform>().OfType<RectTransform>()
                .Where(child => (layout.IncludeInactive || child.gameObject.activeSelf) && !layout.ExcludedChildren.Contains(child))
                .ToArray();
        }

        static int GetLayoutHash(UIEditor layout) {
            HashCode hash = new HashCode();
            hash.Add(((RectTransform)layout.transform).rect);
            hash.Add(layout.CellSize);
            hash.Add(layout.Mode);
            hash.Add(layout.HorizontalOrder);
            hash.Add(layout.VerticalOrder);
            hash.Add(layout.Spacing);
            hash.Add(layout.Columns);
            hash.Add(layout.ChildSize);
            foreach (RectTransform child in GetChildren(layout)) {
                hash.Add(child.GetHashCode());
                hash.Add(child.rect);
                hash.Add(child.localPosition);
                hash.Add(child.localRotation);
                hash.Add(child.localScale);
                hash.Add(child.anchorMin);
                hash.Add(child.anchorMax);
                hash.Add(child.pivot);
            }
            return hash.ToHashCode();
        }
    }
}
