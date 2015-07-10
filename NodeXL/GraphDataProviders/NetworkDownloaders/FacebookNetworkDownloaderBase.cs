using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using System.Threading;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookNetworkDownloaderBase
    {
        protected string m_sAccessToken; //TODO:can be deleted
        protected FacebookModelBase m_oModel;
        protected FacebookAPI m_oFacebookAPI;
        protected FacebookNetworkAnalyzerBase m_oNetworkAnalyzerBase;
        protected int m_iCurrentStep;
        protected int m_iNrOfSteps;

        protected System.Timers.Timer oTimer = new System.Timers.Timer();
        protected int iSecondsToWait = 600;
        protected string sTimerProgress;

        public FacebookNetworkDownloaderBase
        (
            string sAccessToken,
            FacebookModelBase oModel,
            FacebookNetworkAnalyzerBase oNetworkAnalyzerBase,
            int iNrOfSteps
        )
        {
            m_sAccessToken = sAccessToken;
            m_oModel = oModel;
            m_oFacebookAPI = new FacebookAPI(sAccessToken);
            m_oNetworkAnalyzerBase = oNetworkAnalyzerBase;
            m_iNrOfSteps = iNrOfSteps;
        }        

        protected List<JSONObject> DownloadPostsBetweenDates
        (
            String sQuery
        )
        {
            Boolean bHasData = true;
            List<JSONObject> oResults = new List<JSONObject>();
            String sFullQuery = String.Format("{0}&since={1}&until={2}",
                                                sQuery,
                                                DateUtil.ConvertToTimestamp(m_oModel.FromDate),
                                                DateUtil.ConvertToTimestamp(m_oModel.ToDate));
            List<JSONObject> tempResults = new List<JSONObject>();
            Int32 i = 1;

            while (bHasData)
            {
                String sProgress = String.Format("Downloading Posts (Page {0})", i++);
                JSONObject oResult = GraphAPIWithRetry(sFullQuery, sProgress);


                //Make sure we store results inside the selected date range
                tempResults = oResult.Dictionary["data"].Array.Where
                    (x => DateTime.Parse(x.Dictionary["created_time"].String) >= m_oModel.FromDate)
                    .ToList();
                oResults.AddRange(tempResults);
                if (tempResults.Count != oResult.Dictionary["data"].Array.Length)
                {
                    bHasData = false;
                }

                if (HasNext(oResult))
                {
                    sFullQuery = oResult.Dictionary["paging"].Dictionary["next"].String;
                }
                else
                {
                    bHasData = false;
                }
            }

            return oResults;
        }

        protected List<JSONObject> DownloadFromPostToPost
        (
            String sQuery
        )
        {
            Boolean bHasData = true;
            List<JSONObject> oResults = new List<JSONObject>();
            List<JSONObject> oOverallResults = new List<JSONObject>();
            Int32 i = 1;
            Int32 iTotalNumberOfResults = 0;
            Int32 iNrOfPosts = m_oModel.ToPost - m_oModel.FromPost + 1;

            while (bHasData)
            {
                String sProgress = String.Format("Downloading Posts (Page {0})", i++);
                JSONObject oResult = GraphAPIWithRetry(sQuery, sProgress);
                iTotalNumberOfResults += oResult.Dictionary["data"].Array.Length;

                oOverallResults.AddRange(oResult.Dictionary["data"].Array);

                if (HasNext(oResult))
                {
                    sQuery = oResult.Dictionary["paging"].Dictionary["next"].String;
                }
                else
                {
                    bHasData = false;
                    //In case there are less than iNrOfPosts results
                    if (m_oModel.FromPost <= iTotalNumberOfResults)
                    {
                        oResults = oOverallResults.Skip(m_oModel.FromPost - 1).ToList();
                    }
                }

                if (m_oModel.FromPost <= iTotalNumberOfResults &&
                    m_oModel.ToPost <= iTotalNumberOfResults)
                {
                    bHasData = false;
                    oResults = oOverallResults.Skip(m_oModel.FromPost - 1).Take(iNrOfPosts).ToList();
                }
            }

            return oResults;
        }

        protected List<JSONObject>
        DownloadPostLikes
        (
            List<JSONObject> streamPosts
        )
        {
             String sQuery = "/{0}/likes";
             String sProgress = "Step {0}/{1}: Downloading Likes - Post {2}/{3}(Page {4})";

             return ( DownloadLikes(streamPosts.Select(x => x.Dictionary["id"].String).ToList(),
                                    sQuery, sProgress) );                                                   
        }

        protected List<JSONObject>
        DownloadCommentLikes
        (
            List<JSONObject> comments
        )
        {
            String sQuery = "/{0}/likes";
            String sProgress = "Step {0}/{1}: Downloading Likes - Comment {2}/{3}(Page {4})";

            return (DownloadLikes(comments.Select(x => x.Dictionary["id"].String).ToList(),
                                   sQuery, sProgress));
        }

        protected List<JSONObject>
        DownloadPostComments
        (
            List<JSONObject> streamPosts
        )
        {            
            String sQuery = "/{0}/comments?filter=stream&fields=id, created_time, from, message, like_count";               
            String sProgress = "Step {0}/{1}: Downloading Comments - Post {2}/{3}(Page {4})";

            return (DownloadComments(streamPosts.Select(x => x.Dictionary["id"].String).ToList(),
                                   sQuery, sProgress));
        }

        protected List<JSONObject>
        DownloadCommentComments
        (
            List<JSONObject> comments
        )
        {
            String sQuery = "/{0}/comments?filter=stream&fields=id, created_time, from, message, like_count";
            String sProgress = "Step {0}/{1}: Downloading Comments - Comment {2}/{3}(Page {4})";

            return (DownloadComments(comments.Select(x => x.Dictionary["id"].String).ToList(),
                                   sQuery, sProgress));
        }

        //*************************************************************************
        //  Method: DownloadAttributes()
        //
        /// <summary>
        /// Gets the selected attributes for all the friends
        /// </summary>
        ///
        /// <param name="attributes">
        /// The dictionary that holds the attributes and states
        /// for each of them if they are included
        /// </param>
        /// 
        /// <param name="friendUIDs">
        /// A list with friend's UIDs
        /// </param> 
        ///
        /// <param name="fb">
        /// A FacebookAPI instance to make calls to Facebook
        /// </param>
        ///    
        /// <returns>
        /// A dictionary with the attributes' values
        /// </returns>
        //*************************************************************************       

        protected Dictionary<String, Dictionary<String, JSONObject>>
        DownloadAttributes
        (
            List<String> oUsers,
            List<String> oFanPages
        )
        {
            m_iCurrentStep++;
            

            Dictionary<String, Dictionary<String, JSONObject>> oUserAttribtues = DownloadUserAttributes(oUsers);

            Dictionary<String, Dictionary<String, JSONObject>> oFanPageAttributes = DownloadFanPageAttributes(oFanPages);

            return (oUserAttribtues.Union(oFanPageAttributes)
                        .ToDictionary(x => x.Key, x => x.Value));

        }

        protected void
        GetUsersAndFanPages
        (
            List<String> oAllEntities,
            out List<String> oUsers,
            out List<String> oFanPages
        )
        {
            oUsers = new List<String>();
            oFanPages = new List<String>();

            Int32 iMaxNrOfEntitiesPerCall = 50;
            Int32 iTotalNrOfEntities = oAllEntities.Count;
            Int32 iTotalNrOfCalls = (Int32)Math.Ceiling(iTotalNrOfEntities / (Double)iMaxNrOfEntitiesPerCall);

            for (Int32 i = 0; i < iTotalNrOfCalls; i++)
            {
                Int32 iSkip = i * iMaxNrOfEntitiesPerCall;
                String sIDs = String.Join(",", oAllEntities.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall).ToArray());
                String sQuery = String.Format("/?ids={0}&metadata=1&fields=id",
                                                sIDs);

                JSONObject result = GraphAPIWithRetry(sQuery, String.Empty);

                if (result == null)
                {
                    //It means that something went wrong
                    //Try getting results one-by-one and discard the problematic ones
                    foreach (String sID in oAllEntities.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall))
                    {
                        sQuery = String.Format("/{0}?metadata=1&fields=id",
                                                sID);

                        result = GraphAPIWithRetry(sQuery, String.Empty);
                        if (result != null)
                        {
                            if (result.Dictionary["metadata"].Dictionary["type"].String == "user")
                            {
                                oUsers.Add(result.Dictionary["id"].String);
                                //oUsers.AddRange(result.Dictionary.Where(x => x.Key == "metadata" && x.Value.Dictionary["type"].String == "user")
                                //                .Select(x => x.Value.Dictionary["id"].String).ToList());
                            }
                            else if (result.Dictionary["metadata"].Dictionary["type"].String == "page")
                            {
                                oFanPages.Add(result.Dictionary["id"].String);
                                //oFanPages.AddRange(result.Dictionary.Where(x => x.Key=="metadata" && x.Value.Dictionary["type"].String == "page")
                                //                            .Select(x => x.Value.Dictionary["id"].String).ToList());
                            }
                        }
                    }
                }
                else
                {
                    oUsers.AddRange(result.Dictionary.Where(x => x.Value.Dictionary["metadata"].Dictionary["type"].String == "user")
                                                .Select(x => x.Value.Dictionary["id"].String).ToList());

                    oFanPages.AddRange(result.Dictionary.Where(x => x.Value.Dictionary["metadata"].Dictionary["type"].String == "page")
                                                .Select(x => x.Value.Dictionary["id"].String).ToList());
                }
            }

        }

        protected Dictionary<String, Dictionary<String, JSONObject>>
        DownloadUserAttributes
        (
            List<String> oUsers
        )
        {

            Int32 iMaxNrOfEntitiesPerCall = 50;
            Int32 iTotalNrOfEntities = oUsers.Count;
            Int32 iTotalNrOfCalls = (Int32)Math.Ceiling(iTotalNrOfEntities / (Double)iMaxNrOfEntitiesPerCall);

            String sFields = String.Join(",", m_oModel.Attributes.Where(x => x.Value)
                                                .Select(x => x.Key.value).ToArray());

            Dictionary<String, Dictionary<String, JSONObject>> oAttributes = new Dictionary<String, Dictionary<String, JSONObject>>();

            for (Int32 i = 0; i < iTotalNrOfCalls; i++)
            {
                Int32 iSkip = i * iMaxNrOfEntitiesPerCall;
                String sIDs = String.Join(",", oUsers.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall).ToArray());
                String sQuery = String.Format("/?ids={0}&fields={1}",
                                                sIDs, sFields);
                String sProgress = String.Format("Downloading Attributes (Batch {0}/{1})", i + 1, iTotalNrOfCalls);

                JSONObject result = GraphAPIWithRetry(sQuery, sProgress);

                if (result == null)
                {
                    //Something went wrong
                    //Download attributes one at a time
                    foreach (String sID in oUsers.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall).ToArray())
                    {
                        sQuery = String.Format("/{0}&fields={1}",
                                                sID, sFields);
                        result = GraphAPIWithRetry(sQuery, sProgress);
                        if (result != null)
                        {
                            oAttributes = oAttributes.Union(result.Dictionary.ToDictionary(x => x.Key, x => x.Value.Dictionary))
                                .ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                }
                else
                {
                    oAttributes = oAttributes.Union(result.Dictionary.ToDictionary(x => x.Key, x => x.Value.Dictionary))
                                    .ToDictionary(x => x.Key, x => x.Value);
                }
            }

            return oAttributes;
        }

        protected Dictionary<String, Dictionary<String, JSONObject>>
        DownloadFanPageAttributes
        (
            List<String> oFanPages
        )
        {
            Int32 iMaxNrOfEntitiesPerCall = 50;
            Int32 iTotalNrOfEntities = oFanPages.Count;
            Int32 iTotalNrOfCalls = (Int32)Math.Ceiling(iTotalNrOfEntities / (Double)iMaxNrOfEntitiesPerCall);

            String sFields = "id, name, picture";

            Dictionary<String, Dictionary<String, JSONObject>> oAttributes = new Dictionary<String, Dictionary<String, JSONObject>>();

            for (Int32 i = 0; i < iTotalNrOfCalls; i++)
            {
                Int32 iSkip = i * iMaxNrOfEntitiesPerCall;
                String sIDs = String.Join(",", oFanPages.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall).ToArray());
                String sQuery = String.Format("/?ids={0}&fields={1}",
                                                sIDs, sFields);
                String sProgress = String.Format("Downloading Attributes (Batch {0}/{1})", i + 1, iTotalNrOfCalls);

                JSONObject result = GraphAPIWithRetry(sQuery, sProgress);

                oAttributes = oAttributes.Union(result.Dictionary.ToDictionary(x => x.Key, x => x.Value.Dictionary))
                                .ToDictionary(x => x.Key, x => x.Value);
            }

            return oAttributes;
        }

        protected Dictionary<String, String> GetStatusUpdates
        (
            List<string> userUIDs
        )
        {
            Int32 iMaxNrOfEntitiesPerCall = 50;
            Int32 iTotalNrOfEntities = userUIDs.Count;
            Int32 iTotalNrOfCalls = (Int32)Math.Ceiling(iTotalNrOfEntities / (Double)iMaxNrOfEntitiesPerCall);

            Dictionary<String, String> oStatusUpdates = new Dictionary<String, String>();

            for (Int32 i = 0; i < iTotalNrOfCalls; i++)
            {
                Int32 iSkip = i * iMaxNrOfEntitiesPerCall;
                String sIDs = String.Join(",", userUIDs.Skip(iSkip).Take(iMaxNrOfEntitiesPerCall).ToArray());
                String sQuery = String.Format("/statuses?ids={0}&limit=1&fields=message,from", sIDs);
                String sProgress = String.Format("Downloading Status Updates (Batch {0}/{1})", i + 1, iTotalNrOfCalls);

                JSONObject result = GraphAPIWithRetry(sQuery, sProgress);



                oStatusUpdates = oStatusUpdates.Union(result.Dictionary.Where(x => x.Value.Dictionary["data"].Array.Length > 0 &&
                                                                                x.Value.Dictionary["data"].Array[0].Dictionary.ContainsKey("message"))
                                                                        .ToDictionary(x => x.Value.Dictionary["data"]
                                                                        .Array[0].Dictionary["from"].Dictionary["id"].String,
                                                                        x => x.Value.Dictionary["data"].Array[0].Dictionary["message"].String))
                                                .ToDictionary(x => x.Key, x => x.Value);

            }

            return oStatusUpdates;
        }

        protected JSONObject
        GraphAPIWithRetry
        (
            string sQuery,
            string sProgress
        )
        {
            JSONObject result = null;
            bool retry = true;
            int nrOfRetries = 0;
            string retrying = "";
            //Retry until a max number of retries is reached,
            //in case we get an internal server error
            while (retry && nrOfRetries < 10)
            {
                nrOfRetries++;
                if (!String.IsNullOrEmpty(sProgress))
                {
                    this.ReportProgress(sProgress + retrying);
                }
                try
                {
                    result = m_oFacebookAPI.Get(sQuery);
                    retry = false;
                    retrying = "";
                }
                catch (FacebookAPIException e)
                {
                    retrying = " - Retrying";
                    if (!(e.Message.IndexOf("The remote server returned an error: (500) Internal Server Error.", StringComparison.OrdinalIgnoreCase) >= 0) &&
                        !(e.Message.IndexOf("Service temporarily unavailable", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        //TODO: If we catch a call limit exception we should wait 600s Calls to stream have exceeded the rate of 600 calls per 600 seconds.
                        if (e.Message.IndexOf("Calls to stream have exceeded the rate of 600 calls per 600 seconds.", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            iSecondsToWait = 600;
                            oTimer.Interval = 1000;
                            oTimer.Enabled = true;
                            sTimerProgress = sProgress;
                            oTimer.Start();
                            Thread.Sleep(600 * 1000);
                            oTimer.Stop();
                        }
                        else
                        {
                            //retry = false;
                            //retrying = "";
                            //throw e;
                        }
                    }
                }
            }
            return result;
        }

        protected Boolean
        HasNext
        (
            JSONObject result
        )
        {
            return (result.Dictionary.ContainsKey("paging") &&
                     result.Dictionary["paging"].Dictionary.ContainsKey("next"));
        }

        protected void oTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            iSecondsToWait--;
            string sProgress = sTimerProgress;
            if (iSecondsToWait < 0)
            {
                //oTimer.Enabled = false;
                oTimer.Stop();
            }

            this.ReportProgress(sProgress + String.Format(" - Resuming in {0} seconds", iSecondsToWait));
        }

        protected void
        ReportProgress
        (
            String sProgress
        )
        {
            if (m_oNetworkAnalyzerBase.GetType() == typeof(FacebookUserNetworkAnalyzer))
            {
                ((FacebookUserNetworkAnalyzer)m_oNetworkAnalyzerBase).ReportProgress(sProgress);
            }
            else if (m_oNetworkAnalyzerBase.GetType() == typeof(FacebookFanPageNetworkAnalyzer))
            {
                ((FacebookFanPageNetworkAnalyzer)m_oNetworkAnalyzerBase).ReportProgress(sProgress);
            }
            else if (m_oNetworkAnalyzerBase.GetType() == typeof(FacebookGroupNetworkAnalyzer))
            {
                ((FacebookGroupNetworkAnalyzer)m_oNetworkAnalyzerBase).ReportProgress(sProgress);
            }
            else
            {
                throw new Exception("Unhandled type");
            }
        }

        private List<JSONObject> 
        DownloadLikes
        (
            List<String> objectIDs,
            String sQuery,
            String sProgress
        )
        {
            Debug.Assert(m_oFacebookAPI != null);
            m_oNetworkAnalyzerBase.AssertValid();

            JSONObject oResult = null;
            List<JSONObject> likes = new List<JSONObject>();

            m_iCurrentStep++;

            for (int i = 0; i < objectIDs.Count; i++)
            {
                Boolean bHasData = true;
                Int32 j = 1;
                Int32 iTotalNumberOfResults = 0;
                String sLocalQuery = String.Format(sQuery, objectIDs[i]);

                List<JSONObject> oOverallResults = new List<JSONObject>();

                while (bHasData)
                {
                    List<JSONObject> oLocalResults = new List<JSONObject>();
                    String sLocalProgress = String.Format(sProgress,
                                                    m_iCurrentStep, m_iNrOfSteps, i + 1, objectIDs.Count, j++);
                    oResult = GraphAPIWithRetry(sLocalQuery, sLocalProgress);
                    iTotalNumberOfResults += oResult.Dictionary["data"].Array.Length;

                    oLocalResults.AddRange(oResult.Dictionary["data"].Array);

                    //Add the liked object_id to the dictionary
                    oLocalResults.ForEach(x => x.Dictionary.Add("object_id",
                                            JSONObject.CreateFromString("\"" + objectIDs[i] + "\"")));

                    oOverallResults.AddRange(oLocalResults);

                    if (HasNext(oResult))
                    {
                        sLocalQuery = oResult.Dictionary["paging"].Dictionary["next"].String;
                    }
                    else
                    {
                        bHasData = false;
                    }

                    if (m_oModel.Limit && m_oModel.LimitAmount <= iTotalNumberOfResults)
                    {
                        bHasData = false;
                        oOverallResults = oOverallResults.Take(m_oModel.LimitAmount).ToList();
                    }
                }

                likes.AddRange(oOverallResults);
            }

            return likes;
        }

        private List<JSONObject>//Dictionary<string, Dictionary<string, List<string>>>
        DownloadComments
        (
            List<String> objectIDs,
            String sQuery,
            String sProgress
        )
        {
            Debug.Assert(m_oFacebookAPI != null);
            m_oNetworkAnalyzerBase.AssertValid();

            JSONObject oResult = null;
            List<JSONObject> comments = new List<JSONObject>();            

            m_iCurrentStep++;

            for (int i = 0; i < objectIDs.Count; i++)
            {
                Boolean bHasData = true;
                Int32 j = 1;
                Int32 iTotalNumberOfResults = 0;
                String sLocalQuery = String.Format(sQuery, objectIDs[i]);

                List<JSONObject> oOverallResults = new List<JSONObject>();

                while (bHasData)
                {
                    List<JSONObject> oLocalResults = new List<JSONObject>();
                    String sLocalProgress = String.Format(sProgress,
                                                    m_iCurrentStep, m_iNrOfSteps, i + 1, objectIDs.Count, j++);
                    oResult = GraphAPIWithRetry(sLocalQuery, sLocalProgress);
                    iTotalNumberOfResults += oResult.Dictionary["data"].Array.Length;

                    oLocalResults.AddRange(oResult.Dictionary["data"].Array);

                    //Add the commented object_id to the dictionary
                    oLocalResults.ForEach(x => x.Dictionary.Add("object_id",
                                            JSONObject.CreateFromString("\"" + objectIDs[i] + "\"")));

                    oOverallResults.AddRange(oLocalResults);

                    if (HasNext(oResult))
                    {
                        sLocalQuery = oResult.Dictionary["paging"].Dictionary["next"].String;
                    }
                    else
                    {
                        bHasData = false;
                    }

                    if (m_oModel.Limit && m_oModel.LimitAmount <= iTotalNumberOfResults)
                    {
                        bHasData = false;
                        oOverallResults = oOverallResults.Take(m_oModel.LimitAmount).ToList();
                    }
                }

                comments.AddRange(oOverallResults);
            }

            return comments;
        }

    }
}
