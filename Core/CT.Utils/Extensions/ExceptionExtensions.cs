using System;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Class to handle exception extensions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Flatten the exeption to a string
        /// </summary>
        /// <param name="exception">Object of exception</param>
        /// <returns>Returns flattened exception message</returns>
        public static string Flatten(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            System.Text.StringBuilder exceptionMessage = new System.Text.StringBuilder($"Exception: {exception.Message}\nTrace:{exception.StackTrace}\n");
            while (exception?.InnerException != null)
            {
                exception = exception.InnerException;
                exceptionMessage.Append($"\nInner Exception: {exception?.Message}\nTrace:{exception?.StackTrace}\n");
            }

            return exceptionMessage.ToString();
        }
    }
}