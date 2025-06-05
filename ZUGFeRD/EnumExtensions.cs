/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace s2industries.ZUGFeRD
{
    internal static class EnumExtensions
    {
        internal static string EnumToString<T>(this T value) where T : Enum
        {
            if (typeof(T) == typeof(Profile))
            {
                throw new InvalidOperationException("This method is not allowed for Profile enum. Please use Profile extension functions instead.");
            }

            // eventually use attribute value
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                // Prüft, ob das EnumStringValueAttribute gesetzt ist
                var attribute = field.GetCustomAttribute<EnumStringValueAttribute>();
                if (attribute != null)
                {
                    return attribute.Value;
                }
            }

            return value.ToString("g");
        } // !EnumToString()


        internal static string EnumToString<T>(this T? value) where T : struct, Enum
        {
            if (typeof(T) == typeof(Profile))
            {
                throw new InvalidOperationException("This method is not allowed for Profile enum. Please use Profile extension functions instead.");
            }

            if (!value.HasValue)
            {
                return String.Empty;
            }

            return EnumToString(value.Value);
        } // !EnumToString()


        internal static T IntToEnum<T>(this int value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            else
            {
                return default;
            }
        } // !IntToEnum()

        internal static T StringToEnum<T>(this string value) where T : struct, Enum
        {
            T? result = StringToNullableEnum<T>(value);
            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return default;
            }
        } // !StringToEnum()


        internal static T? StringToNullableEnum<T>(this string value) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // find out if we have custom mapping
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = field.GetCustomAttribute<EnumStringValueAttribute>();
                if (attribute != null)
                {
                    if (attribute.Value.Equals(value, StringComparison.OrdinalIgnoreCase) ||
                        attribute.LegacyValues.Any(v => v.Equals(value, StringComparison.OrdinalIgnoreCase)))
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            // use default behavior if we have no custom mapping
            if (Enum.TryParse(value, true, out T result))
            {
                return result;
            }
            else
            {
                return null;
            }
        } // !StringToNullableEnum()


        internal static int EnumToInt<T>(this T value) where T : Enum
        {
            return (int)(object)value;
        } // !EnumToInt()


/*
        internal static string GetDescriptionAttribute<T>(this T value) where T : Enum
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return null;
            }
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description;
        } // !GetDescriptionAttribute()


        internal static T FromDescription<T>(string code) where T : Enum
        {
            if (string.IsNullOrEmpty(code))
            {
                return default;
            }
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var description = value.GetDescriptionAttribute();
                if (description != null && description.Equals(code, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }
            return default;
        } // !FromDescription()
        */


        internal static bool In<T>(this T? input, params T[] allowedValues) where T : struct, Enum
        {
            if (input == null)
            {
                return false;
            }

            return allowedValues.Contains(input.Value);
        } // !In()
    }
}
