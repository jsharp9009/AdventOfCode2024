using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RestroomRedoubt;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        int height = 103, width = 101;
        var initialRobots = parseInput(input);
        var movedRobots = initialRobots.Select(r => Move(r, 100, height, width)).ToList();

        var middleRow = Math.Floor(height / 2.0);
        var middleColumn = Math.Floor(width / 2.0);

        var q1 = movedRobots.Sum(r => r.location.row < middleColumn && r.location.column < middleRow ? 1 : 0);
        var q2 = movedRobots.Sum(r => r.location.row > middleColumn && r.location.column < middleRow ? 1 : 0);
        var q3 = movedRobots.Sum(r => r.location.row < middleColumn && r.location.column > middleRow ? 1 : 0);
        var q4 = movedRobots.Sum(r => r.location.row > middleColumn && r.location.column > middleRow ? 1 : 0);

        Console.WriteLine($"Part 1: {q1 * q2 * q3 * q4}");

        var noOverlap = FindNonOverlapping(initialRobots, height, width);
        Console.WriteLine($"Part 2: {noOverlap}");
    }

    static int FindNonOverlapping(List<Robot> robots, int height, int width)
    {
        var moved = robots;
        var move = 0;
        //moved = [.. moved.Select(r => Move(r, 100, height, width))];
        while (true)
        {
            moved = [.. moved.Select(r => Move(r, 1, height, width))];
            move++;

            foreach (var robot in moved)
            {
                if (!moved.Any(r => r.location.column == robot.location.column + 1 && r.location.row == robot.location.row)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 1 && r.location.row == robot.location.row + 1)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 1 && r.location.row == robot.location.row - 1)) continue;

                if (!moved.Any(r => r.location.column == robot.location.column + 2 && r.location.row == robot.location.row)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 2 && r.location.row == robot.location.row - 1)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 2 && r.location.row == robot.location.row + 1)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 2 && r.location.row == robot.location.row - 2)) continue;
                if (!moved.Any(r => r.location.column == robot.location.column + 2 && r.location.row == robot.location.row + 2)) continue;

                // for (int x = 0; x < height; x++)
                // {
                //     for (int y = 0; y < width; y++)
                //     {
                //         Console.Write(moved.Count(r => r.location.row == x && r.location.column == y));
                //     }
                //     Console.WriteLine();
                // }
                return move;
            }
        }
    }

    static List<Robot> parseInput(string[] input)
    {
        var robots = new List<Robot>();
        var regex = new Regex("p=(-*[0-9]+),(-*[0-9]+) v=(-*[0-9]+),(-*[0-9]+)");
        foreach (var line in input)
        {
            var match = regex.Match(line);
            robots.Add(new Robot(new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value))
                , new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))));
        }
        return robots;
    }

    static Robot Move(Robot robot, int toMove, int height, int width)
    {
        var Row = robot.location.row;
        var Col = robot.location.column;

        //for(int i = 0; i < toMove; i++){
        Row = mod(Row + (robot.velocity.row * toMove), width);
        Col = mod(Col + (robot.velocity.column * toMove), height);
        //} 

        return new Robot(new Point(Row, Col), robot.velocity);
    }

    static int mod(int x, int m)
    {
        return (int)(x - m * Math.Floor(x / (double)m));
    }
}

record Robot(Point location, Point velocity);

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
