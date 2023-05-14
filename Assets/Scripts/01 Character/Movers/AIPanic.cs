using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPanic : AIMover
{
    private float timeCount;

    public AIPanic(Character character) : base(character)
    {
    }

    public Character Chaser
    {
        get;
        set;
    }

    public override Vector3 GetNextPosition()
    {
        if (Chaser == null)
        {
            return Character.transform.position;
        }

        var directionRun = GetDirectionRun();
        if (directionRun != Vector3.zero)
        {
            float range = 3;
            directionRun = directionRun * range + Character.transform.position;
            NavMesh.SamplePosition(directionRun, out var navMeshHit, range, NavMesh.AllAreas);
            NavMeshAgent.SetDestination(navMeshHit.position);
            Chaser = null;
            return NavMeshAgent.nextPosition;
        }
        else
        {
            if (NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.SetDestination(Character.transform.position);
            }

            Character.transform.rotation = Quaternion.LookRotation(-(Character.transform.position - Chaser.transform.position));

            if (Character.CharacterAnimator != null)
                Character.CharacterAnimator.SetTrigger(CharacterAction.Tremble.ToAnimatorHashedKey());

            return Character.transform.position;
        }
    }

    private Vector3 GetDirectionRun()
    {
        Vector3 characterPosition = Character.transform.position;
        Vector3 directionRun = (characterPosition - Chaser.transform.position);
        float angleRun = VectorUlti.GetAngleFromVector(directionRun);

        List<float> allAngles = new List<float>();

        for (int i = 0; i < 60; i++)
        {
            allAngles.Add(i);
            allAngles.Add(-i);
        }


        while(allAngles.Count > 0)
        {
            int randomAngleIndex = Random.Range(0, allAngles.Count);
            float currentAngle = allAngles[randomAngleIndex];
            directionRun = VectorUlti.GetVectorFromAngle(currentAngle + angleRun);
            if (!NavMesh.Raycast(characterPosition, characterPosition + (directionRun * 2.5f), out _, NavMesh.AllAreas))
            {
                return directionRun;
            }

            allAngles.RemoveAt(randomAngleIndex);
        }

        for (int currentAngle = 60; currentAngle <= 90; currentAngle += 15)
        {
            directionRun = VectorUlti.GetVectorFromAngle(currentAngle + angleRun);
            if (!NavMesh.Raycast(characterPosition, characterPosition + (directionRun * 2.5f), out _, NavMesh.AllAreas))
            {
                return directionRun;
            }
            else
            {
                directionRun = VectorUlti.GetVectorFromAngle(-currentAngle + angleRun);
                if (!NavMesh.Raycast(characterPosition, characterPosition + (directionRun * 2.5f), out _, NavMesh.AllAreas))
                {
                    return directionRun;
                }
            }
        }

        return Vector3.zero;
    }

}