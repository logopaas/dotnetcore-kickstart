using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NAFCore.Common.Attributes;
using NAFCore.Common.Localization.Attributes;
using NAFCore.Platform.Services.Client;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("LOGO Group")]
[assembly: AssemblyProduct("Logo Paas Sample Application")]
[assembly: AssemblyTrademark("Logo Group™")]
[assembly: AssemblyDescription("Logo Paas Sample Application")]
[assembly: AssemblyCopyright("Copyright © 2019 LOGO Group")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("846387e4-5671-4b4e-8850-99bf7e262ab6")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// SecurityId
[assembly: NAFSecurityId("c1a20a7f-a4a4-4fc4-b5d3-40a27b101dcb", "Services")]
[assembly: NResourceEnumTypes(typeof(LogoPaasSampleApp.Resource.ResourceEnum))]
