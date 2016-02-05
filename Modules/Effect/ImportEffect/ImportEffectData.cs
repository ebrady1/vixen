using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using Vixen.Module;
using VixenModules.App.ColorGradients;
using VixenModules.App.Curves;
using VixenModules.Effect.Pixel;
using ZedGraph;

namespace VixenModules.Effect.ImportEffect
{
	[DataContract]
	public class ImportEffectData: ModuleDataModelBase
	{
		public ImportEffectData()
		{
			FileName = String.Empty;
			Orientation=StringOrientation.Vertical;
			LevelCurve = new Curve(new PointPairList(new[] { 0.0, 100.0 }, new[] { 100.0, 100.0 }));
			Timing = 0;
			Repeat = 1;
			Scaled = true;
		}

		[DataMember]
		public int Repeat{ get; set; }
	
		[DataMember]
		public Int32 Timing { get; set; }
		
		[DataMember]
		public bool Scaled { get; set; }

		[DataMember]
		public string FileName { get; set; }

		[DataMember]
		public StringOrientation Orientation { get; set; }

		[DataMember]
		public Curve LevelCurve { get; set; }

		[DataMember]
		public UInt32 EffectStrings { get; set; }
		
		[DataMember]
		public UInt32 EffectPixelsPerStrings { get; set; }
		
		public override IModuleDataModel Clone()
		{
			ImportEffectData result = new ImportEffectData 
			{
				Scaled = Scaled,
				Timing = Timing,
				Orientation = Orientation,
				LevelCurve = new Curve(LevelCurve),
				FileName = FileName,
				Repeat = Repeat
			};
			return result;
		}
	}
}
