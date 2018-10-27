using System.Collections.Generic;
using System.Linq;

namespace DrawManager.Api.Entities
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttemptsUntilChooseWinner { get; set; }
        public int DrawId { get; set; }

        public bool Delivered => SelectionSteps.Count == AttemptsUntilChooseWinner + 1 && SelectionSteps.Any(st => st.PriceSelectionStepType == PrizeSelectionStepType.Winner);

        public Draw Draw { get; set; }
        public List<PrizeSelectionStep> SelectionSteps { get; set; }

        public Prize()
        {
            SelectionSteps = new List<PrizeSelectionStep>();
        }
    }
}
