using System.Runtime.CompilerServices;

namespace BuildingBlocks.Utils
{
    public static class ArgumentNullThrowHelper
    {
        internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
        public static void ThrowIfNull(object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }
    }
}
