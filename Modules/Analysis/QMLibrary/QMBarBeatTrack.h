#ifndef QMBARBEATTRACK_H
#define QMBARBEATTRACK_H

#include <ManagedPluginSettings.h>
#include <ManagedPlugin.h>

namespace QMLibrary
{
	public ref class QMBarBeatTrack : ManagedPlugin
	{
		public:
			QMBarBeatTrack(float inputSampleRate);
			void SetBeatsPerBar(int beatsPerBar);

			virtual bool SetPluginSettings(ManagedPluginSettings^ settings) override;
			virtual ManagedPluginSettings^ GetPluginSettings() override;
	};

	public ref class QMBarBeatTrackSettings : public ManagedPluginSettings
	{
		private:
			int m_beatsPerBar;

		public:
			QMBarBeatTrackSettings();
			void SetBeatsPerBar(int beatsPerBar);
			int GetBeatsPerBar();
	};

}

#endif