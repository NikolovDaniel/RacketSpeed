using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Training
{
	public class TrainingViewModel
	{
        /// <summary>
        /// Name of the training.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Start hour of the training.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End hour of the training.
        /// </summary>
        public DateTime End { get; set; }
    }
}

