using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.DAL.Entities
{
    public class Author
    {
        [Key]
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
