using System;
using ParkMinPackages.Foundation.Components;
using ParkMinPackages.Workflow.Default.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ParkMinPackages.Workflow.Default.Components.Features
{
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