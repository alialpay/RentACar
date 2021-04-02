using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        IPaymentDal _paymentDal;
        public IResult Add(CreditCard customer)
        {
            throw new NotImplementedException();
        }
        public PaymentManager(IPaymentDal paymentDal)
        {
            _paymentDal = paymentDal;
        }

        public IResult Delete(CreditCard customer)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CreditCard>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CreditCard>> GetByCardNumber(string cardNumber)
        {
            return new SuccessDataResult<List<CreditCard>>(_paymentDal.GetAll(c => c.CardNumber == cardNumber));
        }

        public IDataResult<CreditCard> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IResult Update(CreditCard customer)
        {
            throw new NotImplementedException();
        }

        public IResult VerifyCard(CreditCard creditCard)
        {
            var result = _paymentDal.Get(c => c.NameOnTheCard == creditCard.NameOnTheCard && c.CardNumber == creditCard.CardNumber && c.CardCvv == creditCard.CardCvv);
            if (result == null)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }
    }
}
