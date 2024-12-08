using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResonantCollinearity;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var map = ParseInput(input);
        var antiNodes = GetAntiNodesPart1(map, input.Length, input[0].Length);
        Console.WriteLine("Part 1: {0}", antiNodes.Count);
        antiNodes = GetAntiNodesPart2(map, input.Length, input[0].Length);
        
        Console.WriteLine("Part 2: {0}", antiNodes.Count);
    }

    static HashSet<Point> GetAntiNodesPart1(Dictionary<char, List<Point>> map, int rows, int columns){
        HashSet<Point> antiNodes = new HashSet<Point>();
        foreach(var freq in map.Values){
            foreach(var p1 in freq){
                foreach (var p2 in freq) {
                    if (p1 == p2) continue;
                    var distance = p1 - p2;
                    var test = p1 + distance;
                    if (test.row >= 0 && test.row < rows && test.column >= 0 && test.column < columns)
                        antiNodes.Add(test);
                    test = p2 - distance;
                    if (test.row >= 0 && test.row < rows && test.column >= 0 && test.column < columns)
                        antiNodes.Add(test);
                }
            }
        }
        return antiNodes;
    }

    static HashSet<Point> GetAntiNodesPart2(Dictionary<char, List<Point>> map, int rows, int columns){
        HashSet<Point> antiNodes = new HashSet<Point>();
        foreach(var freq in map.Values){
            freq.ForEach(f => antiNodes.Add(f));
            foreach(var p1 in freq){
                foreach (var p2 in freq) {
                    if (p1 == p2) continue;
                    var distance = p1 - p2;
                    var test = p1;
                    while (true)
                    {
                        test = test + distance;
                        if (test.row >= 0 && test.row < rows && test.column >= 0 && test.column < columns)
                            antiNodes.Add(test);
                        else
                            break;
                    }
                    test = p2;
                    while (true)
                    {
                        test = test - distance;
                        if (test.row >= 0 && test.row < rows && test.column >= 0 && test.column < columns)
                            antiNodes.Add(test);
                        else break;
                    }
                }
            }
        }
        return antiNodes;
    }

    static Dictionary<char, List<Point>> ParseInput(string[] input){
        var map = new Dictionary<char,List<Point>>();
        for(int row = 0; row < input.Length; row++){
            for (int column = 0; column < input[row].Length; column++){
                if (input[row][column] != '.')
                {
                    var c = input[row][column];
                    if(map.ContainsKey(c)){
                        map[c].Add(new Point(row,column));
                    }
                    else
                    {
                        map.Add(c, new List<Point>(){new Point(row,column)});
                    }
                }
            }
        }
        return map;
    }
}

record Point(int row, int column){
    public static Point operator +(Point p1, Point p2){
        return new Point(p1.row + p2.row, p1.column + p2.column);
    }

    public static Point operator -(Point p1, Point p2){
        return new Point(p1.row - p2.row, p1.column - p2.column);
    }
}

