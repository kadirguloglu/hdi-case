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
}