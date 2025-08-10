using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// An email address. Broken into two parts (id and domain) to help prevent email harvesting.
    /// </summary>
    [Serializable]
    public class GPXEMail : XmlBase
    {
        /// <summary>
        /// id half of email address (billgates2004)
        /// </summary>
        [XmlElement("id")]
        public String Id { get; set; } = String.Empty;

        /// <summary>
        /// domain half of email address (hotmail.com)
        /// </summary>
        [XmlElement("domain")]
        public String Domain { get; set; } = String.Empty;
    }
}
