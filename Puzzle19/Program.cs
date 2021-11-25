using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle19
{
    class Program
    {
        // data_1.txt 239 & 405
        static StreamReader file = new StreamReader(@"C:\Users\iopya\Source\Repos\AOC2020\Puzzle19\data_1.txt");
        static List<string> Examples = new List<string>();
        static Dictionary<int, string> Rules = new Dictionary<int, string>();
        static int[] matchedCase;
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
                if (line != "")
                    Examples.Add(line);
                line = file.ReadLine();
            }
            
            matchedCase = new int[Examples.Count];
            for (int i = 0; i < Examples.Count; i++)
                matchedCase[i] = 0;

            SolveThePuzzle(1);

            for (int i = 0; i < Examples.Count; i++)
                matchedCase[i] = 0;

            SolveThePuzzle(2);

        }
        static void SolveThePuzzle(int part=1)
        {
            if (part == 1)
            {
                Console.WriteLine("Part One");
                int res = GetCasesForPartOne();
                Console.WriteLine(res);
                Console.WriteLine("");

            }
            else if (part == 2)
            {
                Console.WriteLine("Part Two");
                Rules.Remove(8);
                //Rules.Add(8, "42 | 42 8");
                Rules.Add(8, "42 | 42 *");

                Rules.Remove(11);
                //Rules.Add(11, "42 31 | 42 11 31");
                Rules.Add(11, "42 31 | 42 * 31");

                for (int i = 0; i < 5; i++)
                {
                    int x = GetCasesForPartTwo();
                    Console.WriteLine(x);
                }

            }
            else
                return;

        }

        static int GetCasesForPartOne()
        {
            string simpleRule;
            Rules.TryGetValue(0, out simpleRule);
            simpleRule = " " + simpleRule + " ";

            bool bRuleIsComplex = true;
            while (bRuleIsComplex)
                bRuleIsComplex = SimplifyRules(simpleRule, out simpleRule);

            int matchedCount = GetMatchedCases(simpleRule);

            return matchedCount;
        }

        static int GetCasesForPartTwo()
        {
            int res = 0;

            string simpleRule;
            Rules.TryGetValue(0, out simpleRule);
            simpleRule = " " + simpleRule + " ";

            bool bRuleIsComplex = true;
            while (bRuleIsComplex)
                bRuleIsComplex = SimplifyRules(simpleRule, out simpleRule);

            string rule8;
            Rules.TryGetValue(8, out rule8);
            rule8 = rule8.Replace("*", rule8);
            Rules.Remove(8);
            Rules.Add(8, rule8);

            string rule11;
            Rules.TryGetValue(11, out rule11);
            rule11 = rule11.Replace("*", rule11);
            Rules.Remove(11);
            Rules.Add(11, rule11);

            GetMatchedCases(simpleRule);

            for (int i = 0; i < Examples.Count; i++)
            {
                if (matchedCase[i] > 0)
                    res++;
            }

            return res;
        }

        static int GetMatchedCases(string simpleRule)
        {
            simpleRule = "^" + simpleRule.Replace(" ", "") + "$";
            //Console.WriteLine(simpleRule);

            Regex rg = new Regex(simpleRule);
            int matchedCount = 0;
            int caseN = 0;

            foreach (string Example in Examples)
            {
                if (rg.IsMatch(Example))
                {
                    //Console.WriteLine(Example);
                    matchedCount++;
                    matchedCase[caseN]++;
                }
                caseN++;
            }

            return matchedCount;
        }

        static bool SimplifyRules(string simpleRule, out string result)
        {
            string tempRule = null;

            string[] firstRule = simpleRule.Split(" ");

            foreach (string r in firstRule)
            {
                if(r!="a" && r!="b" && r!="(" && r!= ")" && r!="|" && r!= "" && r != "*")
                {
                    Rules.TryGetValue(int.Parse(r), out tempRule);
                    if (tempRule == null)
                        break;
                    
                    if (tempRule != "a" && tempRule != "b")
                        tempRule = " ( " + tempRule + " ) ";
                    else
                        tempRule = " " + tempRule + " ";

                    //string rule = " " + r + " ";
                    simpleRule = simpleRule.Replace(" " + r + " ", tempRule);
                }
            }

            result = simpleRule;
            if (tempRule == null)
                return false;
            else
                return true;
        }
    }
}
