using System;
using System.Net;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.SocialNetworkLib;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.NodeXL.GraphMLLib;
using System.IO;
using System.Text;
using Facebook;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Threading;


namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: FacebookFanPageNetworkAnalyzer
    //
    /// <summary>
    /// Gets networks of Facebook fan page.
    /// </summary>
    ///
    /// <remarks>
    /// Use <see cref="GetNetworkAsync" /> to asynchronously get a undirected network
    /// of a Facebook fan page.
    /// </remarks>
    //*****************************************************************************

    public class FacebookFanPageNetworkAnalyzer : FacebookNetworkAnalyzerBase
    {
        //*************************************************************************
        //  Constructor: sdcsd()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="sdcsd" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookFanPageNetworkAnalyzer()
        {
            // (Do nothing.)

            AssertValid();
        }     

        

        private int NrOfSteps = 8;
        private int CurrentStep = 0;

        //*************************************************************************
        //  Method: GetNetworkAsync()
        //
        /// <summary>
        /// Asynchronously gets a directed network of Facebook friends.
        /// </summary>
        ///
        /// <param name="s_accessToken">
        /// The access_token needed for the authentication in Facebook API.
        /// </param>
        ///
        /// <param name="includeMe">
        /// Specifies whether the ego should be included in the network.
        /// </param>
        ///
        /// <remarks>
        /// When the analysis completes, the <see
        /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
        /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
        /// an XmlDocument containing the network as GraphML.
        ///
        /// <para>
        /// To cancel the analysis, call <see
        /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
        /// </para>
        ///
        /// </remarks>
        //*************************************************************************

        public void
        GetNetworkAsync
        (
            String s_accessToken,
            FacebookFanPageModel oModel
        )
        {            
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();
            
            const String MethodName = "GetNetworkAsync";
            CheckIsBusy(MethodName);

            GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

            oGetNetworkAsyncArgs.AccessToken = s_accessToken;
            oGetNetworkAsyncArgs.Model = oModel;            
            
            m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
        }

        //*************************************************************************
        //  Method: GetNetwork()
        //
        /// <summary>
        /// Synchronously gets a directed network of a Facebook Fan Page.
        /// </summary>
        ///
        /// <param name="s_accessToken">
        /// The access_token needed for the authentication in Facebook API.
        /// </param>
        ///
        /// <param name="includeMe">
        /// Specifies whether the ego should be included in the network.
        /// </param>
        ///
        /// <remarks>
        /// When the analysis completes, the <see
        /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
        /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
        /// an XmlDocument containing the network as GraphML.
        ///
        /// <para>
        /// To cancel the analysis, call <see
        /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
        /// </para>
        ///
        /// </remarks>
        //*************************************************************************

        public XmlDocument
        GetNetwork
        (
            String s_accessToken,
            FacebookFanPageModel oModel
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();

            const String MethodName = "GetNetwork";
            CheckIsBusy(MethodName);

            return (GetFanPageNetworkInternal(s_accessToken, oModel));
        }

        //*************************************************************************
        //  Method: GetFanPageNetworkInternal()
        //
        /// <summary>
        /// Gets different networks for a fan page.
        /// </summary>
        ///
        /// <param name="sAccessToken">
        /// The access_token needed to execute queries in Facebook.
        /// </param>
        ///
        /// <returns>
        /// An XmlDocument containing the network as GraphML.
        /// </returns>
        //*************************************************************************

        protected XmlDocument
        GetFanPageNetworkInternal
        (
            string sAccessToken,
            FacebookFanPageModel oModel
        )

        {
            List<JSONObject> streamPosts;            
            Dictionary<string, Dictionary<string, List<string>>> commentersComments = new Dictionary<string,Dictionary<string,List<string>>>();
            Dictionary<string, List<string>> likersPosts=new Dictionary<string,List<string>>();
            usersDisplayName = new Dictionary<string,string>();

            fbAPI = new FacebookAPI(sAccessToken);

            bGetStatusUpdates = oModel.GetStatusUpdates;
            bGetWallPosts = oModel.GetStatusUpdates;


            if (bGetStatusUpdates)
            {
                NrOfSteps++;
                CurrentStep++;
            }
            if (bGetWallPosts)
            {
                NrOfSteps++;
                CurrentStep++;
            }

            RequestStatistics oRequestStatistics = new RequestStatistics();

            //Create a network downloader
            FacebookFanPageNetworkDownloader oFanPageNetworkDownloader = new FacebookFanPageNetworkDownloader(sAccessToken, oModel, this, NrOfSteps);

            //Download data
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues;
            List<JSONObject> oLikes;
            List<JSONObject> oComments;
            List<JSONObject> oCommentLikes;
            List<JSONObject> oCommentComments;
            List<JSONObject> oShares;
            Dictionary<String, String> oStatusUpdates;            

            oFanPageNetworkDownloader.DownloadNetwork(out streamPosts, out oLikes, out oComments,
                                                        out oCommentLikes, out oCommentComments,
                                                        out oShares, out attributeValues,
                                                        out oStatusUpdates);

            //Add 5 steps to current step
            CurrentStep += 6;

            //Create a network creator
            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Creating Network...", CurrentStep, NrOfSteps));
            FacebookFanPageNetworkCreator oFanPageNetworkCreator = new FacebookFanPageNetworkCreator(oModel, streamPosts,
                                                                                                    oLikes, oComments,
                                                                                                    oCommentLikes, oCommentComments,
                                                                                                    oShares, attributeValues,
                                                                                                    oStatusUpdates);

            //Create network
            VertexCollection oVertices;
            EdgeCollection oEdges;

            oFanPageNetworkCreator.CreateNetwork(out oVertices, out oEdges);            

            //Create the network
            GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(oModel);

            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Loading Network...", CurrentStep, NrOfSteps));
            FacebookFanPageNetworkLoader oFanPageNetworkLoader = new FacebookFanPageNetworkLoader(oVertices, oEdges,
                                                                                                    oModel, this);

            oFanPageNetworkLoader.LoadNetwork(ref oGraphMLXmlDocument);

            String sGraphDescription = GetNetworkDescription(streamPosts, oModel, oGraphMLXmlDocument);

            OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics,
                "FacebookFanPage", String.Format("{0}({1})", m_sFanPage, oModel.FanPageID),
                sGraphDescription,
                m_sFanPage + " Facebook Fan Page",
                m_sFanPage + " Facebook Fan Page"
                );

            return oGraphMLXmlDocument;
        }

        //*************************************************************************
        //  Method: GetNetworkDescription()
        //
        /// <summary>
        /// Gets a description of the network.
        /// </summary>
        ///
        /// <param name="streamPosts">
        /// The downloaded posts.
        /// </param>
        /// 
        /// <param name="fanPageUsernameID">
        /// The specified fan page to analyze.
        /// </param>
        /// 
        /// <param name="netTypes">
        /// The network types to construct fo the fan page.
        /// </param>
        /// 
        /// <param name="nrOfPosts">
        /// Number of first wall posts to analyze.
        /// In case its value is equal to 0, 
        /// the fromDate is taken into consideration.
        /// </param>
        /// 
        /// <param name="fromDate">
        /// Analyze wall posts starting from this date.
        /// </param>
        /// 
        /// <param name="getWallPosts">
        /// Specifies if wall posts are downloaded.
        /// </param>
        /// 
        /// <param name="getStatusUpdates">
        /// Specifies if status updates are downloaded.
        /// </param>
        ///
        /// <param name="oGraphMLXmlDocument">
        /// The GraphMLXmlDocument that contains the network.
        /// </param>
        ///
        /// <returns>
        /// A description of the network.
        /// </returns>
        //*************************************************************************

        protected String
        GetNetworkDescription
        (
            List<JSONObject> streamPosts,
            FacebookFanPageModel oModel,
            GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            Debug.Assert(oGraphMLXmlDocument != null);
            AssertValid();

            NetworkDescriber oNetworkDescriber = new NetworkDescriber();

            JSONObject oFanPage = fbAPI.Get("/"+oModel.FanPageID);
            m_sFanPage = oFanPage.Dictionary["name"].String;

            oNetworkDescriber.AddSentence(
                "The graph represents the {0} network of the \"{1}\" ({2}) Facebook fan page.",
                ConcatenateNetworkTypes(oModel), m_sFanPage, oModel.FanPageID
                );

            oNetworkDescriber.AddNetworkTime(NetworkSource);

            if (oModel.DownloadFromPost)
            {
                oNetworkDescriber.AddSentence(
                    "Wall post from {0} to {1} of the fan page are analyzed.",
                    oModel.FromPost, oModel.ToPost
                    );
            }
            else
            {
                oNetworkDescriber.AddSentence(
                    "Wall posts between {0} and {1} of the fan page are analyzed.",
                    oModel.FromDate, oModel.ToDate
                    );
            }

            if (oModel.UserRelationshipSamePost)
            {
                oNetworkDescriber.AddSentence(String.Format(
                    "There is an edge between users that have {0} the same post(s)."
                    , GetRelationshipNamingsText("verb", oModel))
                    );
            }

            if (oModel.PostSameRelationship)
            {
                oNetworkDescriber.AddSentence(String.Format(
                    "There is an edge between posts that share the same {0}."
                    , GetRelationshipNamingsText("noun", oModel))
                    );
            }

            if (oModel.RelationshipPostAuthor)
            {
                oNetworkDescriber.AddSentence(String.Format(
                    "There is an edge between {0} and post author."
                    , GetRelationshipNamingsText("noun", oModel)
                    ));
            }

            if (oModel.ConsecutiveRelationship)
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between two consecutive commenters."
                    );
            }            

            if (bGetWallPosts)
            {
                oNetworkDescriber.AddSentence(
                    "For each user, 8000 characters of wall posts are downloaded."
                    );
            }           

            if (bGetStatusUpdates)
            {
                oNetworkDescriber.AddSentence(
                    "For each user, 8000 characters of status updates are downloaded."
                    );
            }

            if (bGetWallPosts || bGetStatusUpdates)
            {
                oNetworkDescriber.AddSentence(
                    "URLs, Hashtags and user tags are extracted from the downloaded"
                    + " wall posts/status updates."
                    );
            }

            AddPostDateRangeToNetworkDescription(streamPosts, oNetworkDescriber);

            return (oNetworkDescriber.ConcatenateSentences());
        }

        private void
        AddPostDateRangeToNetworkDescription
        (
            List<JSONObject> streamPosts,
            NetworkDescriber oNetworkDescriber
        )
        {
            DateTime minPostDate = DateTime.MaxValue;
            DateTime maxPostsDate = DateTime.MinValue;

            
            for (int i = 0; i < streamPosts.Count; i++)
            {
                DateTime tmp = DateTime.Parse(streamPosts[i].Dictionary["created_time"].String);                    

                if (tmp <= minPostDate)
                {
                    minPostDate = tmp;
                }
                    
                if(tmp >= maxPostsDate)
                {
                    maxPostsDate = tmp;
                }
            }            

            oNetworkDescriber.AddEventTime(
                "The earliest post in the network was posted ",
                minPostDate
                );

            oNetworkDescriber.AddEventTime(
                "The latest post in the network was posted ",
                maxPostsDate
                );
        }

        private string
        ConcatenateNetworkTypes
        (
            FacebookFanPageModel oModel
        )
        {
            List<string> oConcat = new List<string>();

            if (oModel.UserRelationshipSamePost)
            {
                oConcat.Add("UserRelationshipSamePost");
            }

            if (oModel.PostSameRelationship)
            {
                oConcat.Add("PostSameRelationship");
            }

            if (oModel.RelationshipPostAuthor)
            {
                oConcat.Add("RelationshipPostAuthor");
            }

            if (oModel.ConsecutiveRelationship)
            {
                oConcat.Add("ConsecutiveCommenter");
            }

            return String.Join(", ", oConcat.ToArray());
        }

        private List<RelationshipNaming>
        GetRelationshipNamings
        (
            FacebookFanPageModel oModel
        )
        {
            List<RelationshipNaming> oRelationshipNamings = new List<RelationshipNaming>();
            if (oModel.Like)
            {
                oRelationshipNamings.Add(new RelationshipNaming
                {
                    Relationship = "Like",
                    Noun = "liker",
                    Verb = "liked"
                });
            }
            if (oModel.Comment)
            {
                oRelationshipNamings.Add(new RelationshipNaming
                {
                    Relationship = "Comment",
                    Noun = "commenter",
                    Verb = "commented in"
                });
            }
            if (oModel.Share)
            {
                oRelationshipNamings.Add(new RelationshipNaming
                {
                    Relationship = "Share",
                    Noun = "sharer",
                    Verb = "shared"
                });
            }

            return oRelationshipNamings;

        }

        private string
        GetRelationshipNamingsText
        (
            string sWhat,
            FacebookFanPageModel oModel
        )
        {
            string sRelationship;
            List<RelationshipNaming> oRelationshipNamings = GetRelationshipNamings(oModel);

            if (oRelationshipNamings.Count == 0)
            {
                return "{0}";
            }

            switch (sWhat)
            {
                case "noun":
                    sRelationship = String.Join("/", oRelationshipNamings.Select(x => x.Noun).ToArray());
                    break;
                case "verb":
                    sRelationship = String.Join("/", oRelationshipNamings.Select(x => x.Verb).ToArray());
                    break;
                default:
                    sRelationship = String.Join("/", oRelationshipNamings.Select(x => x.Verb).ToArray());
                    break;
            }

            return sRelationship;
        }

        //*************************************************************************
        //  Method: CreateGraphMLXmlDocument()
        //
        /// <summary>
        /// Creates a GraphMLXmlDocument representing a network of friends in Facebook.
        /// </summary>
        ///        
        /// <returns>
        /// A GraphMLXmlDocument representing a network of a Facebook fan page.
        /// The document includes GraphML-attribute definitions but no vertices or
        /// edges.
        /// </returns>
        //*************************************************************************

        protected GraphMLXmlDocument
        CreateGraphMLXmlDocument
        (
            FacebookFanPageModel oModel                    
        )
        {
            AssertValid();

            

            GraphMLXmlDocument oGraphMLXmlDocument;

            
            oGraphMLXmlDocument = new GraphMLXmlDocument(true);
            

            NodeXLGraphMLUtil.DefineEdgeRelationshipGraphMLAttribute(oGraphMLXmlDocument);            
            NodeXLGraphMLUtil.DefineVertexCustomMenuGraphMLAttributes(oGraphMLXmlDocument);         
            NodeXLGraphMLUtil.DefineVertexImageFileGraphMLAttribute(oGraphMLXmlDocument);

            
            List<AttributeUtils.Attribute> oVertexAttributes = SetVertexAttributes(oModel);

            foreach (AttributeUtils.Attribute oAttribute in oVertexAttributes)
            {
                if ((oModel.Attributes.ContainsKey(oAttribute.value) &&
                    oModel.Attributes[oAttribute.value]) ||
                    !oModel.Attributes.ContainsKey(oAttribute.value))
                {
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, oAttribute.value,
                    oAttribute.name, "string", null);
                }
            }                

            List<AttributeUtils.Attribute> oEdgeAttributes = SetEdgeAttributes(oModel);

            foreach (AttributeUtils.Attribute oAttribute in oEdgeAttributes)
            {                
                oGraphMLXmlDocument.DefineGraphMLAttribute(true, oAttribute.value,
                oAttribute.name, "string", null);                
            }

            return (oGraphMLXmlDocument);
        }


        private List<AttributeUtils.Attribute>
        SetEdgeAttributes
        (
            FacebookFanPageModel oModel
        )
        {
            List<AttributeUtils.Attribute> oEdgeAttributes = new List<AttributeUtils.Attribute>();

            if (oModel.Post)
            {
                oEdgeAttributes.AddRange(AttributeUtils.FanPageUserCreatedPost);
            }

            if (oModel.Like)
            {
                if (oModel.UserRelationshipSamePost)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserLikedSamePost);
                }

                if (oModel.PostSameRelationship)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPagePostHaveSameLiker);
                }

                if (oModel.RelationshipPostAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageLikerPostAuthor);
                }

                if (oModel.RelationshipCommentAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserLikerComment);
                }

            }

            if (oModel.Comment)
            {
                if (oModel.UserRelationshipSamePost)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserCommentedSamePost);
                }

                if (oModel.PostSameRelationship)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPagePostHaveSameCommenter);
                }

                if (oModel.RelationshipPostAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageCommenterPostAuthor);
                }

                if (oModel.ConsecutiveRelationship)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageConsecutiveCommenter);
                }

                if (oModel.RelationshipCommentAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserCommenterComment);
                }
            }

            if (oModel.Share)
            {
                if (oModel.UserRelationshipSamePost)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserSharedSamePost);
                }

                if (oModel.PostSameRelationship)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPagePostHaveSameSharer);
                }

                if (oModel.RelationshipPostAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageSharerPostAuthor);
                }

                if (oModel.RelationshipCommentAuthor)
                {
                    oEdgeAttributes.AddRange(AttributeUtils.FanPageUserSharerComment);
                }
            }

            return oEdgeAttributes.Distinct().ToList();
        }

        private List<AttributeUtils.Attribute>
        SetVertexAttributes
        (
            FacebookFanPageModel oModel
        )
        {
            List<AttributeUtils.Attribute> oVertexAttributes = new List<AttributeUtils.Attribute>();
            oVertexAttributes.AddRange(AttributeUtils.FanPageUserAttributes);

            if (oModel.User)
            {                
                if (oModel.GetStatusUpdates)
                {
                    oVertexAttributes.AddRange(AttributeUtils.StatusUpdatesUserAttributes);
                }                
            }

            if (oModel.Post)
            {

                oVertexAttributes.AddRange(AttributeUtils.PostAttributes);
            }

            return oVertexAttributes.Distinct().ToList();
        }

                

        //*************************************************************************
        //  Method: BackgroundWorker_DoWork()
        //
        /// <summary>
        /// Handles the DoWork event on the BackgroundWorker object.
        /// </summary>
        ///
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        ///
        /// <param name="e">
        /// Standard mouse event arguments.
        /// </param>
        //*************************************************************************

        protected override void
        BackgroundWorker_DoWork
        (
            object sender,
            DoWorkEventArgs e
        )
        {

            Debug.Assert(sender is BackgroundWorker);

            BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

            Debug.Assert(e.Argument is GetNetworkAsyncArgs);

            GetNetworkAsyncArgs oGetNetworkAsyncArgs =
                (GetNetworkAsyncArgs)e.Argument;

            try
            {
                e.Result = GetFanPageNetworkInternal(oGetNetworkAsyncArgs.AccessToken, 
                                                     oGetNetworkAsyncArgs.Model);
            }
            catch (CancellationPendingException)
            {
                e.Cancel = true;
            }
           
        }      


        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        // [Conditional("DEBUG")]

        public override void
        AssertValid()
        {
            base.AssertValid();

            // (Do nothing else.)
        }

        public void
        ReportProgress(string sProgress)
        {
            base.ReportProgress(sProgress);
        }


        public String txt = "";

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// GraphML-attribute IDs.

        protected const String UserID = "uid";
        ///
        protected const String UserNameID = "UserName";
        
        ///Private fields
        private Dictionary<string,string> usersDisplayName;
        /// The source of Twitter networks, used in network descriptions.

        protected const String NetworkSource = "Facebook";

        private bool bGetStatusUpdates;
        private bool bGetWallPosts;
        private FacebookAPI fbAPI;
        private string m_sFanPage;


        //*************************************************************************
        //  Embedded class: GetNetworkAsyncArgs()
        //
        /// <summary>
        /// Contains the arguments needed to asynchronously get a network of Flickr
        /// users.
        /// </summary>
        //*************************************************************************

        protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
        {            
            ///
            public FacebookFanPageModel Model;           
        };
        
        
    }

}