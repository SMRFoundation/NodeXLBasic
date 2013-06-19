
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterStatusParser
//
/// <summary>
/// Parses a Twitter status.
/// </summary>
///
/// <remarks>
/// A tweet is known as a "status" in the Twitter API, so "status" is the term
/// used in this class's methods.
/// </remarks>
//*****************************************************************************

public static class TwitterStatusParser : Object
{
    //*************************************************************************
    //  Method: TryParseStatus()
    //
    /// <summary>
    /// Attempts to parse a Twitter status.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// A status value dictionary, as returned by
    /// TwitterUtil.EnumerateSearchStatuses().  The dictionary keys are names
    /// and the dictionary values are the named values.
    /// </param>
    /// 
    /// <param name="statusID">
    /// Where the status ID gets stored if true is returned.
    /// </param>
    ///
    /// <param name="statusDateUtc">
    /// Where the date the user tweeted the status gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="screenName">
    /// Where the screen name of the user who tweeted the status gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <param name="text">
    /// Where the status's text gets stored if true is returned.
    /// </param>
    ///
    /// <param name="rawStatusJson">
    /// Where the complete, raw status returned by Twitter gets stored if true
    /// is returned, in JSON format.  Includes the surrounding braces.
    /// </param>
    /// 
    /// <param name="userValueDictionary">
    /// Where a dictionary of values for the user who tweeted the status gets
    /// stored if true is returned.
    /// </param>
    /// 
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseStatus
    (
        Dictionary<String, Object> statusValueDictionary,
        out Int64 statusID,
        out DateTime statusDateUtc,
        out String screenName,
        out String text,
        out String rawStatusJson,
        out Dictionary<String, Object> userValueDictionary
    )
    {
        Debug.Assert(statusValueDictionary != null);

        statusID = Int64.MinValue;
        statusDateUtc = DateTime.MinValue;
        screenName = null;
        text = null;
        rawStatusJson = null;
        userValueDictionary = null;

        String statusDateUtcString;

        if (
            !TwitterUtil.TryGetJsonValueFromDictionary(statusValueDictionary,
                "id", out statusID)
            ||
            !TwitterUtil.TryGetJsonValueFromDictionary(statusValueDictionary,
                "created_at", out statusDateUtcString)
            ||
            !TwitterDateParser.TryParseTwitterDate(statusDateUtcString,
                out statusDateUtc)
            ||
            !TwitterUtil.TryGetJsonValueFromDictionary(statusValueDictionary,
                "text", out text)
            )
        {
            return (false);
        }

        userValueDictionary =
            ( Dictionary<String, Object> )statusValueDictionary["user"];

        if (
            userValueDictionary == null
            ||
            !TwitterUtil.TryGetJsonValueFromDictionary(userValueDictionary,
                "screen_name", out screenName)
            )
        {
            return (false);
        }

        rawStatusJson = ValueDictionaryToRawJson(statusValueDictionary);

        return (true);
    }

    //*************************************************************************
    //  Method: UserValueDictionaryToRawJson()
    //
    /// <summary>
    /// Converts a user value dictionary to a JSON string.
    /// </summary>
    ///
    /// <param name="userValueDictionary">
    /// A user value dictionary, as returned by <see cref="TryParseStatus" />.
    /// The dictionary keys are names and the dictionary values are the named
    /// values.
    /// </param>
    /// 
    /// <returns>
    /// The user value dictionary as a JSON string.
    /// </returns>
    //*************************************************************************

    public static String
    UserValueDictionaryToRawJson
    (
        Dictionary<String, Object> userValueDictionary
    )
    {
        Debug.Assert(userValueDictionary != null);

        return ( ValueDictionaryToRawJson(userValueDictionary) );
    }

    //*************************************************************************
    //  Method: ValueDictionaryToRawJson()
    //
    /// <summary>
    /// Converts a value dictionary to a JSON string.
    /// </summary>
    ///
    /// <param name="valueDictionary">
    /// A value dictionary obtained by parsing a JSON string.  The dictionary
    /// keys are names and the dictionary values are the named values.
    /// </param>
    /// 
    /// <returns>
    /// The value dictionary as a JSON string.
    /// </returns>
    //*************************************************************************

    private static String
    ValueDictionaryToRawJson
    (
        Dictionary<String, Object> valueDictionary
    )
    {
        Debug.Assert(valueDictionary != null);

        // We don't have access to the original string response, so rebuild the
        // complete JSON string from the value dictionary.

        StringBuilder stringBuilder = new StringBuilder();

        ( new JavaScriptSerializer() ).Serialize(
            valueDictionary, stringBuilder);

        return ( stringBuilder.ToString() );
    }
}

}
