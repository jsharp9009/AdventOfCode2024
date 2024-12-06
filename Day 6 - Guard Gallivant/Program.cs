using System;
using System.Collections.Generic;
using System.IO;

namespace GuardGallivant;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (HashSet<Point> obstacles, Point start) = ParseInput(input);
        var rows = input.Length;
        var columns = input[0].Length;

        var visited = CountVisited(obstacles, start, rows, columns);
        Console.WriteLine("Part 1: {0}", visited.Count);

        var loops = 0;    
        //Obstacle will be along the path.
        foreach(var p in visited){
            var obs = new HashSet<Point>(obstacles)
            {
                p
            };

            if(checkLoop(obs, start, rows, columns))
                loops++;
        }

        Console.WriteLine("Part 2: {0}", loops);
    }

    static bool checkLoop(HashSet<Point> obstacles, Point start, int rows, int columns){
        var current = start;
        var direction = new Point(-1, 0);
        var visited = new Dictionary<Point, int>(){ { start, 0 } };
        while (true) {
            var next = current + direction;
            if (obstacles.Contains(next)) {
                direction = new Point(direction.column, direction.row * -1);
            }
            else if (next.row < 0 || next.row >= rows || next.column < 0 || next.column >= columns){
                return false;
            }
            else{
                if (visited.ContainsKey(next))
                {
                    visited[next]++;
                    //Magic Number! 
                    //If we visit a point 5 times, its a loop
                    if (visited[next] >= 5)
                    {
                        return true;
                    }
                }
                else
                {
                    visited.Add(next, 1);
                }

                current = next;
            }
        }
    }

    static HashSet<Point> CountVisited(HashSet<Point> obstacles, Point start, int rows, int columns){
        var current = start;
        var direction = new Point(-1, 0);
        var visited = new HashSet<Point>(){start};

        while (true) {
            var next = current + direction;
            if (obstacles.Contains(next)) {
                direction = new Point(direction.column, direction.row * -1);
            }
            else if (next.row < 0 || next.row >= rows || next.column < 0 || next.column >= columns){
                return visited;
            }
            else{
                if(!visited.Contains(next)) visited.Add(next);
                current = next;
            }
        }
    }

    static (HashSet<Point> obstacles, Point start) ParseInput(string[] input){
        var obstacles = new HashSet<Point>();
        var start = new Point(0, 0);
        for(int row = 0; row < input.Length; row++){
            for (int column = 0; column < input[row].Length; column++){
                if (input[row][column] == '#')
                {
                    obstacles.Add(new Point(row, column));
                }
                else if (input[row][column] == '^') {
                    start = new Point(row, column);
                }
            }
        }
        return (obstacles, start);
    }

}

record Point(int row, int column){
    public static Point operator +(Point p1, Point p2){
        return new Point(p1.row + p2.row, p1.column + p2.column);
    }
}
