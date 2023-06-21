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
        /// Birth place of the player.
        /// </summary>
        [Required]
        [StringLength(DataConstants.PlayerBirthPlaceMaxLength,
            MinimumLength = DataConstants.PlayerBirthPlaceMinLength,
            ErrorMessage = DataConstants.PlayerBirthPlaceErrorMessage)]
        public string BirthPlace { get; set; } = null!;

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
        /// Playing hand of the player.
        /// </summary>
        [Required]
        public string PlayingHand { get; set; } = null!;

        /// <summary>
        /// Height of the player.
        /// </summary>
        [Required]
        [Range(DataConstants.PlayerHeightMinValue,
            DataConstants.PlayerHeightMaxValue,
            ErrorMessage = DataConstants.PlayerHeightErrorMessage)]
        public int Height { get; set; }

        /// <summary>
        /// National Ranking of the player.
        /// </summary>
        [Required]
        [Range(DataConstants.PlayerRankingMinValue,
            DataConstants.PlayerRankingMaxValue,
            ErrorMessage = DataConstants.PlayerNationalRankingErrorMessage)]
        public int NationalRanking { get; set; }

        /// <summary>
        /// World Ranking of the player.
        /// </summary>
        [Required]
        [Range(DataConstants.PlayerRankingMinValue,
            DataConstants.PlayerRankingMaxValue,
            ErrorMessage = DataConstants.PlayerWorldRankingErrorMessage)]
        public int WorldRanking { get; set; }

        /// <summary>
        /// Biography of the player.
        /// </summary>
        [Required]
        [StringLength(DataConstants.PlayerBiographyMaxLength,
            MinimumLength = DataConstants.PlayerBiographyMinLength,
            ErrorMessage = DataConstants.PlayerBiographyErrorMessage)]
        public string Biography { get; set; } = null!;

        /// <summary>
        /// Date for when the player has entered the club.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// ImageUrl for player's image.
        /// </summary>
        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}

