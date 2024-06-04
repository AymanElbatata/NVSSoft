using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.DAL.Entities
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPW { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Borrowing> Borrowings { get; set; }
    }
}
