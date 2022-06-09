using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace TommoJProductions.EnhancedFluidContainers
{
    /// <summary>
    /// Represents enchanced fluid container logic.
    /// </summary>
    internal class EnhancedFluidContainerMono : MonoBehaviour
    {
        // Written, 06.04.2019

        internal class Trigger 
        {
            internal FsmBool pouring;
            internal FsmFloat maxCapacity;
            internal FsmFloat fluidLevel;
        }

        internal Trigger[] triggers;


        #region Fields

        /// <summary>
        /// Represents the default name of the fluid container. eg => 'coolant(itemx)'
        /// </summary>
        internal string defaultName;
        /// <summary>
        /// Represents the previous fluid (from last frame update) that was last updated. 
        /// </summary>
        private float previousFluid = 0;
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

        private void Start()
        {
            // Written, 06.04.2019

            PlayMakerFSM p = transform.GetChild(1).GetPlayMaker("Data");
            fluidContainerAmount = p.FsmVariables.FindFsmFloat("Fluid");
            fluidContainerPouringPosition = p.FsmVariables.FindFsmBool("Pouring");
        }

        internal void Update()
        {
            // Written, 06.04.2019

            if (previousFluid != fluidContainerAmount.Value || gameObject.name == defaultName)
            {
                updateName();
                foreach (Trigger t in triggers)
                {
                    updateFluidParticles(fluidContainerPouringPosition.Value && t.fluidLevel.Value < t.maxCapacity.Value);
                }

                previousFluid = fluidContainerAmount.Value;
                fluidParticlesChecked = false;
            }
            else if (!fluidParticlesChecked)
            {
                updateFluidParticles(false);
                fluidParticlesChecked = true;
            }
        }

        #region Methods
                
        /// <summary>
        /// Updates the fluid particles with the param.
        /// </summary>
        /// <param name="inActive">Activate fluid particles?</param>
        private void updateFluidParticles(bool inActive)
        {
            // Written, 16.04.2019

            transform.GetChild(0).gameObject.SetActive(inActive);
        }
        /// <summary>
        /// Updates the name of the fluid container with the current fluid amount (in l or ml)
        /// </summary>
        private void updateName()
        {
            // Written, 06.04.2019

            string fluidAmountDisplay = " - ";

            if (fluidContainerAmount.Value < 1)
                fluidAmountDisplay += (fluidContainerAmount.Value * 1000).ToString("F2") + "ML";
            else
                fluidAmountDisplay += fluidContainerAmount.Value.ToString("F2") + "L";

            int index = defaultName?.IndexOf("(itemx)") ?? -1;
            if (index >= 0 && index < defaultName.Length-1)
            gameObject.name = defaultName.Insert(index, fluidAmountDisplay);
        }
        internal void setDefaultName() 
        {
            defaultName = gameObject.name;
        }

        #endregion
    }
}
