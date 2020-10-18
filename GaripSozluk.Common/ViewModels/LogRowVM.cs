using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class LogRowVM
    {
        [Display(Name ="Başlangıç tarihi")]
        public DateTime? startDate { get; set; }
        [Display(Name = "Bitiş tarihi")]
        public DateTime? endDate { get; set; }
        public List<LogFilterVM> logFilterVMs { get; set; }
        public List<LogVM> logVMs { get; set; }
    }
}
