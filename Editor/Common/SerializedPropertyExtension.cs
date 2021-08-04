/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies and Contributors.
All rights reserved.
*/

using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace Voxell.Inspector
{
  public static class SerializedPropertyExtension
  {
    public static T GetSerializedValue<T>(this SerializedProperty property)
    {
      object @object = property.serializedObject.targetObject;
      string[] propertyNames = property.propertyPath.Split('.');

      // Clear the property path from "Array" and "data[i]".
      if (propertyNames.Length >= 3 && propertyNames[propertyNames.Length - 2] == "Array")
        propertyNames = propertyNames.Take(propertyNames.Length - 2).ToArray();

      // Get the last object of the property path.
      foreach (string path in propertyNames)
      {
        @object = @object.GetType()
          .GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
          .GetValue(@object);
      }

      if (@object.GetType().GetInterfaces().Contains(typeof(IList<T>)))
      {
        int propertyIndex = int.Parse(property.propertyPath[property.propertyPath.Length - 2].ToString());

        return ((IList<T>) @object)[propertyIndex];
      }
      else return (T) @object;
    }
  }
}