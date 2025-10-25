using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Runtime.Synth.Presets
{
	public static class PresetUtilities
	{
		public static string GetPathToPresets() => Path.Combine(Application.streamingAssetsPath, "Presets");
		
		public static List<string> GetPresetNames() => 
			Directory.GetFiles(GetPathToPresets(), "*.json")
			.Select(Path.GetFileName)
			.ToList();
		
		public static string GetPathToPresetFile(string fileName) => Path.Combine(GetPathToPresets(), fileName);
	}
}