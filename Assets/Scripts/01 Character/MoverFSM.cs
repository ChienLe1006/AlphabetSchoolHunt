using Common.FSM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.AI;

public class MoverFSM : IMovable
{
    public event Action<Character> OnStandingStill;
    public event Action<Character> OnMoving;

    private readonly Character character;
    private MoveBehaviour[] moveBehaviours;
    private MoveBehaviour activeMoveBehaviour;

    public MoverFSM(Character character)
    {
        this.character = character;
        moveBehaviours = GetMoveBehaviours(character);
        activeMoveBehaviour = moveBehaviours[0];
        for (int i = 0; i < moveBehaviours.Length; i++)
        {
            moveBehaviours[i].OnStandingStill += (c) => OnStandingStill?.Invoke(c);
            moveBehaviours[i].OnMoving += (c) => OnMoving?.Invoke(c);
        }
    }

    private MoveBehaviour[] GetMoveBehaviours(Character character)
    {
        Role role = character.Role;

        if (role.HasFlag(Role.Manual))
        {
            return new MoveBehaviour[] { new ManualMover(character) };
        }
        else if (role == Role.Hider)
        {
            NavMeshAgent navMeshAgent = character.NavMeshAgent;
            AIPatroller patroller = new AIPatroller(character);
            AIPanic panicAI = new AIPanic(character);
            AIFinishPath finishPathAI = new AIFinishPath(character);

            patroller.Transitions.Add(new PanicTransition(patroller, panicAI));
            panicAI.Transitions.Add(new PanicOverTransition(panicAI, finishPathAI));
            finishPathAI.Transitions.Add(new AIDestinationReachedTransision(finishPathAI, patroller));

            return new MoveBehaviour[]
            {
                patroller,
                panicAI,
                finishPathAI
            };
        }
        else if (role == Role.Seeker)
        {
            NavMeshAgent navMeshAgent = character.NavMeshAgent;
            AIPatroller patroller = new AIPatroller(character);
            AIChaser chaser = new AIChaser(character);
            AIFinishPath finishPathAI = new AIFinishPath(character);

            patroller.Transitions.Add(new ChaseTransition(patroller, chaser));
            chaser.Transitions.Add(new ChaseOverTransition(chaser, finishPathAI));
            finishPathAI.Transitions.Add(new AIDestinationReachedTransision(finishPathAI, patroller));
            finishPathAI.Transitions.Add(new ChaseTransition(finishPathAI, chaser));

            return new MoveBehaviour[]
            {
                patroller,
                chaser,
                finishPathAI
            };
        }

        throw new System.ArgumentException($"Role {role} is not supported!");
    }

    public void Move()
    {
        activeMoveBehaviour = (MoveBehaviour)activeMoveBehaviour.GetNextBehaviour();

        activeMoveBehaviour.Move();
    }
}
