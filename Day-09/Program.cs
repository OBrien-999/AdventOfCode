using Microsoft.VisualStudio.TestTools.UnitTesting;

TestSampleInput();

void TestSampleInput()
{
    string[] inputMotions = System.IO.File.ReadAllLines(@"./day-09-sample-input.txt");
    var expectedPositions = 13;

    var totalPositions = CalculateNumberOfPositions(inputMotions);

    Assert.AreEqual(expectedPositions, totalPositions);
}

int CalculateNumberOfPositions(string[] motions)
{
    // Problem summary:
    // Process each motion in the input
        // First motion (R4 - Right 4 positions): 
        // Move head 1 step at a time until the motion is complete
        // After each step check the distance from the tail if distance is greater than 1
        // Move the tail 

    // Solution summary:
    // Keep the coordinates in a knot class for the head and tail
    // Use coordinate mathematics to calculate the distance between the head and tail
    // Everytime we move the tail to a coordinate we'll add that to a hashset
    // Return the count of the hashset

    return 0;
}

