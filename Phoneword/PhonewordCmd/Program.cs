using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonewordCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a phoneword:");
            var input = Console.ReadLine();

            //TODO: Translate the word into numbers
            var output = Core.PhonewordTranslator.ToNumber(input);

            Console.WriteLine(output);
            Console.Read();
        }
    }
}
