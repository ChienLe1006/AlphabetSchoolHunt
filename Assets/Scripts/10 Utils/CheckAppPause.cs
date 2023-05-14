using UnityEngine;

public class CheckAppPause : MonoBehaviour
{
    private static CheckAppPause instance = null;
    private float pauseBeginTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {            
            SetExitTime();
        }
        else
        {
            int sleepDuration = 10; 
            if (Time.realtimeSinceStartup > pauseBeginTime + sleepDuration)
            {
                AppOpenAdManager.Instance.ShowAdIfAvailable();
            }
        }
    }

    private void SetExitTime()
    {
        pauseBeginTime = Time.realtimeSinceStartup;
    }
}
