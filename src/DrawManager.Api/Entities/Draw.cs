using System;
using System.Collections.Generic;

namespace DrawManager.Api.Entities
{
    public class Draw
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ProgrammedFor { get; set; }
        public DateTime? ExecutedOn { get; set; }

        public int PrizesQty => Prizes?.Count ?? 0;

        public List<Prize> Prizes { get; set; }
        public List<DrawEntry> Entries { get; set; }

        public Draw()
        {
            Prizes = new List<Prize>();
            Entries = new List<DrawEntry>();
        }
    }
}
