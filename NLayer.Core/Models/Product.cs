using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }  //efcore bunun foreign key olduğunu algılıyor
        public Category Category { get; set; } //1e çok ilişkisi var
        public ProductFeature ProductFeature { get; set; } //1e 1 ilişkisi var
    }
}
