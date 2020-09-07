using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AxisPlayer.Models
{
    public class InsecamSelector : IInsecamSelector
    {
        public const string Uri = "https://www.insecam.org/en/bytype/Axis/?page={0}";

        public async Task<List<CameraInfo>> GetSourceLinksAsync(int page)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = await client.GetStringAsync(string.Format(Uri, page));

                var start = content.IndexOf("thumbnail-item__img img-responsive");

                List<CameraInfo> res = new List<CameraInfo>();

                for (int i = 0; i < 6; i++)
                {
                    res.Add(ExtractCameraInfo(content, ref start));
                }

                List<string> bu = new List<string>();

                return res;
            }
        }

        private CameraInfo ExtractCameraInfo(string content, ref int startIndex)
        {
            var res = new CameraInfo();

            res.StreamUri = ExtractValue(content, ref startIndex, "src=");
            res.Location = ExtractValue(content, ref startIndex, "title=");//.Replace("View Axis CCTV IP camera online in ", "");
            int i = res.Location.IndexOf(" in ");
            res.Location = res.Location.Substring(i + 4, res.Location.Length - i - 4);

            return res;
        }

        private string ExtractValue(string content, ref int startIndex, string pattern)
        {
            var start = content.IndexOf(pattern, startIndex) + pattern.Length + 1;
            if (start == -1) return null;
            var end = content.IndexOf("\" ", start);

            startIndex = end;

            return content.Substring(start, end - start);
        }
    }
}
