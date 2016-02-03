using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.CustomEffect
{
	class FPPDecode
	{
		private Byte _vMinor = 0;
		private Byte _vMajor = 1;
		private UInt32 _periodDataStart = UInt32.MaxValue;
		private UInt16 _period = 50;
		private const UInt16 _fixedHeaderLength = 28;
		private UInt16 _numUniverses = 0;    //Ignored by Pi Player
		private UInt16 _universeSize = 0;    //Ignored by Pi Player
		private Byte _gamma = 1;             //0=encoded, 1=linear, 2=RGB
		private Byte _colorEncoding = 2;

		private FileStream _infs = null;

		private MemoryStream	_dataIn = null;
		private Byte[] fileData;

		public FPPDecode()
		{

		}

		public byte[] GetPeriodData(UInt32 period)
		{
			_dataIn.Position = _periodDataStart + (SeqNumChannels * period);
			var buffer = new byte[SeqNumChannels];
			_dataIn.Read(buffer, 0, SeqNumChannels);
			return buffer;
		}

		public Int32 SeqNumChannels
		{ get; set; }

		public UInt32 SeqNumPeriods
		{ get; set; }

		public bool Load(String filePath)
		{
			bool retVal = false;
			try
			{
				using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					_dataIn = new MemoryStream();
					fs.CopyTo(_dataIn);
					_dataIn.Position = 0;
				}

				byte[] header = new byte[_fixedHeaderLength];
				_dataIn.Read(header,0,_fixedHeaderLength);

				if ((header[0] == 'F') && (header[1] == 'S') && (header[2] == 'E') && (header[3] == 'Q'))
				{
					//Beginning of Period Data
					_periodDataStart = (uint)(header[4] + ((header[5] << 8) & 0xFF00));

					// Data header
					_vMinor = header[6];
					_vMajor = header[7];

					// Fixed header length

					//At this time, we really don't care about reading the header length,
					//It is assumed to be 28 bytes, if this value ever changes, then this
					//decoder may need modification.
					//header[8]
					//header[9]

					// Step Size
					 SeqNumChannels =
						(int)(header[10] +
						((header[11] << 8) & 0xFF00) +
						((header[12] << 16) & 0xFF0000) +
						((header[13] << 24) & 0xFF000000));

					// Number of Steps
					SeqNumPeriods =
						(uint)(header[14] +
						((header[15] << 8) & 0xFF00) +
						((header[16] << 16) & 0xFF0000) +
						((header[17] << 24) & 0xFF000000));

					// Step time in ms
					_period = (ushort)(header[18] + ((header[19] << 8) & 0xFF00));

					// universe count
					_numUniverses = (ushort)(header[20] + ((header[21] << 8) & 0xFF00));

					// universe Size
					_universeSize = (ushort)(header[22] + ((header[23] << 8) & 0xFF00));

					//Gamma 
					_gamma = header[24];

					// Color Encoding
					_colorEncoding = header[25];

					//Pad
					//At present, header[26] and header[27] are always assumed to be 0
				}
				else
				{
					retVal = false;
				}

				if (retVal)
				{
					//_seqNumChannels should be the number of channels rounded up to the 
					//nearest value of 4.
				}
			}
			catch (Exception e)
			{
				_infs = null;
				_dataIn = null;
				throw e;
			}
		
			return retVal;
		}
	}
}
