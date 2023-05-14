using UnityEngine;
using System.Collections.Generic;

public abstract class CharacterBehaviour
{
    private readonly Character character;

    private List<Transition> transitions = new List<Transition>();

    public Character Character => character;

    public List<Transition> Transitions
    {
        get => transitions;
        set => transitions = value;
    }

    public CharacterBehaviour GetNextBehaviour()
    {
        foreach (var transition in transitions)
        {
            if (transition.IsConditionFullfilled())
            {
                return transition.End;
            }
        }
        return this;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public abstract void Update();

    public CharacterBehaviour(Character character)
    {
        this.character = character;
    }
}

public abstract class Transition
{
    private readonly CharacterBehaviour start;
    private readonly CharacterBehaviour end;

    public CharacterBehaviour Start => start;

    public CharacterBehaviour End => end;

    public Transition(CharacterBehaviour start, CharacterBehaviour end)
    {
        this.start = start;
        this.end = end;
    }

    public abstract bool IsConditionFullfilled();
}

public class ManualBehaviour : CharacterBehaviour
{
    protected ManualMover mover;

    public ManualBehaviour(Character character) : base(character)
    {
        mover = new ManualMover(character);
    }

    public override void Update()
    {
        mover.Move();
    }
}