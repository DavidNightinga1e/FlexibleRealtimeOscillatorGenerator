using Runtime.UI.Modals;

namespace Runtime
{
	public static class ApplicationContext
	{
		public static ModalsController Modals => Context.ModalsController;
		
		private static ApplicationContextInstance Context => ApplicationContextInstance.Instance;
	}
}