using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens
{
    enum TriggerState {DefaultState, ReverseState};
    class Trigger
    {
 
        private bool flag;
        public TriggerState State
        {
            get
            {
                if (flag) return TriggerState.DefaultState;
                else return TriggerState.ReverseState; 
            }
        }
        public void Switch()
        {
            flag = !flag;
        }
        public bool Value { get { return flag; } }
        public Trigger()
        {

        }

        public Trigger(bool start_state)
        {
            flag = start_state; 
        }
    }
}
