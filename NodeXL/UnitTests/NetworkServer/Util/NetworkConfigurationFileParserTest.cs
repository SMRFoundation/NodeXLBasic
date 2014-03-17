
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
        WriteSampleNetworkConfigurationFile();

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
            Assert.AreEqual(
                "The network configuration file does not contain valid XML."
                ,
                oXmlException.Message
                );

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

        WriteSampleNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        Assert.AreEqual( NetworkType.TwitterSearch,
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

        WriteSampleNetworkConfigurationFile(
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

        WriteSampleNetworkConfigurationFile(
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
        WriteSampleNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.AreEqual("NodeXL", sSearchTerm);

        Assert.AreEqual(
            TwitterSearchNetworkAnalyzer.WhatToInclude.Statuses |
            TwitterSearchNetworkAnalyzer.WhatToInclude.MentionsEdges,
            eWhatToInclude);

        Assert.AreEqual(10, iMaximumStatuses);
        Assert.AreEqual(@"C:\", sNetworkFileFolder);
        Assert.AreEqual(NetworkFileFormats.GraphML, eNetworkFileFormats);
        Assert.IsNull(sNodeXLWorkbookSettingsFilePath);
        Assert.IsFalse(bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<MaximumPeoplePerRequest>100</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest></MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.AreEqual(10, iMaximumStatuses);
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
        // Multiple file formats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>GraphML</NetworkFileFormats>",
            "<NetworkFileFormats>GraphML,NodeXLWorkbook</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath,
            out bAutomateNodeXLWorkbook);

        Assert.AreEqual(
            NetworkFileFormats.GraphML | NetworkFileFormats.NodeXLWorkbook,
            eNetworkFileFormats);
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
        // Automate NodeXL workbook.

        WriteSampleNetworkConfigurationFile(
            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>true</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
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
        // Specify NodeXL workbook options file.

        WriteSampleNetworkConfigurationFile(
            "<NodeXLOptionsFile></NodeXLOptionsFile>",

            "<NodeXLOptionsFile>C:\\Folder\\NodeXLOptions.graphml"
                + "</NodeXLOptionsFile>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.AreEqual("C:\\Folder\\NodeXLOptions.graphml",
            sNodeXLWorkbookSettingsFilePath);
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
        // Missing NodeXL workbook options file.

        WriteSampleNetworkConfigurationFile(
            "<NodeXLOptionsFile></NodeXLOptionsFile>",
            String.Empty
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.IsNull(sNodeXLWorkbookSettingsFilePath);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration7()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration7()
    {
        // Automate NodeXL workbook, mixed case.

        WriteSampleNetworkConfigurationFile(
            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>TrUe</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumStatuses,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out sNodeXLWorkbookSettingsFilePath, out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<SearchTerm>.*</SearchTerm>",
            "<SearchTerm></SearchTerm>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude></WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>Xyz</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<MaximumStatuses>.*</MaximumStatuses>",
            "<MaximumStatuses>Xyz</MaximumStatuses>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
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

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFolder>.*</NetworkFileFolder>",
            "<NetworkFileFolder></NetworkFileFolder>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"NetworkFileFolder") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad6()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad6()
    {
        // Missing NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats></NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad7()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad7()
    {
        // Bad NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats>Xyz</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad8()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad8()
    {
        // Bad AutomateNodeXLWorkbook.

        WriteSampleNetworkConfigurationFile(
            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>xx</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumStatuses;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        String sNodeXLWorkbookSettingsFilePath;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumStatuses,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out sNodeXLWorkbookSettingsFilePath,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The AutomateNodeXLWorkbook value must be \"true\" or"
                + " \"false\"."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: WriteSampleNetworkConfigurationFile()
    //
    /// <summary>
    /// Writes a copy of the sample network configuration file to a temporary
    /// file after optionally modifying the copy.
    /// </summary>
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
        params String [] asPatternReplacementPairs
    )
    {
        using ( StreamReader oStreamReader = new StreamReader(
            SampleNetworkConfigurationRelativePath) )
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

    /// Path to the sample network configuration file that is distributed
    /// with the application, relative to the unit test executable.

    protected const String SampleNetworkConfigurationRelativePath
        = @"..\..\..\NetworkServer\SampleNetworkConfiguration.xml";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NetworkConfigurationFileParser m_oNetworkConfigurationFileParser;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
