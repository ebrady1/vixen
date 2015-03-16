#include <QMFiles/OnsetDetect.h>
#include <QMNoteOnsets.h>


namespace QMLibrary
{
	QMNoteOnsets::QMNoteOnsets(float inputSampleRate)
	{
		m_plugin = new OnsetDetector(inputSampleRate);
	}
}

