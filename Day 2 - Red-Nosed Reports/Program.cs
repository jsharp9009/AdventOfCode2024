using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RedNosedReports;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(s => s.Split(' ')).Select(l => l.Select(i => int.Parse(i)).ToList()).ToList();
        var safe = input.Where(IsSafe).ToList();
        Console.WriteLine("Part 1: {0}", safe.Count());
        safe.AddRange(input.Except(safe).Where(IsSafeWithDampner));
        Console.WriteLine("Part 2: {0}", safe.Count());
    }

    static bool IsSafe(List<int> input){
        var decrease = input[0] > input.Last();
        int dif;
        for(int i = 0; i < input.Count - 1; i++){
            dif = decrease ? input[i] - input[i + 1] : input[i + 1] - input[i];

            if (dif < 1 || dif > 3) {
                return false;
            }
        }
        return true;
    }

    static bool IsSafeWithDampner(List<int> input){
        for(int i = 0; i < input.Count; i++){
            var test = input.Where((num, index) => index != i).ToList();
            if (IsSafe(test)) return true;
        }
        return false;
    }
}
