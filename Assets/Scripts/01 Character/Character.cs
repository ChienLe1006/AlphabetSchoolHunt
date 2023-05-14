using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;
using DG.Tweening;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator aliveAnimator;
    [SerializeField] private GameObject aliveCharacter;
    [SerializeField] private GameObject introCharacter;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent navMeshAgent;   
    [SerializeField] private Transform maxAlert;
    [SerializeField] private LockAlert lockAlert;

    [SerializeField] private SkinCharaterController skinCharater;
    [SerializeField] private MeshFilter fovMeshFilter;
    [SerializeField] private AlphabetHolder alphabetHolder;

    [Header("Prefabs")]
    [SerializeField] private GameObject overSpeedFX;
    [SerializeField] private GameObject smokeMovementFX;
    [SerializeField] private Indicator suspiciousIndicator;
    [SerializeField] private GameObject alphabetCollectedPopup, alphabetReturnedPopup;
    [SerializeField] private GameObject[] alphabets;

    [Header("Properties")]
    [SerializeField] private FieldOfViewParameter fieldOfViewParameter;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float hearingRadius = 5f;
    [SerializeField] private Role role;
    [SerializeField] private float attackDamage;
    [SerializeField] private float hp;
    private float maxHP;
    private Vector3 originScale;

    [Header("Skills")]
    [SerializeField] private GameObject skill;
    [SerializeField] private List<GameObject> skillList;
    

    private FieldOfView fieldOfView;
    private MoverFSM mover;
    private CharacterProperty characterProperty;
    public GameObject Tongue { get; set; }
    public GameObject Shadow { get; set; }
    public Transform MaxAlert => maxAlert;
    public LockAlert LockAlert => lockAlert;

    private float lastAttackTime;
    private bool canMove;
    private bool isPlayingVFX;
    private bool isDead;
    private bool isAttacking;

    private int idAlphabet;
    public Alphabet Alphabet => (Alphabet)idAlphabet;

    public static event Action<Character> OnDie;

    public Animator CharacterAnimator => aliveAnimator;
    public GameObject AliveCharacter => aliveCharacter;
    public GameObject IntroCharacter => introCharacter;
    public FieldOfView FieldOfView => fieldOfView;

    public Role Role => role;
    public float HearingRadius => CharacterProperty.HearingRadius;
    public float AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }
    public float HP
    {
        get { return hp; } 
        set { hp = value; }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
 
    public bool IsAlive => !isDead; //aliveCharacter && aliveCharacter.activeSelf;
    public bool IsPlayer => (Role & Role.Manual) == Role.Manual;
    public bool IsAttacking => Time.time - lastAttackTime <= 1.5f;
    public bool CanHurt => PlayerDataManager.Instance.GetUnlockAlphabet(Alphabet);
    public bool IsAttack
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public float Speed
    {
        get
        {
            return CharacterProperty.Speed;
        }
    }

    public bool CanMove
    {
        get => canMove;
        set
        {
            canMove = value;
            if (navMeshAgent && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = !canMove;
                navMeshAgent.updatePosition = canMove;
                if (canMove)
                {
                    navMeshAgent.Warp(transform.position);
                }
            }
            else
            {
                SoundManager.Instance.StopFootStep();
            }
        }
    }

    public bool CanKill { get; set; } = true;

    public bool CanDie { get; set; } = true;

    public CharacterController CharacterController => characterController;
    public NavMeshAgent NavMeshAgent
    {
        get
        {
            if (navMeshAgent == null)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }
            return navMeshAgent;
        }
    }

    public CharacterProperty CharacterProperty
    {
        set => characterProperty = value;
        get
        {
            if (characterProperty == null)
            {
                characterProperty = new CharacterProperty(fieldOfViewParameter, speed, hearingRadius, role, attackDamage, hp);
            }

            return characterProperty;
        }
    }

    public SkinCharaterController SkinCharater => skinCharater;

    public GameObject SmokeMovementFX => smokeMovementFX;

    public Indicator SuspiciousIndicator => suspiciousIndicator;

    public MoverFSM Mover => mover;

    public GameObject AliveMovingPart { get; set; }    
    public AlphabetHolder AlphabetHolder => alphabetHolder;

    private void Start()
    {
        GameManager.Instance.OnLevelEnd += OnLevelEnd;
        AttackDamage = characterProperty.AttackDamage;
        HP= characterProperty.HP;
        MaxHP = characterProperty.HP;
        speed = characterProperty.Speed;
        skill.SetActive(false);

        if (IsPlayer)
        {           
            fieldOfView = new FieldOfView(CharacterProperty.FieldOfViewParameter, this, this.fovMeshFilter);
            MeshCollider fovMesh = fovMeshFilter.gameObject.AddComponent<MeshCollider>();
            fovMesh.convex = true;
            fovMesh.isTrigger = true;

            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            box.center = new Vector3(0, 0.5f, 0);
            box.size = new Vector3(0.75f, 1, 0.5f);

            for (int i = 0; i < Enum.GetNames(typeof(Alphabet)).Length; i++)
            {
                if (PlayerDataManager.Instance.GetAlphabetAmountInBag(i) > 0)
                {
                    int alphabetAmountInBag = PlayerDataManager.Instance.GetAlphabetAmountInBag(i);
                    for (int j = 0; j < alphabetAmountInBag; j++)
                    {
                        GameObject alphabet = alphabets[i].Spawn(alphabetHolder.transform);
                        alphabet.transform.localEulerAngles = new Vector3(-90, -90, 0);
                        alphabetHolder.Rerange();
                    }
                }
            }
        }
        else fovMeshFilter.gameObject.SetActive(false);
    }

    public void Renew(bool firstSpawn = false)
    {              
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        GetComponentInChildren<Animator>().Rebind();
        if (!firstSpawn) transform.localScale = originScale;
        characterController.enabled = true;
        isDead = false;
        aliveCharacter.SetActive(true);
        CanMove = true;

        //fieldOfView.DeleteFieldOfView();
        StopSkillVFX();
        lastAttackTime = 0;

        hp = maxHP;

        if (IsPlayer)
        {
            aliveCharacter.SetActive(false);
            introCharacter.SetActive(true);
            skinCharater.ChangeSkinCharacter(PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.SKIN));           
        }
    }

    public void SetSkinAlphabet(int id)
    {
        if (!IsPlayer)
        {
            skinCharater.InitAlphabet(id);
            idAlphabet = id;
        }
        else
        {
            if (PlayerDataManager.Instance.FirstSkillUnlock)
            {
                skinCharater.ChangeElementLogo(PlayerDataManager.Instance.GetIdEquipElement());
            }
        }
        GameObject player = aliveCharacter.transform.GetChild(0).gameObject;
        GameObject enemy = aliveCharacter.transform.GetChild(id + 1).gameObject;
        if (IsPlayer)
        {
            player.SetActive(true);
            enemy.SetActive(false);
            aliveAnimator = player.GetComponent<Animator>();
        }
        else
        {
            player.SetActive(false);
            enemy.SetActive(true);
            aliveAnimator = enemy.GetComponent<Animator>();
        }
    }

    public void SetSkill(int id)
    {
        if (!IsPlayer)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i] != null) skillList[i].SetActive(false);
            }
            skillList[id].SetActive(true);
        }
        else
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i] != null) skillList[i].SetActive(false);
            }
            if (PlayerDataManager.Instance.GetIdEquipElement() == 0)
            {
                if (PlayerDataManager.Instance.FirstSkillUnlock)
                {
                    skillList[PlayerDataManager.Instance.GetIdEquipElement()].SetActive(true);
                }
            }
            else
            {
                skillList[PlayerDataManager.Instance.GetIdEquipElement()].SetActive(true);
            }
        }
    }

    //private void InitPet()
    //{
    //    int id = GameManager.Instance.PlayerDataManager.GetIdEquipSkin(TypeEquipment.PET);

    //    if ((petController && petController.Id != id) || (id != -1 && !petController))
    //    {
    //        if (petController)
    //        {
    //            Destroy(petController.gameObject);
    //        }
    //        var pet = Resources.Load<PetController>("Pets/" + (TypePet)id);
    //        petController = Instantiate(pet);
    //        petController.Id = id;
    //    }

    //    if (petController)
    //        petController.Init(this);

    //    if (petController)
    //    {
    //        petController.gameObject.SetActive(true);
    //    }
    //}

    private void OnLevelEnd(LevelResult levelResult)
    {
        if (!IsAlive)
        {
            return;
        }

        aliveAnimator.SetBool(CharacterAction.Run.ToAnimatorHashedKey(), false);

        fieldOfView?.DeleteFieldOfView();

        Character player = GameManager.Instance.CurrentLevelManager.Player;
        switch (levelResult)
        {
            case LevelResult.Win:
                aliveAnimator.SetTrigger(
                    Role == (player.Role ^ Role.Manual) || Role.HasFlag(Role.Manual) ?
                    CharacterAction.Victory.ToAnimatorHashedKey() : CharacterAction.Tremble.ToAnimatorHashedKey());
                break;
            case LevelResult.Lose:
                aliveAnimator.SetTrigger(
                    Role == (player.Role ^ Role.Manual) || Role.HasFlag(Role.Manual) ?
                    CharacterAction.Tremble.ToAnimatorHashedKey() : CharacterAction.Victory.ToAnimatorHashedKey());
                break;
            case LevelResult.NotDecided:
            default:
                break;
        }
    }

    public void SetRole(Role role)
    {
        this.role = role;
        mover = new MoverFSM(this);
    }

    public void Move()
    {
        if (!IsAlive)
            return;

        if (Role == Role.NotSet)
        {
            return;
        }

        mover.Move();
    }

    public void DoRoleAction()
    {
        if (!IsAlive)
            return;

        if (CanMove)
            Move();

        if (Role.HasFlag(Role.Manual))
        {
            if (CanKill)
            {
                if (PlayerDataManager.Instance.CurrentAlphabetAmountInBag < PlayerDataManager.Instance.BagCapacity)
                {
                    fieldOfView.DrawFieldOfViewAndAttack();
                }
                else fieldOfView.DeleteFieldOfView();
            }
            //else
            //{
            //    fieldOfView.DeleteFieldOfView();
            //}
        }
    }

    public void Kill(Character character, Transform player)
    {
        if (!character.IsAlive)
            return;

        if (!CanKill)
            return;

        if (!character.CanDie)
            return;

        lastAttackTime = Time.time;
        //this.aliveAnimator.SetTrigger(CharacterAction.Attack.ToAnimatorHashedKey());
        //SoundManager.Instance.PlaySoundBite(transform.position);
        if (Role.HasFlag(Role.Manual))
        {
            character.Die(player);
            aliveAnimator.SetBool(CharacterAction.Chase.ToAnimatorHashedKey(), false);
        }
        else character.Die();
    }

    private void Die(Transform player = null)
    {
        if (!CanDie)
            return;

        characterController.enabled = false;
        //fieldOfView.DeleteFieldOfView();
        StopSkillVFX();
        
        //aliveCharacter.SetActive(false);
        //deathCharacter.SetActive(true);
        //deathAnimator.Play(CharacterAction.Die.ToAnimatorHashedKey());
        isDead = true;
        aliveAnimator.SetTrigger(CharacterAction.Die.ToAnimatorHashedKey());
        aliveAnimator.SetFloat("DieMotion", RandomDieAnim);
        OnDie?.Invoke(this);

        if (!PlayerDataManager.Instance.FirstAlphabetKill && player != null)
        {
            PlayerDataManager.Instance.FirstAlphabetKill = true;
        }
        if (Role.HasFlag(Role.Manual))
        {
            SoundManager.Instance.StopFootStep();
        }
        if (player) StartCoroutine(Helper.StartAction(() => AlphabetCollected(player), 1f));
    }

    private float[] dieAnim = new float[6] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
    private float RandomDieAnim => dieAnim[UnityEngine.Random.Range(0, dieAnim.Length)];
    
    private void AlphabetCollected(Transform player)
    {
        originScale = transform.localScale;
        transform.DOJump(player.position, 3, 1, 0.5f);
        transform.DOScale(0, 0.5f).OnComplete(OnCollectedComplete(player));        
    }

    private TweenCallback OnCollectedComplete(Transform player)
    {
        return () => 
        {
            SoundManager.Instance.PlaySoundDie();
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            transform.localScale = Vector3.one;

            var playerData = PlayerDataManager.Instance;
            if (playerData.CurrentAlphabetAmountInBag < playerData.BagCapacity && GameManager.Instance.GameStateController.currentState.GetId() != (int)GameState.END_GAME)
            {
                playerData.CurrentAlphabetAmountInBag += 1;

                GameObject alphabet = alphabets[idAlphabet].Spawn(player.GetComponent<Character>().AlphabetHolder.transform);
                alphabet.transform.localEulerAngles = new Vector3(-90, -90, 0);
                player.GetComponent<Character>().AlphabetHolder.Rerange();

                GameObject alphabetPopup = alphabetCollectedPopup.Spawn(player.transform.position);
                alphabetPopup.GetComponent<AlphabetCollectedPopup>().Init(idAlphabet);
                playerData.AddAlphabetAmountInBag(idAlphabet, 1);
                if (playerData.actionUITop != null)
                {
                    playerData.actionUITop(TypeItemAnim.Bag);
                }
 
                PopupCollectedAlphabet.Instance.UpdateDisplay();
            }           
        };
    }

    internal IEnumerator ReturnAlphabetToNPC(Vector3 npcTrans, NPCAlphabet npc)
    {
        foreach (var character in GameManager.Instance.CurrentLevelManager.Characters)
        {
            if (!character.IsAlive) character.Renew(false);
        }

        var playerData = PlayerDataManager.Instance;
        var holdingAlphabet = alphabetHolder.transform;

        if (playerData.GetAlphabetAmountInBag(npc.AlphabetId) > 0)
        {
            SoundManager.Instance.PlaySoundCheckoutNPC();
            npc.Animator.SetTrigger("Eat");

            for (int i = 0; i < holdingAlphabet.childCount; i++)
            {
                if ((int)holdingAlphabet.GetChild(i).GetComponent<HoldingAlphabet>().AlphabetName == npc.AlphabetId)
                {
                    npc.FX.gameObject.SetActive(true);
                    npc.FX.Play();
                    npc.MoneyIndex++;

                    holdingAlphabet.GetChild(i).GetComponent<HoldingAlphabet>().ReturnToNpc(npcTrans, this, AlphabetReturnCallBack(npc, i, npc.MoneyIndex - 1));
                    playerData.CurrentAlphabetAmountInBag -= 1;
                    playerData.AddAlphabetAmountInBag(npc.AlphabetId, -1);
                    yield return new WaitForSeconds(0.1f);
                }
                npc.FX.Stop();
            }
        }
    }

    //internal void ConvertAlphabetToSkill(Vector3 npcTrans)
    //{
    //    StartCoroutine(ReturnAlphabetToNPC(npcTrans));
    //}

    //private IEnumerator ReturnAlphabetToNPC(Vector3 npcTrans)
    //{
    //    foreach (var character in GameManager.Instance.CurrentLevelManager.Characters)
    //    {
    //        if (!character.IsAlive) character.Renew(false);
    //    }

    //    var playerData = PlayerDataManager.Instance;
    //    var collectedAlphabet = NPCSkill.Instance.CollectedAlphabet;
    //    if (collectedAlphabet.Count != 0)
    //    {
    //        for (int i = 0; i < collectedAlphabet.Count; i++)
    //        {
    //            if (collectedAlphabet[i].GetComponent<Character>().idAlphabet == NPCSkill.Instance.AlphabetId)
    //            {
    //                AlphabetReturnedPopup alphabetPopup = Instantiate(alphabetReturnedPopup, transform.position, Quaternion.identity).GetComponent<AlphabetReturnedPopup>();
    //                alphabetPopup.Init(collectedAlphabet[i].GetComponent<Character>().idAlphabet, npcTrans, AlphabetReturnCallBack(true));
    //            }
    //            else
    //            {
    //                AlphabetReturnedPopup alphabetPopup = Instantiate(alphabetReturnedPopup, transform.position, Quaternion.identity).GetComponent<AlphabetReturnedPopup>();
    //                alphabetPopup.Init(collectedAlphabet[i].GetComponent<Character>().idAlphabet, npcTrans, AlphabetReturnCallBack(false));

    //            }

    //            playerData.CurrentAlphabetAmountInBag -= 1;
    //            playerData.AddAlphabetAmountInBag(collectedAlphabet[i].GetComponent<Character>().idAlphabet, -1);

    //            //collectedAlphabet[i].GetComponent<Character>().Renew(false);
    //            collectedAlphabet.RemoveAt(i);
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < Enum.GetNames(typeof(Alphabet)).Length; i++)
    //        {                
    //            if (playerData.GetAlphabetAmountInBag(i) > 0)
    //            {
    //                if (i == NPCSkill.Instance.AlphabetId)
    //                {
    //                    for (int j = 0; j < playerData.GetAlphabetAmountInBag(i); j++)
    //                    {
    //                        AlphabetReturnedPopup alphabetPopup = Instantiate(alphabetReturnedPopup, transform.position, Quaternion.identity).GetComponent<AlphabetReturnedPopup>();
    //                        alphabetPopup.Init(NPCSkill.Instance.AlphabetId, npcTrans, AlphabetReturnCallBack(true));
    //                        playerData.CurrentAlphabetAmountInBag -= 1;
    //                        playerData.AddAlphabetAmountInBag(NPCSkill.Instance.AlphabetId, -1);
    //                    }                       
    //                    yield return new WaitForSeconds(0.1f);
    //                }
    //                else
    //                {
    //                    for (int j = 0; j < playerData.GetAlphabetAmountInBag(i); j++)
    //                    {
    //                        AlphabetReturnedPopup alphabetPopup = Instantiate(alphabetReturnedPopup, transform.position, Quaternion.identity).GetComponent<AlphabetReturnedPopup>();
    //                        alphabetPopup.Init(i, npcTrans, AlphabetReturnCallBack(false));
    //                        playerData.CurrentAlphabetAmountInBag -= 1;
    //                        playerData.AddAlphabetAmountInBag(i, -1);
    //                    }
    //                    yield return new WaitForSeconds(0.1f);
    //                }                  
    //            }              
    //        }
    //    }       
    //}

    private Action AlphabetReturnCallBack(NPCAlphabet npc, int holdIndex, int index)
    {
        return () =>
        {
            var playerData = PlayerDataManager.Instance;
            if (playerData.actionUITop != null)
            {
                playerData.actionUITop(TypeItemAnim.Bag);
            }           

            PopupCollectedAlphabet.Instance.UpdateDisplay();
            npc.ThrowMoney(index);           
        };
    }

    internal void ClearBag()
    {
        for (int i = alphabetHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(alphabetHolder.transform.GetChild(i).gameObject);
        }
    }

    public void Attack()
    {        
        if (isAttacking) return;
        isAttacking = true;
        aliveAnimator.SetTrigger(CharacterAction.Attack.ToAnimatorHashedKey());
        Debug.Log("attack");
    }

    public void PlaySkillVFX()
    {
        if (isPlayingVFX) return;
        isPlayingVFX = true;
        skill.SetActive(true);
        if (skill.GetComponentInChildren<ParticleSystem>()) skill.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void StopSkillVFX()
    {
        isPlayingVFX = false;
        skill.SetActive(false);
    }

    public void BeginReTransform()
    {
        CanMove = false;

    }

    public void EndReTransform()
    {
        CanMove = true;
    }

    public void SetPosition(Vector3 position)
    {
        if (navMeshAgent)
        {
            navMeshAgent.Warp(position);
        }

        transform.position = position;
    }

    private void OnEnable()
    {
        if (navMeshAgent && !navMeshAgent.isOnNavMesh)
        {
            NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas);
            navMeshAgent.Warp(hit.position);
            navMeshAgent.enabled = true;
        }
    }

    private void OnDisable()
    {
        //if (petController)
        //{
        //    petController.gameObject.SetActive(false);
        //}

        if (navMeshAgent)
        {
            navMeshAgent.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (CanMove && transform.position.y < -1.4f)
        {
            transform.position = transform.position.Set(y: 0f);
        }
    }

    //=======================================================================================================
    //private void OnDrawGizmos()
    //{
    //    try
    //    {
    //        if (IsAlive)
    //            Gizmos.DrawWireSphere(transform.position, characterProperty.HearingRadius);
    //    }
    //    catch (Exception)
    //    {
    //        return;
    //    }

    //}
}
