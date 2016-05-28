using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NodeMcuWixelMonitor
{
    public interface IDataDecoder
    {
        DataDecoderResult Decode(string data);
    }

    public class DataDecoder : IDataDecoder
    {
        public DataDecoderResult Decode(string data)
        {
            try
            {
                return DoDecode(data);
            }
            catch
            {
                return null;
            }
        }

        private static DataDecoderResult DoDecode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var splitData = data.Split('\n');
            var filteredData = splitData.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filteredData.Count == 0)
            {
                return null;
            }

            var nodeMcuInfo = JsonConvert.DeserializeObject<NodeMcuInfo>(filteredData.First());
            var packages = new List<Package>();
            for (var i = 1; i < filteredData.Count; ++i)
            {
                var package = JsonConvert.DeserializeObject<Package>(filteredData[i]);
                packages.Add(package);
            }

            var newestPackage = packages.OrderBy(x => x.RelativeTime).FirstOrDefault();
            var result = new DataDecoderResult { UpSince = DateTime.Now.AddSeconds(-nodeMcuInfo.Uptime)};
            if (newestPackage != null)
            {
                result.LastDataAvailableTime = DateTime.Now.AddMilliseconds(-newestPackage.RelativeTime);
            }

            return result;
        }
    }

    public class DataDecoderResult
    {
        public DateTime UpSince { get; set; }
        public DateTime? LastDataAvailableTime { get; set; }
    }
}
