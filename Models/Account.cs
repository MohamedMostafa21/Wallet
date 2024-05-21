using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Digital_Wallet.Models
{
    public class Account
    {
        [DefaultValue(0)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Valid Email")]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
      
}
