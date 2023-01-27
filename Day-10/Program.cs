using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;


// iterate over each input instruction. noop cycle = 0, register = 1
    // if we encounter a noop instruction set the instructionCycle count to 1.   instructionCycle = 1
    // else set instructionCycle count to 2. (addx). 
    // loop for each insructionCycle count.   
        // increment totalCycle counter. totalCycle =  1
        // store value of register at this cycle. register = 1
    // process instruction. totalCycle=1, register = 1

TestSampleInput();

string[] instructions = System.IO.File.ReadAllLines(@"./day-10-input.txt");
Console.WriteLine(CalculateSumOfSignalStrengths(instructions, 6));

void TestSampleInput()
{
    string[] instructions = System.IO.File.ReadAllLines(@"./day-10-sample-input.txt");
    var expectedSumOfSignalStrengths = 13140;

    var sumOfSignalStrengths = CalculateSumOfSignalStrengths(instructions, 6);

    Assert.AreEqual(expectedSumOfSignalStrengths, sumOfSignalStrengths);
}

int CalculateSumOfSignalStrengths(string[] inputInstructions, int numberOfSignalStrengths)
{
    Dictionary<int, int> cycles = new Dictionary<int, int>();
    int register = 1;
    int totalCycles = 0;

    foreach(var inputInstruction in inputInstructions)
    {
        var instruction = ParseInputInstruction(inputInstruction);

        foreach(var cycle in Enumerable.Range(0, instruction.CycleLength)) 
        {
            totalCycles++;
            cycles.Add(totalCycles, register);
        }
        register += instruction.Value;
    }

    var sum = 0;

    for (var i = 20; i <= 220; i+=40)
    {
        sum += cycles[i]*i;
    }

    return sum;
}

Instruction ParseInputInstruction(string inputInstruction)
{
    string pattern = @"(addx|noop)\s?(-?\d+)?";
    MatchCollection matches = Regex.Matches(inputInstruction, pattern);

    var operation = matches[0].Groups[1].Value;
    int value = operation == "noop" ? 0 : Int32.Parse(matches[0].Groups[2].Value);
    return new Instruction(operation, value);
}

public class Instruction
{
    public readonly string Operation;
    public readonly int CycleLength;
    public readonly int Value;  

    public Instruction(string operation, int value)
    {
        Operation = operation;
        Value = value;

        if(operation.Equals("noop"))
            CycleLength = 1;          
        else if (operation.Equals("addx"))
            CycleLength = 2;
        else
            throw new Exception("unsupported operation");                    
    }
}

