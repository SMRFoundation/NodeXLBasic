
using System;
using System.Text;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Common;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: OverallMetrics
//
/// <summary>
/// Stores the overall metrics for a graph.
/// </summary>
//*****************************************************************************

public class OverallMetrics : Object
{
    //*************************************************************************
    //  Constructor: OverallMetrics()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OverallMetrics" /> class.
    /// </summary>
    ///
    /// <param name="directedness">
    /// The graph's directedness.
    /// </param>
    ///
    /// <param name="uniqueEdges">
    /// The number of unique edges.
    /// </param>
    ///
    /// <param name="edgesWithDuplicates">
    /// The number of edges that have duplicates.
    /// </param>
    ///
    /// <param name="selfLoops">
    /// The number of self-loops.
    /// </param>
    ///
    /// <param name="vertices">
    /// The number of vertices.
    /// </param>
    ///
    /// <param name="graphDensity">
    /// The graph's density, or null if not available.
    /// </param>
    ///
    /// <param name="modularity">
    /// The graph's modularity, or null if not available.
    /// </param>
    ///
    /// <param name="connectedComponents">
    /// The number of connected components in the graph.
    /// </param>
    ///
    /// <param name="singleVertexConnectedComponents">
    /// The number of connected components in the graph that have one vertex.
    /// </param>
    ///
    /// <param name="maximumConnectedComponentVertices">
    /// The maximum number of vertices in a connected component.
    /// </param>
    ///
    /// <param name="maximumConnectedComponentEdges">
    /// The maximum number of edges in a connected component.
    /// </param>
    ///
    /// <param name="maximumGeodesicDistance">
    /// The maximum geodesic distance in the graph, or null if not available.
    /// </param>
    ///
    /// <param name="averageGeodesicDistance">
    /// The average geodesic distance in the graph, or null if not available.
    /// </param>
    ///
    /// <param name="reciprocatedVertexPairRatio">
    /// The ratio of the number of vertex pairs that are connected with
    /// directed edges in both directions to the number of vertex pairs that
    /// are connected with any directed edge, or null if not available.
    /// </param>
    ///
    /// <param name="reciprocatedEdgeRatio">
    /// The ratio of directed edges that have a reciprocated edge to the total
    /// number of directed edges, or null if not available.
    /// </param>
    //*************************************************************************

    public OverallMetrics
    (
        GraphDirectedness directedness,
        Int32 uniqueEdges,
        Int32 edgesWithDuplicates,
        Int32 selfLoops,
        Int32 vertices,
        Nullable<Double> graphDensity,
        Nullable<Double> modularity,
        Int32 connectedComponents,
        Int32 singleVertexConnectedComponents,
        Int32 maximumConnectedComponentVertices,
        Int32 maximumConnectedComponentEdges,
        Nullable<Int32> maximumGeodesicDistance,
        Nullable<Double> averageGeodesicDistance,
        Nullable<Double> reciprocatedVertexPairRatio,
        Nullable<Double> reciprocatedEdgeRatio
    )
    {
        m_eDirectedness = directedness;
        m_iUniqueEdges = uniqueEdges;
        m_iEdgesWithDuplicates = edgesWithDuplicates;
        m_iSelfLoops = selfLoops;
        m_iVertices = vertices;
        m_dGraphDensity = graphDensity;
        m_dModularity = modularity;
        m_iConnectedComponents = connectedComponents;
        m_iSingleVertexConnectedComponents = singleVertexConnectedComponents;

        m_iMaximumConnectedComponentVertices =
            maximumConnectedComponentVertices;

        m_iMaximumConnectedComponentEdges = maximumConnectedComponentEdges;
        m_iMaximumGeodesicDistance = maximumGeodesicDistance;
        m_dAverageGeodesicDistance = averageGeodesicDistance;
        m_dReciprocatedVertexPairRatio = reciprocatedVertexPairRatio;
        m_dReciprocatedEdgeRatio = reciprocatedEdgeRatio;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Directedness
    //
    /// <summary>
    /// Gets the graph's directedness.
    /// </summary>
    ///
    /// <value>
    /// The graph's directedness.
    /// </value>
    //*************************************************************************

    public GraphDirectedness
    Directedness
    {
        get
        {
            AssertValid();

            return (m_eDirectedness);
        }
    }

    //*************************************************************************
    //  Property: UniqueEdges
    //
    /// <summary>
    /// Gets the number of unique edges.
    /// </summary>
    ///
    /// <value>
    /// The number of unique edges.
    /// </value>
    //*************************************************************************

    public Int32
    UniqueEdges
    {
        get
        {
            AssertValid();

            return (m_iUniqueEdges);
        }
    }

    //*************************************************************************
    //  Property: EdgesWithDuplicates
    //
    /// <summary>
    /// Gets the number of edges that have duplicates.
    /// </summary>
    ///
    /// <value>
    /// The number of edges that have duplicates.
    /// </value>
    //*************************************************************************

    public Int32
    EdgesWithDuplicates
    {
        get
        {
            AssertValid();

            return (m_iEdgesWithDuplicates);
        }
    }

    //*************************************************************************
    //  Property: TotalEdges
    //
    /// <summary>
    /// Gets the total number of edges.
    /// </summary>
    ///
    /// <value>
    /// The total number of edges.
    /// </value>
    //*************************************************************************

    public Int32
    TotalEdges
    {
        get
        {
            AssertValid();

            return (m_iUniqueEdges + m_iEdgesWithDuplicates);
        }
    }

    //*************************************************************************
    //  Property: SelfLoops
    //
    /// <summary>
    /// Gets the number of self-loops.
    /// </summary>
    ///
    /// <value>
    /// The number of self-loops.
    /// </value>
    //*************************************************************************

    public Int32
    SelfLoops
    {
        get
        {
            AssertValid();

            return (m_iSelfLoops);
        }
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the number of vertices.
    /// </summary>
    ///
    /// <value>
    /// The number of vertices.
    /// </value>
    //*************************************************************************

    public Int32
    Vertices
    {
        get
        {
            AssertValid();

            return (m_iVertices);
        }
    }

    //*************************************************************************
    //  Property: GraphDensity
    //
    /// <summary>
    /// Gets the graph's density.
    /// </summary>
    ///
    /// <value>
    /// The graph's density, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    GraphDensity
    {
        get
        {
            AssertValid();

            return (m_dGraphDensity);
        }
    }

    //*************************************************************************
    //  Property: Modularity
    //
    /// <summary>
    /// Gets the graph's modularity.
    /// </summary>
    ///
    /// <value>
    /// The graph's modularity, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    Modularity
    {
        get
        {
            AssertValid();

            return (m_dModularity);
        }
    }

    //*************************************************************************
    //  Property: ConnectedComponents
    //
    /// <summary>
    /// Gets the number of connected components in the graph.
    /// </summary>
    ///
    /// <value>
    /// The number of connected components in the graph.
    /// </value>
    //*************************************************************************

    public Int32
    ConnectedComponents
    {
        get
        {
            AssertValid();

            return (m_iConnectedComponents);
        }
    }

    //*************************************************************************
    //  Property: SingleVertexConnectedComponents
    //
    /// <summary>
    /// Gets the number of connected components in the graph that have one
    /// vertex.
    /// </summary>
    ///
    /// <value>
    /// The number of connected components in the graph that have one vertex.
    /// </value>
    //*************************************************************************

    public Int32
    SingleVertexConnectedComponents
    {
        get
        {
            AssertValid();

            return (m_iSingleVertexConnectedComponents);
        }
    }

    //*************************************************************************
    //  Property: MaximumConnectedComponentVertices
    //
    /// <summary>
    /// Gets the maximum number of vertices in a connected component.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices in a connected component.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumConnectedComponentVertices
    {
        get
        {
            AssertValid();

            return (m_iMaximumConnectedComponentVertices);
        }
    }

    //*************************************************************************
    //  Property: MaximumConnectedComponentEdges
    //
    /// <summary>
    /// Gets the maximum number of edges in a connected component.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of edges in a connected component.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumConnectedComponentEdges
    {
        get
        {
            AssertValid();

            return (m_iMaximumConnectedComponentEdges);
        }
    }

    //*************************************************************************
    //  Property: MaximumGeodesicDistance
    //
    /// <summary>
    /// Gets the maximum geodesic distance in the graph.
    /// </summary>
    ///
    /// <value>
    /// The maximum geodesic distance in the graph, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Int32>
    MaximumGeodesicDistance
    {
        get
        {
            AssertValid();

            return (m_iMaximumGeodesicDistance);
        }
    }

    //*************************************************************************
    //  Property: AverageGeodesicDistance
    //
    /// <summary>
    /// Gets the average geodesic distance in the graph.
    /// </summary>
    ///
    /// <value>
    /// The average geodesic distance in the graph, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    AverageGeodesicDistance
    {
        get
        {
            AssertValid();

            return (m_dAverageGeodesicDistance);
        }
    }

    //*************************************************************************
    //  Property: ReciprocatedVertexPairRatio
    //
    /// <summary>
    /// Gets the reciprocated vertex pair ratio.
    /// </summary>
    ///
    /// <value>
    /// The ratio of the number of vertex pairs that are connected with
    /// directed edges in both directions to the number of vertex pairs that
    /// are connected with any directed edge, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    ReciprocatedVertexPairRatio
    {
        get
        {
            AssertValid();

            return (m_dReciprocatedVertexPairRatio);
        }
    }

    //*************************************************************************
    //  Property: ReciprocatedEdgeRatio
    //
    /// <summary>
    /// Gets the reciprocated edge ratio.
    /// </summary>
    ///
    /// <value>
    /// The ratio of directed edges that have a reciprocated edge to the total
    /// number of directed edges, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    ReciprocatedEdgeRatio
    {
        get
        {
            AssertValid();

            return (m_dReciprocatedEdgeRatio);
        }
    }

    //*************************************************************************
    //  Method: ConvertToSummaryString()
    //
    /// <summary>
    /// Converts the object to a string that can be used in a graph summary.
    /// </summary>
    ///
    /// <returns>
    /// The object converted to a String that can be used in a graph summary.
    /// </returns>
    //*************************************************************************

    public String
    ConvertToSummaryString()
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.Vertices, m_iVertices);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.UniqueEdges, m_iUniqueEdges);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.EdgesWithDuplicates, m_iEdgesWithDuplicates);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.TotalEdges, this.TotalEdges);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.SelfLoops, m_iSelfLoops);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.ReciprocatedVertexPairRatio,

            NullableToOverallMetricValue<Double>(
                m_dReciprocatedVertexPairRatio)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.ReciprocatedEdgeRatio,
            NullableToOverallMetricValue<Double>(m_dReciprocatedEdgeRatio)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.ConnectedComponents, m_iConnectedComponents);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.SingleVertexConnectedComponents,
            m_iSingleVertexConnectedComponents);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.MaximumConnectedComponentVertices,
            m_iMaximumConnectedComponentVertices);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.MaximumConnectedComponentEdges,
            m_iMaximumConnectedComponentEdges);

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.MaximumGeodesicDistance,
            NullableToOverallMetricValue<Int32>(m_iMaximumGeodesicDistance)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.AverageGeodesicDistance,
            NullableToOverallMetricValue<Double>(m_dAverageGeodesicDistance)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.GraphDensity,
            NullableToOverallMetricValue<Double>(m_dGraphDensity)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.Modularity,
            NullableToOverallMetricValue<Double>(m_dModularity)
            );

        AppendOverallMetricToSummaryString(oStringBuilder,
            OverallMetricNames.NodeXLVersion,
            AssemblyUtil2.GetFileVersion()
            );

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: AppendOverallMetricToSummaryString()
    //
    /// <summary>
    /// Appends an overall metric to a graph summary string.
    /// </summary>
    ///
    /// <param name="oStringBuilder">
    /// StringBuilder used to build the graph summary.
    /// </param>
    ///
    /// <param name="sOverallMetricName">
    /// Name of the overall metric.
    /// </param>
    ///
    /// <param name="oOverallMetricValue">
    /// Value of the overall metric.
    /// </param>
    //*************************************************************************

    protected void
    AppendOverallMetricToSummaryString
    (
        StringBuilder oStringBuilder,
        String sOverallMetricName,
        Object oOverallMetricValue
    )
    {
        Debug.Assert(oStringBuilder != null);
        Debug.Assert( !String.IsNullOrEmpty(sOverallMetricName) );
        Debug.Assert(oOverallMetricValue != null);
        AssertValid();

        if (oStringBuilder.Length > 0)
        {
            oStringBuilder.AppendLine();
        }

        oStringBuilder.AppendFormat(
            "{0}: {1}"
            ,
            sOverallMetricName,
            oOverallMetricValue
            );
    }

    //*************************************************************************
    //  Method: NullableToOverallMetricValue()
    //
    /// <summary>
    /// Converts a Nullable overall metric to a string value to be used in a
    /// graph summary.
    /// </summary>
    ///
    /// <param name="oNullable">
    /// The Nullable to convert.
    /// </param>
    ///
    /// <returns>
    /// If the Nullable has a value, the value in string form is returned.
    /// Otherwise, a "not applicable" string is returned.
    /// </returns>
    //*************************************************************************

    protected String
    NullableToOverallMetricValue<T>
    (
        Nullable<T> oNullable
    )
    where T : struct
    {
        if (oNullable.HasValue)
        {
            return ( oNullable.Value.ToString() );
        }

        return (GraphMetricCalculatorBase.NotApplicableMessage);
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
        // m_eDirectedness
        Debug.Assert(m_iUniqueEdges >= 0);
        Debug.Assert(m_iEdgesWithDuplicates >= 0);
        Debug.Assert(m_iSelfLoops >= 0);
        Debug.Assert(m_iVertices >= 0);
        Debug.Assert(!m_dGraphDensity.HasValue || m_dGraphDensity.Value >= 0);
        Debug.Assert(m_iConnectedComponents >= 0);
        Debug.Assert(m_iSingleVertexConnectedComponents >= 0);
        Debug.Assert(m_iMaximumConnectedComponentVertices >= 0);
        Debug.Assert(m_iMaximumConnectedComponentEdges >= 0);

        Debug.Assert(!m_iMaximumGeodesicDistance.HasValue ||
            m_iMaximumGeodesicDistance.Value >= 0);

        Debug.Assert(!m_dAverageGeodesicDistance.HasValue ||
            m_dAverageGeodesicDistance.Value >= 0);

        Debug.Assert(!m_dReciprocatedVertexPairRatio.HasValue ||
            m_dReciprocatedVertexPairRatio.Value >= 0);

        Debug.Assert(!m_dReciprocatedEdgeRatio.HasValue ||
            m_dReciprocatedEdgeRatio.Value >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph's directedness.

    protected GraphDirectedness m_eDirectedness;

    /// The number of unique edges.

    protected Int32 m_iUniqueEdges;

    /// The number of edges that have duplicates.

    protected Int32 m_iEdgesWithDuplicates;

    /// The number of self-loops.

    protected Int32 m_iSelfLoops;

    /// The number of vertices.

    protected Int32 m_iVertices;

    /// The graph's density, or null.

    protected Nullable<Double> m_dGraphDensity;

    /// The graph's modularity, or null.

    protected Nullable<Double> m_dModularity;

    /// The number of connected components in the graph.
    
    protected Int32 m_iConnectedComponents;

    /// The number of connected components in the graph that have one vertex.

    protected Int32 m_iSingleVertexConnectedComponents;

    /// The maximum number of vertices in a connected component.

    protected Int32 m_iMaximumConnectedComponentVertices;

    /// The maximum number of edges in a connected component.

    protected Int32 m_iMaximumConnectedComponentEdges;

    /// The maximum geodesic distance in the graph, or null.

    protected Nullable<Int32> m_iMaximumGeodesicDistance;

    /// The average geodesic distance in the graph, or null.

    protected Nullable<Double> m_dAverageGeodesicDistance;

    /// The ratio of the number of vertex pairs that are connected with
    /// directed edges in both directions to the number of vertex pairs that
    /// are connected with any directed edge, or null.

    protected Nullable<Double> m_dReciprocatedVertexPairRatio;

    /// The ratio of directed edges that have a reciprocated edge to the total
    /// number of directed edges, or null.

    protected Nullable<Double> m_dReciprocatedEdgeRatio;
}


//*****************************************************************************
//  Class: OverallMetricNames
//
/// <summary>
/// Provides names to use for overall metrics.
/// </summary>
///
/// <remarks>
/// All overall metric names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class OverallMetricNames
{
    ///
    public const String Directedness = "Graph Type";
    ///
    public const String UniqueEdges = "Unique Edges";
    ///
    public const String EdgesWithDuplicates = "Edges With Duplicates";
    ///
    public const String SelfLoops = "Self-Loops";
    ///
    public const String TotalEdges = "Total Edges";
    ///
    public const String Vertices = "Vertices";
    ///
    public const String GraphDensity = "Graph Density";
    ///
    public const String Modularity = "Modularity";
    ///
    public const String ConnectedComponents = "Connected Components";
    ///
    public const String SingleVertexConnectedComponents =
        "Single-Vertex Connected Components";
    ///
    public const String MaximumConnectedComponentVertices =
        "Maximum Vertices in a Connected Component";
    ///
    public const String MaximumConnectedComponentEdges =
        "Maximum Edges in a Connected Component";
    ///
    public const String MaximumGeodesicDistance =
        "Maximum Geodesic Distance (Diameter)";
    ///
    public const String AverageGeodesicDistance =
        "Average Geodesic Distance";
    ///
    public const String ReciprocatedVertexPairRatio =
        "Reciprocated Vertex Pair Ratio";
    ///
    public const String ReciprocatedEdgeRatio =
        "Reciprocated Edge Ratio";
    ///
    public const String NodeXLVersion =
        "NodeXL Version";
}

//*****************************************************************************
//  Class: OverallMetricTooltips
//
/// <summary>
/// Provides tooltips to use for overall metrics.
/// </summary>
///
/// <remarks>
/// All overall metric tooltips are available as public constants.
/// </remarks>
//*****************************************************************************

public static class OverallMetricTooltips
{
    ///
    public const String Directedness = "Directedness of the graph.";
    ///
    public const String UniqueEdges = "An edge is a connection between two vertices." +
                                      "\r\n\r\nThe \"unique\" edges is the number of connections " +
                                      "where multiple connections between A and B are counted only once.";
    ///
    public const String EdgesWithDuplicates = "An edge is a connection between two vertices.\r\n\r\n" +
                                                "The \"duplicate\" edges count is the total number of " +
                                                "multiple connections between two vertices.";
    ///
    public const String SelfLoops = "An edge that starts and ends in the same vertex is a self loop.";
    ///
    public const String TotalEdges = "An edge is a connection between two vertices.\r\n\r\n" +
                                     "The \"total\" edges count is the total number of connections " +
                                     "where multiple connections between A and B are all counted.";
    ///
    public const String Vertices = "A vertex is an element of a network.\r\n\r\n" +
                                   "The vertices count is the number of people or things in the network.";
    ///
    public const String GraphDensity = "Density is the measure of the number edges among a group " +
                                       "of vertices over the total possible number if everyone was " +
                                       "connected to everyone.\r\n\r\nA high graph density means " +
                                       "that most people are connected to many others. A low graph " +
                                       "density means that most people are not connected to many others.";
    ///
    public const String Modularity = "Modularity is a measure of the fitness of the groups that are " +
                                     "created in a clustered network.\r\n\r\n" +
                                     "A group is a set of vertices.\r\n" +
                                     "Many group cluster algorithms are intended to find sets of " +
                                     "vertices that are strongly connected and relatively separate " +
                                     "from other strongly connected groups.\r\n\r\n" +
                                     "Modularity is the measure of the number of edges that leave " +
                                     "a group to connect to vertices in a different group.\r\n" +
                                     "If modularity is high, the clusters or groups created may be " +
                                     "of low quality.  If modularity is low, the groups are well defined.";
    ///
    public const String ConnectedComponents = "A group of vertices that are all connected is a component.\r\n\r\n" +
                                              "This is the number of separate sets of connected vertices.";
    ///
    public const String SingleVertexConnectedComponents =
        "A vertex that has zero connections is \"isolated\" or an \"island\".\r\n\r\n" +
        "This is the count of vertices that have zero connections.";
    ///
    public const String MaximumConnectedComponentVertices =
        "A connected component is composed of a number of vertices.\r\n\r\n" +
        "This is the count of vertices in the largest connected component.";
    ///
    public const String MaximumConnectedComponentEdges =
        "A connected component is composed of a number of edges.\r\n\r\n" +
        "This is the count of total edges in the largest connected component.";
    ///
    public const String MaximumGeodesicDistance =
        "A geodesic is a chain or path composed of edges that link two vertices, " +
        "potentially through intermediate vertices.\r\n\r\n" +
        "A \"shortest path\" is the minimum number of connections needed to link two vertices.\r\n\r\n" +
        "The \"longest\" \"shortest path\" is the \"maximum geodesic distance\".";
    ///
    public const String AverageGeodesicDistance =
        "A geodesic is a chain or path composed of edges that link two vertices, " +
        "potentially through intermediate vertices.\r\n\r\n" +
        "The average length of these paths is the \"average geodesic distance\".";
    ///
    public const String ReciprocatedVertexPairRatio =
        "When two vertices both link to each other their connection is \"reciprocated\".\r\n\r\n" +
        "This is the percentage of vertices that have a reciprocal relationship.";
    ///
    public const String ReciprocatedEdgeRatio =
        "When an edge from A to B is joined by another edge from B to A " +
        "then their connection is \"reciprocated\".\r\n\r\n" +
        "This is the percentage of edges that have a reciprocal relationship.";
    ///
    public const String NodeXLVersion =
        "NodeXL updates frequently and the current version number is provided " +
        "to enable troubleshooting and to ensure older datasets are properly understood.";
}
}
