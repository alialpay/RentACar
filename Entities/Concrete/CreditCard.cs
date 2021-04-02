using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class CreditCard:IEntity
    {
        [Key]
        public int CardId { get; set; }

        public string CardNumber { get; set; }

        public string NameOnTheCard { get; set; }

        public decimal MoneyInTheCard { get; set; }
        public object CardCvv { get; set; }
        public string ExpirationDate { get; set; }
    }
}
