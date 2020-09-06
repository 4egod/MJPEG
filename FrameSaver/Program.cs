using MJPEG;
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

            if (!Directory.Exists(FramesDir))
            {
                Directory.CreateDirectory(FramesDir);
            }

            var decoder = new StreamDecoder();
        
            decoder.OnFrameReceived += Decoder_OnFrameReceived; 
            decoder.StartDecodingAsync("http://83.128.74.78:8083/mjpg/video.mjpg");

            Console.ReadLine();

            //while (true)
            //{
            //    Thread.Sleep(1000);

            //    var frame = decoder.GetLastFrame();

            //    if (frame == null) continue;

            //    Console.WriteLine($"Frame received at: {DateTime.Now}");

            //    using (FileStream fs = new FileStream($"Frames\\{DateTime.Now:HH-mm-ss}.jpeg", FileMode.CreateNew))
            //    {
            //        fs.Write(frame, 0, frame.Length);
            //        fs.Flush();
            //    }
            //}
        }

        private static void Decoder_OnFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            Console.WriteLine($"Frame received at: {DateTime.Now}");

            using (FileStream fs = new FileStream($"{FramesDir}\\{DateTime.Now:HH-mm-ss}.jpeg", FileMode.CreateNew))
            {
                fs.Write(e.Frame, 0, e.Frame.Length);
                fs.Flush();
            }
        }
    }
}
