using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HistorianHysteria;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (List<int> l1, List<int> l2) = ParseInput(input);
        l1 = l1.Order().ToList();
        l2 =  l2.Order().ToList();

        var difference = 0;
        for(int i = 0; i < l1.Count; i++){
            difference += Math.Abs(l1[i] - l2[i]);
        }

        Console.WriteLine("Part 1: {0}", difference);

        var lastChecked = 0;
        var lastCount = 0;

        var similarityScore = 0;

        foreach(var num in l1){
            if(lastChecked == num){
                similarityScore += lastChecked * lastCount;
                continue;
            }

            lastChecked = num;
            lastCount = l2.Count(c => c == num);
            similarityScore += lastChecked * lastCount;
        }

        Console.WriteLine("Part 2: {0}", similarityScore);
    }

    static (List<int> l1, List<int> l2) ParseInput(string[] input){
        var l1 = new List<int>();
        var l2 = new List<int>();
        foreach (var i in input){
            var split = i.Split("   ");
            l1.Add(int.Parse(split[0]));
            l2.Add(int.Parse(split[1]));
        }
        return (l1, l2);
    }
}
