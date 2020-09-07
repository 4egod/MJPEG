using DynamicData.Binding;
using ReactiveUI;
using System.Reactive;

namespace AxisPlayer.ViewModels
{
    public interface IMainViewModel
    {
        int Page { get; }

        ReactiveCommand<Unit, int> Next { get; }

        ReactiveCommand<Unit, int> Prev { get; }

        ObservableCollectionExtended<PlayerViewModel> Sources { get; }
    }
}