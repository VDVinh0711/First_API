using System;
namespace API_EF.Service
{
    public interface ICaches
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData<T>(string key);

    }
}
