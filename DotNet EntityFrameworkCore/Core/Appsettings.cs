namespace DotNet_EntityFrameworkCore.Core
{
    public class AppSettings
    {
        private static IConfiguration _config;
        public static void Configure(IConfiguration config)
        {
            _config = config;
        }

        public static TResult Get<TResult>(string path)
        {
            var val = _config[path];
            if (string.IsNullOrEmpty(val))
            {
                return default(TResult);
            }

            var type = typeof(TResult);
            var utype = Nullable.GetUnderlyingType(type);
            if (utype != null)
                return (TResult)Convert.ChangeType(val, utype);
            else
                return (TResult)Convert.ChangeType(val, type);
        }
    }
}
