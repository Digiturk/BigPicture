using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace BigPicture.Core.Config
{
    public class CommonConfig
    {
        public static CommonConfig _Instance = null;
        public static CommonConfig Instance
        {
            get
            {
                if(_Instance == null)
                {
                    var path = @"config.json";
                    if (HttpContext.Current != null)
                    {
                        path = HttpContext.Current.Server.MapPath(Path.Combine("~/" + path));
                    }

                    _Instance = JsonConvert.DeserializeObject<CommonConfig>(File.ReadAllText(path));
                }

                return _Instance;
            }
        }

        public String GraphRepository { get; set; }        
        public String DocumentRepository { get; set; }

        public Dictionary<String, String> Options { get; set; }
    }   
}
