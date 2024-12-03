using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MullItOver;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");

        var regex = new Regex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)");
        var total = 0;
        foreach (var line in input){
            var matches = regex.Matches(line);
            foreach (Match match in matches){
                total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }

        Console.WriteLine("Part 1: {0}", total);

        regex = new Regex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)|don't\\(\\)|do\\(\\)");
        total = 0;
        var mulEnabled = true;
        foreach (var line in input){
            var matches = regex.Matches(line);
            foreach (Match match in matches){
                switch (match.Groups[0].Value) {
                    case "do()":
                        mulEnabled = true;
                        break;
                    case "don't()":
                        mulEnabled = false;
                        break;
                    default:
                        if(mulEnabled) total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
                        break;
                }
            }
        }

        Console.WriteLine("Part 2: {0}", total);
    }
}
