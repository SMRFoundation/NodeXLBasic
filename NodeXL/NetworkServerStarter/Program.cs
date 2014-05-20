
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.ClickOnceLib;

namespace Smrf.NodeXL.NetworkServerStarter
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// Runs the NetworkServer executable contained in NodeXL's ClickOnce folder.
/// </summary>
///
/// <remarks>
/// See ..\ReadMe.txt for an explanation of why this program is needed.
/// </remarks>
//*****************************************************************************

class Program
{
    //*************************************************************************
    //  Method: Main()
    //
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    ///
    /// <param name="commandLineArguments">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    static void
    Main
    (
        String[] commandLineArguments
    )
    {
        try
        {
            Int32 iNetworkServerExitCode = RunProgram(commandLineArguments);

            Environment.Exit(iNetworkServerExitCode);
        }
        catch (NodeXLClickOncePathException oNodeXLClickOncePathException)
        {
            Exit(ExitCode.CannotFindNetworkServer,

                String.Format(
                    "The {0} program couldn't be started.  {1}"
                    ,
                    NetworkServerFileName,
                    oNodeXLClickOncePathException.Message
                    )
                );
        }
        catch (Exception oException)
        {
            Exit(ExitCode.UnexpectedException,

                "An unexpected problem occurred.  Details:"
                + "\r\n\r\n"
                + ExceptionUtil.GetMessageTrace(oException)
                );
        }
    }

    //*************************************************************************
    //  Method: RunProgram()
    //
    /// <summary>
    /// Runs this program.
    /// </summary>
    ///
    /// <param name="asCommandLineArguments">
    /// Command line arguments.
    /// </param>
    ///
    /// <returns>
    /// The exit code returned by the NetworkServer executable.
    /// </returns>
    //*************************************************************************

    private static Int32
    RunProgram
    (
        String[] asCommandLineArguments
    )
    {
        Debug.Assert(asCommandLineArguments != null);

        // Run the NetworkServer executable in another process while
        // redirecting that process's stardard output and standard error to
        // this program's console.
        //
        // See the documentation for ProcessStartInfo.RedirectStandardOutput
        // for information on how the use of BeginOutputReadLine() is used to
        // avoid deadlock.

        Process oProcess = CreateNetworkServerProcess(asCommandLineArguments);

        oProcess.OutputDataReceived +=
            (Object sendingProcess, DataReceivedEventArgs e) =>
            {
                String sOutputData = e.Data;

                if ( !String.IsNullOrEmpty(sOutputData) )
                {
                    Console.WriteLine(sOutputData);
                }
            };

        oProcess.Start();
        oProcess.BeginOutputReadLine();

        String sStandardError = oProcess.StandardError.ReadToEnd();
        oProcess.WaitForExit();

        Int32 iNetworkServerExitCode = oProcess.ExitCode;

        oProcess.Close();

        if ( !String.IsNullOrEmpty(sStandardError) )
        {
            Console.Error.WriteLine(sStandardError);
        }

        return (iNetworkServerExitCode);
    }

    //*************************************************************************
    //  Method: CreateNetworkServerProcess()
    //
    /// <summary>
    /// Creates a Process object for calling the NetworkServer executable.
    /// </summary>
    ///
    /// <param name="asCommandLineArguments">
    /// Command line arguments.
    /// </param>
    ///
    /// <returns>
    /// A new Process object that will call the NetworkServer executable.
    /// </returns>
    //*************************************************************************

    private static Process
    CreateNetworkServerProcess
    (
        String[] asCommandLineArguments
    )
    {
        Debug.Assert(asCommandLineArguments != null);

        ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();

        oProcessStartInfo.FileName =
            NodeXLClickOncePathUtil.GetPathOfFileInNodeXLFolder(
                NetworkServerFileName);

        oProcessStartInfo.Arguments =
            FormatCommandLineArguments(asCommandLineArguments);

        oProcessStartInfo.UseShellExecute = false;
        oProcessStartInfo.RedirectStandardOutput = true;
        oProcessStartInfo.RedirectStandardError = true;
        oProcessStartInfo.CreateNoWindow = true;

        Process oProcess = new Process();

        oProcess.StartInfo = oProcessStartInfo;

        return (oProcess);
    }

    //*************************************************************************
    //  Method: FormatCommandLineArguments()
    //
    /// <summary>
    /// Formats the arguments to pass to the Process object.
    /// </summary>
    ///
    /// <param name="asCommandLineArguments">
    /// Command line arguments.
    /// </param>
    ///
    /// <returns>
    /// The formatted arguments.
    /// </returns>
    //*************************************************************************

    private static String
    FormatCommandLineArguments
    (
        String[] asCommandLineArguments
    )
    {
        Debug.Assert(asCommandLineArguments != null);

        // Surround each argument with quotes and separate them with spaces.

        StringBuilder oStringBuilder = new StringBuilder();

        foreach (String sCommandLineArgument in asCommandLineArguments)
        {
            if (oStringBuilder.Length > 0)
            {
                oStringBuilder.Append(' ');
            }

            oStringBuilder.AppendFormat(

                "\"{0}\""
                ,
                sCommandLineArgument
                );
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: Exit()
    //
    /// <summary>
    /// Exits the program with a specified exit code.
    /// </summary>
    ///
    /// <param name="eExitCode">
    /// The program exit code to use.
    /// </param>
    ///
    /// <param name="sErrorMessage">
    /// The error message to send to the standard error stream, or null for no
    /// message.
    /// </param>
    //*************************************************************************

    private static void
    Exit
    (
        ExitCode eExitCode,
        String sErrorMessage
    )
    {
        if ( !String.IsNullOrEmpty(sErrorMessage) )
        {
            Console.Error.WriteLine(sErrorMessage);
        }

        Environment.Exit( (Int32)eExitCode );
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Name of the NetworkServer executable file name, without a path.

    private const String NetworkServerFileName = "NodeXLNetworkServer.exe";
}

}
