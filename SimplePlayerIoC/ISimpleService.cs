using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimplePlayerIoC
{
    public interface ISimpleService
    {
        byte[] GetLastFrame();

        void StartDecoding(CancellationToken token);
    }
}
