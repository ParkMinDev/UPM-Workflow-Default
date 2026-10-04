using ParkMinDev.UPM.UGUI.Components;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Components.UIs
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.UIs", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUI")]
	[RequireComponent(typeof(UIActivator))]
	public class BasicUI : Actor
	{
		public UIActivator UIActivator
		{
			get { return _uiActivator; }
		}

		protected override void Awake() {
			base.Awake();
			_uiActivator = GetComponent<UIActivator>();
		}

		UIActivator _uiActivator;
	}
}