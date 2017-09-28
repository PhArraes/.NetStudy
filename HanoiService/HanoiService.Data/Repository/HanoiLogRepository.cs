using HanoiService.Core.Entities;
using HanoiService.Core.Interfaces;
using HanoiService.Core.Interfaces.Repositories;
using HanoiService.Data.Context;
using HanoiService.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Data.Repository
{
    public class HanoiLogRepository : IHanoiExecutionRepository, IHanoiHistoryRepository
    {
        public HanoiContext _context { get; set; }
        public HanoiLogRepository(HanoiContext context)
        {
            _context = context;
        }

        public HanoiExecution Get(int id)
        {
            HanoiExecution ex = null;
            HanoiLog log = _context.Logs.Where(l => l.LogId == id).FirstOrDefault();
            if (log != null)
            {
                ex = new HanoiExecution()
                {
                    HanoiExecutionId = log.LogId,
                    CreationTime = log.CreationTime,
                    DiscsNumber = log.DiscsNumber,
                    EndTime = log.EndTime
                };
            }
            return ex;
        }

        public int Add(int discsNumber)
        {
            HanoiLog log = new HanoiLog() { CreationTime = DateTime.Now, DiscsNumber = discsNumber };
            _context.Logs.Add(log);
            _context.SaveChanges();
            return log.LogId;
        }

        public void UpdateEnd(int id)
        {
            HanoiLog log = _context.Logs.Where(l => l.LogId == id).FirstOrDefault();
            if (log == null)
                throw new Exception("Log not found");
            log.EndTime = DateTime.Now;
            _context.Entry(log).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public IList<HanoiExecution> GetHistoryPaged(int pageIndex, int pageSize)
        {
            int count = _context.Logs.Count();
            if (count < pageIndex * pageSize)
                throw new Exception("Page out of range.");
            int skip = Math.Min(count, pageIndex * pageSize);
            int take = Math.Min(count - (pageIndex * pageSize), pageSize);
            return _context.Logs.OrderBy(l => l.LogId).Skip(skip).Take(take)
                .Select(l => new HanoiExecution()
                {
                    HanoiExecutionId = l.LogId,
                    CreationTime = l.CreationTime,
                    DiscsNumber = l.DiscsNumber,
                    EndTime = l.EndTime
                })
                .ToList();
        }
    }
}
