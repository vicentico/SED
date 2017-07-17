using Newtonsoft.Json.Linq;

namespace Template.Engine.Data.Models
{
    public class JsonOutPut
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public JObject Data { get; set; }
    }
}