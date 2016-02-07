using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.ImportEffect
{
	class  xLightsDecode : IFileDecode 
	{
		private UInt16 _period = 50;
		private const UInt16 _fixedHeaderLength = 512;

		public xLightsDecode()
		{
			SeqNumChannels = 0;
			SeqNumPeriods = 0;
			_periodDataStart = _fixedHeaderLength;
		}
		
		public override byte[] GetPeriodData(UInt32 period)
		{
			if ((period == UInt32.MaxValue) || (period >= SeqNumPeriods))
			{
				return null;
			}
			
			byte[] buffer = new byte[SeqNumChannels];
			
			for (int x = 0; x < SeqNumChannels; x++) 
			{
				_dataIn.Position = _periodDataStart + (period * SeqNumPeriods) + x;
				_dataIn.Read(buffer, x, 1);
			}
			return buffer;
		}

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

				string magicID = System.Text.Encoding.UTF8.GetString(header, 0, 7);
				string xLightsVerStr = System.Text.Encoding.UTF8.GetString(header, 7, 2);
				string numChStr = System.Text.Encoding.UTF8.GetString(header, 9, 8);
				string numPerStr = System.Text.Encoding.UTF8.GetString(header, 17, 8);

				if ((magicID.CompareTo("xLights") == 0) &&
					(xLightsVerStr.CompareTo("01") == 0))
				{
					SeqNumChannels = Convert.ToUInt32(numChStr,10);
					SeqNumPeriods = Convert.ToUInt32(numPerStr,10);
				}

				else
				{
					retVal = false;
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
