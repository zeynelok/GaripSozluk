using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
   public class PostVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage ="Post alanı boş bırakılamaz")]
        [Display(Name ="Post")]
        public string Title { get; set; }

        [Display(Name ="Kategori")]
        public int PostCategoryId { get; set; }
 

    }
}
