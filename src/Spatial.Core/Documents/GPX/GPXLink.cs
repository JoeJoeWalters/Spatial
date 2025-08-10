using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// A link to an external resource (Web page, digital photo, video clip, etc) with additional information.
    /// </summary>
    [Serializable]
    public class GPXLink : XmlBase
    {
        /// <summary>
        /// Text of hyperlink.
        /// </summary>
        [XmlElement("text")]
        public String Text { get; set; } = String.Empty;

        /// <summary>
        /// Mime type of content (image/jpeg)
        /// </summary>
        [XmlElement("type")]
        public String Type { get; set; } = String.Empty;

        /// <summary>
        /// URL of hyperlink.
        /// </summary>
        [XmlElement("href")]
        public String HRef { get; set; } = String.Empty;
    }
}
