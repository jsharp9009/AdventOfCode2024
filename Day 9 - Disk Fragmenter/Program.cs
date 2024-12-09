using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiskFragmenter;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt").Select(c => int.Parse(c.ToString())).ToArray();

        var freeSpace = new List<MyFile>();
        var files = new List<MyFile>();
        var curIndex = 0;
        var fileIndex = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 != 0)
            {
                if (input[i] > 0)
                    freeSpace.Add(new MyFile(-1, curIndex, input[i]));
                else
                    continue;

            }
            else
            {
                files.Add(new MyFile(fileIndex, curIndex, input[i]));
                fileIndex++;
            }
            curIndex += input[i];
        }
        freeSpace.Add(new MyFile(-1, curIndex - 1, 0));

        var freeSpacePart1 = freeSpace.Select(f => new MyFile(f.id, f.start, f.length)).ToList();
        var aFiles = files.OrderByDescending(f => f.id).ToArray();
        
        var checksum = CompressPart1(aFiles, freeSpacePart1);
        Console.WriteLine("Part 1: {0}", checksum);

        checksum = CompressPart2(aFiles, freeSpace);
        Console.WriteLine("Part 2: {0}", checksum);
        
    }

    static long CompressPart1(MyFile[] files, List<MyFile> freeSpace){
        var curFreeSpace = freeSpace[0];
        freeSpace.RemoveAt(0);
        var curIndex = curFreeSpace.start;
        var checksum = 0L;
        var lastFree = freeSpace.Last();
        foreach (var file in files)
        {
            if (file.start < curIndex)
            {

                for (int i = 0; i < file.length; i++)
                {
                    checksum += (file.start + i) * file.id;
                }
                continue;
            }
            else
            {
                for (int i = 0; i < file.length; i++)
                {

                    if (curIndex >= curFreeSpace.end)
                    {
                        curFreeSpace = freeSpace[0];
                        freeSpace.RemoveAt(0);
                        curIndex = curFreeSpace.start;
                    }
                    checksum += curIndex * file.id;
                    curIndex++;
                    lastFree.start--;
                    lastFree.length++;

                    var overlap = freeSpace.FirstOrDefault(f => f.end == lastFree.start && f != lastFree);
                    if (overlap != null)
                    {
                        lastFree.start = overlap.start;
                        lastFree.length = overlap.length + lastFree.length;
                        freeSpace.Remove(overlap);
                    }
                }
            }
        }

       return checksum;
    }

    static long CompressPart2(MyFile[] files, List<MyFile> freeSpace){
        var checksum = 0L;
        foreach (var file in files)
        {
            var newSpace = freeSpace.Where(f => f.start < file.start && f.length >= file.length).OrderBy(f => f.start).FirstOrDefault();
            if (newSpace == null){
                for(int i = 0; i < file.length; i++){
                    checksum += file.id * (file.start + i);
                }
                continue;
            };

            for(int i = 0; i < file.length; i++){
                    checksum += file.id * (newSpace.start + i);
            }

            newSpace.start += file.length;
            newSpace.length -= file.length;
            if(newSpace.length == 0){ freeSpace.Remove(newSpace); }
        }

       return checksum;
    }
}

class MyFile
{
    public int id;
    public int start;
    public int length;

    public int end
    {
        get { return start + length; }
    }

    public MyFile(int id, int start, int length)
    {
        this.id = id;
        this.start = start;
        this.length = length;
    }
}