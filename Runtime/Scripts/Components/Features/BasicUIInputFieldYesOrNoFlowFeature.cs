using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Workflow.Default.Enums;
using ParkMinDev.UPM.Workflow.Default.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUIInputFieldYesOrNoFlowFeature")]
	[RequireComponent(typeof(BasicUIYesOrNoFlowFeature))]
	public sealed class BasicUIInputFieldYesOrNoFlowFeature : Feature<BasicUIYesOrNoFlowFeature>
	{
		// - Public Methods -
		public async UniTask<(YesOrNo Answer, string Input)> InputAsync(CancellationToken cancellationToken) {
			YesOrNo answer = await Owner.ShowUntilAnsweredAsync(cancellationToken);
			return (answer, _inputField.text);
		}

		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_inputField == null)
				throw new ArgumentException($"{nameof(InputField)} is not assigned.");
		}

		// - Public Properties -
		public InputField InputField
		{
			get { return _inputField; }
			set { _inputField = value; }
		}

		// - Internals -
		[Header(Headers.Injectable)]
		[SerializeField] InputField _inputField;
	}
}