using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Spatial.Core.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// Pattern to remove tags that cause a problem when tranforming xml
        /// where polymorphic types in xml are used and no mapping can be made
        /// </summary>
        public static String CleanTags = @"xsi:type=\""[a-zA-Z0-9:;\.\s\(\)\-_\""\,]*";

        /// <summary>
        /// Clean down tags we need to get rid of such as xsi:type for complex polymorphics such as device_t and application_t in TCX files
        /// </summary>
        /// <param name="value">The origional XML</param>
        /// <returns>The cleaned XML</returns>
        public static String CleanXML(this String value)
            => Regex.Replace(value, CleanTags, String.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public static T DeserialiseXML<T>(String data)
        {
            try
            {
                // Clean down tags we need to get rid of such as xsi:type for complex polymorphics such as device_t and application_t in TCX files
                data = data.CleanXML();

                // Load the XML in to the object required 
                // If tagged mappings exist they should be mapped
                StringReader strReader = new StringReader(data);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlTextReader xmlReader = new XmlTextReader(strReader);

                T deserialised = (T)serializer.Deserialize(xmlReader);
                return deserialised;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
