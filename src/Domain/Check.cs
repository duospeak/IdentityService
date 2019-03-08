﻿
namespace System
{
    public static class Check
    {
        public static T NotNull<T>(this T obj, string parameterName) where T : class
        {
            if (obj is null)
            {
                NotNull(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return obj;
        }
    }
}
