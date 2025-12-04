using System.Reflection;

namespace SimpleStorageSystem.Shared.Services.Helper;

public static class ModelChecker
{
    public static bool PropertiesAreNullorWhiteSpace<T>(this T poco)
    {
        if (poco is null)
            throw new Exception();
        PropertyInfo[] properties = poco.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(poco);
            if (value is null)
                continue;
            if (value is string v && String.IsNullOrWhiteSpace(v))
                continue;
            return false;
        }
        return true;
    }
    public static bool AnyPropertyIsNullorWhiteSpace<T>(this T poco)
    {
        if (poco is null)
            throw new Exception();
        PropertyInfo[] properties = poco.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(poco);
            if (value is not null)
                continue;
            if (value is string v && !String.IsNullOrWhiteSpace(v))
                continue;
            return true;
        }
        return false;
    }
}