using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// PlayerImageUrl Entity.
    /// </summary>
    public class PlayerImageUrl
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Url for image resource.
        /// </summary>
        [Required]
        public string Url { get; set; } = null!;

        /// <summary>
        /// Foreign key for Player.
        /// </summary>
        [ForeignKey(nameof(Player))]
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Player Player { get; set; } = null!;
    }
}

