using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.CutomEffect
{
	class FPPDecode
	{
		FPPDecode()
		{

		}

		bool Load(String fileName)
		{
			return false;
		}

		Int32 StepSize()
		{
			return 0;
		}


	}
}
void FRAMECLASS WriteFalconPiModelFile(const wxString& filename, long numChans, long numPeriods,
        SeqDataType *dataBuf, int startAddr, int modelSize)
{
    wxUint16 fixedHeaderLength = 20;
    wxUint32 stepSize = rountTo4(numChans);
    wxFile f;

    size_t ch;
    if (!f.Create(filename,true))
    {
        ConversionError(wxString("Unable to create file: ")+filename);
        return;
    }

    wxUint8* buf;
    buf = (wxUint8 *)calloc(sizeof(wxUint8),stepSize);

    // Header Information
    // Format Identifier
    buf[0] = 'E';
    buf[1] = 'S';
    buf[2] = 'E';
    buf[3] = 'Q';
    // Data offset
    buf[4] = (wxUint8)1; //Hard coded to export a single model for now
    buf[5] = 0; //Pad byte
    buf[6] = 0; //Pad byte
    buf[7] = 0; //Pad byte
    // Step Size
    buf[8] = (wxUint8)(stepSize & 0xFF);
    buf[9] = (wxUint8)((stepSize >> 8) & 0xFF);
    buf[10] = (wxUint8)((stepSize >> 16) & 0xFF);
    buf[11] = (wxUint8)((stepSize >> 24) & 0xFF);
    //Model Start address
    buf[12] = (wxUint8)(startAddr & 0xFF);
    buf[13] = (wxUint8)((startAddr >> 8) & 0xFF);
    buf[14] = (wxUint8)((startAddr >> 16) & 0xFF);
    buf[15] = (wxUint8)((startAddr >> 24) & 0xFF);
    // Model Size
    buf[16] = (wxUint8)(modelSize & 0xFF);
    buf[17] = (wxUint8)((modelSize >> 8) & 0xFF);
    buf[18] = (wxUint8)((modelSize >> 16) & 0xFF);
    buf[19] = (wxUint8)((modelSize >> 24) & 0xFF);
    f.Write(buf,fixedHeaderLength);

    for (long period=0; period < numPeriods; period++)
    {
        //if (period % 500 == 499) AppendConvertStatus (string_format("Writing time period %ld\n",period+1));
        wxYield();
        for(ch=0; ch<stepSize; ch++)
        {
            buf[ch] = (*dataBuf)[period][ch];
        }
        f.Write(buf,stepSize);
    }
    f.Close();