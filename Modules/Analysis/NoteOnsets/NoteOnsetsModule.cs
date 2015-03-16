using System;
using System.Collections.Generic;
using QMLibrary;
using Vixen.Module.Analysis;
using VixenModules.Media.Audio;
using VixenModules.Sequence.Timed;

namespace VixenModules.Analysis.NoteOnsets
{
	public class NoteOnsetsModule : AnalysisModuleInstanceBase
	{
		private NoteOnsetsDetectionManager m_detectionManager;

		public NoteOnsetsModule(Audio audio)
		{
			m_detectionManager = new NoteOnsetsDetectionManager(audio);
		}

		public override void Loading() { }
		public override void Unloading() { }

		public List<MarkCollection> DoNoteOnsetDetection(List<MarkCollection> markCollection)
		{
			List<MarkCollection> retVal = null;
			NoteOnsetsSettingsForm onsetSettings = new NoteOnsetsSettingsForm(m_detectionManager);
			onsetSettings.ShowDialog();
			return retVal;

		}
	}
}
