using System;

using Vixen.Module.Analysis;

namespace VixenModules.Analysis.NoteOnsets
{
	public class NoteOnsetsDescriptor : AnalysisModuleDescriptorBase
	{
		private Guid _typeId = new Guid("{A2BECEF8-315C-4401-A5FC-06AF4DB4D6A6}");

		public override string TypeName
		{
			get { return "Note Onsets"; }
		}

		public override Guid TypeId
		{
			get { return _typeId; }
		}

		public override string Author
		{
			get { return "Ed Brady"; }
		}

		public override string Description
		{
			get { return "Automatic Note Onset Detection"; }
		}

		public override string Version
		{
			get { return "1.0"; }
		}

		public override Type ModuleClass
		{
			get { return typeof(NoteOnsetsModule); }
		}

	}
}