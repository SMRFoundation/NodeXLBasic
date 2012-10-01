﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: MotifCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="MotifCalculator" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class MotifCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: MotifCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MotifCalculatorTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public MotifCalculatorTest()
    {
        m_oMotifCalculator = null;
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
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
        m_oMotifCalculator = new MotifCalculator();
        m_oGraph = new Graph();
        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
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
        m_oMotifCalculator = null;
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs()
    {
        // Empty graph.

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan | Motifs.DParallel, 2, 2, null,
            out oMotifs) );

        Assert.AreEqual(0, oMotifs.Count);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs2()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs2()
    {
        // Simple fan motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();

        // Not a fan.

        m_oEdges.Add(oVertexA, oVertexB);

        // Fan.

        m_oEdges.Add(oVertexC, oVertexD);
        m_oEdges.Add(oVertexC, oVertexE);
        m_oEdges.Add(oVertexC, oVertexF);
        m_oEdges.Add(oVertexF, oVertexG);

        // Fan.  Note the parallel edges in this one.

        m_oEdges.Add(oVertexH, oVertexI);
        m_oEdges.Add(oVertexH, oVertexI);
        m_oEdges.Add(oVertexI, oVertexH);
        m_oEdges.Add(oVertexH, oVertexJ);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan, 2, 2, null, out oMotifs) );

        Assert.AreEqual(2, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is FanMotif);
        }

        VerifyFanMotif(oMotifs, oVertexC, 0.5, oVertexD, oVertexE);
        VerifyFanMotif(oMotifs, oVertexH, 0.5, oVertexI, oVertexJ);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs3()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs3()
    {
        // Fan motifs with self-loop.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);

        // Add a self-loop on oVertexD, which prevents it from being a leaf
        // vertex.

        m_oEdges.Add(oVertexD, oVertexD);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is FanMotif);
        }

        VerifyFanMotif(oMotifs, oVertexA, 0.5, oVertexB, oVertexC);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs4()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs4()
    {
        // Simple two-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();
        IVertex oVertexL = m_oVertices.Add();

        // Not a two-parallel.

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);

        // Two-parallel.

        m_oEdges.Add(oVertexD, oVertexF);
        m_oEdges.Add(oVertexD, oVertexG);
        m_oEdges.Add(oVertexE, oVertexF);
        m_oEdges.Add(oVertexE, oVertexG);

        // Two-parallel.

        m_oEdges.Add(oVertexH, oVertexK);
        m_oEdges.Add(oVertexH, oVertexL);
        m_oEdges.Add(oVertexI, oVertexK);
        m_oEdges.Add(oVertexI, oVertexL);
        m_oEdges.Add(oVertexJ, oVertexK);
        m_oEdges.Add(oVertexJ, oVertexL);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(2, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexF, oVertexG},
            0.0, oVertexD, oVertexE);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexK, oVertexL},
            1.0, oVertexH, oVertexI, oVertexJ);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs5()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs5()
    {
        // Simple two- and three-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";
        IVertex oVertexC = m_oVertices.Add();
        oVertexC.Name = "C";

        IVertex oVertexD = m_oVertices.Add();
        oVertexD.Name = "D";
        IVertex oVertexE = m_oVertices.Add();
        oVertexE.Name = "E";
        IVertex oVertexF = m_oVertices.Add();
        oVertexF.Name = "F";
        IVertex oVertexG = m_oVertices.Add();
        oVertexG.Name = "G";
        
        IVertex oVertexH = m_oVertices.Add();
        oVertexH.Name = "H";
        IVertex oVertexI = m_oVertices.Add();
        oVertexI.Name = "I";
        IVertex oVertexJ = m_oVertices.Add();
        oVertexJ.Name = "J";
        IVertex oVertexK = m_oVertices.Add();
        oVertexK.Name = "K";
        IVertex oVertexL = m_oVertices.Add();
        oVertexL.Name = "L";

        IVertex oVertexM = m_oVertices.Add();
        oVertexM.Name = "M";
        IVertex oVertexN = m_oVertices.Add();
        oVertexN.Name = "N";
        IVertex oVertexO = m_oVertices.Add();
        oVertexO.Name = "O";
        IVertex oVertexP = m_oVertices.Add();
        oVertexP.Name = "P";
        IVertex oVertexQ = m_oVertices.Add();
        oVertexQ.Name = "Q";
        IVertex oVertexR = m_oVertices.Add();
        oVertexR.Name = "R";
        IVertex oVertexS = m_oVertices.Add();
        oVertexS.Name = "S";

        // Not a 2-parallel B-A-C

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);

        // 2-parallel (F,G)<D,E> or (D,E)<F,G> depending on order of processing

        m_oEdges.Add(oVertexD, oVertexF);
        m_oEdges.Add(oVertexD, oVertexG);
        m_oEdges.Add(oVertexE, oVertexF);
        m_oEdges.Add(oVertexE, oVertexG);

        // 2-parallel (K,L)<H,I,J>

        m_oEdges.Add(oVertexH, oVertexK);
        m_oEdges.Add(oVertexH, oVertexL);
        m_oEdges.Add(oVertexI, oVertexK);
        m_oEdges.Add(oVertexI, oVertexL);
        m_oEdges.Add(oVertexJ, oVertexK);
        m_oEdges.Add(oVertexJ, oVertexL);

        // 3-Parallel (O,M,N)<P,Q,R,S>

        m_oEdges.Add(oVertexP, oVertexM);
        m_oEdges.Add(oVertexP, oVertexN);
        m_oEdges.Add(oVertexP, oVertexO);
        m_oEdges.Add(oVertexQ, oVertexM);
        m_oEdges.Add(oVertexQ, oVertexN);
        m_oEdges.Add(oVertexQ, oVertexO);
        m_oEdges.Add(oVertexR, oVertexM);
        m_oEdges.Add(oVertexR, oVertexN);
        m_oEdges.Add(oVertexR, oVertexO);
        m_oEdges.Add(oVertexS, oVertexM);
        m_oEdges.Add(oVertexS, oVertexN);
        m_oEdges.Add(oVertexS, oVertexO);
        
        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(3, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexF, oVertexG}, 0.0,
            oVertexD, oVertexE);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexK, oVertexL}, 0.5,
            oVertexH, oVertexI, oVertexJ);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexM, oVertexN, oVertexO}, 1.0,
            oVertexP, oVertexQ, oVertexR, oVertexS);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs6()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs6()
    {
        // Two-parallel motifs with another vertex off of an anchor vertex.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexX = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        m_oEdges.Add(oVertexA, oVertexX);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexA, oVertexB},
            0.5, oVertexC, oVertexD);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs7()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs7()
    {
        // 2-parallel motif with another vertex off of an anchor vertex (A,B)<C,D> + A-X

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";
        IVertex oVertexC = m_oVertices.Add();
        oVertexC.Name = "C";
        IVertex oVertexD = m_oVertices.Add();
        oVertexD.Name = "D";

        IVertex oVertexX = m_oVertices.Add();
        oVertexX.Name = "X";

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        m_oEdges.Add(oVertexA, oVertexX);

        // 3-Parallel motif with another vertex off of an anchor vertex (O,M,N)<P,Q,R,S> + O-Y

        IVertex oVertexM = m_oVertices.Add();
        oVertexM.Name = "M";
        IVertex oVertexN = m_oVertices.Add();
        oVertexN.Name = "N";
        IVertex oVertexO = m_oVertices.Add();
        oVertexO.Name = "O";
        IVertex oVertexP = m_oVertices.Add();
        oVertexP.Name = "P";
        IVertex oVertexQ = m_oVertices.Add();
        oVertexQ.Name = "Q";
        IVertex oVertexR = m_oVertices.Add();
        oVertexR.Name = "R";
        IVertex oVertexS = m_oVertices.Add();
        oVertexS.Name = "S";

        IVertex oVertexY = m_oVertices.Add();
        oVertexY.Name = "Y";

        m_oEdges.Add(oVertexP, oVertexM);
        m_oEdges.Add(oVertexP, oVertexN);
        m_oEdges.Add(oVertexP, oVertexO);
        m_oEdges.Add(oVertexQ, oVertexM);
        m_oEdges.Add(oVertexQ, oVertexN);
        m_oEdges.Add(oVertexQ, oVertexO);
        m_oEdges.Add(oVertexR, oVertexM);
        m_oEdges.Add(oVertexR, oVertexN);
        m_oEdges.Add(oVertexR, oVertexO);
        m_oEdges.Add(oVertexS, oVertexM);
        m_oEdges.Add(oVertexS, oVertexN);
        m_oEdges.Add(oVertexS, oVertexO);

        m_oEdges.Add(oVertexO, oVertexY);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexA, oVertexB}, 0.0,
            oVertexC, oVertexD);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexM, oVertexN, oVertexO }, 1.0,
            oVertexP, oVertexQ, oVertexR, oVertexS);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs8()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs8()
    {
        // Two-parallel motifs with another vertex off of a potential span
        // vertex, which prevents it from being part of the motif.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexX = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexE);

        m_oEdges.Add(oVertexE, oVertexX);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexA, oVertexB},
            0.5, oVertexC, oVertexD);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs9()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs9()
    {
        // 3-Parallel motif with another vertex off of an anchor vertex (C,D,E)<A,B> + E-X
        // This could be a 2-parallel motif ((A,B)<C,D> + (A,B)<E>, E-X), but this would be 
        // a smaller motif

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";
        IVertex oVertexC = m_oVertices.Add();
        oVertexC.Name = "C";
        IVertex oVertexD = m_oVertices.Add();
        oVertexD.Name = "D";
        IVertex oVertexE = m_oVertices.Add();
        oVertexE.Name = "E";

        IVertex oVertexX = m_oVertices.Add();
        oVertexX.Name = "X";

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexE);

        m_oEdges.Add(oVertexE, oVertexX);

        // 3-Parallel motif with another vertex off of an anchor vertex (P,Q,R,S)<M,N,O> + S-Y
        // This could be a 3-parallel motif (M,N,O)<P,Q,R> + (M,N,O)<S>, S-Y), but this would be 
        // a smaller motif

        IVertex oVertexM = m_oVertices.Add();
        oVertexM.Name = "M";
        IVertex oVertexN = m_oVertices.Add();
        oVertexN.Name = "N";
        IVertex oVertexO = m_oVertices.Add();
        oVertexO.Name = "O";
        IVertex oVertexP = m_oVertices.Add();
        oVertexP.Name = "P";
        IVertex oVertexQ = m_oVertices.Add();
        oVertexQ.Name = "Q";
        IVertex oVertexR = m_oVertices.Add();
        oVertexR.Name = "R";
        IVertex oVertexS = m_oVertices.Add();
        oVertexS.Name = "S";

        IVertex oVertexY = m_oVertices.Add();
        oVertexY.Name = "Y";

        m_oEdges.Add(oVertexP, oVertexM);
        m_oEdges.Add(oVertexP, oVertexN);
        m_oEdges.Add(oVertexP, oVertexO);
        m_oEdges.Add(oVertexQ, oVertexM);
        m_oEdges.Add(oVertexQ, oVertexN);
        m_oEdges.Add(oVertexQ, oVertexO);
        m_oEdges.Add(oVertexR, oVertexM);
        m_oEdges.Add(oVertexR, oVertexN);
        m_oEdges.Add(oVertexR, oVertexO);
        m_oEdges.Add(oVertexS, oVertexM);
        m_oEdges.Add(oVertexS, oVertexN);
        m_oEdges.Add(oVertexS, oVertexO);

        m_oEdges.Add(oVertexS, oVertexY);


        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, int.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexC, oVertexD, oVertexE }, 0.0,
            oVertexA, oVertexB);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexP, oVertexQ, oVertexR, oVertexS }, 1.0,
            oVertexM, oVertexN, oVertexO);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs10()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs10()
    {
        // 2-parallel motif motif with another vertex off of a potential span
        // vertex, which prevents it from being part of the motif. (A,B)<C,D> + (A,B)<E>, E-X, A-AAlt, B-BAlt

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";
        IVertex oVertexC = m_oVertices.Add();
        oVertexC.Name = "C";
        IVertex oVertexD = m_oVertices.Add();
        oVertexD.Name = "D";
        IVertex oVertexE = m_oVertices.Add();
        oVertexE.Name = "E";

        IVertex oVertexX = m_oVertices.Add();
        oVertexX.Name = "X";

        IVertex oVertexAAlt = m_oVertices.Add();
        oVertexAAlt.Name = "AAlt";
        IVertex oVertexBAlt = m_oVertices.Add();
        oVertexBAlt.Name = "BAlt";

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexE);

        m_oEdges.Add(oVertexE, oVertexX);

        m_oEdges.Add(oVertexA, oVertexAAlt);
        m_oEdges.Add(oVertexB, oVertexBAlt);

        // 3-parallel motif motif with another vertex off of a potential span
        // vertex, which prevents it from being part of the motif. (M,N,O)<P,Q,R> + (M,N,O)<S>, S-Y, M-MAlt, N-NAlt, O-OAlt)

        IVertex oVertexM = m_oVertices.Add();
        oVertexM.Name = "M";
        IVertex oVertexN = m_oVertices.Add();
        oVertexN.Name = "N";
        IVertex oVertexO = m_oVertices.Add();
        oVertexO.Name = "O";
        IVertex oVertexP = m_oVertices.Add();
        oVertexP.Name = "P";
        IVertex oVertexQ = m_oVertices.Add();
        oVertexQ.Name = "Q";
        IVertex oVertexR = m_oVertices.Add();
        oVertexR.Name = "R";
        IVertex oVertexS = m_oVertices.Add();
        oVertexS.Name = "S";

        IVertex oVertexY = m_oVertices.Add();
        oVertexY.Name = "Y";

        IVertex oVertexMAlt = m_oVertices.Add();
        oVertexMAlt.Name = "MAlt";
        IVertex oVertexNAlt = m_oVertices.Add();
        oVertexNAlt.Name = "NAlt";
        IVertex oVertexOAlt = m_oVertices.Add();
        oVertexOAlt.Name = "OAlt";

        m_oEdges.Add(oVertexP, oVertexM);
        m_oEdges.Add(oVertexP, oVertexN);
        m_oEdges.Add(oVertexP, oVertexO);
        m_oEdges.Add(oVertexQ, oVertexM);
        m_oEdges.Add(oVertexQ, oVertexN);
        m_oEdges.Add(oVertexQ, oVertexO);
        m_oEdges.Add(oVertexR, oVertexM);
        m_oEdges.Add(oVertexR, oVertexN);
        m_oEdges.Add(oVertexR, oVertexO);
        m_oEdges.Add(oVertexS, oVertexM);
        m_oEdges.Add(oVertexS, oVertexN);
        m_oEdges.Add(oVertexS, oVertexO);

        m_oEdges.Add(oVertexS, oVertexY);

        m_oEdges.Add(oVertexM, oVertexMAlt);
        m_oEdges.Add(oVertexN, oVertexNAlt);
        m_oEdges.Add(oVertexO, oVertexOAlt);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, int.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB }, 0.0,
            oVertexC, oVertexD);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexM, oVertexN, oVertexO }, 1.0,
            oVertexP, oVertexQ, oVertexR);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs11()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs11()
    {
        // 2-parallel motifs with a self-loop on an anchor vertex (A,B)<C,D> + A-A, B-BAlt

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";
        IVertex oVertexC = m_oVertices.Add();
        oVertexC.Name = "C";
        IVertex oVertexD = m_oVertices.Add();
        oVertexD.Name = "D";

        IVertex oVertexBAlt = m_oVertices.Add();
        oVertexBAlt.Name = "BAlt";

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        m_oEdges.Add(oVertexA, oVertexA);
        
        m_oEdges.Add(oVertexB, oVertexBAlt);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB }, 0.5,
            oVertexC, oVertexD);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs12()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs12()
    {
        // 2-parallel motif with a self-loop off of a potential span vertex,
        // which IS ALLOWED to be part of the motif. (A,B)<C,D,E> + E-E

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexE);

        m_oEdges.Add(oVertexE, oVertexE);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB }, 0.5,
            oVertexC, oVertexD, oVertexE);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs13()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs13()
    {
        // Two-parallel motif with parallel edges.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexC, oVertexD},
            0.5, oVertexA, oVertexB);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs14()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs14()
    {
        // 2-parallel motif with parallel edges. (C,D)<A,B>

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexD);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(1, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is DParallelMotif);
        }

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexC, oVertexD }, 0.5,
            oVertexA, oVertexB);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs15()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs15()
    {
        // Fan motif and two-parallel motif.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();

        // Fan.

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);

        // Two-parallel.

        m_oEdges.Add(oVertexD, oVertexF);
        m_oEdges.Add(oVertexD, oVertexG);
        m_oEdges.Add(oVertexE, oVertexF);
        m_oEdges.Add(oVertexE, oVertexG);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan | Motifs.DParallel, 2, 2, null,
            out oMotifs) );

        Assert.AreEqual(2, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexA, 0.5, oVertexB, oVertexC);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexF, oVertexG},
            0.5, oVertexD, oVertexE);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs16()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs16()
    {
        // Fan motif and two-parallel motif.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();

        // Fan.

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);

        // Two-parallel.

        m_oEdges.Add(oVertexD, oVertexF);
        m_oEdges.Add(oVertexD, oVertexG);
        m_oEdges.Add(oVertexE, oVertexF);
        m_oEdges.Add(oVertexE, oVertexG);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan | Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexA, 0.5, oVertexB, oVertexC);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexF, oVertexG }, 0.5,
            oVertexD, oVertexE);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs17()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs17()
    {
        // Fan motif attached to anchor of two-parallel motif, calculate fan
        // motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();

        // Fan.

        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexB, oVertexG);

        // Two-parallel.

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexB, 0.5, oVertexF, oVertexG);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs18()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs18()
    {
        // Fan motif attached to anchor of 3-parallel motif, calculate fan
        // motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();

        // Fan. B<G,H

        m_oEdges.Add(oVertexB, oVertexG);
        m_oEdges.Add(oVertexB, oVertexH);

        // 3-parallel. (A,B,C)<D,E,F>

        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexA, oVertexF);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexC, oVertexD);
        m_oEdges.Add(oVertexC, oVertexE);
        m_oEdges.Add(oVertexC, oVertexF);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan, 2, 3, null, out oMotifs));

        Assert.AreEqual(1, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexB, 0.5, oVertexG, oVertexH);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs19()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs19()
    {
        // Fan motif attached to anchor of two-parallel motif, calculate
        // two-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();

        // Fan.

        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexB, oVertexG);

        // Two-parallel.

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(1, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexA, oVertexB},
            0.5, oVertexC, oVertexD, oVertexE);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs20()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs20()
    {
        // Fan motif attached to anchor of 3-parallel motif, calculate
        // D-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();

        // Fan. B<G,H

        m_oEdges.Add(oVertexB, oVertexG);
        m_oEdges.Add(oVertexB, oVertexH);

        // 3-parallel. (A,B,C)<D,E,F>

        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexA, oVertexF);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexC, oVertexD);
        m_oEdges.Add(oVertexC, oVertexE);
        m_oEdges.Add(oVertexC, oVertexF);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(1, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexA, oVertexB, oVertexC }, 0.5,
            oVertexD, oVertexE, oVertexF);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs21()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs21()
    {
        // Fan motif attached to anchor of two-parallel motif, calculate fan
        // and D-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();

        // Fan.

        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexB, oVertexG);

        // Two-parallel.

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan | Motifs.DParallel, 2, 2, null,
            out oMotifs) );

        Assert.AreEqual(2, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexB, 0.5, oVertexF, oVertexG);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexA, oVertexB},
            0.5, oVertexC, oVertexD, oVertexE);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs22()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs22()
    {
        // Fan motif attached to anchor of 3-parallel motif, calculate fan
        // and two-parallel motifs.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();

        // Fan. B<G,H

        m_oEdges.Add(oVertexB, oVertexG);
        m_oEdges.Add(oVertexB, oVertexH);

        // 3-parallel. (A,B,C)<D,E,F>

        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexA, oVertexF);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexB, oVertexF);
        m_oEdges.Add(oVertexC, oVertexD);
        m_oEdges.Add(oVertexC, oVertexE);
        m_oEdges.Add(oVertexC, oVertexF);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan | Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        VerifyFanMotif(oMotifs, oVertexB, 0.5, oVertexG, oVertexH);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB, oVertexC }, 0.5,
            oVertexD, oVertexE, oVertexF);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs23()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs23()
    {
        // Simple fan motifs with varying number of leaf vertices.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();
        IVertex oVertexL = m_oVertices.Add();
        IVertex oVertexM = m_oVertices.Add();
        IVertex oVertexN = m_oVertices.Add();
        IVertex oVertexO = m_oVertices.Add();
        IVertex oVertexP = m_oVertices.Add();
        IVertex oVertexQ = m_oVertices.Add();
        IVertex oVertexR = m_oVertices.Add();
        IVertex oVertexS = m_oVertices.Add();

        // Fan with 2 leaf vertices.

        m_oEdges.Add(oVertexA, oVertexB);
        m_oEdges.Add(oVertexA, oVertexC);

        // Fan with 3 leaf vertices.

        m_oEdges.Add(oVertexD, oVertexE);
        m_oEdges.Add(oVertexD, oVertexF);
        m_oEdges.Add(oVertexD, oVertexG);

        // Fan with 4 leaf vertices.

        m_oEdges.Add(oVertexH, oVertexI);
        m_oEdges.Add(oVertexH, oVertexJ);
        m_oEdges.Add(oVertexH, oVertexK);
        m_oEdges.Add(oVertexH, oVertexL);

        // Fan with 6 leaf vertices.

        m_oEdges.Add(oVertexM, oVertexN);
        m_oEdges.Add(oVertexM, oVertexO);
        m_oEdges.Add(oVertexM, oVertexP);
        m_oEdges.Add(oVertexM, oVertexQ);
        m_oEdges.Add(oVertexM, oVertexR);
        m_oEdges.Add(oVertexM, oVertexS);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.Fan, 2, 2, null, out oMotifs) );

        Assert.AreEqual(4, oMotifs.Count);

        foreach (Motif oMotif in oMotifs)
        {
            Assert.IsTrue(oMotif is FanMotif);
        }

        VerifyFanMotif(oMotifs, oVertexA, 0.0, oVertexB, oVertexC);

        VerifyFanMotif(oMotifs, oVertexD, 0.25, oVertexE, oVertexF, oVertexG);

        VerifyFanMotif(oMotifs, oVertexH, 0.5, oVertexI, oVertexJ, oVertexK,
            oVertexL);

        VerifyFanMotif(oMotifs, oVertexM, 1.0, oVertexN, oVertexO, oVertexP,
            oVertexQ, oVertexR, oVertexS);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs24()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs24()
    {
        // Simple two-parallel motifs with varying number of span vertices.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();
        IVertex oVertexL = m_oVertices.Add();
        IVertex oVertexM = m_oVertices.Add();
        IVertex oVertexN = m_oVertices.Add();
        IVertex oVertexO = m_oVertices.Add();
        IVertex oVertexP = m_oVertices.Add();
        IVertex oVertexQ = m_oVertices.Add();
        IVertex oVertexR = m_oVertices.Add();
        IVertex oVertexS = m_oVertices.Add();
        IVertex oVertexT = m_oVertices.Add();
        IVertex oVertexU = m_oVertices.Add();
        IVertex oVertexV = m_oVertices.Add();
        IVertex oVertexW = m_oVertices.Add();

        // Two-parallel with 2 span vertices.

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);

        // Two-parallel with 3 span vertices.

        m_oEdges.Add(oVertexE, oVertexG);
        m_oEdges.Add(oVertexE, oVertexH);
        m_oEdges.Add(oVertexE, oVertexI);
        m_oEdges.Add(oVertexF, oVertexG);
        m_oEdges.Add(oVertexF, oVertexH);
        m_oEdges.Add(oVertexF, oVertexI);

        // Two-parallel with 4 span vertices.

        m_oEdges.Add(oVertexJ, oVertexL);
        m_oEdges.Add(oVertexJ, oVertexM);
        m_oEdges.Add(oVertexJ, oVertexN);
        m_oEdges.Add(oVertexJ, oVertexO);
        m_oEdges.Add(oVertexK, oVertexL);
        m_oEdges.Add(oVertexK, oVertexM);
        m_oEdges.Add(oVertexK, oVertexN);
        m_oEdges.Add(oVertexK, oVertexO);

        // Two-parallel with 6 span vertices.

        m_oEdges.Add(oVertexP, oVertexR);
        m_oEdges.Add(oVertexP, oVertexS);
        m_oEdges.Add(oVertexP, oVertexT);
        m_oEdges.Add(oVertexP, oVertexU);
        m_oEdges.Add(oVertexP, oVertexV);
        m_oEdges.Add(oVertexP, oVertexW);
        m_oEdges.Add(oVertexQ, oVertexR);
        m_oEdges.Add(oVertexQ, oVertexS);
        m_oEdges.Add(oVertexQ, oVertexT);
        m_oEdges.Add(oVertexQ, oVertexU);
        m_oEdges.Add(oVertexQ, oVertexV);
        m_oEdges.Add(oVertexQ, oVertexW);

        ICollection<Motif> oMotifs;

        Assert.IsTrue( m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, 2, null, out oMotifs) );

        Assert.AreEqual(4, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexC, oVertexD},
            0.0, oVertexA, oVertexB);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexE, oVertexF},
            0.25, oVertexG, oVertexH, oVertexI);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexJ, oVertexK},
            0.5, oVertexL, oVertexM, oVertexN, oVertexO);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){oVertexP, oVertexQ},
            1.0, oVertexR, oVertexS, oVertexT, oVertexU, oVertexV, oVertexW);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs25()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs25()
    {
        // Simple 2-parallel motifs with varying number of span vertices.

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();
        IVertex oVertexL = m_oVertices.Add();
        IVertex oVertexM = m_oVertices.Add();
        IVertex oVertexN = m_oVertices.Add();
        IVertex oVertexO = m_oVertices.Add();
        IVertex oVertexP = m_oVertices.Add();
        IVertex oVertexQ = m_oVertices.Add();
        IVertex oVertexR = m_oVertices.Add();
        IVertex oVertexS = m_oVertices.Add();
        IVertex oVertexT = m_oVertices.Add();
        IVertex oVertexU = m_oVertices.Add();
        IVertex oVertexV = m_oVertices.Add();
        IVertex oVertexW = m_oVertices.Add();

        // 2-parallel with 2 span vertices.

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);

        // 2-parallel with 3 span vertices.

        m_oEdges.Add(oVertexE, oVertexG);
        m_oEdges.Add(oVertexE, oVertexH);
        m_oEdges.Add(oVertexE, oVertexI);
        m_oEdges.Add(oVertexF, oVertexG);
        m_oEdges.Add(oVertexF, oVertexH);
        m_oEdges.Add(oVertexF, oVertexI);

        // 2-parallel with 4 span vertices.

        m_oEdges.Add(oVertexJ, oVertexL);
        m_oEdges.Add(oVertexJ, oVertexM);
        m_oEdges.Add(oVertexJ, oVertexN);
        m_oEdges.Add(oVertexJ, oVertexO);
        m_oEdges.Add(oVertexK, oVertexL);
        m_oEdges.Add(oVertexK, oVertexM);
        m_oEdges.Add(oVertexK, oVertexN);
        m_oEdges.Add(oVertexK, oVertexO);

        // 2-parallel with 6 span vertices.

        m_oEdges.Add(oVertexP, oVertexR);
        m_oEdges.Add(oVertexP, oVertexS);
        m_oEdges.Add(oVertexP, oVertexT);
        m_oEdges.Add(oVertexP, oVertexU);
        m_oEdges.Add(oVertexP, oVertexV);
        m_oEdges.Add(oVertexP, oVertexW);
        m_oEdges.Add(oVertexQ, oVertexR);
        m_oEdges.Add(oVertexQ, oVertexS);
        m_oEdges.Add(oVertexQ, oVertexT);
        m_oEdges.Add(oVertexQ, oVertexU);
        m_oEdges.Add(oVertexQ, oVertexV);
        m_oEdges.Add(oVertexQ, oVertexW);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(4, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexC, oVertexD }, 0.0,
            oVertexA, oVertexB);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexE, oVertexF }, 0.25,
            oVertexG, oVertexH, oVertexI);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexJ, oVertexK }, 0.5,
            oVertexL, oVertexM, oVertexN, oVertexO);

        VerifyDParallelMotif(oMotifs, new List<IVertex>(){ oVertexP, oVertexQ }, 1.0,
            oVertexR, oVertexS, oVertexT, oVertexU, oVertexV, oVertexW);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs26()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs26()
    {
        // Simple 3-parallel motifs with varying number of span vertices.

        // 3-parallel with 2 span vertices. (A,B,C)<D,E> + A-AAlt
        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();

        IVertex oVertexAAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexC, oVertexD);
        m_oEdges.Add(oVertexC, oVertexE);

        m_oEdges.Add(oVertexA, oVertexAAlt);

        // 3-parallel with 3 span vertices. (F,G,H)<I,J,K> + F-FAlt

        IVertex oVertexF = m_oVertices.Add();
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();

        IVertex oVertexFAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexF, oVertexI);
        m_oEdges.Add(oVertexF, oVertexJ);
        m_oEdges.Add(oVertexF, oVertexK);
        m_oEdges.Add(oVertexG, oVertexI);
        m_oEdges.Add(oVertexG, oVertexJ);
        m_oEdges.Add(oVertexG, oVertexK);
        m_oEdges.Add(oVertexH, oVertexI);
        m_oEdges.Add(oVertexH, oVertexJ);
        m_oEdges.Add(oVertexH, oVertexK);

        m_oEdges.Add(oVertexF, oVertexFAlt);

        // 3-parallel with 4 span vertices. (L,M,N)<O,P,Q,R>

        IVertex oVertexL = m_oVertices.Add();
        IVertex oVertexM = m_oVertices.Add();
        IVertex oVertexN = m_oVertices.Add();
        IVertex oVertexO = m_oVertices.Add();
        IVertex oVertexP = m_oVertices.Add();
        IVertex oVertexQ = m_oVertices.Add();
        IVertex oVertexR = m_oVertices.Add();

        m_oEdges.Add(oVertexL, oVertexO);
        m_oEdges.Add(oVertexL, oVertexP);
        m_oEdges.Add(oVertexL, oVertexQ);
        m_oEdges.Add(oVertexL, oVertexR);
        m_oEdges.Add(oVertexM, oVertexO);
        m_oEdges.Add(oVertexM, oVertexP);
        m_oEdges.Add(oVertexM, oVertexQ);
        m_oEdges.Add(oVertexM, oVertexR);
        m_oEdges.Add(oVertexN, oVertexO);
        m_oEdges.Add(oVertexN, oVertexP);
        m_oEdges.Add(oVertexN, oVertexQ);
        m_oEdges.Add(oVertexN, oVertexR);

        // 3-parallel with 6 span vertices. (S,T,U)<V,W,X,Y,Z,AA>

        IVertex oVertexS = m_oVertices.Add();
        IVertex oVertexT = m_oVertices.Add();
        IVertex oVertexU = m_oVertices.Add();
        IVertex oVertexV = m_oVertices.Add();
        IVertex oVertexW = m_oVertices.Add();
        IVertex oVertexX = m_oVertices.Add();
        IVertex oVertexY = m_oVertices.Add();
        IVertex oVertexZ = m_oVertices.Add();
        IVertex oVertexAA = m_oVertices.Add();

        m_oEdges.Add(oVertexS, oVertexV);
        m_oEdges.Add(oVertexS, oVertexW);
        m_oEdges.Add(oVertexS, oVertexX);
        m_oEdges.Add(oVertexS, oVertexY);
        m_oEdges.Add(oVertexS, oVertexZ);
        m_oEdges.Add(oVertexS, oVertexAA);

        m_oEdges.Add(oVertexT, oVertexV);
        m_oEdges.Add(oVertexT, oVertexW);
        m_oEdges.Add(oVertexT, oVertexX);
        m_oEdges.Add(oVertexT, oVertexY);
        m_oEdges.Add(oVertexT, oVertexZ);
        m_oEdges.Add(oVertexT, oVertexAA);

        m_oEdges.Add(oVertexU, oVertexV);
        m_oEdges.Add(oVertexU, oVertexW);
        m_oEdges.Add(oVertexU, oVertexX);
        m_oEdges.Add(oVertexU, oVertexY);
        m_oEdges.Add(oVertexU, oVertexZ);
        m_oEdges.Add(oVertexU, oVertexAA);
        
        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(4, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB, oVertexC }, 0.0,
            oVertexD, oVertexE);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexF, oVertexG, oVertexH }, 0.25,
            oVertexI, oVertexJ, oVertexK);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexL, oVertexM, oVertexN }, 0.5,
            oVertexO, oVertexP, oVertexQ, oVertexR);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexS, oVertexT, oVertexU }, 1.0,
            oVertexV, oVertexW, oVertexX, oVertexY, oVertexZ, oVertexAA);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs27()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs27()
    {
        // Simple 2-, 3-, and 4-parallel motifs with varying number of span vertices.

        // 2-parallel with 4 span vertices (A,B)<C,D,E,F>
        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexA, oVertexF);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexB, oVertexF);

        // 3-parallel with 2 span vertices. (G,H,I)<J,K> + G-GAlt
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();
        IVertex oVertexK = m_oVertices.Add();

        IVertex oVertexGAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexG, oVertexJ);
        m_oEdges.Add(oVertexG, oVertexK);
        m_oEdges.Add(oVertexH, oVertexJ);
        m_oEdges.Add(oVertexH, oVertexK);
        m_oEdges.Add(oVertexI, oVertexJ);
        m_oEdges.Add(oVertexI, oVertexK);

        m_oEdges.Add(oVertexG, oVertexGAlt);

        // 4-parallel with 3 span vertices. (L,M,N,O)<P,Q,R> + L-LAlt, M-MAlt

        IVertex oVertexL = m_oVertices.Add();
        IVertex oVertexM = m_oVertices.Add();
        IVertex oVertexN = m_oVertices.Add();
        IVertex oVertexO = m_oVertices.Add();
        IVertex oVertexP = m_oVertices.Add();
        IVertex oVertexQ = m_oVertices.Add();
        IVertex oVertexR = m_oVertices.Add();

        IVertex oVertexLAlt = m_oVertices.Add();
        IVertex oVertexMAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexL, oVertexP);
        m_oEdges.Add(oVertexL, oVertexQ);
        m_oEdges.Add(oVertexL, oVertexR);
        m_oEdges.Add(oVertexM, oVertexP);
        m_oEdges.Add(oVertexM, oVertexQ);
        m_oEdges.Add(oVertexM, oVertexR);
        m_oEdges.Add(oVertexN, oVertexP);
        m_oEdges.Add(oVertexN, oVertexQ);
        m_oEdges.Add(oVertexN, oVertexR);
        m_oEdges.Add(oVertexO, oVertexP);
        m_oEdges.Add(oVertexO, oVertexQ);
        m_oEdges.Add(oVertexO, oVertexR);

        m_oEdges.Add(oVertexL, oVertexLAlt);
        m_oEdges.Add(oVertexM, oVertexMAlt);

        // 5-parallel with 2 span vertices. (S,T,U,V,W)<X,Y> + S-SAlt, T-TAlt, U-UAlt, V-VAlt
        IVertex oVertexS = m_oVertices.Add();
        IVertex oVertexT = m_oVertices.Add();
        IVertex oVertexU = m_oVertices.Add();
        IVertex oVertexV = m_oVertices.Add();
        IVertex oVertexW = m_oVertices.Add();
        IVertex oVertexX = m_oVertices.Add();
        IVertex oVertexY = m_oVertices.Add();

        IVertex oVertexSAlt = m_oVertices.Add();
        IVertex oVertexTAlt = m_oVertices.Add();
        IVertex oVertexUAlt = m_oVertices.Add();
        IVertex oVertexVAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexS, oVertexX);
        m_oEdges.Add(oVertexS, oVertexY);
        m_oEdges.Add(oVertexT, oVertexX);
        m_oEdges.Add(oVertexT, oVertexY);
        m_oEdges.Add(oVertexU, oVertexX);
        m_oEdges.Add(oVertexU, oVertexY);
        m_oEdges.Add(oVertexV, oVertexX);
        m_oEdges.Add(oVertexV, oVertexY);
        m_oEdges.Add(oVertexW, oVertexX);
        m_oEdges.Add(oVertexW, oVertexY);

        m_oEdges.Add(oVertexS, oVertexSAlt);
        m_oEdges.Add(oVertexT, oVertexTAlt);
        m_oEdges.Add(oVertexU, oVertexUAlt);
        m_oEdges.Add(oVertexV, oVertexVAlt);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(4, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB }, 1.0,
            oVertexC, oVertexD, oVertexE, oVertexF);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexG, oVertexH, oVertexI }, 0.0,
            oVertexJ, oVertexK);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexL, oVertexM, oVertexN, oVertexO }, 0.5,
            oVertexP, oVertexQ, oVertexR);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexS, oVertexT, oVertexU, oVertexV, oVertexW }, 0.0,
            oVertexX, oVertexY);
    }

    //*************************************************************************
    //  Method: TestCalculateMotifs28()
    //
    /// <summary>
    /// Tests the CalculateMotifs() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateMotifs28()
    {
        // Two DParallel motifs with overlapping anchors

        // 2-parallel with 4 span vertices (A,B)<C,D,E,F>
        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexC);
        m_oEdges.Add(oVertexA, oVertexD);
        m_oEdges.Add(oVertexA, oVertexE);
        m_oEdges.Add(oVertexA, oVertexF);
        m_oEdges.Add(oVertexB, oVertexC);
        m_oEdges.Add(oVertexB, oVertexD);
        m_oEdges.Add(oVertexB, oVertexE);
        m_oEdges.Add(oVertexB, oVertexF);

        // 3-parallel with 2 span vertices. (A,G,H)<I,J> + G-GAlt
        IVertex oVertexG = m_oVertices.Add();
        IVertex oVertexH = m_oVertices.Add();
        IVertex oVertexI = m_oVertices.Add();
        IVertex oVertexJ = m_oVertices.Add();

        IVertex oVertexGAlt = m_oVertices.Add();

        m_oEdges.Add(oVertexG, oVertexI);
        m_oEdges.Add(oVertexG, oVertexJ);
        m_oEdges.Add(oVertexH, oVertexI);
        m_oEdges.Add(oVertexH, oVertexJ);
        m_oEdges.Add(oVertexA, oVertexI);
        m_oEdges.Add(oVertexA, oVertexJ);

        m_oEdges.Add(oVertexG, oVertexGAlt);

        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(2, oMotifs.Count);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexA, oVertexB }, 1.0,
            oVertexC, oVertexD, oVertexE, oVertexF);

        VerifyDParallelMotif(oMotifs, new List<IVertex>() { oVertexG, oVertexH, oVertexA }, 0.0,
            oVertexI, oVertexJ);
    }

    //*************************************************************************
    //  Method: TestDParallelSelfLoops()
    //
    /// <summary>
    /// Test that self loops on two connected nodes doesn't turn into an "anchorless" parallel motif.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDParallelSelfLoops()
    {
        // Invalid 2-parallel motif with a self-loop on each anchor vertex A-A, A-B, B-B
        // We should not find it.

        IVertex oVertexA = m_oVertices.Add();
        oVertexA.Name = "A";
        IVertex oVertexB = m_oVertices.Add();
        oVertexB.Name = "B";

        m_oEdges.Add(oVertexA, oVertexA);
        m_oEdges.Add(oVertexB, oVertexB);
        m_oEdges.Add(oVertexA, oVertexB);
        
        ICollection<Motif> oMotifs;

        Assert.IsTrue(m_oMotifCalculator.TryCalculateMotifs(
            m_oGraph, Motifs.DParallel, 2, Int32.MaxValue, null, out oMotifs));

        Assert.AreEqual(0, oMotifs.Count);
    }

    //*************************************************************************
    //  Method: VerifyFanMotif()
    //
    /// <summary>
    /// Verifies that a fan motif was found.
    /// </summary>
    ///
    /// <param name="oMotifs>
    /// The motifs that were calculated.
    /// </param>
    ///
    /// <param name="oExpectedHeadVertex>
    /// The head vertex of the expected fan motif.
    /// </param>
    ///
    /// <param name="dExpectedArcScale">
    /// The ArcScale value of the expected fan motif.
    /// </param>
    ///
    /// <param name="aoExpectedLeafVertices">
    /// The leaf vertices of the expected fan motif.
    /// </param>
    //*************************************************************************

    protected void
    VerifyFanMotif
    (
        ICollection<Motif> oMotifs,
        IVertex oExpectedHeadVertex,
        Double dExpectedArcScale,
        params IVertex[] aoExpectedLeafVertices
    )
    {
        FanMotif oFanMotif = (FanMotif)oMotifs.Single( oMotif =>
            oMotif is FanMotif
            &&
            ( ( (FanMotif)oMotif ).HeadVertex == oExpectedHeadVertex ) );

        Assert.AreEqual(oExpectedHeadVertex, oFanMotif.HeadVertex);

        Assert.AreEqual(aoExpectedLeafVertices.Length,
            oFanMotif.LeafVertices.Length);

        Assert.AreEqual(dExpectedArcScale, oFanMotif.ArcScale);

        foreach (IVertex oExpectedLeafVertex in aoExpectedLeafVertices)
        {
            IVertex oLeafVertex = oFanMotif.LeafVertices.Single(
                oVertex => (oVertex == oExpectedLeafVertex) );
        }
    }

    //*************************************************************************
    //  Method: VerifyDParallelMotif()
    //
    /// <summary>
    /// Verifies that a D-parallel motif was found.
    /// </summary>
    ///
    /// <param name="oMotifs>
    /// The motifs that were calculated.
    /// </param>
    ///
    /// <param name="oExpectedAnchorVertices>
    /// The anchor vertices of the expected D-parallel motif.
    /// </param>
    ///
    /// <param name="dExpectedSpanScale">
    /// The SpanScale value of the expected two-parallel motif.
    /// </param>
    ///
    /// <param name="aoExpectedSpanVertices">
    /// The span vertices of the expected two-parallel motif.
    /// </param>
    //*************************************************************************

    protected void
    VerifyDParallelMotif
    (
        ICollection<Motif> oMotifs,
        List<IVertex> oExpectedAnchorVertices,
        Double dExpectedSpanScale,
        params IVertex[] aoExpectedSpanVertices
    )
    {
        DParallelMotif oDParallelMotif = (DParallelMotif)oMotifs.Single(
            oMotif =>
            oMotif is DParallelMotif
            &&
            DParallelMotifHasExpectedAnchors((DParallelMotif)oMotif,
                oExpectedAnchorVertices)
            );

        Assert.IsTrue(DParallelMotifHasExpectedAnchors((DParallelMotif)oDParallelMotif,
                oExpectedAnchorVertices));

        Assert.AreEqual(dExpectedSpanScale, oDParallelMotif.SpanScale);

        Assert.AreEqual(aoExpectedSpanVertices.Length,
            oDParallelMotif.SpanVertices.Count);

        foreach (IVertex oExpectedSpanVertex in aoExpectedSpanVertices)
        {
            IVertex oSpanVertex = oDParallelMotif.SpanVertices.Single(
                oVertex => (oVertex == oExpectedSpanVertex));
        }
    }

    //*************************************************************************
    //  Method: DParallelMotifHasExpectedAnchors()
    //
    /// <summary>
    /// Tests whether a D-parallel motif has the expected anchors.
    /// </summary>
    ///
    /// <param name="oDParallelMotif>
    /// The motif to test.
    /// </param>
    ///
    /// <param name="oExpectedAnchorVertices>
    /// The anchor vertices of the expected D-parallel motif.
    /// </param>
    ///
    /// <returns>
    /// true if the specified motif has the expected anchors.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DParallelMotifHasExpectedAnchors
    (
        DParallelMotif oDParallelMotif,
        List<IVertex> oExpectedAnchorVertices
    )
    {
        return UnsortedEquality<IVertex>(oExpectedAnchorVertices, oDParallelMotif.AnchorVertices);
    }

    protected Boolean
    UnsortedEquality<T>
    (
        IEnumerable<T> source,
        IEnumerable<T> destination
    )
    {
        if (source.Count() != destination.Count())
        {
            return false;
        }

        Dictionary<T, int> dictionary = new Dictionary<T, int>();

        foreach (T value in source)
        {
            if (!dictionary.ContainsKey(value))
            {
                dictionary[value] = 1;
            }
            else
            {
                dictionary[value]++;
            }
        }

        foreach (T member in destination)
        {
            if (!dictionary.ContainsKey(member))
            {
                return false;
            }

            dictionary[member]--;
        }

        foreach (KeyValuePair<T,int> kvp in dictionary)
        {
            if (kvp.Value != 0)
            {
                return false;
            }
        }

        return true;
    }



    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being tested.

    protected MotifCalculator m_oMotifCalculator;

    /// The graph being tested.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
