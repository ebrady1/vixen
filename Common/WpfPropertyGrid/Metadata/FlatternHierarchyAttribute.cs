﻿/*
 * Copyright © 2010, Denys Vuika
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace System.Windows.Controls.WpfPropertyGrid
{
  /// <summary>
  /// Specifies whether the property hierarchy should be flatterned for visualization.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  [Obsolete("This attribute is deprecated and is no longer supported by PropertyEditorGrid. Please consider using custom type descriptors or ModelView classes in order to gain flattern behavior.", false)]
  public sealed class FlatternHierarchyAttribute : Attribute { }
}
