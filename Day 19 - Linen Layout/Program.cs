using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace LinenLayout;
//Low 751983468
class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (List<string> towels, List<string> toCreate) = parseInput(input);
        var createCount = toCreate.Count(c => CanCreate(c, towels));
        Console.WriteLine("Part 1: {0}", createCount);
        var possibleCreateCount = 0L;
        foreach(var c in toCreate){
            possibleCreateCount += CountCreate(c, towels);
        }
        Console.WriteLine("Part 2: {0}", possibleCreateCount);
    }


    static bool CanCreate(string pattern, List<string> towels)
    {
        foreach (var towel in towels)
        {
            if (pattern.StartsWith(towel))
            {
                if (pattern.Length == towel.Length) return true;

                if (CanCreate(new string(pattern.Skip(towel.Length).ToArray()), towels)) return true;
            }
        }
        return false;
    }

    static Dictionary<string, long> cache = new Dictionary<string, long>();
    static long CountCreate(string pattern, List<string> towels)
    {
        if (cache.TryGetValue(pattern, out var result)) return result;
        var canCreate = 0L;
        foreach (var towel in towels)
        {
            if (pattern.StartsWith(towel))
            {
                if (pattern.Length == towel.Length)
                {
                    canCreate += 1;
                    continue;
                }
                canCreate += CountCreate(new string(pattern.Skip(towel.Length).ToArray()), towels);
            }
        }
        cache.Add(pattern, canCreate);
        return canCreate;
    }

    static (List<string> towels, List<string> toCreate) parseInput(string[] input)
    {
        var towels = new List<string>(input[0].Split(",").Select(s => s.Trim()));

        return (towels, input.Skip(2).ToList());
    }
}
