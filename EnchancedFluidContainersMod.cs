using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MSCLoader;

namespace TommoJProductions.EnhancedFluidContainers
{
    public class EnchancedFluidContainersMod : Mod
    {
        // Written, 06.04.2019

        #region Mod Properties

        public override string ID => "EnchancedFluidContainers";
        public override string Name => "Enchanced Fluid Containers";
        public override string Version => "0.1.5";
        public override string Author => "tommojphillips";

        #endregion

        #region Fields

        internal static Dictionary<FluidContainersEnum, string> fluidContainerNames =>  
            new Dictionary<FluidContainersEnum, string>()
            {
                { FluidContainersEnum.coolant, "coolant(itemx)" },
                { FluidContainersEnum.brake_fluid, "brake fluid(itemx)" },
                { FluidContainersEnum.motor_oil, "motor oil(itemx)" },
                { FluidContainersEnum.two_stroke_fuel, "two stroke fuel(itemx)" }
                //{ FluidContainers.diesel, "diesel(itemx)" },
                //{ FluidContainers.gasoline, "gasoline(itemx)" }
            };
        internal static Dictionary<FluidContainersEnum, Dictionary<GameObject, string>> fluidContainerTriggers =>
            new Dictionary<FluidContainersEnum, Dictionary<GameObject, string>>()
            {
                { FluidContainersEnum.coolant,
                    new Dictionary<GameObject, string>()
                    {
                        {
                            GameObject.Find("SATSUMA(557kg, 248)/MiscParts/radiator(xxxxx)/OpenCap/CapTrigger_Coolant1"),
                            "Trigger"
                        },
                        {
                            GameObject.Find("SATSUMA(557kg, 248)/MiscParts/racing radiator(xxxxx)/OpenCap/CapTrigger_Coolant2"),
                            "Trigger"
                        }
                    }
                },
                { FluidContainersEnum.brake_fluid,
                    new Dictionary<GameObject, string>()
                    {
                        {
                            GameObject.Find("brake master cylinder(xxxxx)/OpenCap/CapTrigger_BrakeF"),
                            "Trigger"
                        },
                        {
                            GameObject.Find("brake master cylinder(xxxxx)/OpenCap/CapTrigger_BrakeR"),
                            "Trigger"
                        },
                        {
                            GameObject.Find("clutch master cylinder(xxxxx)/OpenCap/CapTrigger_Clutch"),
                            "Trigger"
                        },
                    }

                },
                { FluidContainersEnum.motor_oil,
                    new Dictionary<GameObject, string>()
                    {
                        {
                            GameObject.Find("SATSUMA(557kg, 248)/Chassis/sub frame(xxxxx)/CarMotorPivot/block(Clone)/pivot_cylinder head/cylinder head(Clone)/Bolts/CapTrigger_MotorOil"),
                            "Trigger"
                        },
                    }
                },
                {
                    FluidContainersEnum.two_stroke_fuel,
                    new Dictionary<GameObject, string>()
                    {
                        {
                            GameObject.Find("JONNEZ ES(Clone)/LOD/FuelFiller/OpenCap/CapTrigger_TwoStrokeFuel"),
                            "Trigger"
                        },
                        {
                            GameObject.Find("BOAT/GFX/Motor/Pivot/FuelFiller/OpenCap/CapTrigger_TwoStrokeFuel"),
                            "Trigger"
                        }
                    }
                }
            };

        #endregion

        public override void OnLoad()
        {
            // Written, 06.04.2019

            // Setting up current fluid containers in the world.
            setFluidContainers();
            // Setting up store hooks etc.
            GameObject.Find("STORE/StoreCashRegister/Register").AddComponent<EnhancedFluidContainer_StoreMono>();

            ModConsole.Print(string.Format("{0} v{1}: Loaded", this.Name, this.Version));
        }

        internal static void setFluidContainers()
        {
            // Written, 06.04.2019

            // getting all instances of vaild fluid containers..
            foreach (KeyValuePair<FluidContainersEnum, string> _fluidContainers in fluidContainerNames)
            {
                foreach (GameObject fluidContainerGo in Object.FindObjectsOfType<GameObject>().Where(_go => _go.name == _fluidContainers.Value))
                {
                    EnhancedFluidContainerMono fluidContainerMono = fluidContainerGo.GetComponent<EnhancedFluidContainerMono>();

                    if (fluidContainerMono is null)
                    {
                        fluidContainerMono = fluidContainerGo.AddComponent<EnhancedFluidContainerMono>();
                        fluidContainerMono.triggers = fluidContainerTriggers[_fluidContainers.Key];
                        fluidContainerMono.type = _fluidContainers.Key;
#if DEBUG
                            ModConsole.Print(string.Format("<b>[setFluidContainers] -</b> Vaild fluid container found withOUT efcm addition, '{0}'", fluidContainerGo.name));
#endif
                    }
                }
            }
        }
    }
}
