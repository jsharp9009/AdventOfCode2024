using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GardenGroups;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var pInput = ParseInput(input);
        (var groups, var touches) = GetGroups(pInput);
        var total = groups.Sum(g => g.Count * g.Sum(g => 4 - touches[g].Count));
        Console.WriteLine("Part 1: {0}", total);
        //Learned from a hint, corners = sides, and corners are easier to find
        total = groups.Sum(g => g.Count * CountCorners(g, touches));
        Console.WriteLine("Part 2: {0}", total);
    }

    static (List<List<Point>> groups, Dictionary<Point, HashSet<Point>> touches) GetGroups(Dictionary<char, List<Point>> input)
    {
        List<List<Point>> groups = new List<List<Point>>();
        Dictionary<Point, HashSet<Point>> touches = new Dictionary<Point, HashSet<Point>>();
        foreach (var pair in input)
        {
            var chkd = new List<Point>() { };

            while (chkd.Count < pair.Value.Count)
            {
                var firstPoint = pair.Value.Except(chkd).First();
                chkd.Add(firstPoint);
                var toCheck = new Queue<Point>();
                toCheck.Enqueue(firstPoint);
                var grp = new List<Point>() { firstPoint };
                while (toCheck.TryDequeue(out Point? curr))
                {
                    if (!touches.ContainsKey(curr))
                    {
                        touches.Add(curr, new HashSet<Point>());
                    }
                    foreach (var dir in Point.CardinalDirections)
                    {
                        var nxt = curr + dir;
                        if (pair.Value.Contains(nxt))
                        {
                            touches[curr].Add(nxt);


                            if (touches.TryGetValue(nxt, out HashSet<Point>? value2))
                            {
                                value2.Add(curr);
                            }
                            else
                            {
                                touches.Add(nxt, [curr]);
                            }

                            if (!chkd.Contains(nxt))
                            {
                                grp.Add(nxt);
                                chkd.Add(nxt);
                                toCheck.Enqueue(nxt);
                            }
                        }
                    }
                }
                groups.Add(grp);
            }
        }
        return (groups, touches);
    }

    static Dictionary<char, List<Point>> ParseInput(string[] input)
    {
        var result = new Dictionary<char, List<Point>>();
        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                var c = input[row][col];
                if (result.TryGetValue(c, out List<Point>? value))
                {
                    value.Add(new Point(row, col));
                }
                else
                {
                    result.Add(c, [new(row, col)]);
                }
            }
        }
        return result;
    }


    static List<Point> adjacentDirections = new List<Point>()
    {
        new Point(1, 1),
        new Point(-1, 1),
        new Point(1, -1),
        new Point(-1, -1),
    };
    static int CountCorners(List<Point> group, Dictionary<Point, HashSet<Point>> touches){
        var corners = 0;
        foreach(var point in group){
            var adj = touches[point].ToArray();
            switch(adj.Length){
                case 0:
                    corners += 4;
                    break;
                case 1:
                    corners += 2;
                    break;
                case 2:
                    if (adj[0].row != adj[1].row && adj[0].column != adj[1].column){
                        var adj0 = adj[0];
                        var adj1 = adj[1];
                        var diag = new Point(point.row == adj0.row ? adj1.row : adj0.row, point.column == adj0.column ? adj1.column : adj0.column);
                        if (group.Contains(diag)) corners += 1;
                        else corners += 2;
                    }
                    break;
                case 3:
                    var pTouches = touches[point];
                    var pairAdj = pTouches.Where(p1 => pTouches.Any(p2 => p1 != p2 && (p2.column == p1.column || p2.row == p1.row)));
                    var third = pTouches.Except(pairAdj).First();
                    foreach(var p1 in pairAdj){
                        var diag = new Point(point.row == p1.row ? third.row : p1.row, point.column == p1.column ? third.column : p1.column);
                        if (!group.Contains(diag)) corners += 1;
                    }
                    break;
                case 4:
                    foreach(var dir in adjacentDirections){
                        var diag = point + dir;
                        if (!group.Contains(diag)) corners++;
                    }
                    break;
                default:
                    break;
            }
        }
        return corners;
    }
}

record Point(int row, int column)
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
