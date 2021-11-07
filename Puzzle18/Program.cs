using System;
using System.Collections.Generic;
using System.IO;


namespace Puzzle18
{
    class Program
    {
        static void Main(string[] args)
        {
            //string filePath = @"C:\Users\iopya\source\repos\AOC2020\Puzzle18\data_test.txt";
            string filePath = @"C:\Users\iopya\source\repos\AOC2020\Puzzle18\data_p1.txt";
            StreamReader file = new StreamReader(filePath);

            List<string> tests = new List<string>();

            do
            {
                string line = file.ReadLine();
                tests.Add(line);
            }
            while (!file.EndOfStream);

            Int64 result = 0;
            foreach (string test in tests)
            {
                Int64 res = getSubTest(test);
                Console.WriteLine("{0} = {1}",test, res);
                result += res;
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine(result);
        }

        static Int64 getSubTest(string test)
        {
            Int64 res = 0;

            int leftP = test.LastIndexOf("(");

            if (leftP < 0)
            {
                res = calculate(test);
                return res;
            }

            int rightP = test.IndexOf(")",leftP);

            string subTest = test.Substring(leftP + 1, rightP - leftP - 1);
            
            if (subTest == "")
                subTest = test;
            
            res = calculate(subTest);
            string toReplace = test.Substring(leftP, rightP - leftP + 1);
            string minimizedTest = test.Replace(toReplace, res.ToString());

            res = getSubTest(minimizedTest);
            
            return res;
        }

        static Int64 calculate(string subtest)
        {
            Int64 res = 1;
            string[] elements = subtest.Split(" ");
            string previousOperator = "*";

            foreach (string element in elements)
            {
                if (element != "*" && element != "+")
                {
                    Int64 x = Int64.Parse(element);
                    if (previousOperator == "*")
                        res *= x;
                    else
                        res += x;
                }
                else
                    previousOperator = element;

            }
            return res;
        }
    }
}
