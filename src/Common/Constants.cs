using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerialCaller.Libs.Common;

public static class Constants
{
    public const string NOTEPAD = "notepad.exe";

    public static readonly CultureInfo CULTURE_EN = new("en-US");
    public static readonly CultureInfo CULTURE_FR = new("fr-FR");

    public static readonly JsonSerializerOptions SERIALIZER_OPTIONS = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static readonly ParallelOptions PARALLEL_OPTIONS = new()
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount
    };
}
