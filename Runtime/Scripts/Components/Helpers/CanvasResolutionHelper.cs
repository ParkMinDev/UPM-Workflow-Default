using UnityEngine;
using UnityEngine.UI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ParkMinPackages.Workflow.Default.Components.Helpers
{
	public class CanvasResolutionHelper : MonoBehaviour
	{
#if ODIN_INSPECTOR
		[Button]
#endif
		public void Apply() {
			_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			_canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			_canvasScaler.referenceResolution = new Vector2(_width, _height);

			_uiRoot.sizeDelta = new Vector2(_width, _height);
			_uiRoot.name = $"UI Root ({_width}x{_height})";

			_windowBox.sizeDelta = new Vector2(_width, _height);
			_windowBox.name = $"Window Box ({_width}x{_height})";
		}

#if ODIN_INSPECTOR
		[SerializeField, Required]
#else
		[SerializeField]
#endif
		CanvasScaler _canvasScaler;

#if ODIN_INSPECTOR
		[SerializeField, Required]
#else
		[SerializeField]
#endif
		RectTransform _uiRoot;

#if ODIN_INSPECTOR
		[SerializeField, Required]
#else
		[SerializeField]
#endif
		RectTransform _windowBox;

		[SerializeField] int _width = 1920;
		[SerializeField] int _height = 1080;
	}
}