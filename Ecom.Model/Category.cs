using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required,DisplayName("Category Name")]
        public string Name { get; set; }
    }
}
