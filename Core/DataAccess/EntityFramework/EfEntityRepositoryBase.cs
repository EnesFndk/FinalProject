using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity>
        where TEntity: class, IEntity, new()
        where TContext: DbContext, new()
    {
        //Database'den çektiğimiz ve kendimizin classlarıyla ilişkilendirdiğimizleri burada using ile kullanıcaz.
        //using = using bittiği anda verileri topluyor ve belleği temizliyor hızlıca. performans için çok kullanışlı.
        //var addedEntity = context.Entry(entity); = referansı yakala , addedEntity.State = EntityState.Added = entityframework ile gelen EntityState ile ekliyoruz. ve SaveChanges ile işlemi gerçekleştiriyoruz.
        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }
        //yukarda yaptığımız işlemleri kolay okunması amacıyla deletedEntity yapıyoruz.
        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
        //aynısını update için yapıyoruz ve modified sadece farklı oda update ile aynı anlama geliyor. Entityframework'de update öyle yazıldığı için.
        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            //context.Set<Product>() = bu bizim için tabloyu liste gibi ele alıyor. 
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        //aşağıda yaptığımız işlem = eğerki filtre göndermediyse databasedeki tablodaki tüm  datayı getir ama filtreyi vermişse o filtreyi uygula ve listele
        //databasedeki bütün tabloyu listeye çevir ve bana ver.

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }
    }
}
