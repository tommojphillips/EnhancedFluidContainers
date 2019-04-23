using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MSCLoader;

namespace TommoJProductions.EnchancedFluidContainers
{
    public class EnchancedFluidContainersMod : Mod
    {
        // Written, 06.04.2019

        #region Mod Properties

        public override string ID => "EnchancedFluidContainers";
        public override string Name => "Enchaned Fluid Containers";
        public override string Version => "0.1";
        public override string Author => "tommojphillips";

        #endregion

        #region Fields

        private Dictionary<FluidContainersEnum, string> fluidContainers;

        #endregion

        public override void OnLoad()
        {
            // Written, 06.04.2019

            this.fluidContainers = new Dictionary<FluidContainersEnum, string>();
            this.fluidContainers.Add(FluidContainersEnum.coolant, "coolant(itemx)");
            this.fluidContainers.Add(FluidContainersEnum.brake_fluid, "brake fluid(itemx)");
            this.fluidContainers.Add(FluidContainersEnum.motor_oil, "motor oil(itemx)");
            //this.fluidContainers.Add(FluidContainers.diesel, "diesel(itemx)");
            //this.fluidContainers.Add(FluidContainers.gasoline, "gasoline(itemx)");
            this.setFluidContainers();
            EnchancedFluidContainerStoreMono enchancedFluidContainerStoreMono = GameObject.Find("STORE/StoreCashRegister/Register").AddComponent<EnchancedFluidContainerStoreMono>();
            enchancedFluidContainerStoreMono.action = setFluidContainers;

            ModConsole.Print(string.Format("{0} v{1}: Loaded", this.Name, this.Version));
        }

        private void setFluidContainers()
        {
            // Written, 06.04.2019

            // getting all instances of vaild fluid containers..
            foreach (KeyValuePair<FluidContainersEnum, string> _fluidContainers in this.fluidContainers)
            {
                foreach (GameObject fluidContainerGo in Object.FindObjectsOfType<GameObject>().Where(_go => _go.name == _fluidContainers.Value))
                {
                    EnchancedFluidContainerMono fluidContainerMono = fluidContainerGo.GetComponent<EnchancedFluidContainerMono>();

                    if (fluidContainerMono is null)
                    {
                        fluidContainerMono = fluidContainerGo.AddComponent<EnchancedFluidContainerMono>();

                        switch (_fluidContainers.Key)
                        {
                            case FluidContainersEnum.coolant:
                                fluidContainerMono.triggers = new Dictionary<GameObject, string>()
                                {
                                    { GameObject.Find("SATSUMA(557kg, 248)/MiscParts/radiator(xxxxx)/OpenCap/CapTrigger_Coolant1"), "Trigger" },
                                    { GameObject.Find("SATSUMA(557kg, 248)/MiscParts/racing radiator(xxxxx)/OpenCap/CapTrigger_Coolant2"), "Trigger" }
                                };
                                fluidContainerMono.type = FluidContainersEnum.coolant;
                                break;
                            case FluidContainersEnum.brake_fluid:
                                fluidContainerMono.triggers = new Dictionary<GameObject, string>()
                                {
                                    { GameObject.Find("brake master cylinder(xxxxx)/OpenCap/CapTrigger_BrakeF"), "Trigger" },
                                    { GameObject.Find("brake master cylinder(xxxxx)/OpenCap/CapTrigger_BrakeR"), "Trigger" },
                                    { GameObject.Find("clutch master cylinder(xxxxx)/OpenCap/CapTrigger_Clutch"), "Trigger" },
                                };
                                fluidContainerMono.type = FluidContainersEnum.brake_fluid;
                                break;
                            case FluidContainersEnum.motor_oil:
                                fluidContainerMono.triggers = new Dictionary<GameObject, string>()
                                {
                                    { GameObject.Find("SATSUMA(557kg, 248)/Chassis/sub frame(xxxxx)/CarMotorPivot/block(Clone)/pivot_cylinder head/cylinder head(Clone)/Bolts/CapTrigger_MotorOil"), "Trigger" },
                                };
                                fluidContainerMono.type = FluidContainersEnum.motor_oil;
                                break;
                            default:
                                fluidContainerMono.triggers = null;
                                break;
                        }
                    }
                }
            }
        }
    }
}
