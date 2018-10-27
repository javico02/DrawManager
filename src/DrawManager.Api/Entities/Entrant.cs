using System.Collections.Generic;

namespace DrawManager.Api.Entities
{
    public class Entrant
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<DrawEntry> Entries { get; set; }
        public List<PrizeSelectionStep> SelectionSteps { get; set; }

        public Entrant()
        {
            Entries = new List<DrawEntry>();
            SelectionSteps = new List<PrizeSelectionStep>();
        }
    }
}
