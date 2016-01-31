using System;
using System.Windows;
using Microsoft.Win32;

namespace VixenModules.Editor.EffectEditor.Editors
{
	public class  CustomEffectPathEditor : PropertyEditor
	{
		public CustomEffectPathEditor()
		{
			InlineTemplate = EditorKeys.FilePathEditorKey;
		}

		public override Object ShowDialog(PropertyItem propertyItem, Object propertyValue, IInputElement commandSource)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "Effect Files (*.eseq, *vir)|*.eseq;*.vir;",
				Multiselect = false
			};

			ofd.Title = propertyItem.DisplayName;

			if (ofd.ShowDialog() == true)
			{
				propertyValue = ofd.FileName;
			}

			return propertyValue;
		}
	}
}
