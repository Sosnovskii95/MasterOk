﻿using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Логин")]
        public string LoginClient { get; set; }

        [Display(Name = "Пароль")]
        public string PasswordClient { get; set; }

        [Display(Name = "Фамилия")]
        public string FirstNameClient { get; set; }

        [Display(Name = "Имя")]
        public string LastNameClient { get; set; }

        public ICollection<ProductCheck>? ProductChecks { get; set; }

        public ICollection<CartClient>? CartClients { get; set; }
    }
}