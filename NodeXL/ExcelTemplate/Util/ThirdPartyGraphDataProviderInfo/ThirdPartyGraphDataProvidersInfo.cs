
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

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "Exchange Server Networks",
            "http://exchangespigot.codeplex.com/"
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "MediaWiki Networks",
            "http://wikiimporter.codeplex.com/"
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "ONA Survey Networks",
            "https://www.s2.onasurveys.com/help/nodexl.php"
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "Social Networks",
            "http://socialnetimporter.codeplex.com/"
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "vKontakte and Odnoklassniki Networks",
            "http://runetimporter.codeplex.com/"
            ) );

        oAll.Add( new ThirdPartyGraphDataProviderInfo(
            "VOSON Hyperlink Networks",
            "http://voson.anu.edu.au/node/13#VOSON-NodeXL"
            ) );

        return (oAll);
    }
}

}
