using Cysharp.Threading.Tasks;
using ParkMinPackages.Foundation.Components;
using ParkMinPackages.Workflow.Default.Components.UIs;
using UnityEngine;

namespace ParkMinPackages.Workflow.Default.Components.Features
{
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIShowHideFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public void Show() {
			Owner.UIActivator.ActiveAsync(throwIfTransitioning: false).Forget();
		}

		public void Hide() {
			Owner.UIActivator.DeactivateAsync(throwIfTransitioning: false).Forget();
		}
	}
}