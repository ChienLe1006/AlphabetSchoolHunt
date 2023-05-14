using UnityEngine;

public class ChaseOverTransition : Transition
{
    public ChaseOverTransition(AIChaser start, CharacterBehaviour end) : base(start, end)
    {
    }

    public override bool IsConditionFullfilled()
    {
        if (!IsTargetExist())
        {
            return true;
        }

        return !IsTargetWithinHearingRadius();
    }

    private bool IsTargetExist()
    {
        return (Start as AIChaser).Target.IsAlive;
    }

    private bool IsTargetWithinHearingRadius()
    {
        float distance = Vector3.Distance(Start.Character.transform.position,
                                          (Start as AIChaser).Target.transform.position);

        return distance <= Start.Character.HearingRadius && (Start as AIChaser).Target.CanDie;
    }
}
