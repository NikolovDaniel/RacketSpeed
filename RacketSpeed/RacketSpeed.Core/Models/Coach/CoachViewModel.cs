using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Coach
{
    /// <summary>
    /// Coach view model for presenting.
    /// </summary>
    public class CoachViewModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the Coach.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the Coach.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Coach image url.
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Biography of the Coach.
        /// </summary>
        public string Biography { get; set; } = null!;
    }
}

