using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HoofIt;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(s => s.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray()).ToArray();
        var trailHeads = FindTrailHeads(input);

        var totals = trailHeads.Select(s =>
        {
            var ends = TrailEnds(input, s);
            return new Tuple<int, int>(ends.Distinct().Count(), ends.Count);
        });

        Console.WriteLine("Part 1: {0}", totals.Sum(s => s.Item1));
        Console.WriteLine("Part 2: {0}", totals.Sum(s => s.Item2));
    }

    static List<Point> FindTrailHeads(int[][] input) {
        var result = new List<Point>();
        for (int i = 0; i < input.Length; i++){
            for(int n = 0; n < input[i].Length; n++){
                if (input[i][n] == 0)
                    result.Add(new Point(i, n));
            }
        }
        return result;
    }

    static List<Point> TrailEnds(int[][] input, Point trailhead){
        List<Point> trailEnds = new List<Point>();
        var toCheck = new Queue<Point>();
        toCheck.Enqueue(trailhead);
        while(toCheck.Count > 0){
            var space = toCheck.Dequeue();
            var currentValue = input[space.row][space.column];
            foreach (var dir in Point.CardinalDirections) {
                var check = space + dir;
                if (check.row < 0 || check.row >= input.Length || check.column < 0 || check.column >= input[0].Length) continue;
                var checkValue = input[check.row][check.column];
                if (checkValue == currentValue + 1) { 
                    if (checkValue == 9) { 
                        trailEnds.Add(check);
                        continue;
                    }
                    toCheck.Enqueue(check); 
                }
                
            }
        }
        return trailEnds;
    }
}

record Point(int row, int column){
    public static Point operator +(Point p1, Point p2){
        return new Point(p1.row + p2.row, p1.column + p2.column);
    }

    public static Point operator -(Point p1, Point p2){
        return new Point(p1.row - p2.row, p1.column - p2.column);
    }

    public static readonly Point[] CardinalDirections = { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0) };
}
