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
		[Button("레이아웃 켜기"), BoxGroup("하위 레이아웃"), ShowIf(nameof(_controlLayouts))]
		public void EnableLayouts() {
			SetLayoutsEnabled(true);
		}

		[Button("레이아웃 끄기"), BoxGroup("하위 레이아웃"), ShowIf(nameof(_controlLayouts))]
		public void DisableLayouts() {
			SetLayoutsEnabled(false);
		}

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
			if (Application.isPlaying == false)
				UnityEditor.Undo.RecordObjects(layouts.Cast<UnityEngine.Object>().ToArray(), enabled ? "Enable layouts" : "Disable layouts");
#endif
			foreach (Behaviour layout in layouts) {
				layout.enabled = enabled;
#if UNITY_EDITOR
				if (Application.isPlaying == false) {
					UnityEditor.EditorUtility.SetDirty(layout);
					if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(layout)) {
						UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(layout);
						Behaviour source = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(layout);
						if (source != null && layout.enabled == source.enabled) {
							UnityEditor.SerializedObject serialized = new UnityEditor.SerializedObject(layout);
							UnityEditor.SerializedProperty enabledProperty = serialized.FindProperty("m_Enabled");
							if (enabledProperty.prefabOverride)
								UnityEditor.PrefabUtility.RevertPropertyOverride(enabledProperty, UnityEditor.InteractionMode.UserAction);
						}
					}
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