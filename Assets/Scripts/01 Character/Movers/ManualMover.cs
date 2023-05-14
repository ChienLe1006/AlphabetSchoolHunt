using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ManualMover : MoveBehaviour, IMovable
{
    public ManualMover(Character character) : base(character)
    {
        this.IsMoveByDistance = true;
    }

    public override Vector3 GetNextPosition()
    {
        float x = UltimateJoystick.GetHorizontalAxis(Constants.MAIN_JOINSTICK);
        float z = UltimateJoystick.GetVerticalAxis(Constants.MAIN_JOINSTICK);

#if UNITY_EDITOR
        if (x == 0 && z == 0)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
#endif

        if (x != 0 || z != 0)
        {
            SoundManager.Instance.PlayFootStep();
        }
        else
        {
            SoundManager.Instance.StopFootStep();
        }

        return new Vector3(x, 0, z).normalized * Character.Speed;
    }
}
