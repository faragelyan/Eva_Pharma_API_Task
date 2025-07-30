namespace BookShob.Domain.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string catName { get; set; }
        public int catOrder { get; set; }
        public bool markedAsDeleted { get; set; }


    }
}
