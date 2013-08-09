
using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.DateTimeLib;
using Smrf.SocialNetworkLib;
using Smrf.SocialNetworkLib.Twitter;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterNetworkAnalyzerBase
//
/// <summary>
/// Base class for classes that analyze a Twitter network.
/// </summary>
///
/// <remarks>
/// The derived class must call <see cref="BeforeGetNetwork()" /> before
/// getting each network.
/// </remarks>
//*****************************************************************************

public abstract class TwitterNetworkAnalyzerBase : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterNetworkAnalyzerBase()
    {
        m_oTwitterUtil = null;
        m_oTwitterStatusTextParser = new TwitterStatusTextParser();

        AssertValid();
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

        String sMessage = null;

        const String TimeoutMessage =
            "The Twitter Web service didn't respond.";

        const String RefusedMessage =
            "The Twitter Web service refused to provide the requested"
            + " information."
            ;

        if (oException is WebException)
        {
            WebException oWebException = (WebException)oException;

            if ( TwitterUtil.WebExceptionIsDueToRateLimit(oWebException) )
            {
                // Note that this shouldn't actually occur, because
                // this.GetTwitterResponseAsString() pauses and retries when
                // Twitter rate limits kick in.  This "if" clause is included
                // in case Twitter misbehaves, or if the pause-retry code is
                // ever removed from GetTwitterResponseAsString().

                sMessage = String.Format(

                    RefusedMessage 
                    + "  A likely cause is that you have made too many Twitter"
                    + " requests in the last 15 minutes.  (Twitter"
                    + " limits information requests to prevent its service"
                    + " from being attacked.  Click the '{0}' link for"
                    + " details.)"
                    ,
                    TwitterRateLimitsControl.RateLimitingLinkText
                    );
            }
            else if (oWebException.Response is HttpWebResponse)
            {
                HttpWebResponse oHttpWebResponse =
                    (HttpWebResponse)oWebException.Response;

                switch (oHttpWebResponse.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:  // HTTP 401.

                        sMessage = RefusedMessage
                            + "  The stated reason was \"unauthorized.\"";

                        break;

                    case HttpStatusCode.NotFound:  // HTTP 404.

                        sMessage =
                            "There is no Twitter user with that username."
                            ;

                        break;

                    case HttpStatusCode.RequestTimeout:  // HTTP 408.

                        sMessage = TimeoutMessage;
                        break;

                    case HttpStatusCode.Forbidden:  // HTTP 403.

                        sMessage = RefusedMessage
                            + "  The stated reason was \"forbidden.\"";

                        break;

                    default:

                        break;
                }
            }
            else
            {
                switch (oWebException.Status)
                {
                    case WebExceptionStatus.Timeout:

                        sMessage = TimeoutMessage;
                        break;

                    default:

                        break;
                }
            }
        }

        if (sMessage == null)
        {
            sMessage = ExceptionUtil.GetMessageTrace(oException);
        }

        return (sMessage);
    }

    //*************************************************************************
    //  Method: BeforeGetNetwork()
    //
    /// <summary>
    /// Performs tasks required before getting a network.
    /// </summary>
    ///
    /// <remarks>
    /// The derived class must call this method before getting each network.
    /// </remarks>
    //*************************************************************************

    protected void
    BeforeGetNetwork()
    {
        AssertValid();

        // TwitterAccessToken caches the access token it reads from disk.  Make
        // sure the latest access token is read.

        TwitterAccessToken oTwitterAccessToken = new TwitterAccessToken();

        // A network should never be requested if the access token hasn't been
        // saved yet.

        String sToken, sSecret;

        if ( !oTwitterAccessToken.TryLoad(out sToken, out sSecret) )
        {
            throw new Exception("Twitter access token not set.");
        }

        m_oTwitterUtil = new TwitterUtil(sToken, sSecret,
            HttpNetworkAnalyzerBase.UserAgent,
            HttpNetworkAnalyzerBase.HttpWebRequestTimeoutMs);
    }

    //*************************************************************************
    //  Method: ReportProgressForFollowedOrFollowing()
    //
    /// <summary>
    /// Reports progress before getting the people followed by a user or the
    /// people following a user.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The user' screen name.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true if getting the people followed by the user, false if getting the
    /// people following the user.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgressForFollowedOrFollowing
    (
        String sScreenName,
        Boolean bFollowed
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        AssertValid();

        ReportProgress( String.Format(

            "Getting people {0} \"{1}\"."
            ,
            bFollowed ? "followed by" : "following",
            sScreenName
            ) );
    }

    //*************************************************************************
    //  Method: TryGetUserValueDictionary()
    //
    /// <summary>
    /// Attempts to get a dictionary of values for a specified user from
    /// Twitter.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name to get a dictionary of values for.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="bIgnoreWebAndJsonExceptions">
    /// If a WebException or JSON exception is caught and this is true, false
    /// is returned.  Otherwise, the exception is rethrown.
    /// </param>
    ///
    /// <param name="oUserValueDictionary">
    /// Where a dictionary of values for the user gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetUserValueDictionary
    (
        String sScreenName,
        RequestStatistics oRequestStatistics,
        Boolean bIgnoreWebAndJsonExceptions,
        out Dictionary<String, Object> oUserValueDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        oUserValueDictionary = null;

        String sUrl = String.Format(

            "{0}users/show.json?screen_name={1}&{2}"
            ,
            TwitterApiUrls.Rest,
            TwitterUtil.EncodeUrlParameter(sScreenName),
            TwitterApiUrlParameters.IncludeEntities
            );

        ReportProgress( String.Format(

            "Getting information about \"{0}\"."
            ,
            sScreenName
            ) );
            
        try
        {
            oUserValueDictionary = ( Dictionary<String, Object> )
                ( new JavaScriptSerializer() ).DeserializeObject(
                    GetTwitterResponseAsString(sUrl, oRequestStatistics) );

            return (true);
        }
        catch (Exception oException)
        {
            if (!HttpSocialNetworkUtil.ExceptionIsWebOrJson(oException) ||
                !bIgnoreWebAndJsonExceptions)
            {
                throw oException;
            }

            return (false);
        }
    }

    //*************************************************************************
    //  Method: GetTwitterResponseAsString()
    //
    /// <summary>
    /// Gets a response from a Twitter URL as a string.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the string from.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The string returned by the Twitter server.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    protected String
    GetTwitterResponseAsString
    (
        String sUrl,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        Debug.Assert(m_oTwitterUtil != null);

        return ( m_oTwitterUtil.GetTwitterResponseAsString(sUrl,
            oRequestStatistics, new ReportProgressHandler(this.ReportProgress),
            new CheckCancellationPendingHandler(this.CheckCancellationPending)
            ) );
    }

    //*************************************************************************
    //  Method: EnumerateJsonValues()
    //
    /// <summary>
    /// Gets a JSON response from a Twitter URL, then enumerates a specified
    /// set of values in the response.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the JSON from.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="sJsonName">
    /// If the top level of the JSON response contains a set of name/value
    /// pairs, this parameter should be the name whose value is the array of
    /// objects this method will enumerate.  If the top level of the JSON
    /// response contains an array of objects this method will enumerate, this
    /// parameter should be null. 
    /// </param>
    ///
    /// <param name="iMaximumValues">
    /// Maximum number of values to return, or Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="bSkipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped,
    /// unless they are due to rate limiting.)
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The enumerated values are returned one-by-one, as an Object.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<Object>
    EnumerateJsonValues
    (
        String sUrl,
        String sJsonName,
        Int32 iMaximumValues,
        Boolean bSkipMostPage1Errors,
        RequestStatistics oRequestStatistics
    )
    {
        // Note:
        //
        // The logic in this method is similar to the logic in
        // TwitterUtil.EnumerateSearchStatuses().  In fact, at one time all
        // enumeration was done through this EnumerateJsonValues() method.
        // TwitterUtil.EnumerateSearchStatuses() was created only when
        // version 1.1 of the Twitter API introduced yet another paging scheme,
        // one that differs from the cursor scheme that this method handles.
        //
        // A possible work item is to recombine the two methods into one,
        // possibly by using a delegate to handle the different paging schemes.

        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(iMaximumValues > 0);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        Int32 iPage = 1;
        String sCursor = null;
        Int32 iObjectsEnumerated = 0;

        while (true)
        {
            if (iPage > 1)
            {
                ReportProgress("Getting page " + iPage + ".");
            }

            String sUrlWithCursor = AppendCursorToUrl(sUrl, sCursor);

            Dictionary<String, Object> oValueDictionary = null;
            Object [] aoObjectsThisPage;

            try
            {
                Object oDeserializedTwitterResponse = 
                    ( new JavaScriptSerializer() ).DeserializeObject(
                        GetTwitterResponseAsString(sUrlWithCursor,
                        oRequestStatistics) );

                Object oObjectsThisPageAsObject;

                if (sJsonName == null)
                {
                    // The top level of the Json response contains an array of
                    // objects this method will enumerate.

                    oObjectsThisPageAsObject = oDeserializedTwitterResponse;
                }
                else
                {
                    // The top level of the Json response contains a set of
                    // name/value pairs.  The value for the specified name is
                    // the array of objects this method will enumerate.

                    oValueDictionary = ( Dictionary<String, Object> )
                        oDeserializedTwitterResponse;

                    oObjectsThisPageAsObject = oValueDictionary[sJsonName];
                }

                aoObjectsThisPage = ( Object [] )oObjectsThisPageAsObject;
            }
            catch (Exception oException)
            {
                // Rethrow the exception if appropriate.

                TwitterUtil.OnExceptionWhileEnumeratingJsonValues(
                    oException, iPage, bSkipMostPage1Errors);

                // Otherwise, just halt the enumeration.

                yield break;
            }

            Int32 iObjectsThisPage = aoObjectsThisPage.Length;

            if (iObjectsThisPage == 0)
            {
                yield break;
            }

            for (Int32 i = 0; i < iObjectsThisPage; i++)
            {
                yield return ( aoObjectsThisPage[i] );

                iObjectsEnumerated++;

                if (iObjectsEnumerated == iMaximumValues)
                {
                    yield break;
                }
            }

            iPage++;

            // When the top level of the Json response contains a set of
            // name/value pairs, a next_cursor_str value of "0" means "end of
            // data."

            if (
                oValueDictionary == null
                ||
                !TryGetJsonValueFromDictionary(oValueDictionary,
                    "next_cursor_str", out sCursor)
                ||
                sCursor == "0"
                )
            {
                yield break;
            }

            // Get the next page...
        }
    }

    //*************************************************************************
    //  Method: EnumerateFriendOrFollowerIDs()
    //
    /// <summary>
    /// Enumerates through a collection of the user IDs of the friends or
    /// followers of a specified user.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The screen name to get the friends or followers for.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to get the followed IDs, false to get the follower IDs.
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
    /// <returns>
    /// The enumerated IDs are returned one-by-one, as a string.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<String>
    EnumerateFriendOrFollowerIDs
    (
        String sScreenName,
        Boolean bFollowed,
        Int32 iMaximumPeoplePerRequest,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        String sUrl = String.Format(

            "{0}{1}.json?screen_name={2}"
            ,
            TwitterApiUrls.Rest,
            bFollowed ? "friends/ids" : "followers/ids",
            TwitterUtil.EncodeUrlParameter(sScreenName)
            );

        // The JSON looped through here has an "ids" name whose value is an
        // array of integer IDs.

        foreach ( Object oUserIDAsObject in EnumerateJsonValues(
            sUrl, "ids", iMaximumPeoplePerRequest, true, oRequestStatistics) )
        {
            String sUserID;

            if ( TwitterJsonUtil.TryConvertJsonValueToString(
                oUserIDAsObject, out sUserID) )
            {
                yield return (sUserID);
            }
        }
    }

    //*************************************************************************
    //  Method: EnumerateUserValueDictionaries()
    //
    /// <summary>
    /// Enumerates through a collection of dictionaries of values for a
    /// collection of specified users.
    /// </summary>
    ///
    /// <param name="asUserIDsOrScreenNames">
    /// Array of user IDs or screen names to get user value dictionaries for.
    /// </param>
    ///
    /// <param name="bUserIDsSpecified">
    /// true if <paramref name="asUserIDsOrScreenNames" /> contains IDs, false
    /// if it contains screen names.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The enumerated values are returned one-by-one, as a dictionary.
    /// </returns>
    ///
    /// <remarks>
    /// For each user ID or screen name in <paramref
    /// name="asUserIDsOrScreenNames" />, this method gets information about
    /// the user from Twitter and returns it as a dictionary of values.  The
    /// order of the returned values is not necessarily the same as the order
    /// of the user IDs or screen names.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable< Dictionary<String, Object> >
    EnumerateUserValueDictionaries
    (
        String [] asUserIDsOrScreenNames,
        Boolean bUserIDsSpecified,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(asUserIDsOrScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // We'll use Twitter's users/lookup API, which gets extended
        // information for up to 100 users in one call.

        Int32 iUsers = asUserIDsOrScreenNames.Length;
        Int32 iUsersProcessed = 0;
        Int32 iCalls = 0;

        while (iUsersProcessed < iUsers)
        {
            // For each call, ask for information about as many users as
            // possible until either 100 is reached or the URL reaches an
            // arbitrary maximum length.  Twitter recommends using a POST here
            // (without specifying why), but it would require revising the
            // base-class HTTP calls and isn't worth the trouble.

            Int32 iUsersProcessedThisCall = 0;

            StringBuilder oUrl = new StringBuilder();

            oUrl.AppendFormat(
                "{0}users/lookup.json?{1}&{2}="
                ,
                TwitterApiUrls.Rest,
                TwitterApiUrlParameters.IncludeEntities,
                bUserIDsSpecified ? "user_id" : "screen_name"
                );

            const Int32 MaxUsersPerCall = 100;
            const Int32 MaxUrlLength = 2000;

            // Construct the URL for this call.

            while (
                iUsersProcessed < iUsers
                &&
                iUsersProcessedThisCall < MaxUsersPerCall
                &&
                oUrl.Length < MaxUrlLength
                )
            {
                if (iUsersProcessedThisCall > 0)
                {
                    // Append an encoded comma.  Using an unencoded comma 
                    // causes Twitter to return a 401 "unauthorized" error.
                    //
                    // See this post for an explanation:
                    //
                    // https://dev.twitter.com/discussions/11399

                    oUrl.Append("%2C");
                }

                oUrl.Append( asUserIDsOrScreenNames[iUsersProcessed] );
                iUsersProcessed++;
                iUsersProcessedThisCall++;
            }

            iCalls++;

            if (iCalls > 1)
            {
                ReportProgress("Getting page " + iCalls + ".");
            }

            foreach ( Object oResult in EnumerateJsonValues(oUrl.ToString(),
                null, Int32.MaxValue, true, oRequestStatistics) )
            {
                yield return ( ( Dictionary<String, Object> )oResult );
            }
        }
    }

    //*************************************************************************
    //  Method: AppendCursorToUrl()
    //
    /// <summary>
    /// Appends a cursor to a Twitter URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to append to.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="sCursor">
    /// The cursor to append, or null to not append a cursor.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sUrl" /> with a cursor appended to it if requested.
    /// </returns>
    //*************************************************************************

    protected String
    AppendCursorToUrl
    (
        String sUrl,
        String sCursor
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        AssertValid();

        if (sCursor == null)
        {
            return (sUrl);
        }

        return ( String.Format(
            
            "{0}{1}cursor={2}"
            ,
            sUrl,
            sUrl.IndexOf('?') == -1 ? '?' : '&',
            sCursor
            ) );
    }

    //*************************************************************************
    //  Method: TryGetJsonValueFromDictionary()
    //
    /// <summary>
    /// Attempts to get a non-empty string value from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="oValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.
    /// </param>
    ///
    /// <param name="sName">
    /// The name of the value to get.
    /// </param>
    ///
    /// <param name="sValue">
    /// Where the non-empty value gets stored if true is returned.  If false is
    /// returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the non-empty value was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to get the specified value, which can be of any
    /// JSON type, from <paramref name="oValueDictionary" />.  If the value is
    /// found, this method converts it to a string, stores it at <paramref
    /// name="sValue" />, and returns true.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetJsonValueFromDictionary
    (
        Dictionary<String, Object> oValueDictionary,
        String sName,
        out String sValue
    )
    {
        Debug.Assert(oValueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        AssertValid();

        return ( TwitterJsonUtil.TryGetJsonValueFromDictionary(
            oValueDictionary, sName, out sValue) );
    }

    //*************************************************************************
    //  Method: TryGetUserIDFromDictionary()
    //
    /// <summary>
    /// Attempts to get a user ID from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    ///
    /// <param name="sUserID">
    /// Where the user ID gets stored if true is returned.  If false is
    /// returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the user ID was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetUserIDFromDictionary
    (
        Dictionary<String, Object> oUserValueDictionary,
        out String sUserID
    )
    {
        Debug.Assert(oUserValueDictionary != null);
        AssertValid();

        return ( TryGetJsonValueFromDictionary(oUserValueDictionary, "id_str",
            out sUserID) );
    }

    //*************************************************************************
    //  Method: TryGetScreenNameFromDictionary()
    //
    /// <summary>
    /// Attempts to get a user's screen name from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    ///
    /// <param name="sScreenName">
    /// Where the user's screen name gets stored if true is returned.  If false
    /// is returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the user's screen name was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetScreenNameFromDictionary
    (
        Dictionary<String, Object> oUserValueDictionary,
        out String sScreenName
    )
    {
        Debug.Assert(oUserValueDictionary != null);
        AssertValid();

        return ( TryGetJsonValueFromDictionary(oUserValueDictionary,
            "screen_name", out sScreenName) );
    }

    //*************************************************************************
    //  Method: AppendUserInformationFromValueDictionary()
    //
    /// <summary>
    /// Appends GraphML-Attribute values from a user value dictionary returned
    /// by Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about the user.
    /// </param>
    /// 
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oTwitterUser">
    /// Contains the vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute values to.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include the user's statistics as GraphML-Attribute values.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include the user's latest status as a GraphML-Attribute value.
    /// </param>
    ///
    /// <param name="bExpandLatestStatusUrls">
    /// true to expand all URLs in the latest status that might be shortened
    /// URLs.
    /// </param>
    ///
    /// <remarks>
    /// This method reads information from a value dictionary returned by
    /// Twitter and appends the information to a vertex XML node in the GraphML
    /// document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendUserInformationFromValueDictionary
    (
        Dictionary<String, Object> oUserValueDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument,
        TwitterUser oTwitterUser,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus,
        Boolean bExpandLatestStatusUrls
    )
    {
        Debug.Assert(oUserValueDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterUser != null);
        AssertValid();

        TwitterGraphMLUtil.AppendCommonUserInformationFromValueDictionary(
            oUserValueDictionary, oGraphMLXmlDocument, oTwitterUser);

        if (bIncludeStatistics)
        {
            TwitterGraphMLUtil.AppendUserStatisticsFromValueDictionary(
                oUserValueDictionary, oGraphMLXmlDocument, oTwitterUser);
        }

        // Process the user's latest status if requested.

        Object oStatusValueDictionaryAsObject;

        if (
            bIncludeLatestStatus
            &&
            oUserValueDictionary.TryGetValue("status",
                out oStatusValueDictionaryAsObject)
            &&
            oStatusValueDictionaryAsObject is Dictionary<String, Object>
            )
        {
            Dictionary<String, Object> oStatusValueDictionary =
                ( Dictionary<String, Object> )oStatusValueDictionaryAsObject;

            AppendLatestStatusInformationFromValueDictionary(
                oStatusValueDictionary, oGraphMLXmlDocument, oTwitterUser,
                bIncludeLatestStatus, bExpandLatestStatusUrls);
        }
    }

    //*************************************************************************
    //  Method: AppendLatestStatusInformationFromValueDictionary()
    //
    /// <summary>
    /// Appends GraphML-Attribute values for a user's latest status from a
    /// value dictionary returned by Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="oStatusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about the user's status.
    /// </param>
    /// 
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oTwitterUser">
    /// Contains the vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute values to.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include the user's latest status as a GraphML-Attribute value.
    /// </param>
    ///
    /// <param name="bExpandLatestStatusUrls">
    /// true to expand all URLs in the latest status that might be shortened
    /// URLs.
    /// </param>
    ///
    /// <remarks>
    /// This method reads latest status information from a value dictionary
    /// returned by Twitter and appends the information to a vertex XML node in
    /// the GraphML document.  It also adds the latest status to the status
    /// collection of the <paramref name="oTwitterUser" /> object.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendLatestStatusInformationFromValueDictionary
    (
        Dictionary<String, Object> oStatusValueDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument,
        TwitterUser oTwitterUser,
        Boolean bIncludeLatestStatus,
        Boolean bExpandLatestStatusUrls
    )
    {
        Debug.Assert(oStatusValueDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterUser != null);
        AssertValid();

        String sID, sLatestStatus;

        if (
            !TryGetJsonValueFromDictionary(oStatusValueDictionary, "id_str",
                out sID)
            ||
            !TryGetJsonValueFromDictionary(oStatusValueDictionary, "text",
                out sLatestStatus)
            )
        {
            return;
        }

        String sLatestStatusDateUtc;

        if ( TryGetJsonValueFromDictionary(oStatusValueDictionary,
            "created_at", out sLatestStatusDateUtc) )
        {
            sLatestStatusDateUtc = TwitterDateParser.ParseTwitterDate(
                sLatestStatusDateUtc);
        }

        String sLatitude, sLongitude;

        TwitterGraphMLUtil.GetLatitudeAndLongitudeFromStatusValueDictionary(
            oStatusValueDictionary, out sLatitude, out sLongitude);

        XmlNode oVertexXmlNode = oTwitterUser.VertexXmlNode;

        String sLatestStatusUrls = null;
        String sLatestStatusHashtags = null;

        if (bIncludeLatestStatus)
        {
            // Add the status to the vertex XML node.

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oVertexXmlNode, TwitterGraphMLUtil.VertexLatestStatusID,
                sLatestStatus);

            TwitterGraphMLUtil.GetUrlsAndHashtagsFromStatusValueDictionary(
                oStatusValueDictionary, bExpandLatestStatusUrls,
                out sLatestStatusUrls, out sLatestStatusHashtags);

            if ( !String.IsNullOrEmpty(sLatestStatusUrls) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode,
                    TwitterGraphMLUtil.VertexLatestStatusUrlsID,
                    sLatestStatusUrls);

                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode,
                    TwitterGraphMLUtil.VertexLatestStatusDomainsID,
                    TwitterGraphMLUtil.UrlsToDomains(sLatestStatusUrls) ); 
            }

            if ( !String.IsNullOrEmpty(sLatestStatusHashtags) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode,
                    TwitterGraphMLUtil.VertexLatestStatusHashtagsID,
                    sLatestStatusHashtags);
            }

            if ( !String.IsNullOrEmpty(sLatestStatusDateUtc) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode,
                    TwitterGraphMLUtil.VertexLatestStatusDateUtcID,
                    sLatestStatusDateUtc);
            }

            TwitterGraphMLUtil.
                AppendLatitudeAndLongitudeGraphMLAttributeValues(
                    oGraphMLXmlDocument, oVertexXmlNode, sLatitude,
                    sLongitude);
        }

        oTwitterUser.Statuses.Add( new TwitterStatus(sID, sLatestStatus,
            sLatestStatusDateUtc, sLatitude, sLongitude, sLatestStatusUrls,
            sLatestStatusHashtags) );
    }

    //*************************************************************************
    //  Method: AppendStartTimeRelationshipDateUtcGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML attribute value for the relationship date to an edge
    /// XML node when the relationship date is the start time of the network
    /// request.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeXmlNode">
    /// The edge XML node to add the Graph-ML attribute value to.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendStartTimeRelationshipDateUtcGraphMLAttributeValue
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeXmlNode,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeXmlNode != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,

            TwitterGraphMLUtil.EdgeRelationshipDateUtcID, 

            DateTimeUtil2.ToCultureInvariantString(
                oRequestStatistics.StartTimeUtc)
            );
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNodes()
    //
    /// <overloads>
    /// Appends edge XML nodes for replies-to and mentions relationships.
    /// </overloads>
    ///
    /// <summary>
    /// Appends edge XML nodes for replies-to and mentions relationships for
    /// all statuses.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oTwitterUsers">
    /// Collection of the TwitterUsers in the network.
    /// </param>
    ///
    /// <param name="oUniqueScreenNames">
    /// Collection of the unique screen names in the network.
    /// </param>
    ///
    /// <param name="bIncludeRepliesToEdges">
    /// true to append edges for replies-to relationships.
    /// </param>
    ///
    /// <param name="bIncludeMentionsEdges">
    /// true to append edges for mentions relationships.
    /// </param>
    ///
    /// <param name="bIncludeNonRepliesToNonMentionsEdges">
    /// true to append edges for tweets that don't reply to or mention anyone.
    /// </param>
    ///
    /// <param name="bIncludeStatuses">
    /// true to include the status in the edge XML nodes.
    /// </param>
    //*************************************************************************

    protected void
    AppendRepliesToAndMentionsEdgeXmlNodes
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        IEnumerable<TwitterUser> oTwitterUsers,
        HashSet<String> oUniqueScreenNames,
        Boolean bIncludeRepliesToEdges,
        Boolean bIncludeMentionsEdges,
        Boolean bIncludeNonRepliesToNonMentionsEdges,
        Boolean bIncludeStatuses
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterUsers != null);
        Debug.Assert(oUniqueScreenNames != null);
        AssertValid();

        ReportProgress("Examining relationships.");

        TwitterGraphMLUtil.AppendRepliesToAndMentionsEdgeXmlNodes(
            oGraphMLXmlDocument, oTwitterUsers, oUniqueScreenNames,
            bIncludeRepliesToEdges, bIncludeMentionsEdges,
            bIncludeNonRepliesToNonMentionsEdges, bIncludeStatuses);
    }

    //*************************************************************************
    //  Method: AppendFollowedOrFollowingEdgeXmlNodes()
    //
    /// <overloads>
    /// Appends edge XML nodes for followed or following relationships.
    /// </overloads>
    ///
    /// <summary>
    /// Appends edge XML nodes for the followed or following relationships in
    /// an entire network.
    /// </summary>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to append followed edges, false to append following edges.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendFollowedOrFollowingEdgeXmlNodes
    (
        Dictionary<String, TwitterUser> oUserIDDictionary,
        Boolean bFollowed,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        AppendFollowedOrFollowingEdgeXmlNodes(

            TwitterGraphMLUtil.TwitterUsersToUniqueScreenNames(
                oUserIDDictionary.Values),

            oUserIDDictionary, bFollowed, iMaximumPeoplePerRequest,
            oGraphMLXmlDocument, oRequestStatistics);
    }

    //*************************************************************************
    //  Method: AppendFollowedOrFollowingEdgeXmlNodes()
    //
    /// <summary>
    /// Appends edge XML nodes for the followed or following relationships in
    /// a specified collection of screen names.
    /// </summary>
    ///
    /// <param name="oScreenNames">
    /// Collection of screen names to append edges for.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to append followed edges, false to append following edges.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendFollowedOrFollowingEdgeXmlNodes
    (
        ICollection<String> oScreenNames,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        Boolean bFollowed,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oScreenNames != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        foreach (String sScreenName in oScreenNames)
        {
            ReportProgressForFollowedOrFollowing(sScreenName, bFollowed);

            // We need to find out who are the followed (or followers) of
            // sScreenName, and see if any of them are in oUserIDDictionary,
            // which means they are in the network.

            foreach ( String sOtherUserID in EnumerateFriendOrFollowerIDs(
                sScreenName, bFollowed, iMaximumPeoplePerRequest,
                oRequestStatistics) )
            {
                TwitterUser oOtherTwitterUser;

                if ( oUserIDDictionary.TryGetValue(sOtherUserID,
                    out oOtherTwitterUser) )
                {
                    // sScreenName is a followed (or follower) of sOtherUserID.

                    AppendFollowedOrFollowingEdgeXmlNode(sScreenName,
                        oOtherTwitterUser.ScreenName, bFollowed,
                        oGraphMLXmlDocument, oRequestStatistics);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: AppendFollowedOrFollowingEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node for a followed or following relationship.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The first screen name to use.
    /// </param>
    ///
    /// <param name="sOtherScreenName">
    /// The second screen name to use.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to append a followed edge, false to append a following edge.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendFollowedOrFollowingEdgeXmlNode
    (
        String sScreenName,
        String sOtherScreenName,
        Boolean bFollowed,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert( !String.IsNullOrEmpty(sOtherScreenName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        XmlNode oEdgeXmlNode;

        if (bFollowed)
        {
            oEdgeXmlNode = NodeXLGraphMLUtil.AppendEdgeXmlNode(
                oGraphMLXmlDocument, sScreenName, sOtherScreenName,
                "Followed");
        }
        else
        {
            oEdgeXmlNode = NodeXLGraphMLUtil.AppendEdgeXmlNode(
                oGraphMLXmlDocument, sOtherScreenName, sScreenName,
                "Follower");
        }

        AppendStartTimeRelationshipDateUtcGraphMLAttributeValue(
            oGraphMLXmlDocument, oEdgeXmlNode, oRequestStatistics);
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

        // m_oTwitterUtil
        Debug.Assert(m_oTwitterStatusTextParser != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The source of Twitter networks, used in network descriptions.

    protected const String NetworkSource = "Twitter";

    /// HTTP status codes that have special meaning with Twitter.  When they
    /// occur, the requests are not retried.

    protected static readonly HttpStatusCode []
        HttpStatusCodesToFailImmediately = new HttpStatusCode[] {

            // Occurs when information about a user who has "protected" status
            // is requested, for example.

            HttpStatusCode.Unauthorized,

            // Occurs when the Twitter search API returns a tweet from a user,
            // but then refuses to provide a list of the people followed by the
            // user, probably because the user has protected her account.
            // (But if she has protected her account, why is the search API
            // returning one of her tweets?)

            HttpStatusCode.NotFound,

            // Starting with version 1.1 of the Twitter API, a single HTTP
            // status code (429, "rate limit exceeded") is used for all
            // rate-limit responses.

            (HttpStatusCode)429,

            // Not sure about what causes this one.

            HttpStatusCode.Forbidden,
        };


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Provides utility methods for getting social networks from Twitter.

    protected TwitterUtil m_oTwitterUtil;

    /// Parses the text of a Twitter tweet.  This class uses only one instance
    /// to avoid making TwitterStatusTextParser recompile all of its regular
    /// expressions.

    protected TwitterStatusTextParser m_oTwitterStatusTextParser;


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgsBase()
    //
    /// <summary>
    /// Base class for classes that contain the arguments needed to
    /// asynchronously get a Twitter network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgsBase
    {
        ///
        public Int32 MaximumPeoplePerRequest;
    };
}

}
