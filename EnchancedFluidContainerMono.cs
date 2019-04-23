using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace TommoJProductions.EnchancedFluidContainers
{
    internal class EnchancedFluidContainerMono : MonoBehaviour
    {
        // Written, 06.04.2019

        private string defaultName;
        private float previousFluid;
        private FsmFloat fluidContainerAmount;
        private FsmBool fluidContainerPouring;
        private bool fluidParticlesChecked = true;
        internal FluidContainersEnum type
        {
            get;
            set;
        }
        internal Dictionary<GameObject, string> triggers
        {
            get;
            set;
        }

        private void Start()
        {
            // Written, 06.04.2019

            this.defaultName = this.gameObject.name;
            this.fluidContainerAmount = PlayMakerFSM.FindFsmOnGameObject(this.transform.GetChild(1).gameObject, "Data").FsmVariables.FindFsmFloat("Fluid");
            this.fluidContainerPouring = PlayMakerFSM.FindFsmOnGameObject(this.transform.GetChild(1).gameObject, "Data").FsmVariables.FindFsmBool("Pouring");
        }

        private void Update()
        {
            // Written, 06.04.2019

            try
            {
                if (this.previousFluid != this.fluidContainerAmount.Value)
                {
                    this.updateName();
                    this.fluidParticles();
                    this.previousFluid = this.fluidContainerAmount.Value;
                    this.fluidParticlesChecked = false;
                }
                else
                {
                    if (!this.fluidParticlesChecked)
                    {
                        this.updateFluidParticles(false);
                        this.fluidParticlesChecked = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModConsole.Error("Error: " + ex.ToString());
            }
        }
        private void fluidParticles()
        {
            // Written, 06.04.2019

            if (this.triggers == null)
                return;

            foreach (KeyValuePair<GameObject, string> _trigger in this.triggers)
            {
                PlayMakerFSM trigger = PlayMakerFSM.FindFsmOnGameObject(_trigger.Key, _trigger.Value);

                if (trigger != null)
                {
                    if (trigger.FsmVariables.FindFsmBool("Pouring").Value)
                    {
                        float maxCapacity = trigger.FsmVariables.FindFsmFloat("MaxCapacity").Value;
                        float fluidLevel;

                        switch (this.type)
                        {
                            case FluidContainersEnum.motor_oil:
                                fluidLevel = trigger.FsmVariables.FindFsmFloat("OilLevel").Value;
                                break;
                            default:
                                fluidLevel = trigger.FsmVariables.FindFsmFloat("FluidLevel").Value;
                                break;
                        }
                        this.updateFluidParticles(this.fluidContainerPouring.Value && fluidLevel < maxCapacity);
                    }
                    break;
                }
            }
        }
        private void updateFluidParticles(bool inActive)
        {
            // Written, 16.04.2019

            this.transform.GetChild(0).gameObject.SetActive(inActive);
        }
        private void updateName()
        {
            // Written, 06.04.2019

            this.gameObject.name = this.defaultName.Insert(this.defaultName.IndexOf("(itemx)"), string.Format(" - {0}L", this.fluidContainerAmount.Value));
        }
    }
}
