
using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.UnitTests;
using Smrf.AppLib;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GexfGraphAdapterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GexfGraphAdapter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GexfGraphAdapterTest : Object
{
    //*************************************************************************
    //  Constructor: GexfGraphAdapterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GexfGraphAdapterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GexfGraphAdapterTest()
    {
        m_oGraphAdapter = null;
        m_sTempFileName = null;
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
        m_oGraphAdapter = new GexfGraphAdapter();
        m_sTempFileName = Path.GetTempFileName();
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
        m_oGraphAdapter = null;

        if ( File.Exists(m_sTempFileName) )
        {
            File.Delete(m_sTempFileName);
        }
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestSaveGraph()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph()
    {
        // Simple "hello world" graph from the "GEXF 1.2draft Primer".  No
        // attributes.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            IGraph oGraph = new Graph(bDirected ? GraphDirectedness.Directed
                : GraphDirectedness.Undirected);

            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;

            IVertex oVertex1 = oVertices.Add();
            oVertex1.Name = "Hello";

            IVertex oVertex2 = oVertices.Add();
            oVertex2.Name = "world";

            IEdge oEdge1 = oEdges.Add(oVertex1, oVertex2, bDirected);

            oGraph.SetValue( ReservedMetadataKeys.AllVertexMetadataKeys,
                new String [] {
                    } );

            oGraph.SetValue(ReservedMetadataKeys.AllEdgeMetadataKeys,
                new String [] {
                    } );

            m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

            String sFileContents = FileUtil.ReadTextFile(m_sTempFileName);

            XmlDocument oXmlDocument = new XmlDocument();
            oXmlDocument.LoadXml(sFileContents);
            CheckSchemaLocation(oXmlDocument);

            XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
                oXmlDocument.NameTable);

            oXmlNamespaceManager.AddNamespace("g",
                GexfGraphAdapter.GexfNamespaceUri);

            String [] asRequiredXPaths = new String [] {

                // Graph node.

                String.Format(
                    "/g:gexf/g:graph[@defaultedgetype='{0}']"
                    ,
                    bDirected ? "directed" : "undirected"
                    ),


                // Vertex nodes.

                String.Format(
                    "/g:gexf/g:graph/g:nodes/g:node[@id='{0}'"
                    + " and @label='Hello']"
                    ,
                    oVertex1.ID.ToString(CultureInfo.InvariantCulture)
                    ),

                String.Format(
                    "/g:gexf/g:graph/g:nodes/g:node[@id='{0}'"
                    + " and @label='world']"
                    ,
                    oVertex2.ID.ToString(CultureInfo.InvariantCulture)
                    ),


                // Edge nodes.

                String.Format(
                    "/g:gexf/g:graph/g:edges/g:edge[@id='{0}' and"
                    + " @source='{1}' and @target='{2}']"
                    ,
                    oEdge1.ID.ToString(CultureInfo.InvariantCulture),
                    oVertex1.ID.ToString(CultureInfo.InvariantCulture),
                    oVertex2.ID.ToString(CultureInfo.InvariantCulture)
                    )
                };

            Int32 iRequiredXPaths = asRequiredXPaths.Length;

            for (Int32 i = 0; i < iRequiredXPaths; i++)
            {
                String sRequiredXPath = asRequiredXPaths[i];

                XmlNode oXmlNode = oXmlDocument.SelectSingleNode(
                    sRequiredXPath, oXmlNamespaceManager);

                if (oXmlNode == null)
                {
                    Assert.Fail("Failed on index " + i + ".");
                }
            }

            ValidateAgainstSchema(oXmlDocument);
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraph2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2()
    {
        // With attributes.

        const String VertexAttribute1Key = "VertexAttribute1";
        const String VertexAttribute2Key = "VertexAttribute2";

        const String EdgeAttribute1Key = "EdgeAttribute2";

        Boolean bDirected = true;

        IGraph oGraph = new Graph(bDirected ? GraphDirectedness.Directed
            : GraphDirectedness.Undirected);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;

        IVertex oVertex1 = oVertices.Add();
        oVertex1.Name = "Vertex1";
        oVertex1.SetValue(VertexAttribute1Key, "abcd");
        oVertex1.SetValue(VertexAttribute2Key, "123");

        IVertex oVertex2 = oVertices.Add();
        oVertex2.Name = "Vertex2";
        oVertex2.SetValue(VertexAttribute1Key, "efg");
        oVertex2.SetValue(VertexAttribute2Key, "4567");

        IVertex oVertex3 = oVertices.Add();
        oVertex3.Name = "Vertex3";
        oVertex3.SetValue(VertexAttribute1Key, "9876");

        IEdge oEdge1 = oEdges.Add(oVertex1, oVertex2, bDirected);
        oEdge1.SetValue(EdgeAttribute1Key, "value 1");

        IEdge oEdge2 = oEdges.Add(oVertex2, oVertex3, bDirected);
        oEdge2.SetValue(EdgeAttribute1Key, "value 2");

        oGraph.SetValue( ReservedMetadataKeys.AllVertexMetadataKeys,
            new String [] {
                VertexAttribute1Key,
                VertexAttribute2Key
                } );

        oGraph.SetValue(ReservedMetadataKeys.AllEdgeMetadataKeys,
            new String [] {
                EdgeAttribute1Key
                } );

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents = FileUtil.ReadTextFile(m_sTempFileName);

        XmlDocument oXmlDocument = new XmlDocument();
        oXmlDocument.LoadXml(sFileContents);
        CheckSchemaLocation(oXmlDocument);

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            oXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace("g",
            GexfGraphAdapter.GexfNamespaceUri);

        String [] asRequiredXPaths = new String [] {

            // Graph node.

            String.Format(
                "/g:gexf/g:graph[@defaultedgetype='{0}']"
                ,
                bDirected ? "directed" : "undirected"
                ),


            // Attribute nodes.

            String.Format(
                "/g:gexf/g:graph/g:attributes[@class='node']/"
                + "g:attribute[@id='V-{0}' and @title='{0}'"
                + "and @type='string']"
                ,
                VertexAttribute1Key
                ),

            String.Format(
                "/g:gexf/g:graph/g:attributes[@class='node']/"
                + "g:attribute[@id='V-{0}' and @title='{0}'"
                + "and @type='string']"
                ,
                VertexAttribute2Key
                ),

            String.Format(
                "/g:gexf/g:graph/g:attributes[@class='edge']/"
                + "g:attribute[@id='E-{0}' and @title='{0}'"
                + "and @type='string']"
                ,
                EdgeAttribute1Key
                ),


            // Vertex nodes.

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}'"
                + " and @label='Vertex1']"
                ,
                oVertex1.ID.ToString(CultureInfo.InvariantCulture)
                ),

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='abcd']"
                ,
                oVertex1.ID.ToString(CultureInfo.InvariantCulture),
                "V-" + VertexAttribute1Key
                ),

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='123']"
                ,
                oVertex1.ID.ToString(CultureInfo.InvariantCulture),
                "V-" + VertexAttribute2Key
                ),



            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}'"
                + " and @label='Vertex2']"
                ,
                oVertex2.ID.ToString(CultureInfo.InvariantCulture)
                ),

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='efg']"
                ,
                oVertex2.ID.ToString(CultureInfo.InvariantCulture),
                "V-" + VertexAttribute1Key
                ),

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='4567']"
                ,
                oVertex2.ID.ToString(CultureInfo.InvariantCulture),
                "V-" + VertexAttribute2Key
                ),



            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}'"
                + " and @label='Vertex3']"
                ,
                oVertex3.ID.ToString(CultureInfo.InvariantCulture)
                ),

            String.Format(
                "/g:gexf/g:graph/g:nodes/g:node[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='9876']"
                ,
                oVertex3.ID.ToString(CultureInfo.InvariantCulture),
                "V-" + VertexAttribute1Key
                ),


            // Edge nodes.

            String.Format(
                "/g:gexf/g:graph/g:edges/g:edge[@id='{0}' and"
                + " @source='{1}' and @target='{2}']"
                ,
                oEdge1.ID.ToString(CultureInfo.InvariantCulture),
                oVertex1.ID.ToString(CultureInfo.InvariantCulture),
                oVertex2.ID.ToString(CultureInfo.InvariantCulture)
                ),

            String.Format(
                "/g:gexf/g:graph/g:edges/g:edge[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='value 1']"
                ,
                oEdge1.ID.ToString(CultureInfo.InvariantCulture),
                "E-" + EdgeAttribute1Key
                ),


            String.Format(
                "/g:gexf/g:graph/g:edges/g:edge[@id='{0}' and"
                + " @source='{1}' and @target='{2}']"
                ,
                oEdge2.ID.ToString(CultureInfo.InvariantCulture),
                oVertex2.ID.ToString(CultureInfo.InvariantCulture),
                oVertex3.ID.ToString(CultureInfo.InvariantCulture)
                ),

            String.Format(
                "/g:gexf/g:graph/g:edges/g:edge[@id='{0}']/g:attvalues/"
                + "g:attvalue[@for='{1}' and @value='value 2']"
                ,
                oEdge2.ID.ToString(CultureInfo.InvariantCulture),
                "E-" + EdgeAttribute1Key
                ),
            };

        Int32 iRequiredXPaths = asRequiredXPaths.Length;

        for (Int32 i = 0; i < iRequiredXPaths; i++)
        {
            String sRequiredXPath = asRequiredXPaths[i];

            XmlNode oXmlNode = oXmlDocument.SelectSingleNode(
                sRequiredXPath, oXmlNamespaceManager);

            if (oXmlNode == null)
            {
                Assert.Fail("Failed on index " + i + ".");
            }
        }

        ValidateAgainstSchema(oXmlDocument);
    }

    //*************************************************************************
    //  Method: CheckSchemaLocation()
    //
    /// <summary>
    /// Checks whether a saved graph has an expected schema location attribute.
    /// </summary>
    ///
    /// <param name="oXmlDocument">
    /// The XML document.
    /// </param>
    //*************************************************************************

    protected void
    CheckSchemaLocation
    (
        XmlDocument oXmlDocument
    )
    {
        XmlAttribute oXmlAttribute = oXmlDocument.DocumentElement.Attributes[
            "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance"];

        Assert.IsNotNull(oXmlAttribute);

        Assert.AreEqual(
            "http://www.gexf.net/1.2draft"
            + " http://www.gexf.net/1.2draft/gexf.xsd",
            
            oXmlAttribute.Value
            );
    }

    //*************************************************************************
    //  Method: ValidateAgainstSchema()
    //
    /// <summary>
    /// Checks whether a saved graph has an expected schema location attribute.
    /// </summary>
    ///
    /// <param name="oXmlDocument">
    /// The XML document.
    /// </param>
    //*************************************************************************

    protected void
    ValidateAgainstSchema
    (
        XmlDocument oXmlDocument
    )
    {
        oXmlDocument.Schemas.Add("http://www.gexf.net/1.2draft",
            "http://www.gexf.net/1.2draft/gexf.xsd");

        oXmlDocument.Validate(new System.Xml.Schema.ValidationEventHandler(
            this.ValidationEventHandler));
    }

    //*************************************************************************
    //  Method: ValidationEventHandler()
    //
    /// <summary>
    /// Handles errors during schema validation.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    ValidationEventHandler
    (
        object sender,
        ValidationEventArgs e
    )
    {
        // All such events are validation failures.

        Assert.Fail(e.Message);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IGraphAdapter m_oGraphAdapter;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
