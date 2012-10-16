
using System;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.XmlLib;

namespace Smrf.NodeXL.Adapters
{
//*****************************************************************************
//  Class: GexfGraphAdapter
//
/// <summary>
/// Converts a graph to GEXF format.
/// </summary>
///
/// <remarks>
/// GEXF is an XML format used by the Gephi program.  The format is documented
/// at http://gexf.net/format/.
///
/// <para>
/// This class saves to but does not load from GEXF files.  It does not support
/// mixed-directedness graphs.
/// </para>
///
/// <para>
/// When saving a graph, the <see
/// cref="IGraphAdapter.SaveGraph(IGraph, Stream)" /> caller must add <see
/// cref="ReservedMetadataKeys.AllEdgeMetadataKeys" /> and <see
/// cref="ReservedMetadataKeys.AllVertexMetadataKeys" /> keys to the graph
/// before calling <see cref="IGraphAdapter.SaveGraph(IGraph, Stream)" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GexfGraphAdapter : GraphAdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: GexfGraphAdapter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GexfGraphAdapter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GexfGraphAdapter()
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetSupportedDirectedness()
    //
    /// <summary>
    /// Gets a set of flags indicating the directedness of the graphs that the
    /// implementation can load and save.
    /// </summary>
    ///
    /// <param name="supportsDirected">
    /// Gets set to true if the implementation can load and save directed
    /// graphs.
    /// </param>
    ///
    /// <param name="supportsUndirected">
    /// Gets set to true if the implementation can load and save undirected
    /// graphs.
    /// </param>
    ///
    /// <param name="supportsMixed">
    /// Gets set to true if the implementation can load and save mixed graphs.
    /// </param>
    //*************************************************************************

    protected override void
    GetSupportedDirectedness
    (
        out Boolean supportsDirected,
        out Boolean supportsUndirected,
        out Boolean supportsMixed
    )
    {
        AssertValid();

        supportsDirected = true;
        supportsUndirected = true;
        supportsMixed = false;
    }

    //*************************************************************************
    //  Method: LoadGraphCore()
    //
    /// <summary>
    /// Creates a graph and loads it with graph data read from a <see
    /// cref="Stream" />.
    /// </summary>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="stream" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph, loads it with the graph data read from
    /// <paramref name="stream" />.  It does not close <paramref
    /// name="stream" />.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected override IGraph
    LoadGraphCore
    (
        Stream stream
    )
    {
        Debug.Assert(stream != null);
        AssertValid();

        throw new NotImplementedException();
    }

    //*************************************************************************
    //  Method: SaveGraphCore()
    //
    /// <summary>
    /// Saves graph data to a stream.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="stream">
    /// Stream to save the graph data to.
    /// </param>
    ///
    /// <remarks>
    /// This method saves <paramref name="graph" /> to <paramref
    /// name="stream" />.  It does not close <paramref name="stream" />.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveGraphCore
    (
        IGraph graph,
        Stream stream
    )
    {
        Debug.Assert(graph != null);

        Debug.Assert(graph.Directedness == GraphDirectedness.Directed ||
            graph.Directedness == GraphDirectedness.Undirected);

        Debug.Assert(stream != null);
        AssertValid();

        XmlDocument oXmlDocument = CreateXmlDocument(graph);

        XmlNode oGexfXmlNode = AppendXmlNode(oXmlDocument, "gexf");
        SetXmlNodeAttributes(oGexfXmlNode, "version", "1.2");

        AppendSchemaLocation(oXmlDocument);

        XmlNode oGraphXmlNode = AppendGraphXmlNode(graph, oGexfXmlNode);

        String [] asVertexAttributeNames = GetAttributeNames(graph, true);
        String [] asEdgeAttributeNames = GetAttributeNames(graph, false);

        AppendAttributesXmlNode(oGraphXmlNode, true, asVertexAttributeNames);
        AppendAttributesXmlNode(oGraphXmlNode, false, asEdgeAttributeNames);

        AppendVertices(graph, oGraphXmlNode, asVertexAttributeNames);
        AppendEdges(graph, oGraphXmlNode, asEdgeAttributeNames);

        oXmlDocument.Save(stream);
    }

    //*************************************************************************
    //  Method: CreateXmlDocument()
    //
    /// <summary>
    /// Creates an XML document for a saved GEXF file.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <returns>
    /// A new XML document.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    CreateXmlDocument
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        XmlDocument oXmlDocument = new XmlDocument();

        oXmlDocument.AppendChild(
            oXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null) );

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: AppendGraphXmlNode()
    //
    /// <summary>
    /// Appends a "graph" XML node.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="oGexfXmlNode">
    /// The "gexf" XML node.
    /// </param>
    ///
    /// <returns>
    /// A new "graph" XML node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendGraphXmlNode
    (
        IGraph graph,
        XmlNode oGexfXmlNode
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(oGexfXmlNode != null);
        AssertValid();

        XmlNode oGraphXmlNode = AppendXmlNode(oGexfXmlNode, "graph");

        SetXmlNodeAttributes(oGraphXmlNode, "defaultedgetype",
            (graph.Directedness == GraphDirectedness.Directed)
            ? "directed" : "undirected");

        return (oGraphXmlNode);
    }

    //*************************************************************************
    //  Method: AppendSchemaLocation()
    //
    /// <summary>
    /// Appends the GEXF schema location to the XmlDocument.
    /// </summary>
    ///
    /// <param name="oXmlDocument">
    /// XML document.
    /// </param>
    ///
    /// <remarks>
    /// The XmlDocument must already have a root document element.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendSchemaLocation
    (
        XmlDocument oXmlDocument
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert(oXmlDocument.DocumentElement != null);
        AssertValid();

        XmlAttribute oSchemaLocation = oXmlDocument.CreateAttribute(
            "xsi", "schemaLocation",
            "http://www.w3.org/2001/XMLSchema-instance"
            );

        oSchemaLocation.Value = GexfNamespaceUri + " " + GexfSchemaUri;

        oXmlDocument.DocumentElement.SetAttributeNode(oSchemaLocation);
    }

    //*************************************************************************
    //  Method: AppendAttributesXmlNode()
    //
    /// <summary>
    /// Appends attribute declarations for vertices or edges to the "graph" XML
    /// node.
    /// </summary>
    ///
    /// <param name="oGraphXmlNode">
    /// The XML document's "graph" XML node.
    /// </param>
    ///
    /// <param name="bForVertices">
    /// true to append vertex attribute declarations, false to append edge
    /// attribute declarations.
    /// </param>
    ///
    /// <param name="asAttributeNames">
    /// An array of zero or more attribute names.
    /// </param>
    //*************************************************************************

    protected void
    AppendAttributesXmlNode
    (
        XmlNode oGraphXmlNode,
        Boolean bForVertices,
        String [] asAttributeNames
    )
    {
        Debug.Assert(oGraphXmlNode != null);
        Debug.Assert(asAttributeNames != null);
        AssertValid();

        XmlNode oAttributesXmlNode =
            AppendXmlNode(oGraphXmlNode, "attributes");

        SetXmlNodeAttributes(oAttributesXmlNode,
            "class", bForVertices ? "node" : "edge"
            );

        foreach (String sAttributeName in asAttributeNames)
        {
            XmlNode oAttributeXmlNode = AppendXmlNode(oAttributesXmlNode,
                "attribute");

            SetXmlNodeAttributes(oAttributeXmlNode,

                "id", (bForVertices ? VertexAttributeIDPrefix :
                    EdgeAttributeIDPrefix) + sAttributeName,

                "title", sAttributeName,
                "type", "string"
                );
        }
    }

    //*************************************************************************
    //  Method: AppendVertices()
    //
    /// <summary>
    /// Appends the graph's vertices to the XML document.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="oGraphXmlNode">
    /// The XML document's "graph" XML node.
    /// </param>
    ///
    /// <param name="asVertexAttributeNames">
    /// An array of zero or more vertex attribute names.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertices
    (
        IGraph graph,
        XmlNode oGraphXmlNode,
        String [] asVertexAttributeNames
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(oGraphXmlNode != null);
        Debug.Assert(asVertexAttributeNames != null);
        AssertValid();

        XmlNode oNodesXmlNode = AppendXmlNode(oGraphXmlNode, "nodes");

        foreach (IVertex oVertex in graph.Vertices)
        {
            XmlNode oNodeXmlNode = AppendXmlNode(oNodesXmlNode, "node");

            SetXmlNodeAttributes(oNodeXmlNode,
                "id", GetCultureInvariantID(oVertex),
                "label", oVertex.Name
                );

            AppendAttValueXmlNodes(oVertex, oNodeXmlNode,
                asVertexAttributeNames, VertexAttributeIDPrefix);
        }
    }

    //*************************************************************************
    //  Method: AppendEdges()
    //
    /// <summary>
    /// Appends the graph's edges to the XML document.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="oGraphXmlNode">
    /// The XML document's "graph" XML node.
    /// </param>
    ///
    /// <param name="asEdgeAttributeNames">
    /// An array of zero or more edge attribute names.
    /// </param>
    //*************************************************************************

    protected void
    AppendEdges
    (
        IGraph graph,
        XmlNode oGraphXmlNode,
        String [] asEdgeAttributeNames
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(oGraphXmlNode != null);
        Debug.Assert(asEdgeAttributeNames != null);
        AssertValid();

        XmlNode oEdgesXmlNode = AppendXmlNode(oGraphXmlNode, "edges");

        foreach (IEdge oEdge in graph.Edges)
        {
            XmlNode oEdgeXmlNode = AppendXmlNode(oEdgesXmlNode, "edge");

            SetXmlNodeAttributes(oEdgeXmlNode,
                "id", GetCultureInvariantID(oEdge),
                "source", GetCultureInvariantID(oEdge.Vertex1),
                "target", GetCultureInvariantID(oEdge.Vertex2)
                );

            AppendAttValueXmlNodes(oEdge, oEdgeXmlNode, asEdgeAttributeNames,
                EdgeAttributeIDPrefix);
        }
    }

    //*************************************************************************
    //  Method: AppendAttValueXmlNodes()
    //
    /// <summary>
    /// Appends "attvalue" XML nodes to a vertex or edge XML node.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to read metadata from.
    /// </param>
    ///
    /// <param name="oNodeOrEdgeXmlNode">
    /// The node or edge XML node in that corresponds to <paramref
    /// name="oVertexOrEdge" />.
    /// </param>
    ///
    /// <param name="asAttributeNames">
    /// Array of all possible attribute names for the edge or vertex.
    /// </param>
    ///
    /// <param name="sAttributeIDPrefix">
    /// The prefix to use for each attribute ID.
    /// </param>
    //*************************************************************************

    protected void
    AppendAttValueXmlNodes
    (
        IMetadataProvider oVertexOrEdge,
        XmlNode oNodeOrEdgeXmlNode,
        String [] asAttributeNames,
        String sAttributeIDPrefix
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        Debug.Assert(oNodeOrEdgeXmlNode != null);
        Debug.Assert(asAttributeNames != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeIDPrefix) );
        AssertValid();

        XmlNode oAttValuesXmlNode = null;

        if (asAttributeNames.Length > 0)
        {
            oAttValuesXmlNode = AppendXmlNode(oNodeOrEdgeXmlNode, "attvalues");
        }

        foreach (String sAttributeName in asAttributeNames)
        {
            Object oAttributeValue;

            // Note that the value type isn't checked.  Whatever type it is,
            // it gets converted to a string.

            if (oVertexOrEdge.TryGetValue(sAttributeName,
                out oAttributeValue) && oAttributeValue != null)
            {
                Debug.Assert(oAttValuesXmlNode != null);

                XmlNode oAttValueXmlNode = AppendXmlNode(oAttValuesXmlNode,
                    "attvalue");

                SetXmlNodeAttributes(oAttValueXmlNode,
                    "for", sAttributeIDPrefix + sAttributeName,
                    "value", oAttributeValue.ToString()
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: AppendXmlNode()
    //
    /// <summary>
    /// Creates a new XML node and appends it to a parent node.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendXmlNode
    (
        XmlNode oParentXmlNode,
        String sChildName
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sChildName) );
        AssertValid();

        return ( XmlUtil2.AppendNewNode(oParentXmlNode, sChildName,
            GexfNamespaceUri) );
    }

    //*************************************************************************
    //  Method: SetXmlNodeAttributes()
    //
    /// <summary>
    /// Sets multiple attributes on an XML node.
    /// </summary>
    ///
    /// <param name="oXmlNode">
    /// Node to set attributes on.
    /// </param>
    ///
    /// <param name="asNameValuePairs">
    /// One or more pairs of strings.  The first string in each pair is an
    /// attribute name and the second is the attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This sets multiple attributes on an XML node in one call.  It's an
    /// alternative to calling <see
    /// cref="XmlElement.SetAttribute(String, String)" /> repeatedly.
    /// </remarks>
    //*************************************************************************

    protected void
    SetXmlNodeAttributes
    (
        XmlNode oXmlNode,
        params String[] asNameValuePairs
    )
    {
        Debug.Assert(oXmlNode != null);
        Debug.Assert(asNameValuePairs != null);
        AssertValid();

        XmlUtil2.SetAttributes(oXmlNode, asNameValuePairs);
    }

    //*************************************************************************
    //  Method: GetCultureInvariantID()
    //
    /// <summary>
    /// Gets the ID of a vertex or edge, in a culture-invariant String format.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// Vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// "23", for example.
    /// </returns>
    //*************************************************************************

    protected String
    GetCultureInvariantID
    (
        IIdentityProvider oVertexOrEdge
    )
    {
        Debug.Assert(oVertexOrEdge != null);

        return ( oVertexOrEdge.ID.ToString(CultureInfo.InvariantCulture) );
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
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Prefix used for the "id" attribute names on vertex "attribute" XML
    /// nodes.

    protected const String VertexAttributeIDPrefix = "V-";

    /// Prefix used for the "id" attribute names on edge "attribute" XML nodes.

    protected const String EdgeAttributeIDPrefix = "E-";


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// GEXF namespace.

    public const String GexfNamespaceUri = "http://www.gexf.net/1.2draft";

    /// GEXF schema URI.

    public const String GexfSchemaUri =
        "http://www.gexf.net/1.2draft/gexf.xsd";
}

}
