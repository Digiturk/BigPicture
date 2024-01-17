using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BigPicture.Core.Config
{
    public class CacheServerConfig
    {
        public static CacheServerConfig _Instance = null;
        public static CacheServerConfig Instance
        {
            get
            {
                if (_Instance == null)
                {
                    var path = @"cache-server.json";
                    if (HttpContext.Current != null)
                    {
                        path = HttpContext.Current.Server.MapPath(Path.Combine("~/", path));
                    }
                    _Instance = JsonConvert.DeserializeObject<CacheServerConfig>(File.ReadAllText(path));
                }

                return _Instance;
            }
        }

        public List<string> RedisTableKeywords { get; set; }
        public List<RedisTableMap> RedisTableMap { get; set; }
    }

    public class RedisTableMap
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
