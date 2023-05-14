using UnityEngine;
using UnityEngine.AI;

public class AIChaser : AIMover
{
    public AIChaser(Character character) : base(character)
    {
    }

    public Character Target { get; set; }


    public void SetTarget(Character target)
    {
        this.Target = target;
    }

    public override Vector3 GetNextPosition()
    {
        if (!Target.gameObject.activeSelf)
        {
            return Character.transform.position;
        }

        NavMeshAgent.SetDestination(Target.transform.position);
        return NavMeshAgent.nextPosition;
    }

}
