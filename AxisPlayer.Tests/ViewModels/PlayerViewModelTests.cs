using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJPEG;
using Moq;
using System.Threading;

namespace AxisPlayer.ViewModels.Tests
{
    [TestClass()]
    public class PlayerViewModelTests
    {
        [TestInitialize]
        public void Initialize()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [TestMethod()]
        public void TestChangingUriShouldStartStreamDecoder()
        {
            string expected = "http://foo.com";

            string actual = null;

            var stub = new Mock<IStreamDecoder>();
            stub.Setup(x => x.StartDecodingAsync(It.IsAny<string>()))
                .Callback((string x) => actual = x);

            PlayerViewModel vm = new PlayerViewModel(stub.Object);

            // Should start a new stream decoding job
            vm.Uri = expected;

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}