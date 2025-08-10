using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spatial.Core.Common;
using Spatial.Core.Documents;
using Spatial.RenderEngine;
using System.Net;
using System.Net.Http.Headers;

namespace Spatial.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenderController : ControllerBase
    {
        private readonly Renderer _renderer;
        private Dictionary<string, GeoFile> _files;

        public RenderController()
        {
            _renderer = new Renderer();
            _files = new Dictionary<string, GeoFile>();
        }

        [HttpPost]
        public string Post([FromBody]GeoFile file)
        {
            string tag = Guid.NewGuid().ToString();
            _files[tag] = file;
            return tag;
        }

        [HttpGet]
        public HttpResponseMessage Get(int width, int height, double topLeftLat, double topLeftLong, double bottomRightLat, double bottomRightLong, string tag)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = _renderer.Generate(width, height, new GeoCoordinate(topLeftLat, topLeftLong), new GeoCoordinate(bottomRightLat, bottomRightLong), _files[tag]);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("image/png");
                //new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
