using System.Collections.Generic;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizesEnvelope
    {
        public List<PrizeEnvelope> Prizes { get; set; }
        public int PrizesQty { get; set; }
    }
}
