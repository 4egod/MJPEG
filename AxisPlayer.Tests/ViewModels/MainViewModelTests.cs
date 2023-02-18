using Microsoft.VisualStudio.TestTools.UnitTesting;
using AxisPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using ReactiveUI;
using System.Threading;
using System.Reactive;
using Moq;
using Splat;
using System.Threading.Tasks;
using MJPEG;

namespace AxisPlayer.ViewModels.Tests
{
    using Models;

    [TestClass()]
    public class MainViewModelTests
    {
        private MainViewModel _vm;

        [TestInitialize]
        public void Initialize()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var decoderStub = new Mock<IStreamDecoder>();
            decoderStub.Setup(x => x.StartDecodingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));
                //.Callback((string x) => Debug.Print($"---->{x}"));

            Locator.CurrentMutable.Register(() => decoderStub.Object); 

            List<CameraInfo> cameraInfoList = new List<CameraInfo>();

            for (int i = 0; i < 6; i++)
            {
                cameraInfoList.Add(new CameraInfo() { Location = $"Location {i + 1}", StreamUri = $"http://localhost:{i + 1}/mjpg/video.mjpg" });
            }

            Mock<IInsecamSelector> selectorStub = new Mock<IInsecamSelector>();

            selectorStub.Setup(x => x.GetSourceLinksAsync(It.IsAny<int>())).ReturnsAsync(cameraInfoList);

            _vm = new MainViewModel(selectorStub.Object);
        }

        [TestMethod]
        public void TestPageShouldBeOneAtStart()
        {
            int act = _vm.Page;

            Assert.AreEqual(1, act);
        }

        [TestMethod]
        public void TestPrevButtonShouldBeDisabledAtStart()
        {
            bool act = true; 

            _vm.Prev.CanExecute.Subscribe(canExecute => act = canExecute);

            Assert.AreEqual(false, act);
        }

        [TestMethod]
        public void TestNextButtonShouldBeEnabledAtStart()
        {
            bool act = false;

            _vm.Next.CanExecute.Subscribe(canExecute => act = canExecute);

            Assert.AreEqual(true, act);
        }

        [TestMethod]
        public void TestButtonsFullCycle()
        {
            bool canExecute = false;

            for (int i = 1; i <= MainViewModel.MaxPage; i++)
            {
                _vm.Next.CanExecute.Subscribe(ce => canExecute = ce);

                Assert.AreEqual(i, _vm.Page);

                if (i != MainViewModel.MaxPage)
                {
                    Assert.AreEqual(true, canExecute);

                    _vm.Next.Execute().Subscribe();
                }
                else
                {
                    Assert.AreEqual(false, canExecute);
                }    
            }

            for (int i = _vm.Page; i >= 1; i--)
            {
                _vm.Prev.CanExecute.Subscribe(ce => canExecute = ce);

                Assert.AreEqual(i, _vm.Page);

                if (i != 1)
                {
                    Assert.AreEqual(true, canExecute);

                    _vm.Prev.Execute().Subscribe();
                }
                else
                {
                    Assert.AreEqual(false, canExecute);
                }
            }


        }
    }
}