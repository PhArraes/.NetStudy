using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Core.Entities
{
    public class HanoiExecution
    {
        public int HanoiExecutionId { get; set; }
        public int DiscsNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
