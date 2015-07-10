using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Smrf.AppLib;
using System.Text.RegularExpressions;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookFanPageGroupNetworkCreatorBase : FacebookNetworkCreatorBase
    {
        protected List<JSONObject> m_oCommentLikes;
        protected List<JSONObject> m_oCommentComments;
        protected List<JSONObject> m_oShares;
        protected Dictionary<string, List<JSONObject>> m_oCommentsByPost;
        protected Dictionary<string, List<JSONObject>> m_oLikesByPost;
        protected Dictionary<string, List<JSONObject>> m_oCommentsByUser;
        protected Dictionary<string, List<JSONObject>> m_oLikesByUser;
        protected Dictionary<string, string> m_oPostAuthorPost;

        public FacebookFanPageGroupNetworkCreatorBase
        (
            FacebookFanPageGroupModelBase oModel,
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
                oAttributes,
                oStatusUpdates
        )
        {
            m_oCommentLikes = oCommentLikes;
            m_oCommentComments = oCommentComments;
            m_oShares = oShares;
        }

        

        public new void
        CreateNetwork
        (
            out VertexCollection oVertices,
            out EdgeCollection oEdges
        )
        {
            oVertices = new VertexCollection();
            oEdges = new EdgeCollection();

            GroupLikesAndComments();

            //Fill post-author post dictionary
            m_oPostAuthorPost = m_oPosts.GroupBy(x=>x.Dictionary["id"].String).Select(x=>x.First()).ToDictionary(x => x.Dictionary["id"].String,
                                                            x => x.Dictionary["from"].Dictionary["id"].String);

            CreateVertices(ref oVertices);
            CreateEdges(ref oEdges, ref oVertices);
        }

        protected override void
        CreateVertices
        (
            ref VertexCollection oVertices
        )
        {
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates = new Dictionary<string, List<Dictionary<string, object>>>();
            Dictionary<string, List<Dictionary<string, object>>> wallPosts = new Dictionary<string, List<Dictionary<string, object>>>();
            
            //Only commenters vertices will be added
            if (Model.Comment)
            {
                CreateCommentVertices(ref oVertices);
                if (Model.RelationshipCommentAuthor)
                {
                    CreateCommentCommentVertices(ref oVertices);
                }
            }
            //Only likers vertices will be added
            if (Model.Like)
            {
                CreateLikeVertices(ref oVertices);
                if (Model.RelationshipCommentAuthor)
                {
                    CreateCommentLikeVertices(ref oVertices);
                }
            }

            //Add post vertices if any related network is selected
            if (Model.Post)
            {
                CreatePostVertices(ref oVertices);
            }

            //Add post author vertices if any related network is selected
            if (Model.RelationshipPostAuthor || Model.Post)
            {
                CreatePostAuthorVertices(ref oVertices);
            }
        }

        private void
        CreateCommentVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.FanPageUserAttributes);

            foreach (var item in m_oComments)
            {
                Vertex oVertex = new Vertex(item.Dictionary["from"].Dictionary["id"].String, "", VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));

                AddUserVertexAttributes(ref oVertex, "Commenter", item.Dictionary["message"].String, "",
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }
        }

        private void
        CreateCommentCommentVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.FanPageUserAttributes);

            foreach (var item in m_oCommentComments)
            {
                Vertex oVertex = new Vertex(item.Dictionary["from"].Dictionary["id"].String, "", VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));

                //TODO: Add new attributes
                AddUserVertexAttributes(ref oVertex, "Comment Commenter", item.Dictionary["message"].String, "",
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }
        }

        private void
        CreateLikeVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.FanPageUserAttributes);

            foreach (var item in m_oLikes)
            {
                Vertex oVertex = new Vertex(item.Dictionary["id"].String, "", VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));

                AddUserVertexAttributes(ref oVertex, "Liker", "", GetPostContent(item.Dictionary["object_id"].String),
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }
        }

        private void
        CreateCommentLikeVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.FanPageUserAttributes);


            foreach (var oLike in m_oCommentLikes)
            {
                Vertex oVertex = new Vertex(oLike.Dictionary["id"].String, "", VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));

                //TODO: Fix attributes
                AddUserVertexAttributes(ref oVertex, "Liker", "", "",//GetPostContent(item.Dictionary["post_id"].String),
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }

        }

        private void
        CreatePostAuthorVertices
        (
            ref VertexCollection oVertices
        )
        {
            List<AttributeUtils.Attribute> oUserAttributes = BuildUserAttributes(AttributeUtils.FanPageUserAttributes);

            foreach (JSONObject oPost in m_oPosts)
            {
                Vertex oVertex = new Vertex(oPost.Dictionary["from"].Dictionary["id"].String,
                                            "",
                                            VertexType.User,
                                            new AttributesDictionary<string>(oUserAttributes));


                AddUserVertexAttributes(ref oVertex, "Post Author", "", GetPostContent(oPost),
                                        !oVertices.Contains(oVertex), ref oVertices);

                oVertices.Add(oVertex);
            }
        }

        private void
        CreatePostVertices
        (
            ref VertexCollection oVertices
        )
        {
            foreach (JSONObject oPost in m_oPosts)
            {
                Vertex oVertex = new Vertex(oPost.Dictionary["id"].String,
                                            oPost.Dictionary["id"].String,
                                            VertexType.Post,
                                            new AttributesDictionary<string>(AttributeUtils.PostAttributes));

                string sImage = "";

                if (oPost.Dictionary.ContainsKey("picture"))
                {
                    //Used to be "src" now is "href". Maybe both
                    try
                    {
                        sImage = oPost.Dictionary["picture"].String;
                    }
                    catch (KeyNotFoundException e)
                    {

                    }
                }

                AddPostVertexAttributes(ref oVertex, GetPostContent(oPost),
                                        CreatePostLink(oPost.Dictionary["id"].String), sImage,
                                        GetLikeCount(oPost), GetCommentCount(oPost),
                                        !oVertices.Contains(oVertex));

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

            if (Model.GetStatusUpdates)
            {
                oNewUserAttributes.AddRange(AttributeUtils.StatusUpdatesUserAttributes);
            }

            //if (Model.GetWallPosts)
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
                    oVertex.Attributes.Add(m_oAttributes[oVertex.ID].ToDictionary(x => x.Key, x => GetAttributeValue(x.Key, x.Value)));
                }
                catch (Exception e)
                {
                    //Do Nothing
                    oVertex.Name = "[No Name]_"+oVertex.ID;
                }

                oVertex.Attributes["user_type"] = sUserType;

                oVertex.Attributes["likes_created"] = GetLikesCreated(oVertex.ID).ToString();
                oVertex.Attributes["comments_created"] = GetCommentsCreated(oVertex.ID).ToString();

                oVertex.Attributes["likes_received"] = GetLikesReceived(oVertex.ID).ToString();
                oVertex.Attributes["comments_received"] = GetCommentsReceived(oVertex.ID).ToString();

                if (Model.GetStatusUpdates)
                {
                    AddUserVertexStatusUpdatesAttributes(ref oVertex);
                }

                //if (Model.GetWallPosts)
                //{
                //    AddUserVertexWallPostsAttributes(ref oVertex);
                //}

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
        AddPostVertexAttributes
        (
            ref Vertex oVertex,
            string sContent,
            string sPostURL,
            string sImage,
            string sNrLikes,
            string sNrComments,
            bool bNewVertex
        )
        {
            if (bNewVertex)
            {
                oVertex.Attributes["content"] = sContent;
                oVertex.Attributes["Tooltip"] = StringUtil.BreakIntoLines(sContent, 30);
                oVertex.Attributes["Label"] = sContent;
                oVertex.Attributes["image"] = sImage;
                oVertex.Attributes["post_link"] = sPostURL;
                oVertex.Attributes["likes"] = sNrLikes;
                oVertex.Attributes["comments"] = sNrComments;
                oVertex.Attributes["vertex_type"] = oVertex.Type.ToString();
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
                string sStatusUpdate = "";
                string sStatusTags = "";

                //foreach (var oStatus in m_oStatusUpdates[oVertex.ID].Where(x=>((JSONObject)x["message"]).String != ""))
                //{
                //    if (sStatusUpdate.Length > 8000)
                //    {
                //        break;
                //    }
                //    sStatusUpdate += ((JSONObject)oStatus["message"]).String + "\n\n";
                //    if (oStatus["message_tags"] is List<JSONObject>)
                //    {
                //        sStatusTags = String.Join(",", ((List<JSONObject>)oStatus["message_tags"]).Select(x => x.String).ToArray());
                //        sStatusTags += "\n\n";
                //    }
                //}

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
            //Check if there is any edge selected
            if (Model.UserRelationshipSamePost ||
                Model.PostSameRelationship ||
                Model.RelationshipPostAuthor ||
                Model.ConsecutiveRelationship ||
                Model.RelationshipCommentAuthor)
            {
                //Create selected edges
                if (Model.UserRelationshipSamePost)
                {
                    CreateUserRelationshipSamePostEdges(ref oEdges, ref oVertices);
                }

                if (Model.PostSameRelationship)
                {
                    CreatePostSameRelationshipEdges(ref oEdges, ref oVertices);
                }

                if (Model.RelationshipPostAuthor)
                {
                    CreateRelationshipPostAuthorEdges(ref oEdges, ref oVertices);
                }

                if (Model.ConsecutiveRelationship)
                {
                    CreateConsecutiveRelationshipEdges(ref oEdges, ref oVertices);
                }

                if (Model.RelationshipCommentAuthor)
                {
                    CreateRelationshipCommentAuthorEdges(ref oEdges, ref oVertices);
                }

                if (Model.Post)
                {
                    CreateUserCreatedPostEdges(ref oEdges, ref oVertices);

                    //if (!Model.PostSameRelationship)
                    //{
                    //    CreateAuthorPostEdges(ref oEdges, ref oVertices);
                    //}
                }
            }
        }

        private void
        CreateUserCreatedPostEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Post)
            {
                foreach (var oPost in m_oPosts)
                {

                    Edge oEdge = new Edge(oVertices[oPost.Dictionary["from"].Dictionary["id"].String],
                                          oVertices[oPost.Dictionary["from"].Dictionary["id"].String],
                                          EdgeType.FanPageNetworkUserCreatedPost);

                    AddFanPageUserCreatedPostEdgeAttributes(oEdge, "Created Post",
                                                                  GetPostContent(oPost),
                                                                  CreatePostLink(oPost.Dictionary["id"].String),
                                                                  oPost.Dictionary["created_time"].String,
                                                                  GetLikeCount(oPost),
                                                                  GetCommentCount(oPost));
                    oEdges.Add(oEdge);
                }
            }
        }

        private void
        CreateUserRelationshipSamePostEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Comment)
            {
                foreach (var oItem in m_oCommentsByPost)
                {
                    for (int i = 0; i < oItem.Value.Count - 1; i++)
                    {
                        for (int j = i + 1; j < oItem.Value.Count; j++)
                        {
                            Edge oEdge = new Edge(oVertices[oItem.Value[i].Dictionary["from"].Dictionary["id"].String],
                                                  oVertices[oItem.Value[j].Dictionary["from"].Dictionary["id"].String],
                                                  EdgeType.FanPageNetworkUserCommentSamePost);

                            AddFanPageUserCommentedSamePostEdgeAttributes(oEdge, "Co-Commenter",
                                                                          oItem.Value[i].Dictionary["message"].String,
                                                                          oItem.Value[j].Dictionary["message"].String,
                                                                          GetPostContent(oItem.Key),
                                                                          CreatePostLink(oItem.Key));
                            oEdges.Add(oEdge);
                        }
                    }
                }
            }

            if (Model.Like)
            {
                foreach (var oItem in m_oLikesByPost)
                {
                    for (int i = 0; i < oItem.Value.Count - 1; i++)
                    {
                        for (int j = i + 1; j < oItem.Value.Count; j++)
                        {
                            Edge oEdge = new Edge(oVertices[oItem.Value[i].Dictionary["id"].String],
                                                  oVertices[oItem.Value[j].Dictionary["id"].String],
                                                  EdgeType.FanPageNetworkUserLikeSamePost);
                            AddFanPageUserLikedSamePostEdgeAttributes(oEdge, "Co-Liker",
                                                                      GetPostContent(oItem.Key),
                                                                      CreatePostLink(oItem.Key));
                            oEdges.Add(oEdge);
                        }
                    }
                }
            }

            if (Model.Share)
            {
                //TODO:Implement share
            }
        }

        private void
        CreatePostSameRelationshipEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Comment)
            {
                for (int i = 0; i < m_oCommentsByPost.Count() - 1; i++)
                {
                    for (int j = i + 1; j < m_oCommentsByPost.Count(); j++)
                    {
                        var oIntersectedComments = m_oCommentsByPost.ElementAt(i).Value.Intersect(
                                                    m_oCommentsByPost.ElementAt(j).Value);
                        if (oIntersectedComments != null &&
                            oIntersectedComments.Count() > 0)
                        {
                            Edge oEdge = new Edge(oVertices[m_oCommentsByPost.ElementAt(i).Key],
                                                  oVertices[m_oCommentsByPost.ElementAt(j).Key],
                                                  EdgeType.FanPageNetworkPostSameCommenter);
                            AddFanPagePostHaveSameCommenterEdgeAttributes(oEdge, "Same Commenter", "");
                            oEdges.Add(oEdge);
                        }
                    }
                }
            }

            if (Model.Like)
            {
                for (int i = 0; i < m_oLikesByPost.Count() - 1; i++)
                {
                    for (int j = i + 1; j < m_oLikesByPost.Count(); j++)
                    {
                        var oIntersectedLikes = m_oLikesByPost.ElementAt(i).Value.Intersect(
                                                    m_oLikesByPost.ElementAt(j).Value);
                        if (oIntersectedLikes != null &&
                            oIntersectedLikes.Count() > 0)
                        {
                            Edge oEdge = new Edge(oVertices[m_oLikesByPost.ElementAt(i).Key],
                                                  oVertices[m_oLikesByPost.ElementAt(j).Key],
                                                  EdgeType.FanPageNetworkPostSameLiker);
                            AddFanPagePostHaveSameLikerEdgeAttributes(oEdge, "Same Liker", "");
                            oEdges.Add(oEdge);
                        }
                    }
                }
            }

            if (Model.Share)
            {
                //TODO:Implement share
            }
        }

        private void
        CreateRelationshipPostAuthorEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Comment)
            {
                foreach (var oItem in m_oCommentsByPost)
                {
                    foreach (var oComment in oItem.Value)
                    {
                        Edge oEdge = new Edge(oVertices[oComment.Dictionary["from"].Dictionary["id"].String],
                                                oVertices[m_oPostAuthorPost[oItem.Key]],
                                                EdgeType.FanPageNetworkCommenterPostAuthor);

                        AddFanPageCommenterPostAuthorEdgeAttributes(oEdge, oComment.Dictionary["message"].String,
                                                                    CreateCommentLink(oItem.Key, oComment.Dictionary["id"].String),
                                                                    "Commented Post", GetPostContent(oItem.Key),
                                                                    CreatePostLink(oItem.Key),
                                                                    oComment.Dictionary["like_count"].String,
                                                                    "0",//oComment.Dictionary["comment_count"].String,
                                                                    oComment.Dictionary["created_time"].String);
                        oEdges.Add(oEdge);
                    }
                }
            }

            if (Model.Like)
            {
                foreach (var oItem in m_oLikesByPost)
                {
                    foreach (var oLike in oItem.Value)
                    {
                        Edge oEdge = new Edge(oVertices[oLike.Dictionary["id"].String],
                                                oVertices[m_oPostAuthorPost[oItem.Key]],
                                                EdgeType.FanPageNetworkLikerPostAuthor);
                        AddFanPageLikerPostAuthorEdgeAttributes(oEdge, "Liked Post", GetPostContent(oItem.Key),
                                                                CreatePostLink(oItem.Key));
                        oEdges.Add(oEdge);
                    }
                }

            }

            if (Model.Share)
            {
                //TODO:Implement share
            }

        }

        private void
        CreateConsecutiveRelationshipEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Comment)
            {
                foreach (var oItem in m_oCommentsByPost)
                {
                    for (int i = 0; i < oItem.Value.Count - 1; i++)
                    {
                        for (int j = i + 1; j < oItem.Value.Count; j++)
                        {
                            Edge oEdge = new Edge(oVertices[oItem.Value[j].Dictionary["from"].Dictionary["id"].String],
                                                  oVertices[oItem.Value[i].Dictionary["from"].Dictionary["id"].String],
                                                  EdgeType.FanPageNetworkConsecutiveCommenter);
                            AddFanPageConsecutiveCommenterEdgeAttributes(oEdge, "Consecutive Commenters",
                                                                        GetPostContent(oItem.Key),
                                                                        CreatePostLink(oItem.Key));
                            oEdges.Add(oEdge);
                        }
                    }
                }
            }

            if (Model.Like)
            {
                //TODO:Check if needed to be implemented
            }

            if (Model.Share)
            {
                //TODO:Check if needed to be implemented
            }
        }

        private void
        CreateRelationshipCommentAuthorEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            if (Model.Comment)
            {
                foreach (var oItem in m_oCommentComments)
                {
                    JSONObject oParentComment = m_oComments.FirstOrDefault(
                                            x => x.Dictionary["id"].String == oItem.Dictionary["object_id"].String);

                    if (oParentComment != null)
                    {
                        Edge oEdge = new Edge(oVertices[oItem.Dictionary["from"].Dictionary["id"].String],
                                            oVertices[oParentComment.Dictionary["from"].Dictionary["id"].String],
                                            EdgeType.FanPageNetworkCommenterCommentAuthor);
                        //TODO: FIx attributes
                        AddFanPageCommenterCommentAuthorEdgeAttributes(oEdge, oItem.Dictionary["message"].String,
                                                                    CreateCommentLink(oParentComment.Dictionary["object_id"].String, oItem.Dictionary["id"].String),
                                                                    "Commented Comment", GetPostContent(oParentComment.Dictionary["object_id"].String),
                                                                    CreatePostLink(oParentComment.Dictionary["object_id"].String),
                                                                    oParentComment.Dictionary["message"].String,
                                                                    CreateCommentLink(oParentComment.Dictionary["object_id"].String, oParentComment.Dictionary["id"].String),
                                                                    oItem.Dictionary["created_time"].String);
                        oEdges.Add(oEdge);
                    }

                }
            }

            if (Model.Like)
            {
                foreach (var oItem in m_oCommentLikes)
                {
                    JSONObject oParentComment = m_oComments.FirstOrDefault(
                                            x => x.Dictionary["id"].String == oItem.Dictionary["object_id"].String);

                    //foreach (var oLike in oItem.Value)
                    //{
                    Edge oEdge = new Edge(oVertices[oItem.Dictionary["id"].String],
                                            oVertices[oParentComment.Dictionary["from"].Dictionary["id"].String],
                                            EdgeType.FanPageNetworkLikerCommentAuthor);

                    //FIx Attributes
                    AddFanPageLikerCommentAuthorEdgeAttributes(oEdge,
                                    CreateCommentLink(oParentComment.Dictionary["object_id"].String, oParentComment.Dictionary["id"].String),
                                    oParentComment.Dictionary["message"].String, "Liked Comment",
                                    GetPostContent(oParentComment.Dictionary["object_id"].String),
                                    CreatePostLink(oParentComment.Dictionary["object_id"].String));
                    oEdges.Add(oEdge);
                    //}
                }

            }

            if (Model.Share)
            {
                //TODO:Implement share
            }

        }

        private void
        CreateAuthorPostEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        )
        {
            foreach (JSONObject oPost in m_oPosts)
            {
                Edge oEdge = new Edge(oVertices[oPost.Dictionary["from"].Dictionary["id"].String],
                                      oVertices[oPost.Dictionary["id"].String],
                                      EdgeType.FanPageNetworkAuthorPost);

                AddFanPageAuthorPostEdgeAttributes(oEdge, "Authored",
                                                GetPostContent(oPost),
                                                CreatePostLink(oPost.Dictionary["id"].String));

                oEdges.Add(oEdge);
            }
        }

        private void
        AddFanPageUserCreatedPostEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink,
            string sTime,
            string sTotalLikes,
            string sTotalComments
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
            oEdge.Attributes["time"] = sTime; //DateUtil.ConvertToDateTime(sTime).ToString();
            oEdge.Attributes["e_likes"] = sTotalLikes;
            oEdge.Attributes["e_comments"] = sTotalComments;
        }

        private void
        AddFanPageUserLikedSamePostEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageUserCommentedSamePostEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sVertex1Comment,
            string sVertex2Comment,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["edge_comment"] = String.Format("{0}:\n{1}\n\n{2}:\n{3}",
                                                            oEdge.Vertex1.Name, sVertex1Comment,
                                                            oEdge.Vertex2.Name, sVertex2Comment);
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageUserSharedSamePostEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPagePostHaveSameLikerEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPagePostHaveSameCommenterEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPagePostHaveSameSharerEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageLikerPostAuthorEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageCommenterPostAuthorEdgeAttributes
        (
            Edge oEdge,
            string sComment,
            string sCommentURL,
            string sRelationship,
            string sPostContent,
            string sPostURL,
            string sLikes,
            string sComments,
            string sTimestamp
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["edge_comment"] = sComment;
            oEdge.Attributes["comment_url"] = sCommentURL;
            oEdge.Attributes["post_content"] = sPostContent;
            oEdge.Attributes["post_url"] = sPostURL;
            oEdge.Attributes["no_likes"] = sLikes;
            oEdge.Attributes["no_comments"] = sComments;
            oEdge.Attributes["time"] = sTimestamp; //DateUtil.ConvertToDateTime(sTimestamp).ToString();
        }

        private void
        AddFanPageSharerPostAuthorEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageLikerCommentAuthorEdgeAttributes
        (
            Edge oEdge,
            string sPriorCommentURL,
            string sPriorCommentContent,
            string sRelationship,
            string sPostContent,
            string sPostURL
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["prior_comment"] = sPriorCommentContent;
            oEdge.Attributes["prior_comment_url"] = sPriorCommentURL;
            oEdge.Attributes["post_content"] = sPostContent;
            oEdge.Attributes["post_url"] = sPostURL;
        }

        private void
        AddFanPageCommenterCommentAuthorEdgeAttributes
        (
            Edge oEdge,
            string sComment,
            string sCommentURL,
            string sRelationship,
            string sPostContent,
            string sPostURL,
            string sPriorCommentContent,
            string sPriorCommentURL,
            string sTimestamp
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["edge_comment"] = sComment;
            oEdge.Attributes["comment_url"] = sCommentURL;
            oEdge.Attributes["post_content"] = sPostContent;
            oEdge.Attributes["post_url"] = sPostURL;
            oEdge.Attributes["prior_comment"] = sPriorCommentContent;
            oEdge.Attributes["prior_comment_url"] = sPriorCommentURL;
            oEdge.Attributes["time"] = sTimestamp; //DateUtil.ConvertToDateTime(sTimestamp).ToString();
        }

        private void
        AddFanPageConsecutiveCommenterEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private void
        AddFanPageAuthorPostEdgeAttributes
        (
            Edge oEdge,
            string sRelationship,
            string sPost,
            string sLink
        )
        {
            oEdge.Attributes["relationship"] = sRelationship;
            oEdge.Attributes["post_content"] = sPost;
            oEdge.Attributes["post_url"] = sLink;
        }

        private string
        ManageDuplicateName
        (
            string sVertexName,
            ref VertexCollection oVertices
        )
        {
            Vertex oMatchingVertex = oVertices.FirstOrDefault(x => x.Name == sVertexName);
            int i = 1;
            while (oMatchingVertex != null)
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

            if (oPost != null && oPost.Dictionary.ContainsKey("message"))
            {
                sMessage = oPost.Dictionary["message"].String;
            }

            return sMessage;
        }

        private string
        GetPostContent
        (
            JSONObject oPost
        )
        {
            string sMessage = "";

            if (oPost != null && oPost.Dictionary.ContainsKey("message"))
            {
                sMessage = oPost.Dictionary["message"].String;
            }

            return sMessage;
        }

        private string
        CreatePostLink
        (
            string sPostId
        )
        {
            return "https://www.facebook.com/" + sPostId;
        }

        private string
        CreateCommentLink
        (
            string sPostId,
            string sCommentId
        )
        {
            String[] oCommentId = sCommentId.Split('_');
            sCommentId = oCommentId.Length > 1 ? oCommentId[1] : sCommentId;

            return "https://www.facebook.com/" + sPostId + "/?comment_id=" + sCommentId;
        }

        private string
        GetLikeCount
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary.ContainsKey("likes"))
            {
                return oPost.Dictionary["likes"].Dictionary["summary"].Dictionary["total_count"].Integer.ToString();
            }

            return "";
        }

        private string
        GetCommentCount
        (
            JSONObject oPost
        )
        {
            if (oPost.Dictionary.ContainsKey("comments"))
            {
                return oPost.Dictionary["comments"].Dictionary["summary"].Dictionary["total_count"].Integer.ToString();
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

        private void
        GroupLikesAndComments
        (
        )
        {
            //Group comments by post id
            m_oCommentsByPost = (from c in m_oComments
                                 group c by c.Dictionary["object_id"].String into g
                                 select new { PostID = g.Key, Comments = g.ToList() })
                                 .ToDictionary(x => x.PostID, x => x.Comments);

            //Group likes by post id
            m_oLikesByPost = (from c in m_oLikes
                              group c by c.Dictionary["object_id"].String into g
                              select new { PostID = g.Key, Likes = g.ToList() })
                               .ToDictionary(x => x.PostID, x => x.Likes);

            //Group comments by user id
            m_oCommentsByUser = (from c in m_oComments
                                 group c by c.Dictionary["from"].Dictionary["id"].String into g
                                 select new { PostID = g.Key, Comments = g.ToList() })
                                 .ToDictionary(x => x.PostID, x => x.Comments);

            //Group likes by user id
            m_oLikesByUser = (from c in m_oLikes
                              group c by c.Dictionary["id"].String into g
                              select new { PostID = g.Key, Likes = g.ToList() })
                               .ToDictionary(x => x.PostID, x => x.Likes);
        }

        private int
        GetLikesReceived
        (
            string sUserID
        )
        {
            int iLikes = 0;

            //Count post likes
            if (m_oLikesByUser.ContainsKey(sUserID))
            {
                var oPostIDs = m_oPostAuthorPost.Where(x => x.Value == sUserID);

                if (oPostIDs != null)
                {
                    foreach (var oPostId in oPostIDs)
                    {
                        try
                        {
                            iLikes += m_oLikesByPost[oPostId.Key].Count;
                        }
                        catch (Exception)
                        {
                            //Do Nothing.
                        }
                    }
                }
            }

            //Count comment likes
            if (m_oCommentsByUser.ContainsKey(sUserID) &&
                Model.RelationshipCommentAuthor)
            {
                List<string> oUserComments = m_oCommentsByUser[sUserID].Select(x => x.Dictionary["id"].String).ToList();

                if (oUserComments != null)
                {
                    //iLikes += oUserComments.Intersect(m_oCommentLikes.Keys).Count();
                }
            }

            return iLikes;

        }

        private int
        GetLikesCreated
        (
            string sUserID
        )
        {
            int iLikes = 0;

            //Count post likes
            if (m_oLikesByUser.ContainsKey(sUserID))
            {
                iLikes = m_oLikesByUser[sUserID].Count;
            }

            //Count comment likes
            if (Model.RelationshipCommentAuthor)
            {
                //iLikes += m_oCommentLikes.SelectMany(x => x.Value.Where(y => y.Dictionary["id"].String == sUserID)).Count();
            }

            return iLikes;
        }

        private int
        GetCommentsReceived
        (
            string sUserID
        )
        {
            int iComments = 0;

            if (m_oCommentsByUser.ContainsKey(sUserID))
            {
                //Count post comment
                var oPostIDs = m_oPostAuthorPost.Where(x => x.Value == sUserID);

                if (oPostIDs != null)
                {
                    foreach (var oPostId in oPostIDs)
                    {
                        try
                        {
                            iComments += m_oCommentsByPost[oPostId.Key].Count;
                        }
                        catch (Exception e)
                        {
                            //Do Nothing.
                        }
                    }
                }

                //Count comment comments
                if (Model.RelationshipCommentAuthor)
                {
                    List<string> oUserComments = m_oCommentsByUser[sUserID].Select(x => x.Dictionary["id"].String).ToList();

                    if (oUserComments != null)
                    {
                        iComments += oUserComments.Intersect(m_oCommentComments.Select(x => x.Dictionary["object_id"].String)).Count();
                    }
                }

            }

            return iComments;

        }

        private int
        GetCommentsCreated
        (
            string sUserID
        )
        {
            int iComments = 0;

            //Count post comments
            if (m_oCommentsByUser.ContainsKey(sUserID))
            {
                iComments = m_oCommentsByUser[sUserID].Count;
            }

            //Count comment comments
            if (Model.RelationshipCommentAuthor)
            {
                iComments += m_oCommentComments.Where(x => x.Dictionary["from"].Dictionary["id"].String == sUserID).Count();
            }

            return iComments;
        }

        protected FacebookFanPageGroupModelBase Model
        {
            get { return (FacebookFanPageGroupModelBase)m_oModel; }
        }

    }
}
