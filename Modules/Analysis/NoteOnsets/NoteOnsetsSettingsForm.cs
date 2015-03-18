using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common.Controls;
using QMLibrary;
using VixenModules.Media.Audio;

namespace VixenModules.Analysis.NoteOnsets
{

	public partial class NoteOnsetsSettingsForm : Form
	{
		private ToolTip m_toolTip;
		private Dictionary<String, NoteOnsetsPresetData> m_presets;
		
		public NoteOnsetsSettingsForm()
		{
			InitializeComponent();

			m_toolTip = new ToolTip();
			m_toolTip.AutoPopDelay = 5000;
			m_toolTip.InitialDelay = 500;
			m_toolTip.ReshowDelay = 500;
			m_toolTip.ShowAlways = true;
			m_toolTip.Active = true;

			m_toolTip.SetToolTip(GeneralPurposeButton, "General purpose, good for most uses");
			m_toolTip.SetToolTip(SoftOnsetsButton, "Better for soft note onsets");
			m_toolTip.SetToolTip(PercussiveOnsetsButton, "Better for percussive note onsets");

			m_toolTip.SetToolTip(HighFrequencyRadio, "All music types with a High Frequency component");
			m_toolTip.SetToolTip(SpectralDifferenceRadio, "Percussive and highly dynamic music");
			m_toolTip.SetToolTip(PhaseDeviationRadio, "All music types - Detects Tonal Shifts");
			m_toolTip.SetToolTip(ComplexDomainRadio, "General Purpose Algorithm - Good results in most cases ");
			m_toolTip.SetToolTip(BroadBandEnergyRadio, "Percussive and highly dynamic music");
			m_toolTip.SetToolTip(DetectionFunctionGroupBox, "Detection Algorithms");

			m_toolTip.SetToolTip(SensitivityGroupBox, "Algorithm Sensitivity");
			m_toolTip.SetToolTip(SensitivityTrackBar, "Algorithm Sensitivity");
			m_toolTip.SetToolTip(SensitivityValue, "Algorithm Sensitivity");

			m_toolTip.SetToolTip(AdaptiveWhiteningCheckBox, "Use for highly dynamic music.");

			m_presets = new Dictionary<string, NoteOnsetsPresetData>();
			m_presets[GeneralPurposeButton.Text] = new NoteOnsetsPresetData(ComplexDomainRadio, 50, false);
			m_presets[SoftOnsetsButton.Text] = new NoteOnsetsPresetData(ComplexDomainRadio, 40, true);
			m_presets[PercussiveOnsetsButton.Text] = new NoteOnsetsPresetData(BroadBandEnergyRadio, 40, false);

			Settings = new QMNoteOnsetsSettings();

			SetNoteOnsetOutputSettings();
		}

		public QMNoteOnsetsSettings Settings { get; set; }

		private void SetNoteOnsetOutputSettings()
		{
			if (BroadBandEnergyRadio.Checked)
			{
				Settings.SetDetectionFunction(DetectionFunctionType.BROADBAND_ENERGY_RISE);
			}
			else if (ComplexDomainRadio.Checked)
			{
				Settings.SetDetectionFunction(DetectionFunctionType.COMPLEX_DOMAIN);
			}
			else if (HighFrequencyRadio.Checked)
			{
				Settings.SetDetectionFunction(DetectionFunctionType.HIGH_FREQUENCY);
			}
			else if (PhaseDeviationRadio.Checked)
			{
				Settings.SetDetectionFunction(DetectionFunctionType.PHASE_DEVIATION);
			}
			else if (SpectralDifferenceRadio.Checked)
			{
				Settings.SetDetectionFunction(DetectionFunctionType.SPECTRAL_DIFFERENCE);
			}

			Settings.SetSensitivity(SensitivityTrackBar.Value);
			Settings.SetAdaptiveWhitening(AdaptiveWhiteningCheckBox.Checked);
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
			SetNoteOnsetOutputSettings();
		}

		private void SensitivityTrackBar_ValueChanged(object sender, EventArgs e)
		{
			SensitivityValue.Text = SensitivityTrackBar.Value.ToString();
			SetNoteOnsetOutputSettings();
		}

		private void PresetButton_Click(object sender, EventArgs e)
		{
			if (sender != null)
			{
				Button button = sender as Button;
				PresetsClicked(button.Text);
			}
			
		}

		private void DetectFnRadio_Click(object sender, EventArgs e)
		{
			SetNoteOnsetOutputSettings();
		}

		private void AdaptiveWhiteningCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			SetNoteOnsetOutputSettings();
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			SetNoteOnsetOutputSettings();
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
