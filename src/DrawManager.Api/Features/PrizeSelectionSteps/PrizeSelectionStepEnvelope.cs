using DrawManager.Api.Entities;
using System;

namespace DrawManager.Api.Features.PrizeSelectionSteps
{
    public class PrizeSelectionStepEnvelope
    {
        public int PrizeId { get; set; }
        public int EntrantId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public PrizeSelectionStepType PrizeSelectionStepType { get; set; }

        /// <summary>
        /// Codigo del concursante. Cedula
        /// </summary>
        public string EntrantCode { get; set; }
        /// <summary>
        /// Nombre y Apellidos del concursante.
        /// </summary>
        public string EntrantName { get; set; }
        /// <summary>
        /// Sucursal 1
        /// </summary>
        public string EntrantSubsidiary { get; set; }
        /// <summary>
        /// Oficina
        /// </summary>
        public string EntrantOffice { get; set; }
        /// <summary>
        /// Unidad
        /// </summary>
        public string EntrantUnit { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string EntrantDepartment { get; set; }
        /// <summary>
        /// SubDepartamento
        /// </summary>
        public string EntrantSubDepartment { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        public string EntrantRegion { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        public string EntrantCity { get; set; }
        /// <summary>
        /// Sucursal 2
        /// </summary>
        public string EntrantBranchOffice { get; set; }
    }
}
