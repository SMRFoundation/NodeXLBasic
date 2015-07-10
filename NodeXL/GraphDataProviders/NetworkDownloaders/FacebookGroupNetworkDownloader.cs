using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using System.Diagnostics;
using Smrf.AppLib;
using System.Threading;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookGroupNetworkDownloader : FacebookNetworkDownloaderBase
    {
        public FacebookGroupNetworkDownloader
        (            
            string sAccessToken,
            FacebookGroupModel oModel,
            FacebookGroupNetworkAnalyzer oGroupNetworkAnalyzer,
            int iNrOfSteps
        )
        :
        base
        (
            sAccessToken,
            oModel,
            oGroupNetworkAnalyzer,
            iNrOfSteps
        )
        {
            
        }

        private FacebookGroupModel Model
        {
            get { return (FacebookGroupModel)m_oModel; }
        }

        public void
        DownloadNetwork
        (
            out List<JSONObject> oPosts,
            out List<JSONObject> oLikes,
            out List<JSONObject> oComments,
            out List<JSONObject> oCommentLikes,
            out List<JSONObject> oCommentComments,
            out List<JSONObject> oShares,
            out Dictionary<String, Dictionary<String, JSONObject>> oAttributes,
            out Dictionary<String, String> oStatusUpdates            
        )
        {
            oPosts = DownloadPosts();
            oLikes = new List<JSONObject>();
            oComments = new List<JSONObject>();
            oCommentLikes = new List<JSONObject>();
            oCommentComments = new List<JSONObject>();
            oShares = new List<JSONObject>();
            oAttributes = new Dictionary<String, Dictionary<String, JSONObject>>();
            oStatusUpdates = new Dictionary<String, String>();            

            oTimer.Elapsed += new System.Timers.ElapsedEventHandler(oTimer_Elapsed);

            if (Model.Like)
            {
                oLikes = DownloadPostLikes(oPosts);
            }
            if (Model.Comment)
            {
                oComments = DownloadPostComments(oPosts);
            }

            if (Model.RelationshipCommentAuthor)
            {
                if (Model.Like)
                {
                    oCommentLikes = DownloadCommentLikes(oComments);
                }
                if (Model.Comment)
                {
                    oCommentComments = DownloadCommentComments(oComments);
                }
            }

            if (Model.Share)
            {                
                oShares = DownloadShares(oPosts);
            }

            if (Model.User || Model.Post)
            {
                List<string> oUniqueUsers = GetUniqueUsers(oPosts, oLikes, oComments,
                                                    oShares, oCommentLikes, oCommentComments);
                oAttributes = DownloadUserAttributes(oUniqueUsers);
                
                if (Model.GetStatusUpdates)
                {
                    oStatusUpdates = GetStatusUpdates(oUniqueUsers);
                }
            }
        }

        private List<JSONObject>
        DownloadPosts
        (   
         
        )                                                                                                                                       
        {
            Debug.Assert(m_oFacebookAPI != null);


            string sFirstPart = String.Format("/{0}/feed?fields={1}",
                                Model.GroupID,
                                "from, to, message, id, created_time, shares, picture, " +
                                "likes.limit(1).summary(true), comments.limit(1).filter(stream).summary(true)");

            List<JSONObject> oResults = new List<JSONObject>();
            if (m_oModel.DownloadFromPost)
            {
                oResults = DownloadFromPostToPost(sFirstPart);
            }
            else if (m_oModel.DownloadPostsBetweenDates)
            {
                oResults = DownloadPostsBetweenDates(sFirstPart);
            }

            return oResults;
        }      

        private List<JSONObject>
        DownloadShares
        (
            List<JSONObject> streamPosts
        )
        {
            List<JSONObject> oShares = new List<JSONObject>();

            for (int i = 0; i < streamPosts.Count; i++)
            {
                JSONObject result = m_oFacebookAPI.Get(streamPosts[i].Dictionary["id"].String + "/sharedposts");
                if (result.Dictionary["data"].Dictionary != null)
                {
                    oShares.Add(result);
                }
            }
            

            return oShares;
        }

        private List<string>
        GetUniqueUsers
        (
            List<JSONObject> oPosts,
            List<JSONObject> oLikes,
            List<JSONObject> oComments,
            List<JSONObject> oShares,
            List<JSONObject> oCommentLikes,
            List<JSONObject> oCommentComments
        )
        {
            List<String> oLikers = oLikes.Select(x => x.Dictionary["id"].String).Distinct().ToList();
            List<String> oCommenters = oComments.Select(x => x.Dictionary["from"].Dictionary["id"].String).Distinct().ToList();
            List<String> oAuthors = oPosts.Select(x => x.Dictionary["from"].Dictionary["id"].String).Distinct().ToList();
            List<String> oCommentLikers = oCommentLikes.Select(x => x.Dictionary["id"].String).ToList();
            List<String> oCommentCommenters = oCommentComments.Select(x => x.Dictionary["from"].Dictionary["id"].String).ToList();
            //TODO: Add sharers

            return oLikers.Union(oCommenters).Union(oAuthors).Union(oCommentLikers).Union(oCommentCommenters).Distinct().ToList();
        }        
    }
    
}