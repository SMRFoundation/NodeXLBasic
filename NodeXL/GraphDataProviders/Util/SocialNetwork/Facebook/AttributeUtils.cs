using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Smrf.AppLib
{
    public static class AttributeUtils
    {
        public struct Attribute
        {
            public string name
            {
                get;
                private set;
            }

            public string value
            {
                get;
                private set;
            }

            public Attribute(string name, string value) : this()
            {
                this.name = name;
                this.value = value;
            }
        }

        public static List<Attribute> FacebookSelectableUserAttributes = new List<Attribute>()
        {            
            new Attribute("Name","name"),
            new Attribute("First Name","first_name"),
            new Attribute("Middle Name","middle_name"),
            new Attribute("Last Name","last_name"),
            new Attribute("Hometown","hometown"),
            new Attribute("Location","location"),
            new Attribute("Birthday","birthday"),
            new Attribute("Age Range","age_range"),
            new Attribute("Picture","picture"),
            new Attribute("Email", "email"),
            new Attribute("Timezone","timezone"),
            new Attribute("Gender","gender"),
            new Attribute("Religion","religion"),            
            new Attribute("Relationship","relationship_status"),
            new Attribute("Political Views","political"),
            new Attribute("Favorite Athletes","favorite_athletes"),
            new Attribute("Favorite Teams","favorite_teams"),
            new Attribute("Inspirational People","inspirational_people"),
            new Attribute("Languages","languages"),
            new Attribute("Significant Other","significant_other"),
            new Attribute("Books","books"),
            new Attribute("Quotes","quotes"),
            new Attribute("About Me","about"),
            new Attribute("Work","work"),
            new Attribute("Locale","locale"),
            new Attribute("Website","website"),
            new Attribute("Education","education"),
            new Attribute("Bio","bio"),
            new Attribute("Devices","devices"),
        };

        public static List<Attribute> FacebookExtraUserAttributes = new List<Attribute>()
        {            
            new Attribute("ID","id"),
            new Attribute("Link","link")
        };

        public static List<Attribute> VertexUserAttributes = new List<Attribute>()
        {
            new Attribute("Name","name"),
            new Attribute("First Name","first_name"),
            new Attribute("Middle Name","middle_name"),
            new Attribute("Last Name","last_name"),
            new Attribute("Hometown","hometown"),
            new Attribute("Location","location"),
            new Attribute("Birthday","birthday"),
            new Attribute("Age Range","age_range"),
            new Attribute("Picture","picture"),
            new Attribute("Email", "email"),
            new Attribute("Timezone","timezone"),
            new Attribute("Gender","gender"),
            new Attribute("Religion","religion"),            
            new Attribute("Relationship","relationship_status"),
            new Attribute("Political Views","political"),
            new Attribute("Favorite Athletes","favorite_athletes"),
            new Attribute("Favorite Teams","favorite_teams"),
            new Attribute("Inspirational People","inspirational_people"),
            new Attribute("Languages","languages"),
            new Attribute("Significant Other","significant_other"),
            new Attribute("Books","books"),
            new Attribute("Quotes","quotes"),
            new Attribute("About Me","about"),
            new Attribute("Work","work"),
            new Attribute("Locale","locale"),
            new Attribute("Website","website"),
            new Attribute("Education","education"),
            new Attribute("Bio","bio"),
            new Attribute("Devices","devices"),
            new Attribute("Vertex Type","vertex_type"),
            new Attribute("User Type","user_type"),
            new Attribute("Tweet","comment"),
            new Attribute("Tooltip","Tooltip"),
            new Attribute("Label","Label"),
        };

        public static List<Attribute> StatusUpdatesUserAttributes = new List<Attribute>()
        {
            new Attribute("Status Updates","statuses"),
            new Attribute("Tagged Users (Status Updates)","statuses_tags"),
            new Attribute("URLs (Status Updates)","statuses_urls"),
            new Attribute("Hashtags (Status Updates)","statuses_hashtags"),             
        };

        public static List<Attribute> WallPostsUserAttributes = new List<Attribute>()
        {
            new Attribute("Wall Posts","wall_posts"),
            new Attribute("Tagged Users (Wall Posts)","wall_tags"),
            new Attribute("URLs (Wall Posts)","wall_urls"),
            new Attribute("Hashtags (Wall Posts)","wall_hashtags"),
        };

        public static List<Attribute> FanPageUserAttributes = new List<Attribute>()
        {
            new Attribute("Name","name"),
            new Attribute("First Name","first_name"),
            new Attribute("Middle Name","middle_name"),
            new Attribute("Last Name","last_name"),
            new Attribute("Hometown","hometown"),
            new Attribute("Location","location"),
            new Attribute("Birthday","birthday"),
            new Attribute("Age Range","age_range"),
            new Attribute("Picture","picture"),
            new Attribute("Email", "email"),
            new Attribute("Timezone","timezone"),
            new Attribute("Gender","gender"),
            new Attribute("Religion","religion"),            
            new Attribute("Relationship","relationship_status"),
            new Attribute("Political Views","political"),
            new Attribute("Favorite Athletes","favorite_athletes"),
            new Attribute("Favorite Teams","favorite_teams"),
            new Attribute("Inspirational People","inspirational_people"),
            new Attribute("Languages","languages"),
            new Attribute("Significant Other","significant_other"),
            new Attribute("Books","books"),
            new Attribute("Quotes","quotes"),
            new Attribute("About Me","about"),
            new Attribute("Work","work"),
            new Attribute("Locale","locale"),
            new Attribute("Website","website"),
            new Attribute("Education","education"),
            new Attribute("Bio","bio"),
            new Attribute("Devices","devices"),
            new Attribute("Vertex Type","vertex_type"),
            new Attribute("User Type","user_type"),
            new Attribute("Tweet","comment"),
            new Attribute("Tooltip","Tooltip"),
            new Attribute("Label","Label"),
            new Attribute("Likes Received","likes_received"),
            new Attribute("Likes Created","likes_created"),
            new Attribute("Comments Received","comments_received"),
            new Attribute("Comments Created","comments_created"),
        };

        public static List<Attribute> PostAttributes = new List<Attribute>()
        {
            new Attribute("Content","content"),
            new Attribute("Tooltip","Tooltip"),
            new Attribute("Label","Label"),
            new Attribute("Vertex Type","vertex_type"),
            new Attribute("Image","image"),
            new Attribute("Post URL","post_link"),
            new Attribute("Total Likes","likes"),
            new Attribute("Total Comments","comments"),
        };

        public static List<Attribute> UserNetworkEdgeAttributes = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Tweet","edge_comment"),            
            new Attribute("Feed Of Origin","feed_of_origin"),
            new Attribute("Timestamp","timestamp"),
            new Attribute("Share Count","share_count"),
            new Attribute("Comment Likes","comment_likes"),            
        };

        #region "Fan Page Edge Attributes"

        public static List<Attribute> FanPageUserCreatedPost = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
            new Attribute("Time","time"),
            new Attribute("Total Likes","e_likes"),
            new Attribute("Total Comments","e_comments"),
        };

        public static List<Attribute> FanPageUserLikedSamePost = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPageUserCommentedSamePost = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Tweet","edge_comment"),
            new Attribute("Post Content","post_content"),   
            new Attribute("Post URL","post_url"),
        };


        public static List<Attribute> FanPageUserSharedSamePost = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPagePostHaveSameLiker = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post URL","post_url"),
            new Attribute("Liker","post_liker"),
        };

        public static List<Attribute> FanPagePostHaveSameCommenter = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post URL","post_url"),
            new Attribute("Post1 Comments","post1_comments"),
            new Attribute("Post2 Comments","post2_comments"),
        };

        public static List<Attribute> FanPagePostHaveSameSharer = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post URL","post_url"),            
        };

        public static List<Attribute> FanPageLikerPostAuthor = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPageCommenterPostAuthor = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),            
            new Attribute("Comment Likes","no_likes"),
            new Attribute("Comment Comments","no_comments"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
            new Attribute("Tweet","edge_comment"),
            new Attribute("Comment URL","comment_url"),
            new Attribute("Time","time"),
        };

        public static List<Attribute> FanPageSharerPostAuthor = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPageConsecutiveCommenter = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPageUserLikerComment = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
            new Attribute("Prior Comment","prior_comment"),
            new Attribute("Prior Comment URL","prior_comment_url"),
        };

        public static List<Attribute> FanPageUserCommenterComment = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),            
            new Attribute("Comment Likes","no_likes"),            
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
            new Attribute("Tweet","edge_comment"),
            new Attribute("Comment URL","comment_url"),
            new Attribute("Prior Comment","prior_comment"),
            new Attribute("Prior Comment URL","prior_comment_url"),
            new Attribute("Time","time"),
        };

        public static List<Attribute> FanPageUserSharerComment = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),
        };

        public static List<Attribute> FanPageAuthorPost = new List<Attribute>()
        {            
            new Attribute("Type", "e_type"), //Filled on edge creation
            new Attribute("Relationship","relationship"),
            new Attribute("Post Content","post_content"),
            new Attribute("Post URL","post_url"),            
        };

        #endregion

    }
}
