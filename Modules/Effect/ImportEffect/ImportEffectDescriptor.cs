﻿using System;
using Vixen.Module.Effect;
using Vixen.Sys;
using Vixen.Sys.Attribute;

namespace VixenModules.Effect.ImportEffect
{
	public class ImportEffectDescriptor : EffectModuleDescriptorBase
	{
		private static readonly Guid _typeId = new Guid("7B00B6B5-6CA8-4709-B90A-BAD14DE119DA");
		
		public ImportEffectDescriptor()
		{
			ModulePath = EffectName;
		}
		
		[ModuleDataPath]
		public static string ModulePath { get; set; }

		public override ParameterSignature Parameters
		{
			get { return new ParameterSignature(); }
		}

		public override EffectGroups EffectGroup
		{
			get { return EffectGroups.Pixel; }
		}

		public override string TypeName
		{
			get { return EffectName; }
		}

		public override Guid TypeId
		{
			get { return _typeId; }
		}

		public override Type ModuleClass
		{
			get { return typeof(ImportEffect); }
		}

		public override Type ModuleDataClass
		{
			get { return typeof(ImportEffectData); }
		}

		public override string Author
		{
			get { return "Ed Brady"; }
		}

		public override string Description
		{
			get { return "Allows the user to import externally generated effects"; }
		}

		public override string Version
		{
			get { return "1.0"; }
		}

		public override string EffectName
		{
			get { return "Import"; }
		}
	}
}