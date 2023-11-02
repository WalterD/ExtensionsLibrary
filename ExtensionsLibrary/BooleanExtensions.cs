namespace ExtensionsLibrary
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool? booleanValue)
        {
            return ToYesNoString(booleanValue.HasValue && booleanValue.Value);
        }

        public static string ToYesNoString(this bool booleanValue)
        {
            return booleanValue ? "Yes" : "No";
        }

        public static bool ToBoolean(this bool? nullableBoolean, bool defaultValue = default)
        {
            return nullableBoolean ?? defaultValue;
        }
    }
}