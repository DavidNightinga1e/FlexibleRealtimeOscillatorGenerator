using Runtime.Synth;
using Runtime.UI.Modals;

namespace Runtime
{
	public static class ApplicationContext
	{
		public static class PresetEditor
		{
			public static SynthesizerBehaviour SynthesizerBehaviour => Context.PresetEditorContext.SynthesizerBehaviour;
		}
		
		public static ModalsController Modals => Context.ModalsController;
		
		private static ApplicationContextInstance Context => ApplicationContextInstance.Instance;
	}
}