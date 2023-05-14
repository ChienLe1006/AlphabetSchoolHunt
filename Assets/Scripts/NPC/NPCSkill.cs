using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSkill : Singleton<NPCSkill>
{
    [Header("Skill")]
    [SerializeField] private Skill skill;
    [SerializeField] private Alphabet alphabet;
    [SerializeField] private int amount;
    [SerializeField] private List<GameObject> collectedAlphabet;

    [Header("Components")]
    [SerializeField] private Transform txtCanvas;
    [SerializeField] private Text amountTxt;
    [SerializeField] private SpriteRenderer elementImg, skillImg;
    [SerializeField] private Image iconAlphabet;
 
    private float timer;
    private bool isProcessing;
    private Character player;
    private PlayerDataManager playerData;

    public int AlphabetId => (int)alphabet;
    public int NumberOfAlphabet => amount;
    public List<GameObject> CollectedAlphabet => collectedAlphabet;
    public Transform ReturnPlace => skillImg.transform;

    private void Start()
    {
        playerData = PlayerDataManager.Instance;
        SetTextAmount();
        elementImg.sprite = playerData.DataTexture.GetIconElement((int)skill);
        skillImg.sprite = playerData.DataTexture.GetIconElement((int)skill);
        iconAlphabet.sprite = playerData.DataTexture.GetIconAlphabet((int)alphabet);
    }

    private void Update()
    {
        txtCanvas.LookAt(Camera.main.transform.position);

        if (isProcessing)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                //player.ConvertAlphabetToSkill(transform.position);
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

    internal void SetTextAmount()
    {      
        amountTxt.text = $"{playerData.CurrentAmountInNPC}/{amount}";
    }
}
