// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace MyFitness.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>("AccessToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>("AccessToken", value);
            }
        }

        public static bool HasCompletedInitialSetup
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>("HasCompletedInitialSetup", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>("HasCompletedInitialSetup", value);
            }
        }

        public static string InitialCalculationDate
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>("InitialCalculationDate", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>("InitialCalculationDate", value);
            }
        }

        public static decimal CTL {
            get
            {
                return AppSettings.GetValueOrDefault<decimal>("CTL", (decimal)1.00);
            }
            set
            {
                AppSettings.AddOrUpdateValue<decimal>("CTL", value);
            }
        }

        public static decimal ATL
        {
            get
            {
                return AppSettings.GetValueOrDefault<decimal>("ATL", (decimal)1.00);
            }
            set
            {
                AppSettings.AddOrUpdateValue<decimal>("ATL", value);
            }
        }

        public static decimal TSB
        {
            get
            {
                return AppSettings.GetValueOrDefault<decimal>("TSB", (decimal)1.00);
            }
            set
            {
                AppSettings.AddOrUpdateValue<decimal>("TSB", value);
            }
        }

        public static bool PremiumAthlete
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>("PremiumAthlete", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>("PremiumAthlete", value);
            }
        }

        public static bool CanSwim
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>("CanSwim", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>("CanSwim", value);
            }
        }

        public static double SwimThresholdPace
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>("SwimThresholdPace", 0.00);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>("SwimThresholdPace", value);
            }
        }



    }
}
