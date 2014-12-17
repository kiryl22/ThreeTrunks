using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeTrunks.Data.Models
{
    public class BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
