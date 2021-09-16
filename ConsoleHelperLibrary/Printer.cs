using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelperLibrary
{
    public class Printer
    {
        public void PrintList<T>(List<T> p_list)
        {
            foreach (var item in p_list)
            {
                Console.WriteLine(item);
            }
        }
        public void PrintIndentedList<T>(List<T> p_list)
        {
            foreach (var item in p_list)
            {
                Console.WriteLine($"\t{item}");
            }
        }
    }
}
