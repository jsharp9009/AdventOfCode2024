using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClawContraption;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var machines = ParseInput(input);
        //Part 1 brute force. Part 2 can't be brute forced
        //var sum = machines.Sum(GetCost);
        var sum = machines.Sum(m => GetCostAlgebra(m, false));
        Console.WriteLine("Part 1: {0}", sum);
        sum = machines.Sum(m => GetCostAlgebra(m, true));
        Console.WriteLine("Part 2: {0}", sum);
    }

    static List<ClawMachine> ParseInput(string[] input){
        var buttonRegex = new Regex("Button [A|B]: X\\+([0-9]+), Y\\+([0-9]+)");
        var prizeRegex = new Regex("Prize: X\\=([0-9]+), Y\\=([0-9]+)");
        List<ClawMachine> machines = [];
        for (int i = 0; i < input.Length; i += 4){
            var buttonAMatches = buttonRegex.Match(input[i]);
            var buttonBMatches = buttonRegex.Match(input[i+1]);
            var prizeMatches = prizeRegex.Match(input[i + 2]);

            machines.Add(new ClawMachine(int.Parse(buttonAMatches.Groups[1].Value), int.Parse(buttonAMatches.Groups[2].Value),
                int.Parse(buttonBMatches.Groups[1].Value), int.Parse(buttonBMatches.Groups[2].Value),
                int.Parse(prizeMatches.Groups[1].Value), int.Parse(prizeMatches.Groups[2].Value)));
        }
        return machines;
    }

    //The brute force way, my first attempt
    static int GetCost(ClawMachine machine){
        var maxAPressX = machine.Prize_X / machine.ButtonA_X;
        var maxAPressY = machine.Prize_Y / machine.ButtonA_Y;

        var maxPress = Math.Min(100, Math.Min(maxAPressX, maxAPressY));
        var lowestPrice = int.MaxValue;
        for(int i = maxPress; i >= 0; i--){
            var newPrizeX = machine.Prize_X - (machine.ButtonA_X * i);
            var newPrizeY = machine.Prize_Y - (machine.ButtonA_Y * i);
            
            var xPresses = (double)newPrizeX / machine.ButtonB_X;
            var yPresses = (double)newPrizeY / machine.ButtonB_Y;

            if (xPresses != yPresses || xPresses % 1 != 0 || yPresses % 1 != 0) continue;

            var newPrice = (i * 3) + xPresses;
            if (newPrice > lowestPrice) return lowestPrice;
            lowestPrice = (int)Math.Min(lowestPrice, newPrice);
        }
        return lowestPrice == int.MaxValue ? 0 : lowestPrice;
    }

    static long GetCostAlgebra(ClawMachine machine, bool Part2){
        double targetX = machine.Prize_X;
        double targetY = machine.Prize_Y;

        if(Part2){
            targetX += 10000000000000;
            targetY += 10000000000000;
        }

        var dn = (machine.ButtonA_X * machine.ButtonB_Y) - (machine.ButtonB_X * machine.ButtonA_Y);
        var x = Math.Abs(((targetX * machine.ButtonB_Y) - (targetY * machine.ButtonB_X)) / dn);
        var y = Math.Abs(((machine.ButtonA_X * targetY) - (machine.ButtonA_Y * targetX)) / dn);

        if (x % 1 > 0 || y % 1 > 0) return 0;
        if (!Part2 && (x > 100 || y > 100 || y < 1 || x < 1)) return 0;
        return (long)(x * 3 + y);
    }
}

record ClawMachine(int ButtonA_X, int ButtonA_Y, int ButtonB_X, int ButtonB_Y, int Prize_X, int Prize_Y);