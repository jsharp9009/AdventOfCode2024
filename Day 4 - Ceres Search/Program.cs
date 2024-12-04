using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CeresSearch;

class Program
{
    static readonly List<Tuple<int, int>> _DIRECTIONS = [
        new Tuple<int, int>(0,1),
        new Tuple<int, int>(1,0),
        new Tuple<int, int>(0,-1),
        new Tuple<int, int>(-1,0),
        new Tuple<int, int>(1,1),
        new Tuple<int, int>(1,-1),
        new Tuple<int, int>(-1,-1),
        new Tuple<int, int>(-1,1),
    ];

    static readonly List<Tuple<int, int>> _DIAGNALS = [
        new Tuple<int, int>(1,1),
        new Tuple<int, int>(1,-1)
    ];

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(c => c.ToCharArray()).ToArray();
        var xmasCount = CountXMAS(input);
        Console.WriteLine("Part 1: {0}", xmasCount);
        xmasCount = CountX_MAS(input);
        Console.WriteLine("Part 2: {0}", xmasCount);
    }

    static int CountXMAS(char[][] input){
        int xmasCount = 0;
        for (int y = 0; y < input.Length; y++)
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == 'X')
                {
                    foreach (var dir in _DIRECTIONS)
                    {
                        var checkX = x + dir.Item1;
                        var checkY = y + dir.Item2;
                        if (GetLetter(input, checkX, checkY) == 'M')
                        {
                            checkX += dir.Item1;
                            checkY += dir.Item2;
                            if (GetLetter(input, checkX, checkY) == 'A')
                            {
                                checkX += dir.Item1;
                                checkY += dir.Item2;
                                if (GetLetter(input, checkX, checkY) == 'S')
                                {
                                    xmasCount++;
                                }
                            }
                        }
                    }
                }
            }
            return xmasCount;
    }

    static int CountX_MAS(char[][] input){
        int xmasCount = 0;
        for (int y = 0; y < input.Length; y++)
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == 'A')
                {
                    var valid = true;
                    foreach (var dir in _DIAGNALS)
                    {
                        var checkX = x + dir.Item1;
                        var checkY = y + dir.Item2;
                        var next = GetLetter(input, checkX, checkY);
                        if (next == 'M')
                        {
                            checkX = x + (dir.Item1 * - 1);
                            checkY = y + (dir.Item2 * - 1);

                            if (GetLetter(input, checkX, checkY) != 'S') valid = false;
                        }
                        else if (next == 'S')
                        {
                            checkX = x + (dir.Item1 * - 1);
                            checkY = y + (dir.Item2 * - 1);

                            if (GetLetter(input, checkX, checkY) != 'M') valid = false;
                        }
                        else{
                            valid = false;
                            break;
                        }

                        if (!valid) break;
                    }
                    if (valid) xmasCount++;
                }
            }
            return xmasCount;
    }

    static char GetLetter(char[][] input, int x, int y){
        if (y < 0 || y >= input.Length || x < 0 || x >= input[y].Length) return ' ';
        return input[y][x];
    }
}
