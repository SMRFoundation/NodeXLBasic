using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.SocialNetworkLib
{
    class RelationshipNaming
    {
        private string m_sRelationship;
        private string m_sNoun;
        private string m_sVerb;

        public string Relationship
        {
            get { return m_sRelationship; }
            set { m_sRelationship = value; }
        }

        public string Noun
        {
            get { return m_sNoun; }
            set { m_sNoun = value; }
        }

        public string Verb
        {
            get { return m_sVerb; }
            set { m_sVerb = value; }
        }
    }
}
