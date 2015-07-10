using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.XmlLib;
using Smrf.NodeXL.GraphMLLib;
using System.Xml;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookUserNetworkLoader : FacebookNetworkLoaderBase
    {        

        public FacebookUserNetworkLoader
        (
            VertexCollection oVertices,
            EdgeCollection oEdges,
            FacebookUserModel oModel,
            FacebookUserNetworkAnalyzer oUserNetworkAnalyzer
        )
        :
        base
        (
            oVertices,
            oEdges,
            oModel,
            oUserNetworkAnalyzer
        )
        {
            
        }

        protected new void
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
                    oVertex.Attributes["post_url"]);
            }
            else if (oVertex.Type == VertexType.User)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuTextID,
                                                                "Open Facebook Page for This User");
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, NodeXLGraphMLUtil.VertexMenuActionID,
                    "https://www.facebook.com/" + oVertex.ID);
            }
        }

        protected new void
        LoadVertexImageAttribute
        (
            Vertex oVertex,
            XmlNode oVertexXmlNode,
            ref GraphMLXmlDocument oGraphMLXmlDocument
        )
        {            
            if (!String.IsNullOrEmpty(oVertex.Attributes["picture"]))
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", oVertex.Attributes["picture"]);
            }            
        }       
        
    }    
}
