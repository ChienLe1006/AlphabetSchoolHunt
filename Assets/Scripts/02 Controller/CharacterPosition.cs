using UnityEngine;

[System.Serializable]
public class CharacterPosition
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Role role;
    [SerializeField] private Alphabet alphabet;
    [SerializeField] private Skill skill;

    public Vector3 Position { get => position; }
    public Role Role { get => role; }
    public Alphabet Alphabet { get => alphabet; }
    public Skill Skill { get => skill; }

    public CharacterPosition(Vector3 position, Role role)
    {
        this.position = position;
        //if (role.HasFlag(Role.Seeker))
        //{
        //    role = Role.Seeker;
        //}
        //else if (role.HasFlag(Role.Hider))
        //{
        //    role = Role.Hider;
        //}

        //this.role = role;
    }
}