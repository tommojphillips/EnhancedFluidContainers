using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TommoJProductions.EnchancedFluidContainers
{
    internal class EnchancedFluidContainerStoreMono : MonoBehaviour
    {
        // Written, 17.04.2019

        internal Action action;

        private void Start()
        {
            // Written, 17.04.2019

            try
            {
                FsmState fsmState = (this.GetComponent<PlayMakerFSM>().FsmStates).First((state => state.Name.ToLower() == "purchase"));
                CustomFsmAction fsmCustomAction = new CustomFsmAction()
                {
                    action = this.action,
                    finishAfterAction = false,
                };

                List<FsmStateAction> list = fsmState.Actions.ToList();
                list.Add(fsmCustomAction);
                fsmState.Actions = list.ToArray();
                MSCLoader.ModConsole.Print("purchased state hooked");
            }
            catch (Exception ex)
            {
                MSCLoader.ModConsole.Print("An error occured: " + ex);
            }
            
        }
    }
}
