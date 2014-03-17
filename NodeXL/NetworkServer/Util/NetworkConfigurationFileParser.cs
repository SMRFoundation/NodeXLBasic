﻿
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.GraphDataProviders.Twitter;
using Smrf.SocialNetworkLib;
using Smrf.XmlLib;

namespace Smrf.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: NetworkConfigurationFileParser
//
/// <summary>
/// Parses a network configuration file.
/// </summary>
///
/// <remarks>
/// A network configuration file specifies which network to get and where to
/// save it on disk.  It's in XML format.
///
/// <para>
/// Call <see cref="OpenNetworkConfigurationFile" /> to open the file.  Call
/// <see cref="GetNetworkType" /> to get the type of network, then call 
/// the method that corresponds to that network type to get the configuration
/// details.
/// </para>
///
/// <para>
/// All of the methods throw an XmlException when they detect invalid
/// configuration information.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NetworkConfigurationFileParser : Object
{
    //*************************************************************************
    //  Constructor: NetworkConfigurationFileParser()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NetworkConfigurationFileParser" /> class.
    /// </summary>
    //*************************************************************************

    public NetworkConfigurationFileParser()
    {
        m_oNetworkConfigurationXmlDocument = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: OpenNetworkConfigurationFile()
    //
    /// <summary>
    /// Opens the network configuration file.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <remarks>
    /// This method must be called before any other methods are called.
    /// </remarks>
    //*************************************************************************

    public void
    OpenNetworkConfigurationFile
    (
        String filePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );
        AssertValid();

        m_oNetworkConfigurationXmlDocument = new XmlDocument();

        const String NotFoundMessage =
            "The network configuration file couldn't be found.";

        try
        {
            using ( StreamReader oStreamReader = new StreamReader(filePath) )
            {
                m_oNetworkConfigurationXmlDocument.Load(oStreamReader);
            }
        }
        catch (DirectoryNotFoundException oDirectoryNotFoundException)
        {
            throw new XmlException(NotFoundMessage,
                oDirectoryNotFoundException);
        }
        catch (FileNotFoundException oFileNotFoundException)
        {
            throw new XmlException(NotFoundMessage, oFileNotFoundException);
        }
        catch (IOException oIOException)
        {
            throw new XmlException(
                "The network configuration file couldn't be opened.",
                oIOException);
        }
        catch (UnauthorizedAccessException oUnauthorizedAccessException)
        {
            throw new XmlException(
                "The network configuration file couldn't be opened due to a"
                + " security restriction.",

                oUnauthorizedAccessException);
        }
        catch (XmlException oXmlException)
        {
            throw new XmlException(
                "The network configuration file does not contain valid XML.",
                oXmlException);
        }
    }

    //*************************************************************************
    //  Method: GetNetworkType()
    //
    /// <summary>
    /// Gets the type of network to get.
    /// </summary>
    ///
    /// <returns>
    /// The type of network to get, as a <see cref="NetworkType" />.
    /// </returns>
    //*************************************************************************

    public NetworkType
    GetNetworkType()
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);

        return ( GetRequiredEnumValue<NetworkType>(
            m_oNetworkConfigurationXmlDocument,
            "/NetworkConfiguration/NetworkType/text()",
            "NetworkType"
            ) );
    }

    //*************************************************************************
    //  Method: GetTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Gets the configuration details for a Twitter search network.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// Where the term to search for gets stored.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Where the specification for what should be included in the network gets
    /// stored.
    /// </param>
    ///
    /// <param name="maximumStatuses">
    /// Where the maximum number of statuses to request gets stored.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="networkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookSettingsFilePath">
    /// Where the full path to the workbook settings file to use for the NodeXL
    /// workbook gets stored.  If null, no workbook settings file should be
    /// used.
    /// </param>
    ///
    /// <param name="automateNodeXLWorkbook">
    /// Where a flag specifying whether automation should be run on the NodeXL
    /// workbook gets stored.
    /// </param>
    //*************************************************************************

    public void
    GetTwitterSearchNetworkConfiguration
    (
        out String searchTerm,
        out TwitterSearchNetworkAnalyzer.WhatToInclude whatToInclude,
        out Int32 maximumStatuses,
        out String networkFileFolderPath,
        out NetworkFileFormats networkFileFormats,
        out String nodeXLWorkbookSettingsFilePath,
        out Boolean automateNodeXLWorkbook
    )
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);
        Debug.Assert(GetNetworkType() == NetworkType.TwitterSearch);

        XmlNode oTwitterSearchNetworkConfigurationNode =
            XmlUtil2.SelectRequiredSingleNode(
                m_oNetworkConfigurationXmlDocument,
                "/NetworkConfiguration/TwitterSearchNetworkConfiguration",
                null);

        searchTerm = XmlUtil2.SelectRequiredSingleNodeAsString(
            oTwitterSearchNetworkConfigurationNode, "SearchTerm/text()", null);

        if ( !XmlUtil2.TrySelectSingleNodeAsInt32(
            oTwitterSearchNetworkConfigurationNode,
            "MaximumStatuses/text()", null, out maximumStatuses) )
        {
            // Older versions of NodeXL used a MaximumPeoplePerRequest value,
            // which has been replaced with MaximumStatuses.  To avoid breaking
            // older configuration files, accept either one.

            try
            {
                maximumStatuses = XmlUtil2.SelectRequiredSingleNodeAsInt32(
                    oTwitterSearchNetworkConfigurationNode,
                    "MaximumPeoplePerRequest/text()", null);
            }
            catch
            {
                throw new XmlException(
                    "There must be a MaximumStatuses value.  (This was called"
                    + " MaximumPeoplePerRequest in previous versions of"
                    + " NodeXL.)"
                );
            }
        }

        whatToInclude =
            GetRequiredEnumValue<TwitterSearchNetworkAnalyzer.WhatToInclude>(
                oTwitterSearchNetworkConfigurationNode, "WhatToInclude/text()",
                "WhatToInclude");

        GetTwitterCommonConfiguration(oTwitterSearchNetworkConfigurationNode,
            out networkFileFolderPath, out networkFileFormats,
            out nodeXLWorkbookSettingsFilePath, out automateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: GetTwitterCommonConfiguration()
    //
    /// <summary>
    /// Gets the configuration details common to all Twitter networks.
    /// </summary>
    ///
    /// <param name="oParentNode">
    /// Node containing the common configuration details.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    ///
    /// <param name="sNodeXLSettingsFilePath">
    /// Where the full path to the workbook settings file to use for the NodeXL
    /// workbook gets stored.  If null, no workbook settings file should be
    /// used.
    /// </param>
    ///
    /// <param name="bAutomateNodeXLWorkbook">
    /// Where a flag specifying whether automation should be run on the NodeXL
    /// workbook gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetTwitterCommonConfiguration
    (
        XmlNode oParentNode,
        out String sNetworkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats,
        out String sNodeXLSettingsFilePath,
        out Boolean bAutomateNodeXLWorkbook
    )
    {
        Debug.Assert(oParentNode != null);
        AssertValid();

        sNetworkFileFolderPath = XmlUtil2.SelectRequiredSingleNodeAsString(
            oParentNode, "NetworkFileFolder/text()", null);

        eNetworkFileFormats = GetRequiredEnumValue<NetworkFileFormats>(
            oParentNode, "NetworkFileFormats/text()", "NetworkFileFormats");

        // The NodeXLWorkbookOptionsFile node was added in a later version of
        // the program, so it is not required.

        if ( !XmlUtil2.TrySelectSingleNodeAsString(oParentNode,
            "NodeXLOptionsFile/text()", null, out sNodeXLSettingsFilePath) )
        {
            sNodeXLSettingsFilePath = null;
        }

        // The AutomateNodeXLWorkbook node was added in a later version of the
        // program, so it is not required.

        String sAutomateNodeXLWorkbook;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oParentNode,
            "AutomateNodeXLWorkbook/text()", null,
            out sAutomateNodeXLWorkbook) )
        {
            if ( !Boolean.TryParse(sAutomateNodeXLWorkbook,
                out bAutomateNodeXLWorkbook) )
            {
                throw new XmlException(
                    "The AutomateNodeXLWorkbook value must be \"true\" or"
                    + " \"false\"."
                    );
            }
        }
        else
        {
            bAutomateNodeXLWorkbook = false;
        }
    }

    //*************************************************************************
    //  Method: GetRequiredEnumValue()
    //
    /// <summary>
    /// Gets a required Enum value from the text of a specified node.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="sTagName">
    /// Name of the tag containing the Enum value.  Used in error messages.
    /// </param>
    ///
    /// <returns>
    /// The specified Enum value.
    /// </returns>
    //*************************************************************************

    protected T
    GetRequiredEnumValue<T>
    (
        XmlNode oNode,
        String sXPath,
        String sTagName
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert( !String.IsNullOrEmpty(sTagName) );
        AssertValid();

        Exception oException;

        try
        {
            String sText = XmlUtil2.SelectRequiredSingleNodeAsString(oNode,
                sXPath, null);

            return ( (T)Enum.Parse(typeof(T), sText) );
        }
        catch (XmlException oXmlException)
        {
            oException = oXmlException;
        }
        catch (ArgumentException oArgumentException)
        {
            oException = oArgumentException;
        }

        String sErrorMessage = String.Format(
            "The {0} value is missing or invalid."
            ,
            sTagName
            );

        throw new XmlException(sErrorMessage, oException);
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
        // m_oNetworkConfigurationXmlDocument
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The opened network configuration file, or null if
    /// OpenNetworkConfigurationFile() hasn't been called.

    protected XmlDocument m_oNetworkConfigurationXmlDocument;
}

}
