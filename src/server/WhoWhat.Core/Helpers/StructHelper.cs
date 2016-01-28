using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace WhoWhat.Core.Helpers
{
    public class StructHelper
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            List<DescriptionAttribute> attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false).ToList();

            return attributes.Count > 0 ? attributes[0].Description : enumValue.ToString();
        }
    }
}
