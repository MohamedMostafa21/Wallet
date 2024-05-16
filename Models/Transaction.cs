using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Wallet.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
       
        [Required]
        public int? Amount { get; set; }
        [Column (TypeName = "nvarchar(100)")]
        public string? Note { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        virtual public  User? User { get; set; }
        public int? CategoryId { get; set; }
        virtual public Category? Category { get; set; }
    }
}
