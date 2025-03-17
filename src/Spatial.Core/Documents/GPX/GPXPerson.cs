using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// A person or organization.
    /// </summary>
    [Serializable]
    public class GPXPerson : XmlBase
    {
        /// <summary>
        /// Name of person or organization.
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// Email address.
        /// </summary>
        [XmlElement("email")]
        public GPXEMail EMail { get; set; } = new GPXEMail();

        /// <summary>
        /// Link to Web site or other external information about person.
        /// </summary>
        [XmlElement("link")]
        public GPXLink Link { get; set; } = new GPXLink();
    }
}
