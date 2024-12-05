using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrintQueue;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (Dictionary<int, List<int>> rules, List<List<int>> updates) = ParseInput(input);

        List<List<int>> failed = new List<List<int>>();
        var totalValid = 0;
        foreach(var update in updates){
            var valid = true;
            for(int i = 0; i < update.Count; i++){
                var page = update[i];
                if(rules.ContainsKey(page)){
                    var rule = rules[page];
                    foreach(var rule2 in rule){
                        var index = update.IndexOf(rule2);
                        if(index >= 0 && index < i){
                            valid = false;
                            failed.Add(update);
                            break;
                        }
                    }
                }
                if (!valid) break;
            }
            if(valid){
                totalValid += update[(int)Math.Ceiling(update.Count / 2.0) - 1];
            }        
        }

        Console.WriteLine("Part 1: {0}", totalValid);

        totalValid = 0;
        foreach(var err in failed){
            var valid = true;
            for(int i = 0; i < err.Count; i++){
                var page = err[i];
                if(rules.ContainsKey(page)){
                    var rule = rules[page];
                    foreach(var rule2 in rule){
                        var index = err.IndexOf(rule2);
                        if(index >= 0 && index < i){
                            valid = false;
                            var err1 = err[i];
                            var err2 = err[index];
                            err[i] = err2;
                            err[index] = err1;
                            break;
                        }
                    }
                }
                if (!valid){
                    valid = true;
                    i = -1;
                }
            }
            totalValid += err[(int)Math.Ceiling(err.Count / 2.0) - 1];     
        }
        Console.WriteLine("Part 2: {0}", totalValid);
    }

    static (Dictionary<int, List<int>> rules, List<List<int>> updates) ParseInput(string[] input){
        var rules = new Dictionary<int, List<int>>();
        var i = 0;
        while(true){
            var line = input[i];
            if (string.IsNullOrEmpty(line)) break;

            var split = line.Split("|");
            var key = int.Parse(split[0]);
            var value = int.Parse(split[1]);

            if(rules.ContainsKey(key)){
                rules[key].Add(value);
            }
            else{
                rules.Add(key, new List<int>(){value});
            }
            i++;
        }
        
        var updates = new List<List<int>>();
        for(int n = i + 1; n < input.Length; n++){
            updates.Add(input[n].Split(',').Select(s => int.Parse(s)).ToList());
        }

        return (rules, updates);
    }
}
