﻿using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        //Intercept Bu blogu çalıştır demek.
        public override void Intercept(IInvocation invocation)
        {
            //Uygulama 
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    //
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}


