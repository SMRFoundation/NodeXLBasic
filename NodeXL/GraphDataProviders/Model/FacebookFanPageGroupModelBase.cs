using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookFanPageGroupModelBase : FacebookModelBase
    {
        //Vertices
        private bool m_bUser;
        private bool m_bPost;

        //Relationship
        private bool m_bLike;
        private bool m_bComment;
        private bool m_bShare;

        //Edges
        private bool m_bUserRelationshipSamePost;
        private bool m_bPostSameRelationship;
        private bool m_bRelationshipPostAuthor;
        private bool m_bConsecutiveRelationship;
        private bool m_bRelationshipCommentAuthor;


        //Options        
        private bool m_bIncludePostsByOthers;
        private bool m_bGetStatusUpdates;

        //Properties
        //Properties        
        public bool IncludePostsByOthers
        {
            get { return m_bIncludePostsByOthers; }
            set { m_bIncludePostsByOthers = value; }
        }


        public bool GetStatusUpdates
        {
            get { return m_bGetStatusUpdates; }
            set { m_bGetStatusUpdates = value; }
        }

        public bool UserRelationshipSamePost
        {
            get { return m_bUserRelationshipSamePost; }
            set { m_bUserRelationshipSamePost = value; }
        }

        public bool PostSameRelationship
        {
            get { return m_bPostSameRelationship; }
            set { m_bPostSameRelationship = value; }
        }

        public bool RelationshipPostAuthor
        {
            get { return m_bRelationshipPostAuthor; }
            set { m_bRelationshipPostAuthor = value; }
        }

        public bool RelationshipCommentAuthor
        {
            get { return m_bRelationshipCommentAuthor; }
            set { m_bRelationshipCommentAuthor = value; }
        }

        public bool ConsecutiveRelationship
        {
            get { return m_bConsecutiveRelationship; }
            set { m_bConsecutiveRelationship = value; }
        }

        public bool Like
        {
            get { return m_bLike; }
            set { m_bLike = value; }
        }


        public bool Comment
        {
            get { return m_bComment; }
            set { m_bComment = value; }
        }


        public bool Share
        {
            get { return m_bShare; }
            set { m_bShare = value; }
        }

        public bool User
        {
            get { return m_bUser; }
            set { m_bUser = value; }
        }


        public bool Post
        {
            get { return m_bPost; }
            set { m_bPost = value; }
        }
    }
}
