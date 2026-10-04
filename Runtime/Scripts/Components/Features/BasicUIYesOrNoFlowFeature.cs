using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using ParkMinDev.UPM.Workflow.Default.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUIYesOrNoFlowFeature")]
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIYesOrNoFlowFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public async UniTask<YesOrNo> ShowUntilAnsweredAsync(CancellationToken cancellationToken) {
			await Owner.UIActivator.ActiveAsync(cancellationToken);

			using CancellationTokenSource answerCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			int buttonIndex;
			try {
				buttonIndex = await UniTask.WhenAny(
					_yesButton.OnClickAsync(answerCancellationTokenSource.Token),
					_noButton.OnClickAsync(answerCancellationTokenSource.Token)
				);
			}
			finally {
				answerCancellationTokenSource.Cancel();
			}
			await Owner.UIActivator.DeactivateAsync(cancellationToken);

			return buttonIndex == 0 ? YesOrNo.Yes : YesOrNo.No;
		}

		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_yesButton == null)
				throw new ArgumentException($"{nameof(YesButton)} is not assigned.");
			if (_noButton == null)
				throw new ArgumentException($"{nameof(NoButton)} is not assigned.");
		}

		// - Public Properties -
		public Button YesButton
		{
			get { return _yesButton; }
			set { _yesButton = value; }
		}
		public Button NoButton
		{
			get { return _noButton; }
			set { _noButton = value; }
		}

		// - Internals -
		[Header(Headers.Injectable)]
		[SerializeField] Button _yesButton;
		[SerializeField] Button _noButton;
	}
}