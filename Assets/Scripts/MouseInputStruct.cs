//using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public enum MouseMovementsInput
    {
        Idle,

        Attack_SwingLeft,
        Attack_SwingRight,
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

        MouseInputStruct()
        {

        }
    }
}

