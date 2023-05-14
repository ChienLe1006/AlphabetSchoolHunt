using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIMover : MoveBehaviour
{
    private readonly NavMeshAgent navMeshAgent;
    private Vector3 destination;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public AIMover(Character character) : base(character)
    {
        this.navMeshAgent = character.NavMeshAgent;
        this.IsMoveByDistance = false;
        Character.OnDie += StopMoving;
        GameManager.Instance.OnLevelEnd += StopMoving;
        navMeshAgent.speed = character.Speed;
        navMeshAgent.acceleration = float.MaxValue;
        navMeshAgent.angularSpeed = 360f;
        navMeshAgent.radius = 1f;
        navMeshAgent.height = 3;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    private void StopMoving(Character character)
    {
        if (character == this.Character)
            if (navMeshAgent.isOnNavMesh)
                navMeshAgent.isStopped = true;
    }

    private void StopMoving(LevelResult levelResult)
    {
        if (navMeshAgent.isOnNavMesh)
            navMeshAgent.isStopped = true;
    }

    public override void Move()
    {
        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        navMeshAgent.speed = Character.Speed;
        if (Character.CharacterAnimator)
            Character.CharacterAnimator.SetBool(CharacterAction.Run.ToAnimatorHashedKey(), true);
        base.Move();
    }
}
