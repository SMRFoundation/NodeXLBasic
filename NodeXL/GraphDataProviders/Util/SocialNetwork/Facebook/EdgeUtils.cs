using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.NodeXL.GraphDataProviders.Facebook;

namespace Smrf.AppLib
{
    public static class EdgeUtils
    {
        public static Dictionary<EdgeType, string> FriendlyName = new Dictionary<EdgeType, string>()
        {
            {EdgeType.UserNetworkFriends, "Friend"},
            {EdgeType.UserNetworkUserTagged,"User Tagged"},
            {EdgeType.UserNetworkPostAuthor, "Post Author"},
            {EdgeType.UserNetworkCommenter, "Commenter"},
            {EdgeType.UserNetworkLiker, "Liker"},
            {EdgeType.UserNetworkEgo, "Ego"},
            {EdgeType.FanPageNetworkUserCreatedPost, "User Created Post"},
            {EdgeType.FanPageNetworkUserCommentSamePost, "User Commented Same Post"},
            {EdgeType.FanPageNetworkUserLikeSamePost, "User Liked Same Post"},
            {EdgeType.FanPageNetworkUserShareSamePost, "User Shared Same Post"},
            {EdgeType.FanPageNetworkPostSameCommenter, "Post Same Commenter"},
            {EdgeType.FanPageNetworkPostSameLiker, "Post Same Liker"},
            {EdgeType.FanPageNetworkPostSameSharer, "Post Sam Sharer"},
            {EdgeType.FanPageNetworkCommenterPostAuthor, "User Commented Post"},
            {EdgeType.FanPageNetworkLikerPostAuthor, "User Liked Post"},
            {EdgeType.FanPageNetworkSharerPostAuthor, "User Shared Post"},
            {EdgeType.FanPageNetworkConsecutiveCommenter, "Consecutive Commenter"},
            {EdgeType.FanPageNetworkConsecutiveLiker, "Consecutive Liker"},
            {EdgeType.FanPageNetworkConsecutiveSharer, "Consecutive Sharer"},
            {EdgeType.FanPageNetworkAuthorPost, "Post Author"},
            {EdgeType.FanPageNetworkCommenterCommentAuthor, "User Commented Comment"},
            {EdgeType.FanPageNetworkLikerCommentAuthor, "User Liked Comment"},
        };
    }
}
