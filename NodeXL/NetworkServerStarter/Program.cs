
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
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    static void
    Main
    (
        String[] args
    )
    {
        try
        {
            Int32 iNetworkServerExitCode = RunProgram(args);

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
    /// <param name="aArgs">
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
        String[] aArgs
    )
    {
        Debug.Assert(aArgs != null);

        // Run the NetworkServer executable in another process while
        // redirecting that process's stardard output and standard error to
        // this program's console.
        //
        // See the documentation for ProcessStartInfo.RedirectStandardOutput
        // for information on how the use of BeginOutputReadLine() is used to
        // avoid deadlock.

        Process oProcess = CreateNetworkServerProcess(aArgs);

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
    /// <param name="aArgs">
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
        String[] aArgs
    )
    {
        Debug.Assert(aArgs != null);

        ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();

        oProcessStartInfo.FileName =
            NodeXLClickOncePathUtil.GetPathOfFileInNodeXLFolder(
                NetworkServerFileName);

        oProcessStartInfo.Arguments = String.Join(" ", aArgs);

        oProcessStartInfo.UseShellExecute = false;
        oProcessStartInfo.RedirectStandardOutput = true;
        oProcessStartInfo.RedirectStandardError = true;
        oProcessStartInfo.CreateNoWindow = true;

        Process oProcess = new Process();

        oProcess.StartInfo = oProcessStartInfo;

        return (oProcess);
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
