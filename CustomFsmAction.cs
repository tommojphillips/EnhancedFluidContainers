using System;
using HutongGames.PlayMaker;

namespace TommoJProductions.EnchancedFluidContainers
{
    internal class CustomFsmAction : FsmStateAction
    {
        // Written, 16.04.2019

        internal Action action { get; set; }
        internal bool finishAfterAction { get; set; }

        private void OnEnter()
        {
            // Written, 16.04.2019

            this.action();
            if (this.finishAfterAction)
                this.Finish();
        }
    }
}
