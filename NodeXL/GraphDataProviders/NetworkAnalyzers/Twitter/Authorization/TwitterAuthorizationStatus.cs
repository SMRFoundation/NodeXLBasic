
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Enum: TwitterAuthorizationStatus
//
/// <summary>
/// Specifies the user's Twitter authorization status.
/// </summary>
//*****************************************************************************

public enum
TwitterAuthorizationStatus
{
    /// <summary>
    /// The user has a Twitter account, but she has not yet authorized Twitter
    /// to use it.
    /// </summary>

    HasTwitterAccountNotAuthorized,

    /// <summary>
    /// The user has a Twitter account and she has authorized Twitter to use
    /// it.
    /// </summary>

    HasTwitterAccountAuthorized,
}

}
