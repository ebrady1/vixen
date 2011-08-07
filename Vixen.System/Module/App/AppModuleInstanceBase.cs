﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vixen.Sys;

namespace Vixen.Module.App {
	abstract public class AppModuleInstanceBase : ModuleInstanceBase, IAppModuleInstance, IEqualityComparer<IAppModuleInstance>, IEquatable<IAppModuleInstance>, IEqualityComparer<AppModuleInstanceBase>, IEquatable<AppModuleInstanceBase> {
		abstract public void Loading();

		abstract public void Unloading();

		abstract public IApplication Application { set; }

		public bool Equals(IAppModuleInstance x, IAppModuleInstance y) {
			return base.Equals(x, y);
		}

		public int GetHashCode(IAppModuleInstance obj) {
			return base.GetHashCode(obj);
		}

		public bool Equals(IAppModuleInstance other) {
			return base.Equals(other);
		}

		public bool Equals(AppModuleInstanceBase x, AppModuleInstanceBase y) {
			return Equals(x as IAppModuleInstance, y as IAppModuleInstance);
		}

		public int GetHashCode(AppModuleInstanceBase obj) {
			return GetHashCode(obj as IAppModuleInstance);
		}

		public bool Equals(AppModuleInstanceBase other) {
			return Equals(other as IAppModuleInstance);
		}
	}
}