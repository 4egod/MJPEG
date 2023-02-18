using System.Threading;

namespace MJPEG
{
    public interface IStreamDecoder
    {
        event StreamDecoder.FrameHandler OnFrameReceived;

        void StartDecodingAsync(string uri, CancellationToken token);
    }
}