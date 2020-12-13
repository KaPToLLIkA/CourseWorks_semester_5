using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    class MainClass
    {
        static void Test(F f)
        {
            Console.WriteLine(f(0, 2));
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(sizeof(char));
            Test(Func);
        }

        delegate double F(double a, double b);

        static double Func(double a, double b)
        {
            return a + b;
        }
    }
}
