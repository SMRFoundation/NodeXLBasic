using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.XmlLib;
using System.Xml;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookNetworkLoaderBase
    {
        protected VertexCollection m_oVertices;
        protected EdgeCollection m_oEdges;
        protected FacebookModelBase m_oModel;
        protected FacebookNetworkAnalyzerBase m_oNetworkAnalyzerBase;

        public FacebookNetworkLoaderBase
        (
            VertexCollection oVertices,
            EdgeCollection oEdges,
            FacebookModelBase oModel,
            FacebookNetworkAnalyzerBase oNetworkAnalyzerBase
        )
        {
            m_oVertices = oVertices;
            m_oEdges = oEdges;
            m_oModel = oModel;
            m_oNetworkAnalyzerBase = oNetworkAnalyzerBase;
        }

        public void
        LoadNetwork
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            LoadVertices(ref oGraphMLXmlDocument);
            LoadEdges(ref oGraphMLXmlDocument);
        }

        private void
        LoadVertices
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            XmlNode oVertexXmlNode;

            foreach (Vertex oVertex in m_oVertices)
            {
                oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(oVertex.Name);
                LoadVertexAttributes(oVertex, oVertexXmlNode, ref oGraphMLXmlDocument);
            }
        }

        private void
        LoadEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            XmlNode oEdgeXmlNode;

            foreach (Edge oEdge in m_oEdges)
            {
                oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(oEdge.Vertex1.Name,
                                                                    oEdge.Vertex2.Name);
                LoadEdgeAttributes(oEdge, oEdgeXmlNode, ref oGraphMLXmlDocument);
            }
        }

        private void
        LoadVertexAttributes
        (
            Vertex oVertex,
            XmlNode oVertexXmlNode,
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            foreach (var oAttribute in oVertex.Attributes.Where(x => x.Value != null))
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, oAttribute.Key.value, oAttribute.Value);
            }
            LoadVertexCustomMenu(oVertex, oVertexXmlNode, ref oGraphMLXmlDocument);
            LoadVertexImageAttribute(oVertex, oVertexXmlNode, ref oGraphMLXmlDocument);
        }

        protected void
        LoadVertexCustomMenu
        (
            Vertex oVertex,
            XmlNode oVertexXmlNode,
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            if (oVertex.Type == VertexType.Post)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuTextID,
                                                                "Open Facebook Page for This Post");
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuActionID,
                    oVertex.Attributes["post_link"]);
            }
            else if (oVertex.Type == VertexType.User)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuTextID,
                                                                "Open Facebook Page for This User");
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuActionID,
                    "https://www.facebook.com/" + oVertex.ID);
            }
        }

        protected void
        LoadVertexImageAttribute
        (
            Vertex oVertex,
            XmlNode oVertexXmlNode,
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            if (oVertex.Type == VertexType.User)
            {
                if (!String.IsNullOrEmpty(oVertex.Attributes["picture"]))
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", oVertex.Attributes["picture"]);
                }
            }
            else if (oVertex.Type == VertexType.Post)
            {
                if (!String.IsNullOrEmpty(oVertex.Attributes["image"]))
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", oVertex.Attributes["image"]);
                }
            }
        }

        private void
        LoadEdgeAttributes
        (
            Edge oEdge,
            XmlNode oEdgeXmlNode,
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            foreach (var oAttribute in oEdge.Attributes.Where(x => x.Value != null))
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, oAttribute.Key.value, oAttribute.Value);
            }
        }        
    }
}
