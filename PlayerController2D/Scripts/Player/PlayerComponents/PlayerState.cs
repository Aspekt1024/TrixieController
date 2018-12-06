using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Aspekt.PlayerController
{
    public class PlayerState
    {
        private Dictionary<StateLabels, object> state = new Dictionary<StateLabels, object>();

        public bool Check(StateLabels label)
        {
            if (state.ContainsKey(label))
            {
                return state[label].Equals(true);
            }
            else
            {
                return false;
            }
        }

        public float GetFloat(StateLabels label)
        {
            if (state.ContainsKey(label))
            {
                return (float)state[label];
            }
            else
            {
                return 0f;
            }
        }

        public void Set(StateLabels label, object newValue)
        {
            if (state.ContainsKey(label))
            {
                state[label] = newValue;
            }
            else
            {
                state.Add(label, newValue);
            }
        }
    }

}

