
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAuthorizationControl
//
/// <summary>
/// UserControl that shows a user's Twitter authorization status and provides
/// some help links concerning whitelisting.
/// </summary>
///
/// <remarks>
/// Get and set the user's Twitter authorization status with the <see
/// cref="Status" /> property.
///
/// <para>
/// This control uses the following keyboard shortcuts: I, V, W.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class TwitterAuthorizationControl : UserControl
{
    //*************************************************************************
    //  Constructor: TwitterAuthorizationControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAuthorizationControl" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterAuthorizationControl()
    {
        InitializeComponent();

        this.Status = TwitterAuthorizationStatus.NoTwitterAccount;
        this.lnkRateLimiting.Text = RateLimitingLinkText;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Status
    //
    /// <summary>
    /// Sets the user's Twitter authorization status.
    /// </summary>
    ///
    /// <value>
    /// The user's Twitter authorization status, as a <see
    /// cref="TwitterAuthorizationStatus" />.
    /// </value>
    //*************************************************************************

    public TwitterAuthorizationStatus
    Status
    {
        set
        {
            // Note that unless the status is HasTwitterAccountAuthorized, the
            // radHasTwitterAccountAuthorized radio button is disabled.  The
            // user can't just declare that he has authorized NodeXL to use
            // his account.  That is determined by the
            // TwitterAuthorizationManager that sets this property.

            Boolean bEnableHasTwitterAccountAuthorized = false;

            switch (value)
            {
                case TwitterAuthorizationStatus.NoTwitterAccount:

                    radNoTwitterAccount.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized:

                    radHasTwitterAccountNotAuthorized.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountAuthorized:

                    radHasTwitterAccountAuthorized.Checked = true;
                    bEnableHasTwitterAccountAuthorized = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            radHasTwitterAccountAuthorized.Enabled =
                bEnableHasTwitterAccountAuthorized;

            AssertValid();
        }

        get
        {
            AssertValid();

            if (radNoTwitterAccount.Checked)
            {
                return (TwitterAuthorizationStatus.NoTwitterAccount);
            }

            if (radHasTwitterAccountNotAuthorized.Checked)
            {
                return (
                    TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized);
            }

            return (TwitterAuthorizationStatus.HasTwitterAccountAuthorized);
        }
    }

    //*************************************************************************
    //  Method: lnkRateLimiting_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkRateLimiting LinkLabel.
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

    private void
    lnkRateLimiting_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        FormUtil.ShowInformation(

            "To protect its Web service, Twitter limits the number of"
            + " information requests that can be made within a one-hour"
            + " period.  They call this \"rate limiting.\"  Depending on the"
            + " types of networks you import, you can easily reach Twitter's"
            + " limit."
            + "\r\n\r\n"
            + "The exact limit that Twitter imposes depends on several"
            + " factors.  If you do not have a Twitter account, or you do have"
            + " an account but you have not yet authorized NodeXL to use it to"
            + " import Twitter networks, then Twitter imposes a severe limit."
            + "  If you have authorized NodeXL to use your account, the limit"
            + " is somewhat higher."
            + "\r\n\r\n"
            + "When the limit is reached, NodeXL pauses for about an hour"
            + " until Twitter resets the limit.  These hour-long pauses can"
            + " add up to a long delay before the entire Twitter network is"
            + " imported."
            + "\r\n\r\n"
            + "(As of February 2011, Twitter no longer offers a"
            + " \"whitelisting\" option to people who need higher limits.)"
            );
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Text for the lnkRateLimiting LinkLabel.

    public const String RateLimitingLinkText = 
        "Why this might take a long time: Twitter rate limiting";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
