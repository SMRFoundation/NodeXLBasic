
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: SentenceConcatenatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="SentenceConcatenator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class SentenceConcatenatorTest : Object
{
    //*************************************************************************
    //  Constructor: SentenceConcatenatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SentenceConcatenatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public SentenceConcatenatorTest()
    {
        m_oSentenceConcatenator = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oSentenceConcatenator = new SentenceConcatenator();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oSentenceConcatenator = null;
    }

    //*************************************************************************
    //  Method: TestAddSentence()
    //
    /// <summary>
    /// Tests the AddSentence() and ConcatenateSentences() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAddSentence()
    {
        // No sentences.

        String sConcatenatedSentences =
            m_oSentenceConcatenator.ConcatenateSentences();

        Assert.AreEqual(String.Empty, sConcatenatedSentences);
    }

    //*************************************************************************
    //  Method: TestAddSentence2()
    //
    /// <summary>
    /// Tests the AddSentence() and ConcatenateSentences() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAddSentence2()
    {
        // One sentence.

        m_oSentenceConcatenator.AddSentence("The answer is {0}.", "yes");

        String sConcatenatedSentences =
            m_oSentenceConcatenator.ConcatenateSentences();

        Assert.AreEqual("The answer is yes.", sConcatenatedSentences);
    }

    //*************************************************************************
    //  Method: TestAddSentence3()
    //
    /// <summary>
    /// Tests the AddSentence() and ConcatenateSentences() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAddSentence3()
    {
        // Two sentences.

        m_oSentenceConcatenator.AddSentence("The answer is {0}.", "yes");
        m_oSentenceConcatenator.AddSentence("This has no arguments.");

        String sConcatenatedSentences =
            m_oSentenceConcatenator.ConcatenateSentences();

        Assert.AreEqual("The answer is yes.  This has no arguments.",
            sConcatenatedSentences);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected SentenceConcatenator m_oSentenceConcatenator;
}

}
