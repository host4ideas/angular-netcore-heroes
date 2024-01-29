using Newtonsoft.Json;

namespace HeroesAPI.Helpers
{
    public class HelperJsonSession
    {
        public static T DeserializeObject<T>(string data)
        {
            T objeto = JsonConvert.DeserializeObject<T>(data)!;
            return objeto;
        }

        public static string SerializeObject(object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            return data;
        }
    }
}
