using UnityEngine;
using UnityEngine.AI;

public class AIFinishPath : AIMover
{
    public AIFinishPath(Character character) : base(character)
    {
    }

    public override Vector3 GetNextPosition()
    {
        return NavMeshAgent.nextPosition;
    }
}
