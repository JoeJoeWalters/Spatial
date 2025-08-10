using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Information about the copyright holder and any license governing use of this file. By linking to an appropriate license, you may place your data into the public domain or grant additional usage rights.
    /// </summary>
    [Serializable]
    public class GPXCopyright : XmlBase
    {
        /// <summary>
        /// Year of copyright.
        /// </summary>
        [XmlElement("year")]
        public Int32 Year { get; set; } = 0;

        /// <summary>
        /// Link to external file containing license text.
        /// </summary>
        [XmlElement("license")]
        public String License { get; set; } = String.Empty;

        /// <summary>
        /// Copyright holder (TopoSoft, Inc.) 
        /// </summary>
        [XmlElement("author")]
        public GPXPerson Author { get; set; } = new GPXPerson();
    }
}
