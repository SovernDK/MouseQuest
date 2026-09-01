using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Atlas.Utility 
{
    public static class Util
    {
        public static bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        public static Vector3 ToNearestIntMultiple(Vector3 position, int intMultiple)
        {
            var x = ToNearestIntMultiple(position.x, intMultiple);
            var z = ToNearestIntMultiple(position.z, intMultiple);
            return new Vector3(x, position.y, z);
        }

        /// <summary>
        /// Rounds a float to the nearest integer multiple.
        /// Example: 0.5 with integer multiple 5 ==> 0.
        /// </summary>
        public static float ToNearestIntMultiple(float f, int intMultiple)
        {
            return Mathf.Round(f / (float)intMultiple) * intMultiple;
        }

        public static string RemoveNumberFromDuplicatedName(string objectName)
        {
            // This pattern matches a space followed by a number inside parentheses at the end of the name
            string pattern = @" \(\d+\)$";

            // Remove the pattern from the object name
            return Regex.Replace(objectName, pattern, "");
        }

        /// <summary>
        /// Removes "(Clone)" from the given object's name.
        /// </summary>
        /// <param name="obj">The GameObject to process.</param>
        public static string RemoveCloneTag(string name)
        {
            const string cloneTag = "(Clone)";
            
            if (name.EndsWith(cloneTag))
            {
                return name.Substring(0, name.Length - cloneTag.Length).Trim();
            }

            return name;
        }

        public static string ToItemSnakeCase(string input, string prefix = "", string suffix ="")
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input string cannot be null or whitespace.", nameof(input));
                
            input = input.Replace(" ", "");
            StringBuilder result = new StringBuilder(prefix);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (char.IsUpper(c))
                {
                    if (i > 0) // Add underscore before uppercase letters, except the first one
                        result.Append('_');

                    result.Append(char.ToLower(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            result.Append(suffix);

            return result.ToString();
        }

        public static List<T> RandomlySortList<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            return list.OrderBy(x => rng.Next()).ToList();
        }

        public static (string, string) GetModifiersLocalization(string modifierKey)
        {
            int underscoreIndex = modifierKey.IndexOf('_');
        
            if (underscoreIndex == -1)
            {
                return (modifierKey, modifierKey);
            }
            
            return (modifierKey.Substring(0, underscoreIndex).Trim(), modifierKey.Substring(underscoreIndex + 1).Trim());
        }
    }
}