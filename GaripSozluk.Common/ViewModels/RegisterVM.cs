using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class RegisterVM
    {
        [Display(Name="Nick Name")]
        [Required(ErrorMessage = "Nick Name Boş Bırakılamaz")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email Boş Bırakılamaz")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Doğum Tarihi Boş Bırakılamaz")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre Boş Bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler aynı olmalıdır.")]
        public string ConfirmPassword { get; set; }
    }
}
