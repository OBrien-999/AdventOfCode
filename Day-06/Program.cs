var signal = System.IO.File.ReadAllText(@"./day-06-input.txt");
var packetSize = args.Count() > 0 ?  Int32.Parse(args[0]) : 4;

var dataLength = signal.Length;
var startCharacterPosition = 0;
var endCharacterPosition = 0;

HashSet<char> packet = new HashSet<char>();
for(; endCharacterPosition < dataLength; endCharacterPosition++)
{
    if(!packet.Add(signal[endCharacterPosition]))
    {
        startCharacterPosition++;
        endCharacterPosition = (startCharacterPosition - 1);

        packet.Clear();
        continue;
    }
  
    if((endCharacterPosition - startCharacterPosition + 1) == packetSize)
        break;
}

Console.WriteLine(endCharacterPosition + 1);