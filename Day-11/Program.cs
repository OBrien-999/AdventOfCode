using Day_11;

var numberOfRounds = args.Count() > 0 ? Int32.Parse(args[0]) : 20;
var inputFilePath = @"./input.txt";
var service = new Service(inputFilePath);

Console.WriteLine(service.CalculateLevelOfMonkeyBusiness(numberOfRounds));
