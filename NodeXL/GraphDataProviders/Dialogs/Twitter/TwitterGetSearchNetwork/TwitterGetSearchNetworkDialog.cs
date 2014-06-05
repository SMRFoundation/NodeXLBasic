

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetSearchNetworkDialog
//
/// <summary>
/// Gets the the network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetSearchNetworkDialog :
    TwitterGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetSearchNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetSearchNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetSearchNetworkDialog()
    :
    base( new TwitterSearchNetworkAnalyzer() )
    {
        InitializeComponent();
        InitializeTwitterAuthorizationManager(usrTwitterAuthorization);

        SetHelpText();

        // m_sSearchTerm
        // m_bIncludeFriendEdges
        // m_iMaximumStatuses
        // m_bExpandStatusUrls

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
    //  Method: SetHelpText()
    //
    /// <summary>
    /// Sets the text in the dialog's HelpLinkLabels.
    /// </summary>
    //*************************************************************************

    protected void
    SetHelpText()
    {
        // The text for the HelpLinkLabels is set programmatically instead of
        // via the designer because one of them uses String.Format().

        hllNetworkTypeBasic.Tag = 

            "When you select this option, NodeXL asks Twitter for recent"
            + " tweets that match your search query.  It then creates a vertex"
            + " for each unique user who either tweeted one of those tweets,"
            + " was replied to in one of those tweets, or was mentioned in one"
            + " of those tweets.  Finally, it connects those vertices with"
            + " edges."
            + "\r\n\r\n" 
            + "If a tweet was a reply to someone else, NodeXL creates an edge"
            + " from the tweeter to the replied-to user and gives it a"
            + " \"Relationship\" value of \"Replies-to.\"  There can be only"
            + " one such Replies-to edge for each tweet."
            + "\r\n\r\n" 
            + "If a tweet mentioned someone else, NodeXL creates an edge from"
            + " the tweeter to the mentioned user and gives it a"
            + " \"Relationship\" value of \"Mentions.\"  There can be multiple"
            + " Mentions edges for each tweet.  (Note that a \"Replies-to\" is"
            + " NOT also a \"Mentions.\")"
            + "\r\n\r\n" 
            + "If a tweet neither replied to nor mentioned anyone else, NodeXL"
            + " creates a self-loop edge from the tweeter to herself and gives"
            + " it a \"Relationship\" value of \"Tweet.\""
            + "\r\n\r\n" 
            + "Here are a few important limitations you should be aware of:"
            + "\r\n\r\n" 
            + "1. The search results provided by Twitter are often"
            + " incomplete--you will most likely NOT get all recent tweets"
            + " that match your search query.  The way Twitter puts it is that"
            + " the results are \"focused on relevance and not completeness.\""
            + "\r\n\r\n" 
            + "2. Twitter will not provide NodeXL with tweets older than about"
            + " a week.  It is NOT possible to use NodeXL to get tweets older"
            + " than that."
            + "\r\n\r\n" 
            + "3. The algorithm that Twitter uses to match tweets with a search"
            + " query is undocumented.  It is NOT, however, the same algorithm"
            + " that Twitter uses on its own search page, so you may get"
            + " results from NodeXL that differ from what you get directly"
            + " from Twitter using the same search query."
            ;

        hllNetworkTypeBasicPlusFollows.Tag = String.Format(

            "Importing the network can take a long time when you select this"
            + " option."
            + "\r\n\r\n" 
            + "Due to Twitter rate limiting, NodeXL can import the friends"
            + " of only fifteen users every fifteen minutes.  If the network"
            + " has more than fifteen users, NodeXL has to pause before"
            + " Twitter will allow it to continue importing the network."
            + "\r\n\r\n" 
            + "If the network has 300 users, for example, and you ask for"
            + " friends, it will take at least five hours for NodeXL to import"
            + " the network."
            + "\r\n\r\n" 
            + "Because it takes so long to import friends, NodeXL doesn't"
            + " attempt to import all of them.  Instead, it asks Twitter for"
            + " each user's {0:N0} newest friends.  If a user has {1:N0}"
            + " friends, for example, NodeXL will import only her {0:N0}"
            + " newest friends."
            ,
            TwitterSearchNetworkAnalyzer.MaximumFriends,
            2 * TwitterSearchNetworkAnalyzer.MaximumFriends
            );
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
            // Validate the controls.

            if ( !ValidateRequiredTextBox(txbSearchTerm,
                    "Enter one or more words to search for.",
                    out m_sSearchTerm) )
            {
                return (false);
            }

            String sSearchTermLower = m_sSearchTerm.ToLower();

            foreach (String sProhibitedTerm in new String [] {
                "near:",
                "within:"
                } )
            {
                if (sSearchTermLower.IndexOf(sProhibitedTerm) >= 0)
                {
                    OnInvalidTextBox(txbSearchTerm, String.Format(
                    
                        "Although you can use \"{0}\" on Twitter's own search"
                        + " page, Twitter doesn't allow it to be used when"
                        + " searching from another program, such as NodeXL."
                        + "  Remove the \"{0}\" and try again."
                        ,
                        sProhibitedTerm
                        ) );

                    return (false);
                }
            }

            m_bIncludeFriendEdges = radIncludeFriendEdges.Checked;
            m_iMaximumStatuses = (Int32)nudMaximumStatuses.Value;
            m_bExpandStatusUrls = chkExpandStatusUrls.Checked;
        }
        else
        {
            txbSearchTerm.Text = m_sSearchTerm;
            radIncludeFriendEdges.Checked = m_bIncludeFriendEdges;
            radNoFriendEdges.Checked = !m_bIncludeFriendEdges;
            nudMaximumStatuses.Value = m_iMaximumStatuses;
            chkExpandStatusUrls.Checked = m_bExpandStatusUrls;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the Twitter analysis.
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

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterSearchNetworkAnalyzer);

        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bExpandStatusUrls ?
                TwitterSearchNetworkAnalyzer
                    .WhatToInclude.ExpandedStatusUrls : 0)
            |
            (m_bIncludeFriendEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges : 0)
            ;

        ( (TwitterSearchNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sSearchTerm, eWhatToInclude, m_iMaximumStatuses);
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

        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        EnableControls(!bIsBusy, pnlUserInputs);
        btnOK.Enabled = !bIsBusy;
        this.UseWaitCursor = bIsBusy;
    }

    //*************************************************************************
    //  Method: TwitterGetSearchNetworkDialog_KeyDown()
    //
    /// <summary>
    /// Handles the KeyDown event on the TwitterGetSearchNetworkDialog.
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
    TwitterGetSearchNetworkDialog_KeyDown
    (
        object sender,
        KeyEventArgs e
    )
    {
        if (
            e.KeyCode == Keys.M
            &&
            e.Control
            &&
            DoDataExchange(true)
            )
        {
            // Ctrl+M is a special shortcut that sets controls to get a large
            // network.

            m_iMaximumStatuses = 18000;
            m_bExpandStatusUrls = true;

            DoDataExchange(false);
        }
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

        this.ShowInformation("There are no people in that network.");
        txbSearchTerm.Focus();
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresImageChange()
    //
    /// <summary>
    /// Handles any event that should change the image shown in the
    /// picNetworkPreview PictureBox.
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
    OnEventThatRequiresImageChange
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // The sample images displayed in the picNetworkPreview PictureBox are
        // stored as embedded resources.  Here are the file names, without
        // namespaces:
        //
        //   NoFriendEdges.png
        //   IncludeFriendEdges.png

        String sResourceName = String.Format(

            "Images.TwitterSearchNetworkPreview.{0}FriendEdges.png"
            ,
            radIncludeFriendEdges.Checked ? "Include" : "No"
            );

        // The namespace of the images is Smrf.NodeXL.GraphDataProviders, so
        // use a class that has that namespace as the "type" argument of the
        // Bitmap constructor.

        picNetworkPreview.Image = new Bitmap(
            typeof(GraphDataProviderBase), sResourceName);
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
        // m_bIncludeFriendEdges
        Debug.Assert(m_iMaximumStatuses > 0);
        Debug.Assert(m_iMaximumStatuses != Int32.MaxValue);
        // m_bExpandStatusUrls
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

    /// true to include friend edges.

    protected static Boolean m_bIncludeFriendEdges = false;

    /// Maximum number of tweets to request.

    protected static Int32 m_iMaximumStatuses = 100;

    /// true to expand the URLs in each status.

    protected static Boolean m_bExpandStatusUrls = false;
}

}
