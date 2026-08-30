using System;
using ParkMinPackages.Foundation.Components;
using ParkMinPackages.Foundation.Constants;
using ParkMinPackages.Workflow.Default.Components.UIs;
using ParkMinPackages.Workflow.Default.Interfaces;
using TMPro;
using UnityEngine;

namespace ParkMinPackages.Workflow.Default.Components.Features
{
	[RequireComponent(typeof(BasicUI))]
	public sealed class BasicUITMPTextMessageFeature : Feature<BasicUI>
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
		public TMP_Text TitleText
		{
			get { return _titleText; }
			set { _titleText = value; }
		}
		public TMP_Text MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		// - Internals -
		[Header(Headers.Injectable)]
		[SerializeField] TMP_Text _titleText;
		[SerializeField] TMP_Text _messageText;
	}
}
