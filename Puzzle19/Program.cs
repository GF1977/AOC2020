using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle19
{
    class Program
    {
        // data_1.txt 239 & 405
        static StreamReader file = new StreamReader(@"C:\Users\iopya\Source\Repos\AOC2020\Puzzle19\data.txt");
        static List<string> Examples = new List<string>();
        static Dictionary<int, string> Rules = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            string line = file.ReadLine();

            // Parsing the rules
            while (line != "")
            {
                string[] tmp = line.Split(": ");
                int ruleN = int.Parse(tmp[0]);
                Rules.Add(ruleN, tmp[1].Replace("\"", ""));
                line = file.ReadLine();
            }

            // Parsing examples
            while (line != null)
            {
                Examples.Add(line);
                line = file.ReadLine();
            }
            
            SolveThePuzzle(1);
            SolveThePuzzle(2);

        }
        static void SolveThePuzzle(int part=1)
        {
            if (part == 2)
            {
                // adding 5 levels of loops into 8th and 11th rules
                Rules.Remove(8);
                Rules.Add(8, "42 | 42 ( 42 | 42 ( 42 | 42 ( 42 | 42 ( 42 | 42 42 ) ) ) )");

                Rules.Remove(11);
                Rules.Add(11, "42 31 | 42 ( 42 31 | 42 ( 42 31 | 42 ( 42 31 | 42 ( 42 31 | 42 ( 42 31 | 42 42 31 31 ) 31 ) 31 ) 31 ) 31 ) 31");
            }

            Console.WriteLine("Part {0}", part);
            int res = GetCases();
            Console.WriteLine(res);
            Console.WriteLine("");
        }

        // Return the number of the cases which match with the rule
        static int GetCases()
        {
            string simpleRule;
            Rules.TryGetValue(0, out simpleRule);
            simpleRule = " " + simpleRule + " ";

            bool bRuleIsComplex = true;
            while (bRuleIsComplex)
                bRuleIsComplex = SimplifyRules(simpleRule, out simpleRule);

            int matchedCount = 0;
            simpleRule = "^" + simpleRule.Replace(" ", "") + "$";
            Regex rg = new Regex(simpleRule);

            foreach (string Example in Examples)
            {
                if (rg.IsMatch(Example))
                    matchedCount++;
            }
            return matchedCount;
        }

        //convert the list of rules into the single line of Regular Expression
        static bool SimplifyRules(string simpleRule, out string result)
        {
            string tempRule = null;
            string[] firstRule = simpleRule.Split(" ");

            foreach (string r in firstRule)
            {
                int ruleNumber;
                if(int.TryParse(r, out ruleNumber))
                {
                    Rules.TryGetValue(ruleNumber, out tempRule);
                    tempRule = " " + tempRule + " ";
                    
                    if (tempRule != "a" && tempRule != "b")
                        tempRule = " (" + tempRule + ") ";

                    simpleRule = simpleRule.Replace(" " + r + " ", tempRule);
                }
            }
            result = simpleRule;

            // tempRule == null when all the rules are converted to "a" or "b" and there is nothing to do
            if (tempRule == null)
                return false;
            else
                return true;
        }
    }
}