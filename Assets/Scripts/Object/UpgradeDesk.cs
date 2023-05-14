using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDesk : MonoBehaviour
{
    private bool checking;
    private float timer;

    private void Update()
    {
        if (checking)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                GameManager.Instance.UiController.OpenPopupUpgrade();
                checking = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>().IsPlayer)
        {
            checking = true;
            if (!PlayerDataManager.Instance.FirstGoToUpgrade) PlayerDataManager.Instance.FirstGoToUpgrade = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>().IsPlayer)
        {            
            checking = false;
            timer = 0f;
        }
    }
}
