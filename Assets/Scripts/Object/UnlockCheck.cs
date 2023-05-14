using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UnlockCheck : MonoBehaviour
{
    [SerializeField] private UnlockType unlockType;

    [Header("NPC Alphabet")]
    [SerializeField] private GameObject npc;
    [SerializeField] private ParticleSystem fx;
    private NPCAlphabet _npc;

    [SerializeField] private GameObject money;
    [SerializeField] private Transform content;
    [SerializeField] private Text txtMoney;
    [SerializeField] private Transform unlockTxt;
    private bool isIn;
    private float countTime;

    private PlayerDataManager playerData;
    private int tmpMoney;
    private Character player;

    private void Start()
    {
        playerData = GameManager.Instance.PlayerDataManager;
        _npc = GetComponentInParent<NPCAlphabet>();
        if (unlockType == UnlockType.Level)
        {
            txtMoney.text = playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel).ToString();
        }
        else txtMoney.text = playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId).ToString();
    }

    private void Update()
    {
        if (isIn)
        {
            countTime += Time.deltaTime;
            if (countTime > 0.5f)
            {
                UnLock();
                countTime = 0;
                isIn = false;
            }
        }

        if (unlockTxt != null) unlockTxt.LookAt(GameManager.Instance.MainCamera.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && character.IsPlayer)
        {
            isIn = true;
            player = character;
            content.DOScale(1.1f, 0.25f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && character.IsPlayer)
        {
            isIn = false;
            countTime = 0;
            content.DOScale(1f, 0.25f);
        }       
    }

    private void UnLock()
    {
        if (playerData.GetGold() > 0)
        {
            SoundManager.Instance.PlaySoundPayCash();
            if (unlockType == UnlockType.Level)
            {
                if (playerData.GetGold() > playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel))
                {
                    tmpMoney = playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel);
                }
                else
                {
                    tmpMoney = playerData.GetGold();
                }

                GameManager.Instance.Profile.AddGold(-tmpMoney, "");
                playerData.SetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel, -tmpMoney);
                SetTextMoney(playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel));
                StartCoroutine(PlayMoneyAnim());
            }
            else
            {
                if (playerData.GetGold() > playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId))
                {
                    tmpMoney = playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId);
                }
                else
                {
                    tmpMoney = playerData.GetGold();
                }

                GameManager.Instance.Profile.AddGold(-tmpMoney, "");
                playerData.SetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId, -tmpMoney);
                SetTextMoney(playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId));
                StartCoroutine(PlayMoneyAnim());
            }
        }
    }

    private Tweener tweenMoney;
    private int tmp;
    private void SetTextMoney(int money)
    {
        tweenMoney = tweenMoney ?? DOTween.To(() => tmp, x =>
        {
            tmp = x;
            txtMoney.text = tmp.ToString();
        }, money, 0.5f).SetAutoKill(false).OnComplete(() =>
        {
            if (unlockType == UnlockType.Level)
            {
                tmp = playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel);
                txtMoney.text = tmp.ToString();
                if (playerData.GetAmountMoneyUnlockLevel(GameManager.Instance.CurrentLevel) <= 0)
                {
                    GameManager.Instance.CurrentLevelManager.EndGame(LevelResult.Win);
                }
            }
            else
            {
                tmp = playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId);
                txtMoney.text = tmp.ToString();
                if (playerData.GetAmountMoneyUnlockAlphabet((Alphabet)_npc.AlphabetId) <= 0)
                {
                    GetComponent<ShowInterstitialController>().Show();
                }
                else SpawnObject();
            }
        });
        tweenMoney.ChangeStartValue(tmp + tmpMoney);
        tweenMoney.ChangeEndValue(money);
        tweenMoney.Play();
    }

    private IEnumerator PlayMoneyAnim()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject money = this.money.Spawn(player.transform.position);
            money.transform.DOJump(transform.position, 5, 1, 0.5f).OnComplete(() => money.Despawn());
            yield return new WaitForSeconds(0.1f);
        }
    }

    public async void UnlockNPC()
    {
        var level = GameManager.Instance.CurrentLevelManager;
        level.NumberAlphabetUnlock++;
        if (level.NumberAlphabetUnlock < level.Alphabets.Length) level.ActiveNPC();
        SoundManager.Instance.PlaySoundUnlockNPC();
        playerData.SetUnlockAlphabet((Alphabet)_npc.AlphabetId);
        npc.SetActive(true);
        fx.gameObject.SetActive(true);
        fx.Play();

        if (!playerData.FirstAlphabetUnlock)
        {
            playerData.FirstUnlockAlphabet = (Alphabet)_npc.AlphabetId;
            playerData.FirstNPCAlphabet = _npc;
            playerData.FirstAlphabetUnlock = true;
        }
        gameObject.SetActive(false);

        await Task.Delay(500);
        GameManager.Instance.UiController.OpenPopupNewLetter(_npc.AlphabetId);
    }

    private void SpawnObject()
    {
        GameManager.Instance.CurrentLevelManager.SpawnObject();
    }
}
