﻿

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Office.Tools.Excel;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("NodeXL Excel Template")]
[assembly: AssemblyDescription("Excel template that displays a NodeXL graph")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft Research")]
[assembly: AssemblyProduct("Microsoft NodeXL")]
[assembly: AssemblyCopyright("Copyright ©  2008 Microsoft Corporation")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5a2bac1d-e925-4995-8957-ee76937b6c80")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("1.0.1.66")]
[assembly: AssemblyFileVersion("1.0.1.66")]

// 
// The ExcelLocale1033 attribute controls the locale that is passed to the Excel
// object model. Setting ExcelLocale1033 to true causes the Excel object model to 
// act the same in all locales, which matches the behavior of Visual Basic for 
// Applications. Setting ExcelLocale1033 to false causes the Excel object model to
// act differently when users have different locale settings, which matches the 
// behavior of Visual Studio Tools for Office, Version 2003. This can cause unexpected 
// results in locale-sensitive information such as formula names and date formats.
// 
[assembly: ExcelLocale1033(true)]

[assembly: SecurityTransparent()]
