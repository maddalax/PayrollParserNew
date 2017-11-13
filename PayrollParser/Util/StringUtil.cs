using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollParser.Util
{
    public class StringUtil
    {
        public static string ReplaceFirst(string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
                return text;           
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
        public static IEnumerable<int> FindIndexes(string text, string query)
        {
            return Enumerable.Range(0, text.Length - query.Length)
                .Where(i => query.Equals(text.Substring(i, query.Length)));
        }
    }
}