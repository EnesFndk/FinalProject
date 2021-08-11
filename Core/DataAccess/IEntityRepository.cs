using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    //generic repository pattern yapıyoruz. 
    //Expression ile ProductManager'da filtreliyoruz.p=>p.CategoryId==2 diyerek linq ile yapıyoruz bu expression'u özetle tek bir data getirmek için
    //bu ürünü getirmek için farklı şu ürünü getirmek için farklı yazmak yerine expression kullanmak gayet mantıklı
    //filter=null demek filtre vermeyebilirsin yani hepsini getir. filter de belirli bir filtre koyuyor.

    //***T yi sınırlandırmak istersek buna generic constraint deniyor. where T:class diyerek sınırlandırdık T yi, Referans tip(class) olarak ta sadece IEntity olsun istedik.
    //***özetle burda şunu yaptık : T bir referans tip olmalı ve T ya IEntity olabilir yada IEntity ile implemente olan bir şey olabilir.
    //***new() koyduğumuzda ise IEntity kullanamıyoruz ama IEntity ile implemente olan Customer,Category ve Product'ı kullanabiliyoruz.

    //******Core katmanı diğer katmanları kesinlikle referans almaz********* core katmanı temeldir, arka plandadır.
    //Core katmanı yaptığımızda kodları düzelttiyoruz bu işleme Code Refactoring yani kod iyileştirilmesi diyoruz.
    public interface IEntityRepository<T> where T: class,IEntity,new()
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
