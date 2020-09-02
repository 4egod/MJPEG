using MJPEG.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePlayerIoC
{
    public class SimpleService : ISimpleService
    {
        private StreamDecoder _decoder;

        public SimpleService()
        {
            _decoder = new StreamDecoder("http://83.128.74.78:8083/mjpg/video.mjpg");
            //_decoder.StartDecodingAsync();
        }

        public byte[] GetLastFrame() => _decoder.GetLastFrame();

        public void StartDecoding() => _decoder.StartDecodingAsync();
    }
}
