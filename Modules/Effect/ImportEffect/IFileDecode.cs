using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.ImportEffect
{
	public abstract class IFileDecode
	{
		protected MemoryStream	_dataIn = null;
		protected UInt32 _periodDataStart = UInt32.MaxValue;

		public IFileDecode()
		{
			SeqNumChannels = Int32.MaxValue;
			SeqNumPeriods = UInt32.MaxValue;
		}

		virtual public byte[] GetPeriodData(UInt32 period)
		{
			var buffer = new byte[SeqNumChannels];
			Array.Clear(buffer, 0, (int)SeqNumChannels);

			try
			{
				_dataIn.Position = _periodDataStart + (SeqNumChannels * period);
				_dataIn.Read(buffer, 0, (Int32)SeqNumChannels);
			}
			catch (Exception e) { } 
			return buffer;
		}

		public UInt32 SeqNumChannels
		{ get; set; }

		public UInt32 SeqNumPeriods
		{ get; set; }

		public abstract bool Load(String filePath);

	}
}
