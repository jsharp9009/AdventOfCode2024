using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WarehouseWoes;

class Program
{
    static Dictionary<char, Point> dirPoints = new Dictionary<char, Point>() {
        { '^', new Point(-1,0)},
        { '<', new Point(0,-1)},
        { '>', new Point(0,1)},
        {'v', new Point(1,0)},
    };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (char[][] map, char[] directions) = parseInput(input);
        var map2 = expandMap(map);

        //Part 1
        var newMap = Walk(map, directions, false);
        var sum = 0;
        for (int i = 0; i < newMap.Length; i++)
        {
            for (int n = 0; n < newMap[i].Length; n++)
            {
                if (newMap[i][n] == 'O')
                {
                    sum += (i * 100) + n;
                }
            }
        }
        //Print(newMap);
        Console.WriteLine("Part 1: {0}", sum);

        //Part2
        newMap = Walk(map2, directions, true);
        sum = 0;
        for (int i = 0; i < newMap.Length; i++)
        {
            for (int n = 0; n < newMap[i].Length; n++)
            {
                if (newMap[i][n] == '[')
                {
                    sum += (i * 100) + n;
                }
            }
        }
        //Print(newMap);
        Console.WriteLine("Part 2: {0}", sum);
    }

    static char[][] Walk(char[][] map, char[] directions, bool move2)
    {
        var robot = new Point(0, 0);
        for (int i = 0; i < map.Length; i++)
        {
            for (int n = 0; n < map[i].Length; n++)
            {
                if (map[i][n] == '@')
                {
                    robot = new Point(i, n);
                    break;
                }
            }
            if (robot.row != 0) break;
        }

        foreach (var dir in directions)
        {
            //Print(map);
            var d = dirPoints[dir];
            if (move2)
            {
                if (TryMovePart2(robot, d, ref map, false))
                {
                    DoMovePart2(robot, d, ref map, false);
                    robot += d;
                };
            }
            else
            {
                if (TryMovePart1(robot, d, ref map))
                {
                    robot += d;
                };
            }

        }

        return map;
    }

    static bool TryMovePart1(Point current, Point move, ref char[][] map)
    {
        var toMoveTo = current + move;
        if (toMoveTo.row < 0 || toMoveTo.row >= map.Length || toMoveTo.column < 0 || toMoveTo.column >= map[toMoveTo.row].Length)
            return false;

        if (map[toMoveTo.row][toMoveTo.column] == '#') return false;

        if (map[toMoveTo.row][toMoveTo.column] == '.' || TryMovePart1(toMoveTo, move, ref map))
        {
            map[toMoveTo.row][toMoveTo.column] = map[current.row][current.column];
            map[current.row][current.column] = '.';
            return true;
        }

        return false;
    }

    static bool TryMovePart2(Point current, Point move, ref char[][] map, bool boxCheck)
    {
        var toMoveTo = current + move;
        var verticalMove = move.row != 0;
        if (toMoveTo.row < 0 || toMoveTo.row >= map.Length || toMoveTo.column < 0 || toMoveTo.column >= map[toMoveTo.row].Length)
            return false;
        if (map[toMoveTo.row][toMoveTo.column] == '#') return false;

        if ((map[toMoveTo.row][toMoveTo.column] == ']' || map[toMoveTo.row][toMoveTo.column] == '[') && boxCheck && !verticalMove) return true;

        if (map[toMoveTo.row][toMoveTo.column] == '.')
        {
            if (boxCheck)
            {
                return true;
            }
            else
            {
                bool canMove = false;
                switch (map[current.row][current.column])
                {
                    case '@':
                        canMove = true;
                        break;
                    case '[':
                        canMove = TryMovePart2(new Point(current.row, current.column + 1), move, ref map, true);
                        break;
                    case ']':
                        canMove = TryMovePart2(new Point(current.row, current.column - 1), move, ref map, true);
                        break;
                }
                if (canMove)
                {
                    return true;
                }
            }
        }

        else if (TryMovePart2(toMoveTo, move, ref map, false))
        {
            if (boxCheck)
            {
                return true;
            }
            else
            {
                bool canMove = false;
                switch (map[current.row][current.column])
                {
                    case '@':
                        canMove = true;
                        break;
                    case '[':
                        canMove = TryMovePart2(new Point(current.row, current.column + 1), move, ref map, true);
                        break;
                    case ']':
                        canMove = TryMovePart2(new Point(current.row, current.column - 1), move, ref map, true);
                        break;
                }
                if (canMove)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static void DoMovePart2(Point current, Point move, ref char[][] map, bool boxCheck)
    {
        var toMoveTo = current + move;
        var verticalMove = move.row != 0;
        if (toMoveTo.row < 0 || toMoveTo.row >= map.Length || toMoveTo.column < 0 || toMoveTo.column >= map[toMoveTo.row].Length)
            return;
        if (map[toMoveTo.row][toMoveTo.column] == '#') return;

        if ((map[toMoveTo.row][toMoveTo.column] == ']' || map[toMoveTo.row][toMoveTo.column] == '[') && boxCheck && !verticalMove) return;

        if (map[toMoveTo.row][toMoveTo.column] == '.')
        {
            if (boxCheck)
            {
                map[toMoveTo.row][toMoveTo.column] = map[current.row][current.column];
                map[current.row][current.column] = '.';
                return;
            }
            else
            {
                switch (map[current.row][current.column])
                {
                    case '[':
                        DoMovePart2(new Point(current.row, current.column + 1), move, ref map, true);
                        break;
                    case ']':
                        DoMovePart2(new Point(current.row, current.column - 1), move, ref map, true);
                        break;
                }

                map[toMoveTo.row][toMoveTo.column] = map[current.row][current.column];
                map[current.row][current.column] = '.';
                return;
            }
        }

        DoMovePart2(toMoveTo, move, ref map, false);

        if (boxCheck)
        {
            map[toMoveTo.row][toMoveTo.column] = map[current.row][current.column];
            map[current.row][current.column] = '.';
        }
        else
        {
            switch (map[current.row][current.column])
            {
                case '[':
                    DoMovePart2(new Point(current.row, current.column + 1), move, ref map, true);
                    break;
                case ']':
                    DoMovePart2(new Point(current.row, current.column - 1), move, ref map, true);
                    break;
            }

            map[toMoveTo.row][toMoveTo.column] = map[current.row][current.column];
            map[current.row][current.column] = '.';
        }
    }

    static char[][] expandMap(char[][] map)
    {
        var map2 = new List<List<char>>();
        for (int i = 0; i < map.Length; i++)
        {
            map2.Add(new List<char>());
            for (int n = 0; n < map[i].Length; n++)
            {
                switch (map[i][n])
                {
                    case '.':
                        map2[i].Add('.');
                        map2[i].Add('.');
                        break;
                    case '#':
                        map2[i].Add('#');
                        map2[i].Add('#');
                        break;
                    case 'O':
                        map2[i].Add('[');
                        map2[i].Add(']');
                        break;
                    case '@':
                        map2[i].Add('@');
                        map2[i].Add('.');
                        break;
                }
            }
        }
        return map2.Select(c => c.ToArray()).ToArray();
    }

    static (char[][] map, char[] directions) parseInput(string[] input)
    {
        var map = new List<char[]>();

        var counter = 0;
        while (input[counter].Length > 0)
        {
            map.Add(input[counter].ToCharArray());
            counter++;
        }

        var dirBuilder = new StringBuilder();
        for (int i = counter + 1; i < input.Length; i++)
        {
            dirBuilder.Append(input[i]);
        }

        return (map.ToArray(), dirBuilder.ToString().ToCharArray());
    }

    static void Print(char[][] map)
    {
        Console.WriteLine();
        for (int i = 0; i < map.Length; i++)
        {
            for (int n = 0; n < map[i].Length; n++)
            {
                Console.Write(map[i][n]);
            }
            Console.WriteLine();
        }
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
