// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

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

        public static bool HasCompletedSevenDays
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>("SevenDaysComplete", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>("SevenDaysComplete", value);
            }
        }

        public static bool HasInitialCalculation
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>("initalCalculation", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>("initalCalculation", value);
            }
        }

    }
}