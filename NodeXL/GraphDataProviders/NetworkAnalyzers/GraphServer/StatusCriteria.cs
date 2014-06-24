
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
    /// cref="StatusCriteria" /> class with a search term, minimum status date
    /// and maximum status date.
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
    : this(searchTerm, minimumStatusDateUtc)
    {
        m_oMaximumStatusDateUtc = maximumStatusDateUtc;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: StatusCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StatusCriteria" /> class with a search term, minimum status date
    /// and maximum number of statuses.
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
    /// <param name="maximumStatuses">
    /// Maximum number of statuses to get.
    /// </param>
    //*************************************************************************

    public StatusCriteria
    (
        String searchTerm,
        DateTime minimumStatusDateUtc,
        Int32 maximumStatuses
    )
    : this(searchTerm, minimumStatusDateUtc)
    {
        m_iMaximumStatuses = maximumStatuses;

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
    //  Property: MinimumStatusDateUtc
    //
    /// <summary>
    /// Gets the minimum status date.
    /// </summary>
    ///
    /// <value>
    /// The minimum status date, in UTC.
    /// </value>
    //*************************************************************************

    public DateTime
    MinimumStatusDateUtc
    {
        get
        {
            AssertValid();

            return (m_oMinimumStatusDateUtc);
        }
    }

    //*************************************************************************
    //  Property: HasMaximumStatusDateUtc
    //
    /// <summary>
    /// Gets a flag specifying whether a maximum status date should be
    /// used.
    /// </summary>
    ///
    /// <value>
    /// true if a maximum status date should be used, false if a maximum number
    /// of statuses should be used.
    /// </value>
    //*************************************************************************

    public Boolean
    HasMaximumStatusDateUtc
    {
        get
        {
            AssertValid();

            return (m_oMaximumStatusDateUtc.HasValue);
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
    ///
    /// <remarks>
    /// Use this property only if <see cref="HasMaximumStatusDateUtc" />
    /// returns true.
    /// </remarks>
    //*************************************************************************

    public DateTime
    MaximumStatusDateUtc
    {
        get
        {
            AssertValid();
            Debug.Assert(m_oMaximumStatusDateUtc.HasValue);

            return (m_oMaximumStatusDateUtc.Value);
        }
    }

    //*************************************************************************
    //  Property: MaximumStatuses
    //
    /// <summary>
    /// Gets the maximum number of statuses to get.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of statuses to get.
    /// </value>
    ///
    /// <remarks>
    /// Use this property only if <see cref="HasMaximumStatusDateUtc" />
    /// returns false.
    /// </remarks>
    //*************************************************************************

    public Int32
    MaximumStatuses
    {
        get
        {
            AssertValid();
            Debug.Assert(m_iMaximumStatuses.HasValue);

            return (m_iMaximumStatuses.Value);
        }
    }

    //*************************************************************************
    //  Constructor: StatusCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StatusCriteria" /> class with a search term and minimum status
    /// date.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="minimumStatusDateUtc">
    /// Minimum status date, in UTC.
    /// </param>
    //*************************************************************************

    private StatusCriteria
    (
        String searchTerm,
        DateTime minimumStatusDateUtc
    )
    {
        m_sSearchTerm = searchTerm;
        m_oMinimumStatusDateUtc = minimumStatusDateUtc;
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

        Debug.Assert(m_oMaximumStatusDateUtc.HasValue ==
            !m_iMaximumStatuses.HasValue);

        if (m_oMaximumStatusDateUtc.HasValue)
        {
            Debug.Assert(m_oMaximumStatusDateUtc >= m_oMinimumStatusDateUtc);
        }

        if (m_iMaximumStatuses.HasValue)
        {
            Debug.Assert(m_iMaximumStatuses > 0);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The term to search for.

    protected String m_sSearchTerm;

    /// Minimum status date, in UTC.

    protected DateTime m_oMinimumStatusDateUtc;

    /// Maximum status date, in UTC, or null.

    protected Nullable<DateTime> m_oMaximumStatusDateUtc;

    /// Maximum number of statuses to get, or null.

    protected Nullable<Int32> m_iMaximumStatuses;
}

}
