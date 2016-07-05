using System;

namespace ProAbono.Internal
{
    internal static class Guard
    {
        public static void NotNull(object parameter, string parameterName)
        {
            if(parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void NotNullOrEmpty(string parameter, string parameterName)
        {
            if(parameter == null)
                throw new ArgumentNullException(parameterName);
            if(parameter == string.Empty)
                throw new ArgumentException(string.Format("Parameter {0} is empty.", parameterName), parameterName);
        }

        public static void InRange(int parameter, int min, int max, string parameterName)
        {
            if(parameter < min || parameter > max)
                throw new ArgumentOutOfRangeException(parameterName, parameter, string.Format("Parameter {0} is out of range (must be between {1} and {2})", parameterName, min, max));
        }

        public static void LengthBetween(string parameter, int min, int max, string parameterName)
        {
            NotNull(parameter, parameterName);
            if(parameter.Length < min || parameter.Length > max)
                throw new ArgumentOutOfRangeException(parameterName, parameter.Length, string.Format("Length of string parameter {0} is out of range (must be between {1} and {2})", parameterName, min, max));
        }

        public static void Positive(double parameter, string parameterName)
        {
            if(parameter <= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, "Parameter {0} must be a positive number");
        }

        public static void Future(DateTime parameter, string parameterName)
        {
            if((parameter.Kind == DateTimeKind.Utc && parameter <= DateTime.UtcNow) || (parameter.Kind != DateTimeKind.Utc && parameter <= DateTime.Now))
                throw new ArgumentOutOfRangeException(parameterName, parameter, "Parameter {0} must be a future datetime");
        }
    }
}