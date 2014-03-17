
using System;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterListNameParser
//
/// <summary>
/// Parses Twitter list names.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class TwitterListNameParser
{
    //*************************************************************************
    //  Method: TryParseListName()
    //
    /// <summary>
    /// Attempts to parse a Twitter list name into a slug and owner screen
    /// name.
    /// </summary>
    ///
    /// <param name="listName">
    /// List name in the format "bob/twitterit".  Can't be null.
    /// </param>
    ///
    /// <param name="slug">
    /// Where the slug gets stored if true is returned.  Sample: "twitterit".
    /// </param>
    ///
    /// <param name="ownerScreenName">
    /// Where the owner screen name gets stored if true is returned.  Sample:
    /// "bob".
    /// </param>
    ///
    /// <returns>
    /// true if the list name was successfully parsed, false if the list name
    /// is not valid.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseListName
    (
        String listName,
        out String slug,
        out String ownerScreenName
    )
    {
        Debug.Assert(listName != null);

        // Twitter screen names: alphanumeric or underscore, 15 characters
        // maximum.
        //
        // Twitter list names: alphanumeric, hyphen or underscore, 25
        // characters maximum.
        //
        // Note: "\w" means "alphanumeric or underscore."

        Regex oRegex = new Regex(@"^(?<OwnerScreenName>\w+)/(?<Slug>(\w|-)+)$");

        Match oMatch = oRegex.Match(listName);

        if (oMatch.Success)
        {
            slug = oMatch.Groups["Slug"].Value;
            ownerScreenName = oMatch.Groups["OwnerScreenName"].Value;

            return (true);
        }

        slug = ownerScreenName = null;
        return (false);
    }
}

}
