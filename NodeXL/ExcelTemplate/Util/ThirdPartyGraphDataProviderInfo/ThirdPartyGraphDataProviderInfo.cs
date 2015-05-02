
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ThirdPartyGraphDataProviderInfo
//
/// <summary>
/// Provides information about one third-party graph data provider.
/// </summary>
//*****************************************************************************

public class ThirdPartyGraphDataProviderInfo : Object
{
    //*************************************************************************
    //  Constructor: ThirdPartyGraphDataProviderInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ThirdPartyGraphDataProviderInfo" /> class.
    /// </summary>
    ///
    /// <param name="name">
    /// The graph data provider's name.
    /// </param>
    ///
    /// <param name="url">
    /// The URL from which the graph data provider can be obtained.
    /// </param>
    //*************************************************************************

    public ThirdPartyGraphDataProviderInfo
    (
        String name,
        String url,
        String description
    )
    {
        m_sName = name;
        m_sUrl = url;
        m_sDescription = description;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the graph data provider's name.
    /// </summary>
    ///
    /// <value>
    /// The graph data provider's name.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Property: Url
    //
    /// <summary>
    /// Gets the graph data provider's URL.
    /// </summary>
    ///
    /// <value>
    /// The URL from which the graph data provider can be obtained.
    /// </value>
    //*************************************************************************

    public String
    Url
    {
        get
        {
            AssertValid();

            return (m_sUrl);
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets the graph data provider's description.
    /// </summary>
    ///
    /// <value>
    /// The graph data provider's description.
    /// </value>
    //*************************************************************************

    public String
    Description
    {
        get
        {
            return (m_sDescription);
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
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sUrl) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph data provider's name.

    protected String m_sName;

    /// The URL from which the graph data provider can be obtained.

    protected String m_sUrl;

    /// The graph data provider's description.

    protected String m_sDescription;
}

}
