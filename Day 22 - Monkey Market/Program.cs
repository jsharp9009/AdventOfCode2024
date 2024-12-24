using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace MonkeyMarket;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();

        long total = 0L;
        List<List<(sbyte step, byte bananas)>> bananasSteps = new List<List<(sbyte step, byte bananas)>>();
        foreach(var num in input){
            var secret = num;
            List<(sbyte step, byte bananas)> steps = [(0,((byte)(num % 10)))];

            for(int i = 0; i < 2000; i++){
                secret = GenerateNextNumber(secret);
                var price = (byte)(secret % 10);
                steps.Add(((sbyte)(price - steps.Last().bananas), price));
                
            }
            bananasSteps.Add(steps);
            total += secret;
        }

        Console.WriteLine("Part 1: {0}", total);

        var totals = new Dictionary<long, int>();
        for (int b = 0; b < bananasSteps.Count; b++)
        {
            var chkd = new HashSet<long>();
            for (int i = 4; i < bananasSteps[b].Count; i++)
            {
                var sequence = new List<(sbyte step, byte bananas)> { bananasSteps[b][i - 3], bananasSteps[b][i - 2], bananasSteps[b][i - 1], bananasSteps[b][i] };
                var sequenceTotal = sequence.Last().bananas;
                var sqHash = sequence.Select(s => s.step).Select((n, index) => ((long)n) << 8 * index).Sum();
                if (chkd.Contains(sqHash)) continue;

                if (totals.ContainsKey(sqHash)) totals[sqHash] += sequenceTotal;
                else totals.Add(sqHash, sequenceTotal);

                chkd.Add(sqHash);
            }
        }
        Console.WriteLine("Part 2: {0}", totals.Max(s => s.Value));
    }

    static byte GetSequenceValue(List<(sbyte step, byte bananas)> steps, sbyte[] sequence) {
        var stepLIst = steps.Select(s => s.step).ToList();
        var index = stepLIst.IndexOf(sequence[0]);
        while(index > -1 && index <= 1996){
            if (stepLIst[index + 1] == sequence[1]
            && stepLIst[index + 2] == sequence[2]
            && stepLIst[index + 3] == sequence[3])
                return steps[index + 3].bananas; 

            index = stepLIst.IndexOf(sequence[0], index + 1);
        }
        return 0;
    }

    static long GenerateNextNumber(long secret){
        var val = secret * 64;
        secret = prune(mix(val, secret));
        val = (long)Math.Floor(secret / 32.0);
        secret = prune(mix(val, secret));
        val = secret * 2048;
        return prune(mix(val, secret));
    }

    static long mix(long value, long secret){
        return value ^ secret;
    }

    static long prune(long secret){
        return secret % 16777216;
    }
}
