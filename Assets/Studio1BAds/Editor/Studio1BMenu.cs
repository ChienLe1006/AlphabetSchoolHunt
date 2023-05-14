using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Studio1BAds;
using Studio1BAds.Config;

public class Studio1BMenu : Editor
{
    [MenuItem("Studio1B/App Settings")]
    public static void Settings() {
        if (!Directory.Exists(Studio1BConstants.STUDIO1B_RESOURCES_PATH))
        {
            Directory.CreateDirectory(Studio1BConstants.STUDIO1B_RESOURCES_PATH);
        }
        Studio1BAppSettings appSettings = Resources.Load<Studio1BAppSettings>(Studio1BConstants.STUDIO1B_APP_SETTINGS_NAME);
        if (appSettings == null) {
            appSettings = CreateInstance<Studio1BAppSettings>();
            AssetDatabase.CreateAsset(appSettings, Studio1BAppSettings.STUDIO1B_CONFIGURATION_ASSET_PATH);
            appSettings = Resources.Load<Studio1BAppSettings>(Studio1BConstants.STUDIO1B_APP_SETTINGS_NAME);
        }
        Selection.activeObject = appSettings;
    }
}
