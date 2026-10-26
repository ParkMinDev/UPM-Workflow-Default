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
				foreach (UnityEngine.Transform child in transform) {
					if (child is UnityEngine.RectTransform rectTransform)
						rectTransform.sizeDelta = myRectTransform.sizeDelta;
				}
			}
		}
	}
}