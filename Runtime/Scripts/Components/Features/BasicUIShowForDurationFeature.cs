using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.UGUI.Enums;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUIShowForDurationFeature")]
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIShowForDurationFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public async UniTask ShowForDurationAsync(TimeSpan duration, CancellationToken cancellationToken) {
			await Owner.UIActivator.ActiveAsync(cancellationToken, AnimationCancelBehaviour.ResetToStart);
			try {
				await UniTask.Delay(duration, cancellationToken: cancellationToken, cancelImmediately: true);
			}
			catch (OperationCanceledException) {
				Owner.UIActivator.DeactivateImmediate();
				throw;
			}
			await Owner.UIActivator.DeactivateAsync(cancellationToken, AnimationCancelBehaviour.Complete);
		}
	}
}