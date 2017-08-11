using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuicideWeb.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Display(Name ="Имя группы")]
        public string Name { get; set; }
        public List<Person> Persons { get; set; }

    }
}