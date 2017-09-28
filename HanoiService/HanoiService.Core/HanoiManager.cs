using HanoiService.Core.Entities;
using HanoiService.Core.Interfaces;
using HanoiService.Core.Interfaces.Repositories;
using HanoiService.Core.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HanoiService.Core
{
    public class HanoiManager : IHanoiManager
    {
        private int _maxThreads { get; set; } = 3;
        private int _runningThreads { get; set; }
        private IDictionary<int, HanoiSolver> _solvers { get; set; }
        private Object _rinningThreadsLock = 1;
        private IHanoiExecutionRepository _exeRepository;

        public int _solverCount { get; set; }

        public HanoiManager(IHanoiExecutionRepository exeRepo)
        {
            _exeRepository = exeRepo;
            _runningThreads = 0;
            _solvers = new Dictionary<int, HanoiSolver>();
            _solverCount = 0;
        }

        public bool TryStartHanoiThread(int discsNumber, out int solverId)
        {
            lock (_rinningThreadsLock)
            {
                if (_maxThreads <= _runningThreads)
                {
                    solverId = -1;
                    return false;
                }
                _runningThreads++;
            }
            ManualResetEvent mevent = new ManualResetEvent(false);
            solverId = _exeRepository.Add(discsNumber);
            HanoiSolver solver = new HanoiSolver(solverId, discsNumber, mevent);
            solver.AddListener(this.ThreadEnded);
            _solvers.Add(solverId, solver);
            ThreadPool.QueueUserWorkItem(solver.ThreadPoolCallback);
            return true;
        }

        public bool TryGetCurrentState(int id, out List<List<int>> state)
        {
            List<List<int>> result = (List<List<int>>)new List<List<int>>();
            HanoiExecution exe = null;
            if (_solvers.ContainsKey(id))
            {
                result.Add(_solvers[id].FistPeg.ToList());
                result.Add(_solvers[id].SecondPeg.ToList());
                result.Add(_solvers[id].ThirdPeg.ToList());
                state = result;
                return true;

            }
            else
            {
                exe = _exeRepository.Get(id);
                if (exe != null && exe.EndTime != null)
                {
                    result.Add(new List<int>());
                    result.Add(new List<int>());
                    var discList = Enumerable.Range(1, exe.DiscsNumber);
                    result.Add(discList.ToList());
                    state = result;
                    return true;
                }

            }
            state = null;
            return false;
        }

        public void ThreadEnded(int id)
        {
            lock (_rinningThreadsLock)
            {
                _exeRepository.UpdateEnd(id);
                _runningThreads--;
            }
        }

    }
}
