using System;
using System.IO;
using System.Reflection;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;

namespace Spatial.Core.Tests.Unit
{
    public class TestBase
    {
        public readonly double HalfMarathonTotalMinutes = 133.0; // Total time in minutes for the half marathon
        public readonly double HalfMarathonMovingTime = 124.0; // Moving time in minutes for the half marathon
		public readonly double HalfMarathonDistance = 21.37D; // Distance of the half marathon in km

        internal readonly GeoFile geoTrackFile;
        internal readonly GeoFile geoFile;
        internal readonly GeoFile geoCompare1;
        internal readonly GeoFile geoCompare2;

        internal readonly TCXFile tcxTrackFile;
        internal readonly GPXFile gpxTrackFile;

        public TestBase()
        {
            geoTrackFile = GetXMLData<GPXFile>("Data/GPXFiles/GPXRouteOnly.gpx").ToGeoFile();
            geoFile = GetXMLData<GPXFile>("Data/GPXFiles/HalfMarathon.gpx").ToGeoFile();
            geoCompare1 = GetXMLData<GPXFile>("Data/GPXFiles/Compare1.gpx").ToGeoFile();
            geoCompare2 = GetXMLData<GPXFile>("Data/GPXFiles/Compare2.gpx").ToGeoFile();

            tcxTrackFile = GetXMLData<TCXFile>("Data/TCXFiles/HalfMarathon.tcx");
            gpxTrackFile = GetXMLData<GPXFile>("Data/GPXFiles/HalfMarathon.gpx");
        }

        /// <summary>
        /// Load data from an embedded resource to use for testing
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetEmbeddedResource(string path)
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Gte embedded test data and map it to an object using the XML (de)serialiser
        /// </summary>
        /// <typeparam name="T">The type of object to create and map in to</typeparam>
        /// <param name="path">THe path to the resource in the assembly</param>
        /// <returns>An object of the correct type</returns>
        public T GetXMLData<T>(string path)
        {
            try
            {
                // Get a string representing the XML from the embedded resource
                string data = GetEmbeddedResource(path);
                if (data == null)
                    throw new Exception("No data");

                // Use the base deserialise method to make sure any cleansing is done first
                return XmlHelper.DeserialiseXML<T>(data);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
