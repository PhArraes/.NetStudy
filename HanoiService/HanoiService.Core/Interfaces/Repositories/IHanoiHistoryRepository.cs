using HanoiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Core.Interfaces.Repositories
{
    public interface IHanoiHistoryRepository
    {
        IList<HanoiExecution> GetHistoryPaged(int pageIndex, int pageSize);
    }
}
