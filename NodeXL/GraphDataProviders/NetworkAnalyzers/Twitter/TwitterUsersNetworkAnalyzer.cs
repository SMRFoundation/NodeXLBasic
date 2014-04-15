
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.SocialNetworkLib;
using Smrf.SocialNetworkLib.Twitter;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterUsersNetworkAnalyzer
//
/// <summary>
/// Gets a network that shows the connections between a set of Twitter users
/// specified by either a Twitter List name or a set of Twitter screen names.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get the network, or
/// <see cref="GetNetwork" /> to get it synchronously.
/// </remarks>
//*****************************************************************************

public class TwitterUsersNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterUsersNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterUsersNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterUsersNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: NetworkType
    //
    /// <summary>
    /// Specifies the type of Twitter users network to get.
    /// </summary>
    //*************************************************************************

    public enum
    NetworkType
    {
        /// <summary>
        /// Append vertices for the users specified by the caller, vertices for
        /// the users replied to and mentioned in those users' recent statuses,
        /// and edges for those recent statuses.
        /// </summary>

        Basic = 0,

        /// <summary>
        /// Basic plus vertices and edges for the users who are friends and
        /// followers of the users who were specified by the caller.
        /// </summary>

        BasicPlusFollows = 1,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a network that shows the connections between a set
    /// of Twitter users specified by either a Twitter List name or a set of
    /// Twitter screen names.
    /// </summary>
    ///
    /// <param name="useListName">
    /// If true, <paramref name="listName" /> must be specified and <paramref
    /// name="screenNames" /> is ignored.  If false, <paramref
    /// name="screenNames" /> must be specified and <paramref
    /// name="listName" /> is ignored.
    /// </param>
    ///
    /// <param name="listName">
    /// Twitter List name if <paramref name="useListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="screenNames">
    /// Zero or more Twitter screen names if <paramref name="useListName" /> is
    /// false; unused otherwise.  The screen names can be in any case, but they
    /// get converted to lower case before getting the network.
    /// </param>
    ///
    /// <param name="networkType">
    /// The type of network to get.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand the URLs in the statuses.
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
        Boolean useListName,
        String listName,
        ICollection<String> screenNames,
        NetworkType networkType,
        Boolean expandStatusUrls
    )
    {
        Debug.Assert( !useListName || !String.IsNullOrEmpty(listName) );
        Debug.Assert(useListName || screenNames != null);
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.UseListName = useListName;
        oGetNetworkAsyncArgs.ListName = listName;
        oGetNetworkAsyncArgs.ScreenNames = screenNames;
        oGetNetworkAsyncArgs.NetworkType = networkType;
        oGetNetworkAsyncArgs.ExpandStatusUrls = expandStatusUrls;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetwork()
    //
    /// <summary>
    /// Synchronously gets a network that shows the connections between a set
    /// of Twitter users who were specified by either a Twitter List name or a
    /// set of Twitter screen names.
    /// </summary>
    ///
    /// <param name="useListName">
    /// If true, <paramref name="listName" /> must be specified and <paramref
    /// name="screenNames" /> is ignored.  If false, <paramref
    /// name="screenNames" /> must be specified and <paramref
    /// name="listName" /> is ignored.
    /// </param>
    ///
    /// <param name="listName">
    /// Twitter List name if <paramref name="useListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="screenNames">
    /// Zero or more Twitter screen names if <paramref name="useListName" /> is
    /// false; unused otherwise.  The screen names can be in any case, but they
    /// get converted to lower case before getting the network.
    /// </param>
    ///
    /// <param name="networkType">
    /// The type of network to get.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetNetwork
    (
        Boolean useListName,
        String listName,
        ICollection<String> screenNames,
        NetworkType networkType,
        Boolean expandStatusUrls
    )
    {
        Debug.Assert( !useListName || !String.IsNullOrEmpty(listName) );
        Debug.Assert(useListName || screenNames != null);
        AssertValid();

        return ( GetNetworkInternal(useListName, listName, screenNames,
            networkType, expandStatusUrls) );
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <summary>
    /// Gets the requested network.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eNetworkType">
    /// The type of network to get.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        NetworkType eNetworkType,
        Boolean bExpandStatusUrls
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        AssertValid();

        BeforeGetNetwork();

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument();
        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetNetworkInternal(bUseListName, sListName, oScreenNames,
                eNetworkType, bExpandStatusUrls, oRequestStatistics,
                oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        String sNetworkTitle =
            "Twitter Users " + (bUseListName ? sListName : "Usernames");

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics, 

            GetNetworkDescription(bUseListName, sListName, oScreenNames,
                eNetworkType, bExpandStatusUrls),

            sNetworkTitle, sNetworkTitle
            );

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <summary>
    /// Gets the requested network and stores it in a provided
    /// GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eNetworkType">
    /// The type of network to get.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
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
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        NetworkType eNetworkType,
        Boolean bExpandStatusUrls,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // The key is the Twitter user ID and the value is the corresponding
        // TwitterUser.

        Dictionary<String, TwitterUser> oUserIDDictionary =
            new Dictionary<String, TwitterUser>();

        // Append vertices for the users who were specified by the caller,
        // vertices for the users replied to and mentioned in those users'
        // recent statuses, and edges for those recent statuses.

        GetBasicNetwork(bUseListName, sListName, oScreenNames,
            bExpandStatusUrls, oRequestStatistics, oUserIDDictionary,
            oGraphMLXmlDocument);

        if (eNetworkType == NetworkType.BasicPlusFollows)
        {
            // Append vertices and edges for the users who are friends and
            // followers of the users who were specified by the caller.

            AppendFriendsAndFollowers(
                oRequestStatistics, oUserIDDictionary, oGraphMLXmlDocument);
        }
    }

    //*************************************************************************
    //  Method: GetBasicNetwork()
    //
    /// <summary>
    /// Appends vertices for the users who were specified by the caller,
    /// vertices for the users replied to and mentioned in those users' recent
    /// statuses, and edges for those recent statuses.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    GetBasicNetwork
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        Boolean bExpandStatusUrls,
        RequestStatistics oRequestStatistics,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        ReportProgress("Getting information about the specified users.");

        // Append a vertex for each user specified by the caller.

        foreach ( Dictionary<String, Object> oUserValueDictionary in

            EnumerateSpecifiedUsers(bUseListName, sListName, oScreenNames,
                oRequestStatistics) )
        {
            AppendSpecifiedUserVertexXmlNode(oUserValueDictionary,
                bExpandStatusUrls, oRequestStatistics, oUserIDDictionary,
                oGraphMLXmlDocument);
        }

        // Append vertices for the users replied to and mentioned in the
        // users' recent statuses.

        ReportProgress("Getting the specified users' recent tweets.");

        AppendRepliesToAndMentionsVertexXmlNodes(oRequestStatistics,
            oUserIDDictionary, oGraphMLXmlDocument);

        // Append edges for those recent statuses.

        AppendRepliesToAndMentionsEdgeXmlNodes(

            oGraphMLXmlDocument, oUserIDDictionary.Values,

            TwitterGraphMLUtil.TwitterUsersToUniqueScreenNames(
                oUserIDDictionary.Values),

            true, true, true, true);
    }

    //*************************************************************************
    //  Method: AppendFriendsAndFollowers()
    //
    /// <summary>
    /// Appends vertices and edges for the users who are friends and followers
    /// of the users who were specified by the caller.
    /// </summary>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    AppendFriendsAndFollowers
    (
        RequestStatistics oRequestStatistics,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        AppendFriendsOrFollowers(true, oRequestStatistics,
            oUserIDDictionary, oGraphMLXmlDocument);

        AppendFriendsOrFollowers(false, oRequestStatistics,
            oUserIDDictionary, oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: AppendSpecifiedUserVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node for a user specified by the caller.
    /// </summary>
    ///
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    AppendSpecifiedUserVertexXmlNode
    (
        Dictionary<String, Object> oUserValueDictionary,
        Boolean bExpandStatusUrls,
        RequestStatistics oRequestStatistics,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oUserValueDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        String sScreenName, sUserID;

        if ( TryGetScreenNameAndUserIDFromDictionary(oUserValueDictionary,
                out sScreenName, out sUserID) )
        {
            // Append a vertex for the user.

            TwitterUser oTwitterUser;

            TryAppendVertexXmlNode(sScreenName, sUserID, true,
                oGraphMLXmlDocument, oUserIDDictionary, oUserValueDictionary,
                out oTwitterUser);

            // Get the user's recent statuses and store them in the user's
            // TwitterUser object.

            GetRecentStatuses(sUserID, oTwitterUser, bExpandStatusUrls,
                oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node for one person in the network.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The user's screen name.
    /// </param>
    ///
    /// <param name="sUserID">
    /// The user's ID.
    /// </param>
    ///
    /// <param name="bIsSpecifiedUser">
    /// true if the user was specified by the caller.
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
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    /// 
    /// <param name="oTwitterUser">
    /// Where the TwitterUser that represents the user gets stored.  This gets
    /// set regardless of whether the node already exists.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex XML node was added, false if a vertex XML node already
    /// exists.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendVertexXmlNode
    (
        String sScreenName,
        String sUserID,
        Boolean bIsSpecifiedUser,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        Dictionary<String, Object> oUserValueDictionary,
        out TwitterUser oTwitterUser
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oUserValueDictionary != null);
        AssertValid();

        if( TwitterGraphMLUtil.TryAppendVertexXmlNode(sScreenName, sUserID,
            oGraphMLXmlDocument, oUserIDDictionary, out oTwitterUser) )
        {
            AppendUserInformationFromValueDictionary(oUserValueDictionary,
                oGraphMLXmlDocument, oTwitterUser, true, false, false);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oTwitterUser.VertexXmlNode, VertexIsSpecifiedUserID,
                bIsSpecifiedUser ? "Yes" : "No");

            SetIsSpecifiedUser(oTwitterUser, bIsSpecifiedUser);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsVertexXmlNodes()
    //
    /// <summary>
    /// Appends vertex XML nodes for the users replied to and mentioned in the
    /// specified users' recent statuses.
    /// </summary>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    AppendRepliesToAndMentionsVertexXmlNodes
    (
        RequestStatistics oRequestStatistics,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // This is a collection of unique screen names replied-to or mentioned
        // in the specified users' recent statuses.

        String[] asRepliesToAndMentionsScreenNames =
            GetRepliesToAndMentionsScreenNames(oUserIDDictionary.Values);

        // Get information about each such user.

        foreach ( Dictionary<String, Object> oUserValueDictionary in

            EnumerateUserValueDictionaries(asRepliesToAndMentionsScreenNames,
                false, oRequestStatistics) )
        {
            String sScreenName, sUserID;

            if ( TryGetScreenNameAndUserIDFromDictionary(oUserValueDictionary,
                    out sScreenName, out sUserID) )
            {
                // Append a vertex for the user if one doesn't already exist.

                TwitterUser oTwitterUser;

                TryAppendVertexXmlNode(sScreenName, sUserID, false,
                    oGraphMLXmlDocument, oUserIDDictionary,
                    oUserValueDictionary, out oTwitterUser);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendFriendsOrFollowers()
    //
    /// <summary>
    /// Appends vertices and edges for the users who are friends or followers
    /// of the users who were specified by the caller.
    /// </summary>
    ///
    /// <param name="bAppendFriends">
    /// true to append the friends, false to append the followers.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    AppendFriendsOrFollowers
    (
        Boolean bAppendFriends,
        RequestStatistics oRequestStatistics,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // Get the screen names of the users who were specified by the caller.

        foreach ( String sScreenName in
            GetScreenNames(oUserIDDictionary, true) )
        {
            // Get the user's friends or followers.

            foreach ( Dictionary<String, Object> oUserValueDictionary in

                EnumerateUserValueDictionariesForFriendsOrFollowers(
                    sScreenName, bAppendFriends, oRequestStatistics) )
            {
                String sFriendOrFollowerScreenName, sFriendOrFollowerUserID;

                if ( TryGetScreenNameAndUserIDFromDictionary(
                    oUserValueDictionary, out sFriendOrFollowerScreenName,
                    out sFriendOrFollowerUserID) )
                {
                    // Append a vertex for the friend or follower if he is not
                    // already in the network.

                    TwitterUser oFriendOrFollower;

                    TryAppendVertexXmlNode(sFriendOrFollowerScreenName,
                        sFriendOrFollowerUserID, false, oGraphMLXmlDocument,
                        oUserIDDictionary, oUserValueDictionary,
                        out oFriendOrFollower);

                    AppendFriendOrFollowerEdgeXmlNode(sScreenName,
                        sFriendOrFollowerScreenName, bAppendFriends,
                        oGraphMLXmlDocument, oRequestStatistics);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: GetRecentStatuses()
    //
    /// <summary>
    /// Gets a user's recent statuses and stores them in the user's TwitterUser
    /// object.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// The user's ID.
    /// </param>
    ///
    /// <param name="oTwitterUser">
    /// Represents the user.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    GetRecentStatuses
    (
        String sUserID,
        TwitterUser oTwitterUser,
        Boolean bExpandStatusUrls,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oTwitterUser != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        foreach ( Dictionary<String, Object> oStatusValueDictionary in
            EnumerateRecentStatuses(sUserID, oRequestStatistics) )
        {
            TwitterStatus oTwitterStatus;

            if ( TwitterStatus.TryFromStatusValueDictionary(
                oStatusValueDictionary, bExpandStatusUrls,
                out oTwitterStatus) )
            {
                oTwitterUser.Statuses.Add(oTwitterStatus);
            }
        }
    }

    //*************************************************************************
    //  Method: GetRepliesToAndMentionsScreenNames()
    //
    /// <summary>
    /// Gets all the unique screen names that were replied-to or mentioned
    /// in all the specified users' recent statuses.
    /// </summary>
    ///
    /// <param name="oTwitterUsers">
    /// Collection of TwitterUser objects.
    /// </param>
    ///
    /// <returns>
    /// A collection of all the unique screen names, in lower case.
    /// </returns>
    //*************************************************************************

    protected String[]
    GetRepliesToAndMentionsScreenNames
    (
        IEnumerable<TwitterUser> oTwitterUsers
    )
    {
        Debug.Assert(oTwitterUsers != null);
        AssertValid();

        HashSet<String> oUniqueScreenNames = new HashSet<String>();

        TwitterStatusTextParser oTwitterStatusTextParser =
            new TwitterStatusTextParser();

        foreach ( TwitterStatus oTwitterStatus in
            EnumerateTwitterStatuses(oTwitterUsers) )
        {
            String sRepliedToScreenName;
            String [] asUniqueMentionedScreenNames;

            oTwitterStatusTextParser.GetScreenNames(oTwitterStatus.Text,
                out sRepliedToScreenName,
                out asUniqueMentionedScreenNames);

            if ( !String.IsNullOrEmpty(sRepliedToScreenName) )
            {
                oUniqueScreenNames.Add(sRepliedToScreenName);
            }

            foreach (String sUniqueMentionedScreenName in
                asUniqueMentionedScreenNames)
            {
                oUniqueScreenNames.Add(sUniqueMentionedScreenName);
            }
        }

        return ( oUniqueScreenNames.ToArray() );
    }

    //*************************************************************************
    //  Method: GetFriendOrFollowerUserIDs()
    //
    /// <summary>
    /// Gets the IDs of the friends or followers of a user.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name of the user.
    /// </param>
    ///
    /// <param name="bGetFriendUserIDs">
    /// true to get the friend user IDs, false to get the follower user IDs.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// An array of user IDs.
    /// </returns>
    //*************************************************************************

    protected String[]
    GetFriendOrFollowerUserIDs
    (
        String sScreenName,
        Boolean bGetFriendUserIDs,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        List<String> oFriendOrFollowerIDs = new List<String>();

        ReportProgressForFriendsOrFollowers(sScreenName, bGetFriendUserIDs);

        foreach ( String sFriendOrFollowerID in

            EnumerateFriendOrFollowerIDs(sScreenName, bGetFriendUserIDs,
                MaximumFriendsOrFollowers, oRequestStatistics) )
        {
            oFriendOrFollowerIDs.Add(sFriendOrFollowerID);
        }

        return ( oFriendOrFollowerIDs.ToArray() );
    }

    //*************************************************************************
    //  Method: EnumerateSpecifiedUsers()
    //
    /// <summary>
    /// Enumerates the users who were specified by the caller.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// A user value dictionary is returned by each call until all the basic
    /// users have been enumerated.
    /// </returns>
    //*************************************************************************

    protected IEnumerable< Dictionary<String, Object> >
    EnumerateSpecifiedUsers
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        if (bUseListName)
        {
            return ( EnumerateUserValueDictionariesInList(
                sListName, oRequestStatistics) );
        }

        return ( EnumerateUserValueDictionaries(
            RemoveDuplicateScreenNames(oScreenNames), false,
            oRequestStatistics) );
    }

    //*************************************************************************
    //  Method: EnumerateRecentStatuses()
    //
    /// <summary>
    /// Enumerates the recent statuses from one user.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// The user's ID.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// A status value dictionary is returned by each call until all the recent
    /// statuses have been enumerated.
    /// </returns>
    //*************************************************************************

    protected IEnumerable< Dictionary<String, Object> >
    EnumerateRecentStatuses
    (
        String sUserID,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // Note:
        //
        // The EnumerateJsonValues() call below assumes that the API being
        // called uses Twitter's cursor scheme.  GET statuses/user_timeline
        // actually uses a "max_id" scheme, but as of March 2014 there is no
        // method in NodeXL to accommodate that.  The EnumerateXX() methods in
        // TwitterUtil eventually need to be rewritten to accommodate all of
        // Twitter's paging schemes (there are at least three), but for now,
        // limiting the number of requested statuses to the maximum number that
        // GET statuses/user_timeline will return in one page (200) eliminates
        // the need for paging.

        Debug.Assert(MaximumRecentStatuses <= 200);

        String sUrl = String.Format(

            "{0}statuses/user_timeline.json?user_id={1}&count={2}"
            ,
            TwitterApiUrls.Rest,
            TwitterUtil.EncodeUrlParameter(sUserID),
            MaximumRecentStatuses
            );

        // The JSON contains an array of status value dictionaries.

        foreach ( Object oResult in EnumerateJsonValues(sUrl, null,
            MaximumRecentStatuses, true, oRequestStatistics) )
        {
            yield return ( ( Dictionary<String, Object> )oResult );
        }
    }

    //*************************************************************************
    //  Method: EnumerateUserValueDictionariesInList()
    //
    /// <summary>
    /// Enumerates the user value dictionary for each user in a Twitter List.
    /// </summary>
    ///
    /// <param name="sListName">
    /// Twitter List name.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The enumerated user value dictionaries are returned one-by-one.
    /// </returns>
    //*************************************************************************

    public IEnumerable< Dictionary<String, Object> >
    EnumerateUserValueDictionariesInList
    (
        String sListName,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sListName) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        String sSlug, sOwnerScreenName;

        if ( !TwitterListNameParser.TryParseListName(sListName,
            out sSlug, out sOwnerScreenName) )
        {
            yield break;
        }

        String sUrl = String.Format(

            "{0}lists/members.json?slug={1}&owner_screen_name={2}&{3}"
            ,
            TwitterApiUrls.Rest,
            TwitterUtil.EncodeUrlParameter(sSlug),
            TwitterUtil.EncodeUrlParameter(sOwnerScreenName),
            TwitterApiUrlParameters.IncludeEntities
            );

        // The JSON contains a "users" array for the users in the Twitter list.

        foreach ( Object oResult in EnumerateJsonValues(sUrl, "users",
            Int32.MaxValue, false, oRequestStatistics) )
        {
            yield return ( ( Dictionary<String, Object> )oResult );
        }
    }

    //*************************************************************************
    //  Method: EnumerateUserValueDictionariesForFriendsOrFollowers()
    //
    /// <summary>
    /// Enumerates the user value dictionary for a user's friends or followers.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name of the user.
    /// </param>
    ///
    /// <param name="bGetFriends">
    /// true to enumerate the friends, false to enumerate the followers.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The enumerated user value dictionaries are returned one-by-one.
    /// </returns>
    //*************************************************************************

    protected IEnumerable< Dictionary<String, Object> >
    EnumerateUserValueDictionariesForFriendsOrFollowers
    (
        String sScreenName,
        Boolean bGetFriends,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        return ( EnumerateUserValueDictionaries(

            GetFriendOrFollowerUserIDs(sScreenName, bGetFriends,
                oRequestStatistics),

            true, oRequestStatistics) );
    }

    //*************************************************************************
    //  Method: EnumerateTwitterStatuses()
    //
    /// <summary>
    /// Enumerates through all the TwitterStatus objects stored in a collection
    /// of TwitterUser objects.
    /// </summary>
    ///
    /// <param name="oTwitterUsers">
    /// Collection of TwitterUser objects.
    /// </param>
    //*************************************************************************

    protected IEnumerable<TwitterStatus>
    EnumerateTwitterStatuses
    (
        IEnumerable<TwitterUser> oTwitterUsers
    )
    {
        Debug.Assert(oTwitterUsers != null);
        AssertValid();

        foreach (TwitterUser oTwitterUser in oTwitterUsers)
        {
            foreach (TwitterStatus oTwitterStatus in oTwitterUser.Statuses)
            {
                yield return (oTwitterStatus);
            }
        }
    }

    //*************************************************************************
    //  Method: RemoveDuplicateScreenNames()
    //
    /// <summary>
    /// Eliminates duplicate screen names from a collection.
    /// </summary>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names.  May contain duplicates, and the
    /// screen names can use any combination of lower and upper case.
    /// </param>
    ///
    /// <returns>
    /// An array of unique screen names, all in lower case.
    /// </returns>
    //*************************************************************************

    protected String[]
    RemoveDuplicateScreenNames
    (
        ICollection<String> oScreenNames
    )
    {
        Debug.Assert(oScreenNames != null);
        AssertValid();

        HashSet<String> oUniqueScreenNames = new HashSet<String>();

        foreach (String sScreenName in oScreenNames)
        {
            oUniqueScreenNames.Add( sScreenName.ToLower() );
        }

        return ( oUniqueScreenNames.ToArray() );
    }

    //*************************************************************************
    //  Method: GetScreenNames()
    //
    /// <summary>
    /// Gets the screen names for a set of users already in the network.
    /// </summary>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="bSpecifiedUsers">
    /// true to get the screen names that were specified by the caller, false
    /// to get all other screen names in the network.
    /// </param>
    ///
    /// <returns>
    /// An array of screen names.
    /// </returns>
    //*************************************************************************

    protected String[]
    GetScreenNames
    (
        Dictionary<String, TwitterUser> oUserIDDictionary,
        Boolean bSpecifiedUsers
    )
    {
        Debug.Assert(oUserIDDictionary != null);
        AssertValid();

        List<String> oUserScreenNames = new List<String>();

        foreach (TwitterUser oTwitterUser in oUserIDDictionary.Values)
        {
            if (GetIsSpecifiedUser(oTwitterUser) == bSpecifiedUsers)
            {
                oUserScreenNames.Add(oTwitterUser.ScreenName);
            }
        }

        return ( oUserScreenNames.ToArray() );
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Twitter users.
    /// </summary>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Twitter users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument()
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        TwitterGraphMLUtil.DefineVertexStatisticsGraphMLAttributes(
            oGraphMLXmlDocument);

        oGraphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexIsSpecifiedUserID, "User of Interest?");

        TwitterGraphMLUtil.DefineCommonGraphMLAttributes(oGraphMLXmlDocument);

        TwitterGraphMLUtil.DefineEdgeStatusGraphMLAttributes(
            oGraphMLXmlDocument);

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkDescription()
    //
    /// <summary>
    /// Gets a description of the network.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eNetworkType">
    /// The type of network to get.
    /// </param>
    ///
    /// <param name="bExpandStatusUrls">
    /// true to expand the URLs in the statuses.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        NetworkType eNetworkType,
        Boolean bExpandStatusUrls
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        AssertValid();

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();

        if (bUseListName)
        {
            oNetworkDescriber.AddSentence(

                "The graph represents the network of Twitter users in the"
                + " Twitter List \"{0}\"."
                ,
                sListName
                );
        }
        else
        {
            Int32 iScreenNames = oScreenNames.Count;

            oNetworkDescriber.AddSentence(

                "The graph represents a network of {0} specified Twitter"
                + " {1}."
                ,
                iScreenNames,
                StringUtil.MakePlural("user", iScreenNames)
                );
        }

        oNetworkDescriber.AddNetworkTime(NetworkSource);
        oNetworkDescriber.StartNewParagraph();

        String sNetworkType = 

            "It shows who was mentioned or replied to in the users' recent"
            + " tweets";

        switch (eNetworkType)
        {
            case TwitterUsersNetworkAnalyzer.NetworkType.Basic:

                sNetworkType += ".";
                break;

            case TwitterUsersNetworkAnalyzer.NetworkType.BasicPlusFollows:

                sNetworkType +=
                    ", along with some of the follow relationships.";

                break;

            default:

                Debug.Assert(false);
                break;
        }

        Debug.Assert( !String.IsNullOrEmpty(sNetworkType) );
        oNetworkDescriber.AddSentence(sNetworkType);

        return ( oNetworkDescriber.ConcatenateSentences() );
    }

    //*************************************************************************
    //  Method: SetIsSpecifiedUser()
    //
    /// <summary>
    /// Sets the "is specified user" flag on a TwitterUser object.
    /// </summary>
    ///
    /// <param name="oTwitterUser">
    /// Object to set the flag on.
    /// </param>
    ///
    /// <param name="bIsSpecifiedUser">
    /// true if the user was specified by the caller.
    /// </param>
    //*************************************************************************

    protected void
    SetIsSpecifiedUser
    (
        TwitterUser oTwitterUser,
        Boolean bIsSpecifiedUser
    )
    {
        Debug.Assert(oTwitterUser != null);
        AssertValid();

        oTwitterUser.Tag = bIsSpecifiedUser;
    }

    //*************************************************************************
    //  Method: GetIsSpecifiedUser()
    //
    /// <summary>
    /// Gets the "is specified user" flag on a TwitterUser object.
    /// </summary>
    ///
    /// <param name="oTwitterUser">
    /// Object to get the flag from.
    /// </param>
    ///
    /// <returns>
    /// true if the user was specified by the caller.
    /// </returns>
    //*************************************************************************

    protected Boolean
    GetIsSpecifiedUser
    (
        TwitterUser oTwitterUser
    )
    {
        Debug.Assert(oTwitterUser != null);
        AssertValid();

        Debug.Assert(oTwitterUser.Tag is Boolean);

        return ( (Boolean)oTwitterUser.Tag );
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
                oGetNetworkAsyncArgs.UseListName,
                oGetNetworkAsyncArgs.ListName,
                oGetNetworkAsyncArgs.ScreenNames,
                oGetNetworkAsyncArgs.NetworkType,
                oGetNetworkAsyncArgs.ExpandStatusUrls
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
    //  Protected GraphML-attribute IDs
    //*************************************************************************

    /// Vertex attribute id for indicating whether a vertex represents one of
    /// the users specified by the caller.

    protected const String VertexIsSpecifiedUserID = "IsSpecifiedUser";


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    // These limits are arbitrary and may need to be adjusted.

    /// Maximum number of recent statuses to request for each specified user.

    public const Int32 MaximumRecentStatuses = 100;

    /// Maximum number of friend or follower IDs to request for each user.

    public const Int32 MaximumFriendsOrFollowers = 1000;


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
        public Boolean UseListName;
        ///
        public String ListName;
        ///
        public ICollection<String> ScreenNames;
        ///
        public NetworkType NetworkType;
        ///
        public Boolean ExpandStatusUrls;
    };
}

}
