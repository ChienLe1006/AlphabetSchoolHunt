using DG.Tweening;
using Spine.Unity.AttachmentTools;
using UnityEngine;

public class ChaseTransition : Transition
{
    public ChaseTransition(CharacterBehaviour start, AIChaser end) : base(start, end)
    {
    }

    public override bool IsConditionFullfilled()
    {
        Character target = FindTarget();


        if (target)
        {
            if (!Start.Character.SuspiciousIndicator.isActiveAndEnabled)
                Start.Character.SuspiciousIndicator.gameObject.SetActive(true);

            (End as AIChaser).Target = target;
            return true;
        }
        else
        {
            return false;
        }


    }

    private Character FindTarget()
    {
        if (GameManager.Instance.CurrentLevelManager == null)
            return null;

        var hiders = GameManager.Instance.CurrentLevelManager.Hiders;
        float hearingRadius = Start.Character.HearingRadius;
        Vector3 characterPosition = Start.Character.transform.position;
        Character nearestHider = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < hiders.Length; i++)
        {
            Character hider = hiders[i];
            if (!hider.IsAlive)
            {
                continue;
            }

            float detectRange = hearingRadius;
            detectRange = detectRange < 0 ? 0 : detectRange;
            float distance = Vector3.Distance(characterPosition, hider.transform.position);
            if (distance < minDistance && distance <= detectRange)
            {
                minDistance = distance;
                nearestHider = hider;
            }
        }

        return nearestHider;
    }
}