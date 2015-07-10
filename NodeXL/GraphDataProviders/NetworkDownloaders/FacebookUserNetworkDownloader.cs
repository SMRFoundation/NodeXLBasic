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
    public class FacebookUserNetworkDownloader : FacebookNetworkDownloaderBase
    {
        public FacebookUserNetworkDownloader            
        (            
            string sAccessToken,
            FacebookUserModel oModel,
            FacebookUserNetworkAnalyzer oUserNetworkAnalyzer,
            int iNrOfSteps
        )
            :
        base (
            sAccessToken,
            oModel,
            oUserNetworkAnalyzer,
            iNrOfSteps
            )
        {
           
        }

        private FacebookUserModel Model
        {
            get { return (FacebookUserModel)m_oModel; }
        }

        public void
        DownloadNetwork
        (
            out List<JSONObject> oPosts,
            out List<JSONObject> oLikes,
            out List<JSONObject> oComments,            
            out JSONObject oEgo,
            out Dictionary<String, Dictionary<String, JSONObject>> oAttributes,
            out Dictionary<String, String> oStatusUpdates            
        )
        {
            oTimer.Elapsed += new System.Timers.ElapsedEventHandler(oTimer_Elapsed);

            bool bGetPosts = (Model.PostAuthor || Model.UserTagged ||
                              Model.Comment || Model.Like);

            oEgo = null;
            oPosts = null;
            oLikes = oComments = null;
            oStatusUpdates = null;            
            oAttributes = null;

            oEgo = DownloadEgo();

            String oUserToQuery = oEgo.Dictionary["id"].String;            

            if (bGetPosts)
            {
                oPosts = DownloadPosts(oUserToQuery);
            }

            if(Model.Comment)
            {
                oComments = DownloadPostComments(oPosts);
            }

            if (Model.Like)
            {
                oLikes = DownloadPostLikes(oPosts);
            }

            List<String> oEntities = GetAllEntities(oEgo, oComments, oLikes, oPosts);

            List<String> oUsers, oFanPages;

            GetUsersAndFanPages(oEntities, out oUsers, out oFanPages);

            oAttributes = DownloadAttributes(oUsers, oFanPages);
            

            if (Model.GetTooltips)
            {
                oStatusUpdates = GetStatusUpdates(oUsers);
            }

            
        }

        private JSONObject
        DownloadEgo
        (
        )
        {
            return ( GraphAPIWithRetry("/me", String.Empty) );
        }

        private List<JSONObject>
        DownloadPosts
        (
            String sObjectToQuery
        )
        {
            string sUsersTagged = Model.UserTagged ? ", message_tags, with_tags" : "";
            string sFirstPart = String.Format("/{0}/feed?fields={1}",
                                sObjectToQuery,
                                "from, to, message, id, created_time, shares, picture" + sUsersTagged);

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

        private List<string>
        GetAllEntities
        (            
            JSONObject oEgo,
            List<JSONObject> oComments,
            List<JSONObject> oLikes,
            List<JSONObject> oPosts
        )
        {
            List<string> oAllUsers = new List<string>();
            

            if (oEgo != null)
            {
                oAllUsers.Add(oEgo.Dictionary["id"].String);
            }

            if (oComments != null)
            {
                oAllUsers.AddRange(oComments.Select(x => x.Dictionary["from"].Dictionary["id"].String));
            }

            if (oLikes != null)
            {
                oAllUsers.AddRange(oLikes.Select(x => x.Dictionary["id"].String));
            }

            if (oPosts != null)
            {
                oAllUsers.AddRange(oPosts.Select(x => x.Dictionary["from"].Dictionary["id"].String));
            }

            if (Model.UserTagged)
            {
                List<String> oUserTags = new List<String>();
                foreach (JSONObject post in oPosts)
                {
                    if (post.Dictionary.ContainsKey("message_tags"))
                    {
                        //message_tags is given as a dictionary
                        foreach (KeyValuePair<String, JSONObject> messageTag in post.Dictionary["message_tags"].Dictionary)
                        {
                            oUserTags.AddRange(messageTag.Value.Array.Select(x => x.Dictionary["id"].String));
                        }
                        
                    }
                    if (post.Dictionary.ContainsKey("with_tags"))
                    {
                        //with_tags is given as an array
                        oUserTags.AddRange(post.Dictionary["with_tags"].Dictionary["data"]
                                            .Array.Select(x => x.Dictionary["id"].String));
                    }
                }
                oAllUsers.AddRange(oUserTags);                
            }


            return oAllUsers.Distinct().ToList();
        }       
        
    }
    
}