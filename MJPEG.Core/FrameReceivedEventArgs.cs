using System;

namespace MJPEG.Core
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public byte[] Frame { get; set; }
    }
}
