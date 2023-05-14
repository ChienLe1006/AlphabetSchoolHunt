using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKillCounter : MonoBehaviour
{
    [SerializeField] private UIKillCounterItem killCounterPrefab;

    private List<UIKillCounterItem> killCounterItems = new List<UIKillCounterItem>();
    private int killCount;

    private void Awake()
    {
        Character.OnDie += OnCharacterDie;
        GameManager.Instance.OnRevive += OnRevive;
    }

    public void Init()
    {
        int hiderCount = GameManager.Instance.CurrentLevelManager.Seekers.Length - 1;
        killCount = hiderCount;
        for (int i = 0; i < hiderCount; i++)
        {
            if (i >= killCounterItems.Count)
            {
                killCounterItems.Add(Instantiate(killCounterPrefab, transform));
            }
            killCounterItems[i].gameObject.SetActive(true);
            killCounterItems[i].SetAlive(true);
        }

        for (int i = hiderCount; i < killCounterItems.Count; i++)
        {
            killCounterItems[i].gameObject.SetActive(false);
        }
    }

    private void OnCharacterDie(Character character)
    {
        if (!character.Role.HasFlag(Role.Seeker))
            return;

        if (!character.IsPlayer) killCounterItems[--killCount].SetAlive(false);
    }

    private void OnRevive()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
            killCounterItems[killCount++].SetAlive(true);
    }
}
