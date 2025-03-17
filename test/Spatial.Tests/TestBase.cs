using System;
using System.IO;
using System.Reflection;
using Spatial.Core.Helpers;

namespace Spatial.Tests
{
    public class TestBase
    {
        /// <summary>
        /// Load data from an embedded resource to use for testing
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String GetEmbeddedResource(String path)
        {
            // Get the current assembly information
            var assembly = typeof(TestBase).GetTypeInfo().Assembly;

            // Calculate the path to the resource in the assembly and 
            // fix any directory slashes
            path = $"{assembly.GetName().Name}/{path}".Replace("/", ".");

            // Load the resource stream from the assembly
            try
            {
                Stream resource = assembly.GetManifestResourceStream(path);
                if (resource == null)
                    throw new Exception("No resource found");

                using (TextReader reader = new StreamReader(resource))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                // A forced or unforced error occoured, return nothing ..
                return String.Empty;
            }
        }

        /// <summary>
        /// Gte embedded test data and map it to an object using the XML (de)serialiser
        /// </summary>
        /// <typeparam name="T">The type of object to create and map in to</typeparam>
        /// <param name="path">THe path to the resource in the assembly</param>
        /// <returns>An object of the correct type</returns>
        public T GetXMLData<T>(String path)
        {
            try
            {
                // Get a string representing the XML from the embedded resource
                String data = GetEmbeddedResource(path);
                if (data == null)
                    throw new Exception("No data");

                // Use the base deserialise method to make sure any cleansing is done first
                return XmlHelper.DeserialiseXML<T>(data);
            }
            catch//()Exception ex)
            {
                // Failure in getting data return default of T which will be 
                // empty causing the tests to fail anyway
                return default(T);
            }
        }
    }
}
