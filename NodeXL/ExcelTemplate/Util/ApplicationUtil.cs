
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Smrf.NodeXL.ApplicationUtil;
using Smrf.NodeXL.Common;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ApplicationUtil
//
/// <summary>
/// Contains utility methods dealing with the application as a whole.
/// </summary>
//*****************************************************************************

public static class ApplicationUtil
{
    //*************************************************************************
    //  Method: OpenHelp()
    //
    /// <summary>
    /// Opens the application's help file.
    /// </summary>
    //*************************************************************************

    public static void
    OpenHelp()
    {
        if (m_oHelpProcess != null)
        {
            // The help window is already open.  Activate it.

            Win32Functions.SetForegroundWindow(
                m_oHelpProcess.MainWindowHandle);

            return;
        }

        String sHelpFileFolder;

        if (RunningInDevelopmentEnvironment)
        {
            sHelpFileFolder = Path.Combine(
                GetDevelopmentApplicationFolder(),
                @"..\..\..\Documents\Help\ExcelTemplate"
            );
        }
        else
        {
            sHelpFileFolder = GetApplicationFolder();
        }

        m_oHelpProcess = Process.Start( Path.Combine(
            sHelpFileFolder, HelpFileName) );

        m_oHelpProcess.EnableRaisingEvents = true;

        m_oHelpProcess.Exited += (Object, EventArgs) =>
        {
            m_oHelpProcess.Dispose();
            m_oHelpProcess = null;
        };
    }

    //*************************************************************************
    //  Method: CreateNodeXLWorkbook()
    //
    /// <summary>
    /// Creates a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="excelApplication">
    /// The Excel application.
    /// </param>
    ///
    /// <returns>
    /// The new NodeXL workbook.
    /// </returns>
    ///
    /// <exception cref="IOException">
    /// Occurs when the NodeXL template can't be found.
    /// </exception>
    //*************************************************************************

    public static Workbook
    CreateNodeXLWorkbook
    (
        Microsoft.Office.Interop.Excel.Application excelApplication
    )
    {
        String sNodeXLTemplatePath;

        if ( !TryGetTemplatePath(out sNodeXLTemplatePath) )
        {
            throw new IOException( GetMissingTemplateMessage() );
        }

        return ( excelApplication.Workbooks.Add(sNodeXLTemplatePath) );
    }

    //*************************************************************************
    //  Method: OpenSampleNodeXLWorkbook()
    //
    /// <summary>
    /// Opens a sample NodeXL workbook.
    /// </summary>
    //*************************************************************************

    public static void
    OpenSampleNodeXLWorkbook()
    {
        // To create the sample workbook, an empty NodeXL workbook is created
        // from the NodeXL template, and then a GraphML file containing the
        // sample data is imported into it.  It would be simpler to just
        // distribute a complete sample workbook with NodeXL, but that workbook
        // would have to be updated every time the NodeXL template changes.
        // This way, the latest template is always used.

        String sSampleNodeXLWorkbookSubfolderPath = Path.Combine(
            GetApplicationFolder(), SampleNodeXLWorkbookSubfolder);

        XmlDocument oGraphMLDocument = new XmlDocument();

        try
        {
            oGraphMLDocument.Load( Path.Combine(
                sSampleNodeXLWorkbookSubfolderPath,
                SampleNodeXLWorkbookAsGraphMLFileName) );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        // The sample workbook data contains placeholders for the path to the
        // image files used in the workbook.  Replace those placeholders with a
        // real image path.

        String sImagePath = Path.Combine(sSampleNodeXLWorkbookSubfolderPath,
            SampleNodeXLWorkbookImageSubfolder);

        oGraphMLDocument.LoadXml( oGraphMLDocument.OuterXml.Replace(
            SampleNodeXLWorkbookImagePlaceholder, sImagePath) );

        try
        {
            GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                oGraphMLDocument, null, null, false);
        }
        catch (ConvertGraphMLToNodeXLWorkbookException
            oConvertGraphMLToNodeXLWorkbookException)
        {
            FormUtil.ShowWarning(
                oConvertGraphMLToNodeXLWorkbookException.Message);
        }
    }

    //*************************************************************************
    //  Method: CheckForUpdate()
    //
    /// <summary>
    /// Checks for a newer version of the application.
    /// </summary>
    ///
    /// <remarks>
    /// All interaction with the user is handled by this method.
    /// </remarks>
    //*************************************************************************

    public static void
    CheckForUpdate()
    {
        // Get the version of the installed version of the application.

        FileVersionInfo oCurrentFileVersionInfo =
            AssemblyUtil2.GetFileVersionInfo();

        // Get the version information for the latest version of the
        // application.

        Int32 iLatestVersionFileMajorPart = 0;
        Int32 iLatestVersionFileMinorPart = 0;
        Int32 iLatestVersionFileBuildPart = 0;
        Int32 iLatestVersionFilePrivatePart = 0;

        try
        {
            GetLatestVersionInfo(out iLatestVersionFileMajorPart,
                out iLatestVersionFileMinorPart,
                out iLatestVersionFileBuildPart,
                out iLatestVersionFilePrivatePart
                );
        }
        catch (WebException)
        {
            FormUtil.ShowWarning(
                "The Web site from which updates are obtained could not be"
                + " reached.  Either an Internet connection isn't available,"
                + " or the Web site isn't available."
                );

            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);

            return;
        }

        if (
           iLatestVersionFileMajorPart > oCurrentFileVersionInfo.FileMajorPart
           ||
           iLatestVersionFileMinorPart > oCurrentFileVersionInfo.FileMinorPart
           ||
           iLatestVersionFileBuildPart > oCurrentFileVersionInfo.FileBuildPart
           ||
           iLatestVersionFilePrivatePart >
               oCurrentFileVersionInfo.FilePrivatePart
           )
        {
            String sMessage = String.Format(

                "A new version of {0} is available.  Do you want to open the"
                + " Web page from which the new version can be downloaded?"
                ,
                FormUtil.ApplicationName
                );

            if (MessageBox.Show(sMessage, FormUtil.ApplicationName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                    DialogResult.Yes)
            {
                Process.Start(ProjectInformation.DownloadPageUrl);
            }
        }
        else
        {
            FormUtil.ShowInformation( String.Format(

                "You have the latest version of {0}." 
                ,
                FormUtil.ApplicationName
                ) );
        }
    }

    //*************************************************************************
    //  Method: GetLatestVersionInfo()
    //
    /// <summary>
    /// Gets the version information for the latest version of the application.
    /// </summary>
    ///
    /// <param name="latestVersionFileMajorPart">
    /// Where the major part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFileMinorPart">
    /// Where the minor part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFileBuildPart">
    /// Where the build part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFilePrivatePart">
    /// Where the private part of the version number gets stored.
    /// </param>
    ///
    /// <remarks>
    /// A WebException is thrown if the version information can't be obtained.
    /// </remarks>
    //*************************************************************************

    public static void
    GetLatestVersionInfo
    (
        out Int32 latestVersionFileMajorPart,
        out Int32 latestVersionFileMinorPart,
        out Int32 latestVersionFileBuildPart,
        out Int32 latestVersionFilePrivatePart
    )
    {
        latestVersionFileMajorPart = Int32.MinValue;
        latestVersionFileMinorPart = Int32.MinValue;
        latestVersionFileBuildPart = Int32.MinValue;
        latestVersionFilePrivatePart = Int32.MinValue;

        // The version number of the latest release is embedded in the home
        // page.  Attempt to get the home page contents.

        WebRequest oWebRequest =
            WebRequest.Create(ProjectInformation.HomePageUrl);

        HttpWebResponse oHttpWebResponse = null;
        Stream oStream = null;
        StreamReader oStreamReader = null;
        String sResponse = null;

        try
        {
            oHttpWebResponse = (HttpWebResponse)oWebRequest.GetResponse();
            oStream = oHttpWebResponse.GetResponseStream();
            oStreamReader = new StreamReader(oStream);
            sResponse = oStreamReader.ReadToEnd();
        }
        finally
        {
            if (oStreamReader != null)
            {
                oStreamReader.Close();
            }

            if (oStream != null)
            {
                oStream .Close();
            }

            if (oHttpWebResponse != null)
            {
                oHttpWebResponse.Close();
            }
        }

        // Use a regular expression to parse the response.  Look for the
        // version in this format, for example:
        //
        // NodeXL Excel Template, version 1.0.1.56

        const String Pattern =
            "NodeXL Excel Template, version "
            + "(?<FileMajorPart>\\d+)"
            + "\\."
            + "(?<FileMinorPart>\\d+)"
            + "\\."
            + "(?<FileBuildPart>\\d+)"
            + "\\."
            + "(?<FilePrivatePart>\\d+)"
            ;

        Regex oRegex = new Regex(Pattern);
        Match oMatch = oRegex.Match(sResponse);

        if (!oMatch.Success)
        {
            throw new FormatException(
                "The home page was found but it doesn't appear to contain the"
                + " latest version number."
                );
        }

        latestVersionFileMajorPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileMajorPart"].Value);

        latestVersionFileMinorPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileMinorPart"].Value);

        latestVersionFileBuildPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileBuildPart"].Value);

        latestVersionFilePrivatePart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FilePrivatePart"].Value);
    }

    //*************************************************************************
    //  Method: GetPlugInFolder()
    //
    /// <summary>
    /// Gets the full path to folder where plug-in assemblies are stored.
    /// </summary>
    ///
    /// <returns>
    /// The full path to folder where plug-in assemblies are stored.
    /// </returns>
    //*************************************************************************

    public static String
    GetPlugInFolder()
    {
        return ( Path.Combine(GetApplicationFolder(), PlugInSubfolder) );
    }

    //*************************************************************************
    //  Method: GetApplicationFolder()
    //
    /// <summary>
    /// Gets the full path to the application's folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the application's folder.  Sample:
    /// "C:\Program Files\...\NodeXL Excel Template".
    /// </returns>
    //*************************************************************************

    public static String
    GetApplicationFolder()
    {
        if (RunningInDevelopmentEnvironment)
        {
            return ( GetDevelopmentApplicationFolder() );
        }

        // For versions 1.0.1.113 and earlier, the setup program installed
        // NodeXL into a subfolder of the standard Program Files folder and the
        // program was run from there.  The application folder could then be
        // obtained as follows:
        //
        //   String sApplicationFolder = Path.GetDirectoryName(
        //     Assembly.GetExecutingAssembly().CodeBase);
        //
        // Versions since then have continued to install NodeXL into that
        // subfolder, but ClickOnce is now called at the end of the setup to
        // install the application into the ClickOnce cache, and that is where
        // the application actually runs from.  That means that the location of
        // the executing assembly is now in some obscure location, far from the
        // Program Files folder, and the above code no longer works.
        //
        // For the following comments, note that there is only one NodeXL setup
        // program, and it is always 32-bit.  This is where the setup program
        // installs NodeXL, assuming an English version of Windows:
        //
        //     32-bit Windows: "C:\Program Files"
        //     64-bit Windows: "C:\Program Files (x86)"

        String sProgramFilesFolder;

        if (IntPtr.Size == 4)
        {
            // NodeXL is running within 32-bit Excel.
            //
            // This is what Environment.SpecialFolder.ProgramFiles returns:
            //
            //     32-bit Windows: "C:\Program Files"
            //     64-bit Windows: "C:\Program Files (x86)"
            //
            // Because this matches the folder where NodeXL is installed, no
            // special action is required.

            sProgramFilesFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles);
        }
        else
        {
            // NodeXL is running within 64-bit Excel.
            //
            // This is what Environment.SpecialFolder.ProgramFiles returns:
            //
            //     64-bit Windows: "C:\Program Files"
            //
            // This won't work, because NodeXL is installed at
            // "C:\Program Files (x86)".  Instead, use one of Windows'
            // environment variables.

            sProgramFilesFolder = System.Environment.GetEnvironmentVariable(
                "ProgramFiles(x86)");
        }

        Debug.Assert( !String.IsNullOrEmpty(sProgramFilesFolder) );

        return ( Path.Combine(sProgramFilesFolder, ProgramFilesSubfolder) );
    }

    //*************************************************************************
    //  Method: GetSplashScreenPath()
    //
    /// <summary>
    /// Gets the full path to the HTML file for the application's splash
    /// screen.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the HTML file.  Sample:
    /// "C:\Program Files\...\NodeXL Excel Template\SplashScreen\
    /// SplashScreen.htm".
    /// </returns>
    //*************************************************************************

    public static String
    GetSplashScreenPath()
    {
        String sSplashScreenSubfolderPath = Path.Combine(
            GetApplicationFolder(), SplashScreenSubfolder);

        return ( Path.Combine(sSplashScreenSubfolderPath,
            SplashScreenFileName) );
    }

    //*************************************************************************
    //  Method: OnWorkbookStartup()
    //
    /// <summary>
    /// Performs tasks required when the workbook starts up.
    /// </summary>
    ///
    /// <param name="application">
    /// The Excel application object.
    /// </param>
    //*************************************************************************

    public static void
    OnWorkbookStartup
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        AutoCorrect oAutoCorrect = application.AutoCorrect;

        if (!oAutoCorrect.AutoExpandListRange)
        {
            const String CheckboxPath =
                "Office Button, Excel Options, Proofing, AutoCorrect Options,"
                + " AutoFormat As You Type, Include new rows and columns in"
                + " table"
                ;

            String Message =
                "Excel's \"auto table expansion\" feature is turned off, and"
                + " NodeXL cannot work properly without it.  Do you want"
                + " NodeXL to turn it on for you?"
                + "\n\n"
                + " (You can turn it off later if necessary using "
                + CheckboxPath
                + ".)"
                ;

            if (MessageBox.Show(Message, FormUtil.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1) ==
                DialogResult.Yes)
            {
                oAutoCorrect.AutoExpandListRange = true;
            }
            else
            {
                FormUtil.ShowError(
                    "NodeXL will not work properly.  You should turn on"
                    + " Excel's auto table expansion feature using "
                    + CheckboxPath
                    + "."
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: OnWorkbookShutdown()
    //
    /// <summary>
    /// Performs tasks required when the workbook closes.
    /// </summary>
    //*************************************************************************

    public static void
    OnWorkbookShutdown()
    {
        if (m_oHelpProcess != null)
        {
            m_oHelpProcess.CloseMainWindow();
        }
    }

    //*************************************************************************
    //  Method: GetDevelopmentApplicationFolder()
    //
    /// <summary>
    /// Gets the full path to the application's folder when the application is
    /// running in a development environment.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the application's folder.  Sample:
    /// "E:\NodeXL\ExcelTemplate\bin\Debug"
    /// </returns>
    //*************************************************************************

    private static String
    GetDevelopmentApplicationFolder()
    {
        return ( Path.GetDirectoryName( GetExecutingAssemblyPath() ) );
    }

    //*************************************************************************
    //  Method: TryGetTemplatePath()
    //
    /// <summary>
    /// Attempts to get the full path to the application's template file.
    /// </summary>
    ///
    /// <param name="templatePath">
    /// Where the path to the template file gets stored regardless of the
    /// return value.
    /// </param>
    ///
    /// <remarks>
    /// true if the template file exists.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTemplatePath
    (
        out String templatePath
    )
    {
        String sTemplatesPath;

        if (RunningInDevelopmentEnvironment)
        {
            // Samples, depending on which program is being run:
            //
            //   1. "E:\NodeXL\ExcelTemplate\bin\Debug"
            //
            //   2. "E:\NodeXL\NetworkServer\bin\Debug"

            sTemplatesPath = Path.GetDirectoryName(
                GetExecutingAssemblyPath() );

            // The template in the development environment is under the
            // ExcelTemplate folder.  For case 2, fix the folder.

            sTemplatesPath = sTemplatesPath.Replace("NetworkServer",
                "ExcelTemplate");
        }
        else
        {
            // The setup program puts the template file in the application
            // folder.

            sTemplatesPath = GetApplicationFolder();
        }

        templatePath = Path.Combine(sTemplatesPath, TemplateName);

        return ( File.Exists(templatePath) );
    }

    //*************************************************************************
    //  Method: GetMissingTemplateMessage()
    //
    /// <summary>
    /// Gets a user-friendly message to display when the application's template
    /// file can't be found.
    /// </summary>
    ///
    /// <returns>
    /// A user-friendly message.
    /// </returns>
    //*************************************************************************

    public static String
    GetMissingTemplateMessage()
    {
        String sTemplatePath;

        ApplicationUtil.TryGetTemplatePath(out sTemplatePath);

        return ( String.Format(

            "The {0} Excel template couldn't be found."
            + "\r\n\r\n"
            + "The {0} setup program should have copied the template to"
            + " {1}.  If you moved the template somewhere else, you won't"
            + " be able to use this feature."
            ,
            ApplicationUtil.ApplicationName,
            sTemplatePath
            ) );
    }

    //*************************************************************************
    //  Property: RunningInDevelopmentEnvironment
    //
    /// <summary>
    /// Gets a flag indicating whether the application is running in a
    /// development environment.
    /// </summary>
    ///
    /// <value>
    /// true if the application is running in a development environment, false
    /// if it is running in an installed environment.
    /// </value>
    //*************************************************************************

    private static Boolean
    RunningInDevelopmentEnvironment
    {
        get
        {
            String sExecutingAssemblyPath =
                GetExecutingAssemblyPath().ToLower();

            return (
                sExecutingAssemblyPath.IndexOf(@"bin\debug") >= 0 ||
                sExecutingAssemblyPath.IndexOf(@"bin\release") >= 0
                );
        }
    }

    //*************************************************************************
    //  Method: GetExecutingAssemblyPath()
    //
    /// <summary>
    /// Gets the full path to the executing assembly.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the executing assembly.  Sample:
    /// "...\Some ClickOnce Folder\Smrf.NodeXL.ExcelTemplate.dll".
    /// </returns>
    //*************************************************************************

    private static String
    GetExecutingAssemblyPath()
    {
        // CodeBase returns an URI, such as "file://folder/subfolder/etc".
        // Convert it to a local path.

        Uri oUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);

        return (oUri.LocalPath);
    }


    //*************************************************************************
    //  Private members
    //*************************************************************************

    /// Process for the application's help file, or null if the help file isn't
    /// open.

    private static Process m_oHelpProcess = null;


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Application subfolder within the Program Files folder.  There is no
    /// UI in the setup program to select another folder, so this will always
    /// be the correct subfolder.

    private const String ProgramFilesSubfolder =
        @"Social Media Research Foundation\NodeXL Excel Template";

    /// Subfolder under the application folder where plug-in assemblies are
    /// stored.

    private const String PlugInSubfolder = "PlugIns";

    /// Subfolder under the application folder where the files uses in the
    /// sample NodeXLWorkbook are stored.

    private const String SampleNodeXLWorkbookSubfolder =
        "SampleNodeXLWorkbook";

    /// Subfolder under the application folder where the splash screen is
    /// stored.

    private const String SplashScreenSubfolder =
        "SplashScreen";

    /// File name of the splash screen's HTML file.

    private const String SplashScreenFileName = "SplashScreen.htm";

    /// File name of the application's help file.

    private const String HelpFileName = "NodeXLExcelTemplate.chm";

    /// File name of the GraphML file used to create the sample NodeXLWorkbook.

    private const String SampleNodeXLWorkbookAsGraphMLFileName =
        "SampleNodeXLWorkbookAsGraphML.graphml";

    /// Subfolder under SampleNodeXLWorkbookSubfolder where the images used in
    /// the sample NodeXLWorkbook are stored.

    private const String SampleNodeXLWorkbookImageSubfolder =
        "Images";

    /// Placeholder in the GraphML file for the path to the images used in the
    /// sample NodeXL workbook.

    private const String SampleNodeXLWorkbookImagePlaceholder = "[ImagePath]";


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Application name.

    public const String ApplicationName = "NodeXL";

    /// Name of the application's template.

    public const String TemplateName = "NodeXLGraph.xltx";

    /// Solution ID, as a GUID string.

    public const String SolutionID = "aa51c0f3-62b4-4782-83a8-a15dcdd17698";
}

}
