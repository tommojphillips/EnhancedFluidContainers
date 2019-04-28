using System.Threading;
using System.Linq;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;
using System;

namespace TommoJProductions.EnchancedFluidContainers
{
    /// <summary>
    /// 
    /// </summary>
    internal class EnchancedFluidContainer_StoreMono : MonoBehaviour
    {
        // Written, 17.04.2019

        private FsmInt brakefluidQuantity;
        private FsmInt motorOilQuantity;
        private FsmInt coolantQuantity;

        private void Start()
        {
            // Written, 17.04.2019

            //FsmHook.FsmInject(this.gameObject, "Spawn 11", this.coolantSpawn);
            //FsmHook.FsmInject(this.gameObject, "Spawn 12", this.brakeFluidSpawn);
            //FsmHook.FsmInject(this.gameObject, "Spawn 13", this.motorOilSpawn);

            FsmHook.FsmInject(this.gameObject, "Purchase", this.playerPurchasedItems);

            PlayMakerFSM playMakerFSM = this.gameObject.GetComponent<PlayMakerFSM>();
            this.brakefluidQuantity = playMakerFSM.FsmVariables.FindFsmInt("QBrakeFluid");
            this.motorOilQuantity = playMakerFSM.FsmVariables.FindFsmInt("QMotorOil");
            this.coolantQuantity = playMakerFSM.FsmVariables.FindFsmInt("QCoolant");
        }
        /// <summary>
        /// Occurs when the player has purchased items at temio's shop.
        /// </summary>
        private void playerPurchasedItems()
        {
            // Written, 28.04.2019

            bool fluidContainerSpawned = false;

            if (this.motorOilQuantity.Value > 0 || this.coolantQuantity.Value > 0)
            {
                fluidContainerSpawned = true;
            }
            if (this.brakefluidQuantity.Value > 0)
            {
                fluidContainerSpawned = true;
                this.brakeFluidSpawn();
            }
            if (fluidContainerSpawned)
            {
                this.fluidContainerSpawned();
#if DEBUG
                ModConsole.Print("Player purchased vaild fluid container/s");
#endif
            }
        }
        /// <summary>
        /// Occurs when a purchase contains one or more vaild fluid containers.
        /// </summary>
        private void fluidContainerSpawned()
        {
            // Written, 28.04.2019

            int delay = 2500;
            Thread spawnThread = new Thread(delegate ()
            {
                Thread.Sleep(delay); // 2.5f second delay (allows time for items to spawn).

                EnchancedFluidContainersMod.setFluidContainers();
#if DEBUG
                ModConsole.Print("fluid container/s set.");
#endif
            });
            spawnThread.Start();
#if DEBUG
            ModConsole.Print("One or more vaild fluid container/s purchased; efcm component addition delayed by, " + delay + "ms");
#endif
        }
        /// <summary>
        /// Occurs when a purchase contains one or more brake fluid/s.
        /// </summary>
        private void brakeFluidSpawn()
        {
            // Written, 28.04.2019

            //this.spawn(FluidContainersEnum.brake_fluid, this.brakefluidQuantity.Value);
#if DEBUG
            ModConsole.Print("one or more brake fluid/s purchased");
#endif
        }
    }
}
