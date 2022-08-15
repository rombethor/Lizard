using System.Security.Cryptography;
using System.Text;

namespace Lizard
{
    /// <summary>
    /// A SHA256 hash is generated for each log entry for ease of comparison.
    /// Thus, if the same exception occurrs multiple times, we can just log the occurrences and link them
    /// to the same error data.
    /// 
    /// Although it is possible to generate two equal hashes for different data,
    /// the author considers it unlikely enough to ignore.
    /// </summary>
    public static class LogEntryHash
    {
        public static byte[] Compute(Models.LogEntryAddOptions options)
        {
            string method = options.Exception?.StackTrace?.MethodName ?? string.Empty;
            string stack = options.Exception?.StackTrace?.Content ?? string.Empty;
            string data = $"{options.Message}|{options.Source.Name}|{options.Source.Version}|{method}|{stack}";
            return SHA1.HashData(Encoding.UTF8.GetBytes(data));
        }
    }
}
