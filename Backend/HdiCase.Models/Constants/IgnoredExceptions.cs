using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog.Events;

public static class IgnoredExceptions
{
    private static List<string> IgnoredExceptionTexts = new List<string>()
    {

    };
    public static bool Ignore(LogEvent c)
    {
        try
        {
            foreach (var item in IgnoredExceptionTexts)
            {
                if (c.Properties.ContainsKey("SourceContext"))
                {
                    var isExist = c.Properties["SourceContext"]?.ToString()?.Contains(item) ?? false;
                    if (isExist)
                    {
                        return true;
                    }
                }
                if (c.Properties.ContainsKey("Scope"))
                {
                    var isExist = c.Properties["Scope"]?.ToString()?.Contains(item) ?? false;
                    if (isExist)
                    {
                        return true;
                    }
                }
                if (c.Properties.ContainsKey("RequestPath"))
                {
                    var isExist = c.Properties["RequestPath"]?.ToString()?.Contains(item) ?? false;
                    if (isExist)
                    {
                        return true;
                    }
                }
                if (!string.IsNullOrEmpty(c.MessageTemplate?.Text))
                {
                    var isExist = c.MessageTemplate.Text.Contains(item);
                    if (isExist)
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public static void ConsoleLogException(this Exception ex, string title = "", object? data1 = null, object? data2 = null)
    {
        if (EnvironmentSettings.IsDevelopment)
        {
            string dataText1 = "";
            if (data1 != null)
            {
                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters =
                    {
                        new IgnoreUnsupportedTypesConverter()
                    },
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };
                dataText1 = JsonSerializer.Serialize(data1, options);
            }
            string dataText2 = "";
            if (data2 != null)
            {
                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters =
                    {
                        new IgnoreUnsupportedTypesConverter()
                    },
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };
                dataText2 = JsonSerializer.Serialize(data2, options);
            }
            Console.WriteLine(title + " _ Ex Message = " + ex.Message + " _ Data1 = " + dataText1 + " _ Data2 = " + dataText2);
            Console.WriteLine(title + " _ Ex InnerException Message = " + (ex.InnerException?.Message ?? "Inner exception not found"));
        }
    }
}