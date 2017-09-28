using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HanoiService.Core.Solver
{
    public delegate void ThreadEnd(int id);

    //font: https://stackoverflow.com/questions/35298789/solving-towers-of-hanoi-in-c-sharp-using-recursion
    public class HanoiSolver
    {
        private int _id { get; set; } = -1;
        private int _totalDiscs { get; set; } = 0;
        private ConcurrentStack<int> _firstPeg { get; set; } = new ConcurrentStack<int>();
        private ConcurrentStack<int> _secondPeg { get; set; } = new ConcurrentStack<int>();
        private ConcurrentStack<int> _thirdPeg { get; set; } = new ConcurrentStack<int>();
        private ManualResetEvent _doneEvent;
        private IList<ThreadEnd> _listeners;

        public IList<int> FistPeg { get { return _firstPeg.ToList(); } }
        public IList<int> SecondPeg { get { return _secondPeg.ToList(); } }
        public IList<int> ThirdPeg { get { return _thirdPeg.ToList(); } }

        public HanoiSolver(int id, int discs, ManualResetEvent doneEvent)
        {
            _id = id;
            _totalDiscs = discs;
            _doneEvent = doneEvent;
            _listeners = new List<ThreadEnd>();

            //Create list of items (discs)
            var discList = Enumerable.Range(1, _totalDiscs).Reverse();

            //Add items (discs) to first peg
            foreach (var d in discList)
            {
                _firstPeg.Push(d);
            }
        }

        public void AddListener(ThreadEnd listener)
        {
            _listeners.Add(listener);
        }

        // Wrapper method for use with thread pool.  
        public void ThreadPoolCallback(Object threadContext)
        {
            this.Move(_totalDiscs, _firstPeg, _thirdPeg, _secondPeg);
            _doneEvent.Set();
            foreach (var listener in _listeners)
            {
                listener(_id);
            }
        }

        private void Move(int discs, ConcurrentStack<int> fromPeg, ConcurrentStack<int> toPeg, ConcurrentStack<int> otherPeg)
        {
            int disc = -1;
            if (discs == 1)
            {
                if (fromPeg.TryPop(out disc))
                {
                    toPeg.Push(disc);
                    PrintPegs();
                    return;
                }
                else throw new Exception("Erro ao tentar desempilhar disco.");
            }

            Move(discs - 1, fromPeg, otherPeg, toPeg);
            PrintPegs();

            if (fromPeg.TryPop(out disc))
            {
                toPeg.Push(disc);
            }
            else throw new Exception("Erro ao tentar desempilhar disco.");

            Move(discs - 1, otherPeg, toPeg, fromPeg);
            PrintPegs();
        }

        private void PrintPegs()
        {
            var fp = _firstPeg.Select(x => x.ToString()).ToList();

            if (fp.Count < _totalDiscs)
            {
                fp.AddRange(Enumerable.Repeat(string.Empty, (_totalDiscs - fp.Count)));
            }

            var sp = _secondPeg.Select(x => x.ToString()).ToList();

            if (sp.Count < _totalDiscs)
            {
                sp.AddRange(Enumerable.Repeat(string.Empty, (_totalDiscs - sp.Count)));
            }

            var tp = _thirdPeg.Select(x => x.ToString()).ToList();

            if (tp.Count < _totalDiscs)
            {
                tp.AddRange(Enumerable.Repeat(string.Empty, (_totalDiscs - tp.Count)));
            }

            Console.WriteLine($"{"[First Peg]",10}" + $"{"[_second Peg]",10}" + $"{"[_third Peg]",10}");

            for (var i = 0; i < _totalDiscs; i++)
            {
                Console.WriteLine($"{fp[i],10}" +
                                  $"{sp[i],10}" +
                                  $"{tp[i],10}");
            }
        }
    }
}
