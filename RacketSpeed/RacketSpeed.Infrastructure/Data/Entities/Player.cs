using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Navigation Property.
        /// </summary>
        public PlayerImageUrl PlayerImageUrl { get; set; } = null!;

        /// <summary>
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}

