
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: InvalidJsonException
//
/// <summary>
/// Represents an exception thrown when invalid JSON is detected.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class InvalidJsonException : Exception
{
    //*************************************************************************
    //  Constructor: InvalidJsonException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidJsonException" />
	/// class.
    /// </summary>
    ///
    /// <param name="message">
    /// The exception message.
    /// </param>
    //*************************************************************************

    public InvalidJsonException
	(
        String message
	)
	: base(message)
    {
        // (Do nothing else.)
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
