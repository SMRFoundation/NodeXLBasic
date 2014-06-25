
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
//*****************************************************************************
//  Class: StatusCriteria
//
/// <summary>
/// Specifies the criteria used for getting statuses from the NodeXL Graph
/// Server.
/// </summary>
//*****************************************************************************

public class StatusCriteria : Object
{
    //*************************************************************************
    //  Constructor: StatusCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StatusCriteria" /> class with a search term and a date range.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="minimumStatusDateUtc">
    /// Minimum status date, in UTC.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.
    /// </param>
    //*************************************************************************

    public StatusCriteria
    (
        String searchTerm,
        DateTime minimumStatusDateUtc,
        DateTime maximumStatusDateUtc
    )
    : this(searchTerm, maximumStatusDateUtc)
    {
        m_oMinimumStatusDateUtc = minimumStatusDateUtc;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: StatusCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StatusCriteria" /> class with a search term, maximum status date
    /// and a maximum number of statuses going backward in time.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.
    /// </param>
    ///
    /// <param name="maximumStatusesGoingBackward">
    /// Maximum number of statuses to get, going backward in time.
    /// </param>
    //*************************************************************************

    public StatusCriteria
    (
        String searchTerm,
        DateTime maximumStatusDateUtc,
        Int32 maximumStatusesGoingBackward
    )
    : this(searchTerm, maximumStatusDateUtc)
    {
        m_iMaximumStatusesGoingBackward = maximumStatusesGoingBackward;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SearchTerm
    //
    /// <summary>
    /// Gets the term to search for.
    /// </summary>
    ///
    /// <value>
    /// The term to search for.
    /// </value>
    //*************************************************************************

    public String
    SearchTerm
    {
        get
        {
            AssertValid();

            return (m_sSearchTerm);
        }
    }

    //*************************************************************************
    //  Property: HasDateRange
    //
    /// <summary>
    /// Gets a flag specifying whether a status date range should be used.
    /// </summary>
    ///
    /// <value>
    /// true if a status date range should be used, false if a maximum status
    /// date and a maximum number of statuses going backward should be used.
    /// </value>
    //*************************************************************************

    public Boolean
    HasDateRange
    {
        get
        {
            AssertValid();

            return (m_oMinimumStatusDateUtc.HasValue);
        }
    }

    //*************************************************************************
    //  Property: MinimumStatusDateUtc
    //
    /// <summary>
    /// Gets the minimum status date.
    /// </summary>
    ///
    /// <value>
    /// The minimum status date, in UTC.
    /// </value>
    ///
    /// <remarks>
    /// Use this property only if <see cref="HasDateRange" /> returns true.
    /// </remarks>
    //*************************************************************************

    public DateTime
    MinimumStatusDateUtc
    {
        get
        {
            AssertValid();
            Debug.Assert(m_oMinimumStatusDateUtc.HasValue);

            return (m_oMinimumStatusDateUtc.Value);
        }
    }

    //*************************************************************************
    //  Property: MaximumStatusDateUtc
    //
    /// <summary>
    /// Gets the maximum status date.
    /// </summary>
    ///
    /// <value>
    /// The maximum status date, in UTC.
    /// </value>
    //*************************************************************************

    public DateTime
    MaximumStatusDateUtc
    {
        get
        {
            AssertValid();

            return (m_oMaximumStatusDateUtc);
        }
    }

    //*************************************************************************
    //  Property: MaximumStatusesGoingBackward
    //
    /// <summary>
    /// Gets the maximum number of statuses to get, going backward in time.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of statuses to get, going backward in time.
    /// </value>
    ///
    /// <remarks>
    /// Use this property only if <see cref="HasDateRange" /> returns false.
    /// </remarks>
    //*************************************************************************

    public Int32
    MaximumStatusesGoingBackward
    {
        get
        {
            AssertValid();
            Debug.Assert(m_iMaximumStatusesGoingBackward.HasValue);

            return (m_iMaximumStatusesGoingBackward.Value);
        }
    }

    //*************************************************************************
    //  Constructor: StatusCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StatusCriteria" /> class with a search term and maximum status
    /// date.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="maximumStatusDateUtc">
    /// Maximum status date, in UTC.
    /// </param>
    //*************************************************************************

    private StatusCriteria
    (
        String searchTerm,
        DateTime maximumStatusDateUtc
    )
    {
        m_sSearchTerm = searchTerm;
        m_oMaximumStatusDateUtc = maximumStatusDateUtc;
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
        Debug.Assert( !String.IsNullOrEmpty(m_sSearchTerm) );

        Debug.Assert(m_oMinimumStatusDateUtc.HasValue ==
            !m_iMaximumStatusesGoingBackward.HasValue);

        if (m_oMinimumStatusDateUtc.HasValue)
        {
            Debug.Assert(m_oMaximumStatusDateUtc >= m_oMinimumStatusDateUtc);
        }

        if (m_iMaximumStatusesGoingBackward.HasValue)
        {
            Debug.Assert(m_iMaximumStatusesGoingBackward > 0);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The term to search for.

    protected String m_sSearchTerm;

    /// Minimum status date, in UTC, or null.

    protected Nullable<DateTime> m_oMinimumStatusDateUtc;

    /// Maximum status date, in UTC.

    protected DateTime m_oMaximumStatusDateUtc;

    /// Maximum number of statuses to get going backward in time, or null.

    protected Nullable<Int32> m_iMaximumStatusesGoingBackward;
}

}
