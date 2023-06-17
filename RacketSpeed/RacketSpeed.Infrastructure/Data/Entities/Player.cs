using System;
using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Player Entity.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Birthday of the player.
        /// </summary>
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// First name of the player.
        /// </summary>
        [Required]
        [StringLength(DataConstants.PlayerFirstNameMaxLength,
            MinimumLength = DataConstants.PlayerFirstNameMinLength,
            ErrorMessage = DataConstants.PlayerFirstNameErrorMessage)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the player.
        /// </summary>
        [Required]
        [StringLength(DataConstants.PlayerLastNameMaxLength,
            MinimumLength = DataConstants.PlayerLastNameMinLength,
            ErrorMessage = DataConstants.PlayerLastNameErrorMessage)]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// World Ranking of the player.
        /// </summary>
        [Required]
        [Range(DataConstants.PlayerRankingMinValue,
            DataConstants.PlayerRankingMaxValue,
            ErrorMessage = DataConstants.PlayerRankingErrorMessage)]
        public int Ranking { get; set; }

        /// <summary>
        /// Biography of the player.
        /// </summary>
        [Required]
        [StringLength(DataConstants.PlayerBiographyMaxLength,
            MinimumLength = DataConstants.PlayerBiographyMinLength,
            ErrorMessage = DataConstants.PlayerBiographyErrorMessage)]
        public string Biography { get; set; } = null!;

        /// <summary>
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}

