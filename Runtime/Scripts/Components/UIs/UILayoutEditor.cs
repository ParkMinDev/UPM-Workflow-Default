using System;
using System.Collections.Generic;
using System.Linq;
using ParkMinDev.UPM.Foundation.Constants;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.UIs
{
	[DefaultExecutionOrder(-2), DisallowMultipleComponent, RequireComponent(typeof(RectTransform)), AddComponentMenu("ParkMinDev/Workflow/UI Layout Editor")]
	[MovedFrom(true, "ParkMinDev.UPM.Workflow.Default.Components.UIs", "ParkMinDev.UPM.Workflow.Default", "EditorUILayout")]
	public sealed class UILayoutEditor : MonoBehaviour
	{
#if UNITY_EDITOR
		// - Public Statics -
		public static event Action<UILayoutEditor> ArrangeRequested;
#endif

		// - Public Methods -
		[Button("레이아웃 켜기"), BoxGroup("하위 레이아웃"), ShowIf(nameof(_controlLayouts)), PropertyOrder(1)]
		public void EnableLayouts() {
			SetLayoutsEnabled(true);
		}

		[Button("레이아웃 끄기"), BoxGroup("하위 레이아웃"), ShowIf(nameof(_controlLayouts)), PropertyOrder(2)]
		public void DisableLayouts() {
			SetLayoutsEnabled(false);
		}

#if UNITY_EDITOR
		[Button("속성 변경 되돌리기"), BoxGroup("하위 레이아웃"), ShowIf(nameof(_controlLayouts)), PropertyOrder(4), DisableInPlayMode]
		public void RevertPropertyChanges() {
			if (!_controlLayouts || UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;
			UnityEngine.Object[] targets = GetComponentsInChildren<Transform>(true).SelectMany(child => new UnityEngine.Object[] { child.gameObject }.Concat(child.GetComponents<Component>().Where(component => component != null))).ToArray();
			string[] structuralProperties = { "m_Component", "m_Father", "m_Children", "m_RootOrder", "m_GameObject", "m_CorrespondingSourceObject", "m_PrefabInstance", "m_PrefabAsset", "m_Script" };

			// 속성 복원을 하나의 실행 취소 작업으로 묶기
			UnityEditor.Undo.IncrementCurrentGroup();
			int undoGroup = UnityEditor.Undo.GetCurrentGroup();
			UnityEditor.Undo.SetCurrentGroupName("Revert child object property overrides");
			try {
				foreach (UnityEngine.Object target in targets) {
					if (target == null || !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(target))
						continue;
					UnityEngine.Object source = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(target);
					if (source == null)
						continue;

					// 추가·삭제 및 구조 기록을 제외한 속성 변경 수집
					UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(target);
					using UnityEditor.SerializedObject serialized = new UnityEditor.SerializedObject(target);
					List<string> propertyPaths = new List<string>();
					UnityEditor.SerializedProperty property = serialized.GetIterator();
					bool enterChildren = true;
					while (property.Next(enterChildren)) {
						enterChildren = !structuralProperties.Contains(property.propertyPath.Split('.')[0]);
						if (!enterChildren || !property.prefabOverride)
							continue;
						if (!_includeObjectNames && target is GameObject && property.propertyPath == "m_Name")
							continue;
						propertyPaths.Add(property.propertyPath);
						enterChildren = false;
					}

					// 속성을 프리팹 원본으로 복원
					foreach (string propertyPath in propertyPaths) {
						serialized.Update();
						UnityEditor.SerializedProperty overriddenProperty = serialized.FindProperty(propertyPath);
						if (overriddenProperty != null && overriddenProperty.prefabOverride)
							UnityEditor.PrefabUtility.RevertPropertyOverride(overriddenProperty, UnityEditor.InteractionMode.UserAction);
					}
				}
			}
			finally {
				UnityEditor.Undo.CollapseUndoOperations(undoGroup);
			}
		}
#endif

		// - Public Properties -
		public bool ArrangeChildren => _arrangeChildren;
		public bool AutoLayout => _autoLayout;
		public LayoutMode Mode => _mode;
		public HorizontalDirection HorizontalOrder => _horizontalDirection;
		public VerticalDirection VerticalOrder => _verticalDirection;
		public Vector2 Spacing => _spacing;
		public bool IncludeInactive => _includeInactive;
		public int Columns => Mathf.Max(1, _columns);
		public Vector2 CellSize => ChildSize;
		public bool ResizeChildren => _resizeChildren;
		public Vector2 ChildSize => Vector2.Max(Vector2.zero, _childSize);
		public IReadOnlyList<RectTransform> ExcludedChildren => _excludedChildren;

		// - Handler -
		void Awake() {
			if (_controlLayouts && _enableLayoutsOnAwake)
				EnableLayouts();
		}

		// - Private & Protected -
		[Title(Headers.Settings)]
		[SerializeField, LabelText("자식 배치 사용"), DisableInPlayMode] bool _arrangeChildren;
		[SerializeField, LabelText("하위 레이아웃 제어 사용"), DisableInPlayMode] bool _controlLayouts;

		[SerializeField, BoxGroup("배치"), LabelText("자동 갱신"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] bool _autoLayout;
		[SerializeField, BoxGroup("배치"), LabelText("방식"), EnumToggleButtons, ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] LayoutMode _mode;
		[SerializeField, BoxGroup("배치"), LabelText("가로 방향"), EnumToggleButtons, ShowIf("@_arrangeChildren && _mode != LayoutMode.Vertical"), DisableInPlayMode] HorizontalDirection _horizontalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("세로 방향"), EnumToggleButtons, ShowIf("@_arrangeChildren && _mode != LayoutMode.Horizontal"), DisableInPlayMode] VerticalDirection _verticalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("열 수"), MinValue(1), ShowIf("@_arrangeChildren && _mode == LayoutMode.Grid"), DisableInPlayMode] int _columns = 3;
		[SerializeField, BoxGroup("배치"), LabelText("간격 (가로 / 세로)"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] Vector2 _spacing;

		[SerializeField, BoxGroup("자식 크기"), LabelText("크기 지정 사용"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] bool _resizeChildren;
		[SerializeField, BoxGroup("자식 크기"), LabelText("크기 (가로 / 세로)"), ShowIf("@_arrangeChildren && (_resizeChildren || _mode == LayoutMode.Grid)"), DisableInPlayMode] Vector2 _childSize = new Vector2(1920, 1080f);

		[SerializeField, BoxGroup("배치 대상"), LabelText("비활성 자식 포함"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] bool _includeInactive;
		[SerializeField, BoxGroup("배치 대상"), LabelText("제외할 자식"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode] RectTransform[] _excludedChildren = Array.Empty<RectTransform>();

		[SerializeField, BoxGroup("하위 레이아웃"), LabelText("Awake에서 레이아웃 켜기"), ShowIf(nameof(_controlLayouts)), DisableInPlayMode] bool _enableLayoutsOnAwake;
		[SerializeField, BoxGroup("하위 레이아웃"), LabelText("오브젝트 이름 포함"), ShowIf(nameof(_controlLayouts)), PropertyOrder(3), DisableInPlayMode] bool _includeObjectNames;

#if UNITY_EDITOR
		[Button("지금 배치", ButtonSizes.Medium), BoxGroup("배치"), ShowIf(nameof(_arrangeChildren)), DisableInPlayMode]
		void ArrangeNow() {
			if (_arrangeChildren && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				ArrangeRequested?.Invoke(this);
		}
#endif

		void SetLayoutsEnabled(bool enabled) {
			if (!_controlLayouts)
				return;
			Behaviour[] layouts = GetComponentsInChildren<Behaviour>(true).Where(component => component is ContentSizeFitter || component is LayoutGroup || component is AspectRatioFitter || component is ScrollRect || component is Scrollbar).ToArray();
#if UNITY_EDITOR
			if (Application.isPlaying == false) {
				RectTransform[] rectTransforms = layouts.Select(layout => layout.transform).OfType<RectTransform>().Distinct().ToArray();
				UnityEditor.Undo.RecordObjects(layouts.Cast<UnityEngine.Object>().Concat(rectTransforms).ToArray(), enabled ? "Enable layouts" : "Disable layouts");
			}
#endif
			foreach (Behaviour layout in layouts) {
				layout.enabled = enabled;
#if UNITY_EDITOR
				if (Application.isPlaying == false) {
					UnityEditor.EditorUtility.SetDirty(layout);
					if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(layout))
						UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(layout);
				}
#endif
			}
		}

		// - Class Struct Enum -
		public enum LayoutMode
		{
			[LabelText("가로")] Horizontal,
			[LabelText("세로")] Vertical,
			[LabelText("격자")] Grid
		}

		public enum HorizontalDirection
		{
			[LabelText("→ 오른쪽")] LeftToRight,
			[LabelText("← 왼쪽")] RightToLeft
		}

		public enum VerticalDirection
		{
			[LabelText("↓ 아래쪽")] TopToBottom,
			[LabelText("↑ 위쪽")] BottomToTop
		}
	}
}
