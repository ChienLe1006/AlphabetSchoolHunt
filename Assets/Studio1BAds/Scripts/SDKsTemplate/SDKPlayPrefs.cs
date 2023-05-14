using System;
using UnityEngine;

public class SDKPlayPrefs
{
    public static readonly DateTime zeroTime = new DateTime(0);

    public static DateTime GetDateTime(string key, DateTime def)
    {
        string @string = PlayerPrefs.GetString(key);
        DateTime result = def;
        if (!string.IsNullOrEmpty(@string))
        {
            long dateData = Convert.ToInt64(@string);
            result = DateTime.FromBinary(dateData);
        }
        return result;
    }

    public static void SetDateTime(string key, DateTime val)
    {
        PlayerPrefs.SetString(key, val.ToBinary().ToString());
    }
}