using Microsoft.AspNetCore.Identity;
using Syncfusion.EJ2.Layouts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Wallet.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(8)")]
        public string Type { get; set; } = "Expense";

        [Column(TypeName = "nvarchar(5)")]
        [Required(ErrorMessage = "Icon Is Required")]

        public string Icon { get; set; } = "";
        public int? UserId { get; set; }
        virtual public User? User { get; set; }
        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
