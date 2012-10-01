
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterStatus
//
/// <summary>
/// Stores information about a Twitter status.
/// </summary>
//*****************************************************************************

public class TwitterStatus : Object
{
    //*************************************************************************
    //  Constructor: TwitterStatus()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterStatus" /> class.
    /// </summary>
    ///
    /// <param name="ID">
    /// The status ID.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="text">
    /// The status text.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="parsedDateUtc">
    /// The parsed status date as a culture-invariant UTC string.  See <see
    /// cref="TwitterDateParser.ParseTwitterDate" />.  Can be null or empty.
    /// </param>
    ///
    /// <param name="latitude">
    /// The status's latitude.  Can be null or empty.
    /// </param>
    ///
    /// <param name="longitude">
    /// The status's longitude.  Can be null or empty.
    /// </param>
    ///
    /// <param name="urls">
    /// The status's space-delimited URLs.  Can be null or empty.
    /// </param>
    ///
    /// <param name="hashtags">
    /// The status's space-delimited hashtags.  Can be null or empty.
    /// </param>
    //*************************************************************************

    public TwitterStatus
    (
        String ID,
        String text,
        String parsedDateUtc,
        String latitude,
        String longitude,
        String urls,
        String hashtags
    )
    {
        m_sID = ID;
        m_sText = text;
        m_sParsedDateUtc = parsedDateUtc;
        m_sLatitude = latitude;
        m_sLongitude = longitude;
        m_sUrls = urls;
        m_sHashtags = hashtags;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the status ID.
    /// </summary>
    ///
    /// <value>
    /// The status ID.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    ID
    {
        get
        {
            AssertValid();

            return (m_sID);
        }
    }

    //*************************************************************************
    //  Property: Text
    //
    /// <summary>
    /// Gets the status text.
    /// </summary>
    ///
    /// <value>
    /// The status text.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Text
    {
        get
        {
            AssertValid();

            return (m_sText);
        }
    }

    //*************************************************************************
    //  Property: ParsedDateUtc
    //
    /// <summary>
    /// Gets the parsed status date.
    /// </summary>
    ///
    /// <value>
    /// The parsed status date as a culture-invariant UTC string.  See <see
    /// cref="TwitterDateParser.ParseTwitterDate" />.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    ParsedDateUtc
    {
        get
        {
            AssertValid();

            return (m_sParsedDateUtc);
        }
    }

    //*************************************************************************
    //  Property: Latitude
    //
    /// <summary>
    /// Get's the status's latitude.
    /// </summary>
    ///
    /// <value>
    /// The status's latitude.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Latitude
    {
        get
        {
            AssertValid();

            return (m_sLatitude);
        }
    }

    //*************************************************************************
    //  Property: Longitude
    //
    /// <summary>
    /// Get's the status's longitude.
    /// </summary>
    ///
    /// <value>
    /// The status's longitude.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Longitude
    {
        get
        {
            AssertValid();

            return (m_sLongitude);
        }
    }

    //*************************************************************************
    //  Property: Urls
    //
    /// <summary>
    /// Get's the status's space-delimited URLs.
    /// </summary>
    ///
    /// <value>
    /// The status's space-delimited URLs.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Urls
    {
        get
        {
            AssertValid();

            return (m_sUrls);
        }
    }

    //*************************************************************************
    //  Property: Hashtags
    //
    /// <summary>
    /// Get's the status's space-delimited hashtags.
    /// </summary>
    ///
    /// <value>
    /// The status's space-delimited hashtags.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Hashtags
    {
        get
        {
            AssertValid();

            return (m_sHashtags);
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
        Debug.Assert( !String.IsNullOrEmpty(m_sID) );
        Debug.Assert( !String.IsNullOrEmpty(m_sText) );
        // m_sParsedDateUtc
        // m_sLatitude
        // m_sLongitude
        // m_sUrls
        // m_sHashtags
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The status ID.

    protected String m_sID;

    /// The status text.

    protected String m_sText;

    /// The parsed status date as a culture-invariant UTC string.

    protected String m_sParsedDateUtc;

    /// The status's latitude and longitude.

    protected String m_sLatitude;
    ///
    protected String m_sLongitude;

    /// The status's space-delimited URLs.

    protected String m_sUrls;

    /// The status's space-delimited hashtags.

    protected String m_sHashtags;
}

}
