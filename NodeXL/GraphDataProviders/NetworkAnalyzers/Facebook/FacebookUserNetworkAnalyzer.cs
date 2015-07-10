
//  Copyright (c) Microsoft Corporation.  All rights reserved.

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
//using Microsoft.Research.CommunityTechnologies.DateTimeLib;
using System.IO;
using System.Text;
using Facebook;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;


namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: sdcsd
    //
    /// <summary>
    /// Gets a network of Facebook friends.
    /// </summary>
    ///
    /// <remarks>
    /// Use <see cref="GetNetworkAsync" /> to asynchronously get a directed network
    /// of Facebook freinds.
    /// </remarks>
    //*****************************************************************************

    public class FacebookUserNetworkAnalyzer : FacebookNetworkAnalyzerBase
    {
        //*************************************************************************
        //  Constructor: sdcsd()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="sdcsd" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookUserNetworkAnalyzer()
        {
            // (Do nothing.)

            AssertValid();
        }

        //*************************************************************************
        //  Enum: WhatToInclude
        //
        /// <summary>
        /// Flags that specify what should be included in a network requested from
        /// this class.
        /// </summary>
        ///
        /// <remarks>
        /// The flags can be ORed together.
        /// </remarks>
        //*************************************************************************

        [System.FlagsAttribute]

        public enum
        WhatToInclude
        {
            /// <summary>
            /// Include nothing.
            /// </summary>

            None = 0,

            /// <summary>
            /// Include a vertex for each of the user's contacts.
            /// </summary>

            ContactVertices = 1,

            /// <summary>
            /// Include a vertex for each user who has commented on the user's
            /// photos.
            /// </summary>

            CommenterVertices = 2,

            /// <summary>
            /// Include information about each user in the network.
            /// </summary>

            UserInformation = 4,
        }

        private int NrOfSteps = 4;
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
            FacebookUserModel oModel
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
        /// Synchronously gets a directed network of Facebook friends.
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
            FacebookUserModel oModel
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();


            return (GetFriendsNetworkInternal(s_accessToken, oModel));
        }

        //*************************************************************************
        //  Method: GetFriendsNetworkInternal()
        //
        /// <summary>
        /// Gets the friends network from Facebook.
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
        GetFriendsNetworkInternal
        (
            string sAccessToken,            
            FacebookUserModel oModel
        )

        {
            Debug.Assert(!String.IsNullOrEmpty(sAccessToken));            
            AssertValid();           

            //Set the total nr of steps
            if (oModel.GetTooltips) NrOfSteps++;

            oGraphMLXmlDocument = CreateGraphMLXmlDocument(oModel);

            RequestStatistics oRequestStatistics = new RequestStatistics();


            FacebookUserNetworkDownloader oUserNetworkDownloader = new FacebookUserNetworkDownloader(
                                                                    sAccessToken,
                                                                    oModel,
                                                                    this,
                                                                    NrOfSteps);
            List<JSONObject> oPosts, oLikes, oComments;
            JSONObject oEgo;
            Dictionary<string, Dictionary<string, JSONObject>> oAttributes;
            Dictionary<String, String> oStatusUpdates;


            oUserNetworkDownloader.DownloadNetwork(out oPosts, out oLikes, out oComments,
                                                    out oEgo, out oAttributes, out oStatusUpdates);

            VertexCollection oVertices;
            EdgeCollection oEdges;

            FacebookUserNetworkCreator oUserNetworkCreator = new FacebookUserNetworkCreator(
                                                            oModel, oPosts, oLikes,
                                                            oComments,
                                                            oEgo, oAttributes,
                                                            oStatusUpdates);

            oUserNetworkCreator.CreateNetwork(out oVertices, out oEdges);

            FacebookUserNetworkLoader oUserNetworkLoader = new FacebookUserNetworkLoader(
                                                                oVertices, oEdges, oModel, this
                                                                );

            oUserNetworkLoader.LoadNetwork(ref oGraphMLXmlDocument);

            ReportProgress("Importing downloaded network into NodeXL");

            //After successfull download of the network
            //get the network description
            String sLoggedInUsername = oEgo.Dictionary["name"].String;

            String sGraphDescription = GetNetworkDescription(oModel, sLoggedInUsername, oVertices.Count,
                                        oEdges.Count, oGraphMLXmlDocument);

            
            OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics,
                "FacebookUser", sLoggedInUsername,
                sGraphDescription,
                sLoggedInUsername+" Facebook User",
                sLoggedInUsername + " Facebook User"
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
        /// <param name="includeMe">
        /// A boolean value that states if the logged in user
        /// is included in the network.
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
            FacebookUserModel oModel,
            string sLoggedInUsername,
            int iVertexCount,
            int iEdgeCount,
            GraphMLXmlDocument oGraphMLXmlDocument
        )
        {            
            Debug.Assert(oGraphMLXmlDocument != null);
            AssertValid();

            NetworkDescriber oNetworkDescriber = new NetworkDescriber();

            

            oNetworkDescriber.AddSentence(
                "The graph represents the network of " + sLoggedInUsername + "\'s timeline."
                );

            oNetworkDescriber.AddNetworkTime(NetworkSource);
            
            oNetworkDescriber.AddSentence(
                    "The network has "+iVertexCount+" vertices and "+
                    iEdgeCount+" edges."
                    );

            if (oModel.PostAuthor)
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each post author."
                );
            }
            if (oModel.Comment)
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user that has commented on a post."
                );
            }
            if (oModel.Like)
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user that has liked a post."
                );
            }
            if (oModel.UserTagged)
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user tagged in a post."
                );
            }

            oNetworkDescriber.AddSentence(
                "The network is built "
                );

            if (oModel.DownloadFromPost)
            {
                oNetworkDescriber.AddSentence(
                "from post number "+oModel.FromPost+" to post number "+oModel.ToPost+" of user's timeline."
                );
            }
            else if (oModel.DownloadPostsBetweenDates)
            {
                oNetworkDescriber.AddSentence(
                "upon posts between " + oModel.FromDate.ToString() + " and " + oModel.ToDate.ToString() + " of user's timeline."
                );
            }

            if(oModel.Limit)
            {
                oNetworkDescriber.AddNetworkLimit(oModel.LimitAmount, "comments/likes");
            }

            return (oNetworkDescriber.ConcatenateSentences());
        }
        

        //*************************************************************************
        //  Method: CreateGraphMLXmlDocument()
        //
        /// <summary>
        /// Creates a GraphMLXmlDocument representing a network of friends in Facebook.
        /// </summary>
        ///        
        /// <returns>
        /// A GraphMLXmlDocument representing a network of Facebook friends.  The
        /// document includes GraphML-attribute definitions but no vertices or
        /// edges.
        /// </returns>
        //*************************************************************************

        protected GraphMLXmlDocument
        CreateGraphMLXmlDocument
        (
            FacebookUserModel oModel
        )
        {
            AssertValid();

            GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

            NodeXLGraphMLUtil.DefineVertexImageFileGraphMLAttribute(oGraphMLXmlDocument);
            NodeXLGraphMLUtil.DefineVertexCustomMenuGraphMLAttributes(oGraphMLXmlDocument);            

            NodeXLGraphMLUtil.DefineEdgeRelationshipGraphMLAttribute(oGraphMLXmlDocument);

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
            
            //Define edge attributes
            foreach (AttributeUtils.Attribute oAttribute in oEdgeAttributes)
            {
                oGraphMLXmlDocument.DefineGraphMLAttribute(true, oAttribute.value, oAttribute.name, "string", null);                
            }

            return (oGraphMLXmlDocument);
        }

        private List<AttributeUtils.Attribute>
        SetEdgeAttributes
        (
            FacebookUserModel oModel
        )
        {

            return AttributeUtils.UserNetworkEdgeAttributes;
        }

        private List<AttributeUtils.Attribute>
        SetVertexAttributes
        (
            FacebookUserModel oModel
        )
        {
            List<AttributeUtils.Attribute> oVertexAttributes = new List<AttributeUtils.Attribute>();

            
            oVertexAttributes.AddRange(AttributeUtils.VertexUserAttributes);

            if (oModel.GetTooltips)
            {
                oVertexAttributes.AddRange(AttributeUtils.StatusUpdatesUserAttributes);
            }            

            return oVertexAttributes.Distinct().ToList();
        }

        public void
        ReportProgress
        (
            string sMessage
        )
        {
            base.ReportProgress(sMessage);
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
                e.Result = GetFriendsNetworkInternal
                    (
                    oGetNetworkAsyncArgs.AccessToken,                    
                    oGetNetworkAsyncArgs.Model
                    );
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

        public String txt = "";

        //*************************************************************************
        //  Protected fields
        //*************************************************************************        

        /// GraphML-attribute IDs.

        protected const String UserID = "uid";
        ///
        protected const String UserNameID = "UserName";          
        ///
        private GraphMLXmlDocument oGraphMLXmlDocument;
        /// The source of Twitter networks, used in network descriptions.

        protected const String NetworkSource = "Facebook";


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
            public AttributesDictionary<bool> attributes;
            ///           
            public FacebookUserModel Model;
        };
        
    }

}
