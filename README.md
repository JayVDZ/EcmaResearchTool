# EcmaResearchTool
Tool for exploring reflection opportunities with ECMA

# Depedencies
You need a folder that contains the management agent binary and all required depedencies, i.e. 
- Microsoft.IAM.Connector.GenericSql.dll
- Microsoft.IdentityManagement.ManagedLogger.dll
- Microsoft.MetadirectoryServicesEx.dll
- System.Configuration.ConfigurationManager.dll
- System.Data.Odbc.dll

# Usage
Intended to be run via debug in Visual Studio so you can prod and poke the MA.
Edit the debug profile, adding a path to the folder with all the Management Agent depedenices in to the Command line arguments, i.e.
`"C:\Management Agents\Generic PowerShell"`

# Notes
- A decompiller is super useful when researching Management Agents, i.e. Jetbrains' DotPeek.
- At a minimum you need the management agent dll and Microsoft.MetadirectoryServicesEx.dll as dependencies, though management agents will almost certainly rely on many more dlls. Often the only way to find out what's needed is to run the tool and wait for exceptions telling you you're missing a dependency
- Some dependencies come from the framework, some from NuGet packages that you have to download and extract the dll from via a tool like DotPeek.
- There seems to be a very tight depedency on the MIM Sync Engine. Progress is slow.
