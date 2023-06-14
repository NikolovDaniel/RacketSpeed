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
        public int Ranking { get; set; }

        /// <summary>
        /// Biography of the player.
        /// </summary>
        public string Biography { get; set; } = null!;
    }
}

