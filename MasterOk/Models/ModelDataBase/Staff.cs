using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Фамилия сотрудника")]
        public string FirstNameStaff { get; set; }

        [Display(Name ="Имя сотрудника")]
        public string LastNameStaff { get; set; }

        [Display(Name ="Возраст")]
        public int Age { get; set; }

        [Display(Name ="Должность")]
        public int PositionId { get; set; }

        [Display(Name = "Должность")]
        public Position? Position { get; set; }
    }
}
