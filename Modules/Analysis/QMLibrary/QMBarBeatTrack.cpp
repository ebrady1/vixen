#include <QMFiles/BarBeatTrack.h>
#include <QMBarBeatTrack.h>

namespace QMLibrary
{
	QMBarBeatTrack::QMBarBeatTrack(float inputSampleRate)
	{
		m_plugin = new BarBeatTracker(inputSampleRate);
	}

	void QMBarBeatTrack::SetBeatsPerBar(int beatsPerBar)
	{
		this->SetParameter("bpb", beatsPerBar);
	}

	bool QMBarBeatTrack::SetPluginSettings(ManagedPluginSettings^ settings)
	{
		bool retVal = false;
		if (settings != nullptr)
		{
			int beatsPerBar = ((QMBarBeatTrackSettings^)settings)->GetBeatsPerBar();
			this->SetParameter("bpb", beatsPerBar);
			retVal = true;
		}
		
		return retVal;
	}

	ManagedPluginSettings^ QMBarBeatTrack::GetPluginSettings()
	{
		return nullptr;
	}




	QMBarBeatTrackSettings::QMBarBeatTrackSettings()
	{
		m_beatsPerBar = 4;  //Defautl to 4.
	}

	void QMBarBeatTrackSettings::SetBeatsPerBar(int beatsPerBar)
	{
		m_beatsPerBar = beatsPerBar;
	}

	int QMBarBeatTrackSettings::GetBeatsPerBar()
	{
		return m_beatsPerBar;
	}
}

