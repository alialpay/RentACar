using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        public IResult Add(CreditCard customer)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(CreditCard customer)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CreditCard>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<CreditCard> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IResult Update(CreditCard customer)
        {
            throw new NotImplementedException();
        }
    }
}
