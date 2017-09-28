using HanoiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Core.Interfaces.Services
{
   public interface IHanoiHistoryService
    {
        IList<HanoiExecution> GetHistoryPaged(int pageIndex, int pageSize);
    }
}
