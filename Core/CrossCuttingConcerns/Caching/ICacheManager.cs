using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    //*******BİZ MICROSOFT'UN CACHE SİSTEMİNİ KULLANICAZ O YÜZDEN CACHING ALTINA MICROSOFT KLASÖRÜ KURDUK FAKAT YARIN ÖBÜRGÜN REDİS İLE ÇALIŞMAK İSTERSEK REDİS İÇERİSİNİ DOLDURMAMIZ YETERLİ OLUCAK. **********
    //********GÖSTERMEK AMACIYLA KURDUM REDİS KLASÖRÜNÜ



    //****Cache = verileri her seferinde database'den çekmektense belirli aralıklarla önbelleğe alan ve hızlı sonuçlar üreten yapıdır.
    //Cache'de key ve value değerleri vardır. Her veri tiplerinin base'i object olduğu için object atadık. Cache'de ne kadar durucak = duration.
    //Biz Cache'i c# base'inde bulunan InmemoryCache'den yararlanacağız. Farklı alternatiflerde var.
    public interface ICacheManager
    {
        //Biz T döndürücez bu bir liste de olabilir ismi Get olucak. 
        //Hangi tipte tuttuğumuzu get'in içerisinde belirtiyor olucaz. Generic bir method bu.
        //Ben sana bir key vereyim sen bellekten o key'e karşılık gelen datayı ver diyoruz.
        T Get<T>(string key);
        //bu da generic olmayan versiyonu ama tip dönüşümünü yazmak lazım. farklı bir yazım şekli aslında.
        object Get(string key);
        void Add(string key, object value, int duration);
        //bunu cache'den mi getirelim veritabanından mı ? Cache'de varsa cache'den getiririz. yoksa veritabanından getiririz ama onu cache ekleriz. 
        //bu yazdığımız bool IsAdd(); cache'de var mı sorusunu soruyor.
        bool IsAdd(string key);
        //Cache'den uçurma işlemi
        void Remove(string key);
        //buna biz bir desen veriyoruz mesela şöyle içinde get olanları uçur yada içinde category olanları direk uçur diye pattern veriyoruz. Başında sonunda olması önemli değil yer alması önemli
        void RemoveByPattern(string pattern);
    }
}
