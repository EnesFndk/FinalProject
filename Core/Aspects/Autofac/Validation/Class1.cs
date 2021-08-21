using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    //Engin hocanın NetCoreBackend/Core/Aspects/Autofac/Validation/ValidationAspect.cs 'dan kopyala yapıştır ile ekledik.
    //(Type validatorType) = bana validation type ' ı ver.
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            //Gönderilen validatorType , IValidator değil ise(!typeof) o zaman uyarı ver.
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        //AŞAĞIDAKİ NOTLAR ÖNEMLİ
        //Core.Utilities.Interceptors.MethodInterception içerisinde yer alan OnBefore 'un içini burda dolduruyoruz.
        //Activator.CreateInstance(_validatorType); = ProductValidator'un instance'sini oluştur. Reflection olduğu için çalışma anında çalışıyor. Reflection olduğunu Activator olduğu için anlıyoruz.
        //var entityType = _validatorType(Burda da ProductValidator'un) .BaseType. (Base tipini bul. )GetGenericArguments()[0] (Generic Argumanlarından ilkini bul diyor.)
        //var entities = invocation.(method demek )Arguments.Where (Argumanları nerde bak.)(t => t.GetType()(Tipini getir. )== entityType); (Product tipini.) = Burda da parametrelerini bul diyor kod.
        //foreach ile tek tek gez. Validation tool'u kullanarak , validate et. Yani doğrula.
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
