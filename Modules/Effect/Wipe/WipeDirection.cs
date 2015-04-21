﻿using System.ComponentModel;
using VixenModules.EffectEditor.TypeConverters;

namespace VixenModules.Effect.Wipe {
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum WipeDirection {
		[Description("Wipe Up")]
		Up = 0,
		[Description("Wipe Down")]
		Down = 1,
		[Description("Wipe Right")]
		Right = 2,
		[Description("Wipe Left")]
		Left = 3,
		[Description("Burst In")]
		In = 4,
		[Description("Burst Out")]
		Out = 5
	}
}
