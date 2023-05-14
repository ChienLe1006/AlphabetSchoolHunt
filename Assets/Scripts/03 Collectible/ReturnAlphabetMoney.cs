using System.Threading.Tasks;
using UnityEngine;

public class ReturnAlphabetMoney : MonoBehaviour
{
    [SerializeField] private TypeSoundIngame collectSFX;
    [SerializeField] private GameObject collectFX;
    [SerializeField] private GameObject model;
    [SerializeField] private BoxCollider myCol;
 
    private void OnEnable()
    {
        myCol.enabled = true;
        model.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(GetComponentInParent<NPCAlphabet>().CollectMoney());
    }

    internal async void OnCollected(int amount)
    {
        GameManager.Instance.Profile.AddGold(amount);
        myCol.enabled = false;
        model.SetActive(false);
        SoundManager.Instance.PlaySoundCollectible(collectSFX);
        SimplePool.Spawn(collectFX, transform.position, transform.rotation);
        await Task.Delay(1000);
        gameObject.Despawn();
    }
}
