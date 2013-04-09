
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
        m_oTwitterAccessToken = null;
        m_oTwitterStatusParser = new TwitterStatusParser();

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

            if ( WebExceptionIsDueToRateLimit(oWebException) )
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

                        sMessage = RefusedMessage;
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

                        sMessage = RefusedMessage;
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
    //  Method: WebExceptionIsDueToRateLimit()
    //
    /// <summary>
    /// Determines whether a WebException is due to Twitter rate limits.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="oWebException" /> is due to Twitter rate limits
    /// kicking in.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WebExceptionIsDueToRateLimit
    (
        WebException oWebException
    )
    {
        Debug.Assert(oWebException != null);
        AssertValid();

        // Starting with version 1.1 of the Twitter API, a single HTTP status
        // code (429, "rate limit exceeded") is used for all rate-limit
        // responses.

        return ( WebExceptionHasHttpStatusCode(oWebException,
            (HttpStatusCode)429) );
    }

    //*************************************************************************
    //  Method: GetRateLimitPauseMs()
    //
    /// <summary>
    /// Gets the time to pause before retrying a request after Twitter rate
    /// limits kicks in.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// The time to pause before retrying a request after Twitter rate limits
    /// kick in, in milliseconds.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetRateLimitPauseMs
    (
        WebException oWebException
    )
    {
        Debug.Assert(oWebException != null);
        AssertValid();

        // The Twitter REST API provides a custom X-Rate-Limit-Reset header in
        // the response headers.  This is the time at which the request should
        // be made again, in seconds since 1/1/1970, in UTC.  If this header is
        // available, use it.  Otherwise, use a default pause time.

        WebResponse oWebResponse = oWebException.Response;

        if (oWebResponse != null)
        {
            String sXRateLimitReset =
                oWebResponse.Headers["X-Rate-Limit-Reset"];

            Int32 iSecondsSince1970;

            // (Note that Int32.TryParse() can handle null, which indicates a
            // missing header.)

            if ( Int32.TryParse(sXRateLimitReset, out iSecondsSince1970) )
            {
                DateTime oResetTimeUtc =
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).
                        AddSeconds(iSecondsSince1970);

                Double dRateLimitPauseMs =
                    (oResetTimeUtc - DateTime.UtcNow).TotalMilliseconds;

                // Don't wait longer than two hours.

                if (dRateLimitPauseMs > 0 &&
                    dRateLimitPauseMs <= 2 * 60 * 60 * 1000)
                {
                    return ( (Int32)dRateLimitPauseMs );
                }
            }
        }

        return (DefaultRateLimitPauseMs);
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

        m_oTwitterAccessToken = new TwitterAccessToken();
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Twitter users.
    /// </summary>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include each user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatuses">
    /// true to include each user's latest status.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Twitter users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatuses
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        if (bIncludeStatistics)
        {
            oGraphMLXmlDocument.DefineGraphMLAttributes(false, "int",
                FollowedID, "Followed",
                FollowersID, "Followers",
                StatusesID, "Tweets",
                FavoritesID, "Favorites",
                UtcOffsetID, "Time Zone UTC Offset (Seconds)"
                );

            oGraphMLXmlDocument.DefineVertexStringGraphMLAttributes(
                DescriptionID, "Description",
                LocationID, "Location",
                UrlID, "Web",
                TimeZoneID, "Time Zone",
                JoinedDateUtcID, "Joined Twitter Date (UTC)"
                );
        }

        if (bIncludeLatestStatuses)
        {
            oGraphMLXmlDocument.DefineVertexStringGraphMLAttributes(
                LatestStatusID, "Latest Tweet",
                LatestStatusUrlsID, "URLs in Latest Tweet",
                LatestStatusDomainsID, "Domains in Latest Tweet",
                LatestStatusHashtagsID, "Hashtags in Latest Tweet",
                LatestStatusDateUtcID, "Latest Tweet Date (UTC)"
                );

            DefineLatitudeAndLongitudeGraphMLAttributes(oGraphMLXmlDocument,
                false);
        }

        DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
        DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
        DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);

        oGraphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
            RelationshipDateUtcID, "Relationship Date (UTC)");

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: DefineLatitudeAndLongitudeGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes for latitude and longitude.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="bForEdge">
    /// true if the attributes are for edges, false if they are for vertices.
    /// </param>
    //*************************************************************************

    protected void
    DefineLatitudeAndLongitudeGraphMLAttributes
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Boolean bForEdge
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        oGraphMLXmlDocument.DefineStringGraphMLAttributes(bForEdge,
            LatitudeID, "Latitude",
            LongitudeID, "Longitude"
            );
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
            RestApiUri,
            EncodeUrlParameter(sScreenName),
            IncludeEntitiesUrlParameter
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
            if (!ExceptionIsWebOrJson(oException) ||
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

        Debug.Assert(m_oTwitterAccessToken != null);

        Int32 iRateLimitPauses = 0;

        while (true)
        {
            String sUrlToUse = sUrl;

            // Has the user authorized NodeXL to use her Twitter account?

            String sToken, sSecret;

            if ( m_oTwitterAccessToken.TryLoad(out sToken, out sSecret) )
            {
                // Yes.  Add the required authorization information to the URL.

                // Note: Don't do this outside the while (true) loop.  The
                // authorization information includes a timestamp that will
                // probably expire if the code within the catch block pauses.

                oAuthTwitter oAuthTwitter = new oAuthTwitter();
                oAuthTwitter.Token = sToken;
                oAuthTwitter.TokenSecret = sSecret;

                String sAuthorizedUrl, sAuthorizedPostData;

                oAuthTwitter.ConstructAuthWebRequest(oAuthTwitter.Method.GET,
                    sUrl, String.Empty, out sAuthorizedUrl,
                    out sAuthorizedPostData);

                sUrlToUse = sAuthorizedUrl;
            }

            Stream oStream = null;

            try
            {
                oStream = GetHttpWebResponseStreamWithRetries(sUrlToUse,
                    HttpStatusCodesToFailImmediately, oRequestStatistics,
                    null);

                return ( new StreamReader(oStream).ReadToEnd() );
            }
            catch (WebException oWebException)
            {
                if (!WebExceptionIsDueToRateLimit(oWebException) ||
                    iRateLimitPauses > 0)
                {
                    throw;
                }

                // Twitter rate limits have kicked in.  Pause and try again.

                iRateLimitPauses++;
                Int32 iRateLimitPauseMs = GetRateLimitPauseMs(oWebException);

                DateTime oWakeUpTime = DateTime.Now.AddMilliseconds(
                    iRateLimitPauseMs);

                ReportProgress( String.Format(

                    "Reached Twitter rate limits.  Pausing until {0}."
                    ,
                    oWakeUpTime.ToLongTimeString()
                    ) );

                // Don't pause in one large interval, which would prevent
                // cancellation.

                const Int32 SleepCycleDurationMs = 1000;

                Int32 iSleepCycles = (Int32)Math.Ceiling(
                    (Double)iRateLimitPauseMs / SleepCycleDurationMs) ;

                for (Int32 i = 0; i < iSleepCycles; i++)
                {
                    CheckCancellationPending();
                    System.Threading.Thread.Sleep(SleepCycleDurationMs);
                }
            }
            finally
            {
                if (oStream != null)
                {
                    oStream.Close();
                }
            }
        }
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

                OnExceptionWhileEnumeratingJsonValues(oException, iPage,
                    bSkipMostPage1Errors);

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
            RestApiUri,
            bFollowed ? "friends/ids" : "followers/ids",
            EncodeUrlParameter(sScreenName)
            );

        // The JSON looped through here has an "ids" name whose value is an
        // array of integer IDs.

        foreach ( Object oUserIDAsObject in EnumerateJsonValues(
            sUrl, "ids", iMaximumPeoplePerRequest, true, oRequestStatistics) )
        {
            String sUserID;

            if ( TryConvertJsonValueToString(oUserIDAsObject, out sUserID) )
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
    /// <param name="asUserIDs">
    /// Array of user IDs to get user value dictionaries for.
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
    /// For each user ID in <paramref name="asUserIDs" />, this method gets
    /// information about the user from Twitter and returns it as a dictionary
    /// of values.  The order of the returned values is not necessarily the
    /// same as the order of the user IDs.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable< Dictionary<String, Object> >
    EnumerateUserValueDictionaries
    (
        String [] asUserIDs,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(asUserIDs != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // We'll use Twitter's users/lookup API, which gets extended
        // information for up to 100 users in one call.

        Int32 iUserIDs = asUserIDs.Length;
        Int32 iUserIDsProcessed = 0;
        Int32 iCalls = 0;

        while (iUserIDsProcessed < iUserIDs)
        {
            // For each call, ask for information about as many users as
            // possible until either 100 is reached or the URL reaches an
            // arbitrary maximum length.  Twitter recommends using a POST here
            // (without specifying why), but it would require revising the
            // base-class HTTP calls and isn't worth the trouble.

            Int32 iUserIDsProcessedThisCall = 0;

            StringBuilder oUrl = new StringBuilder();

            oUrl.AppendFormat(
                "{0}users/lookup.json?{1}&user_id="
                ,
                RestApiUri,
                IncludeEntitiesUrlParameter
                );

            const Int32 MaxUserIDsPerCall = 100;
            const Int32 MaxUrlLength = 2000;

            // Construct the URL for this call.

            while (
                iUserIDsProcessed < iUserIDs
                &&
                iUserIDsProcessedThisCall < MaxUserIDsPerCall
                &&
                oUrl.Length < MaxUrlLength
                )
            {
                if (iUserIDsProcessedThisCall > 0)
                {
                    // Append an encoded comma.  Using an unencoded comma 
                    // causes Twitter to return a 401 "unauthorized" error.
                    //
                    // See this post for an explanation:
                    //
                    // https://dev.twitter.com/discussions/11399

                    oUrl.Append("%2C");
                }

                oUrl.Append( asUserIDs[iUserIDsProcessed] );
                iUserIDsProcessed++;
                iUserIDsProcessedThisCall++;
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
    //  Method: EncodeUrlParameter()
    //
    /// <summary>
    /// Encodes an URL parameter using a method appropriate for Twitter and
    /// OAuth.
    /// </summary>
    ///
    /// <param name="urlParameter">
    /// The URL parameter to be encoded.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The encoded parameter.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeUrlParameter
    (
        String urlParameter
    )
    {
        Debug.Assert(urlParameter != null);

        // The OAuth code provides a method for this.  That method is based on
        // RFC 3986, Section 2.1, which is documented in "Percent encoding
        // parameters" at
        // https://dev.twitter.com/docs/auth/percent-encoding-parameters.

        return ( OAuthBase.UrlEncode(urlParameter) );
    }

    //*************************************************************************
    //  Method: OnExceptionWhileEnumeratingJsonValues()
    //
    /// <summary>
    /// Handles an exception throws while enumerating JSON values.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <param name="iPage">
    /// The page the exception was thrown from.
    /// </param>
    ///
    /// <param name="bSkipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped,
    /// unless they are due to rate limiting.)
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="oException" /> is fatal, this method rethrows the
    /// exception.  Otherwise, this method returns and the caller should stop
    /// its enumeration but not throw an exception.
    /// </remarks>
    //*************************************************************************

    protected void
    OnExceptionWhileEnumeratingJsonValues
    (
        Exception oException,
        Int32 iPage,
        Boolean bSkipMostPage1Errors
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert(iPage > 0);
        AssertValid();

        if ( !ExceptionIsWebOrJson(oException) )
        {
            // This is an unknown exception.

            throw (oException);
        }

        if (
            oException is WebException
            &&
            WebExceptionIsDueToRateLimit( (WebException)oException )
            )
        {
            throw (oException);
        }
        else if (iPage == 1)
        {
            if (bSkipMostPage1Errors)
            {
                return;
            }
            else
            {
                throw (oException);
            }
        }
        else
        {
            // Always skip non-rate-limit errors on page 2 and above.

            return;
        }
    }

    //*************************************************************************
    //  Method: ExceptionIsWebOrJson()
    //
    /// <summary>
    /// Determines whether an exception is a WebException or an exception
    /// thrown while parsing JSON.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception to test.
    /// </param>
    ///
    /// <returns>
    /// true if the exception is a WebException or an exception thrown by the
    /// JavaScriptSerializer class.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ExceptionIsWebOrJson
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);

        return (
            oException is WebException ||
            oException is ArgumentException ||
            oException is InvalidOperationException ||
            oException is InvalidCastException
            );
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

        Object oValue;

        if ( oValueDictionary.TryGetValue(sName, out oValue) )
        {
            return ( TryConvertJsonValueToString(oValue, out sValue) );
        }

        sValue = null;
        return (false);
    }

    //*************************************************************************
    //  Method: TryConvertJsonValueToString()
    //
    /// <summary>
    /// Attempts to convert a JSON value to a non-empty string.
    /// </summary>
    ///
    /// <param name="oValue">
    /// A JSON value to convert.  Can be null.
    /// </param>
    ///
    /// <param name="sValue">
    /// Where the non-empty value gets stored if true is returned.  If false is
    /// returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the value was converted.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="oValue" /> is not null and can be converted to a
    /// non-empty string, the string gets stored at <paramref name="sValue" />
    /// and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryConvertJsonValueToString
    (
        Object oValue,
        out String sValue
    )
    {
        AssertValid();

        if (oValue != null)
        {
            sValue = oValue.ToString();

            if (sValue.Length > 0)
            {
                return (true);
            }
        }

        sValue = null;
        return (false);
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
    //  Method: GetUrlsAndHashtagsFromStatusValueDictionary()
    //
    /// <summary>
    /// Attempts to get URLs and hashtags from the entities in a JSON value
    /// dictionary.
    /// </summary>
    ///
    /// <param name="oStatusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="bExpandUrls">
    /// true to expand all URLs that might be shortened URLs.
    /// </param>
    ///
    /// <param name="sUrls">
    /// Where the space-delimited URLs get stored if they are available.  If
    /// they are not available, this gets set to null.
    /// </param>
    ///
    /// <param name="sHashtags">
    /// Where the space-delimited hashtags get stored if they are available.
    /// If they are not available, this gets set to null.  The hashtags are all
    /// in lower case and do not include a leading pound sign.
    /// </param>
    //*************************************************************************

    protected void
    GetUrlsAndHashtagsFromStatusValueDictionary
    (
        Dictionary<String, Object> oStatusValueDictionary,
        Boolean bExpandUrls,
        out String sUrls,
        out String sHashtags
    )
    {
        Debug.Assert(oStatusValueDictionary != null);

        sUrls = sHashtags = null;
        Object oEntitiesAsObject;

        if (
            oStatusValueDictionary.TryGetValue("entities",
                out oEntitiesAsObject)
            &&
            oEntitiesAsObject is Dictionary<String, Object>
            )
        {
            Dictionary<String, Object> oEntityValueDictionary =
                ( Dictionary<String, Object> )oEntitiesAsObject;

            EntityFilter oUrlFilter = null;

            if (bExpandUrls)
            {
                // If there is an (illegal) space in the expanded URL, escape
                // it to prevent it from being interpreted by the caller as a
                // space between URLs.

                oUrlFilter =
                    ( sUrl => UrlUtil.ExpandUrl(sUrl).Replace(" ", "%20") );
            }

            GetEntities(oEntityValueDictionary, "urls", "expanded_url",
                oUrlFilter, out sUrls);

            GetEntities(oEntityValueDictionary, "hashtags", "text",
                null, out sHashtags);

            if (sHashtags != null)
            {
                sHashtags = sHashtags.ToLower();
            }
        }
    }

    //*************************************************************************
    //  Method: GetLatitudeAndLongitudeFromStatusValueDictionary()
    //
    /// <summary>
    /// Attempts to get a latitude and longitude from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="oStatusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="sLatitude">
    /// Where the latitude gets stored if it is available.  If it is not
    /// available, this gets set to null.
    /// </param>
    ///
    /// <param name="sLongitude">
    /// Where the longitude gets stored if it is available.  If it is not
    /// available, this gets set to null.
    /// </param>
    //*************************************************************************

    protected void
    GetLatitudeAndLongitudeFromStatusValueDictionary
    (
        Dictionary<String, Object> oStatusValueDictionary,
        out String sLatitude,
        out String sLongitude
    )
    {
        Debug.Assert(oStatusValueDictionary != null);

        Object oGeoAsObject;

        if (
            oStatusValueDictionary.TryGetValue("geo", out oGeoAsObject)
            &&
            oGeoAsObject is Dictionary<String, Object>
            )
        {
            Dictionary<String, Object> oGeoValueDictionary =
                ( Dictionary<String, Object> )oGeoAsObject;

            Object oCoordinatesAsObject;

            if (
                oGeoValueDictionary.TryGetValue("coordinates",
                    out oCoordinatesAsObject)
                &&
                oCoordinatesAsObject is Object[]
                )
            {
                Object [] aoCoordinates = ( Object[] )oCoordinatesAsObject;

                if (aoCoordinates.Length == 2)
                {
                    TryConvertJsonValueToString(aoCoordinates[0],
                        out sLatitude);

                    TryConvertJsonValueToString(aoCoordinates[1],
                        out sLongitude);

                    return;
                }
            }
        }

        sLatitude = sLongitude = null;
    }

    //*************************************************************************
    //  Delegate: EntityFilter
    //
    /// <summary>
    /// Represents a method that filters an entity parsed from a Twitter JSON
    /// response.
    /// </summary>
    ///
    /// <param name="sEntity">
    /// The entity to filter.
    /// </param>
    ///
    /// <returns>
    /// The filtered entity.
    /// </returns>
    //*************************************************************************

    protected delegate String
    EntityFilter
    (
        String sEntity
    );

    //*************************************************************************
    //  Method: GetEntities()
    //
    /// <summary>
    /// Attempts to get entities from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="oEntityValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains all
    /// entities for a status.
    /// </param>
    ///
    /// <param name="sEntityName">
    /// Name of the value in <paramref name="oEntityValueDictionary" />
    /// containing the entities to get.  Sample: "urls".  The value is assumed
    /// to contain an array of entity objects; an array of URL objects, for
    /// example. 
    /// </param>
    ///
    /// <param name="sEntityChildName">
    /// Name of the child value in each entity to get.  Sample: "expanded_url".
    /// </param>
    ///
    /// <param name="oEntityFilter">
    /// Method that filters each entity before it is appended to <paramref
    /// name="sChildValues" />, or null for no filtering.
    /// </param>
    ///
    /// <param name="sChildValues">
    /// Where the space-delimited child values get stored if they are
    /// available.  If they are not available, this gets set to null.
    /// </param>
    //*************************************************************************

    protected void
    GetEntities
    (
        Dictionary<String, Object> oEntityValueDictionary,
        String sEntityName,
        String sEntityChildName,
        EntityFilter oEntityFilter,
        out String sChildValues
    )
    {
        Debug.Assert(oEntityValueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(sEntityName) );
        Debug.Assert( !String.IsNullOrEmpty(sEntityChildName) );

        StringBuilder oStringBuilder = new StringBuilder();
        Object oEntitiesAsObject;

        if (
            oEntityValueDictionary.TryGetValue(sEntityName,
                out oEntitiesAsObject)
            &&
            oEntitiesAsObject is Object[]
            )
        {
            foreach (Object oEntityAsObject in ( Object [] )oEntitiesAsObject)
            {
                String sChildValue;

                if (
                    oEntityAsObject is Dictionary<String, Object>
                    &&
                    TryGetJsonValueFromDictionary(
                        ( Dictionary<String, Object> )oEntityAsObject,
                        sEntityChildName, out sChildValue)
                    )
                {
                    if (oEntityFilter != null)
                    {
                        sChildValue = oEntityFilter(sChildValue);
                    }

                    if (oStringBuilder.Length > 0)
                    {
                        oStringBuilder.Append(' ');
                    }

                    oStringBuilder.Append(sChildValue);
                }
            }
        }

        if (oStringBuilder.Length == 0)
        {
            sChildValues = null;
        }
        else
        {
            sChildValues = oStringBuilder.ToString();
        }
    }

    //*************************************************************************
    //  Method: TwitterUsersToUniqueScreenNames()
    //
    /// <summary>
    /// Creates a collection of unique screen names from a collection of
    /// TwitterUser objects.
    /// </summary>
    ///
    /// <param name="oTwitterUsers">
    /// Collection of the TwitterUsers in the network.
    /// </param>
    ///
    /// <returns>
    /// A collection of unique screen names. 
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    TwitterUsersToUniqueScreenNames
    (
        IEnumerable<TwitterUser> oTwitterUsers
    )
    {
        Debug.Assert(oTwitterUsers != null);
        AssertValid();

        HashSet<String> oUniqueScreenNames = new HashSet<String>();

        foreach (TwitterUser oTwitterUser in oTwitterUsers)
        {
            oUniqueScreenNames.Add(oTwitterUser.ScreenName);
        }

        return (oUniqueScreenNames);
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a person if such
    /// a node doesn't already exist.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="sUserID">
    /// Twitter user ID to add a vertex XML node for.
    /// </param>
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
    /// <param name="oTwitterUser">
    /// Where the TwitterUser that represents the user gets stored.  This gets
    /// set regardless of whether the node already existed.
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
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        out TwitterUser oTwitterUser
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);

        oTwitterUser = null;

        if ( oUserIDDictionary.TryGetValue(sUserID, out oTwitterUser) )
        {
            return (false);
        }

        XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
            sScreenName);

        oTwitterUser = new TwitterUser(sScreenName, oVertexXmlNode);
        oUserIDDictionary.Add(sUserID, oTwitterUser);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
            MenuTextID, "Open Twitter Page for This Person");

        oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
            MenuActionID, String.Format(UserWebPageUrlPattern, sScreenName) );

        return (true);
    }

    //*************************************************************************
    //  Method: AppendUserInformationFromValueDictionary()
    //
    /// <summary>
    /// Appends GraphML-Attribute values for a user from a value dictionary
    /// returned by Twitter to a vertex XML node.
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
    /// <param name="bAddLatestStatusToTwitterUser">
    /// If true, the user's latest status gets added to <paramref
    /// name="oTwitterUser" />.
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
        Boolean bExpandLatestStatusUrls,
        Boolean bAddLatestStatusToTwitterUser
    )
    {
        Debug.Assert(oUserValueDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterUser != null);
        AssertValid();

        XmlNode oVertexXmlNode = oTwitterUser.VertexXmlNode;

        if (bIncludeStatistics)
        {
            AppendValueFromValueDictionary(oUserValueDictionary,
                "friends_count", oGraphMLXmlDocument, oVertexXmlNode,
                FollowedID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "followers_count", oGraphMLXmlDocument, oVertexXmlNode,
                FollowersID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "statuses_count", oGraphMLXmlDocument, oVertexXmlNode,
                StatusesID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "favourites_count", oGraphMLXmlDocument,
                oVertexXmlNode, FavoritesID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "description", oGraphMLXmlDocument, oVertexXmlNode,
                DescriptionID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "location", oGraphMLXmlDocument, oVertexXmlNode, LocationID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "url", oGraphMLXmlDocument, oVertexXmlNode, UrlID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "time_zone", oGraphMLXmlDocument, oVertexXmlNode, TimeZoneID);

            AppendValueFromValueDictionary(oUserValueDictionary,
                "utc_offset", oGraphMLXmlDocument, oVertexXmlNode,
                UtcOffsetID);

            String sJoinedDateUtc;

            if ( TryGetJsonValueFromDictionary(oUserValueDictionary,
                "created_at", out sJoinedDateUtc) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, JoinedDateUtcID,
                    TwitterDateParser.ParseTwitterDate(sJoinedDateUtc) );
            }
        }

        // Add an image URL GraphML-attribute if it hasn't already been added.
        // (It might have been added from an "entry" node by
        // TwitterSearchNetworkAnalyzer.)

        if (oVertexXmlNode.SelectSingleNode(
            "a:data[@key='" + ImageFileID + "']", 
            oGraphMLXmlDocument.CreateXmlNamespaceManager("a") ) == null)
        {
            AppendValueFromValueDictionary(oUserValueDictionary,
                "profile_image_url", oGraphMLXmlDocument, oVertexXmlNode,
                ImageFileID);
        }

        // Process the user's latest status if requested.

        Object oStatusValueDictionaryAsObject;

        if (
            (bIncludeLatestStatus || bAddLatestStatusToTwitterUser)
            &&
            oUserValueDictionary.TryGetValue("status",
                out oStatusValueDictionaryAsObject)
            &&
            oStatusValueDictionaryAsObject is Dictionary<String, Object>
            )
        {
            Dictionary<String, Object> oStatusValueDictionary =
                ( Dictionary<String, Object> )oStatusValueDictionaryAsObject;

            AppendStatusInformationFromValueDictionary(oStatusValueDictionary,
                oGraphMLXmlDocument, oTwitterUser, bIncludeLatestStatus,
                bExpandLatestStatusUrls, bAddLatestStatusToTwitterUser);
        }
    }

    //*************************************************************************
    //  Method: AppendStatusInformationFromValueDictionary()
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
    /// <param name="bAddLatestStatusToTwitterUser">
    /// If true, the user's latest status gets added to <paramref
    /// name="oTwitterUser" />.
    /// </param>
    ///
    /// <remarks>
    /// This method reads latest status information from a value dictionary
    /// returned by Twitter and appends the information to a vertex XML node in
    /// the GraphML document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendStatusInformationFromValueDictionary
    (
        Dictionary<String, Object> oStatusValueDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument,
        TwitterUser oTwitterUser,
        Boolean bIncludeLatestStatus,
        Boolean bExpandLatestStatusUrls,
        Boolean bAddLatestStatusToTwitterUser
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

        GetLatitudeAndLongitudeFromStatusValueDictionary(
            oStatusValueDictionary, out sLatitude, out sLongitude);

        XmlNode oVertexXmlNode = oTwitterUser.VertexXmlNode;

        String sLatestStatusUrls = null;
        String sLatestStatusHashtags = null;

        if (bIncludeLatestStatus)
        {
            // Add the status to the vertex XML node.

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oVertexXmlNode, LatestStatusID, sLatestStatus);

            GetUrlsAndHashtagsFromStatusValueDictionary(oStatusValueDictionary, 
                bExpandLatestStatusUrls, out sLatestStatusUrls,
                out sLatestStatusHashtags);

            if ( !String.IsNullOrEmpty(sLatestStatusUrls) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, LatestStatusUrlsID,
                    sLatestStatusUrls);

                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, LatestStatusDomainsID,
                    UrlsToDomains(sLatestStatusUrls) ); 
            }

            if ( !String.IsNullOrEmpty(sLatestStatusHashtags) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, LatestStatusHashtagsID,
                    sLatestStatusHashtags);
            }

            if ( !String.IsNullOrEmpty(sLatestStatusDateUtc) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, LatestStatusDateUtcID,
                    sLatestStatusDateUtc);
            }

            AppendLatitudeAndLongitudeGraphMLAttributeValues(
                oGraphMLXmlDocument, oVertexXmlNode, sLatitude, sLongitude);
        }

        if (bAddLatestStatusToTwitterUser)
        {
            oTwitterUser.Statuses.Add( new TwitterStatus(sID, sLatestStatus,
                sLatestStatusDateUtc, sLatitude, sLongitude, sLatestStatusUrls,
                sLatestStatusHashtags) );
        }
    }

    //*************************************************************************
    //  Method: AppendValueFromValueDictionary()
    //
    /// <summary>
    /// Appends a GraphML-Attribute value from a value dictionary returned by
    /// Twitter to an edge or vertex XML node.
    /// </summary>
    ///
    /// <param name="oValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.
    /// </param>
    /// 
    /// <param name="sName">
    /// The name of the value to get from <paramref name="oValueDictionary" />.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the GraphML-Attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method looks for a value named <paramref name="sName" /> in
    /// <paramref name="oValueDictionary" />.  If the value is found, it gets
    /// stored on <paramref name="oEdgeOrVertexXmlNode" /> as a
    /// GraphML-Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendValueFromValueDictionary
    (
        Dictionary<String, Object> oValueDictionary,
        String sName,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oValueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        String sValue;

        if ( TryGetJsonValueFromDictionary(oValueDictionary, sName,
            out sValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, sValue);

            return (true);
        }

        return (false);
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
            RelationshipDateUtcID, 

            DateTimeUtil2.ToCultureInvariantString(
                oRequestStatistics.StartTimeUtc)
            );
    }

    //*************************************************************************
    //  Method: AppendLatitudeAndLongitudeGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends GraphML attribute values for latitude and longitude to an edge
    /// or vertex XML node.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the Graph-ML attribute values to.
    /// </param>
    ///
    /// <param name="sLatitude">
    /// The latitude.  Can be null or empty.
    /// </param>
    ///
    /// <param name="sLongitude">
    /// The longitude.  Can be null or empty.
    /// </param>
    //*************************************************************************

    protected void
    AppendLatitudeAndLongitudeGraphMLAttributeValues
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sLatitude,
        String sLongitude
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        AssertValid();

        if ( !String.IsNullOrEmpty(sLatitude) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, LatitudeID, sLatitude);
        }

        if ( !String.IsNullOrEmpty(sLongitude) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, LongitudeID, sLongitude);
        }
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

        if (!bIncludeRepliesToEdges && !bIncludeMentionsEdges &&
            !bIncludeNonRepliesToNonMentionsEdges)
        {
            return;
        }

        ReportProgress("Examining relationships.");

        // Loop through each status for each user.

        foreach (TwitterUser oTwitterUser in oTwitterUsers)
        {
            foreach (TwitterStatus oTwitterStatus in oTwitterUser.Statuses)
            {
                AppendRepliesToAndMentionsEdgeXmlNodes(oGraphMLXmlDocument,
                    oUniqueScreenNames, bIncludeRepliesToEdges,
                    bIncludeMentionsEdges,
                    bIncludeNonRepliesToNonMentionsEdges,
                    oTwitterUser.ScreenName, oTwitterStatus, bIncludeStatuses);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNodes()
    //
    /// <summary>
    /// Appends edge XML nodes for replies-to and mentions relationships for
    /// one status.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
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
    /// <param name="sScreenName">
    /// The user's screen name.
    /// </param>
    ///
    /// <param name="oTwitterStatus">
    /// One of the user's statuses.
    /// </param>
    ///
    /// <param name="bIncludeStatus">
    /// true to include the status in the edge XML nodes.
    /// </param>
    //*************************************************************************

    protected void
    AppendRepliesToAndMentionsEdgeXmlNodes
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        HashSet<String> oUniqueScreenNames,
        Boolean bIncludeRepliesToEdges,
        Boolean bIncludeMentionsEdges,
        Boolean bIncludeNonRepliesToNonMentionsEdges,
        String sScreenName,
        TwitterStatus oTwitterStatus,
        Boolean bIncludeStatus
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUniqueScreenNames != null);
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        String sStatus = oTwitterStatus.Text;
        String sParsedDateUtc = oTwitterStatus.ParsedDateUtc;
        Boolean bIsReplyTo = false;
        Boolean bIsMentions = false;

        Debug.Assert( !String.IsNullOrEmpty(sStatus) );

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusParser.GetScreenNames(sStatus,
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        if (sRepliedToScreenName != null)
        {
            if (
                sRepliedToScreenName != sScreenName
                &&
                oUniqueScreenNames.Contains(sRepliedToScreenName)
                )
            {
                bIsReplyTo = true;

                if (bIncludeRepliesToEdges)
                {
                    AppendRepliesToAndMentionsEdgeXmlNode(oGraphMLXmlDocument,
                        sScreenName, sRepliedToScreenName,
                        RepliesToRelationship, oTwitterStatus, bIncludeStatus);
                }
            }
        }

        foreach (String sMentionedScreenName in asUniqueMentionedScreenNames)
        {
            if (
                sMentionedScreenName != sScreenName
                &&
                oUniqueScreenNames.Contains(sMentionedScreenName)
                )
            {
                bIsMentions = true;

                if (bIncludeMentionsEdges)
                {
                    AppendRepliesToAndMentionsEdgeXmlNode(oGraphMLXmlDocument,
                        sScreenName, sMentionedScreenName,
                        MentionsRelationship, oTwitterStatus, bIncludeStatus);
                }
            }
        }

        if (bIncludeNonRepliesToNonMentionsEdges && !bIsReplyTo &&
            !bIsMentions)
        {
            // Append a self-loop edge to represent the tweet.

            AppendRepliesToAndMentionsEdgeXmlNode(oGraphMLXmlDocument,
                sScreenName, sScreenName, NonRepliesToNonMentionsRelationship,
                oTwitterStatus, bIncludeStatus);
        }
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node for a replies-to, mentions, or non-replies-to-
    /// non-mentions relationship for one status.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="sScreenName1">
    /// The edge's first screen name.
    /// </param>
    ///
    /// <param name="sScreenName2">
    /// The edge's second screen name.
    /// </param>
    ///
    /// <param name="sRelationship">
    /// A description of the relationship represented by the edge.
    /// </param>
    ///
    /// <param name="oTwitterStatus">
    /// A twitter status.
    /// </param>
    ///
    /// <param name="bIncludeStatus">
    /// true to include the status in the edge XML node.
    /// </param>
    //*************************************************************************

    protected void
    AppendRepliesToAndMentionsEdgeXmlNode
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sScreenName1,
        String sScreenName2,
        String sRelationship,
        TwitterStatus oTwitterStatus,
        Boolean bIncludeStatus
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sScreenName1) );
        Debug.Assert( !String.IsNullOrEmpty(sScreenName2) );
        Debug.Assert( !String.IsNullOrEmpty(sRelationship) );
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        XmlNode oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
            sScreenName1, sScreenName2, sRelationship);

        String sStatusDateUtc = oTwitterStatus.ParsedDateUtc;
        Boolean bHasStatusDateUtc = !String.IsNullOrEmpty(sStatusDateUtc);

        if (bHasStatusDateUtc)
        {
            // The status's date is the relationship date.

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                RelationshipDateUtcID, sStatusDateUtc);
        }

        if (bIncludeStatus)
        {
            String sStatus = oTwitterStatus.Text;

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                StatusID, sStatus);

            String sUrls = oTwitterStatus.Urls;

            if ( !String.IsNullOrEmpty(sUrls) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                    StatusUrlsID, sUrls);

                oGraphMLXmlDocument.AppendGraphMLAttributeValue( oEdgeXmlNode,
                    StatusDomainsID, UrlsToDomains(sUrls) );
            }

            if ( !String.IsNullOrEmpty(oTwitterStatus.Hashtags) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                    StatusHashtagsID, oTwitterStatus.Hashtags);
            }

            if (bHasStatusDateUtc)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                    StatusDateUtcID, sStatusDateUtc);
            }

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                StatusWebPageUrlID, 

                FormatStatusWebPageUrl(sScreenName1, oTwitterStatus)
                );

            AppendLatitudeAndLongitudeGraphMLAttributeValues(
                oGraphMLXmlDocument, oEdgeXmlNode, oTwitterStatus.Latitude,
                oTwitterStatus.Longitude);

            // Precede the ID with a single quote to force Excel to treat the
            // ID as text.  Otherwise, it formats the ID, which is a large
            // number, in scientific notation.

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
                ImportedIDID, "'" + oTwitterStatus.ID);
        }
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
            TwitterUsersToUniqueScreenNames(oUserIDDictionary.Values),
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
            oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                sScreenName, sOtherScreenName, "Followed");
        }
        else
        {
            oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                sOtherScreenName, sScreenName, "Follower");
        }

        AppendStartTimeRelationshipDateUtcGraphMLAttributeValue(
            oGraphMLXmlDocument, oEdgeXmlNode, oRequestStatistics);
    }

    //*************************************************************************
    //  Method: FormatStatusWebPageUrl()
    //
    /// <summary>
    /// Formats a string to use for a status Web page URL.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The status's author.
    /// </param>
    ///
    /// <param name="oTwitterStatus">
    /// The twitter status.
    /// </param>
    //*************************************************************************

    protected String
    FormatStatusWebPageUrl
    (
        String sScreenName,
        TwitterStatus oTwitterStatus
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        return ( String.Format(
            StatusWebPageUrlPattern
            ,
            sScreenName,
            oTwitterStatus.ID
            ) );
    }

    //*************************************************************************
    //  Method: UrlsToDomains()
    //
    /// <summary>
    /// Extracts the domains from a string of space-delimited URLs.
    /// </summary>
    ///
    /// <param name="sSpaceDelimitedUrls">
    /// URLs delimited by spaces.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// Space-delimited domains, one per space-delimited URL.
    /// </returns>
    //*************************************************************************

    protected String
    UrlsToDomains
    (
        String sSpaceDelimitedUrls
    )
    {
        Debug.Assert(sSpaceDelimitedUrls != null);
        AssertValid();

        StringBuilder oDomains = new StringBuilder();

        foreach ( String sUrl in sSpaceDelimitedUrls.Split(new Char[]{' '},
            StringSplitOptions.RemoveEmptyEntries) )
        {
            String sDomain;

            if ( UrlUtil.TryGetDomainFromUrl(sUrl, out sDomain) )
            {
                oDomains.AppendFormat(
                    "{0}{1}",
                    oDomains.Length > 0 ? " " : String.Empty,
                    sDomain
                    );
            }
        }

        return ( oDomains.ToString() );
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

        // m_oTwitterAccessToken
        Debug.Assert(m_oTwitterStatusParser != null);
    }


    //*************************************************************************
    //  Twitter API URIs
    //*************************************************************************

    /// URI of the Twitter REST API.

    protected const String RestApiUri = "http://api.twitter.com/1.1/";

    /// URI of the Twitter search API.

    protected const String SearchApiUri =
        "http://api.twitter.com/1.1/search/tweets.json";

    /// URI of the Twitter OAuth API.

    public const string OAuthApiUri = "https://api.twitter.com/oauth/";

    /// Format pattern for the URL of the Web page for a Twitter user.  The {0}
    /// argument must be replaced with a Twitter screen name.

    protected const String UserWebPageUrlPattern =
        "http://twitter.com/{0}";

    /// Format pattern for the URL of the Web page for a Twitter status.  The
    /// {0} argument must be replaced with a Twitter screen name and the {1}
    /// argument must be replaced with a status ID.

    protected const String StatusWebPageUrlPattern =
        "https://twitter.com/#!/{0}/status/{1}";

    /// Twitter API URL parameter to include entities in the response.

    protected const String IncludeEntitiesUrlParameter = "include_entities=1"; 


    //*************************************************************************
    //  Relationship descriptions
    //*************************************************************************

    /// Values for the edge's RelationshipID GraphML-attribute.

    protected const String RepliesToRelationship = "Replies to";
    ///
    protected const String MentionsRelationship = "Mentions";
    ///
    protected const String NonRepliesToNonMentionsRelationship = "Tweet";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

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


    /// Default time to pause before retrying a request after Twitter rate
    /// limits kick in, in milliseconds.

    protected const Int32 DefaultRateLimitPauseMs = 15 * 60 * 1000;


    /// GraphML-attribute IDs.

    protected const String StatusesID = "Statuses";
    ///
    protected const String FavoritesID = "Favorites";
    ///
    protected const String FollowedID = "Followed";
    ///
    protected const String FollowersID = "Followers";
    ///
    protected const String LatestStatusID = "LatestStatus";
    ///
    protected const String LatestStatusUrlsID = "LatestStatusUrls";
    ///
    protected const String LatestStatusDomainsID = "LatestStatusDomains";
    ///
    protected const String LatestStatusHashtagsID = "LatestStatusHashtags";
    ///
    protected const String LatestStatusDateUtcID = "LatestStatusDateUtc";
    ///
    protected const String StatusID = "Status";
    ///
    protected const String StatusDateUtcID = "StatusDateUtc";
    ///
    protected const String StatusUrlsID = "StatusUrls";
    ///
    protected const String StatusDomainsID = "StatusDomains";
    ///
    protected const String StatusHashtagsID = "StatusHashtags";
    ///
    protected const String StatusWebPageUrlID = "StatusWebPageUrl";
    ///
    protected const String LatitudeID = "Latitude";
    ///
    protected const String LongitudeID = "Longitude";
    ///
    protected const String DescriptionID = "Description";
    ///
    protected const String LocationID = "Location";
    ///
    protected const String UrlID = "Url";
    ///
    protected const String TimeZoneID = "TimeZone";
    ///
    protected const String UtcOffsetID = "UtcOffset";
    ///
    protected const String JoinedDateUtcID = "JoinedDateUtc";
    ///
    protected const String RelationshipDateUtcID = "RelationshipDateUtc";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Gets a Twitter access token if the user has been authorized.  null if
    /// BeforeGetNetwork() hasn't been called.

    protected TwitterAccessToken m_oTwitterAccessToken;

    /// Parses the text of a Twitter tweet.  This class uses only one instance
    /// to avoid making TwitterStatusParser recompile all of its regular
    /// expressions.

    protected TwitterStatusParser m_oTwitterStatusParser;


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
