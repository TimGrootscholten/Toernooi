using System.ComponentModel;

namespace Models;

public static class Extensions
{
    public static string ToCommaSeparateString(this List<int> list)
    {
        return list.Count > 0 ? string.Join(",", list.ConvertAll(x => x.ToString())) : string.Empty;
    }

    public static List<int> CommaSeparateStringToList(this string? str)
    {
        return !string.IsNullOrEmpty(str) ? str.Split(",").Select(int.Parse).ToList() : new List<int>();
    }
    
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var memInfo = type.GetMember(value.ToString());

        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        return value.ToString();
    }
}