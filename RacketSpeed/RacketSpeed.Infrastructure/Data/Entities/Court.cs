using System;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Court Entity.
	/// </summary>
	public class Court
	{
		/// <summary>
		/// Identificator.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Navigation property for CourtSchedule entity.
		/// </summary>
		public IEnumerable<CourtSchedule> CourtSchedules { get; set; } = null!;
	}
}

