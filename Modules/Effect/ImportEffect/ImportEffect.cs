using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Common.Controls.ColorManagement.ColorModels;
using Vixen.Attributes;
using Vixen.Module;
using Vixen.Sys.Attribute;
using VixenModules.App.ColorGradients;
using VixenModules.App.Curves;
using VixenModules.Effect.Pixel;
using VixenModules.EffectEditor.EffectDescriptorAttributes;

namespace VixenModules.Effect.ImportEffect
{
	public class ImportEffect:PixelEffectBase
	{
		private ImportEffectData _data;
		private IFileDecode _decode = null;
		
		public ImportEffect()
		{
			_data = new ImportEffectData();
		}

		public override bool IsDirty
		{
			get
			{
				return base.IsDirty;
			}
			protected set { base.IsDirty = value; }
		}
		
        [Value]
		[ProviderCategory(@"Effect Info", 2)]
		[DisplayName(@"Effect Type")]
		[Description(@"Effect Type")]
		[PropertyEditor("SelectionEditor")]
		[TypeConverter(typeof(EffectTypeConverter))]
		[PropertyOrder(0)]
		public Int32 EffectType 
		{
			get { return _data.EffectType; }
			set
			{
				_data.EffectType = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		
		[Value]
		[ProviderCategory(@"Effect Info", 2)]
		[ProviderDisplayName(@"Effect Filename")]
		[ProviderDescription(@"Effect Filename")]
		[PropertyEditor("ImportEffectPathEditor")]
		[PropertyOrder(1)]
		public String FileName
		{
			get { return _data.FileName; }
			set
			{
				_data.FileName = ConvertPath(value);
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		
		[Value]
		[ProviderCategory(@"Effect Info", 2)]
		[ProviderDisplayName(@"Effect Strings")]
		[ProviderDescription(@"Number of Strings in the Effect")]
		[NumberRange(0, 10000, 1, 0)]
		[PropertyOrder(2)]
		public UInt32 EffectStrings
		{
			get 
			{ 
				return _data.EffectStrings;
			}

			set
			{
				_data.EffectStrings = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		[Value]
		[ProviderCategory(@"Effect Info", 2)]
		[ProviderDisplayName(@"Effect Pixels Per String")]
		[ProviderDescription(@"Number of Pixels in each Effect String")]
		[NumberRange(0, 10000, 1, 0)]
		[PropertyOrder(3)]
		public UInt32 EffectPixelsPerStrings
		{
			get
			{
				return _data.EffectPixelsPerStrings;
			}

			set
			{
				_data.EffectPixelsPerStrings = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		#region Setup

		[Value]
		public override StringOrientation StringOrientation
		{
			get { return _data.Orientation; }
			set
			{
				_data.Orientation = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		#endregion
		
		#region Level properties

        [Value]
		[ProviderCategory(@"Config", 3)]
		[DisplayName(@"Timing")]
		[Description(@"Timing presenting data in the imported effect.")]
		[PropertyEditor("SelectionEditor")]
		[TypeConverter(typeof(TimingConverter))]
		[PropertyOrder(0)]
		public Int32 Timing
		{
			get { return _data.Timing; }
			set
			{
				_data.Timing = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		[Value]
		[ProviderCategory(@"Config", 1)]
		[ProviderDisplayName(@"Period Offset")]
		[ProviderDescription(@"# of periods to offset the Effect")]
		[PropertyEditor("SliderEditor")]
		[NumberRange(0,100,1)]
		[PropertyOrder(1)]
		public Int32 PeriodOffset 
		{
			get { return _data.PeriodOffset; }
			set
			{
				_data.PeriodOffset= value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		[Value]
		[ProviderCategory(@"Config", 1)]
		[ProviderDisplayName(@"Repeat")]
		[ProviderDescription(@"Repeat (Applicable to Effect Time setting only")]
		[PropertyEditor("SliderEditor")]
		[NumberRange(0,10,1)]
		[PropertyOrder(2)]
		public Int32 Repeat 
		{
			get { return _data.Repeat; }
			set
			{
				_data.Repeat = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		
		[Value]
		[ProviderCategory(@"Config", 3)]
		[ProviderDisplayName(@"Scale")]
		[ProviderDescription(@"Scale")]
		[PropertyOrder(3)]
		public bool Scale 
		{
			get { return _data.Scaled; } 
			
			set
			{
				_data.Scaled = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		
		[Value]
		[ProviderCategory(@"Config", 3)]
		[ProviderDisplayName(@"Flip Horizontal")]
		[ProviderDescription(@"Flip Horizontal")]
		[PropertyOrder(4)]
		public bool FlipHorizontal 
		{
			get { return _data.FlipHorizontal; } 
			
			set
			{
				_data.FlipHorizontal = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		[Value]
		[ProviderCategory(@"Config", 3)]
		[ProviderDisplayName(@"Flip Vertical")]
		[ProviderDescription(@"Flip Vertical")]
		[PropertyOrder(5)]
		public bool FlipVertical 
		{
			get { return _data.FlipVertical; } 
			
			set
			{
				_data.FlipVertical = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		
		[Value]
		[ProviderCategory(@"Config", 3)]
		[ProviderDisplayName(@"Reverse")]
		[ProviderDescription(@"Play the effect in Reverse")]
		[PropertyOrder(6)]
		public bool Reverse 
		{
			get { return _data.Reverse; } 
			
			set
			{
				_data.Reverse = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}
		#endregion

		#region Level properties

		[Value]
		[ProviderCategory(@"Brightness", 4)]
		[ProviderDisplayName(@"Brightness")]
		[ProviderDescription(@"Brightness")]
		public Curve LevelCurve
		{
			get { return _data.LevelCurve; }
			set
			{
				_data.LevelCurve = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		#endregion

		public override IModuleDataModel ModuleData
		{
			get { return _data; }
			set
			{
				_data = value as ImportEffectData;
				IsDirty = true;
			}
		}
		
		private string ConvertPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			if (Path.IsPathRooted(path))
			{
				return CopyLocal(path);
			}
			
			return path;
		}

		private string CopyLocal(string path)
		{
			string name = Path.GetFileName(path);
			var destPath = Path.Combine(ImportEffectDescriptor.ModulePath, name);
			File.Copy(path, destPath, true);
			return name;
		}

		protected override void SetupRender()
		{
			bool result = false;
			EffectStrings = (EffectStrings == 0) ? (UInt32)StringCount : EffectStrings;
			EffectPixelsPerStrings = (EffectPixelsPerStrings == 0) ? (UInt32)MaxPixelsPerString : EffectPixelsPerStrings;

			if (!string.IsNullOrEmpty(FileName))
			{
				var filePath = Path.Combine(ImportEffectDescriptor.ModulePath, FileName);
				if (File.Exists(filePath))
				{
					switch (EffectType)
					{
						//FPP FSeq File Format
						case 0:
						{
							_decode = new FSEQDecode();
							break;
						}
					
						//FPP ESeq File Format
						case 1:
						{
							_decode = new ESEQDecode();
							break;
						}
					
						//Glediator File Format
						case 2:
						{
							_decode = new GlediatorDecode(EffectStrings, EffectPixelsPerStrings);
							break;
						}
						
						//xLights File Format
						case 3:
						{
							_decode = new xLightsDecode();
							break;
						}

						default:
						{
							break;
						}
					}
					if (null != _decode)
					{
						result = _decode.Load(filePath);
					}
				}
			}
			if (!result)
			{
				_decode = null;
			}
		}

		protected override void CleanUpRender()
		{
			//Nothing to clean up
		}

		
		protected override void RenderEffect(int frame, ref PixelFrameBuffer frameBuffer)
		{
			double position = 0;
			double iposition = 0;

			if (null != _decode)
			{
				UInt32 periodValue = 0;

				position = (GetEffectTimeIntervalPosition(frame) * Repeat) % 1;
				iposition = (GetEffectTimeIntervalPosition(frame)) % 1;
				
				switch (Timing)
				{
					//Map to Effect Time
					case 0: 
					{
						periodValue = (UInt32)(((position * _decode.SeqNumPeriods) + PeriodOffset) % _decode.SeqNumPeriods);
						break;
					}

					//Map to Period Count
					case 1:
					{
						periodValue = (UInt32) (PeriodOffset + frame);

						break;
					}

					//Map to Sequence Time
					case 2:
					{
						UInt32 tmpPeriod = (UInt32)(StartTime.TotalMilliseconds / (double)FrameTime) + (UInt32)frame;
						periodValue = (tmpPeriod < _decode.SeqNumPeriods) ? tmpPeriod : UInt32.MaxValue;
						break;
					}
				}

				periodValue = (_data.Reverse) ? _decode.SeqNumPeriods - periodValue - 1: periodValue;

				byte[] periodData = _decode.GetPeriodData(periodValue);
				if (periodData == null)
				{
				//	return;
				}

				double BWIndex = 1;
				double BHIndex = 1;
				double index = 0;

				if (Scale)
				{
					if (EffectStrings != 0)
					{
						BWIndex = (double)BufferWi / (double)EffectStrings; 
					}
					
					if (EffectPixelsPerStrings != 0)
					{
						BHIndex = (double)BufferHt / (double)EffectPixelsPerStrings;
					}
				}

				double indexXStart = 0;
				double indexYStart = 0;
				double indexXInc = 1;
				double indexYInc = 1;

				if (FlipVertical)
				{
					indexYStart = BufferHt - 1;
					indexYInc = -1;
				}

				if (FlipHorizontal)
				{
					indexXStart = BufferWi - 1;
					indexXInc = -1;
				}


				for (double indexY = indexYStart, y = 0; y < BufferHt; y++, indexY += indexYInc )
				{
					for (double indexX = indexXStart, x = 0; x < BufferWi; x++, indexX += indexXInc )
					{
						index = ((Math.Floor(indexY / BHIndex)) * (Math.Floor((double)BufferHt / BHIndex)) + 
							Math.Floor(indexX / BWIndex)) * 3.0;
						
						Color c = Color.FromArgb(periodData[(int)index], periodData[(int)index + 1], periodData[(int)index + 2]);
						var hsv = HSV.FromRGB(c);
						hsv.V = hsv.V * LevelCurve.GetValue(iposition * 100) / 100;
						frameBuffer.SetPixel((int)x, (int)y, hsv);
					}
				}

			}
		}
	}
}
