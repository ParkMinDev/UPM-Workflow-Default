using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using ParkMinDev.UPM.Workflow.Default.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUIConfirmFlowFeature")]
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUIConfirmFlowFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public async UniTask ConfirmAsync(CancellationToken cancellationToken) {
			await Owner.UIActivator.ActiveAsync(cancellationToken);
			await _confirmButton.OnClickAsync(cancellationToken);
			await Owner.UIActivator.DeactivateAsync(cancellationToken);
		}

		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_confirmButton == null)
				throw new ArgumentException($"{nameof(ConfirmButton)} is not assigned.");
		}

		// - Public Properties -
		public Button ConfirmButton
		{
			get { return _confirmButton; }
			set { _confirmButton = value; }
		}

		// - Internals -
		[Header(Headers.Injectable)]
		[SerializeField] Button _confirmButton;
	}
}