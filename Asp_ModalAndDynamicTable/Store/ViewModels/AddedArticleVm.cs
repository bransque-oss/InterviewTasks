using System.ComponentModel.DataAnnotations;

namespace Store.ViewModels
{
    public class AddedArticleVm
    {
        [Required(ErrorMessage = "You should select article.")]
        public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal Total => Price is null ? 0 : Price.Value * Quantity;
    }
}
