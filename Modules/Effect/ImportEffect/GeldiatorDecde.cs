using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.ImportEffect
{
	class GlediatorDecode : IFileDecode 
	{
		private UInt32 _x;
		private UInt32 _y;

		public GlediatorDecode(UInt32 x, UInt32 y)
		{
			_x = x;
			_y = y;
			_periodDataStart = 0;
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

				// Step Size
				 SeqNumChannels = (UInt32)_x * _y * 3;

				// Number of Steps
				 SeqNumPeriods = (UInt32)(_dataIn.Length / SeqNumChannels);

				 retVal = true;
				
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
