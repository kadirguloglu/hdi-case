using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        FieldInfo? fi = value.GetType().GetField(value.ToString());

        if (fi != null)
        {
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }
        return value.ToString();
    }

    public static T? GetEnumAttribute<T>(this Enum value)
        where T : Attribute
    {
        FieldInfo? fi = value.GetType().GetField(value.ToString());

        if (fi != null)
        {
            T[] attributes =
                (T[])fi.GetCustomAttributes(
                typeof(T),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0];
            }
        }
        return null;
    }

    public static List<string> GetEnumDescriptions<TEnum>()
        where TEnum : Enum
    {
        List<string> descriptions = new List<string>();
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            descriptions.Add(value.GetEnumDescription());
        }
        return descriptions;
    }

    public static List<T> GetEnumAttributes<T, TEnum>()
        where TEnum : Enum
        where T : Attribute
    {
        List<T> attributes = new List<T>();
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            var attribute = value.GetEnumAttribute<T>();
            if (attribute != null)
            {
                attributes.Add(attribute);
            }
        }
        return attributes;
    }

    public static List<EnumToKeyValueList> GetEnumToKeyValueList(Type enumType)
    {
        List<EnumToKeyValueList> list = new List<EnumToKeyValueList>();
        foreach (Enum value in Enum.GetValues(enumType))
        {
            list.Add(new EnumToKeyValueList
            {
                Key = value.GetEnumDescription(),
                Value = (int)Enum.Parse(enumType, value.ToString()),
                Original = Enum.GetName(enumType, value)
            });
        }
        return list;
    }
}

public class EnumToKeyValueList
{
    public string? Key { get; set; }
    public int Value { get; set; }
    public string? Original { get; set; }
}