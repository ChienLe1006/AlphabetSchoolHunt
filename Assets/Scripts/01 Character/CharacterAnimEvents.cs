using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimEvents : MonoBehaviour
{
    [SerializeField] private Character character;

    public void Attack()
    {
        character.FieldOfView.Attack();
    }

    public void ResetAttack()
    {
        character.IsAttack = false;
        Debug.Log("reset attack");
    }
}
