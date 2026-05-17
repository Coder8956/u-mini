using Newtonsoft.Json;

namespace UMiniFramework.Runtime.Modules.UMDataPer.Base
{
    public abstract class UMBaseDataPerObject
    {
        [JsonProperty] private string CreateTime = "Invalid";
        [JsonProperty] private string LastSaveTime = "Invalid";

        public string GetLastSaveTime()
        {
            return LastSaveTime;
        }

        public string GetCreateTime()
        {
            return CreateTime;
        }
    }
}