using System;
namespace RacketSpeed.Core.Models.Player
{
    /// <summary>
    /// Class to hold all Player Entities and the count of pages needed.
    /// </summary>
    public class PlayersPaginationCountViewModel
    {
        /// <summary>
        /// Page number.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Collection of PlayerViewModel Entities.
        /// </summary>
        public IEnumerable<PlayerViewModel> Players { get; set; } = null!;
    }
}

