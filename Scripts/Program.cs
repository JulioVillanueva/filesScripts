// See https://aka.ms/new-console-template for more information


using Scripts;
using System.Diagnostics.Contracts;

bool Exit;
do
{
    Console.WriteLine("Enter Path From:");
    var PathFrom = Console.ReadLine();
    while (!Directory.Exists(PathFrom))
    {
        Console.WriteLine($"Path From: {PathFrom}");
        Console.WriteLine("Does not exist, Try again:");
        PathFrom = Console.ReadLine();
    }
    Console.WriteLine("Enter Path To");
    var PathTo = Console.ReadLine();
    while (!Directory.Exists(PathTo))
    {
        Console.WriteLine($"Path To: {PathTo}");
        Console.WriteLine("Does not exist, Try again:");
        PathTo = Console.ReadLine();
    }
    Console.WriteLine($"Path From: {PathFrom}");
    Console.WriteLine($"Path To: {PathTo}");
    Console.WriteLine($"Replacing: {PathTo}");
    var fManager = new FileManager(PathFrom, PathTo);
    fManager.ReplaceFiles();
    foreach (var file in fManager.CopiedFiles)
    {
        Console.WriteLine($"Status: {file.OutputMsg} \n From: {file.NewFile.FullName} \n To: {file.OldFile.FullName} \n");
    }
    Console.Write($"Exit?(y/n):");
    Exit = Console.Read() == char.GetNumericValue('y') || Console.Read() == char.GetNumericValue('Y');
}
while(!Exit);


