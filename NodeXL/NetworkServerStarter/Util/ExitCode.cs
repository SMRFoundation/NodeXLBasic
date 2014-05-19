
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.NetworkServerStarter
{
//*****************************************************************************
//  Enum: ExitCode
//
/// <summary>
/// Specifies the program's exit codes.
/// </summary>
//*****************************************************************************

public enum
ExitCode
{
    // The exit codes start at 1000 to distinguish them from the exit codes
    // defined by the NetworkServer program, which start at 0.

    /// <summary>
    /// An unexpected exception occurred.
    /// </summary>

    UnexpectedException = 1001,

    /// <summary>
    /// A problem occurred while attempting to find the NetworkServer
    /// executable.
    /// </summary>

    CannotFindNetworkServer = 1002,
}

}
