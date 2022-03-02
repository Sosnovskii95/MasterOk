﻿using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class ProductCheck
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата заказа")]
        public DateTime DateTimeSale { get; set; }

        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        [Display(Name = "Менеджер")]
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<ProductSold> ProductSolds { get; set; }
    }
}