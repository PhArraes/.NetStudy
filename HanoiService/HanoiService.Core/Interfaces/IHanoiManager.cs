using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Core.Interfaces
{
    public interface IHanoiManager
    {
        bool TryStartHanoiThread(int discsNumber, out int solverID);
        bool TryGetCurrentState(int id, out List<List<int>> state);
    }
}
