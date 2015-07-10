using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public abstract class FacebookNetworkCreatorBase
    {
        protected FacebookModelBase m_oModel;
        protected List<JSONObject> m_oPosts;
        protected List<JSONObject> m_oLikes;
        protected List<JSONObject> m_oComments;        
        protected Dictionary<String, Dictionary<String, JSONObject>> m_oAttributes;
        protected Dictionary<String, String> m_oStatusUpdates;

        public FacebookNetworkCreatorBase
        (
            FacebookModelBase oModel,
            List<JSONObject> oPosts,
            List<JSONObject> oLikes,
            List<JSONObject> oComments,            
            Dictionary<String, Dictionary<String, JSONObject>> oAttributes,
            Dictionary<String, String> oStatusUpdates
        )
        {
            m_oModel = oModel;
            m_oPosts = oPosts;
            m_oLikes = oLikes;
            m_oComments = oComments;            
            m_oAttributes = oAttributes;
            m_oStatusUpdates = oStatusUpdates;
        }

        public void
        CreateNetwork
        (
            out VertexCollection oVertices,
            out EdgeCollection oEdges
        )
        {
            oVertices = new VertexCollection();
            oEdges = new EdgeCollection();

            CreateVertices(ref oVertices);
            CreateEdges(ref oEdges, ref oVertices);
        }

        protected abstract void
        CreateVertices
        (
            ref VertexCollection oVertices
        );

        protected abstract void
        CreateEdges
        (
            ref EdgeCollection oEdges,
            ref VertexCollection oVertices
        );

        protected String
        GetAttributeValue
        (
            String attributeKey,
            JSONObject attributeValue
        )
        {
            if (attributeKey == "picture")
            {
                return attributeValue.Dictionary["data"].Dictionary["url"].String;
            }
            else if (attributeKey == "hometown")
            {
                return attributeValue.Dictionary["name"].String;
            }
            else if (attributeKey == "location")
            {
                return attributeValue.Dictionary["name"].String;
            }
            else if (attributeKey == "age_range")
            {
                String sMin = "", sMax = "";

                if (attributeValue.Dictionary.ContainsKey("min"))
                {
                    sMin = attributeValue.Dictionary["min"].String;
                }

                if (attributeValue.Dictionary.ContainsKey("max"))
                {
                    sMax = " - " + attributeValue.Dictionary["max"].String;
                }

                return sMin + sMax;
            }
            else if (attributeKey == "favorite_athletes")
            {
                return String.Join(", ", attributeValue.Array.Select(x => x.Dictionary["name"].String).ToArray());
            }
            else if (attributeKey == "favorite_teams")
            {
                return String.Join(", ", attributeValue.Array.Select(x => x.Dictionary["name"].String).ToArray());
            }
            else if (attributeKey == "devices")
            {
                return String.Join(", ", attributeValue.Array.Select(x => x.Dictionary["os"].String).ToArray());
            }
            else if (attributeKey == "work")
            {
                return String.Join("\r\n", attributeValue.Array.Select(x =>
                                    x.Dictionary["start_date"].String + " - " +
                                    x.Dictionary["end_date"].String + ":" +
                                    x.Dictionary["position"].String + ", " +
                                    x.Dictionary["employer"].String).ToArray());
            }
            else if (attributeKey == "significant_other")
            {
                return attributeValue.Dictionary["name"].String;
            }
            else if (attributeKey == "languages")
            {
                return String.Join(", ", attributeValue.Array.Select(x => x.Dictionary["name"].String).ToArray());
            }
            else if (attributeKey == "inspirational_people")
            {
                return String.Join(", ", attributeValue.Array.Select(x => x.Dictionary["name"].String).ToArray());
            }
            else if (attributeKey == "education")
            {
                return String.Join(", ", attributeValue.Array.Select(x =>
                                        x.Dictionary["year"].Dictionary["name"].String + " - " +
                                        x.Dictionary["school"].Dictionary["name"].String).ToArray());
            }
            else
            {
                return JSONToString(attributeValue);
            }
        }

        private  String
        JSONToString
        (
            JSONObject oJSONObject
        )
        {
            if (oJSONObject.IsBoolean)
            {
                return oJSONObject.Boolean.ToString();
            }
            else if (oJSONObject.IsInteger)
            {
                return oJSONObject.Integer.ToString();
            }
            else if (oJSONObject.IsString)
            {
                return oJSONObject.String;
            }
            else
            {
                return "";//throw new Exception("JSONObject not a primitive type!");
            }

        }
         
    }
}
