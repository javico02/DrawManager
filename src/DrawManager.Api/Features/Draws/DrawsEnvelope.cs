using System.Collections.Generic;

namespace DrawManager.Api.Features.Draws
{
    public class DrawsEnvelope
    {
        public List<DrawEnvelope> Draws { get; set; }
        public int DrawsCount { get; set; }
    }
}
