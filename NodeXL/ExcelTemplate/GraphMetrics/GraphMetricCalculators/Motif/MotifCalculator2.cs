
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MotifCalculator2
//
/// <summary>
/// Partitions a graph into motifs.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.MotifCalculator" /> for details on how the graph
/// is partitioned into motifs.
/// </remarks>
//*****************************************************************************

public class MotifCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: MotifCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MotifCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public MotifCalculator2()
    {
        m_eMotifsToCalculate = Motifs.None;
        m_iDParallelMinimumAnchorVertices = 2;
        m_iDParallelMaximumAnchorVertices = 2;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MotifsToCalculate
    //
    /// <summary>
    /// Gets or sets the motifs to calculate.
    /// </summary>
    ///
    /// <value>
    /// An ORed combination of the <see cref="Motifs" /> to calculate.  The
    /// default is <see cref="Motifs.None" />.
    /// </value>
    //*************************************************************************

    public Motifs
    MotifsToCalculate
    {
        get
        {
            AssertValid();

            return (m_eMotifsToCalculate);
        }

        set
        {
            m_eMotifsToCalculate = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DParallelMinimumAnchorVertices
    //
    /// <summary>
    /// Gets or sets the minimum number of anchor vertices when grouping the
    /// graph's vertices by D-parallel motifs.
    /// </summary>
    ///
    /// <value>
    /// The minimum number of anchor vertices.  Must be at least 2.  The
    /// default value is 2.
    /// </value>
    //*************************************************************************

    public Int32
    DParallelMinimumAnchorVertices
    {
        get
        {
            AssertValid();

            return (m_iDParallelMinimumAnchorVertices);
        }

        set
        {
            m_iDParallelMinimumAnchorVertices = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DParallelMaximumAnchorVertices
    //
    /// <summary>
    /// Gets or sets the maximum number of anchor vertices when grouping the
    /// graph's vertices by D-parallel motifs.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of anchor vertices.  Must be at least 2.  The
    /// default value is 2.
    /// </value>
    //*************************************************************************

    public Int32
    DParallelMaximumAnchorVertices
    {
        get
        {
            AssertValid();

            return (m_iDParallelMaximumAnchorVertices);
        }

        set
        {
            m_iDParallelMaximumAnchorVertices = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        graphMetricColumns = null;

        // Partition the graph into motifs using the MotifCalculator class in
        // the Algorithms namespace, which knows nothing about Excel.

        Algorithms.MotifCalculator oMotifCalculator =
            new Algorithms.MotifCalculator();

        ICollection<Motif> oMotifs;

        if ( !oMotifCalculator.TryCalculateMotifs(graph,
            m_eMotifsToCalculate, m_iDParallelMinimumAnchorVertices,
            m_iDParallelMaximumAnchorVertices,
            calculateGraphMetricsContext.BackgroundWorker, out oMotifs) )
        {
            // The user cancelled.

            return (false);
        }

        // Convert the collection of motifs to an array of GraphMetricColumn
        // objects.

        graphMetricColumns =
            GroupsToGraphMetricColumnsConverter.Convert<Motif>(
                oMotifs,
                (oMotif) => oMotif.VerticesInMotif,
                (oMotif) => MotifToGroupName(oMotif),
                true,
                (oMotif) => oMotif.CollapsedAttributes
                );

        return (true);
    }

    //*************************************************************************
    //  Method: MotifToGroupName()
    //
    /// <summary>
    /// Gets a group name to use for a motif.
    /// </summary>
    ///
    /// <param name="oMotif">
    /// The motif to get a group name for.
    /// </param>
    ///
    /// <returns>
    /// A group name to use for <paramref name="oMotif" />.
    /// </returns>
    //*************************************************************************

    protected String
    MotifToGroupName
    (
        Motif oMotif
    )
    {
        AssertValid();

        if (oMotif is FanMotif)
        {
            // Sample:
            //
            // Fan motif: "HeadVertexName"

            return ( String.Format(
                "Fan motif: \"{0}\""
                ,
                ( (FanMotif)oMotif ).HeadVertex.Name
                ) );
        }

        if (oMotif is DParallelMotif)
        {
            // Sample:
            //
            // D-parallel motif: "AnchorVertex1Name", "AnchorVertex2Name",
            // "AnchorVertex3Name"

            StringBuilder oGroupName = new StringBuilder();

            oGroupName.Append("D-parallel motif: ");
            Boolean bAppendComma = false;

            foreach (IVertex oAnchorVertex in
                ( (DParallelMotif)oMotif ).AnchorVertices)
            {
                oGroupName.AppendFormat(
                    "{0}\"{1}\""
                    ,
                    bAppendComma ? ", " : String.Empty,
                    oAnchorVertex.Name
                    );

                bAppendComma = true;
            }

            return ( oGroupName.ToString() );
        }

        return ("Unknown motif");
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

        // m_eMotifsToCalculate
        Debug.Assert(m_iDParallelMinimumAnchorVertices >= 2);
        Debug.Assert(m_iDParallelMaximumAnchorVertices >= 2);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The motifs to calculate.

    protected Motifs m_eMotifsToCalculate;

    /// The minimum number of anchor vertices.

    protected Int32 m_iDParallelMinimumAnchorVertices;

    /// The maximum number of anchor vertices.

    protected Int32 m_iDParallelMaximumAnchorVertices;
}

}
