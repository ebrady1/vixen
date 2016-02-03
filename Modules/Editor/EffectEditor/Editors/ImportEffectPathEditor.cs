using System;
using System.Windows;
using Microsoft.Win32;

namespace VixenModules.Editor.EffectEditor.Editors
{
	public class  ImportEffectPathEditor : PropertyEditor
	{
		public ImportEffectPathEditor()
		{
			InlineTemplate = EditorKeys.FilePathEditorKey;
		}

		public override Object ShowDialog(PropertyItem propertyItem, Object propertyValue, IInputElement commandSource)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "Effect Files (*.fseq, *.eseq, *vir)|*.fseq;*.eseq;*.vir;",
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
