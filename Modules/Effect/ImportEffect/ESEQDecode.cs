using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.ImportEffect
{
	class ESEQDecode : IFileDecode
	{
		private const UInt16 _fixedHeaderLength = 20;

		public ESEQDecode()
		{

		}

		public byte[] GetPeriodData(UInt32 period)
		{
			_dataIn.Position = _periodDataStart + (SeqNumChannels * period);
			var buffer = new byte[SeqNumChannels];
			_dataIn.Read(buffer, 0, SeqNumChannels);
			return buffer;
		}

		public UInt32 ModelSize
		{ get; set; }
		
		public override bool Load(String filePath)
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

				if ((header[0] == 'E') && (header[1] == 'S') && (header[2] == 'E') && (header[3] == 'Q'))
				{
					//Beginning of Period Data
					_periodDataStart = _fixedHeaderLength; 

					// Step Size
					 SeqNumChannels =
						(int)(header[8] +
						((header[9] << 8) & 0xFF00) +
						((header[10] << 16) & 0xFF0000) +
						((header[11] << 24) & 0xFF000000));

					// Size of the Model
					ModelSize =
						(uint)(header[16] +
						((header[17] << 8) & 0xFF00) +
						((header[18] << 16) & 0xFF0000) +
						((header[19] << 24) & 0xFF000000));

					SeqNumPeriods = (UInt32)(_dataIn.Length - _fixedHeaderLength) / ModelSize;

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
				_dataIn = null;
				throw e;
			}
		
			return retVal;
		}
	}
}
