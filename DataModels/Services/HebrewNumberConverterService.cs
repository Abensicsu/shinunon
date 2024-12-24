using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Services
{
    public class HebrewNumberConverter
    {
        static Dictionary<int, string> hebrewLetters = new Dictionary<int, string>
    {
        { 1, "א" }, { 2, "ב" }, { 3, "ג" }, { 4, "ד" }, { 5, "ה" },
        { 6, "ו" }, { 7, "ז" }, { 8, "ח" }, { 9, "ט" },
        { 10, "י" }, { 20, "כ" }, { 30, "ל" }, { 40, "מ" }, { 50, "נ" },
        { 60, "ס" }, { 70, "ע" }, { 80, "פ" }, { 90, "צ" },
        { 100, "ק" }, { 200, "ר" }, { 300, "ש" }, { 400, "ת" }
    };

        public static string ConvertToHebrew(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Number must be positive.");
            }

            // Special cases for 15 and 16
            if (number == 15) return "ט״ו";
            if (number == 16) return "ט״ז";

            string result = string.Empty;

            // Handle numbers over 400
            while (number >= 400)
            {
                result += hebrewLetters[400];
                number -= 400;
            }

            // Handle remaining numbers
            //foreach (var value in new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 })
            foreach (var value in hebrewLetters.Keys.OrderByDescending(k => k))
            {
                while (number >= value)
                {
                    if (value == 10 && (number == 15 || number == 16))
                    {
                        // Skip regular 15 and 16 to handle special cases
                        break;
                    }
                    result += hebrewLetters[value];
                    number -= value;
                }
            }

            return AddGeresh(result);
        }

        private static string AddGeresh(string hebrew)
        {
            if (hebrew.Length == 1)
            {
                return $"{hebrew}׳";
            }
            else if (hebrew.Length > 1)
            {
                return $"{hebrew.Substring(0, hebrew.Length - 1)}״{hebrew[^1]}";
            }
            return hebrew;
        }
    }
}
