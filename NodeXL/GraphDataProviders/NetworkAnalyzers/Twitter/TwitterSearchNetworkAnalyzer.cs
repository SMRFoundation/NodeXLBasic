
using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.DateTimeLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterSearchNetworkAnalyzer
//
/// <summary>
/// Gets a network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get the network, or
/// <see cref="GetNetwork" /> to get it synchronously.
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterSearchNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterSearchNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: WhatToInclude
    //
    /// <summary>
    /// Flags that specify what should be included in a network requested from
    /// this class.
    /// </summary>
    ///
    /// <remarks>
    /// The flags can be ORed together.
    /// </remarks>
    //*************************************************************************

    [System.FlagsAttribute]

    public enum
    WhatToInclude
    {
        /// <summary>
        /// Include nothing.
        /// </summary>

        None = 0,

        /// <summary>
        /// Include each status.
        /// </summary>

        Statuses = 1,

        /// <summary>
        /// Expand the URLs contained within each status.  Used only if <see
        /// cref="Statuses" /> is specified.
        /// </summary>

        ExpandedStatusUrls = 2,

        /// <summary>
        /// Include each person's statistics.
        /// </summary>

        Statistics = 4,

        /// <summary>
        /// Include an edge for each followed relationship.
        /// </summary>

        FollowedEdges = 8,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet is a
        /// reply to person B.
        /// </summary>

        RepliesToEdges = 16,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet
        /// mentions person B.
        /// </summary>

        MentionsEdges = 32,

        /// <summary>
        /// Include an edge from person A to person A (a self-loop) if person
        /// A's tweet doesn't reply to or mention anyone.
        /// </summary>

        NonRepliesToNonMentionsEdges = 64,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a directed network of people who have tweeted a
    /// specified search term.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
        WhatToInclude whatToInclude,
        Int32 maximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumPeoplePerRequest > 0);
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.SearchTerm = searchTerm;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.MaximumPeoplePerRequest = maximumPeoplePerRequest;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetwork()
    //
    /// <summary>
    /// Synchronously gets a directed network of people who have tweeted a
    /// specified search term.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
        WhatToInclude whatToInclude,
        Int32 maximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumPeoplePerRequest > 0);
        AssertValid();

        return ( GetNetworkInternal(searchTerm, whatToInclude,
            maximumPeoplePerRequest) );
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <overloads>
    /// Gets the requested network.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the requested network.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        AssertValid();

        BeforeGetNetwork();

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statistics);

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(
            bIncludeStatistics, false);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, TooltipID,
            "Tooltip", "string", null);

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetNetworkInternal(sSearchTerm, eWhatToInclude,
                iMaximumPeoplePerRequest, oRequestStatistics,
                oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics, 

            GetNetworkDescription(sSearchTerm, eWhatToInclude,
                iMaximumPeoplePerRequest, oGraphMLXmlDocument),

            "Twitter Search " + sSearchTerm
            );

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <summary>
    /// Gets the requested network, given a GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    GetNetworkInternal
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        Boolean bIncludeStatuses = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statuses);

        if (bIncludeStatuses)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, StatusID,
                "Tweet", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, StatusUrlsID,
                "URLs in Tweet", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, StatusDomainsID,
                "Domains in Tweet", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, StatusHashtagsID,
                "Hashtags in Tweet", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, StatusDateUtcID,
                "Tweet Date (UTC)", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true,
                StatusWebPageUrlID, "Twitter Page for Tweet", "string", null);

            DefineLatitudeAndLongitudeGraphMLAttributes(oGraphMLXmlDocument,
                true);

            DefineImportedIDGraphMLAttribute(oGraphMLXmlDocument, true);
        }

        // The key is the Twitter user ID and the value is the corresponding
        // TwitterUser.

        Dictionary<String, TwitterUser> oUserIDDictionary =
            new Dictionary<String, TwitterUser>();

        // First, add a vertex for each person who has tweeted the search term.

        AppendVertexXmlNodes(sSearchTerm, eWhatToInclude,
            iMaximumPeoplePerRequest, oGraphMLXmlDocument, oUserIDDictionary,
            oRequestStatistics);

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedEdges) )
        {
            // Look at the people followed by each author, and if a followed
            // has also tweeted the search term, add an edge between the author
            // and the followed.

            AppendFollowedOrFollowingEdgeXmlNodes(oUserIDDictionary, true,
                iMaximumPeoplePerRequest, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude, WhatToInclude.Statistics) )
        {
            AppendStatistics(oGraphMLXmlDocument, oUserIDDictionary,
                oRequestStatistics);
        }

        // Add edges nodes for replies-to and mentions relationships.

        AppendRepliesToAndMentionsEdgeXmlNodes(oGraphMLXmlDocument,
            oUserIDDictionary.Values,
            TwitterUsersToUniqueScreenNames(oUserIDDictionary.Values),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.RepliesToEdges),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.MentionsEdges),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.NonRepliesToNonMentionsEdges),

            bIncludeStatuses
            );
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNodes()
    //
    /// <summary>
    /// Appends a vertex XML node for each person who has tweeted a specified
    /// search term.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNodes
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // Convert spaces in the search term to a plus sign.

        String sUrl = String.Format(

            "{0}?q={1}&rpp=100&result_type=recent&{2}"
            ,
            SearchApiUri,
            UrlUtil.EncodeUrlParameter(sSearchTerm).Replace("%20", "+"),
            IncludeEntitiesUrlParameter
            );

        Boolean bExpandStatusUrls = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.ExpandedStatusUrls);

        ReportProgress("Getting a list of tweets.");

        // The JSON contains a "results" array for the tweets that contain the
        // search term.  Multiple tweets may have the same author.

        foreach ( Object oResult in EnumerateJsonValues(sUrl, "results", 15,
            Int32.MaxValue, true, false, oRequestStatistics) )
        {
            if (oUserIDDictionary.Count == iMaximumPeoplePerRequest)
            {
                break;
            }

            Dictionary<String, Object> oValueDictionary =
                ( Dictionary<String, Object> )oResult;

            String sScreenName, sUserID;

            if (
                !TryGetJsonValueFromDictionary(oValueDictionary, "from_user",
                    out sScreenName)
                ||
                !TryGetJsonValueFromDictionary(oValueDictionary,
                    "from_user_id_str", out sUserID)
                )
            {
                continue;
            }

            TwitterUser oTwitterUser;

            Boolean bIsFirstTweetForAuthor = TryAppendVertexXmlNode(
                sScreenName, sUserID, oGraphMLXmlDocument, oUserIDDictionary,
                out oTwitterUser);

            // Get the author's image URL.

            String sImageUrl;

            if (
                bIsFirstTweetForAuthor
                && 
                TryGetJsonValueFromDictionary(oValueDictionary,
                    "profile_image_url", out sImageUrl)
                )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oTwitterUser.VertexXmlNode, ImageFileID, sImageUrl);
            }

            // Get the status information.

            String sID, sStatus;

            if (
                TryGetJsonValueFromDictionary(oValueDictionary, "id_str",
                    out sID)
                &&
                TryGetJsonValueFromDictionary(oValueDictionary, "text",
                    out sStatus)
                )
            {
                String sStatusDateUtc;

                if ( TryGetJsonValueFromDictionary(oValueDictionary,
                    "created_at", out sStatusDateUtc) )
                {
                    sStatusDateUtc = TwitterDateParser.ParseTwitterDate(
                        sStatusDateUtc);
                }

                String sLatitude, sLongitude;

                GetLatitudeAndLongitudeFromStatusValueDictionary(
                    oValueDictionary, out sLatitude, out sLongitude);

                String sStatusUrls, sStatusHashtags;

                GetUrlsAndHashtagsFromStatusValueDictionary(oValueDictionary, 
                    bExpandStatusUrls, out sStatusUrls, out sStatusHashtags);

                // Note that null date, coordinates, URLs and hashtags are
                // acceptable here.

                oTwitterUser.Statuses.Add( new TwitterStatus(
                    sID, sStatus, sStatusDateUtc, sLatitude, sLongitude,
                    sStatusUrls, sStatusHashtags) );
            }
        }

        AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oUserIDDictionary);
    }

    //*************************************************************************
    //  Method: AppendVertexTooltipXmlNodes()
    //
    /// <summary>
    /// Appends a vertex tooltip XML node for each person in the network.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the user ID and the value is the corresponding TwitterUser.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexTooltipXmlNodes
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        AssertValid();

        foreach (TwitterUser oTwitterUser in oUserIDDictionary.Values)
        {
            // The tooltip is the user's screen name followed by the first
            // tweet returned by Twitter.

            ICollection<TwitterStatus> oStatuses = oTwitterUser.Statuses;

            String sFirstStatus = oStatuses.Count > 0 ?
                oStatuses.First().Text : String.Empty;

            // The Excel template doesn't wrap long tooltip text.  Break the
            // status into lines so the entire tooltip will show in the graph
            // pane.

            sFirstStatus = StringUtil.BreakIntoLines(sFirstStatus, 30);

            String sTooltip = String.Format(
                "{0}\n\n{1}"
                ,
                oTwitterUser.ScreenName,
                sFirstStatus
                );

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oTwitterUser.VertexXmlNode, TooltipID, sTooltip);
        }
    }

    //*************************************************************************
    //  Method: AppendStatistics()
    //
    /// <summary>
    /// Appends user statistics to the GraphML document.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// This method appends statistics to <paramref
    /// name="oGraphMLXmlDocument" /> for each user in <paramref
    /// name="oUserIDDictionary" />.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendStatistics
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        foreach (Dictionary<String, Object> oUserValueDictionary in
            EnumerateUserValueDictionaries(oUserIDDictionary.Keys.ToArray(),
            oRequestStatistics) )
        {
            String sUserID;
            TwitterUser oTwitterUser;

            if (
                TryGetJsonValueFromDictionary(oUserValueDictionary, "id",
                    out sUserID)
                &&
                oUserIDDictionary.TryGetValue(sUserID, out oTwitterUser)
                )
            {
                AppendUserInformationFromValueDictionary(oUserValueDictionary,
                    oGraphMLXmlDocument, oTwitterUser, true, false, false,
                    false);
            }
        }
    }

    //*************************************************************************
    //  Method: WhatToIncludeFlagIsSet()
    //
    /// <summary>
    /// Checks whether a flag is set in an ORed combination of WhatToInclude
    /// flags.
    /// </summary>
    ///
    /// <param name="eORedEnumFlags">
    /// Zero or more ORed Enum flags.
    /// </param>
    ///
    /// <param name="eORedEnumFlagsToCheck">
    /// One or more Enum flags to check.
    /// </param>
    ///
    /// <returns>
    /// true if any of the <paramref name="eORedEnumFlagsToCheck" /> flags are
    /// set in <paramref name="eORedEnumFlags" />.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WhatToIncludeFlagIsSet
    (
        WhatToInclude eORedEnumFlags,
        WhatToInclude eORedEnumFlagsToCheck
    )
    {
        return ( (eORedEnumFlags & eORedEnumFlagsToCheck) != 0 );
    }

    //*************************************************************************
    //  Method: GetNetworkDescription()
    //
    /// <summary>
    /// Gets a description of the network.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument that contains the network.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        const String Int32FormatString = "N0";

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();
        Int32 iVertexXmlNodes = oGraphMLXmlDocument.VertexXmlNodes;

        oNetworkDescriber.AddSentence(

            "The graph represents a network of {0} Twitter {1} whose recent"
            + " tweets contained \"{2}\", taken from a data set limited to a"
            + " maximum of {3} users."
            ,
            iVertexXmlNodes.ToString(Int32FormatString),
            StringUtil.MakePlural("user", iVertexXmlNodes),
            sSearchTerm,

            ( (iMaximumPeoplePerRequest == Int32.MaxValue) ?
                1500 : iMaximumPeoplePerRequest ).ToString(Int32FormatString)
            );

        oNetworkDescriber.AddNetworkTime();

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each follows relationship."
                );
        }

        Boolean bIncludeRepliesToEdges = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.RepliesToEdges);

        if (bIncludeRepliesToEdges)
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each \"replies-to\" relationship in a"
                + " tweet."
                );
        }

        Boolean bIncludeMentionsEdges = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.MentionsEdges);

        if (bIncludeMentionsEdges)
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each \"mentions\" relationship in a"
                + " tweet."
                );
        }

        Boolean bIncludeNonRepliesToNonMentionsEdges = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.NonRepliesToNonMentionsEdges);

        if (bIncludeNonRepliesToNonMentionsEdges)
        {
            oNetworkDescriber.AddSentence(
                "There is a self-loop edge for each tweet that is not a"
                + " \"replies-to\" or \"mentions\"."
                );
        }

        if (bIncludeRepliesToEdges && bIncludeMentionsEdges &&
            bIncludeNonRepliesToNonMentionsEdges)
        {
            // Every collected tweet has an edge that contains the date of the
            // tweet, so the range of tweet dates can be determined.

            AddTweetDateRangeToNetworkDescription(
                oGraphMLXmlDocument, oNetworkDescriber);
        }

        return ( oNetworkDescriber.ConcatenateSentences() );
    }

    //*************************************************************************
    //  Method: AddTweetDateRangeToNetworkDescription()
    //
    /// <summary>
    /// Adds the range of tweet dates to a network description.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument that contains the network.
    /// </param>
    ///
    /// <param name="oNetworkDescriber">
    /// Contcatenates sentences into a network description.
    /// </param>
    //*************************************************************************

    protected void
    AddTweetDateRangeToNetworkDescription
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        NetworkDescriber oNetworkDescriber
    )
    {
        AssertValid();
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oNetworkDescriber != null);

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            oGraphMLXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace("g",
            GraphMLXmlDocument.GraphMLNamespaceUri);

        DateTime oMinimumRelationshipDateUtc = DateTime.MaxValue;
        DateTime oMaximumRelationshipDateUtc = DateTime.MinValue;

        // Loop through the graph's edges.

        foreach ( XmlNode oEdgeXmlNode in
            oGraphMLXmlDocument.DocumentElement.SelectNodes(
                "g:graph/g:edge", oXmlNamespaceManager) )
        {
            // Get the value of the edge's "relationship" GraphML-Attribute.

            String sRelationship;

            if ( !TryGetEdgeGraphMLAttributeValue(oEdgeXmlNode,
                RelationshipID, oXmlNamespaceManager, out sRelationship) )
            {
                continue;
            }

            switch (sRelationship)
            {
                case RepliesToRelationship:
                case MentionsRelationship:
                case NonRepliesToNonMentionsRelationship:

                    // Get the value of the edge's "relationship date"
                    // GraphML-Attribute.

                    String sRelationshipDateUtc;

                    if ( !TryGetEdgeGraphMLAttributeValue(oEdgeXmlNode,
                        RelationshipDateUtcID, oXmlNamespaceManager,
                        out sRelationshipDateUtc) )
                    {
                        break;
                    }

                    DateTime oRelationshipDateUtc;

                    try
                    {
                        // Note that the relationship date may be in an
                        // unrecognized format.

                        oRelationshipDateUtc =
                            DateTimeUtil2.FromCultureInvariantString(
                                sRelationshipDateUtc);
                    }
                    catch (FormatException)
                    {
                        break;
                    }

                    if (oRelationshipDateUtc < oMinimumRelationshipDateUtc)
                    {
                        oMinimumRelationshipDateUtc = oRelationshipDateUtc;
                    }

                    if (oRelationshipDateUtc > oMaximumRelationshipDateUtc)
                    {
                        oMaximumRelationshipDateUtc = oRelationshipDateUtc;
                    }

                    break;

                default:

                    break;
            }
        }

        if (oMinimumRelationshipDateUtc != DateTime.MaxValue)
        {
            oNetworkDescriber.AddSentence(
                "The tweets were made over the {0} period from {1} to {2}."
                ,
                oNetworkDescriber.FormatDuration(oMinimumRelationshipDateUtc, 
                    oMaximumRelationshipDateUtc),

                oNetworkDescriber.FormatEventTime(oMinimumRelationshipDateUtc),
                oNetworkDescriber.FormatEventTime(oMaximumRelationshipDateUtc)
                );
        }
    }

    //*************************************************************************
    //  Method: TryGetEdgeGraphMLAttributeValue()
    //
    /// <summary>
    /// Attempts to get the value of a specified GraphML-Attribute.
    /// </summary>
    ///
    /// <param name="oEdgeXmlNode">
    /// The XmlNode that represents the edge.
    /// </param>
    ///
    /// <param name="sAttributeID">
    /// The GraphML-Attribute's ID.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// The XmlNamespaceManager to use.  It's assumed that "g" is used as the
    /// GraphML namespace prefix.
    /// </param>
    ///
    /// <param name="sAttributeValue">
    /// Where the value of the specified GraphML-Attribute gets stored if true
    /// is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified value was found and was a non-empty string.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetEdgeGraphMLAttributeValue
    (
        XmlNode oEdgeXmlNode,
        String sAttributeID,
        XmlNamespaceManager oXmlNamespaceManager,
        out String sAttributeValue
    )
    {
        Debug.Assert(oEdgeXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeID) );
        Debug.Assert(oXmlNamespaceManager != null);

        AssertValid();

        String sXPath = String.Format(

            "g:data[@key=\"{0}\"]/text()"
            ,
            sAttributeID
            );

        return ( XmlUtil2.TrySelectSingleNodeAsString(
            oEdgeXmlNode, sXPath, oXmlNamespaceManager, out sAttributeValue) );
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
                oGetNetworkAsyncArgs.SearchTerm,
                oGetNetworkAsyncArgs.WhatToInclude,
                oGetNetworkAsyncArgs.MaximumPeoplePerRequest);
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

    /// GraphML-attribute IDs.

    protected const String TooltipID = "Tooltip";


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

    protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
    {
        ///
        public String SearchTerm;
        ///
        public WhatToInclude WhatToInclude;
    };
}

}
