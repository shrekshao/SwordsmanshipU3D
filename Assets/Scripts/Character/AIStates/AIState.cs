using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public abstract class AIState
    {

        public abstract AIState Update(AISwordsmanControl aiSwordsmanControl);
    }
}

