using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class CommentVM
    {
        [Required(ErrorMessage = "Yorum alanı boş bırakılamaz")]
        [Display(Name = "Yorum")]
        public string Content { get; set; }

        public int PostId { get; set; }
    }
}
