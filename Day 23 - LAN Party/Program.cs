using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LANParty;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var connections = ParseInput(input);

        int groups = 0;
        HashSet<string> chkd = [];
        foreach (var group in connections)
        {
            var key = group.Key;
            List<string> myGroups = [];
            foreach (var computer in group.Value)
            {
                if (computer == key || chkd.Contains(computer)) continue;
                foreach (var computer2 in connections[computer])
                {
                    if (computer2 == key || computer2 == computer || chkd.Contains(computer2)) continue;
                    if (connections[computer2].Contains(key))
                    {
                        var groupString = string.Join("", new string[] { key, computer, computer2 }.Order().ToArray());
                        if (!myGroups.Contains(groupString)) myGroups.Add(groupString);
                    }
                }
            }
            groups += myGroups.Count(m => m.Where((c, i) => i % 2 == 0 && c == 't').Any());
            chkd.Add(key);
        }
        Console.WriteLine("Part 1: {0}", groups);

        var maxLength = "";
        foreach (var connectoin in connections)
        {
            var clique = new List<string>() { connectoin.Key };
            foreach (var connection2 in connections)
            {
                if (connectoin.Key == connection2.Key) continue;
                if (clique.All(c => connections[c].Contains(connection2.Key)))
                {
                    clique.Add(connection2.Key);
                }
            }
            var groupString = string.Join(',', [.. clique.Order()]);
            if (groupString.Length > maxLength.Length) maxLength = groupString;
        }
        Console.WriteLine("Part 2: {0}", maxLength);
    }

    static Dictionary<string, List<string>> ParseInput(string[] connections)
    {
        var results = new Dictionary<string, List<string>>();
        foreach (var connection in connections)
        {
            var split = connection.Split('-');
            if (results.ContainsKey(split[0])) results[split[0]].Add(split[1]);
            else results.Add(split[0], [split[1]]);

            if (results.ContainsKey(split[1])) results[split[1]].Add(split[0]);
            else results.Add(split[1], [split[0]]);
        }
        return results;
    }
}
