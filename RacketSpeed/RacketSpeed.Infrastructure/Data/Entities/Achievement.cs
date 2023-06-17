using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Achievement Entity.
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
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

        /// <summary>
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

