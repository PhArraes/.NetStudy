using HanoiService.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanoiService.Core.Entities;
using HanoiService.Core.Interfaces.Repositories;

namespace HanoiService.Core.Services
{
    public class HanoiHistoryService : IHanoiHistoryService
    {
        private IHanoiHistoryRepository _historyRepository { get; set; }

        public HanoiHistoryService(IHanoiHistoryRepository histRepo)
        {
            _historyRepository = histRepo;
        }

        public IList<HanoiExecution> GetHistoryPaged(int pageIndex, int pageSize)
        {
            return _historyRepository.GetHistoryPaged(pageIndex, pageSize);
        }
    }
}
