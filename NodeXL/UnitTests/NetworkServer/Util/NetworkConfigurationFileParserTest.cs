
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.NetworkServer;
using Smrf.NodeXL.GraphDataProviders.Twitter;
using Smrf.SocialNetworkLib;

namespace Smrf.NodeXL.NetworkServer.UnitTests
{
//*****************************************************************************
//  Class: NetworkConfigurationFileParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="NetworkConfigurationFileParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NetworkConfigurationFileParserTest : Object
{
    //*************************************************************************
    //  Constructor: NetworkConfigurationFileParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NetworkConfigurationFileParserTest" /> class.
    /// </summary>
    //*************************************************************************

    public NetworkConfigurationFileParserTest()
    {
        m_oNetworkConfigurationFileParser = null;
        m_sTempFileName = null;
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
        m_oNetworkConfigurationFileParser =
            new NetworkConfigurationFileParser();

        m_sTempFileName = Path.GetTempFileName();
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
        m_oNetworkConfigurationFileParser = null;

        if ( File.Exists(m_sTempFileName) )
        {
            File.Delete(m_sTempFileName);
        }
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFile()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOpenNetworkConfigurationFile()
    {
        // Twitter search network.

        WriteSampleTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFile2()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOpenNetworkConfigurationFile2()
    {
        // Graph Server Twitter search network.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad()
    {
        // Missing folder.

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                @"X:\Abc\NoSuchFile.xyz");
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The network configuration file couldn't be found."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad2()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad2()
    {
        // Missing file.

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                @"C:\NoSuchFile.xyz");
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The network configuration file couldn't be found."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad3()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad3()
    {
        // Bad XML.

        WriteTempFile("BadXML");

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                m_sTempFileName);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue(oXmlException.Message.StartsWith(
                "The network configuration file does not contain valid XML."
                 ) );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetNetworkType()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNetworkType()
    {
        // TwitterSearch.

        WriteSampleTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        Assert.AreEqual( NetworkType.TwitterSearch,
            m_oNetworkConfigurationFileParser.GetNetworkType() );
    }

    //*************************************************************************
    //  Method: TestGetNetworkType2()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNetworkType2()
    {
        // GraphServerTwitterSearch.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        Assert.AreEqual( NetworkType.GraphServerTwitterSearch,
            m_oNetworkConfigurationFileParser.GetNetworkType() );
    }

    //*************************************************************************
    //  Method: TestGetNetworkTypeBad()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetNetworkTypeBad()
    {
        // Missing value.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType></NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        try
        {
            m_oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkType value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetNetworkTypeBad2()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetNetworkTypeBad2()
    {
        // Bad NetworkType.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>BadValue</NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        try
        {
            m_oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkType value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration()
    {
        // Unmodified configuration.

        WriteSampleTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(
            TwitterSearchNetworkAnalyzer.WhatToInclude.None,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration2()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration2()
    {
        // No MaximumPeoplePerRequest.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<MaximumPeoplePerRequest>100</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest></MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(
            TwitterSearchNetworkAnalyzer.WhatToInclude.None,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration3()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration3()
    {
        // All WhatToInclude options.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>ExpandedStatusUrls,  FollowedEdges</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(

            TwitterSearchNetworkAnalyzer.WhatToInclude.ExpandedStatusUrls
            |
            TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges
            ,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration4()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration4()
    {
        // All WhatToInclude options, no spaces.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>ExpandedStatusUrls,FollowedEdges</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(

            TwitterSearchNetworkAnalyzer.WhatToInclude.ExpandedStatusUrls
            |
            TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges
            ,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration5()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration5()
    {
        // All WhatToInclude options, including old values that no longer
        // exist. 

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",

            "<WhatToInclude>"
            + "Statuses,ExpandedStatusUrls,Statistics,FollowedEdges,"
            + "RepliesToEdges,MentionsEdges,NonRepliesToNonMentionsEdges,"
            + "</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(

            TwitterSearchNetworkAnalyzer.WhatToInclude.ExpandedStatusUrls
            |
            TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges
            ,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration6()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration6()
    {
        // "None" WhatToInclude option.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>None</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(TwitterSearchNetworkAnalyzer.WhatToInclude.None,
            eWhatToInclude);

        Assert.AreEqual(100, iMaximumStatuses);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad()
    {
        // Missing SearchTerm.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<SearchTerm>.*</SearchTerm>",
            "<SearchTerm></SearchTerm>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"SearchTerm") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad2()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad2()
    {
        // Missing WhatToInclude.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude></WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad3()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad3()
    {
        // Bad WhatToInclude.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>Xyz</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad4()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad4()
    {
        // Bad MaximumStatuses.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<MaximumStatuses>.*</MaximumStatuses>",
            "<MaximumStatuses>Xyz</MaximumStatuses>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
            
                "There must be a MaximumStatuses value.  (This was called"
                + " MaximumPeoplePerRequest in previous versions of NodeXL.)"
                ,
                oXmlException.Message);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad5()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad5()
    {
        // Missing NetworkFileFolder.

        WriteSampleTwitterSearchNetworkConfigurationFile(
            "<NetworkFileFolder>.*</NetworkFileFolder>",
            "<NetworkFileFolder></NetworkFileFolder>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"NetworkFileFolder") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Tests the GetGraphServerTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetGraphServerTwitterSearchNetworkConfiguration()
    {
        // Unmodified configuration.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser
        .GetGraphServerTwitterSearchNetworkConfiguration(
            out sSearchTerm, out iDaysIncludingToday, out bExpandStatusUrls,
            out sGraphServerUserName, out sGraphServerPassword,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);
        Assert.AreEqual(7, iDaysIncludingToday);
        Assert.IsFalse(bExpandStatusUrls);
        Assert.AreEqual("TheUserName", sGraphServerUserName);
        Assert.AreEqual("ThePassword", sGraphServerPassword);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfiguration2()
    //
    /// <summary>
    /// Tests the GetGraphServerTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetGraphServerTwitterSearchNetworkConfiguration2()
    {
        // Expand status URLs.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<ExpandStatusUrls>.*</ExpandStatusUrls>",
            "<ExpandStatusUrls>True</ExpandStatusUrls>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        m_oNetworkConfigurationFileParser
        .GetGraphServerTwitterSearchNetworkConfiguration(
            out sSearchTerm, out iDaysIncludingToday, out bExpandStatusUrls,
            out sGraphServerUserName, out sGraphServerPassword,
            out sNetworkFileFolderPath);

        Assert.AreEqual("NodeXL", sSearchTerm);
        Assert.AreEqual(7, iDaysIncludingToday);
        Assert.IsTrue(bExpandStatusUrls);
        Assert.AreEqual("TheUserName", sGraphServerUserName);
        Assert.AreEqual("ThePassword", sGraphServerPassword);
        Assert.AreEqual(@"C:\NodeXLNetworks", sNetworkFileFolderPath);
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad()
    {
        // Missing SearchTerm.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<SearchTerm>.*</SearchTerm>",
            "<SearchTerm></SearchTerm>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"SearchTerm") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad2()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad2()
    {
        // Missing DaysIncludingToday.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<DaysIncludingToday>.*</DaysIncludingToday>",
            "<DaysIncludingToday></DaysIncludingToday>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"DaysIncludingToday") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad3()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad3()
    {
        // Bad DaysIncludingToday.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<DaysIncludingToday>.*</DaysIncludingToday>",
            "<DaysIncludingToday>BadValue</DaysIncludingToday>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"DaysIncludingToday") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad4()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad4()
    {
        // Bad DaysIncludingToday.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<DaysIncludingToday>.*</DaysIncludingToday>",
            "<DaysIncludingToday>0</DaysIncludingToday>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "can't be less than 1") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad5()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad5()
    {
        // Missing ExpandStatusUrls.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<ExpandStatusUrls>.*</ExpandStatusUrls>",
            "<ExpandStatusUrls></ExpandStatusUrls>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"ExpandStatusUrls") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad6()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad6()
    {
        // Bad ExpandStatusUrls.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<ExpandStatusUrls>.*</ExpandStatusUrls>",
            "<ExpandStatusUrls>BadValue</ExpandStatusUrls>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "whose value must be a Boolean") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad7()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad7()
    {
        // Missing GraphServerUserName.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<GraphServerUserName>.*</GraphServerUserName>",
            "<GraphServerUserName></GraphServerUserName>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"GraphServerUserName") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad8()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad8()
    {
        // Missing GraphServerPassword.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<GraphServerPassword>.*</GraphServerPassword>",
            "<GraphServerPassword></GraphServerPassword>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"GraphServerPassword") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetGraphServerTwitterSearchNetworkConfigurationBad9()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetGraphServerTwitterSearchNetworkConfigurationBad9()
    {
        // Missing NetworkFileFolder.

        WriteSampleGraphServerTwitterSearchNetworkConfigurationFile(
            "<NetworkFileFolder>.*</NetworkFileFolder>",
            "<NetworkFileFolder></NetworkFileFolder>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        Int32 iDaysIncludingToday;
        Boolean bExpandStatusUrls;
        String sGraphServerUserName;
        String sGraphServerPassword;
        String sNetworkFileFolderPath;

        try
        {
            m_oNetworkConfigurationFileParser
            .GetGraphServerTwitterSearchNetworkConfiguration(
                out sSearchTerm, out iDaysIncludingToday,
                out bExpandStatusUrls, out sGraphServerUserName,
                out sGraphServerPassword, out sNetworkFileFolderPath);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"NetworkFileFolder") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: WriteSampleTwitterSearchNetworkConfigurationFile()
    //
    /// <summary>
    /// Writes a copy of a sample Twitter search network configuration file to
    /// a temporary file after optionally modifying the copy.
    /// </summary>
    ///
    /// <param name="asPatternReplacementPairs">
    /// Array of string pairs.  The first element of each pair is the Regex
    /// pattern to replace in the file.  The second element of each pair is the
    /// replacement string.
    /// </param>
    //*************************************************************************

    protected void
    WriteSampleTwitterSearchNetworkConfigurationFile
    (
        params String [] asPatternReplacementPairs
    )
    {
        WriteSampleNetworkConfigurationFile(
            SampleTwitterSearchNetworkConfigurationFileName,
            asPatternReplacementPairs
            );
    }

    //*************************************************************************
    //  Method: WriteSampleGraphServerTwitterSearchNetworkConfigurationFile()
    //
    /// <summary>
    /// Writes a copy of a sample Graph Server Twitter search network
    /// configuration file to a temporary file after optionally modifying the
    /// copy.
    /// </summary>
    ///
    /// <param name="asPatternReplacementPairs">
    /// Array of string pairs.  The first element of each pair is the Regex
    /// pattern to replace in the file.  The second element of each pair is the
    /// replacement string.
    /// </param>
    //*************************************************************************

    protected void
    WriteSampleGraphServerTwitterSearchNetworkConfigurationFile
    (
        params String [] asPatternReplacementPairs
    )
    {
        WriteSampleNetworkConfigurationFile(
            SampleGraphServerTwitterSearchNetworkConfigurationFileName,
            asPatternReplacementPairs
            );
    }

    //*************************************************************************
    //  Method: WriteSampleNetworkConfigurationFile()
    //
    /// <summary>
    /// Writes a copy of a sample network configuration file to a temporary
    /// file after optionally modifying the copy.
    /// </summary>
    ///
    /// <param name="sSampleNetworkConfigurationFileName">
    /// Name of the sample network configuration file to copy, without a path.
    /// </param>
    ///
    /// <param name="asPatternReplacementPairs">
    /// Array of string pairs.  The first element of each pair is the Regex
    /// pattern to replace in the file.  The second element of each pair is the
    /// replacement string.
    /// </param>
    //*************************************************************************

    protected void
    WriteSampleNetworkConfigurationFile
    (
        String sSampleNetworkConfigurationFileName,
        params String [] asPatternReplacementPairs
    )
    {
        String sSampleNetworkConfigurationFilePath = Path.Combine(
            SampleNetworkConfigurationFolderRelativePath,
            sSampleNetworkConfigurationFileName
            );

        using ( StreamReader oStreamReader = new StreamReader(
            sSampleNetworkConfigurationFilePath) )
        {
            String sFileContents = oStreamReader.ReadToEnd();

            Int32 iArguments = asPatternReplacementPairs.Length;

            for (Int32 i = 0; i < iArguments; i += 2)
            {
                sFileContents = Regex.Replace(sFileContents,
                    asPatternReplacementPairs[i + 0],
                    asPatternReplacementPairs[i + 1]
                    );
            }

            WriteTempFile(sFileContents);
        }
    }

    //*************************************************************************
    //  Method: WriteTempFile()
    //
    /// <summary>
    /// Writes a temporary file.
    /// </summary>
    //*************************************************************************

    protected void
    WriteTempFile
    (
        String sFileContents
    )
    {
        using ( StreamWriter oStreamWriter =
            new StreamWriter(m_sTempFileName) )
        {
            oStreamWriter.Write(sFileContents);
        }
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Path to the folder that contains the sample network configuration files
    /// that are distributed with the application, relative to the unit test
    /// executable.

    protected const String SampleNetworkConfigurationFolderRelativePath
        = @"..\..\..\NetworkServer\SampleNetworkConfigurationFiles";

    /// Name of the sample Twitter search network configuration file, without a
    /// path.

    protected const String SampleTwitterSearchNetworkConfigurationFileName
        = "SampleTwitterSearchNetworkConfiguration.xml";

    /// Name of the sample Graph Server Twitter search network configuration
    /// file, without a path.

    protected const String
        SampleGraphServerTwitterSearchNetworkConfigurationFileName
        = "SampleGraphServerTwitterSearchNetworkConfiguration.xml";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NetworkConfigurationFileParser m_oNetworkConfigurationFileParser;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
