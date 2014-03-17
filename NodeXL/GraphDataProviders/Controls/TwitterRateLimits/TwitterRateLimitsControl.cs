
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterRateLimitsControl
//
/// <summary>
/// UserControl that includes a link that explains Twitter rate limits.
/// </summary>
//*****************************************************************************

public partial class TwitterRateLimitsControl : UserControl
{
    //*************************************************************************
    //  Constructor: TwitterRateLimitsControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterRateLimitsControl" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterRateLimitsControl()
    {
        InitializeComponent();

        this.lnkRateLimiting.Text = RateLimitingLinkText;

        AssertValid();
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

            "NodeXL imports all information about Twitter users and tweets"
            + " directly from Twitter.  Twitter limits how often NodeXL can"
            + " request such information.  They call this \"rate limiting.\""
            + "  Depending on the type of network you import, you can easily"
            + " reach Twitter's limits."
            + "\r\n\r\n"
            + "When a limit is reached, NodeXL pauses for about fifteen minutes"
            + " until Twitter resets the limit.  These pauses can add up to a"
            + " long delay before the entire Twitter network is imported."
            + "\r\n\r\n"
            + "(As of June 2013, Twitter no longer offers higher limits to"
            + " \"whitelisted\" users.  Whitelisting has been discontinued.)"
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
        "This might take a long time: Twitter rate limiting";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
