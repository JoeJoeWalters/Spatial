using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spatial.RenderEngine.Renderers
{
    public class RenderSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public GeoCoordinate TopLeft { get; set; }
        public GeoCoordinate BottomRight { get; set; }
    }
}
