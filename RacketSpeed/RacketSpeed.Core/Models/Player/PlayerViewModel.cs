using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Player
{
    /// <summary>
    /// Player view model for presenting.
    /// </summary>
	public class PlayerViewModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Birthday of the player.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Birth place of the player.
        /// </summary>
        public string BirthPlace { get; set; } = null!;

        /// <summary>
        /// Biography of the player.
        /// </summary>
        public string Biography { get; set; } = null!;

        /// <summary>
        /// Playing hand of the player.
        /// </summary>
        public string PlayingHand { get; set; } = null!;

        /// <summary>
        /// Height of the player.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Date of when the player has entered the club.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// First name of the player.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the player.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// World Ranking of the player.
        /// </summary>
        public int WorldRanking { get; set; }

        /// <summary>
        /// National Ranking of the player.
        /// </summary>
        public int NationalRanking { get; set; }

        /// <summary>
        /// Player image url.
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Property to pass the needed Page Count.
        /// </summary>
        public int PageCount { get; set; }
    }
}

