using System;
using System.Threading.Tasks;

namespace IsDownBot.Services
{
    public interface IDownDetector
    {
        Task<string> GetUrlStatus(string url, TimeSpan timeout);
    }
}
