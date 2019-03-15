
namespace System
{
    public static class Check
    {
        public static T NotNull<T>(this T obj, string parameterName) where T : class
        {
            if (obj.IsNull())
            {
                NotNull(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return obj;
        }

        public static bool IsNull<T>(this T obj)
            where T : class => obj is null;
    }
}
