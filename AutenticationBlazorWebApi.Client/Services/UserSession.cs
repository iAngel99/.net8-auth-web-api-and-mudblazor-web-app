using System.Text.Json;

namespace AutenticationBlazorWebApi.Client.Services
{
    public interface IUserSession
    {


        void SetUserData<T>(string key, T value);


        T GetUserData<T>(string key);
    }
    public class UserSession : IUserSession
    {
        private readonly ISession _session;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void SetUserData<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            _session.SetString(key, jsonData);
        }

        public T GetUserData<T>(string key)
        {
            var jsonData = _session.GetString(key);
            if (jsonData == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }



}
