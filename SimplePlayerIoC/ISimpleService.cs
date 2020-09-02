using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePlayerIoC
{
    public interface ISimpleService
    {
        byte[] GetLastFrame();

        void StartDecoding();
    }
}
