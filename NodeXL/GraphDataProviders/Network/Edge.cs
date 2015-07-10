using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class Edge
    {
        public Vertex Vertex1 { get; set; }        
        public Vertex Vertex2 { get; set; }
        public EdgeType Type { get; set; }
        public EdgeDirection Direction { get; set; }
        public AttributesDictionary<string> Attributes {get; set;}

        public Edge
        (
            Vertex Vertex1,
            Vertex Vertex2,
            EdgeType Type
        )
        {
            this.Vertex1 = Vertex1;
            this.Vertex2 = Vertex2;
            this.Type = Type;
            //TODO: Add attributes in base of edge type
            switch (Type)
            {
                case EdgeType.FanPageNetworkUserCreatedPost:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserCreatedPost);
                    break;
                case EdgeType.FanPageNetworkUserLikeSamePost:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserLikedSamePost);
                    break;
                case EdgeType.FanPageNetworkUserCommentSamePost:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserCommentedSamePost);
                    break;
                case EdgeType.FanPageNetworkUserShareSamePost:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserSharedSamePost);
                    break;
                case EdgeType.FanPageNetworkPostSameLiker:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPagePostHaveSameLiker);
                    break;
                case EdgeType.FanPageNetworkPostSameCommenter:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPagePostHaveSameCommenter);
                    break;
                case EdgeType.FanPageNetworkPostSameSharer:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPagePostHaveSameSharer);
                    break;
                case EdgeType.FanPageNetworkLikerPostAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageLikerPostAuthor);
                    break;
                case EdgeType.FanPageNetworkCommenterPostAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageCommenterPostAuthor);
                    break;
                case EdgeType.FanPageNetworkSharerPostAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageSharerPostAuthor);
                    break;
                case EdgeType.FanPageNetworkConsecutiveCommenter:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageConsecutiveCommenter);
                    break;
                case EdgeType.FanPageNetworkLikerCommentAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserLikerComment);
                    break;
                case EdgeType.FanPageNetworkCommenterCommentAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserCommenterComment);
                    break;
                case EdgeType.FanPageNetworkSharerCommentAuthor:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageUserSharerComment);
                    break;
                case EdgeType.FanPageNetworkAuthorPost:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.FanPageAuthorPost);
                    break;
                default:
                    Attributes = new AttributesDictionary<string>(AttributeUtils.UserNetworkEdgeAttributes);
                    break;
            }
            //Attributes = new AttributesDictionary<string>(AttributeUtils.UserNetworkEdgeAttributes);
            Attributes["e_type"] = EdgeUtils.FriendlyName[Type];
        }

        //public Edge(Vertex Vertex1, Vertex Vertex2, string Type,
        //            string Relationship, string Comment, int Weight,
        //            int Timestamp, int ShareCount, int CommentLikes,
        //            EdgeDirection eDirection)
        //{
        //    this.Vertex1 = Vertex1;
        //    this.Vertex2 = Vertex2;
        //    this.Type = Type;
        //    this.Relationship = Relationship;
        //    this.Comment = Comment;
        //    this.Weight = Weight;
        //    this.ShareCount = ShareCount;
        //    this.CommentLikes = CommentLikes;
        //    this.FeedOfOrigin = "";
        //    if(Timestamp>0)
        //        this.Timestamp = DateUtil.ConvertToDateTime(Timestamp);
        //    this.Direction = eDirection;
        //}

        //public Edge(Vertex Vertex1, Vertex Vertex2, string Type,
        //            string Relationship, string Comment, int Weight,
        //            int Timestamp, int ShareCount, int CommentLikes,
        //            string FeedOfOrigin, EdgeDirection eDirection)
        //{
        //    this.Vertex1 = Vertex1;
        //    this.Vertex2 = Vertex2;
        //    this.Type = Type;
        //    this.Relationship = Relationship;
        //    this.Comment = Comment;
        //    this.Weight = Weight;
        //    this.ShareCount = ShareCount;
        //    this.CommentLikes = CommentLikes;
        //    this.FeedOfOrigin = FeedOfOrigin;
        //    if (Timestamp > 0)
        //        this.Timestamp = DateUtil.ConvertToDateTime(Timestamp);
        //    this.Direction = eDirection;
        //}



        public override int GetHashCode()
        {
            return (Vertex1.GetHashCode() + Vertex2.GetHashCode() + Type.GetHashCode() + Attributes.GetHashCode());
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }
        private bool Equals(Edge obj)
        {            
            bool bAttributesEqual = true;
            foreach (KeyValuePair<AttributeUtils.Attribute, string> oAttribute in this.Attributes)
            {
                if (!obj.Attributes.ContainsKey(oAttribute.Key) ||
                    obj.Attributes[oAttribute.Key] != oAttribute.Value)
                {
                    bAttributesEqual = false;
                    break;
                }
            }
            return (obj != null &&
                    obj.Vertex1.Equals(this.Vertex1) &&
                    obj.Vertex2.Equals(this.Vertex2) &&
                    obj.Type.Equals(this.Type) && bAttributesEqual);
            
        }



    }
}
