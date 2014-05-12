
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.NetworkServer
{
//*****************************************************************************
//  Enum: NetworkType
//
/// <summary>
/// Specifies the type of network to get.
/// </summary>
//*****************************************************************************

public enum
NetworkType
{
    /// <summary>
    /// Twitter search network, obtained directly from Twitter.
    /// </summary>

    TwitterSearch,

    /// <summary>
    /// Twitter search network, obtained from the Graph Server.
    /// </summary>

    GraphServerTwitterSearch,
}

}
