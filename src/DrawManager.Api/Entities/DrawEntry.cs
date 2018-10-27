using System;

namespace DrawManager.Api.Entities
{
    public class DrawEntry
    {
        public int DrawId { get; set; }
        public int EntrantId { get; set; }
        public DateTime RegisteredOn { get; set; }

        public Draw Draw { get; set; }
        public Entrant Entrant { get; set; }
    }
}
