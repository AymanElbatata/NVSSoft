using Liberary_NVSSoft_Task1.DAL.Entities;

namespace Liberary_NVSSoft_Task1.Models
{
    public class BorrowBookStatusAllVM
    {
        public int? BookId { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsOut { get; set; }


        public virtual Author? Author { get; set; }
    }
}
