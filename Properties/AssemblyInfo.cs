using System.Reflection;
using System.Resources;

// General Information
[assembly: AssemblyTitle("Enhanced Fluid Containers mod")]
[assembly: AssemblyProduct("EnhancedFluidContainers")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("Tommo J. Productions")]
[assembly: AssemblyCopyright("Copyright © Tommo J. Productions 2022")]
[assembly: AssemblyTrademark("Azine")]
[assembly: NeutralResourcesLanguage("en-AU")]
[assembly: AssemblyConfiguration("")]

// Version information
[assembly: AssemblyVersion("1.0.457.3")]
//[assembly: AssemblyFileVersion("1.0.457.3")]

public class VersionInfo
{
	public const string lastestRelease = "03.04.2023 07:17 PM";
	public const string version = "1.0.457.3";

    /// <summary>
    /// Represents if the mod has been complied for x64
    /// </summary>
    #if x64
        internal const bool IS_64_BIT = true;
    #else
        internal const bool IS_64_BIT = false;
    #endif
    #if DEBUG
        internal const bool IS_DEBUG_CONFIG = true;
    #else
        internal const bool IS_DEBUG_CONFIG = false;
    #endif
}

