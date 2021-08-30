using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        //Burda Adapter Pattern = adaptasyon dizaynı yöntemini uyguluyoruz. Bir şeyi kendi sistemimize adaptasyon ediyoruz.
        //burda microsoft'un kendi kütüphanesini kullanıcaz.
        //IMemoryCache bir interface o yüzden onu çözmemiz lazım. Onu çözmek içinde CoreModule içinde injection yapıyoruz.
        IMemoryCache _memoryCache;
        public MemoryCacheManager()
        {
            //Bu kodla birlikte memoryCache instance'si oluşuyor. CoreModule'deki gibi injection otomatik eklenmiş oluyor.
            //GetService<IMemoryCache>(); yazdıktan sonra kızarsa using Microsoft.Extensions.DependencyInjection; yaz using kısmına.
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }
        public void Add(string key, object value, int duration)
        {
            //TimeSpan.FromMinutes(duration) = bellekten ne zaman uçurayım ? Ne kadar süre verirsek o kadar süre için kod cache'de kalacak.
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            //bellekte öyle bir cache değeri var mı ? 
            //Böyle bir değer bellekte var mı yok mu ona bakıyor.
            //Sadece key verdiğimizde kızdığı için illa bişey döndürmek istemiyor isek "out _" yaparak sadece bellekte böyle bir anahtar var mı yok mu onu görmek istiyoruz.
            //out _ c# tekniklerinden biri
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        
        public void RemoveByPattern(string pattern)
        {
            //Bu yapıyı ezbere bilmeye gerek yok. Mantığı anlamak gerekiyor.
            //RemoveByPattern çalışma anında bellekten silmeye yarıyor. Elimizde bir sınıfın instance'si var vellekte ve çalışma anında müdahale etmek istiyor isek bunu Reflection ile yaparız
            //Reflection ile çalışma anında elimizde bulunan nesnelere hatta olmayanlarıda yeniden oluşturmak gibi çalışmalar yapabileceğimiz bir yapı --- zaten gördük reflection'u

            //İlk olarak Bellekte Cache ile ilgili olan yapıyı çekmek istiyorum. 
            //EntriesCollection'u nerden biliyoruz? Microsoft ben bellekte cache'lediğimde cache datalarını EntriesCollection'un içine atıyorum dediği için ordan çekiyoruz.
            //Gir belleğe bak Bellekte MemoryCache türünde olan EntriesCollection bul.
            //cacheEntriesCollectionDefinition'u _memoryCache olanları bul.

            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();
            //Onların her bir cache elemanını gez.
            foreach (var cacheItem in cacheEntriesCollection)
            { 
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }
            //pattern'i bu şekilde oluşturuyoruz. Signleline olucak. Compiled olucak vs vs gibi.
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Her bir cache elemanından bu kurallara uyanlar var ise, bu kurallar benim silme işlemini gerçekleştirirken vereceğim değerin ta kendisi olucak.
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
            //tek tek geziyorum ve kurallara uyanların key'lerini buluyorum ve Remove diyerek uçuruyorum.
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}