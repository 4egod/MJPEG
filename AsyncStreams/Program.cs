using MJPEG;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncStreams
{
    class Program
    {
        const string FramesDir = "Frames";

        static async Task Main(string[] args)
        {
            if (Directory.Exists(FramesDir))
            {
                Directory.Delete(FramesDir, true);
            }

            if (!Directory.Exists(FramesDir))
            {
                Directory.CreateDirectory(FramesDir);
            }

            await foreach (var item in AsyncStreamDecoder.GetFrameAsync("http://83.128.74.78:8083/mjpg/video.mjpg"))
            {
                using FileStream fs = new FileStream($"{FramesDir}\\{DateTime.Now:HH-mm-ss-ff}.jpeg", FileMode.CreateNew);

                await fs.WriteAsync(item);
                await fs.FlushAsync();
            }
        }
    }
}
