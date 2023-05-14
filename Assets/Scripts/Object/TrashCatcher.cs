using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCatcher : MonoBehaviour
{
    private bool checking;
    private float timer;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(60);
        GameManager.Instance.CurrentLevelManager.DeleteObject(gameObject);
    }

    private void Update()
    {
        if (checking)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                GameManager.Instance.UiController.OpenPopupTrashCatcher(this.gameObject);
                checking = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>().IsPlayer) checking = true;
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
