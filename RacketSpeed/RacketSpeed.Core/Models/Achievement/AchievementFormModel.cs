using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Achievement
{
    /// <summary>
    /// Achievement form model, holds validation for CRUD operations.
    /// </summary>
    public class AchievementFormModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date of the achievement.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Location of the event.
        /// </summary>
        [Required]
        [StringLength(DataConstants.AchievementLocationMaxLength,
            MinimumLength = DataConstants.AchievementLocationMinLength,
            ErrorMessage = DataConstants.AchievementLocationErrorMessage)]
        public string Location { get; set; } = null!;

        /// <summary>
        /// Description of the achievement.
        /// </summary>
        [Required]
        [StringLength(DataConstants.AchievementContentMaxLength,
            MinimumLength = DataConstants.AchievementContentMinLength,
            ErrorMessage = DataConstants.AchievementContentErrorMessage)]
        public string Content { get; set; } = null!;

    }
}

