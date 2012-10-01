
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: MotifCalculator
//
/// <summary>
/// Calculates motifs for a specified graph.
/// </summary>
//*****************************************************************************

public class MotifCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    public override String
    GraphMetricDescription
    {
        get
        {
            AssertValid();

            return ("motifs");
        }
    }

    //*************************************************************************
    //  Method: TryCalculateMotifs()
    //
    /// <summary>
    /// Attempts to partition the graph into motifs while optionally running on
    /// a background thread.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="motifsToCalculate">
    /// An ORed combination of motifs to calculate.
    /// </param>
    ///
    /// <param name="dMinimum">
    /// The minimum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    /// 
    /// <param name="dMaximum">
    /// The maximum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    /// 
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="motifs">
    /// Where a collection of zero or more <see cref="Motif" /> objects gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryCalculateMotifs
    (
        IGraph graph,
        Motifs motifsToCalculate,
        Int32 dMinimum,
        Int32 dMaximum,
        BackgroundWorker backgroundWorker,
        out ICollection<Motif> motifs
    )
    {
        Debug.Assert(graph != null);

        List<Motif> oMotifs = new List<Motif>();
        motifs = oMotifs;

        if ((motifsToCalculate & Motifs.Fan) != 0)
        {
            ICollection<Motif> oFanMotifs;

            if (!TryCalculateFanMotifs(graph, backgroundWorker,
                out oFanMotifs))
            {
                return (false);
            }

            oMotifs.AddRange(oFanMotifs);
        }

        if ((motifsToCalculate & Motifs.DParallel) != 0)
        {
            ICollection<Motif> oDParallelMotifs;

            if (!TryCalculateDParallelMotifs(graph, dMinimum, dMaximum, backgroundWorker,
                out oDParallelMotifs))
            {
                return (false);
            }

            oMotifs.AddRange(oDParallelMotifs);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateFanMotifs()
    //
    /// <summary>
    /// Attempts to calculate a set of fan motifs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oMotifs">
    /// Where a collection of zero or more <see cref="FanMotif" /> objects gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateFanMotifs
    (
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        out ICollection<Motif> oMotifs
    )
    {
        Debug.Assert(oGraph != null);

        oMotifs = null;

        LinkedList<Motif> oFanMotifs = new LinkedList<Motif>();
        LinkedList<IVertex> oLeaves = new LinkedList<IVertex>();

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;
        Int32 iCalculationsSoFar = 0;

        foreach (IVertex oVertex in oVertices)
        {
            if ( !ReportProgressIfNecessary(iCalculationsSoFar, iVertices,
                oBackgroundWorker) )
            {
                return (false);
            }

            ICollection<IVertex> oAdjacentVertices = oVertex.AdjacentVertices;

            if (oAdjacentVertices.Count >= 2)
            {
                foreach (IVertex oAdjacentVertex in oAdjacentVertices)
                {
                    if (oAdjacentVertex.AdjacentVertices.Count == 1)
                    {
                        oLeaves.AddLast(oAdjacentVertex);
                    }
                }

                if (oLeaves.Count >= 2)
                {
                    oFanMotifs.AddLast(
                        new FanMotif( oVertex, oLeaves.ToArray() ) );
                }

                oLeaves.Clear();
            }

            iCalculationsSoFar++;
        }

        // Set the ArcScale property on each FanMotif object.

        SetFanMotifArcScale(oFanMotifs);

        oMotifs = oFanMotifs;

        return (true);
    }

    //*************************************************************************
    //  Method: SetFanMotifArcScale()
    //
    /// <summary>
    /// Sets the <see cref="FanMotif.ArcScale" /> property on each fan motif.
    /// </summary>
    ///
    /// <param name="oFanMotifs">
    /// A collection of zero or more <see cref="FanMotif" /> objects.
    /// </param>
    //*************************************************************************

    protected void
    SetFanMotifArcScale
    (
        ICollection<Motif> oFanMotifs
    )
    {
        Debug.Assert(oFanMotifs != null);

        // The ArcScale property is the FanMotif's leaf count scaled between 0
        // and 1.0, based on the minimum and maximum leaf counts among all
        // FanMotifs.

        Int32 iMinimumLeafCount = 0;
        Int32 iMaximumLeafCount = 0;

        if (oFanMotifs.Count > 0)
        {
            iMinimumLeafCount = oFanMotifs.Min(
                oMotif => ( (FanMotif)oMotif ).LeafVertices.Length);

            iMaximumLeafCount = oFanMotifs.Max(
                oMotif => ( (FanMotif)oMotif ).LeafVertices.Length);
        }

        foreach (FanMotif oFanMotif in oFanMotifs)
        {
            Single fArcScale;

            if (iMinimumLeafCount == iMaximumLeafCount)
            {
                // All the leaf counts are the same.  Arbitrarily set the
                // ArcScale property to the center of the range.

                fArcScale = 0.5F;
            }
            else
            {
                fArcScale = MathUtil.TransformValueToRange(
                    oFanMotif.LeafVertices.Length,
                    iMinimumLeafCount, iMaximumLeafCount,
                    0F, 1.0F
                    );
            }

            oFanMotif.ArcScale = fArcScale;
        }
    }

    //*************************************************************************
    //  Method: TryCalculateDParallelMotifs()
    //
    /// <summary>
    /// Attempts to calculate a set of D-parallel motifs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="iDMinimum">
    /// The minimum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    /// 
    /// <param name="iDMaximum">
    /// The maximum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    /// 
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oMotifs">
    /// Where a collection of zero or more <see cref="DParallelMotif" />
    /// objects gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateDParallelMotifs
    (
        IGraph oGraph,
        Int32 iDMinimum,
        Int32 iDMaximum,
        BackgroundWorker oBackgroundWorker,
        out ICollection<Motif> oMotifs
    )
    {
        Debug.Assert(oGraph != null);

        oMotifs = null;

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;
        Int32 iCalculationsSoFar = 0;

        // The key is an ordered combination of the vertex IDs of the potential
        // motif's D anchor vertices, and the value is the corresponding
        // potential DParallelMotif object.

        Dictionary<string, DParallelMotif> oPotentialDParallelMotifs =
            new Dictionary<string, DParallelMotif>();

        foreach (IVertex oPotentialSpanVertex in oVertices)
        {
            if ( !ReportProgressIfNecessary(iCalculationsSoFar, iVertices,
                oBackgroundWorker) )
            {
                return (false);
            }

            // Get only the non-self-loop adjacent vertices
            ICollection<IVertex> oPotentialAnchorVertices =
                oPotentialSpanVertex.AdjacentVertices
                .Where<IVertex>(adjVertex => adjVertex != oPotentialSpanVertex).ToList<IVertex>();

            if ( DVerticesMightBeAnchors(oPotentialAnchorVertices, iDMinimum, iDMaximum) )
            {
                AddSpanVertexToPotentialDParallelMotifs(
                    oPotentialSpanVertex, oPotentialAnchorVertices,
                    oPotentialDParallelMotifs);
            }

            iCalculationsSoFar++;
        }

        // Filter the potential D-parallel motifs and add the real ones to
        // the collection of motifs.

        oMotifs = FilterDParallelMotifs(oPotentialDParallelMotifs);

        // Set the SpanScale property on each DParallelMotif object.

        SetDParallelMotifSpanScale(oMotifs);

        return (true);
    }

    //*************************************************************************
    //  Method: DVerticesMightBeAnchors()
    //
    /// <summary>
    /// Determines whether a collection of vertices might be anchor vertices in
    /// a D-parallel motif.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The collection to test.
    /// </param>
    ///
    /// <param name="iDMinimum">
    /// The minimum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    /// 
    /// <param name="iDMaximum">
    /// The maximum number of anchor vertices (dimension) of D-parallel motif to find.
    /// </param>
    ///
    /// <returns>
    /// true if the vertices might be anchor vertices, false if they cannot be.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DVerticesMightBeAnchors
    (
        ICollection<IVertex> oVertices,
        Int32 iDMinimum,
        Int32 iDMaximum
    )
    {
        Debug.Assert(oVertices != null);

        return (

            // There have to be between [iDMinimum, iDMaximum] anchor vertices.

            oVertices.Count >= iDMinimum && oVertices.Count <= iDMaximum
            &&

            // Anchor vertices can't have just one adjacent vertex.

            oVertices.All(oVertex => oVertex.AdjacentVertices.Count != 1)
            );
    }

    //*************************************************************************
    //  Method: AddSpanVertexToPotentialDParallelMotifs()
    //
    /// <summary>
    /// Adds a potential span vertex to a dictionary of potential D-parallel
    /// motifs.
    /// </summary>
    ///
    /// <param name="oPotentialSpanVertex">
    /// Vertex that might be a span vertex.
    /// </param>
    ///
    /// <param name="oDPotentialAnchorVertices">
    /// Collection of D vertices that might be anchor vertices for <paramref
    /// name="oPotentialSpanVertex" />.
    /// </param>
    ///
    /// <param name="oPotentialDParallelMotifs">
    /// The key is an ordered combination of the vertex IDs of the potential
    /// motif's D anchor vertices, and the value is the corresponding
    /// potential DParallelMotif object.
    /// </param>
    //*************************************************************************

    protected void
    AddSpanVertexToPotentialDParallelMotifs
    (
        IVertex oPotentialSpanVertex,
        ICollection<IVertex> oDPotentialAnchorVertices,
        Dictionary<string, DParallelMotif> oPotentialDParallelMotifs
    )
    {
        Debug.Assert(oPotentialSpanVertex != null);
        Debug.Assert(oDPotentialAnchorVertices != null);
        Debug.Assert(oDPotentialAnchorVertices.Count >= 2);
        Debug.Assert(oPotentialDParallelMotifs != null);

        // Is there already a DParallelMotif object for this set of
        // potential anchor vertices?
        
        IOrderedEnumerable<IVertex> oOrderedDPotentialAnchorVertices = oDPotentialAnchorVertices.OrderBy(v => v.ID);           
        string stringKey = string.Join(",", oOrderedDPotentialAnchorVertices.Select(v => v.ID.ToString()).ToArray());

        DParallelMotif oPotentialDParallelMotif;

        if (!oPotentialDParallelMotifs.TryGetValue(
            stringKey, out oPotentialDParallelMotif))
        {
            // No.  Create one.

            oPotentialDParallelMotif = new DParallelMotif(new List<IVertex>(oDPotentialAnchorVertices));

            oPotentialDParallelMotifs.Add(stringKey,
                oPotentialDParallelMotif);
        }

        oPotentialDParallelMotif.SpanVertices.Add(oPotentialSpanVertex);
    }


    //*************************************************************************
    //  Method: FilterDParallelMotifs()
    //
    /// <summary>
    /// Filters a collection of potential D-parallel motifs and adds the real
    /// or more favorable overlapping ones to a new collection.
    /// </summary>
    ///
    /// <param name="oPotentialDParallelMotifs">
    /// The key is an ordered combination of the vertex IDs of the potential
    /// motif's D anchor vertices, and the value is the corresponding
    /// potential DParallelMotif object.
    /// </param>
    ///
    /// <returns>
    /// A new collection of zero or more <see cref="DParallelMotif" />
    /// objects.
    /// </returns>
    //*************************************************************************

    protected ICollection<Motif>
    FilterDParallelMotifs
    (
        Dictionary<string, DParallelMotif> oPotentialDParallelMotifs
    )
    {
        Debug.Assert(oPotentialDParallelMotifs != null);

        HashSet<Motif> currentDParallelMotifs = new HashSet<Motif>();

        Dictionary<IVertex, DParallelMotif> verticesAlreadyInDParallelMotifs =
            new Dictionary<IVertex, DParallelMotif>();

        // Select only those potential D-parallel motifs that have at least
        // two span vertices.

        foreach (DParallelMotif potentialMotif in
            oPotentialDParallelMotifs.Values.Where(
                oPotentialDParallelMotif =>
                oPotentialDParallelMotif.SpanVertices.Count >= 2)
            )
        {
            // If any of the motif's span vertices are included in another
            // D-parallel motif we need to pick the motif to keep
            //
            // If this weren't done, for example, the following ring of vertices 
            // would result in two redundant two-parallel motifs:
            //
            // A-B-C-D-A

            List<DParallelMotif> overlappingMotifs = 
                (from spanVertex in potentialMotif.SpanVertices
                 where verticesAlreadyInDParallelMotifs.ContainsKey(spanVertex)
                 select verticesAlreadyInDParallelMotifs[spanVertex])
                 .Distinct<DParallelMotif>().ToList<DParallelMotif>();

            if (overlappingMotifs.Count > 0) 
            {
                // Our bookkeeping should prevent more than one overlap
                Debug.Assert(overlappingMotifs.Count == 1);

                DParallelMotif existingMotif = overlappingMotifs[0];
                
                int potAnchors = potentialMotif.AnchorVertices.Count, 
                    potSpanners = potentialMotif.SpanVertices.Count,
                    potTotal = potAnchors + potSpanners;

                int existAnchors = existingMotif.AnchorVertices.Count, 
                    existSpanners = existingMotif.SpanVertices.Count,
                    existTotal = existAnchors + existSpanners;

                // Potential motif is larger in total size, so we favor it
                // -- OR --
                // Potential motif is equal in total size and has more spanners, which we favor over more anchors
                if (potTotal > existTotal || 
                    (potTotal == existTotal && potSpanners > existSpanners))
                {
                    
                    // Remove the existing motif from the list of motifs and the dictionary entries for its vertices
                    currentDParallelMotifs.Remove(existingMotif);

                    foreach (IVertex existingSpanVertex in existingMotif.SpanVertices)
                    {
                        verticesAlreadyInDParallelMotifs.Remove(existingSpanVertex);
                    }

                    foreach (IVertex existingAnchorVertex in existingMotif.AnchorVertices)
                    {
                        verticesAlreadyInDParallelMotifs.Remove(existingAnchorVertex);
                    }

                    // Add the potential DParallelMotif and record its vertices
                    AddDParallelMotif(currentDParallelMotifs, verticesAlreadyInDParallelMotifs, potentialMotif);
                }
                else
                {
                    // Potential motif is smaller than the existing one or is the same size with fewer spanners -- do nothing
                }
                
            }
            // If all of the motifs span vertices are not included in others, add the DParallelMotif and record its vertices
            else 
            {
                AddDParallelMotif(currentDParallelMotifs, verticesAlreadyInDParallelMotifs, potentialMotif);
            }
        }

        return currentDParallelMotifs;
    }

    //*************************************************************************
    //  Method: AddDParallelMotif()
    //
    /// <summary>
    /// Adds a new DParallelMotif to the collections and creates mappings from
    /// its vertices to it
    /// </summary>
    /// 
    /// <param name="currentDParallelMotifs">
    /// The current DParallelMotif collection to add to
    /// </param>
    /// 
    /// <param name="verticesAlreadyInDParallelMotifs">
    /// The mapping between seen vertices and their associated DParallelMotifs
    /// </param>
    /// 
    /// <param name="parallelMotifToAdd">
    /// The DParallelMotif to add to the collections
    /// </param>
    //*************************************************************************

    private static void
    AddDParallelMotif
    (
        HashSet<Motif> currentDParallelMotifs,
        Dictionary<IVertex, DParallelMotif> verticesAlreadyInDParallelMotifs,
        DParallelMotif parallelMotifToAdd)
    {
        // Assert that there are no shared anchor and span vertices
        Debug.Assert(parallelMotifToAdd.SpanVertices.Intersect<IVertex>(parallelMotifToAdd.AnchorVertices).Count<IVertex>() == 0);

        currentDParallelMotifs.Add(parallelMotifToAdd);

        foreach (IVertex oVertex in parallelMotifToAdd.SpanVertices)
        {
            // We do not allow overlapping span vertices so we use .Add
            verticesAlreadyInDParallelMotifs.Add(oVertex, parallelMotifToAdd);
        }

        foreach (IVertex oVertex in parallelMotifToAdd.AnchorVertices)
        {
            // We allow overlapping anchor vertices so we use =
            verticesAlreadyInDParallelMotifs[oVertex] = parallelMotifToAdd;
        }
    }

    //*************************************************************************
    //  Method: SetDParallelMotifSpanScale()
    //
    /// <summary>
    /// Sets the <see cref="DParallelMotif.SpanScale" /> property on each
    /// D-parallel motif.
    /// </summary>
    ///
    /// <param name="oDParallelMotifs">
    /// A collection of zero or more <see cref="DParallelMotif" /> objects.
    /// </param>
    //*************************************************************************

    protected void
    SetDParallelMotifSpanScale
    (
        ICollection<Motif> oDParallelMotifs
    )
    {
        Debug.Assert(oDParallelMotifs != null);

        // The SpanScale property is the DParallelMotif's span count scaled
        // between 0 and 1.0, based on the minimum and maximum span counts
        // among all DParallelMotifs.

        Int32 iMinimumSpanCount = 0;
        Int32 iMaximumSpanCount = 0;

        if (oDParallelMotifs.Count > 0)
        {
            iMinimumSpanCount = oDParallelMotifs.Min(
                oMotif => ((DParallelMotif)oMotif).SpanVertices.Count);

            iMaximumSpanCount = oDParallelMotifs.Max(
                oMotif => ((DParallelMotif)oMotif).SpanVertices.Count);
        }

        foreach (DParallelMotif oDParallelMotif in oDParallelMotifs)
        {
            Single fSpanScale;

            if (iMinimumSpanCount == iMaximumSpanCount)
            {
                // All the span counts are the same.  Arbitrarily set the
                // SpanScale property to the center of the range.

                fSpanScale = 0.5F;
            }
            else
            {
                fSpanScale = MathUtil.TransformValueToRange(
                    oDParallelMotif.SpanVertices.Count,
                    iMinimumSpanCount, iMaximumSpanCount,
                    0F, 1.0F
                    );
            }

            oDParallelMotif.SpanScale = fSpanScale;
        }
    }

    //*************************************************************************
    //  Method: ReportProgressIfNecessary()
    //
    /// <summary>
    /// Periodically checks for cancellation and reports progress.
    /// </summary>
    ///
    /// <param name="iCalculationsSoFar">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
    /// metric calculations.
    /// </param>
    ///
    /// <returns>
    /// false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReportProgressIfNecessary
    (
        Int32 iCalculationsSoFar,
        Int32 iTotalCalculations,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(iCalculationsSoFar >= 0);
        Debug.Assert(iTotalCalculations >= 0);

        return (
            (iCalculationsSoFar % VerticesPerProgressReport != 0)
            ||
            ReportProgressAndCheckCancellationPending(
                iCalculationsSoFar, iTotalCalculations, oBackgroundWorker)
            );
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number of vertices that are processed before progress is reported and
    /// the cancellation flag is checked.

    protected const Int32 VerticesPerProgressReport = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
