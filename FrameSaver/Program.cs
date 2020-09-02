using MJPEG.Core;
using System;
using System.IO;
using System.Threading;

namespace FrameSaver
{
    class Program
    {
        const string FramesDir = "Frames";

        static void Main(string[] args)
        {
            if (Directory.Exists(FramesDir))
            {
                Directory.Delete(FramesDir, true);
            }

            if (!Directory.Exists("Frames"))
            {
                Directory.CreateDirectory("Frames");
            }

            var decoder = new StreamDecoder("http://83.128.74.78:8083/mjpg/video.mjpg");

            decoder.StartDecodingAsync();

            while (true)
            {
                Thread.Sleep(1000);

                var frame = decoder.GetLastFrame();

                if (frame == null) continue;

                Console.WriteLine($"Frame received at: {DateTime.Now}");

                using (FileStream fs = new FileStream($"Frames\\{DateTime.Now:HH-mm-ss}.jpeg", FileMode.CreateNew))
                {
                    fs.Write(frame, 0, frame.Length);
                    fs.Flush();
                }
            }
        }
    }
}
