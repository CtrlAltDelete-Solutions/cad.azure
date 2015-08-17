using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CAD.Azure
{
    public class AzureNoSQLDocument : INoSQLDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName="_self")]
        public string DocumentLink { get; set;}
        
    }
}
