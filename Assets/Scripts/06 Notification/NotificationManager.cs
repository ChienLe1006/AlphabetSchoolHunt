
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager>
{
#if UNITY_ANDROID
    private const string CHANNEL_LUCKY = "channel_lucky";
    private const string CHANNEL_OPEN_APP = "channel_open_app";

    int identifierSpin;
    int identifierOpenApp;

    AndroidNotificationChannel luckyChanel;
    AndroidNotificationChannel openAppChanel;

    [SerializeField] private DataNotification data;

    private void Start()
    {
        InitNotiChanel();

        SendNotiOpenApp();
    }

    private void InitNotiChanel()
    {
        luckyChanel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_LUCKY,
            Name = "Spin Channel",
            Importance = Importance.Default,
            Description = "Spin notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(luckyChanel);

        openAppChanel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_OPEN_APP,
            Name = "Open App Channel",
            Importance = Importance.Default,
            Description = "Open App notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(openAppChanel);
    }

    public void SendNotiLuckyWheel()
    {
        var dataLucky = data.DictDataNoti[TypeNoti.Spin];
        int indexT = Random.Range(0, dataLucky.ListTitles.Count);
        string title = dataLucky.ListTitles[indexT];

        int indexC = Random.Range(0, dataLucky.ListContents.Count);
        string content = dataLucky.ListContents[indexC];

        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.SmallIcon = "icon_small";
        notification.LargeIcon = "icon_large";
        notification.FireTime = System.DateTime.Now.AddHours(24);

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierSpin) == NotificationStatus.Scheduled)
        {
            // Replace the currently scheduled notification with a new notification.
            AndroidNotificationCenter.UpdateScheduledNotification(identifierSpin, notification, CHANNEL_LUCKY);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierSpin) == NotificationStatus.Delivered)
        {
            //Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifierSpin);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierSpin) == NotificationStatus.Unknown)
        {
            identifierSpin = AndroidNotificationCenter.SendNotification(notification, CHANNEL_LUCKY);
        }

      
    }

    public void SendNotiOpenApp()
    {
        var dataOpen = data.DictDataNoti[TypeNoti.Spin];
        int indexT = Random.Range(0, dataOpen.ListTitles.Count);
        string title = dataOpen.ListTitles[indexT];

        int indexC = Random.Range(0, dataOpen.ListContents.Count);
        string content = dataOpen.ListContents[indexC];

        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.SmallIcon = "icon_small";
        notification.LargeIcon = "icon_large";
        notification.FireTime = System.DateTime.Now.AddHours(24);

        //identifierOpenApp = AndroidNotificationCenter.SendNotification(notification, CHANNEL_OPEN_APP);

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierOpenApp) == NotificationStatus.Scheduled)
        {
            // Replace the currently scheduled notification with a new notification.
            AndroidNotificationCenter.UpdateScheduledNotification(identifierOpenApp, notification, CHANNEL_OPEN_APP);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierOpenApp) == NotificationStatus.Delivered)
        {
            //Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifierOpenApp);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifierOpenApp) == NotificationStatus.Unknown)
        {
            identifierOpenApp = AndroidNotificationCenter.SendNotification(notification, CHANNEL_OPEN_APP);
        }
    }

#endif
}
