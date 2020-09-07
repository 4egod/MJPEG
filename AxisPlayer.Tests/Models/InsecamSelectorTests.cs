using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AxisPlayer.Models.Tests
{
    /// <summary>
    /// Integration Test
    /// </summary>
    [TestClass()]
    public class InsecamSelectorTests
    {
        [TestMethod()]
        public async Task GetSourceLinksAsyncTest()
        {
            var selector = new InsecamSelector();

            var act = await selector.GetSourceLinksAsync(1);

            Assert.IsNotNull(act);
            Assert.AreEqual(6, act.Count);
        }
    }
}