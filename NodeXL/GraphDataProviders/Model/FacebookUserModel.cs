using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookUserModel : FacebookModelBase
    {
        //Edges        
        private bool m_bPostAuthor;
        private bool m_bUserTagged;
        private bool m_bComment;
        private bool m_bLike;

        //Options                
        private bool m_bGetTooltips;
        private bool m_bIncludeMe;
        
        //Properties
        public bool IncludeMe
        {
            get { return m_bIncludeMe; }
            set { m_bIncludeMe = value; }
        }
        

        public bool GetTooltips
        {
            get { return m_bGetTooltips; }
            set { m_bGetTooltips = value; }
        }        

        public bool PostAuthor
        {
            get { return m_bPostAuthor; }
            set { m_bPostAuthor = value; }
        }

        public bool UserTagged
        {
            get { return m_bUserTagged; }
            set { m_bUserTagged = value; }
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

    }
}
