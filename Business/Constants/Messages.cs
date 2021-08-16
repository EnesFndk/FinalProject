using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
        //static verdiğimizde new'leme ihtiyacı duymuyoruz . Sabit olduğu için static veriyoruz.
        //public olduğu için ilk harflerini büyük yazdık.
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem Bakımda";
        public static string ProductsListed = "Ürünler Listelendi";
    }
}
