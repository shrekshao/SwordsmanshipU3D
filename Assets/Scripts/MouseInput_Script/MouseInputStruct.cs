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

        Block_Exit,

        Special_DragonRoar
    };

    public class MouseInputStruct
    {
        public MouseMovementsInput mouseMovementInput = MouseMovementsInput.Idle;

        //TODO: speed, pattern completions...

        //public MouseInputStruct()
        //{
        //    mouseMovementInput = MouseMovementsInput.Idle;
        //}

        public MouseInputStruct(MouseMovementsInput m)
        {
            mouseMovementInput = m;
        }
    }
}

