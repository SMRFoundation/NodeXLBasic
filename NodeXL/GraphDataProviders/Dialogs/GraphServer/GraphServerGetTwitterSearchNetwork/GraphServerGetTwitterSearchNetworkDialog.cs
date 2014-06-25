
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
//*****************************************************************************
//  Class: GraphServerGetTwitterSearchNetworkDialog
//
/// <summary>
/// Uses the NodeXL Graph Server to get a network of people who have tweeted
/// a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class GraphServerGetTwitterSearchNetworkDialog :
    GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: GraphServerGetTwitterSearchNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphServerGetTwitterSearchNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public GraphServerGetTwitterSearchNetworkDialog()
    :
    base( new GraphServerTwitterSearchNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sSearchTerm
        // m_bUseDateRange
        // m_oStatusDate1Utc
        // m_oStatusDate2Utc
        // m_iMaximumStatusesGoingBackward
        // m_bExpandStatusUrls
        // m_sGraphServerUserName
        // m_sGraphServerPassword

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ToolStripStatusLabel
    //
    /// <summary>
    /// Gets the dialog's ToolStripStatusLabel control.
    /// </summary>
    ///
    /// <value>
    /// The dialog's ToolStripStatusLabel control, or null if the dialog
    /// doesn't have one.  The default is null.
    /// </value>
    ///
    /// <remarks>
    /// If the derived dialog overrides this property and returns a non-null
    /// ToolStripStatusLabel control, the control's text will automatically get
    /// updated when the HttpNetworkAnalyzer fires a ProgressChanged event.
    /// </remarks>
    //*************************************************************************

    protected override ToolStripStatusLabel
    ToolStripStatusLabel
    {
        get
        {
            AssertValid();

            return (this.slStatusLabel);
        }
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            return ( DoDataExchangeFromControls() );
        }

        DoDataExchangeToControls();
        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeFromControls()
    //
    /// <summary>
    /// Transfers data from the dialog's controls to its fields.
    /// </summary>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchangeFromControls()
    {
        if (
            !ValidateRequiredTextBox(txbSearchTerm,
                "Enter one or more words to search for.",
                out m_sSearchTerm)
            )
        {
            return (false);
        }

        m_bUseDateRange = radUseDateRange.Checked;
        m_oStatusDate1Utc = dtpStatusDate1Utc.Value;
        m_oStatusDate2Utc = dtpStatusDate2Utc.Value;

        if (m_bUseDateRange)
        {
            if (m_oStatusDate2Utc < m_oStatusDate1Utc)
            {
                OnInvalidDateTimePicker(dtpStatusDate2Utc,
                    "The second date can't be earlier than the first date."
                    );

                return (false);
            }
        }
        else
        {
            // Don't validate directly into m_iMaximumStatusesGoingBackward
            // here.  If you blank out a NumericUpDown control, its value is
            // Int32.MinValue, and we don't want that stored in
            // m_iMaximumStatusesGoingBackward.

            Int32 iMaximumStatusesGoingBackward;

            if ( !ValidateNumericUpDown(nudMaximumStatusesGoingBackward,
                "a maximum number of tweets",
                out iMaximumStatusesGoingBackward) )
            {
                return (false);
            }

            m_iMaximumStatusesGoingBackward = iMaximumStatusesGoingBackward;
        }

        m_bExpandStatusUrls = chkExpandStatusUrls.Checked;

        if (
            !ValidateRequiredTextBox(txbGraphServerUserName,

                "Enter the user name for your account on the NodeXL Graph"
                    + " Server.", 

                out m_sGraphServerUserName)
            ||
            !ValidateRequiredTextBox(txbGraphServerPassword,

                "Enter the password for your account on the NodeXL Graph"
                    + " Server.", 

                out m_sGraphServerPassword)
            )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeToControls()
    //
    /// <summary>
    /// Transfers data from the dialog's fields to its controls.
    /// </summary>
    //*************************************************************************

    protected void
    DoDataExchangeToControls()
    {
        txbSearchTerm.Text = m_sSearchTerm;
        dtpStatusDate1Utc.Value = m_oStatusDate1Utc;

        radUseDateRange.Checked = m_bUseDateRange;
        radUseMaximumStatusesGoingBackward.Checked = !m_bUseDateRange;

        dtpStatusDate2Utc.Value = m_oStatusDate2Utc;
        nudMaximumStatusesGoingBackward.Value = m_iMaximumStatusesGoingBackward;

        chkExpandStatusUrls.Checked = m_bExpandStatusUrls;
        txbGraphServerUserName.Text = m_sGraphServerUserName;
        txbGraphServerPassword.Text = m_sGraphServerPassword;

        EnableControls();
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the analysis.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) was called and succeeded.
    /// </remarks>
    //*************************************************************************

    protected override void
    StartAnalysis()
    {
        AssertValid();

        m_oGraphMLXmlDocument = null;

        Debug.Assert(m_oHttpNetworkAnalyzer is
            GraphServerTwitterSearchNetworkAnalyzer);

        GraphServerTwitterSearchNetworkAnalyzer
            oGraphServerTwitterSearchNetworkAnalyzer =
                (GraphServerTwitterSearchNetworkAnalyzer)
                    m_oHttpNetworkAnalyzer;

        if (m_bUseDateRange)
        {
            oGraphServerTwitterSearchNetworkAnalyzer.GetNetworkAsync(
                m_sSearchTerm,
                m_oStatusDate1Utc,
                AddDayToDate(m_oStatusDate2Utc),
                m_bExpandStatusUrls,
                m_sGraphServerUserName,
                m_sGraphServerPassword
                );
        }
        else
        {
            oGraphServerTwitterSearchNetworkAnalyzer.GetNetworkAsync(
                m_sSearchTerm,
                AddDayToDate(m_oStatusDate1Utc),
                m_iMaximumStatusesGoingBackward,
                m_bExpandStatusUrls,
                m_sGraphServerUserName,
                m_sGraphServerPassword
                );
        }
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected override void
    EnableControls()
    {
        AssertValid();

        Boolean bUseDateRange = radUseDateRange.Checked;
        flpUseDateRange.Enabled = bUseDateRange;
        flpUseMaximumStatuses.Enabled = !bUseDateRange;

        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        EnableControls(!bIsBusy, pnlUserInputs);
        btnOK.Enabled = !bIsBusy;
        this.UseWaitCursor = bIsBusy;
    }

    //*************************************************************************
    //  Method: OnEmptyGraph()
    //
    /// <summary>
    /// Handles the case where a graph was successfully obtained by is empty.
    /// </summary>
    //*************************************************************************

    protected override void
    OnEmptyGraph()
    {
        AssertValid();

        this.ShowInformation(
            "That network is not available from the NodeXL Graph Server."
            );

        txbSearchTerm.Focus();
    }

    //*************************************************************************
    //  Method: AddDayToDate()
    //
    /// <summary>
    /// Adds one day minus one second to a date.
    /// </summary>
    ///
    /// <param name="oDate">
    /// Date to add to.  The time component must be zero.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="oDate" /> with one day minus one second added
    /// to it.
    /// </returns>
    ///
    /// <remarks>
    /// The dialog's DateTimePicker controls provide dates that have their time
    /// components set to zero.  This method sets the time component of the
    /// maximum date to 23:59:59.
    ///
    /// <para>
    /// If the user specifies a date range of 5/1/2013 through 5/2/2013, for
    /// example, the range to request from the Graph Server should be 5/1/2013
    /// at 00:00:00 through 5/2/2013 at 23:59:59.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected DateTime
    AddDayToDate
    (
        DateTime oDate
    )
    {
        Debug.Assert(oDate.TimeOfDay == TimeSpan.Zero);
        AssertValid();

        return ( oDate.AddDays(1).AddSeconds(-1) );
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnOKClick();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_sSearchTerm != null);
        // m_bUseDateRange
        // m_oStatusDate1Utc
        // m_oStatusDate2Utc
        // m_iMaximumStatusesGoingBackward
        // m_bExpandStatusUrls
        Debug.Assert(m_sGraphServerUserName != null);
        Debug.Assert(m_sGraphServerPassword != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The search term to use.

    protected static String m_sSearchTerm = "NodeXL";

    /// true to use a date range, false to use a maximum number of statuses
    /// going backward in time.

    protected static Boolean m_bUseDateRange = false;

    /// First status date, in UTC.

    protected static DateTime m_oStatusDate1Utc = DateTime.Now.Date;

    /// Second status date, in UTC.

    protected static DateTime m_oStatusDate2Utc = DateTime.Now.Date;

    /// Maximum number of statuses to get, going backward in time.

    protected static Int32 m_iMaximumStatusesGoingBackward = 1000;

    /// true to expand the URLs in each status.

    protected static Boolean m_bExpandStatusUrls = false;

    /// User name for the account to use on the NodeXL Graph Server.

    protected static String m_sGraphServerUserName = String.Empty;

    /// Password for the account to use on the NodeXL Graph Server.

    protected static String m_sGraphServerPassword = String.Empty;
}
}
