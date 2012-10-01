
// Define SHOW_DIAGNOSTICS_IN_MESSAGE_BOX to show some diagnostic information
// in message boxes.  If this is defined, you must add a reference to
// System.Windows.Forms to this project.

// #define SHOW_DIAGNOSTICS_IN_MESSAGE_BOX

// -----------------------------------------------------------------------
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// -----------------------------------------------------------------------

/*
This is a modified version of a sample file from "Deploying a Visual Studio
Tools for the Office System 3.0 Solution for the 2007 Microsoft Office System
Using Windows Installer (Part 2 of 2) Windows Installers," at
http://msdn.microsoft.com/en-us/library/cc616991.aspx on 3/6/2010.

Modifications are marked with "NodeXLModification".
*/

using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Security.Permissions;
using System.Security;
using Microsoft.Win32;
using System.Runtime.InteropServices;  // NodeXLModification
using System.Text;  // NodeXLModification
using System.Diagnostics;
using System.Collections;  // NodeXLModification

namespace ClickOnceCustomActions
{
    [RunInstaller(true)]

    public class ClickOnceInstaller
        : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            try
            {
                SecurityPermission permission =
                    new SecurityPermission(PermissionState.Unrestricted);
                permission.Demand();
            }
            catch (SecurityException)
            {
                throw new InstallException(
                    "You have insufficient privileges to " +
                    "install the add-in into the ClickOnce cache. " + 
                    "Please contact your system administrator.");
            }
            string deploymentLocation = Context.Parameters["deploymentLocation"];
            if (String.IsNullOrEmpty(deploymentLocation))
            {
                throw new InstallException("Deployment location not configured. Setup unable to continue");
            }

            #if true  // NodeXLModification

            try
            {
                UninstallVersionWithOldName(deploymentLocation);
            }
            catch (Exception oException)
            {
                #if SHOW_DIAGNOSTICS_IN_MESSAGE_BOX
                System.Windows.Forms.MessageBox.Show("Exception: "
                    + oException.Message);

                System.Windows.Forms.MessageBox.Show("Exception: "
                    + oException.StackTrace);
                #endif

                throw (oException);
            }

            #endif

            string arguments = String.Format(
                "/S /I \"{0}\"", deploymentLocation);

            #if SHOW_DIAGNOSTICS_IN_MESSAGE_BOX
            System.Windows.Forms.MessageBox.Show("Installing " + arguments);
            #endif

            int exitCode = ExecuteVSTOInstaller(arguments);
            if (exitCode != 0)
            {
                string message = null;
                switch (exitCode)
                {
                    case -300:
                        message = String.Format(
                            "The Visual Studio Tools for Office solution was signed by an untrusted publisher and as such cannot be installed automatically. Please use your browser to navigate to {0} in order to install the solution manually. You will be prompted if the solution is trusted for execution.",
                            deploymentLocation);
                        break;
                    default:
                        message = String.Format(
                            "The installation of the ClickOnce solution failed with exit code {0}",
                            exitCode);
                        break;
                }
                throw new InstallException(message);
            }
            stateSaver.Add("deploymentLocation", deploymentLocation);
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            string deploymentLocation = (string)savedState["deploymentLocation"];
            if (deploymentLocation != null)
            {
                string arguments = String.Format(
                    "/S /U \"{0}\"", deploymentLocation);

                #if SHOW_DIAGNOSTICS_IN_MESSAGE_BOX
                System.Windows.Forms.MessageBox.Show("Uninstalling "
                    + arguments);
                #endif

                ExecuteVSTOInstaller(arguments);
            }

            // This is in case the user wants to install an earlier version.

            RemoveSolutionMetadataSubKey(
                "Smrf.NodeXL.ExcelTemplate.vsto");

            base.Uninstall(savedState);
        }

        int ExecuteVSTOInstaller(string arguments)
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            string subPath = @"Microsoft Shared\VSTO\9.0\VSTOInstaller.exe";
            string vstoInstallerPath = Path.Combine(basePath, subPath);
            if (File.Exists(vstoInstallerPath) == false)
            {
                throw new InstallException(
                    "The Visual Studio Tools for Office installer was not found.");
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(vstoInstallerPath);
            startInfo.Arguments = arguments;

            #if true  // NodeXLModification

            // These are required to pass elevated privileges to
            // VSTOInstaller.exe.

            startInfo.Verb = "runas";
            startInfo.UseShellExecute = false;

            #endif

            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode;
        }

        protected void
        UninstallVersionWithOldName(string deploymentLocation)
        {
            // The product used to be called "Microsoft NodeXL Excel Template"
            // and it was from the company "Microsoft Research."  As of October
            // 2011, the product is called "NodeXL Excel Template" and it is
            // from the company "Social Media Research Foundation."
            //
            // If the user previously installed the product when it had its old
            // name, the old version needs to be fully removed from the
            // machine.

            UninstallVstoVersionWithOldName(deploymentLocation);

            RemoveSolutionMetadataSubKey(
                "Microsoft.NodeXL.ExcelTemplate.vsto");

            RemoveOldStartMenuItem();
        }

        protected void
        UninstallVstoVersionWithOldName(string deploymentLocation)
        {
            // Before this method was added, when a user attempted to upgrade
            // from a version that used the previous name to a version that
            // used the new name, VSTOInstaller.exe would raise a "-400" error
            // with the following details:
            //
            //   "The customization cannot be installed because another version
            //   is currently installed and cannot be upgraded from this
            //   location."
            //
            // This was occurring because during an upgrade, the Windows
            // Installer does not call this custom action's Uninstall() method.
            // (Uninstall() gets called only during an actual uninstall.)
            // Therefore, the previous ClickOnce application was still
            // installed when Install() attempted to install a newer version,
            // and VSTOInstaller.exe balked.
            //
            // This method uninstalls the version that uses the previous name,
            // if such a version exists.

            // Sample deploymentLocation string:
            //
            //   "C:\Program Files (x86)\
            //   Social Media Research Foundation\
            //   NodeXL Excel Template\
            //   Smrf.NodeXL.ExcelTemplate.vsto"
            //
            // Replace parts of this string to get this:
            //
            //   "C:\Program Files (x86)\
            //   Microsoft Research\
            //   Microsoft NodeXL Excel Template\
            //   Microsoft.NodeXL.ExcelTemplate.vsto"

            String sOldDeploymentLocation = deploymentLocation

                .Replace(
                "Social Media Research Foundation",
                "Microsoft Research"
                )
                
                .Replace(
                "NodeXL Excel Template",
                "Microsoft NodeXL Excel Template"
                )
                
                .Replace(
                "Smrf.NodeXL.ExcelTemplate.vsto",
                "Microsoft.NodeXL.ExcelTemplate.vsto"
                )
                ;

            if ( File.Exists(sOldDeploymentLocation) )
            {
                string arguments = String.Format(
                    "/S /U \"{0}\"", sOldDeploymentLocation);

                #if SHOW_DIAGNOSTICS_IN_MESSAGE_BOX
                System.Windows.Forms.MessageBox.Show("Uninstalling old "
                    + arguments);
                #endif

                ExecuteVSTOInstaller(arguments);
            }
        }

        protected void
        RemoveSolutionMetadataSubKey(String vstoFileName)
        {
            // Before this method was added, when a Windows 7 user attempted to
            // upgrade from a version that used the previous name to a version
            // that used the new name, the Setup.exe would work but the user
            // would get this runtime error the first time he opened the
            // upgraded version:
            //
            //   "Could not find file 'C:\Program Files (x86)\Microsoft
            //   Research\Microsoft NodeXL Excel Template\
            //   Microsoft.NodeXL.ExcelTemplate.vsto'."
            //
            // Interestingly, the error message would not appear and the
            // template worked the SECOND time he opened the upgraded version.
            // Also, this did not occur on a Vista machine that was tested.
            //
            // This was occurring because VSTO sometimes caches some metadata
            // for the template in the registry, and the VSTO loader was
            // looking for the VSTO manifest in the cached, old location.
            //
            // The following code fixes this by deleting the registry key in
            // which the cached metadata is stored.  The key looks something
            // like this:
            //
            //   HKEY_CURRENT_USER\Software\Microsoft\VSTO\SolutionMetadata\
            //   {AB3EFACF-64A0-4616-9F1E-F6258AEBF0B5}
            //
            // and it has a locationUri value of something like this:
            //
            //   file:///C:/Program Files (x86)/Microsoft Research/
            //   Microsoft NodeXL Excel Template/
            //   Microsoft.NodeXL.ExcelTemplate.vsto

            RegistryKey oSolutionMetadataKey = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\VSTO\SolutionMetadata", true);

            if (oSolutionMetadataKey == null)
            {
                return;
            }

            using (oSolutionMetadataKey)
            {
                foreach ( String sSubKeyName in
                    oSolutionMetadataKey.GetSubKeyNames() )
                {
                    using ( RegistryKey oSubKey =
                        oSolutionMetadataKey.OpenSubKey(sSubKeyName) )
                    {
                        const String LocationUriName = "locationUri";

                        Object oLocationUriValue =
                            oSubKey.GetValue(LocationUriName);

                        if (
                            oLocationUriValue != null
                            &&
                            oSubKey.GetValueKind(LocationUriName) ==
                                RegistryValueKind.String
                            &&
                            ( (String)oLocationUriValue ).ToLower().IndexOf(
                                vstoFileName.ToLower() ) >= 0
                            )
                        {
                            #if SHOW_DIAGNOSTICS_IN_MESSAGE_BOX
                            System.Windows.Forms.MessageBox.Show(
                                "Removing registry key with name "
                                + vstoFileName);
                            #endif

                            oSolutionMetadataKey.DeleteSubKeyTree(sSubKeyName);
                            return;
                        }
                    }

                }
            }
        }

        protected void
        RemoveOldStartMenuItem()
        {
            // The old Start Menu folder was called "Microsoft NodeXL."  The
            // new folder is called "NodeXL."  The new setup program does not
            // remove the old folder.

            const String OldSubfolder = "Microsoft NodeXL";

            try
            {
                // This is the per-user folder, which is what the Setup program
                // installs when the user selects Just for Me.


                Directory.Delete(Path.Combine(

                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Programs),

                    OldSubfolder), true);
            }
            catch (Exception)
            {
                // This can occur for several reasons, including if the folder
                // doesn't exist.  The error is relatively benign, so ignore
                // it.
            }

            try
            {
                // This is the all-user folder, which is what the Setup program
                // installs when the user selects Everyone.

                StringBuilder oFolder = new StringBuilder(260);

                SHGetSpecialFolderPath(IntPtr.Zero, oFolder,
                    CSIDL_COMMON_PROGRAMS, false);

                Directory.Delete(Path.Combine(oFolder.ToString(), OldSubfolder),
                    true);

            }
            catch (Exception)
            {
            }
        }


        #if true  // NodeXLModification

        [DllImport("shell32.dll")]

        static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner,
            [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

        const int CSIDL_COMMON_PROGRAMS = 23;

        #endif
    }
}
