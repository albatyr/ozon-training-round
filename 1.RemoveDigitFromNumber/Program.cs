using System.Text;

using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

const int min = 1;
const int max = 100000;

var line = input.ReadLine();
if (string.IsNullOrWhiteSpace(line))
    return;

var testsCount = int.Parse(line);
if (testsCount is < min or > max)
    return;

var linesTotalLength = 0;

for (var i = 0; i < testsCount; i++)
{
    line = input.ReadLine();
    
    if (string.IsNullOrWhiteSpace(line))
        return;

    linesTotalLength += line.Length;
    if (linesTotalLength > max)
        return;
    
    var adjustedSalary = RemoveLowestImpactDigit(line);
    
    output.WriteLine(adjustedSalary);
}

string RemoveLowestImpactDigit(string salary)
{
    if (salary.Length == 1)
        return "0";

    var builder = new StringBuilder(salary.Length - 1);
    var removed = false;

    for (var i = 0; i < salary.Length; i++)
    {
        if (!removed && i < salary.Length - 1 && salary[i] < salary[i + 1])
        {
            removed = true;
            continue;
        }
        
        builder.Append(salary[i]);
    }

    if (!removed)
        builder.Length--;

    return builder.ToString();
}