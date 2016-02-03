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

namespace VixenModules.Effect.CustomEffect
{
	public class CustomEffect:PixelEffectBase
	{
		private CustomEffectData _data;
		private FPPDecode _decode = null;
		
		public CustomEffect()
		{
			_data = new CustomEffectData();
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
		[ProviderCategory(@"Config", 2)]
		[ProviderDisplayName(@"Filename")]
		[ProviderDescription(@"Filename")]
		[PropertyEditor("CustomEffectPathEditor")]
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
		[ProviderCategory(@"Brightness", 3)]
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
				_data = value as CustomEffectData;
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
			var destPath = Path.Combine(CustomEffectDescriptor.ModulePath, name);
			File.Copy(path, destPath, true);
			return name;
		}

		protected override void SetupRender()
		{
			if (!string.IsNullOrEmpty(FileName))
			{
				var filePath = Path.Combine(CustomEffectDescriptor.ModulePath, FileName);
				if (File.Exists(filePath))
				{
					_decode = new FPPDecode();
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
				position = (GetEffectTimeIntervalPosition(frame) * 1) % 1;
				byte[] periodData = _decode.GetPeriodData((UInt32)(position * _decode.SeqNumPeriods));

				int index = 0;
				for (int y = 0; y < BufferHt; y++)
				{
					for (int x = 0; x < BufferWi; x++)
					{
						index = ((y * BufferWi) + x) * 3;
						Color c = Color.FromArgb(periodData[index], periodData[index + 1], periodData[index + 2]);
						var hsv = HSV.FromRGB(c);
						hsv.V = hsv.V * LevelCurve.GetValue(position * 100) / 100;
						frameBuffer.SetPixel(x, y, hsv);
					}
				}

			}
		/*
			int x, y, n, colorIdx;
			int colorcnt = Colors.Count();
			int barCount = Repeat * colorcnt;
			double position = (GetEffectTimeIntervalPosition(frame) * Speed) % 1;
			if (barCount < 1) barCount = 1;


			if (Direction < CustomEffectDirection.Left || Direction == CustomEffectDirection.AlternateUp || Direction == CustomEffectDirection.AlternateDown)
			{
				int barHt = BufferHt / barCount+1;
				if (barHt < 1) barHt = 1;
				int halfHt = BufferHt / 2;
				int blockHt = colorcnt * barHt;
				if (blockHt < 1) blockHt = 1;
				int fOffset = (int) (position*blockHt*Repeat);// : Speed * frame / 4 % blockHt);
				if(Direction == CustomEffectDirection.AlternateUp || Direction == CustomEffectDirection.AlternateDown)
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
						case CustomEffectDirection.Down:
						case CustomEffectDirection.AlternateDown:
							// down
							for (x = 0; x < BufferWi; x++)
							{
								frameBuffer.SetPixel(x, y, hsv);
							}
							break;
						case CustomEffectDirection.Expand:
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
						case CustomEffectDirection.Compress:
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
				if (Direction > CustomEffectDirection.AlternateDown)
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
						case CustomEffectDirection.Right:
						case CustomEffectDirection.AlternateRight:
							// right
							for (y = 0; y < BufferHt; y++)
							{
								frameBuffer.SetPixel(BufferWi - x - 1, y, hsv);
							}
							break;
						case CustomEffectDirection.HExpand:
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
						case CustomEffectDirection.HCompress:
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
