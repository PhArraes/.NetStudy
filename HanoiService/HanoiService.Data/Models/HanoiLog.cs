using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Data.Models
{
    public class HanoiLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        [Required]
        public int DiscsNumber { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
