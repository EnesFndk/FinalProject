using Business.Absract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductsService
    {
        IProductDal _productDal;
        ILogger _logger;

        //burda ben productmanager olarak Ilogger'a ihtiyaç duyuyorum diyor.
        //AurofacBusinessModule'e sistem arkasında newlettik(Reflection ile).Newledikten sonra (ILogger logger)'a veriyor ve sistem performanslıda çalışmış oluyor.
        public ProductManager(IProductDal productDal, ILogger logger)
        {
            _productDal = productDal;
            _logger = logger;
            
        }
        //AOP nedir? projede hata olduğunda onları düzeltmek için kullanılır.
        //Attribute koyduğumuz zaman mesela Add'i çağırıcağımız zaman üstüne bakıp Attribute var mı bakıyor, Varsa onu çalıştırıyor. Örnek Attribute [Validate]. Add çalışmadan önce Attribute çalışıyor.
        //Attribute lar classlara methodlara vs vs yapılara anlam yüklemek için kullanılıyor.
        //
        //[ValidateAspect(typeof (ProductValidator))] = ProductValidator daki kurallara göre Add metodunu doğrula demek. Bunun kodu da Core.Aspects.Autofac.Validation içerisinde yazıyor.

        //Attribute'lara tipleri typeof ile atıyoruz.
        [ValidationAspect(typeof (ProductValidator))]
        public IResult Add(Product product)
        {
            _productDal.Add(product);

             return new SuccessResult(Messages.ProductAdded);



            //virtual = bizim ezmemizi bekleyen methodlar.
            //interceptor = araya girmek demek = yani method'un başında ortasında sonunda çalışmak.
            //Burda kodlar çorba olacağına AOP ile düzenli şekilde log işlemide yapabiliriz. Bu kod kötü kod. Aşadağıdaki kötü kodu görmek için yorum satırına alıyorum.
            //_logger.Log();
            //try
            //{
            //    _productDal.Add(product);

            //    return new SuccessResult(Messages.ProductAdded);
            //}
            //catch (Exception exception)
            //{
            //    _logger.Log();
            //}
            //return new ErrorResult();


            //****Loglama = yapılan operasyonların bir yerde kaydını tutmaya yarıyor. Örnek = kim, ne zaman, nerede, ne ürün ekledi? gibi.
            //Loglamayı başta sonda yada ortada çalıştırabiliriz.Add içerisinde, add altında.
            //Bu şekilde yazdığımızda business katmanında sadece business code'lar yer alıcak aşağıdaki kodu yazmamıza gerek kalmıcak çünkü arkada onu çalıştıran Core katmanında var zaten.
            //business code = yönetim tarafından koyulan kurallardır aslında.Örnek = bir kategoride max 10 ürün olsun diyebilir.
            //*** Business code 'ları genelde API ve arayüze yazıyorlar bu kesinlikle hatalı. Business code kesinlikle Business'e yazılmalıdır.
            //Validation ise girilen ürünün bilgilerinin doğru olup olmadığını denetliyor. Kurallara uyup uymadığını denetliyor.
            //Arayüze neden yazılmaz = çünkü yeni bir arayüz yapıldığında taşımanız gerekir.
            //ValidationTool.Validate(new ProductValidator(), product);

            //IResult eklediğimizde Add kızdı çünkü içinde birşey döndürmemiz gerekiyordu. Bu yüzden Result'u döndürdük. Return'ü _productDal'a eklemedik çünkü Add var .
            //business kodlar buraya yazılır.
            //if ile olan kötü kod. 
            //Messages.ProductNameInvalid = business constants'a eklediğimiz yerleri buraya ekliyoruz temiz kod oluyor. Orda tek yerden düzeltebiliriz.
            //validation = business kodu ayrı , doğrulama kodu ayrı yazılmalı.
            //doğrulama kodu = şifre buna uymalı, ismin min 2 tane olmalı vs vs uyumuyla alakalı olan kodlara doğrulama kodu deniyor.
            //iş kuralı (business) kodu = ehliyet alıcaksınız , bir kişiye ehliyet verilip verilmeyeceği , yani  kişi sürücü sınavından belirlenen puanı almış mı diye kontrol ediyoruz ona göre kabul ediyoruz.
            //****ÖNEMLİ*****ŞİMDİ BU AŞAĞIDAKİ IF'LERI kullanmak yerine ve FluentValidation ile bu kodlardan kurtulup orda yedekleme yapabiliriz ve temiz kod olur. *********

            //AŞAĞIDAKİ IF'LER NOTLAR BOŞA ÇIKMASIN DİYE KÖTÜ KOD'U DA GÖRMEK İÇİN YORUM SATIRI OLARAK EKLENMİŞTİR.
            //if (product.UnitPrice<=0 )
            //{
            //    return new ErrorResult(Messages.UnitPriceInvalid);
            //}
            //if (product.ProductName.Length<2)
            //{
            //    return new ErrorResult(Messages.ProductNameInvalid);
            //}
            //**********_productDal.Add(product); üstte olması lazım kod sıralaması önemli çünkü yoksa çalışmaz. if yerine normal ErrorResult da yapsaydık olmazdı. Kod sıralaması önemli 
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
