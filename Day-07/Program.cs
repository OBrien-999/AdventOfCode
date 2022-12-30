using System.Collections.Generic;
using System.Linq;

FileSystem fileSystem = new FileSystem(System.IO.File.ReadAllText(@"./day-07-input.txt"));

var directorySizes = fileSystem.GetDirectorySizes();
var sumOfDirsLessThanThreshold = 
    directorySizes
        .Where( x => x.Value <= 100000)
        .Select(x => x.Value)
        .Sum();

const string RootDirectory = "/";
const int TotalAvailableDiskSpace = 70000000;
const int MinimumUnusedSpaceRequirement = 30000000;

var usedDiskSpace = directorySizes.GetValueOrDefault(RootDirectory);
var currentAvailableDiskSpace = TotalAvailableDiskSpace - usedDiskSpace;

var spaceToSetFree = MinimumUnusedSpaceRequirement - currentAvailableDiskSpace;
var smallestDirToDelete = 
    directorySizes
        .Where(x => (x.Value) >= spaceToSetFree)
        .OrderBy(x => x.Value)
        .FirstOrDefault();


Console.WriteLine($"Part 1: {sumOfDirsLessThanThreshold}");
Console.WriteLine($"Part 2: {smallestDirToDelete.Value}");


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
        public Directory? GetDirectory(string name)
        {
            return Directories?.FirstOrDefault(d => d.Name == name);
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

    public Dictionary<string, int> GetDirectorySizes()
    {
        var directorySizes = new Dictionary<string, int>();
        var rootDirectorySize = 0;

        foreach (var file in root.Files)
        {
            rootDirectorySize += file.Size;
        }

        foreach (var directory in root.Directories)
        {
            rootDirectorySize += GetDirectorySize(directory, directorySizes);
        }

        directorySizes.Add("/", rootDirectorySize);
        
        return directorySizes;
    }

    private int GetDirectorySize(Directory directory, Dictionary<string, int> dictionarySizes)
    {
        int dirSize = 0;
        Random rand = new Random();

        foreach (var file in directory.Files)
        {
            dirSize += file.Size;
        }

        foreach (var subdirectory in directory.Directories)
        {
            var subDirSize = GetDirectorySize(subdirectory, dictionarySizes);
            dirSize += subDirSize;
        }

        if (dictionarySizes.TryGetValue(directory.Name, out var existingDirectory))
            dictionarySizes.Add(directory.Name + Guid.NewGuid().ToString(), dirSize);
        else
            dictionarySizes.Add(directory.Name, dirSize);

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
