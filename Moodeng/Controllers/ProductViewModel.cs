using System.Linq;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class ProductViewModel
    {
        public object Id { get; internal set; }
        public IQueryable<SelectListItem> Categories { get; internal set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string RetailId { get; set; }
    }
}