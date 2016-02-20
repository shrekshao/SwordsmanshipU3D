//using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public enum MouseMovementsInput
    {
        Idle,

        Attack_SwingLeftIdle,
        Attack_SwingLeft,
        Attack_SwingRightIdle,
        Attack_SwingRight,
        Attack_StabIdle,
        Attack_Stab,

        Block_Front,
        Block_Left,
        Block_Right,

        Special_DragonRoar
    };

    public class MouseInputStruct
    {
        public MouseMovementsInput mouseMovementInput;

        //TODO: speed, pattern completions...

        public MouseInputStruct()
        {

        }
    }
}

