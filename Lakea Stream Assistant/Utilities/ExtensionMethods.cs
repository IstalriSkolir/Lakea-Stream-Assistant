namespace Lakea_Stream_Assistant.Utilities
{
    public static  class ExtensionMethods
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
