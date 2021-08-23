using Business.Absract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductsService
    {
        IProductDal _productDal;
        //ILogger _logger;
        ICategoryService _categoryService;

        //burda ben productmanager olarak Ilogger'a ihtiyaç duyuyorum diyor.
        //AurofacBusinessModule'e sistem arkasında newlettik(Reflection ile).Newledikten sonra (ILogger logger)'a veriyor ve sistem performanslıda çalışmış oluyor.


        //Service'ler 1 kere yaz ordan kullanılsın diye.
        //**************Bir entity Manager kendisi hariç başka bir Dal'ı enjecte edemez. Yani biz buraya IProductDal yazdık ama ICategoryDal yazamayız.**********************
        //Hem Başka bir Dal enjecte edemediğimiz için hemde patron verilen kuralı güncellemek isterse farklı bişey söylerse diye ICategoryService'den enjecte ediyoruz.
        //burda çağırmamız gereken kural ICategoryService'den gelmeli Patronun "Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez." dediğine göre.
        //artık CategoryService'den operasyon çağırıcaksak buraya çağırıcaz. Çünkü onu entegre ettik.
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            //_logger = logger;
            _categoryService = categoryService;
            
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            {
                _productDal.Update(product);

                return new SuccessResult(Messages.ProductCountOfCategoryError);
            }
            return new ErrorResult();
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
            //bu kodu bilmeyen bile isimlerinden anlar iş kuralları olduğunu.
            //buraya istediğimiz kadar iş kuralı yazabiliriz.
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfCategoryLimitExceded());

            //if içerisindeki result = kurala uymayan bir durum oluşmussa, result'u döndürebilirim. result= kurala uymayan.
            if (result!=null)
            {
                return result;
            }
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

            //Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez.




            //************* BusinessRules ile yazdığımız için aşadaki kötü kodlara ihtiyacımız yok. Ama ben görmek için yorum satırına alıyorum.*********************
            //bize patron görev vermişti onları yaptık.
            //aşağıdaki 2 kuraldanda geçiyor ise success.
            //if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    if (CheckIfProductNameExists(product.ProductName).Success)
            //    {
            //        _productDal.Add(product);

            //        return new SuccessResult(Messages.ProductAdded);
            //    }
            //}
            //return new ErrorResult();



            //****bir kategoride en fazla 10 ürün olabilir = iş kuralı patron söyledi yap.
            //****bunu if ile böyle yazdık. bu kötü kod. Bunu update'e de yazmamız gerekiyor.
            //****patron sonra bir kategoride en fazla 15 ürün olabilir dedi ve add de 15 yaptık tamam ama programcı update'de eklemeyi unutursa patlar. kodlar çorba olur.
            //**** kötü kod gözüksün diye burda yorum satırına alıyorum.
            //var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            //if (result>=10)
            //{
            //    return new ErrorResult(Messages.ProductCountOfCategoryError);
            //}



            //patronun istediği "bir kategoride en fazla 10 ürün olabilir" iş kuralını en altta private olarak yazdık ordan çekiyoruz clean code tekniği ile yazıyoruz.
            //CheckIfProductCountOfCategoryCorrect bunu biz kafamızdan belirledik. Herkes tarafından anlaşılsın diye ing. yazdık. 
            //aşağıdaki kod iyi kod yorum satırı yaptım.
            //if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    _productDal.Add(product);

            //    return new SuccessResult(Messages.ProductAdded);
            //}
            //return new ErrorResult();




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



        //******şimdi patron bize bir kategoride en fazla 10 ürün olsun dedi onu yazıyoruz.
        //******neden private yazdık bunu sadece business de kullacağımız için.
        //******bu bir iş kuralı parçacığı olduğu için CheckIfProductCountOfCategoryCorrect(Product product) 'de yazabiliriz ama farklı bir method geldiğinde düzenleme ihtiyacı duyacağın için alttaki şekilde yazmak daha iyi.
        //******Aşağıdaki kod aslında database'deki "Select count(*) from products where categoryId=1" kodu çalıştırır.
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            //successResult içerisinde neden message yok çünkü kullanıcıya başardın diye bir mesaj göndermek saçma olur.
            return new SuccessResult();
        }


        //**patron bizden aynı isimli ürün eklenemez dedi. nasıl yapıcaz??
        //***CheckIfProductNameExists =Bu ürün eklenmiş mi eklenmemiş mi
        //**parametre olarak productName göndermemiz lazım çünkü patron aynı isimli ürün eklenemez dedi.
        //
        private IResult CheckIfProductNameExists(string productName)
        {
            //ürünlere git bak ürün isminde productName var mı ? aşağıdaki kod. Any= var mı demek. Any'i linq'e çöztürmek lazım.
            //Any = bu koda uyan kayıt var mı = "_productDal.GetAll(p => p.ProductName == productName)"
            //******Aşağıdaki kod aslında database'deki "Select count(*) from products where categoryId=1" kodu çalıştırır.
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            //eğer böyle bir data varsa (result) yoksa demek isteseydik (!result)
            if (result )
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            //successResult içerisinde neden message yok çünkü kullanıcıya başardın diye bir mesaj göndermek saçma olur.
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            //Bunu category'de neden yazmadık çünkü bunu category'de yazarsak bu tek başına service olur. Bizden Category için isterlerse tamam ama şuan farklı bir durum var.
            //product için categoryservice nasıl yorumlanıyor o yüzden bu şekilde yazıyoruz.
            

            //burda bizim için tümünü getiriyor. GetAll bize IDataResult verdi.
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
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
