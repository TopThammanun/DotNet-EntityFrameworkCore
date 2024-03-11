namespace DotNet_EntityFrameworkCore.Core
{
    public class SessionData
    {
        static Func<IDictionary<object, object>> _getDicFunc;
        public static void Set(string key, object data)
        {
            if (_getDicFunc == null)
                throw new Exception("Must be configure first");
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            //if (data == null)
            //    throw new ArgumentNullException(nameof(data));
            _getDicFunc()[key] = data;
        }

        public static T Get<T>(string key)
        {
            if (_getDicFunc == null)
                throw new Exception("Must be configure first");

            if (_getDicFunc().ContainsKey(key))
                return (T)_getDicFunc()[key];
            else
                return default(T);
        }

        public static void Init(Func<IDictionary<object, object>> getDicFunc)
        {
            _getDicFunc = getDicFunc;
        }

        public static UserInfo UserInfo { get => Get<UserInfo>("UserInfo"); }
        public static ClientInfo ClientInfo { get => Get<ClientInfo>("ClientInfo"); }
        public static string Jwt { get => Get<string>("Jwt"); }
        public static string RefreshToken { get => Get<string>("RefreshToken"); }
        public static string CurrentHospitalCode { get => Get<string>("CurrentHospitalCode"); }
        public static string SystemType { get => Get<string>("SystemType"); }
        public static string ClientId { get => Get<string>("ClientId"); }
    }
}
