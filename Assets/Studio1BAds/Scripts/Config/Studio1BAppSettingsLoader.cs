using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Studio1BAds.Config
{

    public class Studio1BAppSettingsLoader
    {
        private static Studio1BAppSettings appSettings;
        public static Studio1BAppSettings AppSettings
        {
            get
            {
                if (appSettings == null)
                {
                    LoadAppSettings();
                }
                return appSettings;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Init()
        {
            LoadAppSettings();

        }

        private static void LoadAppSettings()
        {
            appSettings = Resources.Load<Studio1BAppSettings>(Studio1BConstants.STUDIO1B_APP_SETTINGS_NAME);
        }

    }
}
