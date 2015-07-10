using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class FacebookGroupModel : FacebookFanPageGroupModelBase
    {
        //General
        private string m_sGroupID;
        private string m_sGroupName; //can be deleted         

        //Properties
        public string GroupID
        {
            get { return m_sGroupID; }
            set { m_sGroupID = value; }
        }


        public string GroupName
        {
            get { return m_sGroupName; }
            set { m_sGroupName = value; }
        }

    }
}
