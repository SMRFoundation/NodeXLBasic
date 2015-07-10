using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookFanPageModel : FacebookFanPageGroupModelBase
    {
        //General
        private string m_sFanPageID;
        private string m_sFanPageName; //can be deleted        

        //Properties
        public string FanPageID
        {
            get { return m_sFanPageID; }
            set { m_sFanPageID = value; }
        }


        public string FanPageName
        {
            get { return m_sFanPageName; }
            set { m_sFanPageName = value; }
        }

    }
}
