

using System;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.GraphDataProviders;
using Smrf.NodeXL.GraphDataProviders.Twitter;

using Smrf.SocialNetworkLib;
using Smrf.AppLib;


namespace Smrf.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// The application's entry point.
/// </summary>
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
        string[] args
    )
    {
        try
        {
            RunProgram(args);
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
    /// Runs the program.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    private static void
    RunProgram
    (
        string[] args
    )
    {
        String sNetworkConfigurationFilePath;

        ParseCommandLine(args, out sNetworkConfigurationFilePath);

        NetworkConfigurationFileParser oNetworkConfigurationFileParser =
            new NetworkConfigurationFileParser();

        NetworkType eNetworkType = NetworkType.TwitterSearch;

        try
        {
            oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                sNetworkConfigurationFilePath);

            eNetworkType = oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            OnNetworkConfigurationFileException(oXmlException);
        }

        XmlDocument oXmlDocument = null;
        String sNetworkFileFolderPath = null;
        DateTime oStartTime = DateTime.Now;

        switch (eNetworkType)
        {
            case NetworkType.TwitterSearch:

                GetTwitterSearchNetwork(oStartTime,
                    sNetworkConfigurationFilePath,
                    oNetworkConfigurationFileParser, out oXmlDocument,
                    out sNetworkFileFolderPath);

                break;

            case NetworkType.GraphServerTwitterSearch:

                GetGraphServerTwitterSearchNetwork(oStartTime,
                    sNetworkConfigurationFilePath,
                    oNetworkConfigurationFileParser, out oXmlDocument,
                    out sNetworkFileFolderPath);

                break;

            default:

                Debug.Assert(false);
                break;
        }

        Debug.Assert(oXmlDocument != null);

        SaveNetworkToGraphML(oStartTime, oXmlDocument,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath);

        Exit(ExitCode.Success, null);
    }

    //*************************************************************************
    //  Method: ParseCommandLine()
    //
    /// <summary>
    /// Parses the command line arguments.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Where the full path of the specified network configuration file gets
    /// stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    ParseCommandLine
    (
        string[] args,
        out String sNetworkConfigurationFilePath
    )
    {
        Debug.Assert(args != null);

        sNetworkConfigurationFilePath = null;

        if (args.Length == 1)
        {
            String sArgument = args[0];

            if (sArgument == "/?")
            {
                Console.WriteLine(UsageMessage);
                Exit(ExitCode.Success, null);
            }

            // Note that the validity of the file name is checked by
            // NetworkConfigurationFileParser, not by this class.

            sNetworkConfigurationFilePath = sArgument;
            return;
        }

        Exit(ExitCode.InvalidCommandLineArguments,
            "\r\nThe command line arguments are not valid.\r\n" + UsageMessage
            );
    }

    //*************************************************************************
    //  Method: GetTwitterSearchNetwork()
    //
    /// <summary>
    /// Gets a Twitter search network directly from Twitter.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <param name="oNetworkConfigurationFileParser">
    /// Parses a network configuration file.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where the XML document containing the network as GraphML gets stored.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    GetTwitterSearchNetwork
    (
        DateTime oStartTime,
        String sNetworkConfigurationFilePath,
        NetworkConfigurationFileParser oNetworkConfigurationFileParser,
        out XmlDocument oXmlDocument,
        out String sNetworkFileFolderPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert(oNetworkConfigurationFileParser != null);

        oXmlDocument = null;
        sNetworkFileFolderPath = null;

        String sSearchTerm = null;

        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude =
            TwitterSearchNetworkAnalyzer.WhatToInclude.None;

        Int32 iMaximumTweets = Int32.MinValue;

        try
        {
            oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumTweets,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            // (This call exits the program.)

            OnNetworkConfigurationFileException(oXmlException);
        }

        TwitterSearchNetworkAnalyzer oTwitterSearchNetworkAnalyzer =
            new TwitterSearchNetworkAnalyzer();

        SubscribeToProgressChangedEvent(oTwitterSearchNetworkAnalyzer);

        Console.WriteLine(
            "Getting the Twitter search network specified in \"{0}\".  The"
            + " search term is \"{1}\"."
            ,
            sNetworkConfigurationFilePath,
            sSearchTerm
            );

        try
        {
            oXmlDocument = oTwitterSearchNetworkAnalyzer.GetNetwork(
                sSearchTerm, eWhatToInclude, iMaximumTweets);
        }
        catch (Exception oException)
        {
            // Note that this call might exit the program.

            oXmlDocument = OnGetNetworkException(oStartTime, oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oTwitterSearchNetworkAnalyzer);
        }
    }

    //*************************************************************************
    //  Method: GetGraphServerTwitterSearchNetwork()
    //
    /// <summary>
    /// Gets a Twitter search network from the Graph Server.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <param name="oNetworkConfigurationFileParser">
    /// Parses a network configuration file.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where the XML document containing the network as GraphML gets stored.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    GetGraphServerTwitterSearchNetwork
    (
        DateTime oStartTime,
        String sNetworkConfigurationFilePath,
        NetworkConfigurationFileParser oNetworkConfigurationFileParser,
        out XmlDocument oXmlDocument,
        out String sNetworkFileFolderPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert(oNetworkConfigurationFileParser != null);

        oXmlDocument = null;
        sNetworkFileFolderPath = null;

        String sSearchTerm = null;
        Int32 iStartDateInDaysBeforeToday = Int32.MinValue;
        Int32 iMaximumStatusesGoingBackward = Int32.MinValue;
        Boolean bExpandStatusUrls = false;
        String sGraphServerUserName = null;
        String sGraphServerPassword = null;

        try
        {
            oNetworkConfigurationFileParser.
            GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm,
                out iStartDateInDaysBeforeToday,
                out iMaximumStatusesGoingBackward,
                out bExpandStatusUrls,
                out sGraphServerUserName,
                out sGraphServerPassword,
                out sNetworkFileFolderPath
                );
        }
        catch (XmlException oXmlException)
        {
            // (This call exits the program.)

            OnNetworkConfigurationFileException(oXmlException);
        }

        DateTime oMaximumStatusDateUtc = GetMaximumStatusDateUtc(
            oStartTime, iStartDateInDaysBeforeToday);

        Smrf.NodeXL.GraphDataProviders.GraphServer.GraphServerTwitterSearchNetworkAnalyzer
            oGraphServerTwitterSearchNetworkAnalyzer =
                new Smrf.NodeXL.GraphDataProviders.GraphServer.GraphServerTwitterSearchNetworkAnalyzer();

        SubscribeToProgressChangedEvent(
            oGraphServerTwitterSearchNetworkAnalyzer);

        Console.WriteLine(
            "Getting the Graph Server Twitter search network specified in"
            + " \"{0}\".  The search term is \"{1}\".  The start date is"
            + " {2}, UTC.  The maximum number of tweets going backward is {3}."
            ,
            sNetworkConfigurationFilePath,
            sSearchTerm,
            oMaximumStatusDateUtc,
            iMaximumStatusesGoingBackward
            );

        try
        {
            oXmlDocument = oGraphServerTwitterSearchNetworkAnalyzer.GetNetwork(
                sSearchTerm, oMaximumStatusDateUtc,
                iMaximumStatusesGoingBackward, bExpandStatusUrls,
                sGraphServerUserName, sGraphServerPassword);
        }
        catch (Exception oException)
        {
            // Note that this call might exit the program.

            oXmlDocument = OnGetNetworkException(oStartTime, oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oGraphServerTwitterSearchNetworkAnalyzer);
        }
    }

    //*************************************************************************
    //  Method: GetMaximumStatusDateUtc()
    //
    /// <summary>
    /// Gets the maximum status date.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="iStartDateInDaysBeforeToday">
    /// The tweet start date, specified as the number of days before today.
    /// If today is June 10, for example, and you set this to 7, then the
    /// returned date will be June 3.  Must be greater than or equal to zero.
    /// </param>
    ///
    /// <returns>
    /// The maximum status date, in UTC.
    /// </returns>
    //*************************************************************************

    private static DateTime
    GetMaximumStatusDateUtc
    (
        DateTime oStartTime,
        Int32 iStartDateInDaysBeforeToday
    )
    {
        Debug.Assert(iStartDateInDaysBeforeToday >= 0);

        // Sample oStartTime: 2014/06/10 3:00 PM

        // Sample oStartTime.Date: 2014/06/10 12:00 AM

        // Sample returned value: 2014/06/03 12:00 AM

        return ( oStartTime.Date.AddDays(-iStartDateInDaysBeforeToday) );
    }

    //*************************************************************************
    //  Method: SubscribeToProgressChangedEvent()
    //
    /// <summary>
    /// Subscribes to the ProgressChanged event on a network analyzer.
    /// </summary>
    ///
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    //*************************************************************************

    private static void
    SubscribeToProgressChangedEvent
    (
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        oHttpNetworkAnalyzerBase.ProgressChanged +=
            new ProgressChangedEventHandler(
                HttpNetworkAnalyzer_ProgressChanged);
    }

    //*************************************************************************
    //  Method: SaveNetworkToGraphML()
    //
    /// <summary>
    /// Saves a network to a GraphML file.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// The XML document containing the network as GraphML.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    //*************************************************************************

    private static void
    SaveNetworkToGraphML
    (
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );

        // Sample network file path:
        //
        // C:\NetworkConfiguration_2010-06-01_02-00-00.graphml
        
        String sNetworkFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            String.Empty, "graphml");

        Console.WriteLine(
            "Saving the network to the GraphML file \"{0}\"."
            ,
            sNetworkFilePath
            );

        try
        {
            // Don't allow sharing.  This is so the GraphMLFileProcessor
            // program won't be able to process the file while it is still
            // being written.

            using ( FileStream fileStream = File.Open(sNetworkFilePath,
                FileMode.Create, FileAccess.Write, FileShare.None) )
            {
                oXmlDocument.Save(fileStream);
            }
        }
        catch (IOException oIOException)
        {
            Exit(ExitCode.SaveNetworkFileError,
                "The file couldn't be saved.  Details:\r\n\r\n"
                + oIOException.Message
                );
        }
    }

    //*************************************************************************
    //  Method: OnNetworkConfigurationFileException()
    //
    /// <summary>
    /// Handles an exception thrown while parsing the network configuration
    /// file.
    /// </summary>
    ///
    /// <param name="oXmlException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <remarks>
    /// The NetworkConfigurationFileParser wraps all parsing errors in
    /// XmlExceptions.
    ///
    /// <para>
    /// This method exits the program.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    private static void
    OnNetworkConfigurationFileException
    (
        XmlException oXmlException
    )
    {
        Debug.Assert(oXmlException != null);

        Exit(ExitCode.InvalidNetworkConfigurationFile,
            "\r\n" + oXmlException.Message + "\r\n" + UsageMessage
            );
    }

    //*************************************************************************
    //  Method: OnGetNetworkException()
    //
    /// <summary>
    /// Handles an exception thrown while getting a network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    ///
    /// <returns>
    /// If the exception is a <see cref="PartialNetworkException" />, an
    /// XmlDocument describing the partial network is returned.  Otherwise,
    /// the program exits.
    /// </returns>
    //*************************************************************************

    private static XmlDocument
    OnGetNetworkException
    (
        DateTime oStartTime,
        Exception oException,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        if (oException is PartialNetworkException)
        {
            return ( OnGetNetworkPartialNetworkException(
                oStartTime, (PartialNetworkException)oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oHttpNetworkAnalyzerBase) );
        }
        else
        {
            // (This call exits the program.)

            OnGetNetworkOtherException(oStartTime, oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oHttpNetworkAnalyzerBase);

            // Make the compiler happy.

            return (null);
        }
    }

    //*************************************************************************
    //  Method: OnGetNetworkPartialNetworkException()
    //
    /// <summary>
    /// Handles a PartialNetworkException thrown while getting a network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oPartialNetworkException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    ///
    /// <returns>
    /// The partial network, as GraphML.
    /// </returns>
    //*************************************************************************

    private static XmlDocument
    OnGetNetworkPartialNetworkException
    (
        DateTime oStartTime,
        PartialNetworkException oPartialNetworkException,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oPartialNetworkException != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        // Write a text file to let the user know that a partial network was
        // obtained.
        //
        // Sample file path:
        //
        // C:\PartialNetwork_NetworkConfiguration_2010-06-01_02-00-00.txt

        String sFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            "PartialNetworkInfo_", "txt");

        using ( StreamWriter oStreamWriter = new StreamWriter(sFilePath) )
        {
            Debug.Assert(oHttpNetworkAnalyzerBase != null);

            oStreamWriter.WriteLine( oPartialNetworkException.ToMessage(
                oHttpNetworkAnalyzerBase.ExceptionToMessage(
                    oPartialNetworkException.RequestStatistics.
                        LastUnexpectedException)
                ) );
        }

        return (oPartialNetworkException.PartialNetwork);
    }

    //*************************************************************************
    //  Method: OnGetNetworkOtherException()
    //
    /// <summary>
    /// Handles an exception (other than a PartialNetworkException) thrown
    /// while getting a network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    ///
    /// <remarks>
    /// This method exits the program.
    /// </remarks>
    //*************************************************************************

    private static void
    OnGetNetworkOtherException
    (
        DateTime oStartTime,
        Exception oException,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        // Sample error file path:
        //
        // C:\Error_NetworkConfiguration_2010-06-01_02-00-00.txt

        String sErrorFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            "Error_", "txt");

        String sErrorMessage =
            "The network couldn't be obtained.  Details:"
            + "\r\n\r\n"
            + oHttpNetworkAnalyzerBase.ExceptionToMessage(oException)
            ;

        using ( StreamWriter oStreamWriter = new StreamWriter(sErrorFilePath) )
        {
            Debug.Assert(oHttpNetworkAnalyzerBase != null);

            oStreamWriter.WriteLine(sErrorMessage);
        }

        Exit(ExitCode.CouldNotGetNetwork, sErrorMessage);
    }

    //*************************************************************************
    //  Method: HttpNetworkAnalyzer_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the HttpNetworkAnalyzer objects.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private static void
    HttpNetworkAnalyzer_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        Debug.Assert(e.UserState is String);

        Console.WriteLine( (String)e.UserState );
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

    /// Program usage message.

    private const String UsageMessage =

        "\r\n"
        + "For information on how to use this program, please go to "
        + " http://nodexl.codeplex.com/discussions/545884."
        ;
}

}
