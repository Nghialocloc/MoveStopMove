using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSight : MonoBehaviour
{
    [SerializeField] private Character character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_CHARACTER))
        {
            Character target = Cache.GetCharacter(other);
            if (!target.IsDead)
            {
                character.AddTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAG_CHARACTER))
        {
            Character target = Cache.GetCharacter(other);
            character.RemoveTarget(target);
        }
    }
}
