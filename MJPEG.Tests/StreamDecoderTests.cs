using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace MJPEG.Tests
{
    [TestClass()]
    public class StreamDecoderTests
    {
        private StreamDecoder _decoder;

        [TestInitialize]
        public void Initialize()
        {
            _decoder = new StreamDecoder();
        }

        [DataTestMethod]
        [DataRow("--", "\r\n\r\n")]
        [DataRow("@@", "##")]
        public void SliceStreamTest(string beginPattern, string endPattern)
        {
            string expected = $"{beginPattern}data{endPattern}";

            string s = $"some stream content {expected} some stream content";

            byte[] beginPatternAsBytes = Encoding.UTF8.GetBytes(beginPattern);

            byte[] endPatternAsBytes = Encoding.UTF8.GetBytes(endPattern);

            byte[] buf = Encoding.UTF8.GetBytes(s);

            using MemoryStream ms = new MemoryStream(buf);

            byte[] actual = _decoder.SliceStream(ms, beginPatternAsBytes, endPatternAsBytes);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, Encoding.UTF8.GetString(actual));
        }

        [DataTestMethod()]
        [DataRow(77777)]
        [DataRow(777777)]
        [DataRow(7777777)]
        public void GetContentLengthTest(int expectedLength)
        {
            string s = $"--myboundary\r\nContent-Type: image/jpeg\r\nContent-Length: {expectedLength}\r\n\r\n#some_stream_content#";

            byte[] buf = Encoding.UTF8.GetBytes(s);

            using MemoryStream ms = new MemoryStream(buf);

            int actualLength = _decoder.GetContentLength(ms);

            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void GetContentTest()
        {
            byte[] buf = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            using MemoryStream ms = new MemoryStream(buf);

            byte[] act = _decoder.GetContent(ms, 3);

            Assert.IsNotNull(act);
            Assert.AreEqual(3, act.Length);
            Assert.AreEqual(0, act[0]);
            Assert.AreEqual(1, act[1]);
            Assert.AreEqual(2, act[2]);
        }

        [TestMethod]
        public void CheckImageTest()
        {
            byte[] pattern = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0xFF, 0xD9 };
            byte[] wrongPattern = new byte[10];

            bool actual = _decoder.CheckRawImage(pattern);
            Assert.IsTrue(actual);

            actual = _decoder.CheckRawImage(wrongPattern);
            Assert.IsFalse(actual);
        }
    }
}