using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public enum EdgeType
    {
        UserNetworkFriends,
        UserNetworkUserTagged,
        UserNetworkPostAuthor,
        UserNetworkCommenter,
        UserNetworkLiker,
        UserNetworkEgo,
        FanPageNetworkUserCreatedPost,
        FanPageNetworkUserCommentSamePost,
        FanPageNetworkUserLikeSamePost,
        FanPageNetworkUserShareSamePost,
        FanPageNetworkPostSameCommenter,
        FanPageNetworkPostSameLiker,
        FanPageNetworkPostSameSharer,
        FanPageNetworkCommenterPostAuthor,
        FanPageNetworkLikerPostAuthor,
        FanPageNetworkSharerPostAuthor,
        FanPageNetworkConsecutiveCommenter,
        FanPageNetworkConsecutiveLiker,
        FanPageNetworkConsecutiveSharer,
        FanPageNetworkCommenterCommentAuthor,
        FanPageNetworkLikerCommentAuthor,
        FanPageNetworkSharerCommentAuthor,
        FanPageNetworkAuthorPost,
    }
}
