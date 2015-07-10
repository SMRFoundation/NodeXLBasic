using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.XmlLib;
using Smrf.NodeXL.GraphMLLib;
using System.Xml;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookGroupNetworkLoader : FacebookNetworkLoaderBase
    {
        public FacebookGroupNetworkLoader
        (
            VertexCollection oVertices,
            EdgeCollection oEdges,
            FacebookGroupModel oModel,
            FacebookGroupNetworkAnalyzer oGroupNetworkAnalyzer
        )
        :
        base 
        (
            oVertices,
            oEdges,
            oModel,
            oGroupNetworkAnalyzer
        )
        {
            
        }        
    }    
}
