using System;

namespace MJPEG
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public byte[] Frame { get; set; }
    }
}
