using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace TommoJProductions.EnchancedFluidContainers
{
    /// <summary>
    /// Represents enchanced fluid container logic.
    /// </summary>
    internal class EnchancedFluidContainerMono : MonoBehaviour
    {
        // Written, 06.04.2019

        #region Fields

        /// <summary>
        /// Represents the default name of the fluid container. eg => 'coolant(itemx)'
        /// </summary>
        private string defaultName;
        /// <summary>
        /// Represents the previous fluid (from last frame update) that was last updated. 
        /// </summary>
        private float previousFluid;
        /// <summary>
        /// Represents current amount of fluid in the container.
        /// </summary>
        private FsmFloat fluidContainerAmount;
        /// <summary>
        /// Represents if the fluid container is in a pouring position.
        /// </summary>
        private FsmBool fluidContainerPouringPosition;
        /// <summary>
        /// Represents if the fp haave been checked.
        /// </summary>
        private bool fluidParticlesChecked = true;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the type of container this is.
        /// </summary>
        internal FluidContainersEnum type
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the triggers for the fluid container.
        /// </summary>
        internal Dictionary<GameObject, string> triggers
        {
            get;
            set;
        }

        #endregion

        private void Start()
        {
            // Written, 06.04.2019

            this.defaultName = this.gameObject.name;
            this.fluidContainerAmount = PlayMakerFSM.FindFsmOnGameObject(this.transform.GetChild(1).gameObject, "Data").FsmVariables.FindFsmFloat("Fluid");
            this.fluidContainerPouringPosition = PlayMakerFSM.FindFsmOnGameObject(this.transform.GetChild(1).gameObject, "Data").FsmVariables.FindFsmBool("Pouring");
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

        #region Methods

        /// <summary>
        /// Fluid particles logic
        /// </summary>
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
                        this.updateFluidParticles(this.fluidContainerPouringPosition.Value && fluidLevel < maxCapacity);
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Updates the fluid particles with the param.
        /// </summary>
        /// <param name="inActive">Activate fluid particles?</param>
        private void updateFluidParticles(bool inActive)
        {
            // Written, 16.04.2019

            this.transform.GetChild(0).gameObject.SetActive(inActive);
        }
        /// <summary>
        /// Updates the name of the fluid container with the current fluid amount (in l or ml)
        /// </summary>
        private void updateName()
        {
            // Written, 06.04.2019

            string fluidAmountDisplay = " - ";

            if (this.fluidContainerAmount.Value < 1)
                fluidAmountDisplay += (this.fluidContainerAmount.Value * 1000).ToString("F2") + "ML";
            else
                fluidAmountDisplay += this.fluidContainerAmount.Value.ToString("F2") + "L";

            this.gameObject.name = this.defaultName.Insert(this.defaultName.IndexOf("(itemx)"), fluidAmountDisplay);
        }

        #endregion
    }
}
