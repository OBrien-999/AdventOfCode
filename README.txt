Create new console project:
* create a new directory for your project
* from within the new directory, run 
  $ dotnet new console --framework net6.0

Run console application
* from within the directory where the project file is located, run
  $ dotnet run
* or provide the path
  $ dotnet run --project Day-11\Day-11.csproj 

Create a new test project
* create a new directory 
* from within the new directory, run
  $ dotnet new mstest --framework net6.0

Run test application
* from within the directory where the project file is located, run
  $ dotnet test
* or provide the path
  $ dotnet test Day-11.Tests\Day-11.Tests.csproj

Add a project reference
* from within the directory containing the project you'd like to add a reference to, run
  $ dotnet add reference ..\Day-11\Day-11.csproj