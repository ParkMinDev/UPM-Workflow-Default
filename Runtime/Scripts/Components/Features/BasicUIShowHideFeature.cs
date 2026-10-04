using Cysharp.Threading.Tasks;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Objects.Threading;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUIShowHideFeature")]
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIShowHideFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public void Show() {
			Owner.UIActivator.ActiveAsync(_showHideCancellationTokenSource.CancelPreviousAndCreateToken()).Forget();
		}

		public void Hide() {
			Owner.UIActivator.DeactivateAsync(_showHideCancellationTokenSource.CancelPreviousAndCreateToken()).Forget();
		}

		// - Handler -
		protected override void OnDestroy() {
			_showHideCancellationTokenSource.Dispose();
			base.OnDestroy();
		}

		// - Internals -
		readonly AutoRenewCancellationTokenSource _showHideCancellationTokenSource = new AutoRenewCancellationTokenSource();
	}
}