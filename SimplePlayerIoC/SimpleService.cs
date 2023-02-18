using MJPEG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimplePlayerIoC
{
    public class SimpleService : ISimpleService
    {
        private StreamDecoder _decoder;

        public SimpleService()
        {
            _decoder = new StreamDecoder();
        }

        public byte[] GetLastFrame() => _decoder.GetLastFrame();

        public void StartDecoding(CancellationToken token) => _decoder.StartDecodingAsync("http://83.128.74.78:8083/mjpg/video.mjpg",token);
    }
}
