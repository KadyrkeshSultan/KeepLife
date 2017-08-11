using System.ComponentModel.DataAnnotations;

namespace SuicideWeb.Models
{
    public class ResultScan
    {
        public enum SeverityLevel
        {
            [Display(Name ="Низкий")]
            Низкий,
            [Display(Name = "Средний")]
            Средний,
            [Display(Name = "Высокий")]
            Высокий
        }
        public int UserId { get; set; }

        [Display(Name ="Пользователь")]
        public string User { get; set; }

        [Display(Name ="Совпадений по тезаурусу")]
        public int MatchesDic { get; set; }

        [Display(Name = "Совпадений по группам")]
        public int MatchesGroups { get; set; }

        [Display(Name ="Уровень критичности")]
        public SeverityLevel Level { get; set; }

        [Display(Name = "Запись со стены")]
        public string TextWall { get; set; }

        [Display(Name = "Текст статуса")]
        public string TextStatus { get; set; }

        public string InThesaurus { get; set; }

        public string Original { get; set; }

    }
}