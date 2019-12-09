using System;
using System.Collections.Generic;

namespace DrawManager.Api.Entities
{
    public class DrawEntry
    {
        /// <summary>
        /// Id de la participación. Autogenerado. Llave primaria.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id del sorteo a la que pertenece la participación. Llave foránea.
        /// </summary>
        public int DrawId { get; set; }
        /// <summary>
        /// Id del participante a la que pertenece la participación. Llave foránea.
        /// </summary>
        public int EntrantId { get; set; }
        /// <summary>
        /// Fecha de registro de la participación.
        /// </summary>
        public DateTime RegisteredOn { get; set; }

        /// <summary>
        /// Sorteo.
        /// </summary>
        public Draw Draw { get; set; }
        /// <summary>
        /// Participante.
        /// </summary>
        public Entrant Entrant { get; set; }
        /// <summary>
        /// Pasos de selección.
        /// </summary>
        public List<PrizeSelectionStep> SelectionSteps { get; set; }
    }
}
