
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.ClickOnceLib;

namespace Smrf.NodeXL.GraphMLFileProcessorStarter
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// Runs the GraphMLFileProcessor executable contained in NodeXL's ClickOnce
/// folder.
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
            RunProgram(args);

            Console.Write(

                "\r\n"
                +"The {0} program has been started in another window."
                + "\r\n"
                ,
                GraphMLFileProcessorProgramName
                );

            Exit(ExitCode.Success, null);
        }
        catch (NodeXLClickOncePathException oNodeXLClickOncePathException)
        {
            Exit(ExitCode.CannotFindGraphMLFileProcessor,

                String.Format(
                    "The {0} program couldn't be started.  {1}"
                    ,
                    GraphMLFileProcessorFileName,
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
    //*************************************************************************

    private static void
    RunProgram
    (
        String[] aArgs
    )
    {
        Debug.Assert(aArgs != null);

        // Run the GraphMLFileProcessor executable in another process.  Don't
        // wait for it to exit.

        Process oProcess = CreateGraphMLFileProcessorProcess(aArgs);
        oProcess.Start();
    }

    //*************************************************************************
    //  Method: CreateGraphMLFileProcessorProcess()
    //
    /// <summary>
    /// Creates a Process object for calling the GraphMLFileProcessor
    /// executable.
    /// </summary>
    ///
    /// <param name="aArgs">
    /// Command line arguments.
    /// </param>
    ///
    /// <returns>
    /// A new Process object that will call the GraphMLFileProcessor
    /// executable.
    /// </returns>
    //*************************************************************************

    private static Process
    CreateGraphMLFileProcessorProcess
    (
        String[] aArgs
    )
    {
        Debug.Assert(aArgs != null);

        ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();

        oProcessStartInfo.FileName =
            NodeXLClickOncePathUtil.GetPathOfFileInNodeXLFolder(
                GraphMLFileProcessorFileName);

        oProcessStartInfo.Arguments = String.Join(" ", aArgs);

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
    /// message..
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

    /// Friendly name of the GraphMLFileProcessor program.

    private const String GraphMLFileProcessorProgramName =
        "GraphML File Processor";

    /// Name of the GraphMLFileProcessor executable file name, without a path.

    private const String GraphMLFileProcessorFileName =
        "NodeXLGraphMLFileProcessor.exe";
}

}
