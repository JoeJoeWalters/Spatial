using Spatial.Core.Common;
using Spatial.Core.Documents;
using System.IO;

namespace Spatial.RenderEngine.Renderers
{
    public interface IRenderer
    {
        Stream Generate(GeoFile file, RenderSettings renderProperties);
    }
}
