
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: WordCounter
//
/// <summary>
/// Counts words in one or more documents.
/// </summary>
///
/// <remarks>
/// Call <see cref="TermCounterBase{TCountedTerm}.CountTermsInDocument" /> for
/// each document that contains words that need to be counted.  When done, call
/// <see
/// cref="TermCounterBase{TCountedTerm}.CalculateSalienceOfCountedTerms" /> to
/// calculate the salience of each word within all the documents, then use <see
/// cref="TermCounterBase{TCountedTerm}.CountedTerms" /> to get a collection of
/// the words that were counted in all the documents.
///
/// <para>
/// When words are counted, the words "B" and "b" are considered the same word.
/// The <see cref="CountedWord.Word" /> string is always in lower case.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class WordCounter : TermCounterBase<CountedWord>
{
    //*************************************************************************
    //  Constructor: WordCounter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordCounter" /> class.
    /// </summary>
    ///
    /// <param name="wordsToSkip">
    /// An array of words that should be skipped when counting words.  Can be
    /// empty but not null.  The case of the words is irrelevant.
    /// </param>
    //*************************************************************************

    public WordCounter
    (
        String [] wordsToSkip
    )
    : base(wordsToSkip)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CountTermsInWords()
    //
    /// <summary>
    /// Counts the terms in a list of words that have been extracted from a
    /// document.
    /// </summary>
    ///
    /// <param name="oWords">
    /// The collection of words to count the terms in.  Excludes URLs,
    /// punctuation, and the "words to skip" that were specified in the
    /// constructor.  The words are all in lower case.
    /// </param>
    ///
    /// <param name="oCountedTerms">
    /// The dictionary of terms that have been counted so far. The key is
    /// determined by the derived class and the value is the counted term.
    /// </param>
    ///
    /// <param name="oKeysCountedInThisCall">
    /// The keys in <paramref name="oCountedTerms" /> for which terms were
    /// counted.
    /// </param>
    ///
    /// <remarks>
    /// For each term found in <paramref name="oWords" />, this method
    /// increments the term's count in <paramref name="oCountedTerms" /> if the
    /// term was previously counted, or adds a key/value pair to <paramref
    /// name="oCountedTerms" /> with a count of 1 if the term was not
    /// previously counted.  It also adds the key to <paramref
    /// name="oKeysCountedInThisCall" />.
    /// </remarks>
    //*************************************************************************

    protected override void
    CountTermsInWords
    (
        IList<String> oWords,
        Dictionary<String, CountedWord> oCountedTerms,
        HashSet<String> oKeysCountedInThisCall
    )
    {
        Debug.Assert(oWords != null);
        Debug.Assert(oCountedTerms != null);
        Debug.Assert(oKeysCountedInThisCall != null);
        AssertValid();

        CountedWord oCountedWord;

        foreach (String sWord in oWords)
        {
            if ( !oCountedTerms.TryGetValue(sWord, out oCountedWord) )
            {
                oCountedWord = new CountedWord(sWord);
                oCountedTerms.Add(sWord, oCountedWord);
            }

            oCountedWord.Count++;
            oKeysCountedInThisCall.Add(sWord);
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

    public new void
    AssertValid()
    {
        base.AssertValid();
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
