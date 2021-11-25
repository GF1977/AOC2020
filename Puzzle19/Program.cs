﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Puzzle19
{
    class Program
    {
        static Dictionary<int, string> Rules = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@"C:\Users\iopya\Source\Repos\AOC2020\Puzzle19\data.txt");
            string line = file.ReadLine();
            List<string> Examples = new List<string>();
            
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

            string simpleRule;
            Rules.TryGetValue(0, out simpleRule);
            simpleRule = " " + simpleRule + " ";

            bool bRuleIsComplex = true;
            while (bRuleIsComplex)
                bRuleIsComplex = simplifyRules(simpleRule, out simpleRule);


            simpleRule = "^" + simpleRule.Replace(" ", "") + "$";
            //Console.WriteLine(simpleRule);

            Regex rg = new Regex(simpleRule);
            int matchedCount=0;

            foreach (string Example in Examples)
            {
                if (rg.IsMatch(Example))
                {
                    // Console.WriteLine(Example);
                    matchedCount++;
                }
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("The answer: {0}", matchedCount);

        }
        
        static bool simplifyRules(string simpleRule, out string result)
        {
            string tempRule = null;

            string[] firstRule = simpleRule.Split(" ");

            foreach (string r in firstRule)
            {
                if(r!="a" && r!="b" && r!="(" && r!= ")" && r!="|" && r!= "")
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
