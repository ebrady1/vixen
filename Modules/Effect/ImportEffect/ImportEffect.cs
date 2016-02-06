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
		private const int SPEED_MIN = 0;
		private const int SPEED_MAX = 20;
		private const int SPEED_MID = SPEED_MAX / 2;

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
		[ProviderDisplayName(@"Effect Filename")]
		[ProviderDescription(@"Effect Filename")]
		[PropertyEditor("ImportEffectPathEditor")]
		[PropertyOrder(0)]
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
		public Int32 EffectStrings
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
		public Int32 EffectPixelsPerStrings
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
		[ProviderDisplayName(@"Speed/Repeat")]
		[ProviderDescription(@"Speed or Repeat Count of Effect")]
		[PropertyEditor("SliderEditor")]
		[NumberRange(SPEED_MIN,SPEED_MAX,1)]
		[PropertyOrder(1)]
		public int Speed 
		{
			get { return _data.Speed; }
			set
			{
				_data.Speed = value;
				IsDirty = true;
				OnPropertyChanged();
			}
		}

		[Value]
		[ProviderCategory(@"Config", 3)]
		[ProviderDisplayName(@"Scale")]
		[ProviderDescription(@"Scale")]
		[PropertyOrder(2)]
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
			if (!string.IsNullOrEmpty(FileName))
			{
				var filePath = Path.Combine(ImportEffectDescriptor.ModulePath, FileName);
				if (File.Exists(filePath))
				{
					var ext = Path.GetExtension(filePath);
					if (ext.CompareTo(".eseq") == 0)
					{
						_decode = new ESEQDecode();
					}
					else if (ext.CompareTo(".fseq") == 0)
					{
						_decode = new FSEQDecode();
					}
					_decode.Load(filePath);
				}
				else
				{
					_decode = null;
				}
			}
			else
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
			if (null != _decode)
			{
				UInt32 periodValue = 0;
				position = (GetEffectTimeIntervalPosition(frame) * 1) % 1;
				double speed = 0;

				if (Speed > 0)
				{
					speed = (Speed > SPEED_MID) ? (Speed - SPEED_MID) : (1.0 / Math.Abs(Speed - SPEED_MID - 1));
				}
				switch (Timing)
				{
					//Map to Effect Time
					case 0: 
					{
						periodValue = (UInt32)((position * _decode.SeqNumPeriods * speed) % _decode.SeqNumPeriods);
						break;
					}

					//Map to Period Count
					case 1:
					{
						periodValue = (UInt32)((frame * speed)% _decode.SeqNumPeriods);
						break;
					}

					//Map to Sequence Time
					case 2:
					{
						break;
					}
				}

				byte[] periodData = _decode.GetPeriodData(periodValue);
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

				for (double y = 0; y < BufferHt; y++ )
				{
					for (double x = 0; x < BufferWi; x++)
					{
						index = ((Math.Floor(y / BHIndex)) * (Math.Floor((double)BufferHt / BHIndex)) + 
							Math.Floor(x / BWIndex)) * 3.0;
						
						Color c = Color.FromArgb(periodData[(int)index], periodData[(int)index + 1], periodData[(int)index + 2]);
						var hsv = HSV.FromRGB(c);
						hsv.V = hsv.V * LevelCurve.GetValue(position * 100) / 100;
						frameBuffer.SetPixel((int)x, (int)y, hsv);
					}
				}

			}
		/*
			int x, y, n, colorIdx;
			int colorcnt = Colors.Count();
			int barCount = Repeat * colorcnt;
			double position = (GetEffectTimeIntervalPosition(frame) * Speed) % 1;
			if (barCount < 1) barCount = 1;


			if (Direction < ImportEffectDirection.Left || Direction == ImportEffectDirection.AlternateUp || Direction == ImportEffectDirection.AlternateDown)
			{
				int barHt = BufferHt / barCount+1;
				if (barHt < 1) barHt = 1;
				int halfHt = BufferHt / 2;
				int blockHt = colorcnt * barHt;
				if (blockHt < 1) blockHt = 1;
				int fOffset = (int) (position*blockHt*Repeat);// : Speed * frame / 4 % blockHt);
				if(Direction == ImportEffectDirection.AlternateUp || Direction == ImportEffectDirection.AlternateDown)
				{
					fOffset = (int)(Math.Floor(position*barCount)*barHt);
				}
				int indexAdjust = 1;
				
				for (y = 0; y < BufferHt; y++)
				{
					n = y + fOffset;
					colorIdx = ((n + indexAdjust) % blockHt) / barHt;
					//we need the integer division here to make things work
					double colorPosition = ((double)(n + indexAdjust) / barHt) - ((n + indexAdjust) / barHt);
					Color c = Colors[colorIdx].GetColorAt(colorPosition);
					var hsv = HSV.FromRGB(c);
					if (Highlight && (n + indexAdjust) % barHt == 0) hsv.S = 0.0f;
					if (Show3D) hsv.V *= (float)(barHt - (n + indexAdjust) % barHt - 1) / barHt;

					hsv.V = hsv.V * LevelCurve.GetValue(GetEffectTimeIntervalPosition(frame) * 100) / 100;

					switch (Direction)
					{
						case ImportEffectDirection.Down:
						case ImportEffectDirection.AlternateDown:
							// down
							for (x = 0; x < BufferWi; x++)
							{
								frameBuffer.SetPixel(x, y, hsv);
							}
							break;
						case ImportEffectDirection.Expand:
							// expand
							if (y <= halfHt)
							{
								for (x = 0; x < BufferWi; x++)
								{
									frameBuffer.SetPixel(x, y, hsv);
									frameBuffer.SetPixel(x, BufferHt - y - 1, hsv);
								}
							}
							break;
						case ImportEffectDirection.Compress:
							// compress
							if (y >= halfHt)
							{
								for (x = 0; x < BufferWi; x++)
								{
									frameBuffer.SetPixel(x, y, hsv);
									frameBuffer.SetPixel(x, BufferHt - y - 1, hsv);
								}
							}
							break;
						default:
							// up & AlternateUp
							for (x = 0; x < BufferWi; x++)
							{
								frameBuffer.SetPixel(x, BufferHt - y - 1, hsv);
							}
							break;
					}
				}
			}
			else
			{
				int barWi = BufferWi / barCount+1;
				if (barWi < 1) barWi = 1;
				int halfWi = BufferWi / 2;
				int blockWi = colorcnt * barWi;
				if (blockWi < 1) blockWi = 1;
				int fOffset = (int)(position * blockWi * Repeat);
				if (Direction > ImportEffectDirection.AlternateDown)
				{
					fOffset = (int)(Math.Floor(position * barCount) * barWi);
				} 
				
				for (x = 0; x < BufferWi; x++)
				{
					n = x + fOffset;
					colorIdx = ((n + 1) % blockWi) / barWi;
					//we need the integer division here to make things work
					double colorPosition = ((double)(n + 1) / barWi) - ((n + 1) / barWi);
					Color c = Colors[colorIdx].GetColorAt( colorPosition );
					var hsv = HSV.FromRGB(c);
					if (Highlight && n % barWi == 0) hsv.S = 0.0f;
					if (Show3D) hsv.V *= (float)(barWi - n % barWi - 1) / barWi;
					hsv.V = hsv.V * LevelCurve.GetValue(GetEffectTimeIntervalPosition(frame) * 100) / 100;
					switch (Direction)
					{
						case ImportEffectDirection.Right:
						case ImportEffectDirection.AlternateRight:
							// right
							for (y = 0; y < BufferHt; y++)
							{
								frameBuffer.SetPixel(BufferWi - x - 1, y, hsv);
							}
							break;
						case ImportEffectDirection.HExpand:
							// H-expand
							if (x <= halfWi)
							{
								for (y = 0; y < BufferHt; y++)
								{
									frameBuffer.SetPixel(x, y, hsv);
									frameBuffer.SetPixel(BufferWi - x - 1, y, hsv);
								}
							}
							break;
						case ImportEffectDirection.HCompress:
							// H-compress
							if (x >= halfWi)
							{
								for (y = 0; y < BufferHt; y++)
								{
									frameBuffer.SetPixel(x, y, hsv);
									frameBuffer.SetPixel(BufferWi - x - 1, y, hsv);
								}
							}
							break;
						default:
							// left & AlternateLeft
							for (y = 0; y < BufferHt; y++)
							{
								frameBuffer.SetPixel(x, y, hsv);
							}
							break;
					}
				}
			}
		*/
		}
	}
}
