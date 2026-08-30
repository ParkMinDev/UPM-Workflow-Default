using Cysharp.Threading.Tasks;
using ParkMinPackages.Foundation.Components;
using ParkMinPackages.Foundation.Objects.Threading;
using ParkMinPackages.Workflow.Default.Components.UIs;
using UnityEngine;

namespace ParkMinPackages.Workflow.Default.Components.Features
{
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