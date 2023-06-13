using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Training Entity.
	/// </summary>
	public class Training
	{
		/// <summary>
		/// Identificator.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the training.
		/// </summary>
		[Required]
		[StringLength(DataConstants.TrainingNameMaxLength,
			MinimumLength = DataConstants.TrainingNameMinLength,
			ErrorMessage = DataConstants.TrainingNameErrorMessage)]
		public string Name { get; set; } = null!;

		/// <summary>
		/// Start hour of the training.
		/// </summary>
		[Required]
		public DateTime Start { get; set; }

		/// <summary>
		/// End hour of the training.
		/// </summary>
		[Required]
		public DateTime End { get; set; }

		/// <summary>
		/// Identificator for Coach Entity.
		/// </summary>
		[ForeignKey(nameof(Coach))]
		public Guid CoachId { get; set; }

		/// <summary>
		/// Coach entity.
		/// </summary>
		public Coach Coach { get; set; } = null!;

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}

