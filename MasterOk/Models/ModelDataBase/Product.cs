﻿using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название товара")]
        public string TitleProduct { get; set; }

        [Display(Name = "Гарантия")]
        public int Warranty { get; set; }

        [Display(Name = "Стоимость")]
        public double Price { get; set; }

        [Display(Name = "Подкатегория")]
        public int? SubCategoryId { get; set; }

        public SubCategory? SubCategory { get; set; }

        public ICollection<PathImage>? NameImages { get; set; }
    }
}