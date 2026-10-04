using System;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Workflow.Default.Components.UIs;
using ParkMinDev.UPM.Workflow.Default.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "BasicUITextMessageFeature")]
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUITextMessageFeature : Feature<BasicUI>
	{
		// - Public Methods -
		public void SetMessage(string title, string message) {
			_titleText.text = title;
			_messageText.text = message;
		}

		public void SetActiveByContent() {
			_titleText.gameObject.SetActive(!string.IsNullOrEmpty(_titleText.text));
			_messageText.gameObject.SetActive(!string.IsNullOrEmpty(_messageText.text));
		}

		public string GetTitle() {
			return _titleText.text;
		}

		public string GetMessage() {
			return _messageText.text;
		}

		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_titleText == null)
				throw new ArgumentException($"{nameof(TitleText)} is not assigned.");
			if (_messageText == null)
				throw new ArgumentException($"{nameof(MessageText)} is not assigned.");
		}

		// - Public Properties -
		public Text TitleText
		{
			get { return _titleText; }
			set { _titleText = value; }
		}
		public Text MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		// - Internals -
		[Header(Headers.Injectable)]
		[SerializeField] Text _titleText;
		[SerializeField] Text _messageText;
	}
}
