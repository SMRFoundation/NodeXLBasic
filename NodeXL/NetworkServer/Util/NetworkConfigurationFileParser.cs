
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using Smrf.NodeXL.GraphDataProviders.Twitter;
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

                "The network configuration file does not contain valid XML."
                + "  Details: " + oXmlException.Message,

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
            null,
            String.Empty,
            "NetworkType"
            ) );
    }

    //*************************************************************************
    //  Method: GetTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Gets the configuration details for a Twitter search network obtained
    /// directly from Twitter.
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
    //*************************************************************************

    public void
    GetTwitterSearchNetworkConfiguration
    (
        out String searchTerm,
        out TwitterSearchNetworkAnalyzer.WhatToInclude whatToInclude,
        out Int32 maximumStatuses,
        out String networkFileFolderPath
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

                oTwitterSearchNetworkConfigurationNode,
                "WhatToInclude/text()",

                // These WhatToInclude values were removed in version
                // 1.0.1.328.  They are now automatically included in the
                // network.   Old configuration files might still specify them,
                // so they need to be ignored.

                new String[] {
                    "Statuses",
                    "Statistics",
                    "RepliesToEdges",
                    "MentionsEdges",
                    "NonRepliesToNonMentionsEdges",
                    },

                "None",
                "WhatToInclude"
                );

        GetCommonConfiguration(oTwitterSearchNetworkConfigurationNode,
            out networkFileFolderPath);
    }

    //*************************************************************************
    //  Method: GetGraphServerTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Gets the configuration details for a Twitter search network obtained
    /// from the Graph Server.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// Where the term to search for gets stored.
    /// </param>
    ///
    /// <param name="daysIncludingToday">
    /// Where the number of days to include in the tweet date range gets
    /// stored.  The range always ends today.  If today is May 26, for example,
    /// and daysIncludingToday is set to 2, then the tweet date range is May 25
    /// through May 26, inclusive.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// Where the "expand status URLs" flag gets stored.
    /// </param>
    ///
    /// <param name="graphServerUserName">
    /// Where the Graph Server user name gets stored.
    /// </param>
    ///
    /// <param name="graphServerPassword">
    /// Where the Graph Server password gets stored.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    //*************************************************************************

    public void
    GetGraphServerTwitterSearchNetworkConfiguration
    (
        out String searchTerm,
        out Int32 daysIncludingToday,
        out Boolean expandStatusUrls,
        out String graphServerUserName,
        out String graphServerPassword,
        out String networkFileFolderPath
    )
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);
        Debug.Assert(GetNetworkType() == NetworkType.GraphServerTwitterSearch);

        XmlNode oGraphServerTwitterSearchNetworkConfigurationNode =
            XmlUtil2.SelectRequiredSingleNode(
                m_oNetworkConfigurationXmlDocument,

                "/NetworkConfiguration/"
                    + " GraphServerTwitterSearchNetworkConfiguration",

                null);

        searchTerm = XmlUtil2.SelectRequiredSingleNodeAsString(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            "SearchTerm/text()", null);

        daysIncludingToday = XmlUtil2.SelectRequiredSingleNodeAsInt32(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            "DaysIncludingToday/text()", null);

        if (daysIncludingToday < 1)
        {
            throw new XmlException(
                "The daysIncludingToday value can't be less than 1."
                );
        }

        expandStatusUrls = XmlUtil2.SelectRequiredSingleNodeAsBoolean(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            "ExpandStatusUrls/text()", null);

        graphServerUserName = XmlUtil2.SelectRequiredSingleNodeAsString(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            "GraphServerUserName/text()", null);

        graphServerPassword = XmlUtil2.SelectRequiredSingleNodeAsString(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            "GraphServerPassword/text()", null);

        GetCommonConfiguration(
            oGraphServerTwitterSearchNetworkConfigurationNode,
            out networkFileFolderPath);
    }

    //*************************************************************************
    //  Method: GetCommonConfiguration()
    //
    /// <summary>
    /// Gets the configuration details common to all networks.
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
    //*************************************************************************

    protected void
    GetCommonConfiguration
    (
        XmlNode oParentNode,
        out String sNetworkFileFolderPath
    )
    {
        Debug.Assert(oParentNode != null);
        AssertValid();

        sNetworkFileFolderPath = XmlUtil2.SelectRequiredSingleNodeAsString(
            oParentNode, "NetworkFileFolder/text()", null);
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
    /// <param name="asEnumValuesToIgnore">
    /// One or more values to remove from the Enum string before converting it
    /// to the Enum value, or null to not remove any values.  This is used for
    /// backward compatibility in the situation where an Enum used to have a
    /// value that it no longer has.
    /// </param>
    ///
    /// <param name="sValueToUseIfEmpty">
    /// The Enum value to use if the text of the specified node is empty after
    /// asEnumValuesToIgnore are removed.  Sample: "None".
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
        String[] asEnumValuesToIgnore,
        String sValueToUseIfEmpty,
        String sTagName
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(sValueToUseIfEmpty != null);
        Debug.Assert( !String.IsNullOrEmpty(sTagName) );
        AssertValid();

        Exception oException;

        try
        {
            String sEnumAsString = XmlUtil2.SelectRequiredSingleNodeAsString(
                oNode, sXPath, null);

            if (asEnumValuesToIgnore != null)
            {
                sEnumAsString = RemoveEnumValues(
                    sEnumAsString, asEnumValuesToIgnore);

                if (sEnumAsString.Length == 0)
                {
                    sEnumAsString = sValueToUseIfEmpty;
                }
            }

            return ( (T)Enum.Parse(typeof(T), sEnumAsString) );
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
    //  Method: RemoveEnumValues()
    //
    /// <summary>
    /// Removes specified values from Enum text.
    /// </summary>
    ///
    /// <param name="sEnumAsString">
    /// An Enum value as a string.  Sample:
    /// "ExpandedStatusUrls, Statuses, FollowedEdges".
    /// </param>
    ///
    /// <param name="asEnumValuesToIgnore">
    /// One or more values to remove from the Enum string before converting it
    /// to the Enum value.  Sample: "Statuses".
    /// </param>
    ///
    /// <returns>
    /// The Enum value as a string, without the ignored values.  Sample:
    /// "ExpandedStatusUrls,FollowedEdges".
    /// </returns>
    //*************************************************************************

    protected String
    RemoveEnumValues
    (
        String sEnumAsString,
        String[] asEnumValuesToIgnore
    )
    {
        Debug.Assert(sEnumAsString != null);
        Debug.Assert(asEnumValuesToIgnore != null);
        AssertValid();

        List<String> oFilteredEnumValues = new List<String>(

            sEnumAsString.Split(new Char[]{',', ' '},
                StringSplitOptions.RemoveEmptyEntries)
            );

        foreach (String sEnumValueToIgnore in asEnumValuesToIgnore)
        {
            oFilteredEnumValues.Remove(sEnumValueToIgnore);
        }

        return ( String.Join(",", oFilteredEnumValues.ToArray() ) );
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
