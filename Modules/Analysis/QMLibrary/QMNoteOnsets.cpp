#include <QMFiles/OnsetDetect.h>
#include <QMNoteOnsets.h>


namespace QMLibrary
{
	QMNoteOnsets::QMNoteOnsets(float inputSampleRate)
	{
		m_plugin = new OnsetDetector(inputSampleRate);
	}

	bool QMNoteOnsets::SetAdaptiveWhitening(bool enable)
	{
		m_plugin->setParameter("whiten", ((enable) ? 1 : 0));
		return true;
	}

	bool QMNoteOnsets::SetDetectionFunction(DetectionFunctionType detectFn)
	{
		m_plugin->setParameter("dftype", (float)detectFn);
		return true;
	}

	bool QMNoteOnsets::SetSensitivity(int sensitivity)
	{
		m_plugin->setParameter("sensitivity", sensitivity);
		return true;
	}

	bool QMNoteOnsets::SetPluginSettings(ManagedPluginSettings^ pluginSettings)
	{
		bool retVal = false;
		if (pluginSettings != nullptr)
		{
			QMNoteOnsetsSettings^ settings = (QMNoteOnsetsSettings^)pluginSettings;
			this->SetDetectionFunction(settings->GetDetectionFunction());
			this->SetSensitivity(settings->GetSensitivity());
			this->SetAdaptiveWhitening(settings->GetAdaptiveWhitening());
		}

		return retVal;
	}

	ManagedPluginSettings^ QMNoteOnsets::GetPluginSettings()
	{
		return nullptr;
	}

	QMNoteOnsetsSettings::QMNoteOnsetsSettings()
	{
		m_detectFn = DetectionFunctionType::COMPLEX_DOMAIN;
		m_sensitivity = 50;
		m_whiten = false;
	}

	DetectionFunctionType QMNoteOnsetsSettings::GetDetectionFunction()
	{
		return m_detectFn;
	}

	bool QMNoteOnsetsSettings::SetDetectionFunction(DetectionFunctionType detectionFn)
	{
		m_detectFn = detectionFn;
		return true;
	}

	int QMNoteOnsetsSettings::GetSensitivity()
	{
		return m_sensitivity;
	}

	bool QMNoteOnsetsSettings::SetSensitivity(int sensitivity)
	{
		m_sensitivity = sensitivity;
		return true;
	}

	bool QMNoteOnsetsSettings::GetAdaptiveWhitening()
	{
		return m_whiten;
	}

	bool QMNoteOnsetsSettings::SetAdaptiveWhitening(bool whiten)
	{
		m_whiten = whiten;
		return true;
	}
}

