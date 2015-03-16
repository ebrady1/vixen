using System;
using System.Collections.Generic;
using QMLibrary;
using VixenModules.Media.Audio;
using VixenModules.Sequence.Timed;

namespace VixenModules.Analysis.NoteOnsets
{
	public class NoteOnsetsDetectionManager
	{

		public NoteOnsetsDetectionManager(Audio audio)
		{
			Audio = audio;
		}

		public Audio Audio { get; set; }


		public List<MarkCollection> DoDetection(int audioTime, NoteOnsetsSettings settings)
		{
			ManagedPlugin plugin = null;
			byte[] bSamples;
			float[] fSamples;

			int snippetTime = audioTime;

			List<MarkCollection> retVal = new List<MarkCollection>();

			if (Audio.Channels != 0)
			{
				plugin = new QMNoteOnsets(Audio.Frequency);

				bSamples = Audio.GetSamples(0, (int)Audio.NumberSamples);
				fSamples = new float[(int)(Audio.Frequency * snippetTime)];

				int dataStep = Audio.BytesPerSample;

				for (int j = 0, sampleNum = 0; j < bSamples.Length; j += dataStep, sampleNum++)
				{
					fSamples[sampleNum] = dataStep == 2 ?
						BitConverter.ToInt16(bSamples, j) : BitConverter.ToInt32(bSamples, j);
				}

				//				plugin.SetParameter("bpb", bbSettings.Settings.BeatsPerBar);

				plugin.Initialise(1,
					(uint)plugin.GetPreferredStepSize(),
					(uint)plugin.GetPreferredBlockSize());

				//				retVal = BuildMarkCollections(markCollection, bbSettings.Settings);
			}

			return retVal;
		}

	}
}
