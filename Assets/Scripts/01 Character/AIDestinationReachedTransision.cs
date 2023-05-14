public class AIDestinationReachedTransision : Transition
{
    public AIDestinationReachedTransision(AIMover start, AIMover end) : base(start, end)
    {
    }

    public override bool IsConditionFullfilled()
    {
        return (Start as AIMover).NavMeshAgent.remainingDistance < 0.2f || (Start as AIMover).NavMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete;
    }
}
