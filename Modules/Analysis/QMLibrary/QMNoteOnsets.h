#ifndef QMNOTEONSETS_H
#define QMNOTEONSETS_H

#include <ManagedPlugin.h>
#include <ManagedPluginSettings.h>

namespace QMLibrary
{

	public enum class DetectionFunctionType : int
	{
		HIGH_FREQUENCY = 0x0,
		SPECTRAL_DIFFERENCE = 0x1, 
		PHASE_DEVIATION = 0x2,
		COMPLEX_DOMAIN = 0x3,
		BROADBAND_ENERGY_RISE = 0x4

	} ;

	public ref class QMNoteOnsets : public ManagedPlugin
	{

		public:
			QMNoteOnsets(float inputSampleRate);
			virtual bool SetPluginSettings(ManagedPluginSettings^ settings) override;
			virtual ManagedPluginSettings^ GetPluginSettings() override;

			bool SetDetectionFunction(DetectionFunctionType detectionFn);
			bool SetAdaptiveWhitening(bool enable);
			bool SetSensitivity(int sensitivity);
	};

	public ref class QMNoteOnsetsSettings : public ManagedPluginSettings
	{
		private:
			DetectionFunctionType m_detectFn;
			int m_sensitivity;
			bool m_whiten;

		public:
			QMNoteOnsetsSettings();

			DetectionFunctionType GetDetectionFunction();
			bool SetDetectionFunction(DetectionFunctionType detectionFunction);

			int GetSensitivity();
			bool SetSensitivity(int sensitivity);

			bool GetAdaptiveWhitening();
			bool SetAdaptiveWhitening(bool whiten);
	};

}

#endif //QMNOTEONSETS_H