
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ExcelTextUtil
//
/// <summary>
/// Static utility methods for working with Excel text.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ExcelTextUtil
{
    //*************************************************************************
    //  Method: ForceCellText()
    //
    /// <summary>
    /// Forces a string to always appear as text when inserted into a cell.
    /// </summary>
    ///
    /// <param name="cellText">
    /// String that should always appear as text.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="cellText" /> with a prepended apostrophe.
    /// </returns>
    ///
    /// <remarks>
    /// When the string "5/1", for example, is programmatically inserted into
    /// an Excel cell, Excel will convert it to a date even if the cell is
    /// formatted as Text.  If the string is prepended with an apostrophe,
    /// Excel will always treat it as text, regardless of the cell format.
    /// </remarks>
    //*************************************************************************

    public static String
    ForceCellText
    (
        String cellText
    )
    {
        Debug.Assert(cellText != null);

        return ("'" + cellText);
    }

    //*************************************************************************
    //  Method: UnforceCellText()
    //
    /// <summary>
    /// Reveses the effect of <see cref="ForceCellText" />.
    /// </summary>
    ///
    /// <param name="cellText">
    /// String that may include a prepended apostrophe.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="cellText" /> with any prepended apostrophe removed.
    /// </returns>
    //*************************************************************************

    public static String
    UnforceCellText
    (
        String cellText
    )
    {
        Debug.Assert(cellText != null);

        if ( cellText.StartsWith("'") )
        {
            cellText = cellText.Substring(1);
        }

        return (cellText);
    }
}

}
