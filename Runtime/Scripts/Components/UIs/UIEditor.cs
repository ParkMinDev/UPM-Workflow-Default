using System;
using System.Collections.Generic;
using ParkMinDev.UPM.Foundation.Constants;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace ParkMinDev.UPM.Workflow.Default.Components.UIs
{
	[DisallowMultipleComponent, RequireComponent(typeof(RectTransform)), AddComponentMenu("ParkMinDev/Workflow/UI Editor")]
	[MovedFrom(true, "ParkMinDev.UPM.Workflow.Default.Components.UIs", "ParkMinDev.UPM.Workflow.Default", "UILayoutEditor")]
	public sealed class UIEditor : MonoBehaviour
	{
#if UNITY_EDITOR
		// - Public Statics -
		public static event Action<UIEditor> ArrangeRequested;
#endif

		// - Public Properties -
		public bool AutoLayout => _autoLayout;
		public LayoutMode Mode => _mode;
		public HorizontalDirection HorizontalOrder => _horizontalDirection;
		public VerticalDirection VerticalOrder => _verticalDirection;
		public Vector2 Spacing => _spacing;
		public bool IncludeInactive => _includeInactive;
		public int Columns => Mathf.Max(1, _columns);
		public Vector2 CellSize => Vector2.Max(Vector2.zero, _cellSize);
		public Vector2 ChildSize => Vector2.Max(Vector2.zero, _childSize);
		public IReadOnlyList<RectTransform> ExcludedChildren => _excludedChildren;

		// - Private & Protected -
		[Title(Headers.Settings)]
		[SerializeField, BoxGroup("배치"), LabelText("방식"), EnumToggleButtons, DisableInPlayMode] LayoutMode _mode;
		[SerializeField, BoxGroup("배치"), LabelText("가로 방향"), EnumToggleButtons, ShowIf("@_mode != LayoutMode.Vertical"), DisableInPlayMode] HorizontalDirection _horizontalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("세로 방향"), EnumToggleButtons, ShowIf("@_mode != LayoutMode.Horizontal"), DisableInPlayMode] VerticalDirection _verticalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("열 수"), MinValue(1), ShowIf("@_mode == LayoutMode.Grid"), DisableInPlayMode] int _columns = 3;
		[SerializeField, BoxGroup("배치"), LabelText("셀 크기 (가로 / 세로)"), ShowIf("@_mode == LayoutMode.Grid"), DisableInPlayMode] Vector2 _cellSize = new Vector2(1920, 1080f);
		[SerializeField, BoxGroup("배치"), LabelText("간격 (가로 / 세로)"), DisableInPlayMode] Vector2 _spacing;

		[SerializeField, BoxGroup("자식 크기"), LabelText("크기 (가로 / 세로)"), DisableInPlayMode] Vector2 _childSize = new Vector2(1920, 1080f);

		[SerializeField, BoxGroup("적용 대상"), LabelText("자동 적용"), DisableInPlayMode] bool _autoLayout;
		[SerializeField, BoxGroup("적용 대상"), LabelText("비활성 자식 포함"), DisableInPlayMode] bool _includeInactive;
		[SerializeField, BoxGroup("적용 대상"), LabelText("제외할 자식"), DisableInPlayMode] RectTransform[] _excludedChildren = Array.Empty<RectTransform>();

#if UNITY_EDITOR
		[Button("지금 적용", ButtonSizes.Medium), BoxGroup("적용 대상"), DisableInPlayMode]
		void ArrangeNow() {
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				ArrangeRequested?.Invoke(this);
		}
#endif

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
