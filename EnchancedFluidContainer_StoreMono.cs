using System.Threading;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace TommoJProductions.EnchancedFluidContainers
{
    /// <summary>
    /// 
    /// </summary>
    internal class EnchancedFluidContainer_StoreMono : MonoBehaviour
    {
        // Written, 17.04.2019

        /// <summary>
        /// Represents the quantity of brake fluid brought in previous purchase.
        /// </summary>
        private FsmInt brakefluidQuantity;
        /// <summary>
        /// Represents the quantity of motor oil brought in previous purchase.
        /// </summary>
        private FsmInt motorOilQuantity;
        /// <summary>
        /// Represents the quantity of coolant brought in previous purchase.
        /// </summary>
        private FsmInt coolantQuantity;
        /// <summary>
        /// Represents if logic should check shopping bags for brakefluid.
        /// </summary>
        private bool shoppingBagSpawn = false;
        /// <summary>
        /// Represents the delay (ms) for applying logic after item purchase.
        /// </summary>
        private const int delay = 2500;

        /// <summary>
        /// Occurs when mono starts.
        /// </summary>
        private void Start()
        {
            // Written, 17.04.2019

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
                this.shoppingBagSpawn = true;
            }
            if (fluidContainerSpawned)
            {
                this.fluidContainerSpawned();
#if DEBUG
                ModConsole.Print("<b>[playerPurchasedItems] -</b> Player purchased vaild fluid container/s");
#endif
            }
        }
        /// <summary>
        /// Occurs when a purchase contains one or more vaild fluid containers.
        /// </summary>
        private void fluidContainerSpawned()
        {
            // Written, 02.05.2019

            Thread spawnThread = new Thread(delegate ()
            {
                Thread.Sleep(delay); // 2.5f second delay (allows time for items to spawn).

                if (this.shoppingBagSpawn)
                {
                    GameObject shoppingBagSpawn = GameObject.Find("STORE/LOD/Shop/ShoppingBagSpawn"); // Location of all shopping bags in game.

                    for (int i = 0; i < shoppingBagSpawn.transform.childCount; i++) // enumerating over all child gameobjets (a shopping bag) of bag spawn.
                    {
                        GameObject shoppingBag = shoppingBagSpawn.transform.GetChild(i).gameObject;
                        GameObject spawnBag = shoppingBag.transform.GetChild(shoppingBag.transform.childCount - 1).gameObject; // SpawnBag child contains all items purchased (with an id).

                        for (int j = 0; j < spawnBag.transform.childCount; j++)
                        {
                            GameObject child = spawnBag.transform.GetChild(j).gameObject; // maybe brake fluid conatiner.

                            if (child.name.Contains("brakefluid"))
                            {
                                FsmHook.FsmInject(shoppingBag, "Play anim", this.fluidContainerSpawned);
#if DEBUG
                                ModConsole.Print(string.Format("<b>[fluidContainerSpawned] -</b> Injected shopping bag, (index: {0}).", j));
#endif
                                break;
                            }
                        }
                    }
                    this.shoppingBagSpawn = false;
                }
                EnchancedFluidContainersMod.setFluidContainers();
            });
            spawnThread.Start();
#if DEBUG            
            ModConsole.Print("<b>[fluidContainerSpawned] -</b> One or more vaild fluid container/s purchased; efcm component addition delayed by, " + delay + "ms");
            if (this.shoppingBagSpawn)
                ModConsole.Print("<b>[fluidContainerSpawned] -</b> Shopping bag will be injected.");
#endif
        }
    }
}
