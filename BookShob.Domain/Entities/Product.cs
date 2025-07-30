using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShob.Domain.Entities
{
    public class Product
    {
        //[Key]
        public int Id { get; set; }
        //[Required]
        //[MaxLength(50)]
        public string Title { get; set; }
        //[MaxLength(250)]
        public string Description { get; set; }
        //[Required]
        //[MaxLength(50)]
        public string Author { get; set; }

        //[Required]
        [Range(1, 1000)]
        //[Column("BookPrice")]
        public decimal Price { get; set; }
        //[Column("isDeleted")]
        public bool markedAsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
