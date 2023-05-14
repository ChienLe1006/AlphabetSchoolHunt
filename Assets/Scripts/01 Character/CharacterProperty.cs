using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CharacterProperty
{
    [BoxGroup]
    [SerializeField] private FieldOfViewParameter fieldOfViewParameter;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float hearingRadius = 10f;
    [SerializeField] private Role role;
    [SerializeField] private float attackDamage;
    [SerializeField] private float hp;

    public CharacterProperty()
    {
    }

    public CharacterProperty(FieldOfViewParameter fieldOfViewParameter, float speed, float hearingRadius, Role role, float attackDamage, float hp)
    {
        this.fieldOfViewParameter = fieldOfViewParameter;
        this.speed = speed;
        this.hearingRadius = hearingRadius;
        this.role = role;
        this.attackDamage= attackDamage;
        this.hp = hp;
    }

    public FieldOfViewParameter FieldOfViewParameter { get => fieldOfViewParameter; }
    public float Speed { get => speed; }
    public float HearingRadius { get => hearingRadius; }
    public Role Role { get => role; }
    public float AttackDamage { get => attackDamage; }
    public float HP { get => hp; set => hp = value; }
}
