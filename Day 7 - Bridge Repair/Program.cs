using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BridgeRepair;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var equations = parseInput(input);

        var part1 = equations.Where(e => CanBeTrue(e));
        var part1Total = part1.Sum(p => p.Item1);
        Console.WriteLine("Part 1: {0}", part1Total);

        var testPart2 = equations.Except(part1);
        var part2 = testPart2.Where(e => CanBeTrue(e, true));
        var part2Total = part2.Sum(p => p.Item1);
        Console.WriteLine("Part 2: {0}", part2Total + part1Total);
    }

    static List<Tuple<long, List<long>>> parseInput(string[] input){
        var result = new List<Tuple<long, List<long>>>();
        foreach(var line in input){
            var parts = line.Split(":");
            var testValue = long.Parse(parts[0].Trim());
            var nums = parts[1].Trim().Split(" ");
            var numsList = new List<long>();
            foreach(var num in nums){
                numsList.Add(long.Parse(num.Trim()));
            }
            result.Add(Tuple.Create(testValue, numsList));
        }
        return result;
    }

    static bool CanBeTrue(Tuple<long, List<long>> equation, bool concat = false){
        HashSet<long> curValues = new HashSet<long>() { equation.Item2.First() };
        for(int i = 1; i < equation.Item2.Count; i++){
            var newValues = new HashSet<long>();
            foreach(var value in curValues){
                var tVal = value + equation.Item2[i];
                if(tVal <= equation.Item1) newValues.Add(tVal);

                tVal = value * equation.Item2[i];
                if(tVal <= equation.Item1) newValues.Add(tVal);

                if(concat){
                    tVal = long.Parse(value.ToString() + equation.Item2[i].ToString());
                    if(tVal <= equation.Item1) newValues.Add(tVal);
                }
            }
            curValues = newValues;
        }
        return curValues.Contains(equation.Item1);
    }
}