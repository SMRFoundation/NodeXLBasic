using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookModelBase
    {
        //General        
        protected AttributesDictionary<bool> m_oAttributes;

        //Options
        protected bool m_bDownloadFromPost;
        protected int m_iFromPost = -1;
        protected int m_iToPost = -1;
        protected bool m_bDownloadPostsBetweenDates;
        protected DateTime m_oFromDate;
        protected DateTime m_oToDate;
        protected bool m_bLimit;
        protected int m_iLimitAmount;
                
        //Properties
        public int LimitAmount
        {
            get { return m_iLimitAmount; }
            set { m_iLimitAmount = value; }
        }

        public bool DownloadFromPost
        {
            get { return m_bDownloadFromPost; }
            set { m_bDownloadFromPost = value; }
        }

        public int FromPost
        {
            get { return m_iFromPost; }
            set { m_iFromPost = value; }
        }

        public int ToPost
        {
            get { return m_iToPost; }
            set { m_iToPost = value; }
        }

        public bool DownloadPostsBetweenDates
        {
            get { return m_bDownloadPostsBetweenDates; }
            set { m_bDownloadPostsBetweenDates = value; }
        }

        public DateTime FromDate
        {
            get { return m_oFromDate; }
            set { m_oFromDate = value; }
        }

        public DateTime ToDate
        {
            get { return m_oToDate; }
            set { m_oToDate = value; }
        }

        public bool Limit
        {
            get { return m_bLimit; }
            set { m_bLimit = value; }
        }

        public AttributesDictionary<bool> Attributes
        {
            get { return m_oAttributes; }
            set { m_oAttributes = value; }
        }
    }
}
