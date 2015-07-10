using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Smrf.AppLib;
using System.Text.RegularExpressions;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookUserNetworkCreator : FacebookNetworkCreatorBase
    {                
        private JSONObject m_oEgo;                
        
        public FacebookUserNetworkCreator
        (
            FacebookUserModel oModel,
            List<JSONObject> oPosts,
            List<JSONObject> oLikes,
            List<JSONObject> oComments,            
            JSONObject oEgo,
            Dictionary<String, Dictionary<String, JSONObject>> oAttributes,
            Dictionary<String, String> oStatusUpdates            
        )
        :
        base 
        (
            oModel,
            oPosts,
            oLikes,
            oComments,
            oAttributes,
            oStatusUpdates
        )
        {                        
            m_oEgo = oEgo;                                  
        }

        

        protected override void
        CreateVertices
        (
            ref VertexCollection oVertices            
        )
        {            

            Dictionary<string, List<Dictionary<string, object>>> statusUpdates = new Dictionary<string, List<Dictionary<string, object>>>();
            Dictionary<string, List<Dictionary<string, object>>> wallPosts = new Dictionary<string, List<Dictionary<string, object>>>();
            
            CreateEgoVertex(ref oVertices);

            bool bGetPosts = (Model.PostAuthor || Model.UserTagged ||
                              Model.Comment || Model.Like);            

            if (bGetPosts || Model.Comment || Model.Like)
            {
                CreatePostAuthorVertices(ref oVertices);
            }

            if (Model.UserTagged)
            {
                CreateUserTaggedVertices(ref oVertices);
            }

            if (Model.Comment)
            {
                CreateCommentVertices(ref oVertices);
                
            }
            
            if (Model.Like)
            {
                CreateLikeVertices(ref oVertices);                
            }            
        }        

        private void
        CreateEgoVertex
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.VertexUserAttributes);

            Vertex oVertex = new Vertex(m_oEgo.Dictionary["id"].String,
                                        ManageDuplicateName(m_oEgo.Dictionary["name"].String, ref oVertices),
                                        VertexType.User, new AttributesDictionary<string>(oUserAttributes));
            AddUserVertexAttributes(ref oVertex, "Ego", "", "",
                                    !oVertices.Contains(oVertex), ref oVertices);
            oVertices.Add(oVertex);
        }

        private void
        CreateUserTaggedVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.VertexUserAttributes);

            foreach (JSONObject oPost in m_oPosts)
            {                
                if(oPost.Dictionary.ContainsKey("message_tags"))
                {
                    //message_tags is given as a dictionary
                    foreach (KeyValuePair<String, JSONObject> messageTag in oPost.Dictionary["message_tags"].Dictionary)
                    {
                        foreach(String sTaggedId in (messageTag.Value.Array.Select(x => x.Dictionary["id"].String)))
                        {                            
                            Vertex oUserTagged = new Vertex(sTaggedId, "", VertexType.User,
                                                new AttributesDictionary<string>(oUserAttributes));//"User Tagged"
                            try
                            {
                                AddUserVertexAttributes(ref oUserTagged, "User Tagged", "", GetPostMessage(oPost),
                                                        !oVertices.Contains(oUserTagged), ref oVertices);
                                oVertices.Add(oUserTagged);
                            }
                            catch (KeyNotFoundException e)
                            {
                                //Do Nothing
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        
                    }                    
                }

                if (oPost.Dictionary.ContainsKey("with_tags"))
                {
                    foreach (String sTaggedId in oPost.Dictionary["with_tags"].Dictionary["data"]
                                            .Array.Select(x => x.Dictionary["id"].String))
                    {                       
                        Vertex oUserTagged = new Vertex(sTaggedId, "", VertexType.User,
                                                new AttributesDictionary<string>(oUserAttributes));//"User Tagged"
                        try
                        {
                            AddUserVertexAttributes(ref oUserTagged, "User Tagged", "", GetPostMessage(oPost),
                                                    !oVertices.Contains(oUserTagged), ref oVertices);
                            oVertices.Add(oUserTagged);
                        }
                        catch (KeyNotFoundException e)
                        {
                            //Do Nothing
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
        }


        private void
        CreateCommentVertices
        (
            ref VertexCollection oVertices            
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.VertexUserAttributes);
            
            foreach (var item in m_oComments)
            {
                Vertex oVertex = new Vertex(item.Dictionary["from"].Dictionary["id"].String, "", VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));

                try
                {
                    AddUserVertexAttributes(ref oVertex, "Commenter", item.Dictionary["message"].String, "",
                                            !oVertices.Contains(oVertex), ref oVertices);

                    oVertices.Add(oVertex);
                }
                catch (KeyNotFoundException e)
                {
                    //Do Nothing
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void
        CreateLikeVertices
        (
            ref VertexCollection oVertices           
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.VertexUserAttributes);

            foreach (var item in m_oLikes)
            {
                Vertex oVertex = new Vertex(item.Dictionary["id"].String, "", VertexType.User, 
                                            new AttributesDictionary<string>(oUserAttributes));

                try
                {
                    AddUserVertexAttributes(ref oVertex, "Liker", "", GetPostContent(item.Dictionary["object_id"].String),
                                            !oVertices.Contains(oVertex), ref oVertices);

                    oVertices.Add(oVertex);
                }
                catch (KeyNotFoundException e)
                {
                    //Do Nothing
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void
        CreatePostAuthorVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.VertexUserAttributes);

            foreach (JSONObject oPost in m_oPosts)
            {
                Vertex oVertex = new Vertex(oPost.Dictionary["from"].Dictionary["id"].String,
                                            "",
                                            VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));


                AddUserVertexAttributes(ref oVertex, "Post Author", "", GetPostMessage(oPost),
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }
        }        

        private List<AttributeUtils.Attribute>
        BuildUserAttributes
        (
            List<AttributeUtils.Attribute> oBaseUserAttributes
        )
        {
            List<AttributeUtils.Attribute> oNewUserAttributes = new List<AttributeUtils.Attribute>();
            oNewUserAttributes.AddRange(oBaseUserAttributes);

            if (Model.GetTooltips)
            {
                oNewUserAttributes.AddRange(AttributeUtils.StatusUpdatesUserAttributes);
            }

            //if (m_oModel.GetWallPosts)
            //{
            //    oNewUserAttributes.AddRange(AttributeUtils.WallPostsUserAttributes);
            //}

            return oNewUserAttributes;
        }

        private void
        AddUserVertexAttributes
        (
            ref Vertex oVertex,
            string sUserType,
            string sComment,
            string sPost, //if the user is a liker
            bool bNewVertex,
            ref VertexCollection oVertices
        )
        {
            oVertex.Attributes["vertex_type"] = oVertex.Type.ToString();            
            if (bNewVertex)
            {                
                //Add vertex name
                try
                {
                    oVertex.Name = ManageDuplicateName(m_oAttributes[oVertex.ID]["name"].String, ref oVertices);                    
                    oVertex.Attributes.Add(m_oAttributes[oVertex.ID].ToDictionary(x => x.Key, x => GetAttributeValue(x.Key,x.Value)));
                }                
                catch (Exception e)
                {
                    //Do Nothing
                    oVertex.Name = "[No Name]";
                }

                oVertex.Attributes["user_type"] = sUserType;

                if (Model.GetTooltips)
                {
                    AddUserVertexStatusUpdatesAttributes(ref oVertex);
                }                

                if (sUserType == "Commenter")
                {
                    oVertex.Attributes["comment"] = sComment;
                    oVertex.Attributes["Tooltip"] = String.Format("{0}\n\n{1}", oVertex.Name, StringUtil.BreakIntoLines(sComment, 30));
                    oVertex.Attributes["Label"] = String.Format("{0}\n\n{1}", oVertex.Name, sComment);
                }
                else //if (sUserType == "Liker")
                {
                    oVertex.Attributes["Tooltip"] = String.Format("{0}\n\n{1}", oVertex.Name, StringUtil.BreakIntoLines(sPost, 30));
                    oVertex.Attributes["Label"] = String.Format("{0}\n\n{1}", oVertex.Name, sPost);
                }

            }
            else
            {
                if (sUserType == "Commenter")
                {
                    oVertex.Attributes["comment"] += "\n\n" + sComment;
                    oVertex.Attributes["Tooltip"] += "\n\n" + StringUtil.BreakIntoLines(sComment, 30);
                    oVertex.Attributes["Label"] += "\n\n" + sComment;
                }
                else //if (sUserType == "Liker")
                {
                    oVertex.Attributes["Tooltip"] += "\n\n" + StringUtil.BreakIntoLines(sPost, 30);
                    oVertex.Attributes["Label"] += "\n\n" + sPost;
                }
            }
        }        

        private void
        AddUserVertexStatusUpdatesAttributes
        (
            ref Vertex oVertex
        )
        {
            if (m_oStatusUpdates.ContainsKey(oVertex.ID))
            {
                string sStatusUpdate = m_oStatusUpdates[oVertex.ID];
                string sStatusTags = "";                

                oVertex.Attributes["statuses"] = sStatusUpdate;
                oVertex.Attributes["statuses_tags"] = sStatusTags;
                oVertex.Attributes["statuses_urls"] = GetURLs(sStatusUpdate, ' ');
                oVertex.Attributes["statuses_hashtags"] = GetHashtags(sStatusUpdate, ' ');
            }                
        }

        private void
        AddUserVertexWallPostsAttributes
        (
            ref Vertex oVertex
        )
        {
            //if (m_oWallPosts.ContainsKey(oVertex.ID))
            //{
            //    string sWallPost = "";
            //    string sWallTags = "";

            //    foreach (var oWallPost in m_oWallPosts[oVertex.ID].Where(x => ((JSONObject)x["message"]).String != ""))
            //    {
            //        if (sWallPost.Length > 8000)
            //        {
            //            break;
            //        }
            //        sWallPost += ((JSONObject)oWallPost["message"]).String + "\n\n";
            //        if (oWallPost["message_tags"] is List<JSONObject>)
            //        {
            //            sWallTags = String.Join(",", ((List<JSONObject>)oWallPost["message_tags"]).Select(x => x.String).ToArray());
            //            sWallTags += "\n\n";
            //        }
            //    }

            //    oVertex.Attributes["wall_posts"] = sWallPost;
            //    oVertex.Attributes["wall_tags"] = sWallTags;
            //    oVertex.Attributes["wall_urls"] = GetURLs(sWallPost, ' ');
            //    oVertex.Attributes["wall_hashtags"] = GetHashtags(sWallPost, ' ');
            //}            
        }

        protected override void
        CreateEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices            
        )
        {
            if (Model.PostAuthor)
            {
                CreatePostAuthorEdges(ref oEdges, ref oVertices);
            }

            if (Model.UserTagged)
            {
                CreateUserTaggedEdges(ref oEdges, ref oVertices);
            }

            if (Model.Comment)
            {
                CreateCommentEdges(ref oEdges, ref oVertices);
            }

            if (Model.Like)
            {
                CreateLikeEdges(ref oEdges, ref oVertices);
            }

            if (Model.IncludeMe)
            {
                CreateIncludeMeEdges(ref oEdges, ref oVertices);
            }
        }        

        private void
        CreatePostAuthorEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            foreach (JSONObject oPost in m_oPosts)
            {

                Vertex oAuthor = oVertices[oPost.Dictionary["from"].Dictionary["id"].String];
                Vertex oOwner = null;
                try
                {
                    oOwner = oVertices[m_oEgo.Dictionary["id"].String];
                }
                catch (KeyNotFoundException e)
                {
                    //There is no "to" key in the dictionary
                    //Set the owner of the post same as the author
                    oOwner = oAuthor;
                }
                Edge oEdge = new Edge(oAuthor, oOwner, EdgeType.UserNetworkPostAuthor);

                AddAttributesToEdge(oEdge, "Post Author", GetPostMessage(oPost),
                                    oOwner.Name,
                                    DateUtil.ConvertToDateTime(oPost.Dictionary["created_time"].String).ToString(),
                                    GetShareCount(oPost), "");

                if (oAuthor != null && oOwner != null)
                {
                    oEdges.Add(oEdge);
                }

            }              
        }

        private void
        CreateUserTaggedEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            
            foreach (JSONObject oPost in m_oPosts)
            {
                Vertex oAuthor = oVertices[oPost.Dictionary["from"].Dictionary["id"].String];
                if (oAuthor != null)
                {
                    if (oPost.Dictionary.ContainsKey("message_tags"))
                    {
                        //message_tags is given as a dictionary
                        foreach (KeyValuePair<String, JSONObject> messageTag in oPost.Dictionary["message_tags"].Dictionary)
                        {
                            foreach (String sTaggedId in (messageTag.Value.Array.Select(x => x.Dictionary["id"].String)))
                            {
                                Vertex oUserTagged = oVertices[sTaggedId];
                                if (oUserTagged != null)
                                {
                                    Edge oEdge = new Edge(oUserTagged, oAuthor, EdgeType.UserNetworkUserTagged);

                                    AddAttributesToEdge(oEdge, "User Tagged", "",
                                                        oVertices[m_oEgo.Dictionary["id"].String].Name,
                                                        DateUtil.ConvertToDateTime(oPost.Dictionary["created_time"].String).ToString(),
                                                        "", "");
                                    oEdges.Add(oEdge);
                                }
                            }

                        }
                    }

                    if (oPost.Dictionary.ContainsKey("with_tags"))
                    {
                        foreach (String sTaggedId in oPost.Dictionary["with_tags"].Dictionary["data"]
                                                .Array.Select(x => x.Dictionary["id"].String))
                        {
                            Vertex oUserTagged = oVertices[sTaggedId];
                            if (oUserTagged != null)
                            {
                                Edge oEdge = new Edge(oUserTagged, oAuthor, EdgeType.UserNetworkUserTagged);

                                AddAttributesToEdge(oEdge, "User Tagged", "",
                                                    oVertices[m_oEgo.Dictionary["id"].String].Name,
                                                    DateUtil.ConvertToDateTime(oPost.Dictionary["created_time"].String).ToString(),
                                                    "", "");
                                oEdges.Add(oEdge);
                            }
                        }
                    }                    
                }
            }
            
        }

        private void
        CreateCommentEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            foreach (JSONObject oCommenter in m_oComments)
            {
                JSONObject oPost = m_oPosts.FirstOrDefault(x => x.Dictionary["id"].String == oCommenter.Dictionary["object_id"].String);
                Vertex oAuthor = oVertices[oPost.Dictionary["from"].Dictionary["id"].String];
                Vertex oCommenterV = oVertices[oCommenter.Dictionary["from"].Dictionary["id"].String];
                if (oAuthor != null && oCommenterV != null)
                {
                    Edge oEdge = new Edge(oCommenterV, oAuthor, EdgeType.UserNetworkCommenter);

                    AddAttributesToEdge(oEdge, "Commenter", oCommenter.Dictionary["message"].String,
                                        oVertices[m_oEgo.Dictionary["id"].String].Name,
                                        oCommenter.Dictionary["created_time"].String,
                                        "", oCommenter.Dictionary["like_count"].Integer.ToString());

                    oEdges.Add(oEdge);
                }


            }
        }

        private void
        CreateLikeEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            foreach (JSONObject oLiker in m_oLikes)
            {
                JSONObject oPost = m_oPosts.FirstOrDefault(x => x.Dictionary["id"].String == oLiker.Dictionary["object_id"].String);
                Vertex oAuthor = oVertices[oPost.Dictionary["from"].Dictionary["id"].String];
                //Vertex oAuthor = oVertices[GetPostAuthorForLikerCommenter(oLiker)];
                Vertex oCommenterV = oVertices[oLiker.Dictionary["id"].String];
                if (oCommenterV != null && oAuthor != null)
                {
                    Edge oEdge = new Edge(oCommenterV, oAuthor, EdgeType.UserNetworkLiker);

                    AddAttributesToEdge(oEdge, "Liker", "",
                                        oVertices[m_oEgo.Dictionary["id"].String].Name,
                                        "", "", "");
                    oEdges.Add(oEdge);
                }
            }
        }

        private void
        CreateIncludeMeEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            List<Vertex> oFriends = oVertices.Where(x => x.Type == VertexType.User && x.Attributes["user_type"] == "Friend").ToList();
            Vertex oEgo = oVertices.FirstOrDefault(x => x.Type == VertexType.User && x.Attributes["user_type"] == "Ego");

            if (oEgo != null)
            {
                foreach (Vertex oFriend in oFriends)
                {
                    Edge oEdge = new Edge(oEgo, oFriend, EdgeType.UserNetworkEgo);
                    
                    AddAttributesToEdge(oEdge, "Friend", "", "", "", "", "");
                    Edge oEdge1 = new Edge(oFriend, oEgo, EdgeType.UserNetworkEgo);
                    
                    AddAttributesToEdge(oEdge1, "Friend", "", "", "", "", "");
                    oEdges.Add(oEdge);
                }
            }
        }

        private void AddAttributesToEdge
        (
            Edge oEdge,
            string sRelationship,
            string sComment,
            string sFeedOfOrigin,
            string sTimestamp,
            string sShareCount,
            string sCommentLikes
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["edge_comment"] = sComment;
            oEdge.Attributes["feed_of_origin"] = sFeedOfOrigin;
            oEdge.Attributes["timestamp"] = sTimestamp;
            oEdge.Attributes["share_count"] = sShareCount;
            oEdge.Attributes["comment_likes"] = sCommentLikes;
        }

        private string
        ManageDuplicateName
        (
            string sVertexName,
            ref VertexCollection oVertices  
        )
        {
            Vertex oMatchingVertex = oVertices.FirstOrDefault(x=>x.Name==sVertexName);
            int i = 1;            
            while(oMatchingVertex != null)
            {
                sVertexName += i.ToString();
                oMatchingVertex = oVertices.FirstOrDefault(x => x.Name == sVertexName);
                i++;
            }

            return sVertexName;
        }

        private string
        GetPostContent
        (
            string sPostId
        )
        {
            string sMessage = "";

            JSONObject oPost = m_oPosts.FirstOrDefault(x => x.Dictionary["id"].String == sPostId);

            if (oPost != null)
            {
                sMessage = GetPostMessage(oPost);
            }

            return sMessage;
        }

        private string
        CreatePostLink
        (
            string sPostId
        )
        {
            return "https://www.facebook.com/"+sPostId;
        }

        private string
        CreateCommentLink
        (
            string sPostId,
            string sCommentId
        )
        {
            sCommentId = sCommentId.Split('_')[1];
            return "https://www.facebook.com/" + sPostId+"/?comment_id="+sCommentId;
        }

        private string
        GetLikeCount
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary["like_info"].Dictionary.ContainsKey("like_count"))
            {
                return oPost.Dictionary["like_info"].Dictionary["like_count"].Integer.ToString();
            }

            return "";
        }

        private string
        GetCommentCount
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary["comment_info"].Dictionary.ContainsKey("comment_count"))
            {
                return oPost.Dictionary["comment_info"].Dictionary["comment_count"].Integer.ToString();
            }

            return "";
        }

        private String
        GetShareCount
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary.ContainsKey("shares"))
            {
                return oPost.Dictionary["shares"].Integer.ToString();
            }

            return "";
        }

        private String
        GetPostMessage
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary.ContainsKey("message"))
            {
                return oPost.Dictionary["message"].String;
            }

            return "";
        }

        //*************************************************************************
        //  Method: GetURLs()
        //
        /// <summary>
        /// Returns the URLs found in a string
        /// </summary>
        ///
        /// <param name="txt">
        /// The text to search for URLs
        /// </param> 
        ///
        /// <param name="concatenator">
        /// The concatenator of the URLs
        /// </param>
        /// 
        /// <returns>
        /// A string with found URLs concatenated with the specified concatenator
        /// </returns>
        //*************************************************************************
        private string
        GetURLs
        (
            string txt,
            char concatenator
        )
        {
            Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            txt = " ";
            foreach (Match match in mactches)
            {
                txt += match.Value + concatenator;

            }

            return txt.Remove(txt.Length - 1);
        }
        

        //*************************************************************************
        //  Method: GetHashtags()
        //
        /// <summary>
        /// Returns the hashtags found in a string
        /// </summary>
        ///
        /// <param name="txt">
        /// The text to search for hashtags
        /// </param> 
        ///
        /// <param name="concatenator">
        /// The concatenator of the hashtags
        /// </param>
        /// 
        /// <returns>
        /// A string with found hashtags concatenated with the specified concatenator
        /// </returns>
        //*************************************************************************
        private string
        GetHashtags
        (
            string txt,
            char concatenator
        )
        {
            //(#)((?:[A-Za-z0-9-_]*))
            Regex regx = new Regex("(?:(?<=\\s)|^)#(\\w*[A-Za-z_]+\\w*)", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            txt = " ";
            foreach (Match match in mactches)
            {
                txt += match.Value + concatenator;

            }

            return txt.Remove(txt.Length - 1);
        }

        private FacebookUserModel
        Model
        {
            get { return ((FacebookUserModel)m_oModel ); }
        }

    }
}
