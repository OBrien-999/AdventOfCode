using System.Collections.Generic;
using System.Linq;

FileSystem fileSystem = new FileSystem(System.IO.File.ReadAllText(@"./day-07-input.txt"));
Console.WriteLine(fileSystem.FindDirectories());

public class FileSystem
{
    private class Directory
    {
        public string Name { get; }
        public List<Directory> Directories { get; }
        public List<File> Files { get; }
        public Directory(string name)
        {
            Name = name;
            Directories = new List<Directory>();
            Files = new List<File>();
        }
        public void AddDirectory(Directory directory)
        {
            Directories.Add(directory);
        }
        public void AddFile(File file)
        {
            Files.Add(file);
        }
        public Directory GetDirectory(string name)
        {
            return Directories.FirstOrDefault(d => d.Name == name);
        }
    }
    private class File
    {
        public string Name { get; }
        public int Size { get; }
        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
    private readonly Directory root;
    public FileSystem(string input)
    {
        root = ParseInput(input);
    }
    public int FindDirectories()
    {
        int totalSum = 0;
        foreach (var directory in root.Directories)
        {
            ComputeTotalSize(directory, ref totalSum);
        }
        return totalSum;
    }
    private int ComputeTotalSize(Directory directory, ref int totalSum)
    {
        int dirSize = 0;
        foreach (var file in directory.Files)
        {
            dirSize += file.Size;
        }

        foreach (var subdirectory in directory.Directories)
        {
            var subDirSize = ComputeTotalSize(subdirectory, ref totalSum);
            dirSize += subDirSize;
        }

        if(dirSize <= 100000)
            totalSum += dirSize;

        return dirSize;
    }
    private Directory ParseInput(string input)
    {
        var lines = input.Split("\n");
        var root = new Directory("/");
        var stack = new Stack<Directory>();
        stack.Push(root);
        foreach (var line in lines)
        {
            if (line.StartsWith("$"))
            {
                var command = line[1..].Trim();
                if (command.StartsWith("cd"))
                {
                    var directoryName = command[3..].Trim();
                    if (directoryName == "/")
                    {
                        stack.Clear();
                        stack.Push(root);
                    }
                    else if (directoryName == "..")
                    {
                        stack.Pop();
                    }
                    else
                    {
                        var directory = stack.Peek().GetDirectory(directoryName);
                        stack.Push(directory);
                    }
                }
                else if (command == "ls")
                {
                    // List the contents of the current directory.
                }
            }
            else
            {
                var tokens = line.Split();
                if (tokens[0] == "dir")
                {
                    var directoryName = tokens[1];
                    var directory = new Directory(directoryName);
                    stack.Peek().AddDirectory(directory);
                }
                else
                {
                    var fileSize = int.Parse(tokens[0]);
                    var fileName = tokens[1];
                    var file = new File(fileName, fileSize);
                    stack.Peek().AddFile(file);
                }
            }
        }
        return root;
    }
}
