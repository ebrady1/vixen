using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QMLibrary;
using Vixen.Module.Analysis;
using VixenModules.Media.Audio;
using VixenModules.Sequence.Timed;

namespace VixenModules.Analysis.NoteOnsets
{
	public class NoteOnsetsModule : AnalysisModuleInstanceBase
	{
		private Audio m_audioModule = null;
		private ManagedPlugin m_plugin = null;
		private IDictionary<int, ICollection<ManagedFeature>> m_featureSet;
		private byte[] m_bSamples;
		private float[] m_fSamples;

		private const string NOTEONSET_COLLECTION_NAME = "Note Onsets";

		public NoteOnsetsModule(Audio audio)
		{
			m_audioModule = audio;
			m_featureSet = null;
		}

		public override void Loading() { }
		public override void Unloading() { }

		private MarkCollection
			ExtractAllMarksFromFeatureSet(ICollection<ManagedFeature> featureSet)
		{
			MarkCollection mc = new MarkCollection();

			foreach (ManagedFeature feature in featureSet)
			{
				//if (feature.hasTimestamp)
				{
					double featureMS = feature.timestamp.totalMilliseconds();
					mc.Marks.Add(TimeSpan.FromMilliseconds(featureMS));
				}
			}
			return mc;
		}

		private IDictionary<int, ICollection<ManagedFeature>> GenerateFeatures(ManagedPlugin plugin, float[] fSampleData, bool showProgress = true)
		{
			int i = 0;
			int j = 0;
			IDictionary<int, ICollection<ManagedFeature>> retVal =
				new Dictionary<int, ICollection<ManagedFeature>>();

//			BeatsAndBarsProgress progressDlg = new BeatsAndBarsProgress();
			if (showProgress)
			{
//				progressDlg.Show();
			}

			int stepSize = plugin.GetPreferredStepSize();

			double progressVal = 0;
			uint frequency = (uint)m_audioModule.Frequency;
			if (frequency != 0)
			{
				float[] fSamples = new float[plugin.GetPreferredBlockSize()];
				for (j = 0;
					((fSampleData.Length - j) >= plugin.GetPreferredBlockSize());
					j += stepSize)
				{
					progressVal = ((double)j / (double)fSampleData.Length) * 100.0;
//					progressDlg.UpdateProgress((int)progressVal);

					Array.Copy(fSampleData, j, fSamples, 0, fSamples.Length);
					plugin.Process(fSamples,
							ManagedRealtime.frame2RealTime(j, (uint)m_audioModule.Frequency));
				}

				Array.Clear(fSamples, 0, fSamples.Length);
				Array.Copy(fSampleData, j, fSamples, 0, fSampleData.Length - j);
				plugin.Process(fSamples,
						ManagedRealtime.frame2RealTime(j, (uint)m_audioModule.Frequency));

//				progressDlg.Close();

				retVal = plugin.GetRemainingFeatures();
			}

			return retVal;
		}

		private List<MarkCollection> BuildMarkCollections(List<MarkCollection> markCollection,
													QMNoteOnsetsSettings settings)
		{
			List<MarkCollection> retVal = new List<MarkCollection>();

			m_featureSet = GenerateFeatures(m_plugin, m_fSamples);
			markCollection.RemoveAll(x => x.Name.Equals(NOTEONSET_COLLECTION_NAME));
			MarkCollection mc = ExtractAllMarksFromFeatureSet(m_featureSet[0]);
			mc.MarkColor = Color.White;
			mc.Name = NOTEONSET_COLLECTION_NAME;
			mc.Enabled = true;
			retVal.Add(mc);

			return retVal;
		}

		public List<MarkCollection> DoNoteOnsetDetection(List<MarkCollection> markCollection)
		{
			List<MarkCollection> retVal = new List<MarkCollection>();

			if (m_audioModule.Channels != 0)
			{
				m_plugin = new QMNoteOnsets(m_audioModule.Frequency);

				m_bSamples = m_audioModule.GetSamples(0, (int)m_audioModule.NumberSamples);
				m_fSamples = new float[m_audioModule.NumberSamples];

				int dataStep = m_audioModule.BytesPerSample;

				for (int j = 0, sampleNum = 0; j < m_bSamples.Length; j += dataStep, sampleNum++)
				{
					//m_fSamples[sampleNum] = dataStep == 2 ?
					//	BitConverter.ToInt16(m_bSamples, j) : BitConverter.ToInt32(m_bSamples, j);

					m_fSamples[sampleNum] = dataStep == 2 ?
						BitConverter.ToInt16(m_bSamples, j) : BitConverter.ToInt16(m_bSamples, j);
				}

				NoteOnsetsSettingsForm onsetSettings = new NoteOnsetsSettingsForm();

				DialogResult result = onsetSettings.ShowDialog();
				if (result == DialogResult.OK)
				{
					QMNoteOnsetsSettings settings = onsetSettings.Settings;
					m_plugin.SetPluginSettings(settings);
					m_plugin.Initialise();

					retVal = BuildMarkCollections(markCollection, onsetSettings.Settings);
				}	
			}

			return retVal;
		}
	}
}
