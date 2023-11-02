using System;

namespace ExtensionsLibrary
{
    public static class TaskExtensions
    {
        /// <summary>
        /// FireAndForget
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="continueOnCapturedContext">continueOnCapturedContext</param>
        /// <param name="onException">onException</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Pending>")]
        public static async void FireAndForget(this System.Threading.Tasks.Task task, bool continueOnCapturedContext, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
