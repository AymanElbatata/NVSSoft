using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.DAL.Entities
{
    public class Book
    {
        [Key]
        public int? BookId { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Author? Author { get; set; }

        public virtual ICollection<Borrowing> Borrowings { get; set; }


    }
}
