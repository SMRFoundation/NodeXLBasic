
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: TwitterStatus
//
/// <summary>
/// Stores information about a Twitter status.
/// </summary>
///
/// <remarks>
/// This is meant for use while creating Twitter GraphML XML documents for use
/// with the NodeXL Excel Template.
/// </remarks>
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
    /// cref="Smrf.SocialNetworkLib.Twitter.TwitterDateParser.ParseTwitterDate"
    /// />.  Can be null or empty.
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
        m_ID = ID;
        m_Text = text;
        m_ParsedDateUtc = parsedDateUtc;
        m_Latitude = latitude;
        m_Longitude = longitude;
        m_Urls = urls;
        m_Hashtags = hashtags;

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

            return (m_ID);
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

            return (m_Text);
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
    /// cref="Smrf.SocialNetworkLib.Twitter.TwitterDateParser.ParseTwitterDate"
    /// />.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    ParsedDateUtc
    {
        get
        {
            AssertValid();

            return (m_ParsedDateUtc);
        }
    }

    //*************************************************************************
    //  Property: Latitude
    //
    /// <summary>
    /// Gets the status's latitude.
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

            return (m_Latitude);
        }
    }

    //*************************************************************************
    //  Property: Longitude
    //
    /// <summary>
    /// Gets the status's longitude.
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

            return (m_Longitude);
        }
    }

    //*************************************************************************
    //  Property: Urls
    //
    /// <summary>
    /// Gets or sets the status's space-delimited URLs.
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

            return (m_Urls);
        }

		set
		{
			m_Urls = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Hashtags
    //
    /// <summary>
    /// Gets the status's space-delimited hashtags.
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

            return (m_Hashtags);
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
        Debug.Assert( !String.IsNullOrEmpty(m_ID) );
        Debug.Assert( !String.IsNullOrEmpty(m_Text) );
        // m_ParsedDateUtc
        // m_Latitude
        // m_Longitude
        // m_Urls
        // m_Hashtags
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The status ID.

    protected String m_ID;

    /// The status text.

    protected String m_Text;

    /// The parsed status date as a culture-invariant UTC string.

    protected String m_ParsedDateUtc;

    /// The status's latitude and longitude.

    protected String m_Latitude;
    ///
    protected String m_Longitude;

    /// The status's space-delimited URLs.

    protected String m_Urls;

    /// The status's space-delimited hashtags.

    protected String m_Hashtags;
}

}
