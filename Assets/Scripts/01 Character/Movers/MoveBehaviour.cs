using System;
using UnityEngine;

public abstract class MoveBehaviour : CharacterBehaviour, IMovable
{
    public event Action<Character> OnStandingStill;
    public event Action<Character> OnMoving;

    protected readonly CharacterController characterController;

    protected float lastProducedSmokeTime;

    protected MoveBehaviour(Character character) : base(character)
    {
        characterController = character.CharacterController;
        lastProducedSmokeTime = float.MinValue;
    }

    public bool IsMoveByDistance
    {
        get;
        set;
    }

    public abstract Vector3 GetNextPosition();

    public override void Update()
    {
    }

    private Vector3 GetMoveDistance(Vector3 nextMovePosition)
    {
        return (nextMovePosition - Character.transform.position);
    }

    public virtual void Move()
    {
        Vector3 nextPosition = GetNextPosition();
        Vector3 previousPosition = Character.transform.position;
        bool isMoved = false;

        if (!IsMoveByDistance)
        {
            nextPosition = GetMoveDistance(nextPosition);
            nextPosition.y = 0;
            isMoved = true;
        }
        else
        {
            isMoved = nextPosition != Vector3.zero;
            characterController.SimpleMove(nextPosition);
            RotateFacing(nextPosition);
        }

        if (isMoved)
        {
            Character.StopSkillVFX();
            if (Time.time - lastProducedSmokeTime > Constants.TIME_PRODUCE_SMOKE_WHEN_MOVING)
            {
                if (Character.SmokeMovementFX)
                    SimplePool.Spawn(Character.SmokeMovementFX, previousPosition.Set(y: 1.5f), Quaternion.identity);
                lastProducedSmokeTime = Time.time;
            }

            if (Character.CharacterAnimator)
                Character.CharacterAnimator.SetBool("Run", true);

            OnMoving?.Invoke(Character);           
        }
        else
        {
            lastProducedSmokeTime = float.MinValue;

            if (Character.CharacterAnimator)
                Character.CharacterAnimator.SetBool("Run", false);

            OnStandingStill?.Invoke(Character);

            //if (Input.GetMouseButtonUp(0))
            //{
            //    //Character.CharacterAnimator.SetTrigger(CharacterAction.Attack.ToAnimatorHashedKey());
            //    //SoundManager.Instance.PlaySoundBite(Character.transform.position);
            //    Character.Skill.SetActive(true);
            //}
        }
    }

    private void RotateFacing(Vector3 directionFacing)
    {
        if (directionFacing == Vector3.zero)
            return;

        var targetRotation = Quaternion.LookRotation(directionFacing);

        Character.transform.rotation = Quaternion.RotateTowards(
                Character.transform.rotation,
                targetRotation,
                720 * Time.deltaTime);
    }
}