using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, CarDbContext>, ICustomerDal
    {
        public List<CustomerDetailDto> GetCustomerDetails()
        {
            using (CarDbContext context = new CarDbContext())
            {
                var result = from cus in context.Customers
                             join us in context.Users
                             on cus.UserId equals us.Id

                             select new CustomerDetailDto
                             {
                                 CustomerId = cus.Id,
                                 FirstName = us.FirstName,
                                 LastName = us.LastName,
                                 CompanyName = cus.CompanyName

                             };
                return result.ToList();
            }
        }
    }
}