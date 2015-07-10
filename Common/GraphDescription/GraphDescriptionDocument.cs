using System;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Smrf.XmlLib
{
    //*****************************************************************************
    //  Class: GraphDescriptionDocument
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

    public class GraphDescriptionDocument : XmlDocument
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

        public GraphDescriptionDocument
        (
            
        )
        {
            this.AppendChild(this.CreateXmlDeclaration("1.0", "UTF-8", null));

            // (The XML Schema reference presented in the Primer is optional and is
            // skipped here.)

            // Add the root XML node.

            m_oGraphDescriptionXmlNode = AppendXmlNode(this, "GraphDescription");

            m_oContent = new StringBuilder();

            //// Add the graph node and its required directedness value.

            //m_oGraphXmlNode = AppendXmlNode(m_oGraphMLXmlNode, "graph");

            //SetXmlNodeAttributes(m_oGraphXmlNode, "edgedefault",
            //    directed ? "directed" : "undirected");

            m_iVertexXmlNodes = 0;

            AssertValid();
        }

        //*************************************************************************
        //  Property: GraphXmlNode
        //
        /// <summary>
        /// Gets the "graph" XML node.
        /// </summary>
        ///
        /// <value>
        /// The XML node for the "graph" GraphML element.
        /// </value>
        //*************************************************************************

        public XmlNode
        GraphDescriptionXmlNode
        {
            get
            {
                AssertValid();

                return (m_oGraphDescriptionXmlNode);
            }
        }
        

        //*************************************************************************
        //  Property: VertexXmlNodes
        //
        /// <summary>
        /// Gets the number of XML nodes that represent a vertex.
        /// </summary>
        ///
        /// <value>
        /// The number of XML nodes that represent a vertex.
        /// </value>
        //*************************************************************************

        public Int32
        VertexXmlNodes
        {
            get
            {
                AssertValid();

                return (m_iVertexXmlNodes);
            }
        }        

        //*************************************************************************
        //  Method: AppendGraphTypeXmlNode()
        //
        /// <summary>
        /// Creates a new XML node representing a vertex and appends it to the
        /// "graph" XML node.
        /// </summary>
        ///
        /// <param name="vertexID">
        /// Vertex ID.
        /// </param>
        ///
        /// <returns>
        /// The new node.
        /// </returns>
        //*************************************************************************

        public XmlNode
        AppendGraphTypeXmlNode
        (
            String graphType
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(graphType));
            AssertValid();

            // The "id" attribute is required.

            XmlNode oGraphTypeXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "GraphType", graphType);
            
            return (oGraphTypeXmlNode);
        }

        //*************************************************************************
        //  Method: AppendGraphSourceXmlNode()
        //
        /// <summary>
        /// Creates a new XML node representing an edge and appends it to the
        /// "graph" XML node.
        /// </summary>
        ///
        /// <param name="vertex1ID">
        /// ID of the edge's first vertex.
        /// </param>
        ///
        /// <param name="vertex2ID">
        /// ID of the edge's second vertex.
        /// </param>
        ///
        /// <returns>
        /// The new node.
        /// </returns>
        //*************************************************************************

        public XmlNode
        AppendGraphSourceXmlNode
        (
            String graphSource
        )
        {
            
            Debug.Assert(!String.IsNullOrEmpty(graphSource));            
            AssertValid();

            XmlNode oGraphSourceXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "GraphSource", graphSource);            

            return (oGraphSourceXmlNode);
        }

        public XmlNode
        AppendGraphTermXmlNode
        (
            String graphTerm
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(graphTerm));
            AssertValid();

            XmlNode oGraphTermXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "GraphTerm", graphTerm);

            return (oGraphTermXmlNode);
        }

        public XmlNode
        AppendDescriptionXmlNode
        (
            String description
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(description));
            AssertValid();

            XmlNode oDescriptionXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "Description", description);

            m_oContent.AppendLine(description);

            return (oDescriptionXmlNode);
        }

        public XmlNode
        AppendShortDescriptionXmlNode
        (
            String shortDescription
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(shortDescription));
            AssertValid();

            XmlNode oShortDescriptionXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "ShortDescription", shortDescription);

            return (oShortDescriptionXmlNode);
        }

        public XmlNode
        AppendSectionsXmlNode
        (
        )
        {
            AssertValid();
            XmlNode oSectionsXmlNode = AppendXmlNode(m_oGraphDescriptionXmlNode, "Sections");

            return (oSectionsXmlNode);
        }

        public XmlNode
        AppendSectionXmlNode
        (
            XmlNode sections,
            String name,
            String value
        )
        {
            AssertValid();
            XmlNode oSectionXmlNode = AppendXmlNode(sections, "Section");
            SetXmlNodeAttributes(oSectionXmlNode, "name", name, "value", value);

            m_oContent.AppendLine();
            m_oContent.AppendLine(value);

            return (oSectionXmlNode);
        }

        public XmlNode
        AppendKeyValueElementXmlNode
        (
            XmlNode section,
            String key,
            String value
        )
        {
            AssertValid();
            XmlNode oKeyValueElementXmlNode = AppendXmlNode(section, "Element");
            SetXmlNodeAttributes(oKeyValueElementXmlNode, "type", "KeyValueElement");
            AppendXmlNode(oKeyValueElementXmlNode, "Key", key);
            AppendXmlNode(oKeyValueElementXmlNode, "Value", value);

            m_oContent.AppendLine(key + ":" + value);

            return (oKeyValueElementXmlNode);
        }

        public XmlNode
        AppendValueElementXmlNode
        (
            XmlNode section,
            String value
        )
        {
            AssertValid();
            XmlNode oValueElementXmlNode = AppendXmlNode(section, "Element");
            SetXmlNodeAttributes(oValueElementXmlNode, "type", "ValueElement");
            AppendXmlNode(oValueElementXmlNode, "Value", value);

            m_oContent.AppendLine(value);

            return (oValueElementXmlNode);
        }

        public XmlNode
        AppendActionsXmlNode
        (
            XmlNode element            
        )
        {
            AssertValid();
            XmlNode oActionsXmlNode = AppendXmlNode(element, "Actions");

            return (oActionsXmlNode);
        }

        public XmlNode
        AppendActionXmlNode
        (
            XmlNode actions,
            String name,
            String URL
        )
        {
            AssertValid();
            XmlNode oActionXmlNode = AppendXmlNode(actions, "Action");
            SetXmlNodeAttributes(oActionXmlNode, "name", name, "URL", URL);

            return (oActionXmlNode);
        }

        public String 
        ToString
        (
        )
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                this.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public String
        ToDisplayableString
        (
        )
        {
            return (m_oContent.ToString());
        }

        

        //*************************************************************************
        //  Method: AppendXmlNode()
        //
        /// <overloads>
        /// Creates a new XML node and appends it to a parent node.
        /// </overloads>
        ///
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
            Debug.Assert(!String.IsNullOrEmpty(sChildName));
            // AssertValid();

            return (XmlUtil2.AppendNewNode(oParentXmlNode, sChildName,
                GraphDescriptionNamespaceUri));
        }

        //*************************************************************************
        //  Method: AppendXmlNode()
        //
        /// <summary>
        /// Creates a new XML node, appends it to a parent node, and sets its inner
        /// text.
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
        /// <param name="sInnerText">
        /// The new node's inner text.  Can be empty but not null.
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
            String sChildName,
            String sInnerText
        )
        {
            Debug.Assert(oParentXmlNode != null);
            Debug.Assert(!String.IsNullOrEmpty(sChildName));
            Debug.Assert(sInnerText != null);
            AssertValid();

            XmlNode oNewXmlNode = AppendXmlNode(oParentXmlNode, sChildName);
            oNewXmlNode.InnerText = sInnerText;

            return (oNewXmlNode);
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
        //  Method: CreateXmlNamespaceManager()
        //
        /// <overloads>
        /// Creates an XmlNamespaceManager object to use with the document.
        /// </overloads>
        ///
        /// <summary>
        /// Creates an XmlNamespaceManager object to use with this document.
        /// </summary>
        ///
        /// <param name="prefix">
        /// The prefix to use for the default GraphML namespace.
        /// </param>
        ///
        /// <returns>
        /// An XmlNamespaceManager object to use with the document.
        /// </returns>
        ///
        /// <remarks>
        /// Any prefix will do, so long as it is also included in every XPath
        /// expression when the document is searched.  For example, if you want all
        /// "node" XML nodes in the document, this would work:
        ///
        /// <code>
        /// XmlNamespaceManager oXmlNamespaceManager =
        ///     oGraphMLXmlDocument.CreateXmlNamespaceManager("g");
        ///
        /// foreach ( XmlNode oVertexXmlNode in oGraphMLXmlDocument.SelectNodes(
        ///     "g:graphml/g:graph/g:node", oXmlNamespaceManager) )
        /// {
        /// ...
        /// }
        /// </code>
        ///
        /// </remarks>
        //*************************************************************************

        public XmlNamespaceManager
        CreateXmlNamespaceManager
        (
            String prefix
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(prefix));

            return (CreateXmlNamespaceManager(this, prefix));
        }

        //*************************************************************************
        //  Method: CreateXmlNamespaceManager()
        //
        /// <summary>
        /// Creates an XmlNamespaceManager object to use with a specified document.
        /// </summary>
        ///
        /// <param name="graphMLXmlDocument">
        /// XML document containing GraphML that represents a graph.  This does not
        /// have to be a GraphMLXmlDocument.
        /// </param>
        ///
        /// <param name="prefix">
        /// The prefix to use for the default GraphML namespace.
        /// </param>
        ///
        /// <returns>
        /// An XmlNamespaceManager object to use with the document.
        /// </returns>
        ///
        /// <remarks>
        /// See the other overload for details on using this method.
        /// </remarks>
        //*************************************************************************

        public static XmlNamespaceManager
        CreateXmlNamespaceManager
        (
            XmlDocument graphMLXmlDocument,
            String prefix
        )
        {
            Debug.Assert(graphMLXmlDocument != null);
            Debug.Assert(!String.IsNullOrEmpty(prefix));

            XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
                graphMLXmlDocument.NameTable);

            oXmlNamespaceManager.AddNamespace(prefix, GraphDescriptionNamespaceUri);

            return (oXmlNamespaceManager);
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
            Debug.Assert(m_oGraphDescriptionXmlNode != null);            
            Debug.Assert(m_iVertexXmlNodes >= 0);
        }


        //*************************************************************************
        //  Public constants
        //*************************************************************************

        /// GraphML namespace URI.

        public const String GraphDescriptionNamespaceUri =
            "http://nodexlgraphgallery.org/xmlns";


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// Root GraphDescription XML node.

        protected XmlNode m_oGraphDescriptionXmlNode;

        /// Graph XML node.

        protected XmlNode m_oGraphXmlNode2;

        /// The number of XML nodes that represent a vertex.

        protected Int32 m_iVertexXmlNodes;

        protected StringBuilder m_oContent;
    }

}
