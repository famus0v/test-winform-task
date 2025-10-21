using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.Entity
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(100, ErrorMessage = "Имя не может превышать 100 символов")]
        public string Name { get; set; }

        [Range(0, 150, ErrorMessage = "Возраст должен быть от 0 до 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateTime Birthdate { get; set; }

        public Person()
        {
            Name = string.Empty;
            Birthdate = DateTime.Now;
        }
    }
}
