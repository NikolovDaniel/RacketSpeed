﻿namespace RacketSpeed.Infrastructure.Utilities
{
    public static class DataConstants
    {
        /// <summary>
        /// Data constants for Required Fields.
        /// </summary>
        public const string RequiredFieldErrorMessage = "Полето е задължително.";

        /// <summary>
        /// Data constants for Post Entity.
        /// </summary>
        public const int PostTitleMinLength = 100;
        public const int PostTitleMaxLength = 200;
        public const string PostTitleErrorMessage = "Заглавието трябва да съдържа между 100 и 200 символа.";

        public const int PostContentMinLength = 800;
        public const int PostContentMaxLength = 5000;
        public const string PostContentErrorMessage = "Съдържанието трябва да съдържа между 800 и 5000 символа.";

        /// <summary>
        /// Data constants for Achievement Entity.
        /// </summary>
        public const int AchievementLocationMinLength = 3;
        public const int AchievementLocationMaxLength = 100;
        public const string AchievementLocationErrorMessage = "Локацията трябва да съдържа между 3 и 100 букви.";

        public const int AchievementContentMinLength = 50;
        public const int AchievementContentMaxLength = 500;
        public const string AchievementContentErrorMessage = "Съдържанието трябва да съдържа между 50 и 500 символа.";

        /// <summary>
        /// Data constants for Player Entity.
        /// </summary>
        public const int PlayerAgeMinValue = 6;
        public const int PlayerAgeMaxValue = 100;
        public const string PlayerAgeErrorMessage = "Възрастта трябва да бъде между 6 и 100 години.";

        public const int PlayerFirstNameMinLength = 3;
        public const int PlayerFirstNameMaxLength = 30;
        public const string PlayerFirstNameErrorMessage = "Първото име трябва да съдържа между 3 и 30 букви.";
        
        public const int PlayerLastNameMinLength = 3;
        public const int PlayerLastNameMaxLength = 30;
        public const string PlayerLastNameErrorMessage = "Фамилното име трябва да съдържа между 3 и 30 букви.";

        public const int PlayerRankingMinValue = 1;
        public const int PlayerRankingMaxValue = 5000;
        public const string PlayerNationalRankingErrorMessage = "Националният ранкинг трябва да бъде между 1 и 5000-то място.";
        public const string PlayerWorldRankingErrorMessage = "Световният ранкинг трябва да бъде между 1 и 5000-то място.";

        public const int PlayerBiographyMinLength = 20;
        public const int PlayerBiographyMaxLength = 1000;
        public const string PlayerBiographyErrorMessage = "Биографията трябва да съдържа между 20 и 1000 символа.";

        public const int PlayerBirthPlaceMinLength = 5;
        public const int PlayerBirthPlaceMaxLength = 200;
        public const string PlayerBirthPlaceErrorMessage = "Мястото на раждана трябва да съдържа между 5 и 200 букви.";

        public const int PlayerHeightMinValue = 90;
        public const int PlayerHeightMaxValue = 230;
        public const string PlayerHeightErrorMessage = "Височината трябва да бъде между 90 и 230 сантиметра.";

        /// <summary>
        /// Data constants for Coach Entity.
        /// </summary>
        public const int CoachFirstNameMinLength = 3;
        public const int CoachFirstNameMaxLength = 30;
        public const string CoachFirstNameErrorMessage = "Първото име трябва да съдържа между 3 и 30 букви.";

        public const int CoachLastNameMinLength = 3;
        public const int CoachLastNameMaxLength = 30;
        public const string CoachLastNameErrorMessage = "Фамилното име трябва да съдържа между 3 и 30 букви.";

        public const int CoachBiographyMinLength = 20;
        public const int CoachBiographyMaxLength = 1000;
        public const string CoachBiographyErrorMessage = "Биографията трябва да съдържа между 20 и 1000 символа.";

        /// <summary>
        /// Data constants for Training Entity.
        /// </summary>
        public const int TrainingNameMinLength = 4;
        public const int TrainingNameMaxLength = 30;
        public const string TrainingNameErrorMessage = "Името на тренировката трябва да съдържа между 4 и 30 символа.";

        public const int TrainingDayOfWeekMinLength = 5;
        public const int TrainingDayOfWeekMaxLength = 10;
        public const string TrainingDayOfWeekErrorMessage = "Денят трябва да съдържа между 5 и 10 букви.";

        /// <summary>
        /// Data constants for ApplicationUser Entity.
        /// </summary>
        public const int UserFirstNameMinLength = 3;
        public const int UserFirstNameMaxLength = 50;
        public const string UserFirstNameErrorMessage = "Първото име трябва да съдържа между 3 и 50 букви.";

        public const int UserLastNameMinLength = 3;
        public const int UserLastNameMaxLength = 50;
        public const string UserLastNameErrorMessage = "Фамилното име трябва да съдържа между 3 и 50 букви.";

        /// <summary>
        /// Data constants for Event Entity.
        /// </summary>
        public const int EventTitleMinLength = 50;
        public const int EventTitleMaxLength = 200;
        public const string EventTitleErrorMessage = "Заглавието трябва да съдържа между 50 и 200 символа.";

        public const int EventCategoryMinLength = 5;
        public const int EventCategoryMaxLength = 30;
        public const string EventCategoryErrorMessage = "Категорията трябва да съдържа между 5 и 30 символа.";

        public const int EventLocationMinLength = 10;
        public const int EventLocationMaxLength = 100;
        public const string EventLocationErrorMessage = "Локацията трябва да съдържа между 10 и 100 символа.";

        public const int EventContentMinLength = 800;
        public const int EventContentMaxLength = 5000;
        public const string EventContentErrorMessage = "Съдържанието трябва да съдържа между 800 и 5000 символа.";

        /// <summary>
        /// Data constants for SignKid Entity.
        /// </summary>
        public const int SignKidFullNameMinLength = 7;
        public const int SignKidFullNameMaxLength = 100;
        public const string SignKidFullNameErrorMessage = "Имената трябва да съдържат между 7 и 100 букви.";

        public const string SignKidPhoneNumberErrorMessage = "Телефонът трябва да бъде в този формат: +359886005040";

        public const int SignKidEmailAddressMinLength = 8;
        public const int SignKidEmailAddressMaxLength = 100;
        public const string SignKidEmailAddressErrorMessageOnInvalidEmail = "Невалиден емайл адрес.";
        public const string SignKidEmailAddressErrorMessageOnInvalidLength = "Емайл адресът трябва да съдържа между 8 и 100 символа.";

        public const int SignKidMessageMinLength = 5;
        public const int SignKidMessageMaxLength = 200;
        public const string SignKidMessageErrorMessage = "Съобщението трябва да съдържа между 5 и 200 символа.";

        /// <summary>
        /// Data constants for Reservation Entity.  
        /// </summary>
        public const int ReservationPeopleCountMinValue = 2;
        public const int ReservationPeopleCountMaxValue = 4;
        public const string ReservationPeopleCountErrorMessage = "Играчите трябва да бъдат между 2 и 4.";

        public const int ReservationRacketsBookedMinValue = 0;
        public const int ReservationRacketsBookedMaxValue = 4;
        public const string ReservationRacketsBookedErrorMessage = "Ракетите под наем трябва да бъдат между 0 и 4.";

        public const int ReservationLocationMinValue = 5;
        public const int ReservationLocationMaxValue = 30;
        public const string ReservationLocationErrorMessage = "Името на обекта трябва да съдържа между 5 и 30 символа.";

        public const int ReservationCourtNumberMinValue = 1;
        public const int ReservationCourtNumberMaxValue = 4;
        public const string ReservationCourtNumberErrorMessage = "Корта може да бъде между 1 и 4 номер.";
    }
}

