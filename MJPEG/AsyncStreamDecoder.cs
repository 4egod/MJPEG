#if NETSTANDARD2_1

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MJPEG
{
    public static class AsyncStreamDecoder
    {
        public static async IAsyncEnumerable<byte[]> GetFrameAsync(string uri)
        {
            using HttpClient client = new HttpClient();

            while (true)
            {
                using (var stream = await client.GetStreamAsync(uri).ConfigureAwait(false))
                {
                    while (true)
                    {
                        int contentLength = GetContentLength(stream);

                        if (contentLength == 0) break;

                        var content = GetContent(stream, contentLength);

                        if (content == null) break;

                        yield return content;
                    }
                }
            }
        }

        internal static byte[] SliceStream(Stream stream, byte[] beginPattern, byte[] endPattern)
        {
            int offset = 0;
            bool beginCathed = false;

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int x = stream.ReadByte();

                    if (x == -1) return null;

                    if (!beginCathed)
                    {
                        if (x == beginPattern[offset])
                        {
                            offset++;

                            ms.WriteByte((byte)x);

                            if (offset == beginPattern.Length)
                            {
                                beginCathed = true;
                                offset = 0;
                            }
                        }
                        else
                        {
                            ms.SetLength(0);
                            offset = 0;
                        }
                    }
                    else
                    {
                        ms.WriteByte((byte)x);

                        if (x == endPattern[offset])
                        {
                            offset++;

                            if (offset == endPattern.Length)
                            {
                                return ms.ToArray();
                            }
                        }
                        else
                        {
                            offset = 0;
                        }
                    }
                }
            }
        }

        internal static int GetContentLength(Stream stream)
        {
            int res = 0;

            // Get header which starts with "--my" and ends with "\r\n\r\n"
            var header = SliceStream(stream, new byte[] { 0x2D, 0x2D, 0x6D, 0x79 }, new byte[] { 0x0D, 0x0A, 0x0D, 0x0A });

            if (header == null || header.Length < 7)
            {
                return 0;
            }

            for (int i = header.Length - 5; i >= 0; i--)
            {
                if (header[i] == 0x20)
                {
                    string s = Encoding.UTF8.GetString(header, i, header.Length - i - 4);

                    if (!int.TryParse(s, out res))
                    {
                        return 0;
                    }

                    break;
                }
            }
#if DEBUG
            string dbg = $"Frame header:\n{Encoding.UTF8.GetString(header, 0, header.Length - 4)}\nParsed Length: {res}\n";
            Debug.WriteLine(dbg);
#endif
            return res;
        }

        internal static byte[] GetContent(Stream stream, int contentLength)
        {
            int bytesProcessed = 0;

            byte[] res = new byte[contentLength];

            while (bytesProcessed != contentLength)
            {
                int bytesReceived = stream.Read(res, bytesProcessed, contentLength - bytesProcessed);

                if (bytesReceived == 0) return null;

                bytesProcessed += bytesReceived;
            }

            return res;
        }
    }
}
#endif