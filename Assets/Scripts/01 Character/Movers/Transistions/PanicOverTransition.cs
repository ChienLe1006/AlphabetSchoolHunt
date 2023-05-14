using System;
using UnityEngine;

public class PanicOverTransition : Transition
{
    public PanicOverTransition(AIPanic start, CharacterBehaviour end) : base(start, end)
    {
    }

    public override bool IsConditionFullfilled()
    {
        Character chaser = (Start as AIPanic).Chaser;
        return !chaser || (Start as AIPanic).NavMeshAgent.remainingDistance < 0.4f;
    }
}
