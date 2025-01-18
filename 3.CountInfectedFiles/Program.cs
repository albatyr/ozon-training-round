using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    if (!int.TryParse(input.ReadLine(), out var jsonLinesCount))
        return;

    var jsonBuilder = new StringBuilder();
    
    for (var j = 0; j < jsonLinesCount; j++)
    {
        jsonBuilder.Append(input.ReadLine());
    }

    var json = jsonBuilder.ToString();
    
    var folder = JsonSerializer.Deserialize<Folder>(json, new JsonSerializerOptions{MaxDepth = 1024});
    
    if (folder is null)
        return;
    
    var infectedFilesCount = CountInfectedFiles(folder);

    output.WriteLine(infectedFilesCount.ToString());
}

int CountInfectedFiles(Folder folder)
{
    if (folder.Files.Any(file => file.EndsWith(".hack", StringComparison.OrdinalIgnoreCase)))
    {
        return CountAllFiles(folder);
    }

    var infectedFilesCount = 0;
    
    foreach (var subFolder in folder.Folders)
    {
        infectedFilesCount += CountInfectedFiles(subFolder);
    }
    
    return infectedFilesCount;
}

int CountAllFiles(Folder folder)
{
    var filesCount = folder.Files.Count;
    
    foreach (var subFolder in folder.Folders)
    {
        filesCount += CountAllFiles(subFolder);
    }
    
    return filesCount;
}

class Folder
{
    [JsonPropertyName("files")]
    public List<string> Files { get; set; } = new List<string>();
    
    [JsonPropertyName("folders")]
    public List<Folder> Folders { get; set; } = new List<Folder>();
}