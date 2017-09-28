using HanoiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Core.Interfaces.Repositories
{
    public interface IHanoiExecutionRepository
    {
        int Add(int numDiscs);
        void UpdateEnd(int id);
        HanoiExecution Get(int id);
    }
}
