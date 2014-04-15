
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphTitleCreator
//
/// <summary>
/// Creates a title for a graph.
/// </summary>
//*****************************************************************************

public static class GraphTitleCreator
{
    //*************************************************************************
    //  Method: CreateGraphTitle()
    //
    /// <summary>
    /// Creates a title for a graph.
    /// </summary>
    ///
    /// <returns>
    /// A graph title.
    /// </returns>
    //*************************************************************************

    public static String
    CreateGraphTitle
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        GraphHistory oGraphHistory =
            ( new PerWorkbookSettings(workbook) ).GraphHistory;

        String sGraphTitle;

        // Order of precedence: suggested title, suggested file name,
        // workbook name.

        if (
            !oGraphHistory.TryGetValue(
                GraphHistoryKeys.ImportSuggestedTitle,
                out sGraphTitle)
            &&
            !oGraphHistory.TryGetValue(
                GraphHistoryKeys.ImportSuggestedFileNameNoExtension,
                out sGraphTitle)
            )
        {
            Debug.Assert( !String.IsNullOrEmpty(workbook.Name) );

            sGraphTitle = workbook.Name;
        }

        return (sGraphTitle);
    }
}
}
