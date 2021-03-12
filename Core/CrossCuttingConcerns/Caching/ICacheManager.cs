using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        object Get(string key);
        void Add(string key, object value, int duration);
        bool IsAdd(string key);         // cache'de var mı? metotu
        void Remove(string key);
        void RemoveByPattern(string pattern);       // regex pattern versek, başı sonu önemli değil içinde "get" olanlar vs...

    }
}
