using Sirenix.OdinInspector;

namespace ParkMinDev.UPM.Workflow.Default.Components.UIs
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.UIs", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "UIRoot")]
	public class UIRoot : Actor
	{
		[Button]
		void SetChildsSize() {
			UnityEngine.RectTransform myRectTransform = GetComponent<UnityEngine.RectTransform>();
			if (myRectTransform != null) {
				UnityEngine.RectTransform[] childRectTransforms = GetComponentsInChildren<UnityEngine.RectTransform>();
				foreach (UnityEngine.RectTransform rectTransform in childRectTransforms) {
					if (rectTransform != transform)
						rectTransform.sizeDelta = myRectTransform.sizeDelta;
				}
			}
		}
	}
}