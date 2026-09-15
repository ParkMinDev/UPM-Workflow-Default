using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ParkMinPackages.Foundation.Components;
using ParkMinPackages.UGUI.Enums;
using ParkMinPackages.Workflow.Default.Components.UIs;
using UnityEngine;

namespace ParkMinPackages.Workflow.Default.Components.Features
{
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIShowForDurationFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public async UniTask ShowForDurationAsync(TimeSpan duration, CancellationToken cancellationToken) {
			await Owner.UIActivator.ActiveAsync(cancellationToken, false, AnimationCancelBehaviour.ResetToStart);
			CancellationToken cancellationTokenUntilNextActivationRequest = Owner.UIActivator.CancellationTokenUntilNextActivationRequest;
			await UniTask.Delay(duration, cancellationToken: cancellationTokenUntilNextActivationRequest, cancelImmediately: true);
			await Owner.UIActivator.DeactivateAsync(cancellationToken, false, AnimationCancelBehaviour.Complete);
		}
	}
}
