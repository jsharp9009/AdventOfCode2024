using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlutonianPebbles;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt").Split(" ").Select(i => int.Parse(i)).ToArray();
        var dInput = InputToDictionary(input);
        
        for(int i = 0; i < 25; i++){
            dInput = Blink(dInput);
        }

        var total = dInput.Sum(s => s.Value);
        Console.WriteLine("Part 1: {0}", total);

        for(int i = 0; i < 50; i++){
            dInput = Blink(dInput);
        }

        long ltotal = dInput.Sum(s => s.Value);
        Console.WriteLine("Part 2: {0}", ltotal);
    }

    static Dictionary<long, long> Blink(Dictionary<long, long> rocks) {
        var newRocks = new Dictionary<long, long>();
        foreach (var rockSet in rocks) {
            if (rockSet.Key == 0) {
                newRocks = AddValue(newRocks, 1, rockSet.Value);
            }
            else if (rockSet.Key.ToString().Length % 2 == 0){
                var rockString = rockSet.Key.ToString();
                var half = rockString.Length / 2;
                var r1 = int.Parse(rockString.Substring(0, half));
                var r2 = int.Parse(rockString.Substring(half));

                newRocks = AddValue(newRocks, r1, rockSet.Value);
                newRocks = AddValue(newRocks, r2, rockSet.Value);
            }
            else {
                var newKey = rockSet.Key * 2024;
                newRocks = AddValue(newRocks, newKey, rockSet.Value);
            }
        }
        return newRocks;
    }

    static Dictionary<long, long> AddValue(Dictionary<long, long> rocks, long key, long value){
        if (rocks.ContainsKey(key)) rocks[key] += value;
                else rocks.Add(key, value);
        return rocks;
    }

    static Dictionary<long, long> InputToDictionary(int[] input){
        var dict = new Dictionary<long, long>();
        foreach(var r in input){
            if (dict.ContainsKey(r)) dict[r]++;
                else dict.Add(r, 1);
        }
        return dict;
    }
}