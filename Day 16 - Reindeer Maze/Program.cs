using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReindeerMaze;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(s => s.ToCharArray()).ToArray();
        var start = Find('S', input);
        var end = Find('E', input);

        var paths = Walk(start, input);
        var bestScore = paths.Select(s => s.Score).Min();
        Console.WriteLine("Part 1: {0}", bestScore);

        var tileCount = paths.Where(s => s.Score == bestScore).SelectMany(s => s.Path).Distinct().Count();
        Console.WriteLine("Part 2: {0}", tileCount);
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

    static List<State> Walk(Point start, char[][] map){
        var toCheck = new Queue<State>();
        var startState = new State(start, new Point(0, 1), new List<Point>() { start }, 0);
        toCheck.Enqueue(startState);
        var checkd = new HashSet<State>() {};
        List<State> finishedPaths = new List<State>();

        while(toCheck.TryDequeue(out State path)){
            if (map[path.Position.row][path.Position.column] == 'E')
            {
                finishedPaths.Add(path);
                continue;
            }
            if (checkd.Any(p => p.Position == path.Position && p.direction == path.direction && p.Score < path.Score)) continue;
            checkd.Add(path);
            foreach(var newDir in TurnDirections(path.direction)){
                var next = path.Position + newDir;
                if (next.row >= map.Length || next.column > map[0].Length || next.row < 0 || next.column < 0) continue;
                if (map[next.row][next.column] == '#') continue;
                if (path.Path.Contains(next)) continue;

                var newPath = new List<Point>(path.Path)
                {
                    next
                };
                toCheck.Enqueue(new State(next, newDir, newPath, path.Score + (newDir == path.direction ? 1: 1001)));
            }
        }
        return finishedPaths;
    }
    
    static IEnumerable<Point> TurnDirections(Point startDirection){
        yield return startDirection;
        var newDir = new Point(startDirection.column * -1, startDirection.row);
        yield return newDir;
        newDir = new Point(startDirection.column, startDirection.row * -1);
        yield return newDir;
    }
}

public record State(Point Position, Point direction, List<Point> Path, int Score);

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
