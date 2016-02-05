﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VixenModules.Effect.ImportEffect
{
	public class TimingConverter : TypeConverter
	{
		private const int SELECTIONS = 3;
		private String[]  m_selections;

		public TimingConverter()
		{
			m_selections = new String[SELECTIONS] ;
			m_selections[0] = "Effect Time";
			m_selections[1] = "Period Count";
			m_selections[2] = "Map to Sequence Time";
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			int retVal = 0;
			for (int x = 0; x < SELECTIONS; x++ )
			{
				if (m_selections[x].CompareTo(value) == 0)
				{
					retVal = x;
					break;
				}
			}
			return retVal;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
			Type destinationType)
		{
			String retVal = m_selections[0];
			int intVal = Convert.ToInt32(value);
			if ((intVal >= 0) && (intVal < SELECTIONS))
			{
				retVal = m_selections[intVal];
			}
			return retVal;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			StandardValuesCollection values = new StandardValuesCollection(m_selections);
			return values;
		}
	}
}
