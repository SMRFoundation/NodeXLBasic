using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.XmlLib;
using Smrf.NodeXL.GraphMLLib;
using System.Xml;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookFanPageNetworkLoader : FacebookNetworkLoaderBase
    {
        public FacebookFanPageNetworkLoader
        (
            VertexCollection oVertices,
            EdgeCollection oEdges,
            FacebookFanPageModel oModel,
            FacebookFanPageNetworkAnalyzer oFanPageNetworkAnalyzer
        )
        :
        base
        (
            oVertices,
            oEdges,
            oModel,
            oFanPageNetworkAnalyzer
        )
        {
            
        }
    }    
}
