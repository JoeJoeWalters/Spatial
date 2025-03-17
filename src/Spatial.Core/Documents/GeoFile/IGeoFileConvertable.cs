using System;

namespace Spatial.Core.Documents
{
    public interface IGeoFileConvertable
    {
        /// <summary>
        /// Requirement to have a method that converts to the common format
        /// </summary>
        /// <returns>A common GeoFile conversion of the origional</returns>
        GeoFile ToGeoFile();

        /// <summary>
        /// Convert a GeoFile to the native format
        /// </summary>
        /// <param name="file">The GeoFile format to convert from</param>
        /// <returns>Success Or Failure flag</returns>
        Boolean FromGeoFile(GeoFile file);
    }
}
