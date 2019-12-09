using System;
using System.Collections.Generic;

namespace DrawManager.Api.Entities
{
    public class Draw
    {
        /// <summary>
        /// Id del sorteo. Autogenerado.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nombre del sorteo.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descripcion del sorteo.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Bandera que especifica si el concurso soporta multiples participaciones por parte de un mismo concursante.
        /// </summary>
        public bool AllowMultipleParticipations { get; set; }
        /// <summary>
        /// Fecha para la que se encuentra programada el sorteo.
        /// </summary>
        public DateTime ProgrammedFor { get; set; }
        /// <summary>
        /// Fecha de ejecucion y cierre del sorteo.
        /// </summary>
        public DateTime? ExecutedOn { get; set; }
        /// <summary>
        /// Grupo del sorteo.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Cantidad de premios del sorteo.
        /// </summary>
        public int PrizesQty => Prizes.Count;
        /// <summary>
        /// Cantidad de participaciones del sorteo.
        /// </summary>
        public int EntriesQty => Entries.Count;
        /// <summary>
        /// Bandera que indica cuando el sorteo se ha completado.
        /// </summary>
        public bool IsCompleted => Prizes.Count > 0 && Prizes.TrueForAll(p => p.Delivered);

        /// <summary>
        /// Premios.
        /// </summary>
        public List<Prize> Prizes { get; set; }
        /// <summary>
        /// Participaciones.
        /// </summary>
        public List<DrawEntry> Entries { get; set; }

        public Draw()
        {
            Prizes = new List<Prize>();
            Entries = new List<DrawEntry>();
        }
    }
}
