using System;
using System.ComponentModel.DataAnnotations;

namespace SuicideWeb.Models
{
    public class Person
    {
        public enum Genders { Мужской, Женский }
        [Display(Name ="Группа")]
        public int GroupId { get; set; }
        [Display(Name = "ID")]
        public int PersonId { get; set; }
        [Display(Name = "ФИО")]
        public string Name { get; set; }
        [Display(Name = "Пол")]
        public Genders Gender { get; set; }
        [Display(Name = "День рождения")]
        public DateTime BirthDay { get; set; }
        public Group Group { get; set; }
        public long VkId { get; set; }

    }
}