using MJPEG;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace AxisPlayer.ViewModels
{
    public class PlayerViewModel : ReactiveObject, IPlayerViewModel
    {
        private IStreamDecoder _decoder;

        public PlayerViewModel(IStreamDecoder decoder)
        {
            _decoder = decoder;

            this.WhenAnyValue(x => x.Uri)
                .Where(uri => !string.IsNullOrEmpty(uri))
                .Subscribe(UpdateSource);

            Observable
                .FromEventPattern<FrameReceivedEventArgs>(_decoder, nameof(IStreamDecoder.OnFrameReceived))
                //.SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(new SynchronizationContextScheduler(SynchronizationContext.Current))
                .Subscribe(x => { Frame = x.EventArgs.Frame; });
        }

        [Reactive]
        public string Uri { get; set; }

        [Reactive]
        public string Location { get; set; }

        [Reactive]
        public byte[] Frame { get; set; }

        private void UpdateSource(string uri)
        {
            Debug.WriteLine($"Update source: {uri}");

            Frame = null;
            _decoder.StartDecodingAsync(uri, default);
        }
    }
}
