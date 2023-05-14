using System.Collections.Generic;
using UnityEngine;

public class CharactersContainer
{
    private readonly Character player;
    private readonly Stack<Character> availableHiders = new Stack<Character>();
    private readonly Stack<Character> availableSeekers = new Stack<Character>();
    private Dictionary<Role, Stack<Character>> availableCharacters = new Dictionary<Role, Stack<Character>>();

    public CharactersContainer(Character playerPrefab)
    {
        //player = SpawnCharacter(playerPrefab);
        //player.gameObject.SetActive(false);
    }

    private Character SpawnCharacter(Role role)
    {
        var characters = GameManager.Instance.CharacterPrefabs;
        var character = Object.Instantiate(characters[0]);
        character.SetRole(role);
        return character;
    }

    public Character GetCharacter(Role role, bool isActive = true)
    {
        Character character = null;
        if (availableCharacters.ContainsKey(role))
        {
            var stack = availableCharacters[role];
            if (stack.Count > 0)
            {
                character = stack.Pop();
            }
        }

        if (!character)
        character = SpawnCharacter(role);
        return character;
    }

    public void ReturnCharacter(Character character)
    {
        if (!character)
            return;

        character.gameObject.SetActive(false);

        if (availableCharacters.ContainsKey(character.Role))
        {
            var stack = availableCharacters[character.Role];
            stack.Push(character);
        }
        else
        {
            var stack = new Stack<Character>();
            stack.Push(character);
            availableCharacters.Add(character.Role, stack);
        }


    }
}
