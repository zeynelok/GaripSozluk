using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class SearchVM
    {
        [Required (ErrorMessage ="Metin kısmı boş bırakılamaz")]
        [Display(Name ="Metin")]
        public string text { get; set; }

        [Display(Name ="Sıralama")]
        public int ranking { get; set; }
        [Display(Name ="Başlangıç Tarihi")]
        public DateTime? startDate { get; set; }

        [Display(Name ="Bitiş Tarihi")]
        public DateTime? endDate { get; set; }
        public List<PostVM> posts { get; set; }
         

    }
}
