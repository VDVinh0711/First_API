using Newtonsoft.Json;

namespace API_EF.Helper
{
    public static class Extentions
    {
        public static T To<T>(this object input)
        {
            if (input == null)
            {
                return default;
            }
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public static T Deserialize<T>(this string input)
        {
            if (input == null)
            {
                return default;
            }

            return (T)JsonConvert.DeserializeObject<T>(input);
        }

        public static string Serialize(this object input)
        {
            if (input == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(input);
        }
    }
}
