using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Performance;
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
{/* Cross Cutting Concersns
  *     Validation      doğrulama
  *     Log             kayıt
  *     Cache           önbellek
  *     Transaction     işlem
  *     Authorization   yetkilendirme
  Bunlar katmanları dikine keser. Bunların hepsi iş kuralı olarak burada yapılırsa çorbaya döner
  */
    public class CarManager : ICarService
    {
        ICarDal _carDal;                            // Bir iş sınıfı, başka bir sınıfı new lemez. Bu şekilde injection yapıyoruz.
                                                    // Ampulden generate constructor şıkkını kullanıyoruz. İş sınıflarında constructor kullanıyoruz.
       // IBrandService _brandService;                // bir entity manager kendisi hariç bir dalı'ı enjekte edemez. bu yüzden service kullanılmalı

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        //Claim
        [SecuredOperation("car.add,admin")]                     //burası yetki kontrolü yapar// Bu authorization aspect'leri genellikle business'a yazılır. Çünkü her projenin yetkilendirme algoritması değişebilir. Zaten altyapıyı core'a yazmıştık, bu aspect kısmı ise business'a yazacağız
        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Add(Car car)
        {
            // business codes




            IResult result = BusinessRules.Run(CheckIfBrandExists(car.BrandId), CheckIfCarCountOfColorCorrect(car.ColorId));        // yarın bir gün yeni bir kural gelirse virgülle ayır ve ekle

            if (result != null)     // yani "kurala uymayan bir durum aluşmuşsa"
            {
                return result;
            }
            _carDal.Add(car);

            return new SuccessResult(Messages.CarAdded);

            
        }
       

        [CacheAspect]       //key,value
        public IDataResult<List<Car>> GetAll()
        {
            if (DateTime.Now.Hour == 2)        // saat 2 de listelenme işleminin yapılmasını engelliyoruz
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
        
        [CacheAspect]
       // [PerformanceAspect(5)]      // bu metodun çalışması 5sn yi geçerse beni uyar
        public IDataResult<Car> GetById(int carId)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c => c.Id == carId));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails());
        }
        public IDataResult<List<CarDetailDto>> GetCarDetail(int carId)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails().Where(x => x.BrandId == carId).ToList());
        }

        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]  // bellekteki ICarService'deki içerisinde Get olan tüm keyleri sil demek
        public IResult Update(Car car)
        {
            if (CheckIfCarCountOfColorCorrect(car.ColorId).Success)
            {
                _carDal.Update(car);

                return new SuccessResult(Messages.CarUpdated);
            }

            return new ErrorResult();
        }

        private IResult CheckIfCarCountOfColorCorrect(int colorId)      // aynı renkte en fazla 5 araba olabilir. bu bir iş kuralı. update için add için de update için vs de geçerli olabilir. o yüzden ayırdık 
        {
            // select count(*) from cars where brandId = 1
            var result = _carDal.GetAll(c => c.ColorId == colorId).Count;       // eklemek istediğim arabanın renginden kaç tane kayıt var?
            if (result > 5)                                                     // kayıt 5'den fazla mı
            {
                return new ErrorResult(Messages.CarCountOfColorError);
            }
            return new SuccessResult();
        }

        
        private IResult CheckIfBrandExists(int brandId)      
        {
            // select count(*) from cars where brandId = 1
            var result = _carDal.GetAll(c => c.BrandId == brandId).Any();       
            if (result)                                                     // result true demek bu
            {
                return new ErrorResult(Messages.BrandAlreadyExists);
            }
            return new SuccessResult();
        }

       // [TransactionScopeAspect]
        public IResult AddTransactionalTest(Car car)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Car>> GetAllByColorId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p => p.ColorId == id));
        }

        public IDataResult<List<CarDetailDto>> GetCarsByBrandId(int brandId)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails().Where(x => x.BrandId == brandId).ToList());
        }
    }
}
// carDetaildto döndüren bir method getall