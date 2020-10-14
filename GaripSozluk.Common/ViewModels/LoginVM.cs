using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class LoginVM
    {
        [Display(Name ="Nick Name")]
        [Required(ErrorMessage ="Kullanıcı Adı Boş Bırakılamaz")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre Boş Bırakılamaz")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool IsPersistent { get; set; }

    }
}
