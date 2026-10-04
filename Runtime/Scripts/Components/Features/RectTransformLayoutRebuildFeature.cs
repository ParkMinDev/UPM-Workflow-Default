using System;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Workflow.Default.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinDev.UPM.Workflow.Default.Components.Features
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Components.Features", sourceAssembly: "ParkMinPackages.Workflow.Default", sourceClassName: "RectTransformLayoutRebuildFeature")]
	public sealed class RectTransformLayoutRebuildFeature : Feature<RectTransform>, ILayoutRebuildable
	{
		// - Public Methods -
		public void RebuildLayout() {
			ContentSizeFitter[] contentSizeFitters = Owner.GetComponentsInChildren<ContentSizeFitter>();
			Array.Sort(contentSizeFitters, CompareByHierarchyDepthDescending);

			foreach (ContentSizeFitter contentSizeFitter in contentSizeFitters) {
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(Owner);
		}

		// - Internals -
		int CompareByHierarchyDepthDescending(ContentSizeFitter left, ContentSizeFitter right) {
			return GetHierarchyDepth(right.transform).CompareTo(GetHierarchyDepth(left.transform));
		}

		int GetHierarchyDepth(Transform transform) {
			int depth = 0;

			while (transform != null) {
				depth++;
				transform = transform.parent;
			}

			return depth;
		}
	}
}