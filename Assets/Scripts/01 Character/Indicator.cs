using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private new Spine.Unity.SkeletonAnimation animation;

    private Camera mainCamera;
    private float timeStart;

    private void Start()
    {
        mainCamera = GameManager.Instance.MainCamera.GetComponent<Camera>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);

        if (timeStart != -1 && Time.time - timeStart >= duration)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        RunAnimation();
        timeStart = Time.time;
    }

    public void ShowFor(float time)
    {
        if (isActiveAndEnabled)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
        timeStart = -1;
        StartCoroutine(ShowForCoroutine(time));
    }

    private IEnumerator ShowForCoroutine(float time)
    {
        yield return new WaitForSeconds(1.167f);
        animation.AnimationState.SetAnimation(0, "appear", false);
        yield return new WaitForSeconds(1.167f);
        animation.AnimationState.SetAnimation(0, "appear", false);
        yield return new WaitForSeconds(1.167f);
        animation.AnimationState.SetAnimation(0, "appear", false);
        gameObject.SetActive(false);
    }

    private void RunAnimation()
    {
        StopAllCoroutines();
        if (animation.AnimationState == null)
        {
            animation.Initialize(false);
        }

        animation.AnimationState.TimeScale = 1;
        animation.AnimationState.SetAnimation(0, "appear", false);

    }
}
