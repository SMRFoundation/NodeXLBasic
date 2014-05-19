
using System;
using System.IO;
using System.Diagnostics;

namespace Smrf.NodeXL.ClickOnceLib
{
//*****************************************************************************
//  Class: NodeXLClickOncePathUtil
//
/// <summary>
/// Utility methods for working with the path to the ClickOnce folder where the
/// NodeXL Excel Template gets published.
/// </summary>
//*****************************************************************************

static class NodeXLClickOncePathUtil
{
    //*************************************************************************
    //  Method: GetPathOfFileInNodeXLFolder()
    //
    /// <summary>
    /// Gets the full path of a specified file in NodeXL's ClickOnce folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the specified file.  The file is guaranteed to exist.
    /// </returns>
    ///
    /// <exception cref="NodeXLClickOncePathException">
    /// Thrown if the full path can't be found or the file doesn't exist.
    /// </exception>
    //*************************************************************************

    public static String
    GetPathOfFileInNodeXLFolder
    (
        String fileName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(fileName) );

        // Sample: SomeClickOnceFolderPath\DeployedTemplate\..

        String filePath = GetNodeXLFolderPath();

        // Sample: SomeClickOnceFolderPath\DeployedTemplate\..\FileName

        filePath = Path.Combine(filePath, fileName);

        if ( !File.Exists(filePath) )
        {
            throw new NodeXLClickOncePathException( String.Format(

                "There doesn't seem to be a NodeXL file named \"{0}\" on"
                + " this computer.  Is the latest version of NodeXL installed"
                + " on this computer?"
                ,
                fileName
                ) );
        }

        return (filePath);
    }

    //*************************************************************************
    //  Method: GetNodeXLTemplatePath()
    //
    /// <summary>
    /// Gets the full path to the NodeXL template file in NodeXL's ClickOnce
    /// folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the NodeXL template file.  The file is guaranteed to
    /// exist.  Sample:
    /// "SomeClickOnceFolderPath\DeployedTemplate\NodeXLGraph.xltx".
    /// </returns>
    ///
    /// <exception cref="NodeXLClickOncePathException">
    /// Thrown if the full path can't be found or the template file doesn't
    /// exist.
    /// </exception>
    //*************************************************************************

    public static String
    GetNodeXLTemplatePath()
    {
        String nodeXLTemplateShortcutFilePath =
            GetNodeXLTemplateShortcutFilePath();

        if ( !File.Exists(nodeXLTemplateShortcutFilePath) )
        {
            throw new NodeXLClickOncePathException( String.Format(

                "There doesn't seem to be a shortcut named \"{0}\" on the"
                + " Windows Start menu (or Start screen).  Is NodeXL version"
                + " 1.0.1.300 or later installed on this computer?"
                ,
                NodeXLTemplateShortcutName
                ) );
        }

        String nodeXLTemplatePath =
            GetShortcutTargetPath(nodeXLTemplateShortcutFilePath);

        if ( !File.Exists(nodeXLTemplatePath) )
        {
            throw new NodeXLClickOncePathException( String.Format(

                "The NodeXL template file couldn't be found.  The NodeXL"
                + " Start menu shortcut indicates that the template file"
                + " should be at \"{0}\", but there is no such file."
                ,
                nodeXLTemplatePath
                ) );
        }

        return (nodeXLTemplatePath);
    }

    //*************************************************************************
    //  Method: GetNodeXLFolderPath()
    //
    /// <summary>
    /// Gets the full path to NodeXL's ClickOnce folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the ClickOnce folder.  The folder is guaranteed to
    /// exist.
    /// </returns>
    //*************************************************************************

    private static String
    GetNodeXLFolderPath()
    {
        // Sample: SomeClickOnceFolderPath\DeployedTemplate\NodeXLGraph.xltx

        String nodeXLFolderPath = GetNodeXLTemplatePath();

        // Sample: SomeClickOnceFolderPath\DeployedTemplate

        nodeXLFolderPath = Path.GetDirectoryName(nodeXLFolderPath);

        // Sample: SomeClickOnceFolderPath\DeployedTemplate\..

        nodeXLFolderPath = Path.Combine(nodeXLFolderPath, "..");

        return (nodeXLFolderPath);
    }

    //*************************************************************************
    //  Method: GetNodeXLTemplateShortcutFilePath()
    //
    /// <summary>
    /// Gets the full path to the NodeXL template shortcut file on the Start
    /// menu.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the shortcut file.
    /// </returns>
    //*************************************************************************

    private static String
    GetNodeXLTemplateShortcutFilePath()
    {
        return ( Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
            NodeXLTemplateShortcutFileName
            ) );
    }

    //*************************************************************************
    //  Method: GetShortcutTargetPath()
    //
    /// <summary>
    /// Gets the full path to the target of a shortcut.
    /// </summary>
    ///
    /// <param name="shortcutFilePath">
    /// The full path to the shortcut file.
    /// </param>
    ///
    /// <returns>
    /// The full path to the target of the shortcut.
    /// </returns>
    //*************************************************************************

    private static String
    GetShortcutTargetPath
    (
        String shortcutFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(shortcutFilePath) );
        Debug.Assert( File.Exists(shortcutFilePath) );

        // This code came from here:
        //
        // http://www.codeproject.com/Tips/44329/
        // Edit-shortcuts-lnk-properties-with-C

        IWshRuntimeLibrary.WshShell wshShell =
            new IWshRuntimeLibrary.WshShell();

        // This opens the existing shortcut.  It doesn't create anything.

        IWshRuntimeLibrary.IWshShortcut shortcut =
            (IWshRuntimeLibrary.IWshShortcut)
            wshShell.CreateShortcut(shortcutFilePath);

        return (shortcut.TargetPath);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Name of the NodeXL template shortcut, without a path or extension.

    private const String NodeXLTemplateShortcutName =
        "NodeXL Excel Template";

    /// Name of the NodeXL template shortcut file, without a path.

    private const String NodeXLTemplateShortcutFileName =
        NodeXLTemplateShortcutName + ".lnk";
}
}
