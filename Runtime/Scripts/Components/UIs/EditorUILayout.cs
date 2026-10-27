using System;
using System.Collections.Generic;
using ParkMinDev.UPM.Foundation.Constants;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Components.UIs
{
	[DisallowMultipleComponent, RequireComponent(typeof(RectTransform)), AddComponentMenu("ParkMinDev/Workflow/Editor UI Layout")]
	public sealed class EditorUILayout : MonoBehaviour
	{
#if UNITY_EDITOR
		// - Public Statics -
		public static event Action<EditorUILayout> ArrangeRequested;
#endif

		// - Public Properties -
		public bool AutoLayout
		{
			get { return _autoLayout; }
		}
		public LayoutMode Mode
		{
			get { return _mode; }
		}
		public HorizontalDirection HorizontalOrder
		{
			get { return _horizontalDirection; }
		}
		public VerticalDirection VerticalOrder
		{
			get { return _verticalDirection; }
		}
		public Vector2 Spacing
		{
			get { return _spacing; }
		}
		public bool IncludeInactive
		{
			get { return _includeInactive; }
		}
		public int Columns
		{
			get { return Mathf.Max(1, _columns); }
		}
		public Vector2 CellSize
		{
			get { return ChildSize; }
		}
		public bool ResizeChildren
		{
			get { return _resizeChildren; }
		}
		public Vector2 ChildSize
		{
			get { return Vector2.Max(Vector2.zero, _childSize); }
		}
		public IReadOnlyList<RectTransform> ExcludedChildren
		{
			get { return _excludedChildren; }
		}

		// - Private & Protected -
		[Title(Headers.Settings)]
		[SerializeField, BoxGroup("배치"), LabelText("자동 배치"), DisableInPlayMode] bool _autoLayout = true;
		[SerializeField, BoxGroup("배치"), LabelText("방식"), EnumToggleButtons, DisableInPlayMode] LayoutMode _mode;
		[SerializeField, BoxGroup("배치"), LabelText("가로 방향"), EnumToggleButtons, ShowIf("@_mode != LayoutMode.Vertical"), DisableInPlayMode] HorizontalDirection _horizontalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("세로 방향"), EnumToggleButtons, ShowIf("@_mode != LayoutMode.Horizontal"), DisableInPlayMode] VerticalDirection _verticalDirection;
		[SerializeField, BoxGroup("배치"), LabelText("열 수"), MinValue(1), ShowIf("@_mode == LayoutMode.Grid"), DisableInPlayMode] int _columns = 3;

		[Title(Headers.Settings + " / Spacing")]
		[SerializeField, BoxGroup("Spacing"), LabelText("간격 (가로 / 세로)"), DisableInPlayMode] Vector2 _spacing = new Vector2(0, 0);

		[Title(Headers.Settings + " / Child Size")]
		[SerializeField, BoxGroup("자식 크기"), LabelText("크기 지정 사용"), DisableInPlayMode] bool _resizeChildren;
		[SerializeField, BoxGroup("자식 크기"), LabelText("크기 (가로 / 세로)"), ShowIf("@_resizeChildren || _mode == LayoutMode.Grid"), DisableInPlayMode] Vector2 _childSize = new Vector2(1920, 1080f);

		[Title(Headers.Optional)]
		[SerializeField, BoxGroup("배치 대상"), LabelText("비활성 자식 포함"), DisableInPlayMode] bool _includeInactive;
		[SerializeField, BoxGroup("배치 대상"), LabelText("제외할 자식"), DisableInPlayMode] RectTransform[] _excludedChildren = Array.Empty<RectTransform>();

#if UNITY_EDITOR
		[Button("지금 배치", ButtonSizes.Medium), BoxGroup("배치"), DisableInPlayMode]
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