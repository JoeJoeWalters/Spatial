using System;
using System.Collections.Generic;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Common route format for internal processing
    /// </summary>
    public class GeoFileRoute
    { 
        /// <summary>
        /// Name of the route
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// List of ordered geographic points to indicate the path of the route
        /// </summary>
        public List<GeoCoordinateExtended> Points { get; set; }
    }

}
