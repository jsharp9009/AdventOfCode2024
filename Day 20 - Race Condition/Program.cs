using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RaceCondition;
// Too Low 1326
class Program
{
    static readonly Point[] cheatDirections = { new Point(0, 2), new Point(2, 0), new Point(0, -2), new Point(-2, 0) };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(s => s.ToCharArray()).ToArray();
        var start = Find('S', input);
        var path = Walk(start, input);
        Console.WriteLine("Part 1: {0}", FindCheats(path, 100, 2));
        Console.WriteLine("Part 2: {0}", FindCheats(path, 100, 20));
    }

    static int FindCheats(List<Point> path, int threshold, int cheatLength){
        var cheatCount = 0;
        for (int i = 0; i < path.Count; i++)
        {
            var check = path[i];
            var manhattan = path.AsParallel().ToDictionary(p => p, p => Math.Abs(p.row - check.row) + Math.Abs(p.column - check.column))
                .Where(p => p.Value <= cheatLength && p.Key != check).ToDictionary(p => p.Key, p => p.Value);
            cheatCount += path.AsParallel().Where((p, index) => manhattan.ContainsKey(p) && index - (i + manhattan[p]) >= threshold).Count();
        }
        return cheatCount;
    }

    static Point Find(char c, char[][] map){
        for(int row = 0; row < map.Length; row++){
            for(int col = 0; col < map[row].Length; col++){
                if (map[row][col] == c){
                    return new Point(row, col);
                }
            }
        }
        return new Point(-1, -1);
    }

    static List<Point> Walk(Point start, char[][] map){
        List<Point> path = new List<Point>(){start};
        var curr = start;
        while(true){
            foreach(var dir in Point.CardinalDirections){
                var next = curr + dir;

                if (next.row >= map.Length || next.column > map[0].Length || next.row < 0 || next.column < 0) continue;
                if (map[next.row][next.column] == '#') continue;
                if (path.Contains(next)) continue;

                path.Add(next);
                curr = next;
                break;
            }
            if(map[curr.row][curr.column] == 'E') break;
        }
        return path;
    }
}

public record State(Point Position, Point direction);

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
