using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                    _Instance = JsonConvert.DeserializeObject<CommonConfig>(File.ReadAllText(@"config.json"));
                }

                return _Instance;
            }
        }

        public String Repository { get; set; }        
    }   
}
