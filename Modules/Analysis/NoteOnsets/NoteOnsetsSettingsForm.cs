using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common.Controls;
using VixenModules.Media.Audio;

namespace VixenModules.Analysis.NoteOnsets
{

	public partial class NoteOnsetsSettingsForm : Form
	{
		private ToolTip m_toolTip;
		private PreviewWaveform m_previewWaveForm;
		private Dictionary<String, NoteOnsetsPresetData> m_presets;
		private NoteOnsetsDetectionManager m_detectionManager;

		public NoteOnsetsSettingsForm(NoteOnsetsDetectionManager detectionManager)
		{
			InitializeComponent();

			m_detectionManager = detectionManager;

			m_toolTip = new ToolTip();
			m_toolTip.AutoPopDelay = 5000;
			m_toolTip.InitialDelay = 500;
			m_toolTip.ReshowDelay = 500;
			m_toolTip.ShowAlways = true;
			m_toolTip.Active = true;

			m_toolTip.SetToolTip(GeneralPurposeButton, "Use settings for General Purpose detection.  Good for most uses");
			m_toolTip.SetToolTip(SoftOnsetsButton, "Use settings to detect soft note onsets");
			m_toolTip.SetToolTip(PercussiveOnsetsButton, "Use settings to detect percussive note onsets");

			m_toolTip.SetToolTip(HighFrequencyRadio, "Detect using High Frequency Content of Audio");
			m_toolTip.SetToolTip(SpectralDifferenceRadio, "Detect using Spectral Differences in Audio");
			m_toolTip.SetToolTip(PhaseDeviationRadio, "Detect using Phase Deviations found in Audio");
			m_toolTip.SetToolTip(ComplexDomainRadio, "Detect using Complex Domain algorithm");
			m_toolTip.SetToolTip(BroadBandEnergyRadio, "Detect using broardband energy changes");
			m_toolTip.SetToolTip(DetectionFunctionGroupBox, "Detection Algorithm");

			m_toolTip.SetToolTip(SensitivityGroupBox, "Algorithm Sensitivity");
			m_toolTip.SetToolTip(SensitivityTrackBar, "Algorithm Sensitivity");
			m_toolTip.SetToolTip(SensitivityValue, "Algorithm Sensitivity");

			m_toolTip.SetToolTip(OtherSettingsGroupBox, "Other Algorithm Settings");
			m_toolTip.SetToolTip(AdaptiveWhiteningCheckBox, "Other Algorithm Settings");

//			m_settingsData = m_settingsData ?? new BeatBarSettingsData("Beats");

			m_presets = new Dictionary<string, NoteOnsetsPresetData>();
			m_presets[GeneralPurposeButton.Text] = new NoteOnsetsPresetData(ComplexDomainRadio, 50, false);
			m_presets[SoftOnsetsButton.Text] = new NoteOnsetsPresetData(ComplexDomainRadio, 40, true);
			m_presets[PercussiveOnsetsButton.Text] = new NoteOnsetsPresetData(BroadBandEnergyRadio, 40, false);
			SetNoteOnsetOutputSettings();

			m_previewWaveForm = new PreviewWaveform(m_detectionManager.Audio);
			m_previewWaveForm.Width = PreviewGroupBox.Width - 10;
			m_previewWaveForm.Height = 75;
			m_previewWaveForm.Location = new Point(PreviewGroupBox.Location.X + 5, 25);

			PreviewGroupBox.Controls.Add(m_previewWaveForm);
		}

		private void SetNoteOnsetOutputSettings()
		{
			
		}

		private void NoteOnsetsSettings_Load(object sender, EventArgs e)
		{
			PresetsClicked(GeneralPurposeButton.Text);
		}

		private void PresetsClicked(String selectedItem)
		{
			NoteOnsetsPresetData presetData = null;
		
			m_presets.TryGetValue(selectedItem, out presetData);
			if (presetData != null)
			{
				presetData.EnabledRadio.Checked = true;
				SensitivityTrackBar.Value = presetData.Sensitivity;
				AdaptiveWhiteningCheckBox.Checked = presetData.Whitening;
				SetNoteOnsetOutputSettings();
			}
		}

		private void SensitivityTrackBar_Scroll(object sender, EventArgs e)
		{
			SensitivityValue.Text = SensitivityTrackBar.Value.ToString();
		}

		private void SensitivityTrackBar_ValueChanged(object sender, EventArgs e)
		{
			SensitivityValue.Text = SensitivityTrackBar.Value.ToString();
		}

		private void PresetButton_Click(object sender, EventArgs e)
		{
			Button button = sender as Button;
			PresetsClicked(button.Text);
		}
	}

	internal class NoteOnsetsPresetData
	{
		public NoteOnsetsPresetData(RadioButton enabledRadio, int sensitivity, bool whiten)
		{
			EnabledRadio = enabledRadio;
			Sensitivity = sensitivity;
			Whitening = whiten;
		}

		public RadioButton EnabledRadio { get; set; }
		public int Sensitivity { get; set; }
		public bool Whitening { get; set; }
	}

}
