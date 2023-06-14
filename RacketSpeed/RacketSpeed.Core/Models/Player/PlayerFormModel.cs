using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Player
{
    /// <summary>
    /// Player form model, holds validation for CRUD operations.
    /// </summary>
    public class PlayerFormModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
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

    }
}

