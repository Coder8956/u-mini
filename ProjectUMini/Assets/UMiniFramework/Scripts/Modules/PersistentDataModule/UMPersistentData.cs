using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UMiniFramework.Scripts.Modules.PersistentDataModule
{
    public abstract class UMPersistentData
    {
        public string ToJson()
        {
            string jsonStr = JsonConvert.SerializeObject(this, Formatting.Indented);
            jsonStr = Regex.Unescape(jsonStr);
            return jsonStr;
        }
    }
}