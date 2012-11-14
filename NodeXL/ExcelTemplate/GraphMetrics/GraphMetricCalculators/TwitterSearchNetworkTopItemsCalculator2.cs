
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkTopItemsCalculator2
//
/// <summary>
/// Calculates the top items in the tweets within a Twitter search network.
/// </summary>
///
/// <remarks>
/// This class calculates the top URLs, hashtags, replies-to and mentions in
/// the tweets within a Twitter search network, as well as the top tweeters.
/// It does so for the entire graph, as well as for the graph's top groups,
/// ranked by vertex count.
///
/// <para>
/// These top items will get written later to a new worksheet by the <see
/// cref="GraphMetricWriter" /> class.
/// </para>
///
/// <para>
/// This class also calculates top hashtags for each of the graph's groups, not
/// just the top groups.  These top hashtags will get written later to the
/// group worksheet.
/// </para>
///
/// <para>
/// If the workbook doesn't contain a Twitter search network, this class does
/// nothing.
/// </para>
///
/// <para>
/// This graph metric calculator differs from most other calculators in that it
/// reads tweet-related columns that were written to the Excel workbook by the
/// TwitterSearchNetworkImporter class.  The other calculators look only at how
/// the graph's vertices are connected to each other.  Therefore, there is no
/// corresponding lower-level TwitterSearchNetworkTopItemsCalculator class in
/// the <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the top items in a
/// Twitter search network cannot be calculated outside of this ExcelTemplate
/// project.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkTopItemsCalculator2 : TopItemsCalculatorBase2
{
    //*************************************************************************
    //  Constructor: TwitterSearchNetworkTopItemsCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterSearchNetworkTopItemsCalculator2()
    {
        m_oTwitterStatusParser = new TwitterStatusParser();

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricToCalculate
    //
    /// <summary>
    /// Gets the graph metric that will be calculated.
    /// </summary>
    ///
    /// <value>
    /// The graph metric that will be calculated, as a <see
    /// cref="GraphMetrics" /> flag.
    /// </value>
    //*************************************************************************

    protected override GraphMetrics
    GraphMetricToCalculate
    {
        get
        {
            AssertValid();

            return (GraphMetrics.TwitterSearchNetworkTopItems);
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsInternal()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    TryCalculateGraphMetricsInternal
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // Get information about each of the graph's groups, including a
        // "dummy" group for the entire graph.

        List<GroupEdgeInfo> oAllGroupEdgeInfos =
            GroupEdgeSorter.SortGroupEdges(oGraph, Int32.MaxValue, true,
                false);

        Int32 iTableIndex = 1;

        // Add the columns for the top URLs, domains and hashtags.

        AddColumnsForTweetContent(ref iTableIndex, oAllGroupEdgeInfos,
            UrlsInTweetColumnName, true,
            GroupTableColumnNames.TopUrlsInTweet, oGraphMetricColumns);

        AddColumnsForTweetContent(ref iTableIndex, oAllGroupEdgeInfos,
            DomainsInTweetColumnName, false,
            GroupTableColumnNames.TopDomainsInTweet, oGraphMetricColumns);

        AddColumnsForTweetContent(ref iTableIndex, oAllGroupEdgeInfos,
            HashtagsInTweetColumnName, false,
            GroupTableColumnNames.TopHashtagsInTweet, oGraphMetricColumns);

        // Add the columns for the top words and top word pair tables on the
        // Twitter search network top items worksheet.

        if ( !TryAddColumnsForWordsAndWordPairs(ref iTableIndex, oGraph,
            oAllGroupEdgeInfos, oCalculateGraphMetricsContext,
            oGraphMetricColumns) )
        {
            return (false);
        }

        // Add the columns for the top replied-to and top mentioned.

        AddColumnsForRepliesToAndMentions(ref iTableIndex, oAllGroupEdgeInfos,
            oGraphMetricColumns);

        // Add the columns for the top tweeters.

        AddColumnsForTweeters(ref iTableIndex, oGraph, oGraphMetricColumns);

        AdjustColumnWidths(oGraphMetricColumns, oAllGroupEdgeInfos);

        return (true);
    }

    //*************************************************************************
    //  Method: AddColumnsForTweetContent()
    //
    /// <summary>
    /// Adds "tweet content" columns.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the table on the Twitter search network top
    /// items worksheet.  This gets incremented by this method.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="sEdgeColumnName">
    /// Name of the edge column to calculate the metric from.  The column must
    /// contain space-delimited strings.  Sample: "URLs in Tweet", in which
    /// case the column contains space-delimited URLs that this method counts
    /// and ranks.
    /// </param>
    ///
    /// <param name="bEdgeColumnContainsUrls">
    /// true if the content might contains URLs.
    /// </param>
    ///
    /// <param name="sGroupTableColumnName">
    /// Name of the column to add for the group table on the group worksheet.
    /// Sample: "Top URLs in Tweet".
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a set of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The added set corresponds to
    /// one table that needs to be written to the Twitter search network top
    /// items worksheet; for example, the table that shows top URLs in tweets.
    /// The added set contains one pair of columns for each group in <paramref
    /// name="oAllGroupEdgeInfos" />.  A sample pair of columns is "Top URLs in
    /// G7" and "G7 Count", where G7 is the name of a group.
    ///
    /// <para>
    /// It also adds a column for the group worksheet.  Each cell in the column
    /// is a space-delimited list of the top strings in the group's tweets; the
    /// top URLs in the group's tweets, for example.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForTweetContent
    (
        ref Int32 iTableIndex,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        String sEdgeColumnName,
        Boolean bEdgeColumnContainsUrls,
        String sGroupTableColumnName,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert( !String.IsNullOrEmpty(sEdgeColumnName) );
        Debug.Assert(oGraphMetricColumns != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupTableColumnName) );
        AssertValid();

        List<GraphMetricValueWithID> oTopStringsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        Int32 iGroups = oAllGroupEdgeInfos.Count;
        String sTableName = GetTableName(ref iTableIndex);

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // The key is a string being counted (an URL in the tweets, for
            // example), and the value is the number of times the string was
            // found in the edges.

            Dictionary<String, Int32> oStringCounts =
                CountDelimitedStringsInEdgeColumn(oGroupEdgeInfo.Edges,
                    sEdgeColumnName);

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                // Populate two GraphMetricValueOrdered collections with the
                // top strings and their counts.

                List<GraphMetricValueOrdered> oTopStrings, oTopStringCounts;

                GetGraphMetricValues(oStringCounts, out oTopStrings,
                    out oTopStringCounts);

                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                GetGraphMetricColumnNames(sEdgeColumnName,
                    GetGroupName(oGroupEdgeInfo),
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                // Add a pair of columns for the Twitter search network top
                // items worksheet.

                AddGraphMetricColumnPair(sTableName, sGraphMetricColumn1Name,
                    sGraphMetricColumn2Name, oTopStrings, oTopStringCounts,
                    bEdgeColumnContainsUrls, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add a cell for
                // the column on the group worksheet.

                oTopStringsForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),
                    ConcatenateTopStrings(oStringCounts) ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            sGroupTableColumnName, ExcelTableUtil.AutoColumnWidth, null, null,
            oTopStringsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: AddColumnsForRepliesToAndMentions()
    //
    /// <summary>
    /// Add the columns for the top replies-to and top mentions.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The start index to use for naming the top replies-to and top mentions
    /// tables.  This gets incremented by this method.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds two sets of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The first set corresponds to
    /// a top replies-to table that needs to be written to the workbook, and
    /// the second set corresponds to a top mentions table.
    ///
    /// <para>
    /// It also adds columns for the group worksheet.  Each cell in a column is
    /// a space-delimited list of the top replies-to or mentions in the group's
    /// tweets.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForRepliesToAndMentions
    (
        ref Int32 iTableIndex,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        List<GraphMetricValueWithID> oTopRepliesToForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oTopMentionsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        String sRepliesToTableName = GetTableName(ref iTableIndex);
        String sMentionsTableName = GetTableName(ref iTableIndex);
        Int32 iGroups = oAllGroupEdgeInfos.Count;

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // The key is a screen name and the value is the number of times
            // the screen name was found as a "reply-to" in all the edges.

            Dictionary<String, Int32> oRepliesToCounts =
                new Dictionary<String, Int32>();

            // The key is a screen name and the value is the number of times
            // the screen name was found as a "mentions" in all the edges.

            Dictionary<String, Int32> oMentionsCounts =
                new Dictionary<String, Int32>();

            foreach (IEdge oEdge in oGroupEdgeInfo.Edges)
            {
                String sStatus;

                if ( TryGetStringValueForEdge(oEdge, StatusColumnName,
                    out sStatus) )
                {
                    String sReplyToScreenName;
                    String [] asMentionedScreenNames;

                    m_oTwitterStatusParser.GetScreenNames(sStatus,
                        out sReplyToScreenName, out asMentionedScreenNames);

                    if (sReplyToScreenName != null)
                    {
                        CountString(sReplyToScreenName, oRepliesToCounts);
                    }

                    foreach (String sMentionedScreenName in
                        asMentionedScreenNames)
                    {
                        CountString(sMentionedScreenName, oMentionsCounts);
                    }
                }
            }

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                String sGroupName = GetGroupName(oGroupEdgeInfo);

                // Populate GraphMetricValueOrdered collections with the top
                // replies-to and their counts.

                List<GraphMetricValueOrdered> oTopStrings, oTopStringCounts;
                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                GetGraphMetricValues(oRepliesToCounts, out oTopStrings,
                    out oTopStringCounts);

                GetGraphMetricColumnNames("Replied-To", sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sRepliesToTableName,
                    sGraphMetricColumn1Name, sGraphMetricColumn2Name,
                    oTopStrings, oTopStringCounts, false, oGraphMetricColumns);

                // Repeat for mentions.

                GetGraphMetricValues(oMentionsCounts, out oTopStrings,
                    out oTopStringCounts);

                GetGraphMetricColumnNames("Mentioned", sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sMentionsTableName,
                    sGraphMetricColumn1Name, sGraphMetricColumn2Name,
                    oTopStrings, oTopStringCounts, false, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add cells for
                // the columns on the group worksheet.

                oTopRepliesToForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),
                    ConcatenateTopStrings(oRepliesToCounts) ) );

                oTopMentionsForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),
                    ConcatenateTopStrings(oMentionsCounts) ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopRepliesToInTweet,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopRepliesToForGroupWorksheet.ToArray() ) );

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopMentionsInTweet,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopMentionsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: TryAddColumnsForWordsAndWordPairs()
    //
    /// <summary>
    /// Adds the columns for the top words and top word pair tables on the
    /// Twitter search network top items worksheet.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method adds two sets of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The sets correspond to top
    /// words and top word pairs tables that need to be written to the
    /// workbook.
    ///
    /// <para>
    /// It also adds columns for the group worksheet.  Each cell in a column is
    /// a space-delimited list of the top words or word pairs in the group's
    /// tweets.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryAddColumnsForWordsAndWordPairs
    (
        ref Int32 iTableIndex,
        IGraph oGraph,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oGraph != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // Calculate all word metrics for all the graph's groups.

        GraphMetricColumn [] oWordMetricColumns;

        if ( !TryCalculateWordMetrics(oGraph, oCalculateGraphMetricsContext,
            out oWordMetricColumns) )
        {
            return (false);
        }

        // Not all of the word metrics are needed, and they are in the wrong
        // format.  Filter and reformat them.

        FilterAndReformatWordMetrics(ref iTableIndex, oWordMetricColumns,
            oAllGroupEdgeInfos, oGraphMetricColumns);

        return (true);
    }

    //*************************************************************************
    //  Method: AddColumnsForTweeters()
    //
    /// <summary>
    /// Adds the columns for the top tweeters.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the Top Tweeters table.  This gets
    /// incremented by this method.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds one set of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The set corresponds to a Top
    /// Tweeters table that needs to be written to the workbook.
    ///
    /// <para>
    /// It also adds a column for the group worksheet.  Each cell in the column
    /// is a space-delimited list of the top tweeters in the group.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForTweeters
    (
        ref Int32 iTableIndex,
        IGraph oGraph,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        String sTweetersTableName = GetTableName(ref iTableIndex);
        const String TableDescription = "Tweeters";

        // Add a pair of columns for the entire graph.

        String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

        GetGraphMetricColumnNames(TableDescription, null,
            out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

        AddColumnsForRankedVertices(oGraph.Vertices, TweetsColumnName,
            MaximumTopItems, WorksheetNames.TwitterSearchNetworkTopItems,
            sTweetersTableName, sGraphMetricColumn1Name,
            sGraphMetricColumn2Name, oGraphMetricColumns);

        GroupInfo [] aoGroups;

        List<GraphMetricValueWithID> oTopTweetersForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        if ( GroupUtil.TryGetGroups(oGraph, out aoGroups) )
        {
            // Add a pair of columns for each of the graph's top groups.

            Int32 iGroup = 0;

            foreach ( ExcelTemplateGroupInfo oGroupInfo in
                ExcelTemplateGroupUtil.GetTopGroups(aoGroups, Int32.MaxValue) )
            {
                if (iGroup < MaximumTopGroups)
                {
                    GetGraphMetricColumnNames(TableDescription, oGroupInfo.Name,
                        out sGraphMetricColumn1Name,
                        out sGraphMetricColumn2Name);

                    AddColumnsForRankedVertices(oGroupInfo.Vertices,
                        TweetsColumnName, MaximumTopItems,
                        WorksheetNames.TwitterSearchNetworkTopItems,
                        sTweetersTableName, sGraphMetricColumn1Name,
                        sGraphMetricColumn2Name, oGraphMetricColumns);
                }

                // Add a cell to the column for the group worksheet.

                IList<ItemInformation> oItems = RankVertices(
                    oGroupInfo.Vertices, TweetsColumnName);

                Debug.Assert(oGroupInfo.RowID.HasValue);

                oTopTweetersForGroupWorksheet.Add( new GraphMetricValueWithID(
                    oGroupInfo.RowID.Value, 

                    String.Join( " ",
                        (from oItem in oItems
                        select oItem.Name).Take(MaximumTopItems).ToArray() )
                    ) );

                iGroup++;
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopTweeters,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopTweetersForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: AdjustColumnWidths()
    //
    /// <summary>
    /// Adjusts the width of the columns on the Twitter search network top
    /// items worksheet.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumns">
    /// Complete collection of GraphMetricColumn objects.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <remarks>
    /// This method forces each column in the Twitter search network top items
    /// worksheet to be the width of the longest column name in that column.
    ///
    /// <para>
    /// <see cref="GraphMetricWriter" /> "stacks" the tables created by this
    /// class on top of each other in the Twitter search network top items
    /// worksheet, so that the "Top URLs in Tweet in Entire Graph", "Top
    /// Domains in Tweet in Entire Graph", and "Top Hashtags in Tweet in Entire
    /// Graph" graph metric columns all end up in Excel column A, for example.
    /// The graph metric column widths specified by this class are all
    /// ExcelTableUtil.AutoColumnWidth, which would result in the Excel column
    /// having the width of the widest cell in the column.  Because the cells
    /// can contain long text, like URLs, the Excel column would often end up
    /// being too wide.  This method fixes that problem by changing all the
    /// graph metric column widths from AutoColumnWidth to the longest column
    /// name in the column.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AdjustColumnWidths
    (
        List<GraphMetricColumn> oGraphMetricColumns,
        List<GroupEdgeInfo> oAllGroupEdgeInfos
    )
    {
        Debug.Assert(oGraphMetricColumns != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        AssertValid();

        // Sample top group names: "Entire Graph", "G1", "G2", ..., "G10".

        var oTopGroupNames =
            (from oGroupEdgeInfo in oAllGroupEdgeInfos
                select GetGroupName(oGroupEdgeInfo) ?? EntireGraphGroupName
            ).Take(MaximumTopGroups + 1);

        // The columns in oGraphMetricColumns are not assumed to be in any
        // order.
        //
        // The first pass through the following loop selects only those columns
        // that involve the entire graph. For example:
        //
        //  "Top URLs in Tweet in Entire Graph"
        //  "Top Domains in Tweet in Entire Graph"
        //  "Top Hashtags in Tweet in Entire Graph"
        //  ...
        //
        // The second pass selects only those columns that involve the first
        // real group.  For example:
        //
        //  "Top URLs in Tweet in G1"
        //  "Top Domains in Tweet in G1"
        //  "Top Hashtags in Tweet in G1"
        //  ...
        //
        // ...and so on through all the top groups.

        foreach (String sGroupName in oTopGroupNames)
        {
            var oColumnsForGroup =

                from oGraphMetricColumn in oGraphMetricColumns

                where
                (
                    oGraphMetricColumn.TableName.StartsWith(
                        TableNames.TwitterSearchNetworkTopItemsRoot)
                    &&
                    oGraphMetricColumn.ColumnName.StartsWith("Top ")
                    &&
                    oGraphMetricColumn.ColumnName.EndsWith("in " + sGroupName)
                )
                select oGraphMetricColumn
                ;

            if (oColumnsForGroup.Count() > 0)
            {
                Double dMaximumColumnWidthChars = oColumnsForGroup.Max(
                    oGraphMetricColumn => oGraphMetricColumn.ColumnName.Length);

                foreach (GraphMetricColumn oGraphMetricColumn in
                    oColumnsForGroup)
                {
                    oGraphMetricColumn.ColumnWidthChars =
                        dMaximumColumnWidthChars;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TryCalculateWordMetrics()
    //
    /// <summary>
    /// Attempts to calculate word metrics.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oWordMetricColumns">
    /// Where the calculated word metric columns get stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the word metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateWordMetrics
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        out GraphMetricColumn [] oWordMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        AssertValid();

        // Use the WordMetricCalculator2() class to calculate word metrics for
        // all groups.
        //
        // This is somewhat wasteful, because we don't actually need all the
        // word metrics for all groups.  A future version might refactor common
        // code out of WordMetricCalculator2() that can be called by that class
        // and this one.

        GraphMetricUserSettings oGraphMetricUserSettings =
            oCalculateGraphMetricsContext.GraphMetricUserSettings;

        WordMetricUserSettings oWordMetricUserSettings =
            oGraphMetricUserSettings.WordMetricUserSettings;

        GraphMetrics eOriginalGraphMetricsToCalculate =
            oGraphMetricUserSettings.GraphMetricsToCalculate;

        Boolean bOriginalTextColumnIsOnEdgeWorksheet =
            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet;

        String sOriginalTextColumnName =
            oWordMetricUserSettings.TextColumnName;

        Boolean bOriginalCountByGroup = oWordMetricUserSettings.CountByGroup;

        oGraphMetricUserSettings.GraphMetricsToCalculate = GraphMetrics.Words;
        oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet = true;
        oWordMetricUserSettings.TextColumnName = StatusColumnName;
        oWordMetricUserSettings.CountByGroup = true;

        try
        {
            return ( ( new WordMetricCalculator2() ).TryCalculateGraphMetrics(
                oGraph, oCalculateGraphMetricsContext,
                out oWordMetricColumns) );
        }
        finally
        {
            oGraphMetricUserSettings.GraphMetricsToCalculate =
                eOriginalGraphMetricsToCalculate;

            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet =
                bOriginalTextColumnIsOnEdgeWorksheet;

            oWordMetricUserSettings.TextColumnName = sOriginalTextColumnName;
            oWordMetricUserSettings.CountByGroup = bOriginalCountByGroup;
        }
    }

    //*************************************************************************
    //  Method: FilterAndReformatWordMetrics()
    //
    /// <summary>
    /// Filters and reformats calculated word metrics.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="oWordMetricColumns">
    /// Word metric columns calculated by WordMetricCalculator2.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method takes the word metric columns calculated by
    /// WordMetricCalculator2, filters out what it doesn't need, and reformats
    /// the rest into new graph metric columns.
    /// </remarks>
    //*************************************************************************

    protected void
    FilterAndReformatWordMetrics
    (
        ref Int32 iTableIndex,
        GraphMetricColumn [] oWordMetricColumns,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oWordMetricColumns != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // This method assumes a certain column order created by
        // WordMetricCalculator2.  This can be changed in the future to search
        // for columns, but that seems wasteful for now.

        Debug.Assert(oWordMetricColumns.Length == 10);
        Debug.Assert(oWordMetricColumns[0] is GraphMetricColumnOrdered);

        Debug.Assert(oWordMetricColumns[0].ColumnName ==
            WordTableColumnNames.Word);

        Debug.Assert(oWordMetricColumns[1].ColumnName ==
            WordTableColumnNames.Count);

        Debug.Assert(oWordMetricColumns[3].ColumnName ==
            WordTableColumnNames.Group);

        Debug.Assert(oWordMetricColumns[4].ColumnName ==
            WordPairTableColumnNames.Word1);

        Debug.Assert(oWordMetricColumns[5].ColumnName ==
            WordPairTableColumnNames.Word2);

        Debug.Assert(oWordMetricColumns[6].ColumnName ==
            WordPairTableColumnNames.Count);

        Debug.Assert(oWordMetricColumns[9].ColumnName ==
            WordPairTableColumnNames.Group);

        // Filter and reformat calculated metrics for words.

        FilterAndReformatWordMetrics(ref iTableIndex, "Words in Tweet",
            (GraphMetricColumnOrdered)oWordMetricColumns[0],
            null,
            (GraphMetricColumnOrdered)oWordMetricColumns[1],
            (GraphMetricColumnOrdered)oWordMetricColumns[3],
            oAllGroupEdgeInfos, GroupTableColumnNames.TopWordsInTweet,
            oGraphMetricColumns
            );

        // Filter and reformat calculated metrics for word pairs.

        FilterAndReformatWordMetrics(ref iTableIndex, "Word Pairs in Tweet",
            (GraphMetricColumnOrdered)oWordMetricColumns[4],
            (GraphMetricColumnOrdered)oWordMetricColumns[5],
            (GraphMetricColumnOrdered)oWordMetricColumns[6],
            (GraphMetricColumnOrdered)oWordMetricColumns[9],
            oAllGroupEdgeInfos, GroupTableColumnNames.TopWordPairsInTweet,
            oGraphMetricColumns
            );
    }

    //*************************************************************************
    //  Method: FilterAndReformatWordMetrics()
    //
    /// <summary>
    /// Filters and reformats calculated metrics for words or word pairs.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="sTableDescription">
    /// Description of the table the graph metric columns will eventually be
    /// written to.  Sample: "Words in Tweet".
    /// </param>
    ///
    /// <param name="oWord1Column">
    /// Column that was calculated for either the word (for words) or word 1
    /// (for word pairs).
    /// </param>
    ///
    /// <param name="oWord2Column">
    /// Column that was calculated for word 2, or null if this is being called
    /// for words.
    /// </param>
    ///
    /// <param name="oCountColumn">
    /// Column that was calculated for the word or word pair count.
    /// </param>
    ///
    /// <param name="oGroupNameColumn">
    /// Column that was calculated for the group name.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="sGroupTableColumnName">
    /// Name of the column to add for the group table on the group worksheet.
    /// Sample: "Top Words in Tweet".
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method takes word metric columns calculated by
    /// WordMetricCalculator2 for either words or word pairs, filters out what
    /// it doesn't need, and reformats the rest into new graph metric columns.
    /// </remarks>
    //*************************************************************************

    protected void
    FilterAndReformatWordMetrics
    (
        ref Int32 iTableIndex,
        String sTableDescription,
        GraphMetricColumnOrdered oWord1Column,
        GraphMetricColumnOrdered oWord2Column,
        GraphMetricColumnOrdered oCountColumn,
        GraphMetricColumnOrdered oGroupNameColumn,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        String sGroupTableColumnName,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert( !String.IsNullOrEmpty(sTableDescription) );
        Debug.Assert(oWord1Column != null);
        // oWord2Column
        Debug.Assert(oCountColumn != null);
        Debug.Assert(oGroupNameColumn != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupTableColumnName) );
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        Boolean bIsForWords = (oWord2Column == null);

        GraphMetricValueOrdered [] aoWord1Values =
            oWord1Column.GraphMetricValuesOrdered;

        GraphMetricValueOrdered [] aoWord2Values = null;

        if (!bIsForWords)
        {
            aoWord2Values = oWord2Column.GraphMetricValuesOrdered;
        }

        GraphMetricValueOrdered [] aoCountValues =
            oCountColumn.GraphMetricValuesOrdered;

        GraphMetricValueOrdered [] aoGroupNameValues =
            oGroupNameColumn.GraphMetricValuesOrdered;

        List<GraphMetricValueWithID> oTopWordsOrWordPairsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        // The key is a group name from the word or word pair table created by
        // WordMetricCalculator2 and the value is the index of the first row
        // for that group.

        Dictionary<String, Int32> oGroupNameIndexes =
            GetGroupNameIndexesFromWordMetrics(aoGroupNameValues);

        Int32 iGroups = oAllGroupEdgeInfos.Count;
        String sTableName = GetTableName(ref iTableIndex);

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // Populate GraphMetricValueOrdered collections with the top words
            // or word pairs and their counts for this group.

            List<GraphMetricValueOrdered> oTopWordsOrWordPairs =
                new List<GraphMetricValueOrdered>();

            List<GraphMetricValueOrdered> oTopCounts =
                new List<GraphMetricValueOrdered>();

            String sGroupName = GetGroupName(oGroupEdgeInfo);

            String sGroupNameOrDummyGroupName =
                sGroupName ?? GroupEdgeSorter.DummyGroupNameForEntireGraph;

            Int32 iFirstRowForGroup;

            if ( oGroupNameIndexes.TryGetValue(sGroupNameOrDummyGroupName,
                out iFirstRowForGroup) )
            {
                CopyWordMetricsForGroup(sGroupNameOrDummyGroupName,
                    aoWord1Values, aoWord2Values, aoCountValues,
                    aoGroupNameValues, iFirstRowForGroup, oTopWordsOrWordPairs,
                    oTopCounts);
            }

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                GetGraphMetricColumnNames(sTableDescription, sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sTableName, sGraphMetricColumn1Name,
                    sGraphMetricColumn2Name, oTopWordsOrWordPairs, oTopCounts,
                    false, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add a cell for
                // the column on the group worksheet.

                oTopWordsOrWordPairsForGroupWorksheet.Add(
                    new GraphMetricValueWithID(
                        GetGroupRowID(oGroupEdgeInfo),

                        ExcelUtil.ForceCellText(
                            ConcatenateTopWordsOrWordPairs(oTopWordsOrWordPairs,
                                bIsForWords) )
                    ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups, sGroupTableColumnName,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopWordsOrWordPairsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: GetGroupNameIndexesFromWordMetrics()
    //
    /// <summary>
    /// Gets the indexes of the start of each group for words or word pairs.
    /// </summary>
    ///
    /// <param name="aoGroupNameValues">
    /// The group name values for words or word pairs.
    /// </param>
    ///
    /// <returns>
    /// The key is a group name from the word or word pair table created by
    /// WordMetricCalculator2 and the value is the index of the first row for
    /// that group.
    ///
    /// <para>
    /// Note that the group name for the "dummy" group that represents the
    /// entire graph is <see
    /// cref="GroupEdgeSorter.DummyGroupNameForEntireGraph" />.
    /// </para>
    ///
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, Int32>
    GetGroupNameIndexesFromWordMetrics
    (
        GraphMetricValueOrdered [] aoGroupNameValues
    )
    {
        Debug.Assert(aoGroupNameValues != null);
        AssertValid();

        Dictionary<String, Int32> oGroupNameIndexes =
            new Dictionary<String, Int32>();

        Int32 iRows = aoGroupNameValues.Length;
        String sCurrentGroupName = String.Empty;

        for (Int32 iRow = 0; iRow < iRows; iRow++)
        {
            Object oGroupNameAsObject = aoGroupNameValues[iRow].Value;

            if (oGroupNameAsObject is String)
            {
                String sGroupName = (String)oGroupNameAsObject;

                if (sGroupName != sCurrentGroupName)
                {
                    oGroupNameIndexes.Add(sGroupName, iRow);
                    sCurrentGroupName = sGroupName;
                }
            }
        }

        return (oGroupNameIndexes);
    }

    //*************************************************************************
    //  Method: CopyWordMetricsForGroup()
    //
    /// <summary>
    /// Copies calculated metrics for words or word pairs.
    /// </summary>
    ///
    /// <param name="sGroupNameOrDummyGroupName">
    /// Name of the group, or the dummy name if the group is the "dummy" group
    /// for the entire graph.
    /// </param>
    ///
    /// <param name="aoWord1Values">
    /// Values that were calculated for either the word (for words) or word 1
    /// (for word pairs).
    /// </param>
    ///
    /// <param name="aoWord2Values">
    /// Values that were calculated for word 2, or null if this is being called
    /// for words.
    /// </param>
    ///
    /// <param name="aoCountValues">
    /// Values that were calculated for the word or word pair count.
    /// </param>
    ///
    /// <param name="aoGroupNameValues">
    /// Values that were calculated for the group name.
    /// </param>
    ///
    /// <param name="iFirstRowForGroup">
    /// The first row for the group in the calculated values.
    /// </param>
    ///
    /// <param name="oTopWordsOrWordPairs">
    /// The collection of words or word pairs that this method fills in.
    /// </param>
    ///
    /// <param name="oTopCounts">
    /// The collection of counts that this method fills in.
    /// </param>
    ///
    /// <remarks>
    /// This method copies the word or word pair metrics that were calculated
    /// for a group to new collections.
    /// </remarks>
    //*************************************************************************

    protected void
    CopyWordMetricsForGroup
    (
        String sGroupNameOrDummyGroupName,
        GraphMetricValueOrdered [] aoWord1Values,
        GraphMetricValueOrdered [] aoWord2Values,
        GraphMetricValueOrdered [] aoCountValues,
        GraphMetricValueOrdered [] aoGroupNameValues,
        Int32 iFirstRowForGroup,
        List<GraphMetricValueOrdered> oTopWordsOrWordPairs,
        List<GraphMetricValueOrdered> oTopCounts
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sGroupNameOrDummyGroupName) );
        Debug.Assert(aoWord1Values != null);
        // aoWord2Values
        Debug.Assert(aoCountValues != null);
        Debug.Assert(aoGroupNameValues != null);
        Debug.Assert(iFirstRowForGroup >= 0);
        Debug.Assert(oTopWordsOrWordPairs != null);
        Debug.Assert(oTopCounts != null);
        AssertValid();

        Int32 iRows = aoGroupNameValues.Length;

        for (Int32 iRow = iFirstRowForGroup, iItems = 0;
            iRow < iRows && iItems < MaximumTopItems;
            iRow++, iItems++)
        {
            Object oWord1AsObject = aoWord1Values[iRow].Value;
            Object oCountAsObject = aoCountValues[iRow].Value;
            Object oGroupNameAsObject = aoGroupNameValues[iRow].Value;

            if (
                !(oGroupNameAsObject is String)
                ||
                (String)oGroupNameAsObject != sGroupNameOrDummyGroupName
                ||
                !(oWord1AsObject is String)
                ||
                !(oCountAsObject is Int32)
                )
            {
                break;
            }

            String sWordOrWordPair = ExcelUtil.UnforceCellText(
                (String)oWord1AsObject );

            if (aoWord2Values != null)
            {
                Object oWord2AsObject = aoWord2Values[iRow].Value;

                if ( !(oWord2AsObject is String) )
                {
                    break;
                }

                sWordOrWordPair = FormatWordPair( sWordOrWordPair,
                    ExcelUtil.UnforceCellText( (String)oWord2AsObject ) );
            }

            oTopWordsOrWordPairs.Add( new GraphMetricValueOrdered(
                ExcelUtil.ForceCellText(sWordOrWordPair) ) );

            oTopCounts.Add( new GraphMetricValueOrdered(oCountAsObject) );
        }
    }

    //*************************************************************************
    //  Method: ConcatenateTopWordsOrWordPairs()
    //
    /// <summary>
    /// Concatenates a list of top words or word pairs.
    /// </summary>
    ///
    /// <param name="oTopWordsOrWordPairs">
    /// The top words or word pairs to concatenate.
    /// </param>
    ///
    /// <param name="bWords">
    /// true if <paramref name="oTopWordsOrWordPairs" /> contains words, false
    /// if it contains word pairs.
    /// </param>
    ///
    /// <returns>
    /// The top words or word pairs concatenated with a space.
    /// </returns>
    //*************************************************************************

    protected String
    ConcatenateTopWordsOrWordPairs
    (
        List<GraphMetricValueOrdered> oTopWordsOrWordPairs,
        Boolean bWords
    )
    {
        Debug.Assert(oTopWordsOrWordPairs != null);
        AssertValid();

        // An extra space is added between word pairs for legibility.

        return ( String.Join( bWords ? " " : "  ",

            (from oGroupMetricValueWithID in oTopWordsOrWordPairs
            select ExcelUtil.UnforceCellText(
                (String)oGroupMetricValueWithID.Value ) ).Take(
                    MaximumTopItems).ToArray() ) );
    }

    //*************************************************************************
    //  Method: FormatWordPair()
    //
    /// <summary>
    /// Formats a word pair.
    /// </summary>
    ///
    /// <param name="sWord1">
    /// The first word.
    /// </param>
    ///
    /// <param name="sWord2">
    /// The second word.
    /// </param>
    ///
    /// <returns>
    /// A formatted word pair.
    /// </returns>
    //*************************************************************************

    protected String
    FormatWordPair
    (
        String sWord1,
        String sWord2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWord1) );
        Debug.Assert( !String.IsNullOrEmpty(sWord2) );
        AssertValid();

        return (sWord1 + "," + sWord2);
    }

    //*************************************************************************
    //  Method: TryGetStringValueForEdge()
    //
    /// <summary>
    /// Attempts to get a non-empty String metadata value from an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the metadata value from.
    /// </param>
    ///
    /// <param name="sKey">
    /// The value's key.
    /// </param>
    ///
    /// <param name="sValue">
    /// Where the non-empty string value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the value was found.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetStringValueForEdge
    (
        IEdge oEdge,
        String sKey,
        out String sValue
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        Object oValue;

        if (
            oEdge.TryGetValue(sKey, typeof(String), out oValue)
            &&
            oValue != null
            )
        {
            sValue = (String)oValue;
            return (sValue.Length > 0);
        }

        sValue = null;
        return (false);
    }

    //*************************************************************************
    //  Method: GetTableName()
    //
    /// <summary>
    /// Gets the name to use for a new table on the Twitter search network top
    /// items worksheet.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index of the table that will be added.  This gets incremented by
    /// this method.
    /// </param>
    ///
    /// <returns>
    /// The name to use for a new table.
    /// </returns>
    //*************************************************************************

    protected String
    GetTableName
    (
        ref Int32 iTableIndex
    )
    {
        Debug.Assert(iTableIndex >= 1);
        AssertValid();

        return (TableNames.TwitterSearchNetworkTopItemsRoot + iTableIndex++);
    }

    //*************************************************************************
    //  Method: GetGroupName()
    //
    /// <summary>
    /// Gets a group name from a GroupEdgeInfo object.
    /// </summary>
    ///
    /// <param name="oGroupEdgeInfo">
    /// The object to get a group name from.
    /// </param>
    ///
    /// <returns>
    /// The group name, or null if <paramref name="oGroupEdgeInfo" /> is the
    /// "dummy" group that represents the entire graph.
    /// </returns>
    //*************************************************************************

    protected String
    GetGroupName
    (
        GroupEdgeInfo oGroupEdgeInfo
    )
    {
        AssertValid();

        return ( (oGroupEdgeInfo.GroupInfo == null) ?
            null : oGroupEdgeInfo.GroupInfo.Name );
    }

    //*************************************************************************
    //  Method: GetGroupRowID()
    //
    /// <summary>
    /// Gets the row ID for a group.
    /// </summary>
    ///
    /// <param name="oGroupEdgeInfo">
    /// Information about the group to get the row ID for.
    /// </param>
    ///
    /// <returns>
    /// The row ID for the specified group.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetGroupRowID
    (
        GroupEdgeInfo oGroupEdgeInfo
    )
    {
        Debug.Assert(oGroupEdgeInfo != null);
        Debug.Assert(oGroupEdgeInfo.GroupInfo.RowID.HasValue);
        AssertValid();

        return (oGroupEdgeInfo.GroupInfo.RowID.Value);
    }

    //*************************************************************************
    //  Method: GetGraphMetricColumnNames()
    //
    /// <summary>
    /// Gets the column names to use for a pair of graph metric columns.
    /// </summary>
    ///
    /// <param name="sTableDescription">
    /// Description of the table the graph metric columns will eventually be
    /// written to.  Sample: "URLs in Tweet".
    /// </param>
    ///
    /// <param name="sGroupName">
    /// Name of the group to which the pair of graph metric columns
    /// corresponds, or null if it's the "dummy" group that represents the
    /// entire graph.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn1Name">
    /// Where the name of the first graph metric column gets stored.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn2Name">
    /// Where the name of the second graph metric column gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetGraphMetricColumnNames
    (
        String sTableDescription,
        String sGroupName,
        out String sGraphMetricColumn1Name,
        out String sGraphMetricColumn2Name
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableDescription) );
        AssertValid();

        Boolean bEntireGraph = (sGroupName == null);

        sGraphMetricColumn1Name = String.Format(
            "Top {0} in {1}"
            ,
            sTableDescription,
            bEntireGraph ? EntireGraphGroupName : sGroupName
            );

        sGraphMetricColumn2Name =
            (bEntireGraph ? EntireGraphGroupName : sGroupName) + " Count";
    }

    //*************************************************************************
    //  Method: GetGraphMetricValues()
    //
    /// <summary>
    /// Gets a pair of GraphMetricValueOrdered collections populated with top
    /// strings and their counts.
    /// </summary>
    ///
    /// <param name="oStringCounts">
    /// The key is a string being counted and the value is the number of times
    /// the string was found.
    /// </param>
    ///
    /// <param name="oTopStrings">
    /// Where this method stores the top strings in <paramref
    /// name="oStringCounts" />.
    /// </param>
    ///
    /// <param name="oTopStringCounts">
    /// Where this method stores the counts for the top strings in <paramref
    /// name="oStringCounts" />.
    /// </param>
    //*************************************************************************

    protected void
    GetGraphMetricValues
    (
        Dictionary<String, Int32> oStringCounts,
        out List<GraphMetricValueOrdered> oTopStrings,
        out List<GraphMetricValueOrdered> oTopStringCounts
    )
    {
        Debug.Assert(oStringCounts != null);
        AssertValid();

        oTopStrings = new List<GraphMetricValueOrdered>();
        oTopStringCounts = new List<GraphMetricValueOrdered>();

        // Sort the Dictionary by descending string counts.

        foreach ( String sKey in GetTopStrings(oStringCounts) )
        {
            oTopStrings.Add( new GraphMetricValueOrdered(sKey) );

            oTopStringCounts.Add(
                new GraphMetricValueOrdered( oStringCounts[sKey] ) );
        }
    }

    //*************************************************************************
    //  Method: AddGraphMetricColumnPair()
    //
    /// <summary>
    /// Adds a pair of GraphMetricColumn objects to a collection.
    /// </summary>
    ///
    /// <param name="sTableName">
    /// The name of the table the columns will eventually be written to.  The
    /// table is on the Twitter search network top items worksheet.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn1Name">
    /// The name of the first graph metric column.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn2Name">
    /// The name of the second graph metric column.
    /// </param>
    ///
    /// <param name="oTopStrings">
    /// Collection of top strings.
    /// </param>
    ///
    /// <param name="oTopStringCounts">
    /// Collection of top string counts.
    /// </param>
    ///
    /// <param name="bTopStringsContainUrls">
    /// true if the collection of top strings might contains URLs.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    //*************************************************************************

    protected void
    AddGraphMetricColumnPair
    (
        String sTableName,
        String sGraphMetricColumn1Name,
        String sGraphMetricColumn2Name,
        List<GraphMetricValueOrdered> oTopStrings,
        List<GraphMetricValueOrdered> oTopStringCounts,
        Boolean bTopStringsContainUrls,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn1Name) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn2Name) );
        Debug.Assert(oTopStrings != null);
        Debug.Assert(oTopStringCounts != null);
        Debug.Assert(oTopStrings.Count == oTopStringCounts.Count);
        AssertValid();

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            WorksheetNames.TwitterSearchNetworkTopItems, sTableName,
            sGraphMetricColumn1Name, ExcelTableUtil.AutoColumnWidth, null,
            null, oTopStrings.ToArray()
            ) );

        if (bTopStringsContainUrls)
        {
            oGraphMetricColumns[oGraphMetricColumns.Count - 1]
                .ConvertUrlsToHyperlinks = true;
        }

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            WorksheetNames.TwitterSearchNetworkTopItems, sTableName,
            sGraphMetricColumn2Name, ExcelTableUtil.AutoColumnWidth, null,
            null, oTopStringCounts.ToArray()
            ) );
    }

    //*************************************************************************
    //  Method: CountDelimitedStringsInEdgeColumn()
    //
    /// <summary>
    /// Counts the number of times each string was found in a specified edge
    /// column.
    /// </summary>
    ///
    /// <param name="oEdges">
    /// The edges to calculate the metrics from.
    /// </param>
    ///
    /// <param name="sEdgeColumnName">
    /// Name of the edge column to calculate the metric from.  The column must
    /// contain space-delimited strings.  Sample: "URLs in Tweet", in which
    /// case the column contains space-delimited URLs that this method counts
    /// and ranks.
    /// </param>
    ///
    /// <returns>
    /// A Dictionary in which the key is a string being counted (an URL in the
    /// tweets, for example), and the value is the number of times the string
    /// was found in the edge column.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, Int32>
    CountDelimitedStringsInEdgeColumn
    (
        IEnumerable<IEdge> oEdges,
        String sEdgeColumnName
    )
    {
        Debug.Assert(oEdges != null);
        Debug.Assert( !String.IsNullOrEmpty(sEdgeColumnName) );
        AssertValid();

        // The key is a string being counted (an URL in a Tweet, for example),
        // and the value is the number of times the string was found in the
        // edge column.

        Dictionary<String, Int32> oStringCounts =
            new Dictionary<String, Int32>();

        foreach (IEdge oEdge in oEdges)
        {
            String sSpaceDelimitedCellValue;

            if ( TryGetStringValueForEdge(oEdge, sEdgeColumnName, 
                out sSpaceDelimitedCellValue) )
            {
                foreach ( String sString in sSpaceDelimitedCellValue.Split(
                    new Char[] {' '}, StringSplitOptions.RemoveEmptyEntries) )
                {
                    CountString(sString, oStringCounts);
                }
            }
        }

        return (oStringCounts);
    }

    //*************************************************************************
    //  Method: GetTopStrings()
    //
    /// <summary>
    /// Gets the top string counts in a dictionary of strings.
    /// </summary>
    ///
    /// <param name="oStringCounts">
    /// The key is a string being counted (an URL in the tweets, for example),
    /// and the value is the number of times the string was found in the edge
    /// column.
    /// </param>
    ///
    /// <returns>
    /// The top strings being counted, sorted by the number of times the string
    /// was found in the edge column.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<String>
    GetTopStrings
    (
        Dictionary<String, Int32> oStringCounts
    )
    {
        Debug.Assert(oStringCounts != null);
        AssertValid();

        // Sort the Dictionary by descending string counts.

        return (
            (from sKey in oStringCounts.Keys
            orderby oStringCounts[sKey] descending
            select sKey).Take(MaximumTopItems)
            );
    }

    //*************************************************************************
    //  Method: ConcatenateTopStrings()
    //
    /// <summary>
    /// Concatenates the top strings from a dictionary of string counts.
    /// </summary>
    ///
    /// <param name="oStringCounts">
    /// The key is a string and the value is the number of times the string was
    /// found.
    /// </param>
    ///
    /// <remarks>
    /// The top strings in <paramref name="oStringCounts" />, separated by
    /// spaces.
    /// </remarks>
    //*************************************************************************

    protected String
    ConcatenateTopStrings
    (
        Dictionary<String, Int32> oStringCounts
    )
    {
        Debug.Assert(oStringCounts != null);
        AssertValid();

        return ( String.Join( " ", GetTopStrings(oStringCounts).ToArray() ) );
    }

    //*************************************************************************
    //  Method: CountString()
    //
    /// <summary>
    /// Adds 1 to the count for a string in a dictionary of string counts.
    /// </summary>
    ///
    /// <param name="sString">
    /// The string to count.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="oStringCounts">
    /// The key is a string being counted and the value is the number of times
    /// the string was found.
    /// </param>
    //*************************************************************************

    protected void
    CountString
    (
        String sString,
        Dictionary<String, Int32> oStringCounts
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sString) );
        Debug.Assert(oStringCounts != null);
        AssertValid();

        Int32 iStringCount;

        if ( oStringCounts.TryGetValue(sString, out iStringCount) )
        {
            iStringCount++;
        }
        else
        {
            iStringCount = 1;
        }

        oStringCounts[sString] = iStringCount;
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oTwitterStatusParser != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The number of top groups to calculate metrics for, when the groups are
    /// sorted by descending vertex count.

    protected const Int32 MaximumTopGroups = 10;

    /// Maximum number of top items to include.

    protected const Int32 MaximumTopItems = 10;


    /// Column names on the edge table.

    protected const String StatusColumnName = "Tweet";
    ///
    protected const String UrlsInTweetColumnName = "URLs in Tweet";
    ///
    protected const String DomainsInTweetColumnName = "Domains in Tweet";
    ///
    protected const String HashtagsInTweetColumnName = "Hashtags in Tweet";


    /// Column names on the vertex table.

    protected const String TweetsColumnName = "Tweets";

    /// Display text for the dummy group that represents the entire graph.

    protected const String EntireGraphGroupName = "Entire Graph";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Parses the text of a Twitter tweet.  This class uses only one instance
    /// to avoid making TwitterStatusParser recompile all of its regular
    /// expressions.

    protected TwitterStatusParser m_oTwitterStatusParser;
}

}
