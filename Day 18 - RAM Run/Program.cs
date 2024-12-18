using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RAMRun;

class Program
{
    static void Main(string[] args)
    {
        // //Test
        // var size = 7;
        // var bytes = 12;

        // Real
        var size = 71;
        var bytes = 1024;

        var input = File.ReadAllLines("input.txt").Select(s => s.Split(',')).Select(p => new Point(int.Parse(p[0]), int.Parse(p[1]))).ToList();
        (var shortest, var path) = ShortestPath(input.Take(bytes).ToList(), size);
        Console.WriteLine("Part 1: {0}", shortest);
        path.Reverse();
        var overlap = FindFirstOverlap(input, path);

        for(int i = overlap; i < input.Count; i++){
            (shortest, path) = ShortestPath(input.Take(i).ToList(), size);
            if (shortest == -1){
                var b = input[i - 1];
                Console.WriteLine("Part 2: {0},{1}", b.row, b.column);
                break;
            }
            else{
                path.Reverse();
                overlap = FindFirstOverlap(input, path);
                if (overlap > i) i = overlap;
            }
        }
    }

    static int FindFirstOverlap(List<Point> input, List<Point> path){
        for(int i = 0; i < input.Count; i++){
            if (path.Contains(input[i])) return i;
        }
        return -1;
    }

    static (int, List<Point> path) ShortestPath(List<Point> obstacles, int size){
        var start = new Point(0, 0);
        var chkd = new HashSet<Point>();
        var preds = new Dictionary<Point, Point>();
        var distances = new Dictionary<Point, int>
        {
            { start, 0 }
        };
        while(true){
            var toCheck = distances.Where(d => !chkd.Contains(d.Key)).OrderBy(p => p.Value).FirstOrDefault();
            if (toCheck.Equals(default(KeyValuePair<Point, int>))) break;
            foreach(var dir in Point.CardinalDirections){
                var next = toCheck.Key + dir;
                if(next.row < 0 || next.column < 0 || next.row >= size || next.column >= size) continue;
                if(obstacles.Contains(next)) continue;
                
                if(distances.ContainsKey(next)){
                    if(distances[next] > toCheck.Value + 1){
                        distances[next] = toCheck.Value + 1;
                        preds[next] = toCheck.Key;
                    }
                }
                else{
                    distances.Add(next, toCheck.Value + 1);
                    preds.Add(next, toCheck.Key);
                }
            }
            chkd.Add(toCheck.Key);
        }
        var toReturn = new Point(size - 1, size - 1);
        var dist = distances.TryGetValue(toReturn, out int value) ? value : -1;
        if (dist == -1) return (-1, new List<Point>());
        
        var path = new List<Point>(){toReturn};
        while(preds.TryGetValue(path.Last(), out Point p)){
            path.Add(p);
        }
        return (dist, path);
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