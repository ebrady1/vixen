using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.CustomEffect
{
	public abstract class IFileDecode
	{
		protected MemoryStream	_dataIn = null;
		protected UInt32 _periodDataStart = UInt32.MaxValue;

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

		public abstract bool Load(String filePath);

	}
}
