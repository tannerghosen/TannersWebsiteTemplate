using System.Diagnostics;

namespace TannersWebsiteTemplate
{
    /*
     * Methods: Write
     * Properties/Misc: Log
     */
    /// <summary>
    /// This class handles logging for various classes throughout the program, including a stacktrace output in case we have an error. 
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Our log file
        /// </summary>
        public static string Log = "./TWT.log";

        /// <summary>
        /// Writes a message to our log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messagetype"></param>
        public static async Task Write(string message, string messagetype = "LOG")
        {
            string Time = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt");
            StackTrace st = new StackTrace(); // Create a stack trace
            StackFrame parentsf = st.GetFrame(1); // this is the parent of the method call
            StackFrame grandparentsf = st.GetFrame(2); // this is the grandparent (parent's parent) of the method call
            using (StreamWriter writer = new StreamWriter(Log, true))
            {
                await writer.WriteLineAsync("(" + Time + ") [" + messagetype + "]: " + message);
                if (messagetype == "ERROR" || messagetype == "DEBUG") // if error, let's help out by giving the stack trace
                {
                    if (messagetype == "ERROR") await Statistics.IncrementErrors();
                    string stack = grandparentsf != null ? grandparentsf.GetMethod().Name + " -> " + parentsf.GetMethod().Name : parentsf.GetMethod().Name; // this is a string that says Grandparent -> Parent
                    await writer.WriteLineAsync("(" + Time + ") [" + messagetype + "]: The stack is as follows: " + stack + ".");
                }
                writer.Close();
            }
        }
    }
}
