using System;

namespace DrawManager.Api.Features.Draws
{
    public class DrawEnvelope
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ProgrammedFor { get; set; }
        public DateTime? ExecutedOn { get; set; }

        public int PrizesQty { get; set; }
    }
}
