using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelperLibrary
{
    public class StringFudger
    {
        public string removeSubString(string p_string, string p_substring)
        {
            int startIndex = p_string.IndexOf(p_substring);
            if (startIndex != -1)
            {
                // found it
                // remove it
                string name = p_string.Remove(startIndex, p_substring.Length);
                return name;
            }
            else
            {
                return p_string;
            }
        }
    }
}
