
using System;
using System.Text;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphSummarizer
//
/// <summary>
/// Generates a summary of the graph.
/// </summary>
///
/// <remarks>
/// Call <see cref="TrySummarizeGraph" /> or <see cref="SummarizeGraph" /> to
/// get a string that summarizes the graph.
/// </remarks>
//*****************************************************************************

public static class GraphSummarizer : Object
{
    //*************************************************************************
    //  Method: TrySummarizeGraph()
    //
    /// <summary>
    /// Gets a string that summarizes the graph, or shows a message that a
    /// summary isn't available.
    /// </summary>
    ///
    /// <param name="graphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="autoFillWorkbookResults">
    /// Stores the results of a call to <see
    /// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
    /// </param>
    ///
    /// <param name="overallMetrics">
    /// Stores the overall metrics for a graph, or null or an empty string if
    /// not available.
    /// </param>
    ///
    /// <param name="topNByMetrics">
    /// The graph's top-N-by metrics, as a descriptive string, or null or an
    /// empty string if not available.
    /// </param>
    ///
    /// <param name="twitterSearchNetworkTopItems">
    /// The top items in the tweets within a Twitter search network, as a
    /// descriptive string, or null or an empty string if not available.
    /// </param>
    ///
    /// <param name="graphSummary">
    /// Where the graph summary gets stored if true is returned.
    /// </param>
    ///
    /// <remarks>
    /// true if the graph summary was obtained.
    /// </remarks>
    ///
    /// <remarks>
    /// If a graph summary is available, it gets stored at <paramref
    /// name="graphSummary" /> and true is returned.  Otherwise, a message is
    /// shown and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TrySummarizeGraph
    (
        GraphHistory graphHistory,
        AutoFillWorkbookResults autoFillWorkbookResults,
        OverallMetrics overallMetrics,
        String topNByMetrics,
        String twitterSearchNetworkTopItems,
        out String graphSummary
    )
    {
        Debug.Assert(graphHistory != null);
        Debug.Assert(autoFillWorkbookResults != null);

        graphSummary = SummarizeGraph(graphHistory, autoFillWorkbookResults,
            overallMetrics, topNByMetrics, twitterSearchNetworkTopItems);

        if (graphSummary.Length == 0)
        {
            FormUtil.ShowWarning("A graph summary is not available.");
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SummarizeGraph()
    //
    /// <summary>
    /// Gets a string that summarizes the graph.
    /// </summary>
    ///
    /// <param name="graphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="autoFillWorkbookResults">
    /// Stores the results of a call to <see
    /// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
    /// </param>
    ///
    /// <param name="overallMetrics">
    /// Stores the overall metrics for a graph, or null if not available.
    /// </param>
    ///
    /// <param name="topNByMetrics">
    /// The graph's top-N-by metrics, as a descriptive string, or null if not
    /// available.
    /// </param>
    ///
    /// <param name="twitterSearchNetworkTopItems">
    /// The top items in the tweets within a Twitter search network, as a
    /// descriptive string, or null or an empty string if not available.
    /// </param>
    ///
    /// <returns>
    /// A string that summarizes the graph.  If a summary isn't available, an
    /// empty string is returned.
    /// </returns>
    //*************************************************************************

    public static String
    SummarizeGraph
    (
        GraphHistory graphHistory,
        AutoFillWorkbookResults autoFillWorkbookResults,
        OverallMetrics overallMetrics,
        String topNByMetrics,
        String twitterSearchNetworkTopItems
    )
    {
        Debug.Assert(graphHistory != null);
        Debug.Assert(autoFillWorkbookResults != null);

        StringBuilder oStringBuilder = new StringBuilder();

        AppendGraphHistoryValues(graphHistory, oStringBuilder,
            GraphHistoryKeys.ImportDescription,
            GraphHistoryKeys.GraphDirectedness,
            GraphHistoryKeys.LayoutAlgorithm
            );

        StringUtil.AppendLineAfterEmptyLine( oStringBuilder,
            autoFillWorkbookResults.ConvertToSummaryString() );

        if (overallMetrics != null)
        {
            StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
                "Overall Graph Metrics:");

            oStringBuilder.AppendLine(
                overallMetrics.ConvertToSummaryString() );
        }

        StringUtil.AppendLineAfterEmptyLine(oStringBuilder, topNByMetrics);

        StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
            twitterSearchNetworkTopItems);

        AppendGraphHistoryValues(graphHistory, oStringBuilder,
            GraphHistoryKeys.GroupingDescription,
            GraphHistoryKeys.Comments
            );

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: AppendGraphHistoryValues()
    //
    /// <summary>
    /// Appends graph history values to the graph history if the values are
    /// available.
    /// </summary>
    ///
    /// <param name="oGraphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="oStringBuilder">
    /// Object used to build the graph history.
    /// </param>
    ///
    /// <param name="asGraphHistoryKeys">
    /// Array of the keys for the values to append.
    /// </param>
    //*************************************************************************

    private static void
    AppendGraphHistoryValues
    (
        GraphHistory oGraphHistory,
        StringBuilder oStringBuilder,
        params String [] asGraphHistoryKeys
    )
    {
        Debug.Assert(oGraphHistory != null);
        Debug.Assert(oStringBuilder != null);
        Debug.Assert(asGraphHistoryKeys != null);

        String sGraphHistoryValue;

        foreach (String sGraphHistoryKey in asGraphHistoryKeys)
        {
            if ( oGraphHistory.TryGetValue(sGraphHistoryKey,
                out sGraphHistoryValue) )
            {
                StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
                    sGraphHistoryValue);
            }
        }
    }
}

}
