using Business.Abstract;
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
    public class CarManager : ICarService
    {
        ICarDal _carDal;                // Bir iş sınıfı, başka bir sınıfı new lemez. Bu şekilde injection yapıyoruz.
                                        // Ampulden generate constructor şıkkını kullanıyoruz. İş sınıflarında constructor kullanıyoruz.
        IBrandService _brandService;    // Eğer 
        public CarManager(ICarDal carDal, IBrandService brandService)
        {
            _carDal = carDal;
            _brandService = brandService;
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult Add(Car car)
        {
            // business codes

            
            
            if (CheckIfCarCountOfBrandCorrect(car.BrandId).Success)
            {
                _carDal.Add(car);

                return new SuccessResult(Messages.CarAdded);
            }

            return new ErrorResult();

            
        }

        public IDataResult<List<Car>> GetAll()
        {
            if (DateTime.Now.Hour == 2)        // saat 22 de listelenme işleminin yapılmasını engelliyoruz
            {
                return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }
            //iş kodları
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(),Messages.CarListed);
        }

        public IDataResult<List<Car>> GetAllByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p => p.BrandId == id));        // her p için p'nin BrandId'si benim gönderdiğim id'ye eşitse onları filtrele
        }

        public IDataResult<List<Car>> GetByDailyPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p => p.DailyPrice >= min && p.DailyPrice <= max));
        }

        public IDataResult<Car> GetById(int carId)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c => c.Id == carId));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails());
        }


        public IResult Update(Car car)
        {
            throw new NotImplementedException();
        }

        private IResult CheckIfCarCountOfBrandCorrect(int brandId)
        {
            // select count(*) from cars where brandId = 1
            var result = _carDal.GetAll(c => c.BrandId == brandId).Count;
            if (result > 5)
            {
                return new ErrorResult(Messages.CarCountOfBrandError);
            }
            return new SuccessResult();
        }
        
    }
}
