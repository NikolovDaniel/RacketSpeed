using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Infrastructure.Utilities
{
    public static class DataConstants
    {
        /// <summary>
        /// Data constants for Post Entity.
        /// </summary>
        public const int PostTitleMinLength = 100;
        public const int PostTitleMaxLength = 200;
        public const string PostTitleErrorMessage = "Title should be between 100 and 200 characters.";

        public const int PostContentMinLength = 800;
        public const int PostContentMaxLength = 5000;
        public const string PostContentErrorMessage = "Content should be between 800 and 5000 characters.";

        /// <summary>
        /// Data constants for Achievement Entity.
        /// </summary>
        public const int AchievementLocationMinLength = 3;
        public const int AchievementLocationMaxLength = 100;
        public const string AchievementLocationErrorMessage = "Location should be between 3 and 100 characters.";

        public const int AchievementContentMinLength = 50;
        public const int AchievementContentMaxLength = 500;
        public const string AchievementContentErrorMessage = "Content should be between 50 and 500 characters.";

        /// <summary>
        /// Data constants for Player Entity.
        /// </summary>
        public const int PlayerAgeMinValue = 6;
        public const int PlayerAgeMaxValue = 100;
        public const string PlayerAgeErrorMessage = "Age should be between 6 and 100 years.";

        public const int PlayerFirstNameMinLength = 3;
        public const int PlayerFirstNameMaxLength = 30;
        public const string PlayerFirstNameErrorMessage = "First name should be between 3 and 30 characters.";
        
        public const int PlayerLastNameMinLength = 3;
        public const int PlayerLastNameMaxLength = 30;
        public const string PlayerLastNameErrorMessage = "Last name should be between 3 and 30 characters.";

        public const int PlayerRankingMinValue = 1;
        public const int PlayerRankingMaxValue = 5000;
        public const string PlayerNationalRankingErrorMessage = "National Ranking should be between 1st and 5000th place.";
        public const string PlayerWorldRankingErrorMessage = "World Ranking should be between 1st and 5000th place.";

        public const int PlayerBiographyMinLength = 20;
        public const int PlayerBiographyMaxLength = 1000;
        public const string PlayerBiographyErrorMessage = "Biography should be between 20 and 1000 characters";

        public const int PlayerBirthPlaceMinLength = 5;
        public const int PlayerBirthPlaceMaxLength = 200;
        public const string PlayerBirthPlaceErrorMessage = "Birth place should be between 5 and 200 characters";

        public const int PlayerHeightMinValue = 90;
        public const int PlayerHeightMaxValue = 230;
        public const string PlayerHeightErrorMessage = "Height should be between 90cm and 230cm.";

        /// <summary>
        /// Data constants for Coach Entity.
        /// </summary>
        public const int CoachFirstNameMinLength = 3;
        public const int CoachFirstNameMaxLength = 30;
        public const string CoachFirstNameErrorMessage = "First name should be between 3 and 30 characters.";

        public const int CoachLastNameMinLength = 3;
        public const int CoachLastNameMaxLength = 30;
        public const string CoachLastNameErrorMessage = "Last name should be between 3 and 30 characters.";

        public const int CoachBiographyMinLength = 20;
        public const int CoachBiographyMaxLength = 1000;
        public const string CoachBiographyErrorMessage = "Biography should be between 20 and 1000 characters";


        public const int TrainingNameMinLength = 5;
        public const int TrainingNameMaxLength = 30;
        public const string TrainingNameErrorMessage = "Training name should be between 5 and 30 characters";

        /// <summary>
        /// Data constants for ApplicationUser Entity.
        /// </summary>
        public const int UserFirstNameMinLength = 3;
        public const int UserFirstNameMaxLength = 50;
        public const string UserFirstNameErrorMessage = "First name should be between 3 and 50 characters.";

        public const int UserLastNameMinLength = 3;
        public const int UserLastNameMaxLength = 50;
        public const string UserLastNameErrorMessage = "Last name should be between 3 and 50 characters.";

    }
}

