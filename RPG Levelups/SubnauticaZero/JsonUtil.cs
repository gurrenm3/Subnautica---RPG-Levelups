using Newtonsoft.Json;

namespace RPG_Levelups
{
    internal partial class JsonUtil
    {
        public static string Serialize<T>(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
