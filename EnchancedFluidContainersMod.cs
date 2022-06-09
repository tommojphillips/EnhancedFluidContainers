using UnityEngine;
using MSCLoader;
using TommoJProductions.ModApi;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using static TommoJProductions.EnhancedFluidContainers.EnhancedFluidContainerMono;

namespace TommoJProductions.EnhancedFluidContainers
{
    public class EnchancedFluidContainersMod : Mod
    {
        // Written, 06.04.2019

        #region Mod Properties

        public override string ID => "EnchancedFluidContainers";
        public override string Name => "Enchanced Fluid Containers";
        public override string Version => VersionInfo.version;
        public override string Description => VersionInfo.lastestRelease;
        public override string Author => "tommojphillips";

        #endregion

        #region Fields

        #endregion


        public override void PreLoad()
        {
            activateGame_state = GameObject.Find("Systems/Setup Game").GetPlayMakerState("Activate game");
            spawner = activateGame_state.GetAction<ActivateGameObject>(2).gameObject.GameObject.Value;
            createItems = spawner.transform.FindChild("CreateItems").gameObject;
            inject();

            ModConsole.Print(string.Format("{0} v{1}: Loaded", this.Name, this.Version));
        }

        private FsmState activateGame_state;
        private PlayMakerFSM tempFsm;
        private GameObject spawner;
        private GameObject createItems;
        internal class TriggerExt : Trigger
        {
            private PlayMakerFSM _trigger;

            internal PlayMakerFSM trigger 
            {
                get => _trigger;
                set
                {
                    fluidLevel = value.FsmVariables.FloatVariables[0];
                    maxCapacity = value.FsmVariables.FloatVariables[1];
                    pouring = value.FsmVariables.BoolVariables[0];
                    _trigger = value;
                }
            }
        }
        
        private FsmGameObject twoStroke;
        private TriggerExt[] twoStrokeTriggers;

        private FsmGameObject coolant;
        private TriggerExt[] coolantTriggers;

        private FsmGameObject motorOil;
        private TriggerExt[] motorOilTriggers;

        private FsmGameObject brakeFluid;
        private TriggerExt[] brakeFluidTriggers;

        private void inject()
        {
            // Written, 07.06.2022

            GameObject databaseMotor = GameObject.Find("Database/DatabaseMotor");
            GameObject databaseOrders = GameObject.Find("Database/DatabaseOrders");
            GameObject databaseMechanics = GameObject.Find("Database/DatabaseMechanics");
            GameObject cylinderhead = databaseMotor.transform.FindChild("Cylinderhead").GetPlayMaker("Data").FsmVariables.FindFsmGameObject("ThisPart").Value;
            GameObject clutchMasterCylinder = databaseMechanics.transform.FindChild("ClutchMasterCylinder").GetPlayMaker("Data").FsmVariables.FindFsmGameObject("ActivateThis").Value;
            GameObject brakeMasterCylinder = databaseMechanics.transform.FindChild("BrakeMasterCylinder").GetPlayMaker("Data").FsmVariables.FindFsmGameObject("ActivateThis").Value;
            GameObject racingRad = databaseOrders.transform.FindChild("Racing Radiator").GetPlayMaker("Data").FsmVariables.FindFsmGameObject("ActivateThis").Value;
            GameObject rad = databaseMechanics.transform.FindChild("Radiator").GetPlayMaker("Data").FsmVariables.FindFsmGameObject("ActivateThis").Value;

            // two stroke
            tempFsm = createItems.GetPlayMaker("TwoStroke");
            twoStroke = tempFsm.FsmVariables.FindFsmGameObject("New");
            twoStrokeTriggers = new TriggerExt[2];
            twoStrokeTriggers[0] = new TriggerExt() { trigger = GameObject.Find("JONNEZ ES(Clone)/LOD/FuelFiller/OpenCap/CapTrigger_TwoStrokeFuel").GetComponent<PlayMakerFSM>() };
            twoStrokeTriggers[1] = new TriggerExt() { trigger = GameObject.Find("BOAT/GFX/Motor/Pivot/FuelFiller/OpenCap/CapTrigger_TwoStrokeFuel").GetComponent<PlayMakerFSM>() };
            tempFsm.GetState("Create product").appendNewAction(onTwoStrokeSpawn);
            tempFsm.GetState("Add ID").insertNewAction(onTwoStrokeSpawn, 5);

            // motor oil
            tempFsm = createItems.GetPlayMaker("MotorOil");
            motorOil = tempFsm.FsmVariables.FindFsmGameObject("New");
            motorOilTriggers = new TriggerExt[1];
            motorOilTriggers[0] = new TriggerExt() { trigger = cylinderhead.transform.FindChild("Bolts/CapTrigger_MotorOil").GetComponent<PlayMakerFSM>() };
            tempFsm.GetState("Create product").appendNewAction(onMotorOilSpawn);
            tempFsm.GetState("Add ID").insertNewAction(onMotorOilSpawn, 5);

            // coolant
            tempFsm = createItems.GetPlayMaker("Coolant");
            coolant = tempFsm.FsmVariables.FindFsmGameObject("New");
            coolantTriggers = new TriggerExt[2];
            coolantTriggers[0] = new TriggerExt() { trigger = rad.transform.FindChild("OpenCap/CapTrigger_Coolant1").GetComponent<PlayMakerFSM>() };
            coolantTriggers[1] = new TriggerExt() { trigger = racingRad.transform.FindChild("OpenCap/CapTrigger_Coolant2").GetComponent<PlayMakerFSM>() };
            tempFsm.GetState("Create product").appendNewAction(onCoolantSpawn);
            tempFsm.GetState("Add ID").insertNewAction(onCoolantSpawn, 5);

            // brake fluid
            tempFsm = createItems.GetPlayMaker("BrakeFluid");
            brakeFluid = tempFsm.FsmVariables.FindFsmGameObject("New");
            brakeFluidTriggers = new TriggerExt[3];
            brakeFluidTriggers[0] = new TriggerExt() { trigger = clutchMasterCylinder.transform.FindChild("OpenCap/CapTrigger_Clutch").GetComponent<PlayMakerFSM>() };
            brakeFluidTriggers[1] = new TriggerExt() { trigger = brakeMasterCylinder.transform.FindChild("OpenCap/CapTrigger_BrakeR").GetComponent<PlayMakerFSM>() };
            brakeFluidTriggers[2] = new TriggerExt() { trigger = brakeMasterCylinder.transform.FindChild("OpenCap/CapTrigger_BrakeF").GetComponent<PlayMakerFSM>() };
            tempFsm.GetState("Create product").appendNewAction(onBrakeFluidSpawn);
            tempFsm.GetState("Add ID").insertNewAction(onBrakeFluidSpawn, 5);

            ModConsole.Print("create items injected");
        }

        private void onTwoStrokeSpawn()
        {
            // Written, 07.06.2022

            onSpawn(twoStroke, twoStrokeTriggers);
            twoStroke.Value.transform.GetChild(0).localPosition = new Vector3(-0.003f, -0.12f, 0.193f);
        }
        private void onCoolantSpawn()
        {
            // Written, 07.06.2022

            onSpawn(coolant, coolantTriggers);
        }
        private void onBrakeFluidSpawn()
        {
            // Written, 07.06.2022

            onSpawn(brakeFluid, brakeFluidTriggers);
        }
        private void onMotorOilSpawn()
        {
            // Written, 07.06.2022

            onSpawn(motorOil, motorOilTriggers);
        }

        private void onSpawn(FsmGameObject g, params Trigger[] triggers)
        {
            // Written, 08.06.2022

            EnhancedFluidContainerMono e = g.Value.AddComponent<EnhancedFluidContainerMono>();
            g.Value.GetPlayMakerState("State 1").insertNewAction(e.setDefaultName, 3);
            e.triggers = triggers;
#if DEBUG
            ModConsole.Print(string.Format("<b>[EFC.onSpawn]</b> - {0} injected", g.Value.name));
#endif
        }
    }
}
