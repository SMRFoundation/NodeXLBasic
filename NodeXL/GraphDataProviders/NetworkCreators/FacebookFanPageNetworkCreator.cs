using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Smrf.AppLib;
using System.Text.RegularExpressions;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookFanPageNetworkCreator : FacebookFanPageGroupNetworkCreatorBase
    {
        public FacebookFanPageNetworkCreator
        (
            FacebookFanPageModel oModel,
            List<JSONObject> oPosts,
            List<JSONObject> oLikes,
            List<JSONObject> oComments,
            List<JSONObject> oCommentLikes,
            List<JSONObject> oCommentComments,
            List<JSONObject> oShares,
            Dictionary<string, Dictionary<string, JSONObject>> oAttributes,
            Dictionary<String, String> oStatusUpdates                       
        )
        :
        base
        (
            oModel,
            oPosts,
            oLikes,
            oComments,
            oCommentLikes,
            oCommentComments,
            oShares,
            oAttributes,
            oStatusUpdates
        )
        {            
                                  
        }
    }
}
