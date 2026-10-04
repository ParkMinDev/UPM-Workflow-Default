namespace ParkMinDev.UPM.Workflow.Default.Interfaces
{
	public interface ILayoutRebuildable
	{
		public void RebuildLayout();
	}

	public static class ILayoutRebuildableExtensions
	{
		public static T WithRebuildLayout<T>(this T layoutRebuildable) where T : ILayoutRebuildable {
			layoutRebuildable.RebuildLayout();
			return layoutRebuildable;
		}
	}
}
