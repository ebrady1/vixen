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
		}

		[DataMember]
		public bool FitToTime { get; set; }

		[DataMember]
		public string FileName { get; set; }

		[DataMember]
		public StringOrientation Orientation { get; set; }

		[DataMember]
		public Curve LevelCurve { get; set; }

		public override IModuleDataModel Clone()
		{
			ImportEffectData result = new ImportEffectData 
			{
				FitToTime = FitToTime,
				Orientation = Orientation,
				LevelCurve = new Curve(LevelCurve),
				FileName = FileName
			};
			return result;
		}
	}
}
