using MSCLoader;

namespace TommoJProductions.EnchancedFluidContainers
{
    public class EnchancedFluidContainersMod : Mod
    {
        public override string ID => "EnchancedFluidContainers";
        public override string Name => "Enchaned Fluid Containers";
        public override string Version => "0.1";
        public override string Author => "tommojphillips";

        public override void OnLoad()
        {
            // Written, 06.04.2019

            ModConsole.Print(string.Format("{0} v{1}: Loaded", this.Name, this.Version));
        }
    }
}
