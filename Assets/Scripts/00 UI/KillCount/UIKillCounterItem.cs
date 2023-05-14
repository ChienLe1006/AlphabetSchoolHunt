using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIKillCounterItem: MonoBehaviour
{
    [SerializeField] private Image imgDead;

    public void SetAlive(bool isAlive)
    {
        imgDead.gameObject.SetActive(!isAlive);

        if (!isAlive)
        {
            imgDead.transform.DOPunchScale(Vector3.one * 1.3f, 0.5f, 5);
        }
    }
}