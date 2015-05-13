﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common.Controls;
using Common.Controls.ColorManagement.ColorModels;
using Common.Controls.ColorManagement.ColorPicker;
using Vixen.Module.Effect;
using Vixen.Sys;
using VixenModules.Property.Color;

namespace System.Windows.Controls.WpfPropertyGrid.Controls
{
	public class ColorEditor : TypeEditor
	{
		public ColorEditor()
		{
			EditedType = KnownTypes.Wpf.Color;
			InlineTemplate = EditorKeys.ColorEditorKey;
			ExtendedTemplate = null;
		}

		public override void ShowDialog(PropertyItemValue propertyValue, IInputElement commandSource)
		{
			if (propertyValue == null) return;
			if (propertyValue.ParentProperty.IsReadOnly) return;
			
			HashSet<Color> discreteColors = GetDiscreteColors(propertyValue.ParentProperty.Component);

			Color colorValue=Color.Black;
			if (propertyValue.Value != null)
			{
				colorValue = (Color)propertyValue.Value;	
			}
			DialogResult result;
			if (discreteColors.Any())
			{
				using (DiscreteColorPicker dcp = new DiscreteColorPicker())
				{
					dcp.ValidColors = discreteColors;
					dcp.SingleColorOnly = true;
					dcp.SelectedColors = new List<Color> { colorValue };	
					result = dcp.ShowDialog();
					if (result == DialogResult.OK)
					{
						propertyValue.Value = !dcp.SelectedColors.Any() ? discreteColors.First() : dcp.SelectedColors.First();
					}

				}
			}
			else
			{
				using (ColorPicker cp = new ColorPicker())
				{
					cp.LockValue_V = true;
					cp.Color = XYZ.FromRGB(colorValue);
					result = cp.ShowDialog();
					if (result == DialogResult.OK)
					{
						propertyValue.Value = cp.Color.ToRGB().ToArgb();
					}
				}
			}


		}

		private HashSet<Color> GetDiscreteColors(Object component)
		{
			HashSet<Color> validColors = new HashSet<Color>();
			if (component is IEffect)
			{
				IEffect effect = (IEffect)component;
				validColors.AddRange(effect.TargetNodes.SelectMany(x => ColorModule.getValidColorsForElementNode(x, true)));
			}
			else if (component is Array)
			{
				foreach (var item in (Array)component)
				{
					if (item is IEffect)
					{
						IEffect effect = (IEffect)item;
						validColors.AddRange(effect.TargetNodes.SelectMany(x => ColorModule.getValidColorsForElementNode(x, true)));
					}
				}
			}

			return validColors;
		}
	}
}
