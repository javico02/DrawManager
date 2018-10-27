using DrawManager.Api.Entities;
using System;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizeSelectionStepEnvelope
    {
        public int PrizeId { get; set; }
        public int EntrantId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public PrizeSelectionStepType PrizeSelectionStepType { get; set; }

        public string EntrantName { get; set; }
        public string EntrantCode { get; set; }
    }
}
