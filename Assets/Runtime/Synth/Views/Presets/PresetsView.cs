using System;
using System.Collections.Generic;
using System.IO;
using Runtime.Synth.Presets;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Synth.Views
{
	public class PresetsView : MonoBehaviour
	{
		[SerializeField] private GameObject presetViewPrefab;
		[SerializeField] private RectTransform presetListContainer;
		[SerializeField] private Button nextPresetButton;
		[SerializeField] private Button prevPresetButton;
		[SerializeField] private Button duplicatePresetButton;
		[SerializeField] private Button renamePresetButton;
		[SerializeField] private Button savePresetButton;

		private List<PresetInstanceView> _presetViews = new();
		private int _selectedIndex = 0;
		
		public SynthesizerPreset ActivePreset { get; private set; }

		public event Action OnPresetChanged; 

		private void Awake()
		{
			nextPresetButton.onClick.AddListener(NextPreset);
			prevPresetButton.onClick.AddListener(PrevPreset);
			duplicatePresetButton.onClick.AddListener(DuplicateSelectedPreset);
			savePresetButton.onClick.AddListener(SavePreset);
		}

		private void SavePreset()
		{
			string fileName = _presetViews[_selectedIndex].FileName;
			string path = PresetUtilities.GetPathToPresetFile(fileName);
			using var streamWriter = new StreamWriter(path, false);
			streamWriter.Write(ActivePreset.ToJson());
			streamWriter.Close();
		}

		private void DuplicateSelectedPreset()
		{
			string selectedPresetName = _presetViews[_selectedIndex].FileName;
			string pathToPresetFile = PresetUtilities.GetPathToPresetFile(selectedPresetName);
			File.Copy(pathToPresetFile, pathToPresetFile.Replace(".json", "") + "(Copy).json");
			PreparePresets();
		}

		private void Start()
		{
			PreparePresets();
		}

		private void PrevPreset()
		{
			_presetViews[_selectedIndex].SetSelected(false);
			
			_selectedIndex--;
			if (_selectedIndex < 0)
				_selectedIndex = _presetViews.Count - 1;
			
			_presetViews[_selectedIndex].SetSelected(true);
			
			LoadSelectedPreset();
		}

		private void NextPreset()
		{
			_presetViews[_selectedIndex].SetSelected(false);
			
			_selectedIndex++;
			if (_selectedIndex >= _presetViews.Count)
				_selectedIndex = 0;
			
			_presetViews[_selectedIndex].SetSelected(true);
			
			LoadSelectedPreset();
		}

		private void PreparePresets()
		{
			for (int i = presetListContainer.childCount - 1; i >= 0; i--)
			{
				Destroy(presetListContainer.GetChild(i).gameObject);
			}
			_presetViews.Clear();
			
			var presetNames = PresetUtilities.GetPresetNames();

			foreach (var presetName in presetNames)
			{
				GameObject instance = Instantiate(presetViewPrefab, presetListContainer);
				var view = instance.GetComponent<PresetInstanceView>();
				view.FileName = presetName;
				_presetViews.Add(view);
			}

			_selectedIndex = 0;
			_presetViews[_selectedIndex].SetSelected(true);
			LoadSelectedPreset();
		}

		private void LoadSelectedPreset()
		{
			string fileName = _presetViews[_selectedIndex].FileName;
			SynthesizerPreset preset = LoadPreset(fileName);
			ActivePreset = preset;
			OnPresetChanged?.Invoke();
		}

		private SynthesizerPreset LoadPreset(string presetName)
		{
			var path = PresetUtilities.GetPathToPresetFile(presetName);
			using var streamReader = new StreamReader(path);
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			return SynthesizerPreset.FromJson(json);
		}
	}
}