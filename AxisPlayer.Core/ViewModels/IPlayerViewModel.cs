namespace AxisPlayer.ViewModels
{
    public interface IPlayerViewModel
    {
        byte[] Frame { get; set; }
        string Location { get; set; }
        string Uri { get; set; }
    }
}