
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ClickOnceLib
{
//*****************************************************************************
//  Class: NodeXLClickOncePathException
//
/// <summary>
/// Represents an exception thrown when an error occurs in the <see
/// cref="NodeXLClickOncePathUtil" /> class.
/// </summary>
//*****************************************************************************

public class NodeXLClickOncePathException : Exception
{
    //*************************************************************************
    //  Constructor: NodeXLClickOncePathException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLClickOncePathException" /> class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public NodeXLClickOncePathException
    (
        String message
    )
    : base(message)
    {
        // (Do nothing.)
    }
}
}
