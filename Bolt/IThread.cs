using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolt
{
    public interface IThread
    {
        void Setup();
        void Run(ConcurrentQueue<string> queue);
        void TakeDown();
    }
}
