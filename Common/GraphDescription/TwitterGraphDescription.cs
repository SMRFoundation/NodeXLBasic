using System;
using System.Xml;
using System.Globalization;
using System.Diagnostics;

namespace Smrf.XmlLib
{
    //*****************************************************************************
    //  Class: FacebookGraphDescription
    //
    /// <summary>
    /// Represents an XML document containing GraphML that represents a graph.
    /// </summary>
    ///
    /// <remarks>
    /// See the "GraphML Primer" for details on the GraphML XML schema:
    ///
    /// <para>
    /// http://graphml.graphdrawing.org/primer/graphml-primer.html
    /// </para>
    ///
    /// <para>
    /// Creating a <see cref="GraphMLXmlDocument" /> automatically creates an XML
    /// declaration, a root "graphml" XML node, and a "graph" child XML node.  Use
    /// <see cref="DefineGraphMLAttribute" />, <see cref="AppendVertexXmlNode" />,
    /// <see cref="AppendEdgeXmlNode" />, and <see
    /// cref="AppendGraphMLAttributeValue(XmlNode, String, String)" /> to populate
    /// the document with vertices, edges, and vertex/edge attributes.
    /// </para>
    ///
    /// </remarks>
    //*****************************************************************************

    public class TwitterGraphDescription : GraphDescriptionDocument
    {
        //*************************************************************************
        //  Constructor: GraphMLXmlDocument()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMLXmlDocument" />
        /// class.
        /// </summary>
        ///
        /// <param name="directed">
        /// true if the graph is directed, false if it is undirected.
        /// </param>
        //*************************************************************************

        public TwitterGraphDescription
        (
            String graphType,
            String termUserName
        )
            :
            base()
        {
            AppendGraphTypeXmlNode(graphType);
            AppendGraphSourceXmlNode("Twitter");
            AppendGraphTermXmlNode(termUserName);
        }

    }

}
