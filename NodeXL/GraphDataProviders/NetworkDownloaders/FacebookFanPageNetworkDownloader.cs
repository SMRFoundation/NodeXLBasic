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
    public class FacebookFanPageNetworkDownloader : FacebookNetworkDownloaderBase
    {
        public FacebookFanPageNetworkDownloader
        (            
            string sAccessToken,
            FacebookFanPageModel oModel,
            FacebookFanPageNetworkAnalyzer oFanPageNetworkAnalyzer,
            int iNrOfSteps
        )
        :
        base 
        (
            sAccessToken,
            oModel,
            oFanPageNetworkAnalyzer,
            iNrOfSteps
        )
        {            
            
        }

        private FacebookFanPageModel Model
        {
            get { return (FacebookFanPageModel)m_oModel; }
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
            oAttributes = new Dictionary<string, Dictionary<string, JSONObject>>();
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
                //oShares = DownloadShares(oPosts);
            }

            if (Model.User || Model.Post)
            {
                List<String> oUniqueUsers = GetUniqueUsers(oPosts, oLikes, oComments,
                                                           oShares, oCommentLikes, oCommentComments);

                List<String> oUsers, oFanPages;

                GetUsersAndFanPages(oUniqueUsers, out oUsers, out oFanPages);

                oAttributes = DownloadAttributes(oUsers, oFanPages);

                if (Model.GetStatusUpdates)
                {
                    oStatusUpdates = GetStatusUpdates(oUsers);
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
                                Model.FanPageID,
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
            JSONObject streamPosts
        )
        {
            List<JSONObject> oShares = new List<JSONObject>();
            if (streamPosts.IsArray)
            {
                for (int i = 0; i < streamPosts.Array.Length; i++)
                {
                    JSONObject result = m_oFacebookAPI.Get(streamPosts.Array[i].Dictionary["post_id"].String + "/sharedposts");
                    if (result.Dictionary["data"].Dictionary != null)
                    {
                        oShares.Add(result);
                    }
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