using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCAlphabet : MonoBehaviour
{
    [SerializeField] private Alphabet alphabet;
    [SerializeField] private SpriteRenderer alphabetIcon;
    [SerializeField] private Transform unlockTrans;
    [SerializeField] private GameObject npc;
    [SerializeField] private Animator anim;
    private float defaultPlusHeight = 0.17f;

    [SerializeField] private GameObject money;
    [SerializeField] private Transform moneyHolder;
    [SerializeField] private Transform[] moneyPos;
    [SerializeField] private Transform mouthPos;
    [SerializeField] private List<GameObject> moneyList;

    [SerializeField] private ParticleSystem fx;

    private int moneyIndex;
    private float defaultY;

    private float timer;
    private bool isProcessing;
    private Character player;
    private PlayerDataManager playerData;
    public int AlphabetId => (int)alphabet;
    public Transform ReturnPlace => alphabetIcon.transform;
    public Transform UnlockTrans => unlockTrans;
    public int MoneyIndex { get => moneyIndex; set => moneyIndex = value; }
    public ParticleSystem FX => fx;
    public Animator Animator => anim;

    private void Start()
    {
        playerData = PlayerDataManager.Instance;
        alphabetIcon.sprite = playerData.DataTexture.GetIconAlphabet((int)alphabet);
        defaultY = moneyPos[0].position.y;
        fx.gameObject.SetActive(false);

        if (playerData.GetUnlockAlphabet(alphabet))
        {
            unlockTrans.gameObject.SetActive(false);
            npc.SetActive(true);
        }
        else
        {
            unlockTrans.gameObject.SetActive(true);
            npc.SetActive(false);
        }
    }

    private void Update()
    {
        if (isProcessing)
        {
            timer += Time.deltaTime;
            if (timer > 0.25f)
            {               
                StartCoroutine(player.ReturnAlphabetToNPC(transform.position, this));
                isProcessing = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isProcessing = true;
        Character character = other.GetComponent<Character>();
        if (character != null && character.Role.HasFlag(Role.Manual))
        {
            player = character;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isProcessing = false;
        timer = 0;
    }

    internal void ThrowMoney(int index)
    {
        if (!playerData.FirstAlphabetReturn) playerData.FirstAlphabetReturn = true;
        SoundManager.Instance.PlaySoundCastNPC();

        float x = moneyPos[index % 9].position.x;
        float y = index / 9 * defaultPlusHeight + defaultY;
        float z = moneyPos[index % 9].position.z;

        GameObject money = this.money.Spawn(mouthPos.position, Quaternion.Euler(new Vector3(0, Random.Range(-90, 90), 0)), Vector3.one, moneyHolder); //Instantiate(this.money, mouthPos.position, Quaternion.Euler(new Vector3(0, Random.Range(-90,90), 0)), moneyHolder);
        money.transform.DOJump(new Vector3(x, y, z), 5, 1, 0.5f);
        money.transform.DORotate(new Vector3(0, Random.Range(-7, 7) + transform.eulerAngles.y, 0), 0.5f);
        moneyList.Add(money);
    }

    internal IEnumerator CollectMoney()
    {
        moneyIndex = 0;
        for (int i = 0; i < moneyList.Count; i++)
        {
            moneyList[i].GetComponent<BoxCollider>().enabled = false;
        }

        for (int i = 0; i < moneyList.Count; i++)
        {
            moneyList[i].GetComponent<ReturnAlphabetMoney>().OnCollected(playerData.DataMoneyReceiveFromAlphabet.dataMoneyReceiveFromAlphabet[alphabet]);
            yield return new WaitForSeconds(0.1f);
        }
        moneyList.Clear();
    }
}
