using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [Title("Camera")]
    [SerializeField] private Vector3 camPosition;
    [SerializeField] private Vector3 camRotation;
    [SerializeField] private float camMoveDuration;
    [SerializeField] private Ease camEase;

    [Title("Map size")]
    [SerializeField] private Vector3 mapCenter = new Vector3(0, 0);
    [SerializeField] private float mapSize = 48.62f;

    [Title("Alphabet")]
    [SerializeField] private Alphabet[] alphabets, unlockAlphabets;
    [SerializeField] private Vector3 camLetterPos, camLetterRot;
    [SerializeField] private float camLetterDuration = 1;
    [SerializeField] private NPCAlphabet[] npcs;

    [Title("Level")]
    [SerializeField] private GameObject checkUnlockLevel;
    [SerializeField] private Vector3 unlockLevelCamPos;
    [SerializeField] private Vector3 unlockLevelCamRot;
    [SerializeField] private float unlockLevelCamDuration = 1;
    [SerializeField] private UnlockLetterUIController unlockLetterUIController;

    [Title("Object in level")]
    [SerializeField] private GameObject decorNPC;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Vector3[] objectPos;
    [SerializeField] private Transform upgradeDesk;
    [field:SerializeField] public Door Door { get; private set; }
    private int objQuantity;

    [Title("Characters")]
    [SerializeField] private CharacterProperty[] characterProperty;
    [SerializeField] private CharacterPosition[] characterPositions;
    [SerializeField, HideInInspector] private int hiderAmount;
    [SerializeField, HideInInspector] private int seekerAmount;

    private Character player;
    private Character[] characters;
    private Character[] hiders;
    private Character[] seekers;
    private int seekerFoundCount;   

    public Character[] Hiders => hiders;
    public Character[] Seekers => seekers;
    public Character[] Characters => characters;
    public Character Player => player;

    public int SeekerFoundCount { get => seekerFoundCount; private set => seekerFoundCount = value; }

    public LevelResult Result { get; private set; }
    public Vector3 MapCenter => mapCenter;
    public float MapSize => mapSize;
    public Vector3 CamPosition => camPosition;
    public Vector3 CamRotation => camRotation;
    public float CamMoveDuration => camMoveDuration;
    public Ease CamEase => camEase;
    public Alphabet[] Alphabets => alphabets;
    public Vector3 CamLetterPos => camLetterPos;
    public Vector3 CamLetterRot => camLetterRot;
    public float CamLetterDuration => camLetterDuration;
    public bool IsSpecialLevel => characterPositions.Length == 1;    

    public UnlockLetterUIController UnlockLetterUIController => unlockLetterUIController;
    public GameObject CheckUnlockLevel => checkUnlockLevel;
    public GameObject DecorNPC => decorNPC;
    public int NumberAlphabetUnlock { get; set; }
    public Transform UpgradeDesk => upgradeDesk;

    private void Start()
    {
        Character.OnDie += OnCharacterDie;
        GameManager.Instance.OnLevelEnd += ClearAlphabetInBagAndUnlockNPC;
        GameManager.Instance.SetUpNewLevel(this);

        for (int i = 0; i < unlockAlphabets.Length; i++)
        {
            PlayerDataManager.Instance.SetUnlockAlphabet(unlockAlphabets[i]);
        }

        for (int i = 0; i < alphabets.Length; i++)
        {
            if (PlayerDataManager.Instance.GetUnlockAlphabet(alphabets[i]))
            {
                NumberAlphabetUnlock++;
            }
        }
        if (NumberAlphabetUnlock == alphabets.Length)
        {
            checkUnlockLevel.SetActive(true);
        }
        else checkUnlockLevel.SetActive(false);
        ActiveNPC();
    }

    public void StartLevel()
    {
        SpawnCharacters();
        SetUpLevelEnvironment();
    }

    internal void ActiveNPC()
    {
        if (NumberAlphabetUnlock == 0)
        {
            for (int i = 0; i < npcs.Length; i++)
            {
                npcs[i].gameObject.SetActive(false);
            }
            npcs[0].gameObject.SetActive(true);
            return;
        }
        else if (NumberAlphabetUnlock == npcs.Length)
        {
            for (int i = 0; i < npcs.Length; i++)
            {
                npcs[i].gameObject.SetActive(true);
            }
            return;
        }
        for (int i = 0; i <= NumberAlphabetUnlock; i++)
        {
            npcs[i].gameObject.SetActive(true);
        }
        for (int i = NumberAlphabetUnlock + 1; i < npcs.Length; i++)
        {
            npcs[i].gameObject.SetActive(false);
        }
    }

    private void SpawnCharacters()
    {
        GameMode gameMode = GameManager.Instance.CurrentGameMode;
        Role playerRole = gameMode.ToCharacterRole() | Role.Manual;
        player = GameManager.Instance.CharacterPool.GetCharacter(playerRole, false);

        hiders = new Character[gameMode == GameMode.HIDE ? hiderAmount : hiderAmount + seekerAmount - 1];
        seekers = new Character[gameMode == GameMode.HIDE ? seekerAmount : 1];
        characters = new Character[hiderAmount + seekerAmount];

        var characterAmountToChoose = gameMode == GameMode.HIDE ? hiderAmount : seekerAmount;
        int chosenCharacterIndex = 0; //Random.Range(0, characterAmountToChoose);

        int hiderIndex = 0;
        int seekerIndex = -1;

        for (int i = 0; i < characterPositions.Length; i++)
        {
            if (characterPositions[i].Role.HasFlag(Role.Seeker))
            {
                seekerIndex++;

                if (gameMode != GameMode.SEEK)
                {
                    seekers[seekerIndex] = GameManager.Instance.CharacterPool.GetCharacter(Role.Seeker, false);
                    characters[i] = seekers[seekerIndex];
                    SetUpCharacter(seekers[seekerIndex], characterPositions[i]);
                    continue;
                }


                if (gameMode == GameMode.SEEK && seekerIndex == chosenCharacterIndex)
                {
                    seekers[0] = player;
                    characters[i] = player;
                    SetUpCharacter(player, characterPositions[i]);
                    continue;
                }
            }

            hiders[hiderIndex] = chosenCharacterIndex == hiderIndex && gameMode == GameMode.HIDE
                        ? player
                        : GameManager.Instance.CharacterPool.GetCharacter(Role.Hider, false);
            SetUpCharacter(hiders[hiderIndex], characterPositions[i]);
            characters[i] = hiders[hiderIndex];
            hiderIndex++;
        }
    }

    //private void SpawnCharacters()
    //{
    //    GameMode gameMode = GameManager.Instance.CurrentGameMode;
    //    Role playerRole = gameMode.ToCharacterRole() | Role.Manual;
    //    player = GameManager.Instance.CharacterPool.GetCharacter(playerRole, false);

    //    seekers = new Character[seekerAmount];
    //    characters = new Character[seekerAmount];

    //    //int characterAmountToChoose = gameMode == GameMode.HIDE ? hiderAmount : seekerAmount;
    //    int chosenCharacterIndex = 0; //Random.Range(0, characterAmountToChoose);

    //    int seekerIndex = 0;

    //    for (int i = 0; i < characterPositions.Length; i++)
    //    {
    //        seekers[seekerIndex] = chosenCharacterIndex == seekerIndex && gameMode == GameMode.SEEK
    //                    ? player
    //                    : GameManager.Instance.CharacterPool.GetCharacter(Role.Seeker, false);
    //        characters[i] = seekers[seekerIndex];
    //        SetUpCharacter(seekers[seekerIndex], characterPositions[i]);
    //        seekers[i] = player;
    //        seekerIndex++;
    //    }
    //}

    private void SetUpCharacter(Character character, CharacterPosition characterPosition)
    {
        character.BeginReTransform();
        character.SetSkinAlphabet((int)characterPosition.Alphabet);
        character.SetSkill((int)characterPosition.Skill);
        character.transform.position = characterPosition.Position.Set(y: 0f);
        character.transform.rotation = Quaternion.identity;
        character.transform.eulerAngles = new Vector3(0, 90, 0);
        character.CharacterProperty = GetCharacterProperty(character.Role);
        character.EndReTransform();
        character.gameObject.SetActive(true);
        character.Renew(true);
    }

    private CharacterProperty GetCharacterProperty(Role role)
    {
        for (int i = 0; i < characterProperty.Length; i++)
        {
            if (characterProperty[i].Role == role)
            {
                return characterProperty[i];
            }
        }
        return null;
    }

    private void SetUpLevelEnvironment()
    {
        SeekerFoundCount = 0;
        Result = LevelResult.NotDecided;
    }

    private void ClearAlphabetInBagAndUnlockNPC(LevelResult levelResult)
    {
        if (levelResult == LevelResult.Win)
        {
            var playerData = GameManager.Instance.PlayerDataManager;
            for (int i = 0; i < alphabets.Length; i++)
            {
                PlayerPrefs.DeleteKey("amount" + (int)alphabets[i]);
                PlayerPrefs.DeleteKey("alphabet" + alphabets[i]);
                PlayerPrefs.DeleteKey("amount_money_unlock" + alphabets[i]);
            }
            playerData.CurrentAlphabetAmountInBag = 0;
            player.ClearBag();
            playerData.actionUITop?.Invoke(TypeItemAnim.Bag);
        }
    }

    private void OnDestroy()
    {
        Character.OnDie -= OnCharacterDie;
        GameManager.Instance.OnLevelEnd -= ClearAlphabetInBagAndUnlockNPC;
        DespawnCharacters();
        RemoveObject();
    }

    private void DespawnCharacters()
    {
        if (Characters == null)
        {
            return;
        }

        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] != null)
                GameManager.Instance.CharacterPool.ReturnCharacter(Characters[i]);
        }
    }

    private void RemoveObject()
    {
        if (specialObjects.Count == 0) return;

        for (int i = specialObjects.Count - 1; i >= 0 ; i--)
        {
            if (specialObjects[i] != null) Destroy(specialObjects[i]);
        }
    }

    public void OnCharacterDie(Character character)
    {
        if (character.Role.HasFlag(Role.Seeker) && !character.IsPlayer)
        {
            SeekerFoundCount++;
        }

        CheckGameResult();
    }

    public LevelResult CheckGameResult()
    {
        if (Result != LevelResult.NotDecided)
        {
            return Result;
        }

        if (!player.IsAlive)
        {
            EndGame(LevelResult.Lose);
        }

        return LevelResult.NotDecided;
    }

    public void EndGame(LevelResult levelResult)
    {
        Result = levelResult;
        GameManager.Instance.EndCurrentLevel();
    }

    public void Revive()
    {
        player.Renew();
        Result = LevelResult.NotDecided;
    }

    public Character FindNearestCharacter(Alphabet alphabet, Vector3 position)
    {
        Character nearestCharacter = null;
        float minDistance = float.MaxValue;

        foreach (var character in characters)
        {
            if (!character.IsAlive || character.Alphabet != alphabet)
            {
                continue;
            }

            float distance = Vector3.Distance(character.transform.position, position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    public NPCAlphabet FindNearestNPC(Vector3 position)
    {
        NPCAlphabet nearestNPC = null;
        float minDistance = float.MaxValue;

        foreach (var npc in npcs)
        {
            if (npc.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(npc.transform.position, position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNPC = npc;
                }
            }
        }
        return nearestNPC;
    }

    //public Character FindNearestCharacter(Role role, Vector3 position)
    //{
    //    Character nearestCharacter = null;
    //    float minDistance = float.MaxValue;

    //    foreach (var character in characters)
    //    {
    //        if (!character.IsAlive || !character.Role.HasFlag(role))
    //        {
    //            continue;
    //        }

    //        float distance = Vector3.Distance(character.transform.position, position);

    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            nearestCharacter = character;
    //        }
    //    }

    //    return nearestCharacter;
    //}

    public Character FindNearestCharacter(Role[] roles, Vector3 position)
    {
        Character nearestCharacter = null;
        float maxDistance = float.MaxValue;

        foreach (var character in characters)
        {
            if (!character.IsAlive)
            {
                continue;
            }

            bool isRoleCorrect = false;
            for (int i = 0; i < roles.Length; i++)
            {
                if (character.Role.HasFlag(roles[i]))
                {
                    isRoleCorrect = true;
                    break;
                }
            }

            if (!isRoleCorrect)
            {
                continue;
            }

            float distance = Vector3.Distance(character.transform.position, position);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    internal void ShowNextLevelCheck()
    {
        GameManager.Instance.MainCamera.ShowLevelUnlockCheck(unlockLevelCamPos, unlockLevelCamRot, unlockLevelCamDuration,
            () =>
            {
                checkUnlockLevel.SetActive(true);
            });
    }
    
    private List<GameObject> specialObjects = new List<GameObject>();
    internal void SpawnObject()
    {
        if (objQuantity == 0)
        {
            objQuantity++;
            int index = Random.Range(0, objects.Length);
            var obj = Instantiate(objects[index]);
            obj.transform.position = objectPos[index];
            specialObjects.Add(obj);
        }
    }

    internal void DeleteObject(GameObject obj)
    {
        Destroy(obj);
        objQuantity--;
        specialObjects.Remove(obj);
    }

    //==================================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mapCenter, new Vector3(mapSize, 2.9f, mapSize));
        Gizmos.color = Color.white;
    }

    private void OnValidate()
    {
        if (characterPositions != null)
        {
            int hiderCount = 0;
            int seekerCount = 0;

            for (int i = 0; i < characterPositions.Length; i++)
            {
                switch (characterPositions[i].Role)
                {
                    case Role.Hider:
                        hiderCount++;
                        break;
                    case Role.Seeker:
                        seekerCount++;
                        break;
                    default:
                        Debug.LogError($"{nameof(characterPositions)} ở vị trí {i} có {nameof(Role)} không chính xác!\n" +
                                       $"Giá trị chỉ có thể là {nameof(Role.Hider)} hoặc {nameof(Role.Seeker)}");
                        break;
                }
            }
            hiderAmount = hiderCount;
            seekerAmount = seekerCount;
        }
    }

#if UNITY_EDITOR
    #region Character
    [Title("Nên xóa players trước khi chạy map!")]
    [Button]
    public void _ApplyPlayers()
    {
        Character[] characters = FindObjectsOfType<Character>();
        if (characters.Length == 0)
            return;

        characterPositions = new CharacterPosition[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            var character = characters[i];
            characterPositions[i] = new CharacterPosition(character.transform.position, character.Role);
        }
    }


    [Button]
    public void _DeletePlayers()
    {
        Character[] characters = FindObjectsOfType<Character>();
        for (int i = 0; i < characters.Length; i++)
        {
            DestroyImmediate(characters[i].gameObject);
        }
    }

    [Button]
    public void _ReApplyPlayers()
    {
        Character prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<Character>("Assets/Prefabs/Character/CharacterNew.prefab");

        foreach (var character in characterPositions)
        {
            Character newCharacter = (Character)UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
            newCharacter.transform.position = character.Position;
        }
    }

    #endregion

#endif
}
