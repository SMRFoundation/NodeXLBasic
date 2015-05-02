
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ThirdPartyGraphDataProvidersInfo
//
/// <summary>
/// Provides information about all third-party graph data providers.
/// </summary>
//*****************************************************************************

public static class ThirdPartyGraphDataProvidersInfo : Object
{
    //*************************************************************************
    //  Method: GetAll()
    //
    /// <summary>
    /// Gets a collection of information about all third-party graph data
    /// providers.
    /// </summary>
    ///
    /// <returns>
    /// A collection of <see cref="ThirdPartyGraphDataProviderInfo" /> objects.
    /// </returns>
    //*************************************************************************

    public static IEnumerable<ThirdPartyGraphDataProviderInfo>
    GetAll()
    {
        List<ThirdPartyGraphDataProviderInfo> oAll =
            new List<ThirdPartyGraphDataProviderInfo>();

        oAll.Add(new ThirdPartyGraphDataProviderInfo(
            "Connected Action NodeXL Graph Server",
            "https://graphserverimporter.codeplex.com/",
            "The Connected Action NodeXL Graph Server Database enables "+
            "NodeXL users to collect and store their social media data "+
            "from Twitter and Facebook in a personal Cloud Storage locker."
            ));

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "Exchange Server Networks",
            "http://exchangespigot.codeplex.com/",
            "Exchange Spigot for NodeXL enables Microsoft Excel plugin "+
            "NodeXL to collect messaging data from the Microsoft "+
            "Exchange Server and display that data as a graph."
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "MediaWiki Networks",
            "http://wikiimporter.codeplex.com/",
            "WikiImporter for NodeXL a new graph data provider for "+
            "NodeXL which allow users to directly download and import "+
            "different MediaWiki networks."
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "ONA Survey Networks",
            "https://www.s2.onasurveys.com/help/nodexl.php",
            "ONA Surveys has been custom built " +
            "to allow users to ask respondents questions about their " +
            "relationship with other people, groups,entities or in fact any " +
            "thing you like. You can download data in graphml format and load " +
            "it straight into NodeXL."
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "Social Networks",
            "http://socialnetimporter.codeplex.com/",
            "Social Network Importer for NodeXL is a new graph data provider "+
            "for NodeXL which will allow each user to directly download and "+
            "import from within NodeXL different Facebook networks."
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "vKontakte and Odnoklassniki Networks",
            "http://runetimporter.codeplex.com/",
            "The RuNet Importer for NodeXL is a network graph data provider " +
            "which allows users to download & import Social Network graph " +
            "data from VKontakte & Odnoklassniki."
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "VOSON Hyperlink Networks",
            "http://voson.anu.edu.au/node/13#VOSON-NodeXL",
            "Import hyperlink networks into NodeXL with the VOSON System -- "+
            "a web-based software incorporating web mining, data visualisation, "+
            "and traditional empirical social science methods"
            ) );

        return (oAll);
    }
}

}
