using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.VisualBasic;

namespace KeypadConundrum;

class Program
{
    static Dictionary<Point, char> numberic_keypad_map = new Dictionary<Point, char>{
        {new Point(0,0), '7'},
        {new Point(0,1), '8'},
        {new Point(0,2), '9'},
        {new Point(1,0), '4'},
        {new Point(1,1), '5'},
        {new Point(1,2), '6'},
        {new Point(2,0), '1'},
        {new Point(2,1), '2'},
        {new Point(2,2), '3'},
        {new Point(3,1), '0'},
        {new Point(3,2), 'A'},
    };

    static Dictionary<Point, char> directional_keypad_map = new Dictionary<Point, char>(){
        {new Point(0,1), '^'},
        {new Point(0,2), 'a'},
        {new Point(1,0), '<'},
        {new Point(1,1), 'v'},
        {new Point(1,2), '>'},
    };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var moveCache = new Dictionary<(char start, char end), List<string>>();
        moveCache = GetKeypadMap(numberic_keypad_map, moveCache);
        moveCache = GetKeypadMap(directional_keypad_map, moveCache);

        var inputMap = input.ToDictionary(x => x, x => GetNumVal(x));

        var total = 0L;
        foreach (var line in inputMap)
        {
            var shortest = GetShortestPath(moveCache, 2, line.Key, 0);
            total += shortest * line.Value;
        }
        Console.WriteLine("Part 1: {0}", total);

        total = 0L;
        _cache = new Dictionary<(string path, int depth), long>();

        foreach (var line in inputMap)
        {
            var shortest = GetShortestPath(moveCache, 25, line.Key, 0);
            total += shortest * line.Value;
        }
        Console.WriteLine("Part 2: {0}", total);

    }

    static int GetNumVal(string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            if (char.IsDigit(c)) { sb.Append(c); };
        }
        return int.Parse(sb.ToString());
    }

    static Dictionary<(string path, int depth), long> _cache = new Dictionary<(string path, int depth), long>();
    static long GetShortestPath(Dictionary<(char start, char end), List<string>> moveCache, int maxDept, string pattern, int depth)
    {
        if (_cache.TryGetValue((pattern, depth), out var result)) return result;
        var total = 0L;
        var curChar = depth == 0 ? 'A' : 'a';
        foreach (var c in pattern)
        {
            if (depth == maxDept) total += moveCache[(curChar, c)][0].Length;
            else total += moveCache[(curChar, c)].Min(m => GetShortestPath(moveCache, maxDept, m, depth + 1));
            curChar = c;
        }
        _cache.Add((pattern, depth), total);
        return total;
    }

    static Dictionary<Point, char> moveMap = new Dictionary<Point, char>()
    {
        {new Point(0,1), '>'},
        {new Point(0,-1), '<'},
        {new Point(1, 0), 'v'},
        {new Point(-1,0), '^'},
    };
    static string ToMoveString(List<Point> path)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < path.Count - 1; i++)
        {
            var move = path[i + 1] - path[i];
            sb.Append(moveMap[move]);
        }
        sb.Append('a');
        return sb.ToString();
    }

    static Dictionary<(char start, char end), List<string>> GetKeypadMap(Dictionary<Point, char> map, Dictionary<(char start, char end), List<string>> paths)
    {
        foreach (var p in map)
        {
            foreach (var q in map)
            {
                if (p.Value == q.Value)
                {
                    paths[(p.Value, q.Value)] = ["a"];
                    continue;
                }
                paths[(p.Value, q.Value)] = FindAllPaths(map, p.Key, q.Key);
            }
        }
        return paths;
    }

    static List<string> FindAllPaths(Dictionary<Point, char> map, Point start, Point end)
    {
        Queue<List<Point>> paths = new Queue<List<Point>>();
        paths.Enqueue([start]);
        var finalPaths = new List<string>();
        while (paths.TryDequeue(out var path))
        {
            var last = path.Last();
            foreach (var dir in Point.CardinalDirections)
            {
                var nw = last + dir;
                if (map.ContainsKey(nw) && !path.Contains(nw))
                {
                    var newPath = new List<Point>(path) { nw };
                    paths.Enqueue(newPath);
                    if (nw == end)
                    {
                        finalPaths.Add(ToMoveString(newPath));
                    }
                }
            }
        }
        var min = finalPaths.Min(s => s.Length);
        finalPaths.RemoveAll(f => f.Length > min);
        return finalPaths;
    }


}

public record Point(int row, int column)
{
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.row + p2.row, p1.column + p2.column);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.row - p2.row, p1.column - p2.column);
    }

    public static readonly Point[] CardinalDirections = { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0) };
}