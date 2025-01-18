using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

const int min = 1;
const int max = 100000;

if (!int.TryParse(input.ReadLine(), out var testsCount))
    return;

if (testsCount is < min or > max)
    return;

for (var i = 0; i < testsCount; i++)
{
    var line1 = input.ReadLine();
    var line2 = input.ReadLine();
    var line3 = input.ReadLine();

    var valid = ValidateTest(line1, line2, line3);
    
    output.WriteLine(valid ? "YES" : "NO");
}

bool ValidateTest(string? line1, string? line2, string? line3)
{
    if (string.IsNullOrWhiteSpace(line1) || 
        string.IsNullOrWhiteSpace(line2) ||
        string.IsNullOrWhiteSpace(line3))
    {
        return false;
    }

    if (!int.TryParse(line1, out var numbersCount))
        return false;
    
    if (numbersCount is < min or > max)
        return false;
    
    var numbers = new List<int>();
    
    foreach (var s in line2.Split(' '))
    {
        if (!int.TryParse(s, out var parsedValue))
            return false;
        
        numbers.Add(parsedValue);
    }

    if (numbers.Count != numbersCount)
        return false;

    numbers.Sort();

    return line3.Equals(string.Join(" ", numbers));
}