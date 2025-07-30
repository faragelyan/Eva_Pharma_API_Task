using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShob.Domain.Entities
{
    public class Category
    {
        //[Key]
        public int Id { get; set; }
        //[Required]
        //[MaxLength(50)]
        public string catName { get; set; }
        //[Required]
        public int catOrder { get; set; }
        //[NotMapped]
        public DateTime createdDate { get; set; }
        //[Column("isDeleted")]
        public bool markedAsDeleted { get; set; }
        public List<Product> products { get; set; } = new List<Product>();
    }
}
