using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Wallet.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
       
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be Greater than 0. ")]
        public int Amount { get; set; }
        [Column (TypeName = "nvarchar(100)")]
        public string? Note { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        virtual public  User? User { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please Choose a Caterogry. ")]
        public int? CategoryId { get; set; } = 0;
        virtual public Category? Category { get; set; }

        [NotMapped]
        public string? CategoryWithIcon
        {
            get
            {
                return Category == null ? "" : Category.Icon + " " + Category.Title ;
            }
        }
        [NotMapped]
        public string? TypeAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString();
            }
        }
    }
}
