

// Define UseLocalGraphService to use a local GraphService WCF service instead
// of the real service.

// #define UseLocalGraphService

using System;
using System.Xml;
using System.ComponentModel;
using System.ServiceModel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.SocialNetworkLib;
using Smrf.GraphServer.WcfService;
using Smrf.NodeXL.GraphDataProviders.Twitter;

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
//*****************************************************************************
//  Class: GraphServerTwitterSearchNetworkAnalyzer
//
/// <summary>
/// Uses the NodeXL Graph Server to get a network created from a specified set
/// of tweets.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync(String, DateTime, DateTime, Boolean,
/// String, String)" /> to asynchronously get the network, or <see
/// cref="GetNetwork" /> to get it synchronously.
///
/// <para>
/// The network is obtained from the NodeXL Graph Server, which is a server
/// that periodically collects tweets and user information for a small set of
/// search terms and stores the data in a database.  This class makes an HTTP
/// call to a WCF service running on the server, and the server returns a
/// complete graph (as GraphML) to this class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphServerTwitterSearchNetworkAnalyzer : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: GraphServerTwitterSearchNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphServerTwitterSearchNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public GraphServerTwitterSearchNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <overloads>
    /// Asynchronously gets a network created from a specified set of tweets.
    /// </overloads>
    ///
    /// <summary>
    /// Asynchronously gets a network created from a set of tweets specified by
    /// a search term and a date range.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="minimumStatusDateUtc">
    /// Minimum status date, in UTC.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand the URLs in each status.
    /// </param>
    ///
    /// <param name="graphServerUserName">
    /// User name for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="graphServerPassword">
    /// Password for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see
    /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
    /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
    /// an XmlDocument containing the network as GraphML.
    ///
    /// <para>
    /// To cancel the analysis, call <see
    /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetNetworkAsync
    (
        String searchTerm,
        DateTime minimumStatusDateUtc,
        DateTime maximumStatusDateUtc,
        Boolean expandStatusUrls,
        String graphServerUserName,
        String graphServerPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumStatusDateUtc >= minimumStatusDateUtc);
        Debug.Assert( !String.IsNullOrEmpty(graphServerUserName) );
        Debug.Assert( !String.IsNullOrEmpty(graphServerPassword) );
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.StatusCriteria = new StatusCriteria(
            searchTerm, minimumStatusDateUtc, maximumStatusDateUtc);

        oGetNetworkAsyncArgs.ExpandStatusUrls = expandStatusUrls;
        oGetNetworkAsyncArgs.GraphServerUserName = graphServerUserName;
        oGetNetworkAsyncArgs.GraphServerPassword = graphServerPassword;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a network created from a set of tweets specified by
    /// a search term, a maximum date and a maximum number of tweets going
    /// backward.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.  Note that is a DateTime and not just a
    /// date, so if you want all statuses on 2014/04/01, for example, you have
    /// to specify 2014/04/01 23:59:59.
    /// </param>
    ///
    /// <param name="maximumStatusesGoingBackward">
    /// Maximum number of statuses to get, going backward in time.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand the URLs in each status.
    /// </param>
    ///
    /// <param name="graphServerUserName">
    /// User name for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="graphServerPassword">
    /// Password for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see
    /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
    /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
    /// an XmlDocument containing the network as GraphML.
    ///
    /// <para>
    /// To cancel the analysis, call <see
    /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetNetworkAsync
    (
        String searchTerm,
        DateTime maximumStatusDateUtc,
        Int32 maximumStatusesGoingBackward,
        Boolean expandStatusUrls,
        String graphServerUserName,
        String graphServerPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumStatusesGoingBackward > 0);
        Debug.Assert( !String.IsNullOrEmpty(graphServerUserName) );
        Debug.Assert( !String.IsNullOrEmpty(graphServerPassword) );
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.StatusCriteria = new StatusCriteria(
            searchTerm, maximumStatusDateUtc, maximumStatusesGoingBackward);

        oGetNetworkAsyncArgs.ExpandStatusUrls = expandStatusUrls;
        oGetNetworkAsyncArgs.GraphServerUserName = graphServerUserName;
        oGetNetworkAsyncArgs.GraphServerPassword = graphServerPassword;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetwork()
    //
    /// <summary>
    /// Synchronously gets a network created from a specified set of tweets.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.  Note that is a DateTime and not just a
    /// date, so if you want all statuses on 2014/04/01, for example, you have
    /// to specify 2014/04/01 23:59:59.
    /// </param>
    ///
    /// <param name="maximumStatusesGoingBackward">
    /// Maximum number of statuses to get, going backward in time.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand the URLs in each status.
    /// </param>
    ///
    /// <param name="graphServerUserName">
    /// User name for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="graphServerPassword">
    /// Password for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetNetwork
    (
        String searchTerm,
        DateTime maximumStatusDateUtc,
        Int32 maximumStatusesGoingBackward,
        Boolean expandStatusUrls,
        String graphServerUserName,
        String graphServerPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumStatusesGoingBackward > 0);
        Debug.Assert( !String.IsNullOrEmpty(graphServerUserName) );
        Debug.Assert( !String.IsNullOrEmpty(graphServerPassword) );
        AssertValid();

        StatusCriteria oStatusCriteria = new StatusCriteria(
            searchTerm, maximumStatusDateUtc, maximumStatusesGoingBackward);

        return ( GetNetworkInternal(oStatusCriteria, expandStatusUrls,
            graphServerUserName, graphServerPassword) );
    }

    //*************************************************************************
    //  Method: ExceptionToMessage()
    //
    /// <summary>
    /// Converts an exception to an error message appropriate for a user
    /// interface.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <returns>
    /// An error message appropriate for a user interface.
    /// </returns>
    //*************************************************************************

    public override String
    ExceptionToMessage
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);
        AssertValid();

        String message;

        if ( oException is FaultException<String> )
        {
            // The GraphService throws a FaultException<String> with a friendly
            // error message when the graph can't be obtained for a known
            // reason.

            message = ( ( FaultException<String> )oException ).Detail;
        }
        else
        {
            message = "An unexpected problem occurred.\r\n\r\n"
                + ExceptionUtil.GetMessageTrace(oException);
        }

        return (message);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <overloads>
    /// Gets the requested network.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the requested network and handles exceptions.
    /// </summary>
    ///
    /// <param name="oStatusCriteria">
    /// Specifies the criteria used for getting statuses from the NodeXL Graph
    /// Server.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in each status.
    /// </param>
    ///
    /// <param name="sGraphServerUserName">
    /// User name for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="sGraphServerPassword">
    /// Password for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        StatusCriteria oStatusCriteria,
        Boolean bExpandStatusUrls,
        String sGraphServerUserName,
        String sGraphServerPassword
    )
    {
        Debug.Assert(oStatusCriteria != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphServerUserName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphServerPassword) );
        AssertValid();

        XmlDocument oGraphMLXmlDocument = null;
        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            oGraphMLXmlDocument = GetNetworkInternal(
                oStatusCriteria, bExpandStatusUrls, sGraphServerUserName,
                sGraphServerPassword, oRequestStatistics);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, new XmlDocument(),
                oRequestStatistics);
        }

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics, 

            GetNetworkDescription(oStatusCriteria, oGraphMLXmlDocument),

            SnaTitleCreator.CreateSnaTitle(
                oStatusCriteria.SearchTerm,
                oRequestStatistics),

            "Graph Server " + oStatusCriteria.SearchTerm
            );

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <summary>
    /// Gets the requested network.
    /// </summary>
    ///
    /// <param name="oStatusCriteria">
    /// Specifies the criteria used for getting statuses from the NodeXL Graph
    /// Server.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in each status.
    /// </param>
    ///
    /// <param name="sGraphServerUserName">
    /// User name for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="sGraphServerPassword">
    /// Password for the account to use on the NodeXL Graph Server.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument containing the requested network.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        StatusCriteria oStatusCriteria,
        Boolean bExpandStatusUrls,
        String sGraphServerUserName,
        String sGraphServerPassword,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oStatusCriteria != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphServerUserName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphServerPassword) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        GraphServiceClient oClient = new GraphServiceClient(
            GetWcfServiceBinding(), new EndpointAddress(GraphServiceUrl) );

        Byte [] abtZippedGraphML;

        // There are two ways to get the network: With a maximum status date,
        // and with a maximum number of statuses.

        if (oStatusCriteria.HasDateRange)
        {
            abtZippedGraphML = oClient.GetTwitterSearchNetworkAsZippedGraphML(
                oStatusCriteria.SearchTerm,
                oStatusCriteria.MinimumStatusDateUtc,
                oStatusCriteria.MaximumStatusDateUtc,
                bExpandStatusUrls,
                sGraphServerUserName,
                sGraphServerPassword
                );
        }
        else
        {
            abtZippedGraphML = oClient.GetTwitterSearchNetworkAsZippedGraphML2(
                oStatusCriteria.SearchTerm,
                oStatusCriteria.MaximumStatusDateUtc,
                oStatusCriteria.MaximumStatusesGoingBackward,
                bExpandStatusUrls,
                sGraphServerUserName,
                sGraphServerPassword
                );
        }

        String sGraphML = ZipUtil.UnzipOneTextFile(abtZippedGraphML);
        abtZippedGraphML = null;

        XmlDocument oXmlDocument = new XmlDocument();

        // Note: When the DotNetZip library used by ZipUtil unzips the GraphML,
        // it includes a BOM as the first character.  Remove that character.

        oXmlDocument.LoadXml( sGraphML.Substring(1) );

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkDescription()
    //
    /// <summary>
    /// Gets a description of the network.
    /// </summary>
    ///
    /// <param name="oStatusCriteria">
    /// Specifies the criteria used for getting statuses from the NodeXL Graph
    /// Server.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The XmlDocument that contains the network.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        StatusCriteria oStatusCriteria,
        XmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oStatusCriteria != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        const String Int32FormatString = "N0";

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();

        Int32 iVertexCount = GetVertexCount(oGraphMLXmlDocument);

        oNetworkDescriber.AddSentence(

            "The graph represents a network of {0} Twitter {1} whose tweets in"
            + " the requested range contained \"{2}\", or who {3} replied to"
            + " or mentioned in those tweets."
            ,
            iVertexCount.ToString(Int32FormatString),
            StringUtil.MakePlural("user", iVertexCount),
            oStatusCriteria.SearchTerm,
            iVertexCount > 1 ? "were" : "was"
            );

        oNetworkDescriber.AddNetworkTime(NetworkSource);

        if (oStatusCriteria.HasDateRange)
        {
            oNetworkDescriber.AddSentenceNewParagraph(

                "The requested date range was from {0} through {1}."
                ,
                oNetworkDescriber.FormatEventTime(
                    oStatusCriteria.MinimumStatusDateUtc),

                oNetworkDescriber.FormatEventTime(
                    oStatusCriteria.MaximumStatusDateUtc)
                );
        }
        else
        {
            oNetworkDescriber.AddSentenceNewParagraph(

                "The requested start date was {0} and the maximum number of"
                + " tweets (going backward in time) was {1}."
                ,
                oNetworkDescriber.FormatEventTime(
                    oStatusCriteria.MaximumStatusDateUtc),

                oStatusCriteria.MaximumStatusesGoingBackward.ToString(
                    Int32FormatString)
                );
        }

        oNetworkDescriber.StartNewParagraph();

        TweetDateRangeAnalyzer.AddTweetDateRangeToNetworkDescription(
            oGraphMLXmlDocument, oNetworkDescriber);

        oNetworkDescriber.AddSentenceNewParagraph(
            "There is an edge for each \"replies-to\" relationship in a tweet,"
            + " an edge for each \"mentions\" relationship in a tweet, and a"
            + " self-loop edge for each tweet that is not a \"replies-to\" or"
            + " \"mentions\"."
            );

        return ( oNetworkDescriber.ConcatenateSentences() );
    }

    //*************************************************************************
    //  Method: GetVertexCount()
    //
    /// <summary>
    /// Gets the number of vertices in the network.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The XmlDocument that contains the network.
    /// </param>
    ///
    /// <returns>
    /// The number of vertices in the network.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetVertexCount
    (
        XmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        Int32 iVertexCount = 0;
        XmlNode oXmlNode;

        XmlNamespaceManager oXmlNamespaceManager =
            GraphMLXmlDocument.CreateXmlNamespaceManager(
                oGraphMLXmlDocument, "g");

        if ( XmlUtil2.TrySelectSingleNode(oGraphMLXmlDocument,
            "g:graphml/g:graph/g:node", oXmlNamespaceManager,
            out oXmlNode) )
        {
            while (oXmlNode != null)
            {
                if (oXmlNode.Name == "node")
                {
                    iVertexCount++;
                }

                oXmlNode = oXmlNode.NextSibling;
            }
        }

        return (iVertexCount);
    }

    //*************************************************************************
    //  Method: GetWcfServiceBinding()
    //
    /// <summary>
    /// Returns the binding to use for communicating with the Graph Server's
    /// WCF service.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="BasicHttpBinding" /> object.
    /// </returns>
    //*************************************************************************

    protected BasicHttpBinding
    GetWcfServiceBinding()
    {
        BasicHttpBinding oBasicHttpBinding = new BasicHttpBinding();

        // These settings were copied from the NodeXLGraphGalleryExporter
        // class, which uses a similar binding.

        oBasicHttpBinding.Name = "BasicHttpBinding";

        oBasicHttpBinding.SendTimeout =
            new TimeSpan(0, SendTimeoutMinutes, 0);

        oBasicHttpBinding.ReceiveTimeout =
            new TimeSpan(0, ReceiveTimeoutMinutes, 0);

        oBasicHttpBinding.MaxBufferSize = MaximumBytes;
        oBasicHttpBinding.MaxReceivedMessageSize = MaximumBytes;
        oBasicHttpBinding.TransferMode = TransferMode.Buffered;

        XmlDictionaryReaderQuotas oReaderQuotas =
            new XmlDictionaryReaderQuotas();

        oReaderQuotas.MaxArrayLength = MaximumBytes;
        oReaderQuotas.MaxStringContentLength = MaximumBytes;
        oBasicHttpBinding.ReaderQuotas = oReaderQuotas;

        return (oBasicHttpBinding);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(e.Argument is GetNetworkAsyncArgs);

        GetNetworkAsyncArgs oGetNetworkAsyncArgs =
            (GetNetworkAsyncArgs)e.Argument;

        try
        {
            e.Result = GetNetworkInternal(
                oGetNetworkAsyncArgs.StatusCriteria,
                oGetNetworkAsyncArgs.ExpandStatusUrls,
                oGetNetworkAsyncArgs.GraphServerUserName,
                oGetNetworkAsyncArgs.GraphServerPassword
                );
        }
        catch (CancellationPendingException)
        {
            e.Cancel = true;
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// URL of the Graph Server.  This is an Elastic IP address created within
    /// Amazon EC2 and assigned to the Graph Server EC2 instance.

    protected const String GraphServiceUrl =

    #if UseLocalGraphService
        "http://localhost/GraphService/GraphService.svc";
    #else
        "http://184.72.239.155/GraphService/GraphService.svc";
    #endif

    /// Send timeout, in minutes.

    protected const Int32 SendTimeoutMinutes = 20;

    /// Receive timeout, in minutes.

    protected const Int32 ReceiveTimeoutMinutes = 20;


    /// The source of Graph Server networks, used in network descriptions.

    protected const String NetworkSource = "the NodeXL Graph Server";


    /// Maximum number of bytes that can be received from the WCF service.
    /// This is the value to use for length-related parameters in the WCF
    /// binding object.  If this maximum is exceeded, a
    /// System.ServiceModel.CommunicationException will occur when attempting
    /// to receive a network.
    ///
    /// This must be the same value that is specified many times in the
    /// Web.config file in the GraphServer\WcfService project.  Search for the
    /// following text in the Web.config file to find the value:
    //
    //     maxRequestLength
    //     maxReceivedMessageSize
    //     maxArrayLength
    //     maxStringContentLength

    protected const Int32 MaximumBytes = 50000000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : Object
    {
        ///
        public StatusCriteria StatusCriteria;
        ///
        public Boolean ExpandStatusUrls;
        ///
        public String GraphServerUserName;
        ///
        public String GraphServerPassword;
    };
}

}
