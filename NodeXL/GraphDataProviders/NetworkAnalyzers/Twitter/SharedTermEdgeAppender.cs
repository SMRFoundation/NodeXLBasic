

#if AddExtraEdges

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: SharedTermEdgeAppender
//
/// <summary>
/// Appends edge XML nodes to a GraphML document for terms that were tweeted by
/// pairs of Twitter users.
/// </summary>
//*****************************************************************************

public class SharedTermEdgeAppender : Object
{
    //*************************************************************************
    //  Constructor: SharedTermEdgeAppender()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SharedTermEdgeAppender" />
    /// class.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term that was searched for.  This gets skipped (along with other
    /// stop words) when determining shared words, but included when
    /// determining shared word pairs.
    /// </param>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument that the edge XML nodes will be appended to.
    /// </param>
    ///
    /// <param name="twitterUsers">
    /// Collection of the Twitter users in the network.
    /// </param>
    //*************************************************************************

    public SharedTermEdgeAppender
    (
        String searchTerm,
        GraphMLXmlDocument graphMLXmlDocument,
        IEnumerable<TwitterUser> twitterUsers
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(twitterUsers != null);

        m_oGraphMLXmlDocument = graphMLXmlDocument;
        m_oTwitterUsers = twitterUsers;

        CreateTermCounters(searchTerm);

        AssertValid();
    }

    //*************************************************************************
    //  Method: AppendSharedHashtagEdges()
    //
    /// <summary>
    /// Appends edge XML nodes for shared hashtags.
    /// </summary>
    ///
    /// <remarks>
    /// For each pair of Twitter users who have tweeted the same hashtag, this
    /// method adds an edge XML node to the GraphML document.
    /// </remarks>
    //*************************************************************************

    public void
    AppendSharedHashtagEdges()
    {
        AssertValid();

        AppendSharedTermEdgeXmlNodes(
            GetHashtagsFromStatus,
            GetTopHashtags(),
            "Shared Hashtag",
            TwitterSearchNetworkAnalyzer.SharedHashtagID
            );
    }

    //*************************************************************************
    //  Method: AppendSharedUrlEdges()
    //
    /// <summary>
    /// Appends edge XML nodes for shared URLs.
    /// </summary>
    ///
    /// <remarks>
    /// For each pair of Twitter users who have tweeted the same URL, this
    /// method adds an edge XML node to the GraphML document.
    /// </remarks>
    //*************************************************************************

    public void
    AppendSharedUrlEdges()
    {
        AssertValid();

        AppendSharedTermEdgeXmlNodes(
            GetUrlsFromStatus,
            GetTopUrls(),
            "Shared URL",
            TwitterSearchNetworkAnalyzer.SharedUrlID
            );
    }

    //*************************************************************************
    //  Method: AppendSharedWordEdges()
    //
    /// <summary>
    /// Appends edge XML nodes for shared words.
    /// </summary>
    ///
    /// <remarks>
    /// For each pair of Twitter users who have tweeted the same word, this
    /// method adds an edge XML node to the GraphML document.
    /// </remarks>
    //*************************************************************************

    public void
    AppendSharedWordEdges()
    {
        AssertValid();

        AppendSharedTermEdgeXmlNodes(
            GetWordsFromStatus,
            GetTopWords(),
            "Shared Word",
            TwitterSearchNetworkAnalyzer.SharedWordID
            );
    }

    //*************************************************************************
    //  Method: AppendSharedWordPairEdges()
    //
    /// <summary>
    /// Appends edge XML nodes for shared word pairs.
    /// </summary>
    ///
    /// <remarks>
    /// For each pair of Twitter users who have tweeted the same word pair,
    /// this method adds an edge XML node to the GraphML document.
    /// </remarks>
    //*************************************************************************

    public void
    AppendSharedWordPairEdges()
    {
        AssertValid();

        AppendSharedTermEdgeXmlNodes(
            GetWordPairsFromStatus,
            GetTopWordPairs(),
            "Shared Word Pair",
            TwitterSearchNetworkAnalyzer.SharedWordPairID
            );
    }

    //*************************************************************************
    //  Method: CreateTermCounters()
    //
    /// <summary>
    /// Creates the word and word pair counters.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term that was searched for.  This gets skipped (along with other
    /// stop words) when determining shared words, but included when
    /// determining shared word pairs.
    /// </param>
    //*************************************************************************

    protected void
    CreateTermCounters
    (
        String sSearchTerm
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );

        List<String> oWordsToSkip = new List<String>(
            StringUtil.SplitOnSpaces(
                WordCounter.SampleSpaceDelimitedEnglishStopWords)
            );

        m_oWordPairCounter = new WordPairCounter( oWordsToSkip.ToArray() );

        oWordsToSkip.AddRange( StringUtil.SplitOnSpaces(sSearchTerm) );
        oWordsToSkip.Add("rt");

        m_oWordCounter = new WordCounter( oWordsToSkip.ToArray() );
    }

    //*************************************************************************
    //  Method: AppendSharedTermEdgeXmlNodes()
    //
    /// <summary>
    /// Appends edge XML nodes for shared terms.
    /// </summary>
    ///
    /// <param name="oGetTermsFromStatus">
    /// Method that will get the terms in one Twitter status.
    /// </param>
    ///
    /// <param name="oTopTerms">
    /// Top terms among all statuses.  Edges will be appended only for terms in
    /// this collection.
    /// </param>
    ///
    /// <param name="sRelationshipDescription">
    /// A description of the shared relationship.  Sample: "Shared Hashtag".
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML-attribute ID for the shared term.  Sample: "SharedHashtag".
    /// </param>
    //*************************************************************************

    protected void
    AppendSharedTermEdgeXmlNodes
    (
        GetTermsFromStatus oGetTermsFromStatus,
        HashSet<String> oTopTerms,
        String sRelationshipDescription,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oGetTermsFromStatus != null);
        Debug.Assert(oTopTerms != null);
        Debug.Assert( !String.IsNullOrEmpty(sRelationshipDescription) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        // The key is a term and the value is a collection of the screen names
        // for the unique users who have tweeted that term.

        foreach ( KeyValuePair< String, HashSet<String> > oKeyValuePair in
            GetTermDictionary(oGetTermsFromStatus, oTopTerms) )
        {
            String [] asScreenNames = oKeyValuePair.Value.ToArray();
            Int32 iScreenNames = asScreenNames.Length;

            // Prevent the number of edges from blowing up.

            if (iScreenNames > MaximumTermTweeters)
            {
                continue;
            }

            for (Int32 i = 0; i < iScreenNames; i++)
            {
                for (Int32 j = i + 1; j < iScreenNames; j++)
                {
                    String sScreenNameI = asScreenNames[i];
                    String sScreenNameJ = asScreenNames[j];

                    if (sScreenNameI != sScreenNameJ)
                    {
                        XmlNode oEdgeXmlNode =
                            m_oGraphMLXmlDocument.AppendEdgeXmlNode(
                                sScreenNameI, sScreenNameJ);

                        m_oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                            oEdgeXmlNode,
                            HttpNetworkAnalyzerBase.RelationshipID,
                            sRelationshipDescription);

                        m_oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                            oEdgeXmlNode, sGraphMLAttributeID,
                            oKeyValuePair.Key);
                    }
                }
            }
        }
    }

    //*************************************************************************
    //  Delegate: GetTermsFromStatus()
    //
    /// <summary>
    /// Represents a method that will get the terms in one Twitter status.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <returns>
    /// A collection of the terms found in the status.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected delegate IEnumerable<String>
    GetTermsFromStatus
    (
        TwitterStatus oTwitterStatus
    );

    //*************************************************************************
    //  Method: GetHashtagsFromStatus()
    //
    /// <summary>
    /// Get the hashtags in one Twitter status.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <returns>
    /// A collection of the hashtags found in the status.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<String>
    GetHashtagsFromStatus
    (
        TwitterStatus oTwitterStatus
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        return ( GetHashtagsOrUrlsFromStatus(oTwitterStatus, true) );
    }

    //*************************************************************************
    //  Method: GetUrlsFromStatus()
    //
    /// <summary>
    /// Get the URLs in one Twitter status.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <returns>
    /// A collection of the URLs found in the status.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<String>
    GetUrlsFromStatus
    (
        TwitterStatus oTwitterStatus
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        return ( GetHashtagsOrUrlsFromStatus(oTwitterStatus, false) );
    }

    //*************************************************************************
    //  Method: GetWordsFromStatus()
    //
    /// <summary>
    /// Get the words in one Twitter status.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <returns>
    /// A collection of the words found in the status.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<String>
    GetWordsFromStatus
    (
        TwitterStatus oTwitterStatus
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        m_oWordCounter.Clear();
        m_oWordCounter.CountTermsInDocument(oTwitterStatus.Text);

        return (
            from CountedWord oCountedWord in m_oWordCounter.CountedTerms
            select oCountedWord.Word
            );
    }

    //*************************************************************************
    //  Method: GetWordPairsFromStatus()
    //
    /// <summary>
    /// Get the word pairs in one Twitter status.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <returns>
    /// A collection of the word pairs found in the status.  Sample word pair:
    /// "president obama".
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<String>
    GetWordPairsFromStatus
    (
        TwitterStatus oTwitterStatus
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        m_oWordPairCounter.Clear();
        m_oWordPairCounter.CountTermsInDocument(oTwitterStatus.Text);

        return (
            from CountedWordPair oCountedWordPair in
                m_oWordPairCounter.CountedTerms

            select oCountedWordPair.ToString()
            );
    }

    //*************************************************************************
    //  Method: GetHashtagsOrUrlsFromStatus()
    //
    /// <summary>
    /// Get the hashtags or URLs in one Twitter status as a collection of
    /// strings.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <param name="bGetHashtags">
    /// true to get the hashtags, false to get the URLs.
    /// </param>
    ///
    /// <returns>
    /// A collection of the hashtags or URLs found in the status.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty but it cannot be null.  It may
    /// contain duplicates.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<String>
    GetHashtagsOrUrlsFromStatus
    (
        TwitterStatus oTwitterStatus,
        Boolean bGetHashtags
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        return ( StringUtil.SplitOnSpaces(
            GetSpaceDelimitedHashtagsOrUrlsFromStatus(
                oTwitterStatus, bGetHashtags) ) );
    }

    //*************************************************************************
    //  Method: GetSpaceDelimitedHashtagsOrUrlsFromStatus()
    //
    /// <summary>
    /// Get the hashtags or URLs in one Twitter status as a space-delimited
    /// string.
    /// </summary>
    ///
    /// <param name="oTwitterStatus">
    /// A Twitter status.
    /// </param>
    ///
    /// <param name="bGetHashtags">
    /// true to get the hashtags, false to get the URLs.
    /// </param>
    ///
    /// <returns>
    /// The hashtags or URLs found in the status, delimited by spaces.
    /// </returns>
    ///
    /// <remarks>
    /// The returned string may be empty but it cannot be null.  It may contain
    /// duplicates.
    /// </remarks>
    //*************************************************************************

    protected String
    GetSpaceDelimitedHashtagsOrUrlsFromStatus
    (
        TwitterStatus oTwitterStatus,
        Boolean bGetHashtags
    )
    {
        Debug.Assert(oTwitterStatus != null);
        AssertValid();

        String sHashtagsOrUrls = bGetHashtags ?
            oTwitterStatus.Hashtags : oTwitterStatus.Urls;

        if ( String.IsNullOrEmpty(sHashtagsOrUrls) )
        {
            sHashtagsOrUrls = String.Empty;
        }

        if (bGetHashtags)
        {
            // Two hashtags with different casing are considered the
            // same hashtag.  This is not true for URLs, since bitly
            // URLs are case-sensitive, for example.

            sHashtagsOrUrls = sHashtagsOrUrls.ToLower();
        }

        return (sHashtagsOrUrls);
    }

    //*************************************************************************
    //  Method: GetTopHashtags()
    //
    /// <summary>
    /// Get the top hashtags among all Twitter statuses.
    /// </summary>
    ///
    /// <returns>
    /// The top hashtags among all statuses.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopHashtags()
    {
        AssertValid();

        return ( GetTopHashtagsOrUrls(true) );
    }

    //*************************************************************************
    //  Method: GetTopUrls()
    //
    /// <summary>
    /// Get the top URLs among all Twitter statuses.
    /// </summary>
    ///
    /// <returns>
    /// The top URLs among all statuses.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopUrls()
    {
        AssertValid();

        return ( GetTopHashtagsOrUrls(false) );
    }

    //*************************************************************************
    //  Method: GetTopHashtagsOrUrls()
    //
    /// <summary>
    /// Get the top hashtags or URLs among all Twitter statuses.
    /// </summary>
    ///
    /// <param name="bGetTopHashtags">
    /// true to get the top hashtags, false to get the top URLs.
    /// </param>
    ///
    /// <returns>
    /// The top hashtags or URLs among all statuses.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopHashtagsOrUrls
    (
        Boolean bGetTopHashtags
    )
    {
        AssertValid();

        m_oWordCounter.Clear();

        if (!bGetTopHashtags)
        {
            // We're counting URLs, so don't skip them.

            m_oWordCounter.SkipUrlsAndPunctuation = false;
        }

        foreach ( TwitterStatus oTwitterStatus in GetAllTwitterStatuses() )
        {
            // Count the hashtags or URLs as if they were words.

            String sSpaceDelimitedHashtagsOrUrls =
                GetSpaceDelimitedHashtagsOrUrlsFromStatus(
                    oTwitterStatus, bGetTopHashtags);

            m_oWordCounter.CountTermsInDocument(sSpaceDelimitedHashtagsOrUrls);
        }

        HashSet<String> oTopHashtagsOrUrl = GetTopWordsFromWordCounter();

        m_oWordCounter.Clear();
        m_oWordCounter.SkipUrlsAndPunctuation = true;

        return (oTopHashtagsOrUrl);
    }

    //*************************************************************************
    //  Method: GetTopWords()
    //
    /// <summary>
    /// Get the top words among all Twitter statuses.
    /// </summary>
    ///
    /// <returns>
    /// The top words among all statuses.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopWords()
    {
        AssertValid();

        m_oWordCounter.Clear();

        foreach ( TwitterStatus oTwitterStatus in GetAllTwitterStatuses() )
        {
            m_oWordCounter.CountTermsInDocument(oTwitterStatus.Text);
        }

        HashSet<String> oTopWords = GetTopWordsFromWordCounter();

        m_oWordCounter.Clear();

        return (oTopWords);
    }

    //*************************************************************************
    //  Method: GetTopWordsFromWordCounter()
    //
    /// <summary>
    /// Get the top words that have been counted by the WordCounter.
    /// </summary>
    ///
    /// <returns>
    /// The top words that have been counted by the WordCounter.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopWordsFromWordCounter()
    {
        AssertValid();

        HashSet<String> oTopWords = new HashSet<String>(

            (from CountedWord oCountedWord in m_oWordCounter.CountedTerms
            orderby oCountedWord.Salience descending
            select oCountedWord.Word).Take(TopTerms)
            );

        return (oTopWords);
    }

    //*************************************************************************
    //  Method: GetTopWordPairs()
    //
    /// <summary>
    /// Get the top word pairs among all Twitter statuses.
    /// </summary>
    ///
    /// <returns>
    /// The top word pairs among all statuses.
    /// </returns>
    //*************************************************************************

    protected HashSet<String>
    GetTopWordPairs()
    {
        AssertValid();

        m_oWordPairCounter.Clear();

        foreach ( TwitterStatus oTwitterStatus in GetAllTwitterStatuses() )
        {
            m_oWordPairCounter.CountTermsInDocument(oTwitterStatus.Text);
        }

        HashSet<String> oTopWordPairs = new HashSet<String>(

            (from CountedWordPair oCountedWordPair in
                m_oWordPairCounter.CountedTerms

            orderby oCountedWordPair.Salience descending
            select oCountedWordPair.ToString() ).Take(TopTerms)
            );

        m_oWordPairCounter.Clear();

        return (oTopWordPairs);
    }

    //*************************************************************************
    //  Method: GetTermDictionary()
    //
    /// <summary>
    /// Gets a dictionary of terms found in all the Twitter users' statuses.
    /// </summary>
    ///
    /// <param name="oGetTermsFromStatus">
    /// Method that will get the terms in one Twitter status.
    /// </param>
    ///
    /// <param name="oTopTerms">
    /// Top terms among all statuses.  Edges will be appended only for terms in
    /// this collection.
    /// </param>
    ///
    /// <returns>
    /// A dictionary.  The key is a term and the value is a collection of the
    /// screen names for the unique users who have tweeted that term.
    /// </returns>
    //*************************************************************************

    protected Dictionary< String, HashSet<String> >
    GetTermDictionary
    (
        GetTermsFromStatus oGetTermsFromStatus,
        HashSet<String> oTopTerms
    )
    {
        Debug.Assert(oGetTermsFromStatus != null);
        Debug.Assert(oTopTerms != null);
        AssertValid();

        Dictionary< String, HashSet<String> > oTermDictionary = 
            new Dictionary< String, HashSet<String> >();

        foreach (TwitterUser oTwitterUser in m_oTwitterUsers)
        {
            foreach (TwitterStatus oTwitterStatus in oTwitterUser.Statuses)
            {
                foreach ( String sTerm in oGetTermsFromStatus(oTwitterStatus) )
                {
                    if ( !oTopTerms.Contains(sTerm) )
                    {
                        continue;
                    }

                    HashSet<String> oScreenNames;

                    if ( !oTermDictionary.TryGetValue(sTerm,
                        out oScreenNames) )
                    {
                        oScreenNames = new HashSet<String>();
                        oTermDictionary.Add(sTerm, oScreenNames);
                    }

                    // Note that if the same person tweeted the term multiple
                    // times, he will get added to the HashSet only once.

                    oScreenNames.Add(oTwitterUser.ScreenName);
                }
            }
        }

        return (oTermDictionary);
    }

    //*************************************************************************
    //  Method: GetAllTwitterStatuses()
    //
    /// <summary>
    /// Gets an enumerator for enumerating all statuses from all Twitter users.
    /// </summary>
    ///
    /// <returns>
    /// An enumerator for enumerating all statuses from all Twitter users.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<TwitterStatus>
    GetAllTwitterStatuses()
    {
        AssertValid();

        foreach (TwitterUser oTwitterUser in m_oTwitterUsers)
        {
            foreach (TwitterStatus oTwitterStatus in oTwitterUser.Statuses)
            {
                yield return (oTwitterStatus);
            }
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oGraphMLXmlDocument != null);
        Debug.Assert(m_oTwitterUsers != null);
        Debug.Assert(m_oWordCounter != null);
        Debug.Assert(m_oWordPairCounter != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Only this number of top terms are used for appending edges.

    protected const Int32 TopTerms = 10;

    /// If more than MaximumTermTweeters users tweeted a shared term, edges
    /// aren't appended for the shared term.

    protected const Int32 MaximumTermTweeters = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// GraphMLXmlDocument that the edge XML nodes will be appended to.

    protected GraphMLXmlDocument m_oGraphMLXmlDocument;

    /// Collection of the Twitter users in the network.

    protected IEnumerable<TwitterUser> m_oTwitterUsers;

    /// Counts words in a tweet.

    protected WordCounter m_oWordCounter;

    /// Counts word pairs in a tweet.

    protected WordPairCounter m_oWordPairCounter;
}

}

#endif
