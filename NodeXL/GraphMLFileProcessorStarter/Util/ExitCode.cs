
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphMLFileProcessorStarter
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
    /// <summary>
    /// The GraphMLFileProcessor executable was successfully started.
    /// </summary>

    Success = 0,

    /// <summary>
    /// An unexpected exception occurred.
    /// </summary>

    UnexpectedException = 1,

    /// <summary>
    /// A problem occurred while attempting to find the GraphMLFileProcessor
    /// executable.
    /// </summary>

    CannotFindGraphMLFileProcessor = 2,
}

}
