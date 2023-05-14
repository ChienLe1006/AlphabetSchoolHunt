using System.Collections.Generic;
using UnityEngine;

public static class CharacterAnimatorHashedKey
{
    private static readonly Dictionary<CharacterAction, int> actionHashedKeys = new Dictionary<CharacterAction, int>();

    public static int ToAnimatorHashedKey(this CharacterAction action)
    {
        if (!actionHashedKeys.ContainsKey(action))
        {
            actionHashedKeys[action] = Animator.StringToHash(action.ToString());
        }
        return actionHashedKeys[action];
    }
}
