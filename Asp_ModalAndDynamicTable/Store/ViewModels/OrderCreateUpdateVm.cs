using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.ViewModels
{
    public class OrderCreateUpdateVm
    {
        public int? Id { get; set; }
        public IList<AddedArticleVm>? AddedArticles { get; set; }
        public IEnumerable<SelectListItem>? AvailableArticles { get; set; }
        public AddedArticleVm? ArticleToAdd { get; set; }
        public bool HasChanges { get; set; }
    }
}
