using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Character player;

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && character.CanHurt)
        {
            player.CharacterAnimator.SetBool(CharacterAction.Chase.ToAnimatorHashedKey(), true);
            player.CharacterAnimator.SetFloat("AttackMotion", Random.Range(0, 2));
        }            
    }

    private void OnTriggerExit(Collider other)
    {
        player.CharacterAnimator.SetBool(CharacterAction.Chase.ToAnimatorHashedKey(), false);
        player.StopSkillVFX();
    }
}
