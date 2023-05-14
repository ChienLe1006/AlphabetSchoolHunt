using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    [SerializeField] private int moneyAmount = 200;
    [SerializeField] private SpriteRenderer footprint;
    [SerializeField] private float footprintLifeTime = 2f;
    [SerializeField] private float timeBetweenFootStep = 0.2f;
    [SerializeField] private float affectTime = 8f;
    [SerializeField] private SpriteRenderer puddleSprite;
    [SerializeField] private GameObject cleanIcon;
    private bool isIn;
    private float timer;

    private static Dictionary<Character, Coroutine> effectingCharacters = new Dictionary<Character, Coroutine>();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(60);
        GameManager.Instance.CurrentLevelManager.DeleteObject(gameObject);
    }

    private void Update()
    {
        if (isIn)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                OpenMiniGamePopup();
                isIn = false;
                timer = 0f;
            }
        }    
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentLevelManager == null)
        {
            return;
        }

        var characters = GameManager.Instance.CurrentLevelManager.Characters;
        if (characters == null)
        {
            return;
        }

        foreach (var character in characters)
        {
            if (!character.IsAlive)
            {
                continue;
            }

            if (!IsWithinRange(character.transform.position))
            {
                continue;
            }

            if (effectingCharacters.ContainsKey(character))
            {
                var printFootprintsCoroutine = effectingCharacters[character];
                character.StopCoroutine(printFootprintsCoroutine);
                effectingCharacters[character] = character.StartCoroutine(PrintFootprints(character));
            }
            else
            {
                effectingCharacters.Add(character, character.StartCoroutine(PrintFootprints(character)));
            }
        }

        if (cleanIcon.activeInHierarchy) cleanIcon.transform.LookAt(GameManager.Instance.MainCamera.transform.position);
    }

    private bool IsWithinRange(Vector3 point)
    {
        if (puddleSprite != null)
        {
            return puddleSprite.bounds.Contains(point.Set(y: transform.position.y));
        }
        return false;
    }

    private IEnumerator PrintFootprints(Character character)
    {
        Vector3 eulerAngles = footprint.transform.eulerAngles;
        float y = transform.position.y + 0.1f;
        float timeElapsed = 0;
        float nextFootPrintTime = 0;
        bool isRightLeg = true;
        Queue<SpriteRenderer> printedFootPrint = new Queue<SpriteRenderer>();

        while (timeElapsed < affectTime)
        {
            timeElapsed += Time.deltaTime;

            Vector3 newFootprintPosition = character.transform.position;

            if (timeElapsed > nextFootPrintTime && !IsWithinRange(newFootprintPosition) && footprint != null)
            {
                newFootprintPosition.y = y;
                Vector3 characterOffset = isRightLeg ? character.transform.right : -character.transform.right;
                newFootprintPosition += Vector3.ClampMagnitude(characterOffset, 0.5f);
                isRightLeg = !isRightLeg;

                Quaternion newFootprintRotation = Quaternion.Euler(eulerAngles + character.transform.eulerAngles);
                var newFootprint = SimplePool.Spawn(footprint, newFootprintPosition, newFootprintRotation);
                newFootprint.color = footprint.color;
                newFootprint.DOFade(0, footprintLifeTime)
                            .SetEase(Ease.InCirc)
                            .OnComplete(() => SimplePool.Despawn(printedFootPrint.Dequeue().gameObject));

                printedFootPrint.Enqueue(newFootprint);
                nextFootPrintTime += timeBetweenFootStep;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character player = other.GetComponent<Character>();
        if (player != null && player.IsPlayer)
        {
            cleanIcon.SetActive(true);
            cleanIcon.GetComponent<Animator>().Rebind();
            isIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character player = other.GetComponent<Character>();
        if (player != null && player.IsPlayer)
        {
            cleanIcon.GetComponent<Animator>().SetTrigger("Fade");
            isIn = false;
            timer = 0f;
        }
    }

    private void OpenMiniGamePopup()
    {
        GameManager.Instance.UiController.OpenPopupWaterCleaning(moneyAmount, this.gameObject);
    }
}