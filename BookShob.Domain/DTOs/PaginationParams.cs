using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShob.Domain.DTOs
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 10;  

        private const int MaxPageSize = 50;
        public int ValidPageSize => PageSize > MaxPageSize ? MaxPageSize : PageSize;
    }
}
