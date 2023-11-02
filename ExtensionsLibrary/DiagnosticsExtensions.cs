namespace ExtensionsLibrary
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class DiagnosticsExtensions
    {


        /// <summary>
        /// Gets the name of the executing class and method.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Executing Class And Method Name</returns>
        public static string GetExecutingClassAndMethodName(this object @class, [CallerMemberName] string methodName = "")
        {
            return $"{@class.GetType().Name}.{methodName}()";
        }

        /// <summary>
        /// GetExecutingClassName
        /// </summary>
        /// <param name="class">class</param>
        /// <returns>string</returns>
        public static string GetExecutingClassName(this object @class)
        {
            return @class.GetType().Name;
        }

        /// <summary>
        /// GetExecutingMethodName
        /// </summary>
        /// <param name="_">class</param>
        /// <param name="methodName">methodName</param>
        /// <returns>string</returns>
        public static string GetExecutingMethodName(this object _, [CallerMemberName] string methodName = "")
        {
            return $"{methodName}()";
        }

        /// <summary>
        /// GetExecutingMethodName
        /// </summary>
        /// <param name="methodName">methodName</param>
        /// <returns></returns>
        public static string GetExecutingMethodName([CallerMemberName] string methodName = "")
        {
            return $"{methodName}()";
        }

        /// <summary>
        /// Gets the executing paths.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>string namespaceName, string className, string classWithNamespace, string methodName</returns>
        public static (string namespaceName, string className, string classWithNamespace, string methodName) GetExecutingPaths(this object @class, [CallerMemberName] string methodName = "")
        {
            Type t = @class.GetType();
            return (t.Namespace, t.Name, t.FullName, methodName);
        }

        /// <summary>
        /// Gets the executing method path.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Full path to executing method</returns>
        public static string GetExecutingMethodPath(this object @class, [CallerMemberName] string methodName = "")
        {
            var paths = @class.GetExecutingPaths(methodName);
            return $"{paths.classWithNamespace}.{methodName}";
        }

        /// <summary>
        /// To the milliseconds.
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <returns>Milliseconds string</returns>
        public static string ToMilliseconds(this Stopwatch stopwatch)
        {
            return $"{stopwatch.ElapsedMilliseconds.ToString(@"n0")} ms";
        }
    }
}
