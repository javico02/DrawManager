using System.Collections.Generic;

namespace DrawManager.Api.Entities
{
    public class Entrant
    {
        /// <summary>
        /// Id de concurasante. Autogenerado.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Codigo del concursante. Cedula
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Nombre y Apellidos del concursante.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sucursal 1
        /// </summary>
        public string Subsidiary { get; set; }
        /// <summary>
        /// Oficina
        /// </summary>
        public string Office { get; set; }
        /// <summary>
        /// Unidad
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// SubDepartamento
        /// </summary>
        public string SubDepartment { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Sucursal 2
        /// </summary>
        public string BranchOffice { get; set; }

        public List<DrawEntry> Entries { get; set; }
        public List<PrizeSelectionStep> SelectionSteps { get; set; }

        public Entrant()
        {
            Entries = new List<DrawEntry>();
            SelectionSteps = new List<PrizeSelectionStep>();
        }
    }
}
