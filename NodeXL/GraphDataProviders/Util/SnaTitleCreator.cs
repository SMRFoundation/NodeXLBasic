
using System;
using System.Diagnostics;
using Smrf.SocialNetworkLib;

namespace Smrf.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: SnaTitleCreator
//
/// <summary>
/// Creates a title for Social Network Analysis graphs.
/// </summary>
//*****************************************************************************

public static class SnaTitleCreator
{
    //*************************************************************************
    //  Method: CreateSnaTitle()
    //
    /// <summary>
    /// Creates a title for SNA graphs.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The search term that was used.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// A title.
    /// </returns>
    //*************************************************************************

    public static String
    CreateSnaTitle
    (
        String searchTerm,
        RequestStatistics requestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(requestStatistics != null);

        // Sample title, where searchTerm is "usfca":
        //
        //   "usfca Twitter NodeXL SNA Map and Report for Saturday,
        //   05 April 2014 at 18:47 UTC"

        DateTime oStartTimeUtc = requestStatistics.StartTimeUtc;

        return ( String.Format(

            "{0} Twitter NodeXL SNA Map and Report for {1} at {2} UTC"
            ,
            searchTerm,
            oStartTimeUtc.ToString("dddd, dd MMMM yyyy"),
            oStartTimeUtc.ToString("HH:mm")
            ) );
    }
}

}
