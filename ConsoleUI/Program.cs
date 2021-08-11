using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    class Program
    {   
        //EntityFramework ile sadece database de işlem yaptık ne business ne de console da baştan kod yazmadık sadece InMemoryProduckDal yerine EfProductDal yazdık.
        //Business'de hiç farklı kod yazmaya gerek kalmadı. Bu şekilde clean bir işlem yapmış olduk.
        //Bu yaptığımız EntityFramework ile SOLID'in O özelliği yani Open Closed Principle
        static void Main(string[] args)
        {
            //ProductManager productManager = new ProductManager(new EfProductDal());

            //foreach (var product in productManager.GetAll())
            //{
            //    Console.WriteLine(product.ProductName);
            //}

            //CategoryId'si 2 olanları getir demek aşağıdaki
            //ProductManager productManager2 = new ProductManager(new EfProductDal());

            //foreach (var product in productManager2.GetAllByCategoryId(2))
            //{
            //    Console.WriteLine(product.ProductName);
            //}

            //Unitprice'ı mix 40 olan max 100  olanları getir.

            //ProductTest();

            //CategoryManager bir CategoryDal istediği için EfCategoryDal'ı newliyoruz.
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll())
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager2 = new ProductManager(new EfProductDal());

            foreach (var product in productManager2.GetByUnitPrice(40, 100))
            {
                Console.WriteLine(product.ProductName);
            }
        }
    }
}
