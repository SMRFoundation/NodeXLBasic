
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using System.Collections.Generic;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: DParallelMotifTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="DParallelMotif" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DParallelMotifTest : Object
{
    //*************************************************************************
    //  Constructor: DParallelMotifTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DParallelMotifTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public DParallelMotifTest()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        IVertex oAnchorVertex1 = new Vertex();
        IVertex oAnchorVertex2 = new Vertex();
        IVertex oAnchorVertex3 = new Vertex();

        DParallelMotif o2ParallelMotif = new DParallelMotif(
            new List<IVertex>(){oAnchorVertex1, oAnchorVertex2});

        Assert.AreEqual(oAnchorVertex1, o2ParallelMotif.AnchorVertices[0]);
        Assert.AreEqual(oAnchorVertex2, o2ParallelMotif.AnchorVertices[1]);

        DParallelMotif o3ParallelMotif = new DParallelMotif(
            new List<IVertex>(){oAnchorVertex1, oAnchorVertex2, oAnchorVertex3});

        Assert.AreEqual(oAnchorVertex1, o3ParallelMotif.AnchorVertices[0]);
        Assert.AreEqual(oAnchorVertex2, o3ParallelMotif.AnchorVertices[1]);
        Assert.AreEqual(oAnchorVertex3, o3ParallelMotif.AnchorVertices[2]);

        Assert.AreEqual(0, o2ParallelMotif.SpanVertices.Count);
        Assert.AreEqual(0, o3ParallelMotif.SpanVertices.Count);
        Assert.AreEqual(1.0, o3ParallelMotif.SpanScale);
    }

    //*************************************************************************
    //  Method: TestSpanVertices()
    //
    /// <summary>
    /// Tests the SpanVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSpanVertices()
    {
        IVertex oAnchorVertex1 = new Vertex();
        IVertex oAnchorVertex2 = new Vertex();

        DParallelMotif oDParallelMotif = new DParallelMotif(
            new List<IVertex>() { oAnchorVertex1, oAnchorVertex2 });

        IVertex oSpanVertex1 = new Vertex();
        IVertex oSpanVertex2 = new Vertex();

        oDParallelMotif.SpanVertices.Add(oSpanVertex1);
        oDParallelMotif.SpanVertices.Add(oSpanVertex2);

        Assert.AreEqual(2, oDParallelMotif.SpanVertices.Count);
        Assert.AreEqual( oSpanVertex1, oDParallelMotif.SpanVertices[0] );
        Assert.AreEqual( oSpanVertex2, oDParallelMotif.SpanVertices[1] );
    }

    //*************************************************************************
    //  Method: TestAllVertices()
    //
    /// <summary>
    /// Tests the AllVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAllVertices()
    {
        IVertex oAnchorVertex1 = new Vertex();
        IVertex oAnchorVertex2 = new Vertex();

        DParallelMotif oDParallelMotif = new DParallelMotif(
            new List<IVertex>() {oAnchorVertex1, oAnchorVertex2} );

        IVertex oSpanVertex1 = new Vertex();
        IVertex oSpanVertex2 = new Vertex();

        oDParallelMotif.SpanVertices.Add(oSpanVertex1);
        oDParallelMotif.SpanVertices.Add(oSpanVertex2);

        IVertex[] aoVerticesInMotif = oDParallelMotif.VerticesInMotif;

        // Note that the anchor vertices aren't actually a part of the motif.

        Assert.AreEqual(2, aoVerticesInMotif.Length);

        Assert.AreEqual( oSpanVertex1, aoVerticesInMotif.Single(
            oVertex => oVertex == oSpanVertex1) );

        Assert.AreEqual( oSpanVertex2, aoVerticesInMotif.Single(
            oVertex => oVertex == oSpanVertex2) );
    }

    //*************************************************************************
    //  Method: TestCollapsedAttributes()
    //
    /// <summary>
    /// Tests the CollapsedAttributes property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCollapsedAttributes()
    {
        // With vertex names.

        IVertex oAnchorVertex1 = new Vertex();
        IVertex oAnchorVertex2 = new Vertex();

        oAnchorVertex1.Name = "Name1";
        oAnchorVertex2.Name = "Name2";

        DParallelMotif oDParallelMotif = new DParallelMotif(
            new List<IVertex>(){oAnchorVertex1, oAnchorVertex2} );

        IVertex oSpanVertex1 = new Vertex();
        IVertex oSpanVertex2 = new Vertex();

        oDParallelMotif.SpanVertices.Add(oSpanVertex1);
        oDParallelMotif.SpanVertices.Add(oSpanVertex2);

        String sCollapsedAttributes = oDParallelMotif.CollapsedAttributes;

        CollapsedGroupAttributes oCollapsedGroupAttributes =
            CollapsedGroupAttributes.FromString(sCollapsedAttributes);

        Assert.AreEqual( CollapsedGroupAttributeValues.DParallelMotifType,
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.Type] );

        Assert.AreEqual( "2", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.AnchorVertices] );

        Assert.IsTrue( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(0) ) );

        Assert.IsTrue( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(1)));

        Assert.AreEqual( "Name1", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(0)]);

        Assert.AreEqual( "Name2", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(1)]);

        Assert.AreEqual( "2", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.SpanVertices] );

        Assert.IsTrue( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.SpanScale) );

        Assert.AreEqual( "1",
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.SpanScale] );
    }

    //*************************************************************************
    //  Method: TestCollapsedAttributes2()
    //
    /// <summary>
    /// Tests the CollapsedAttributes property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCollapsedAttributes2()
    {
        // Without vertex names and with SpanScale set to a non-default value.

        IVertex oAnchorVertex1 = new Vertex();
        IVertex oAnchorVertex2 = new Vertex();

        DParallelMotif oDParallelMotif = new DParallelMotif(
            new List<IVertex>() {oAnchorVertex1, oAnchorVertex2} );

        IVertex oSpanVertex1 = new Vertex();
        IVertex oSpanVertex2 = new Vertex();

        oDParallelMotif.SpanVertices.Add(oSpanVertex1);
        oDParallelMotif.SpanVertices.Add(oSpanVertex2);

        oDParallelMotif.SpanScale = 0.5;

        String sCollapsedAttributes = oDParallelMotif.CollapsedAttributes;

        CollapsedGroupAttributes oCollapsedGroupAttributes =
            CollapsedGroupAttributes.FromString(sCollapsedAttributes);

        Assert.AreEqual( CollapsedGroupAttributeValues.DParallelMotifType,
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.Type] );

        Assert.AreEqual( "2", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.AnchorVertices] );

        Assert.IsFalse( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(0) ) );

        Assert.IsFalse( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(1) ) );

        Assert.AreEqual( "2", oCollapsedGroupAttributes[
            CollapsedGroupAttributeKeys.SpanVertices] );

        Assert.IsTrue( oCollapsedGroupAttributes.ContainsKey(
            CollapsedGroupAttributeKeys.SpanScale) );

        Assert.AreEqual( "0.5",
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.SpanScale] );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
