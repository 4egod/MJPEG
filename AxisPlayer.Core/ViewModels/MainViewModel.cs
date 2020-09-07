using DynamicData.Binding;
using MJPEG;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AxisPlayer.ViewModels
{
    using Models;

    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        public const int MaxPage = 10;

        private IInsecamSelector _insecamSelector;

        public MainViewModel(IInsecamSelector insecamSelector)
        {
            _insecamSelector = insecamSelector;

            this.WhenAnyValue(x => x.Page).Subscribe(async page => await UpdatePage(page));

            Prev = ReactiveCommand.Create(() => Page--, canExecute: this.WhenAnyValue(x => x.Page, (page) => page > 1));

            Next = ReactiveCommand.Create(() => Page++, canExecute: this.WhenAnyValue(x => x.Page, (page) => page < MaxPage));
        }

        [Reactive]
        public int Page { get; private set; } = 1;

        public ReactiveCommand<Unit, int> Next { get; }

        public ReactiveCommand<Unit, int> Prev { get; }

        [Reactive]
        public ObservableCollectionExtended<PlayerViewModel> Sources { get; private set; }

        private async Task UpdatePage(int page)
        {
            Debug.WriteLine($"Update page: {page}");

            var links = await _insecamSelector.GetSourceLinksAsync(page);

            if (Sources == null)
            {
                Sources = new ObservableCollectionExtended<PlayerViewModel>();

                foreach (var item in links)
                {
                    IStreamDecoder decoder = Locator.Current.GetService<IStreamDecoder>();

                    Sources.Add(new PlayerViewModel(decoder)
                    {
                        Location = item.Location,
                        Uri = item.StreamUri
                    });
                }
            }
            else
            {
                for (int i = 0; i < links.Count; i++)
                {
                    Sources[i].Location = links[i].Location;
                    Sources[i].Uri = links[i].StreamUri;
                }
            }
        }
    }
}
