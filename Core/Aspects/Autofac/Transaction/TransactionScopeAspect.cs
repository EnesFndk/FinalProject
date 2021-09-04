using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    //Transactional Yönetimi ?= Uygulamalarda tutarlılığı korumak için yaptığımız yöntem
    //(Örn= benim hesapta 100 lira para var arkadaşıma 10 lira göndericem. Benim hesabımın 10 lira düşücek şekilde update edilmesi arkadaşımın hesabının 10 lira artacak şekilde update edilmesi )
    //yani 2 tane veritabanı işi var. Fakat benim hesaptan giderken güncelledi arkadaşımın hesabına yazarken sistem hata verdi. Paramı iade etmesi gerekiyor yani işlemi geri alması gerekiyor
    public class TransactionScopeAspect : MethodInterception
    {
        //Intercept Bu blogu çalıştır demek.
        public override void Intercept(IInvocation invocation)
        {
            //burda şablon oluşturuyoruz aslında.
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    //Methodu çalıştır. Bu productManager içine implemente ettiğimiz Transaction içindeki methodu çalıştır demek.
                    invocation.Proceed();
                    //Başarılı olursa bitir.
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    //eğer başarısız olursa uyarı verir ve işlemleri geri alır.
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}


