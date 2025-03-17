using System;
using System.Collections.Generic;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Common Geography file to handle other file formats to be converted
    /// into this common format for processing
    /// </summary>
    public class GeoFile
    {
        /// <summary>
        /// Name of the Geography file
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The author of the file
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// LIst of routes recorded against this file
        /// </summary>
        public List<GeoFileRoute> Routes { get; set; }
    }
}
