using System;

namespace DrawManager.Api.Entities
{
    public class PrizeSelectionStep
    {
        public int PrizeId { get; set; }
        public int EntrantId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public PrizeSelectionStepType PrizeSelectionStepType { get; set; }

        public Prize Prize { get; set; }
        public Entrant Entrant { get; set; }
    }
}
