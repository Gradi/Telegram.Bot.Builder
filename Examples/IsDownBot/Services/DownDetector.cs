using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IsDownBot.Services
{
    public class DownDetector : IDownDetector
    {
        public async Task<string> GetUrlStatus(string url, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                using var client = new HttpClient();
                client.Timeout = timeout;
                using var response = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead);

                var responseString = new StringBuilder()
                    .Append("Response time: ").Append(stopwatch.Elapsed).AppendLine()
                    .Append((int)response.StatusCode).Append(' ').Append(response.StatusCode.ToString()).AppendLine();

                foreach (var (k, v) in response.Headers)
                {
                    responseString.Append(k).Append(": ").Append(string.Join(", ", v)).AppendLine();
                }

                return responseString.ToString();
            }
            catch(TimeoutException)
            {
                return $"Timeout after {stopwatch.Elapsed}";
            }
            catch(TaskCanceledException)
            {
                return $"Timeout after {stopwatch.Elapsed}";
            }
            catch(Exception exception)
            {
                return $"General error after {stopwatch.Elapsed}: {exception.Message}";
            }
        }
    }
}
