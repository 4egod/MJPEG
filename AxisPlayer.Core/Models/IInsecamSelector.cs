using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxisPlayer.Models
{
    public interface IInsecamSelector
    {
        Task<List<CameraInfo>> GetSourceLinksAsync(int page);
    }
}