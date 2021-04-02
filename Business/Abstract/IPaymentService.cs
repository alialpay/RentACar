using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPaymentService
    {
        IResult Add(CreditCard customer);
        IResult Update(CreditCard customer);
        IResult Delete(CreditCard customer);
        IDataResult<List<CreditCard>> GetAll();
        IDataResult<CreditCard> GetById(int id);
        IResult VerifyCard(CreditCard creditCard);
        IDataResult<List<CreditCard>> GetByCardNumber(string cardNumber);

    }
}
