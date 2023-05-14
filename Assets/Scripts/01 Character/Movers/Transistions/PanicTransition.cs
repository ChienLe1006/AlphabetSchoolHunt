using UnityEngine;

public class PanicTransition : Transition
{
    public PanicTransition(CharacterBehaviour start, AIPanic end) : base(start, end)
    {
    }

    public override bool IsConditionFullfilled()
    {
        Character chaser = FindTarget();

        if (chaser)
        {
            (End as AIPanic).Chaser = chaser;
            return true;
        };

        return false;
    }

    private Character FindTarget()
    {
        if (GameManager.Instance.CurrentLevelManager == null)
            return null;

        var seekers = GameManager.Instance.CurrentLevelManager.Seekers;
        float hearingRadius = Start.Character.HearingRadius;
        Vector3 characterPosition = Start.Character.transform.position;
        Character nearestHider = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < seekers.Length; i++)
        {
            Character seeker = seekers[i];
            if (!seeker.IsAlive)
            {
                continue;
            }

            float detectRange = hearingRadius;
            detectRange = detectRange < 0 ? 0 : detectRange;
            float distance = Vector3.Distance(characterPosition, seeker.transform.position);
            if (distance < minDistance && distance <= detectRange)
            {
                minDistance = distance;
                nearestHider = seeker;
            }
        }

        return nearestHider;
    }
}

public class PanicManyTransition : Transition
{
    private readonly Role[] rolesToPanic;

    public PanicManyTransition(CharacterBehaviour start, CharacterBehaviour end, Role[] rolesToPanic) : base(start, end)
    {
        this.rolesToPanic = rolesToPanic;
    }

    public override bool IsConditionFullfilled()
    {
        Character chaser = GameManager.Instance.CurrentLevelManager
                              .FindNearestCharacter(rolesToPanic, Start.Character.transform.position);

        if (chaser &&
            Vector3.Distance(chaser.transform.position, Start.Character.transform.position)
            <= Start.Character.HearingRadius)
        {
            (End as AIPanic).Chaser = chaser;
            return true;
        };

        return false;
    }
}