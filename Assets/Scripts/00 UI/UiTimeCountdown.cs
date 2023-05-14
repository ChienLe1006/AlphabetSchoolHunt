using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTimeCountdown : MonoBehaviour
{
    private int timeCountdown = 3;
    private float time;
    private int indexTime;

    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtTitle;

    public void Setup()
    {
        time = 0;
        indexTime = timeCountdown;
        txtTime.text = timeCountdown.ToString();
        this.gameObject.SetActive(true);

        if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
        {
            txtTitle.text = "Time to hide";
        }
        else
        {
            txtTitle.text = "Starting in";
        }
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            time = 0;
            indexTime--;
            indexTime = indexTime > 0 ? indexTime : 0;

            txtTime.text = indexTime.ToString();

            if (indexTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }


    }
}
