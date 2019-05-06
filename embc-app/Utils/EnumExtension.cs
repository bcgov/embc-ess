using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            return GetAttribute<DescriptionAttribute>(enumValue).Description;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return GetAttribute<DisplayNameAttribute>(enumValue).DisplayName;
        }

        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
