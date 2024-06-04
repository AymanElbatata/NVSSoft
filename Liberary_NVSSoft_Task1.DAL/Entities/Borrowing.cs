using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.DAL.Entities
{
    public class Borrowing
    {
        [Key]
        public int? BorrowingId { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }

        public bool? IsDone { get; set; }
        public bool? IsDeleted { get; set; }


        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }

    }
}
