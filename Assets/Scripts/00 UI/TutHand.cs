using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutHand : MonoBehaviour
{
    [SerializeField] private List<Transform> listTransPoint;
    [SerializeField] private Transform transHands;
    private List<Vector3> paths;

    Tweener tween;

    private void OnEnable()
    {
        if (paths == null)
        {
            paths = new List<Vector3>();
            for (int i = 0; i < listTransPoint.Count; i++)
            {
                paths.Add(listTransPoint[i].position);
            }
        }

        tween = transHands.DOPath(paths.ToArray(), 2f, PathType.Linear, PathMode.TopDown2D).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
        tween.Rewind();
    }

    void Update()
    {

        var x = UltimateJoystick.GetHorizontalAxis(Constants.MAIN_JOINSTICK);
        var z = UltimateJoystick.GetVerticalAxis(Constants.MAIN_JOINSTICK);

        if (x != 0 || z != 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
