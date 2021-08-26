using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    //EntityFramework veritabanındakiler ile kendi classları ilişkilendirmek için kullanılıyor.
    //DbContext EntityFramework ile gelen databasecontext. Default olarak var studio içerisinde
    public class NorthwindContext:DbContext
    {
        //override yazdıktan sonra OnConfiguring seçiyoruz ve bu metod senin projen hangi veritabanı ile ilişkili olduğunu belirteceğimiz yer. onu silip aşağıdakini yazıyoruz.
        //optionsBuilder.UseSqlServer(); = sql serverine nasıl bağlanacağımı belirtmek için
        //Database = Nortwind bu databaselerden ismi nortwind olana bağlan demek ve trusted_connection=true ise isim şifre gerektirmeden girmemizi sağlıyor.
        //sql de büyük küçük harf farketmez.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database = Northwind;Trusted_Connection=true");
        }

        //dbset ile benim product class'ımı database'deki nortwind içerisindeki products tablosunu ile bağla. aynı şekilde category ve customer ile de geçerli.
        //Daha sonra EfProductDal'a gidiyoruz ve kullanmaya başlıyoruz.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

    }
}