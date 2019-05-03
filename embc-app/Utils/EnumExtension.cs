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
        public static string GetDescription(this Enum genericEnum)
        {
            //Type genericEnumType = genericEnum.GetType();
            //MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            //if ((memberInfo != null && memberInfo.Length > 0))
            //{
            //    var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            //    if (attributes != null && attributes.Any())
            //    {
            //        return ((System.ComponentModel.DescriptionAttribute)attributes.ElementAt(0)).Description;
            //    }
            //}
            //return genericEnum.ToString();
            return GetAttribute<DescriptionAttribute>(genericEnum).Description;
        }

        public static string GetDisplayName(this Enum genericEnum)
        {
            //Type genericEnumType = genericEnum.GetType();
            //MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            //if ((memberInfo != null && memberInfo.Length > 0))
            //{
            //    var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false);
            //    if (attributes != null && attributes.Any())
            //    {
            //        return ((System.ComponentModel.DisplayNameAttribute)attributes.ElementAt(0)).DisplayName;
            //    }
            //}
            //return genericEnum.ToString();
            return GetAttribute<DisplayNameAttribute>(genericEnum).DisplayName;
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
