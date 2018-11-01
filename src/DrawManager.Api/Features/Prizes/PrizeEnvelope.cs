using DrawManager.Api.Features.PrizeSelectionSteps;
using System;
using System.Collections.Generic;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizeEnvelope
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttemptsUntilChooseWinner { get; set; }
        public DateTime? ExecutedOn { get; set; }
        public int DrawId { get; set; }

        public bool Delivered { get; set; }

        public List<PrizeSelectionStepEnvelope> SelectionSteps { get; set; }
    }
}
