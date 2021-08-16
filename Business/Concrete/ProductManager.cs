using Business.Absract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductsService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IResult Add(Product product)
        {
            //IResult eklediğimizde Add kızdı çünkü içinde birşey döndürmemiz gerekiyordu. Bu yüzden Result'u döndürdük. Return'ü _productDal'a eklemedik çünkü Add var .
            //business kodlar buraya yazılır.
            //if ile olan kötü kod. 
            //Messages.ProductNameInvalid = business constants'a eklediğimiz yerleri buraya ekliyoruz temiz kod oluyor. Orda tek yerden düzeltebiliriz.
            if (product.ProductName.Length<2)
            {
                return new ErrorResult(Messages.ProductNameInvalid);
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            //saat 22 de bakım olduğundan mesaj geliyor gibi bir senaryo yapıyoruz.
            if (DateTime.Now.Hour==22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            //IDataResult'un içinde data, işlem sonucu ve mesaj olduğu için bu şekilde yazıyoruz yoksa bize kızıyor.
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
        }

        //her p için p'nin categoryid si benim gönderdiğim id ye eşitse filtrele diyor aşağıdaki kodda.
        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p=>p.CategoryId==id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>( _productDal.Get(p=>p.ProductId == productId));
        }

        //iki fiyat aralığını getiriyor.
        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        //BURADA SİSTEM BAKIMDA YAZDIRDIK TABİ ÜSTTEKİ GETPRODUCTDETAILS OLDUĞU İÇİN 2. KEZ YAZDIRAMIYORUM O YÜZDEN BURAYI YORUM SATIRI YAPTIM.
        //public IDataResult<List<ProductDetailDto>> GetProductDetails()
        //{
        //    if (DateTime.Now.Hour == 22)
        //    {
        //        return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
        //    }
        //    return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        //}
    }
}
