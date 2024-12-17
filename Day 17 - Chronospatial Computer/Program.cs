using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChronospatialComputer;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        (List<long> program, Machine machine) = ParseInput(input);
        var output = machine.Copy().RunProgram(program);
        Console.WriteLine("Part 1: {0}", string.Join(",", output.Select(s => s.ToString())));

        var regA = 1L;
        while (true)
        {
            var run = machine.Copy();
            run.RegisterA = regA;
            output = run.RunProgram(program);

            bool match = program[(program.Count - output.Count)..]
                     .Select((x, i) => output[i] == x)
                     .All(b => b);

            if (match && output.Count == program.Count)
            {
                Console.WriteLine("Part 2: {0}", regA);
                break;
            }

            if (match) regA = (regA * 8) - 1;
            else regA += 1;
        }
    }



    static (List<long> program, Machine machine) ParseInput(string input)
    {
        var regex = new Regex("[0-9]+");
        var program = new List<long>();
        var matches = regex.Matches(input);
        var machine = new Machine();
        for (int i = 0; i < matches.Count; i++)
        {
            if (i == 0) machine.RegisterA = long.Parse(matches[i].Value);
            else if (i == 1) machine.RegisterB = long.Parse(matches[i].Value);
            else if (i == 2) machine.RegisterC = long.Parse(matches[i].Value);
            else program.Add(long.Parse(matches[i].Value));
        }
        return (program, machine);
    }
}

class Machine()
{
    public long RegisterA { get; set; }
    public long RegisterB { get; set; }
    public long RegisterC { get; set; }
    public List<long> RunProgram(List<long> program)
    {
        var output = new List<long>();
        for (int i = 0; i < program.Count; i += 2)
        {
            var value = program[i + 1];
            switch (program[i])
            {
                case 0:
                    RegisterA = (long)(RegisterA / Math.Pow(2, ComboOperator(value)));
                    break;
                case 1:
                    RegisterB ^= value;
                    break;
                case 2:
                    RegisterB = mod(ComboOperator(value), 8);
                    break;
                case 3:
                    if (RegisterA != 0)
                        i = (int)value - 2;
                    break;
                case 4:
                    RegisterB ^= RegisterC;
                    break;
                case 5:
                    output.Add(mod(ComboOperator(value), 8));
                    break;
                case 6:
                    RegisterB = (long)(RegisterA / Math.Pow(2, ComboOperator(value)));
                    break;
                case 7:
                    RegisterC = (long)(RegisterA / Math.Pow(2, ComboOperator(value)));
                    break;
            }
        }
        return output;
    }

    long ComboOperator(long c)
    {
        switch (c)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return c;
            case 4:
                return RegisterA;
            case 5:
                return RegisterB;
            case 6:
                return RegisterC;
            case 7:
                throw new NotImplementedException();
        }
        return -1;
    }

    public Machine Copy()
    {
        return new Machine() { RegisterA = this.RegisterA, RegisterB = this.RegisterB, RegisterC = this.RegisterC };
    }

    static long mod(long x, long m)
    {
        return (long)(x - m * Math.Floor(x / (double)m));
    }
}
