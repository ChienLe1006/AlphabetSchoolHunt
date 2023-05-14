using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatroller : AIMover
{
    public AIPatroller(Character character) : base(character)
    {
    }

    public override Vector3 GetNextPosition()
    {
        if (NavMeshAgent.remainingDistance < 0.5f)
            NavMeshAgent.SetDestination(GetRandomPosition());

        return NavMeshAgent.nextPosition;
    }

    private Vector3 GetRandomPosition()
    {
        float range = Random.Range(GameManager.Instance.CurrentLevelManager.MapSize, GameManager.Instance.CurrentLevelManager.MapSize * 2);
        Vector3 randomDirection = Random.insideUnitSphere * range;
        Vector3 samplePosition = Character.transform.position + randomDirection;
        NavMesh.SamplePosition(samplePosition, out var navMeshHit, range, NavMesh.AllAreas);

        return navMeshHit.position;
    }
}

