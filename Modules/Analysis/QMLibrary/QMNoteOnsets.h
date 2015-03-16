#ifndef QMNOTEONSETS_H
#define QMNOTEONSETS_H

#include <ManagedPlugin.h>

namespace QMLibrary
{
	public ref class QMNoteOnsets : public ManagedPlugin
	{
		public:
			QMNoteOnsets(float inputSampleRate);
	};

}

#endif //QMNOTEONSETS_H