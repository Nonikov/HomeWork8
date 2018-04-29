using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork8
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dic = new Dictionary<string , int> { ["a"] =1, ["aa"] = 2 };
            Console.WriteLine(dic["aa"]);

            Console.ReadKey();
        }
    }
}
