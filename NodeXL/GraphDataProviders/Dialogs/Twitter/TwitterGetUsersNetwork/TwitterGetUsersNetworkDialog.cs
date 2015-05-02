

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using Smrf.AppLib;
using Smrf.SocialNetworkLib.Twitter;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetUsersNetworkDialog
//
/// <summary>
/// Gets a network that shows the connections between a set of Twitter users
/// specified by either a Twitter List name or a set of Twitter screen names.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetUsersNetworkDialog
    : TwitterGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetUsersNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetUsersNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetUsersNetworkDialog()
    :
    base ( new TwitterUsersNetworkAnalyzer() )
    {
        InitializeComponent();
        InitializeTwitterAuthorizationManager(usrTwitterAuthorization);

        // A Twitter screen name can have 15 characters.  Allow for a LF-CR
        // between screen names in the TextBox.

        txbScreenNames.MaxLength = MaximumScreenNames * (15 + 2);

        SetHelpText();

        // m_bUseListName
        // m_sScreenNames
        // m_sListName
        // m_eNetworkType
        // m_bLimitToSpecifiedUsers
        // m_iMaximumStatusesPerUser
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

            "When you select this option, NodeXL analyzes each user's most"
            + " recent tweets."
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
            ;

        hllNetworkTypeBasicPlusFollows.Tag = String.Format(

            "Importing the network can take a long time when you select this"
            + " option."
            + "\r\n\r\n" 
            + "Due to Twitter rate limiting, NodeXL can import the friends"
            + " and followers of only fifteen users every fifteen minutes.  If"
            + " you specify more than fifteen usernames (or you specify a"
            + " Twitter List that has more than fifteen users), NodeXL has to"
            + " pause before Twitter will allow it to continue importing the"
            + " network."
            + "\r\n\r\n" 
            + " If you specify 120 users, for example, and you ask for friends"
            + " and followers, it will take at least two hours for NodeXL to"
            + " import the network."
            + "\r\n\r\n" 
            + "Because it takes so long to import friends and followers,"
            + " NodeXL doesn't attempt to import all of them.  Instead, it"
            + " asks Twitter for each user's {0:N0} newest friends and"
            + " followers.  If a user has {1:N0} friends, for example, NodeXL"
            + " will import only her {0:N0} newest friends."
            ,
            TwitterUsersNetworkAnalyzer.MaximumFriendsOrFollowers,
            2 * TwitterUsersNetworkAnalyzer.MaximumFriendsOrFollowers
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
            m_bUseListName = radUseListName.Checked;

            if (m_bUseListName)
            {
                if ( !ValidateListName(out m_sListName) )
                {
                    return (false);
                }
            }
            else
            {
                if ( !ValidateScreenNames(out m_sScreenNames) )
                {
                    return (false);
                }
            }

            m_eNetworkType = GetNetworkType();
            m_bLimitToSpecifiedUsers = chkLimitToSpecifiedUsers.Checked;
            m_iMaximumStatusesPerUser = (Int32)nudMaximumStatusesPerUser.Value;
            m_bExpandStatusUrls = chkExpandStatusUrls.Checked;
        }
        else
        {
            radUseListName.Checked = m_bUseListName;
            radUseScreenNames.Checked = !m_bUseListName;

            txbListName.Text = m_sListName;
            txbScreenNames.Text = m_sScreenNames;

            SetNetworkType(m_eNetworkType);
            chkLimitToSpecifiedUsers.Checked = m_bLimitToSpecifiedUsers;
            nudMaximumStatusesPerUser.Value = m_iMaximumStatusesPerUser;
            chkExpandStatusUrls.Checked = m_bExpandStatusUrls;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateListName()
    //
    /// <summary>
    /// Validates the Twitter list name.
    /// </summary>
    ///
    /// <param name="sListName">
    /// Where the list name gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the list name is valid.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateListName
    (
        out String sListName
    )
    {
        const String ListNameMessage =

            "Enter a Twitter List name.  A List name looks like \"bob/bobs\","
            + " where \"bob\" is a Twitter username and \"bobs\" is a List"
            + " that bob created."
            ;

        if ( !ValidateRequiredTextBox(txbListName, ListNameMessage,
            out sListName) )
        {
            return (false);
        }

        String sSlug, sOwnerScreenName;

        if ( !TwitterListNameParser.TryParseListName(
            sListName, out sSlug, out sOwnerScreenName) )
        {
            return ( OnInvalidTextBox(txbListName, ListNameMessage) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateScreenNames()
    //
    /// <summary>
    /// Validates the Twitter screen names.
    /// </summary>
    ///
    /// <param name="sScreenNames">
    /// Where the delimited screen names get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the screen names are valid.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateScreenNames
    (
        out String sScreenNames
    )
    {
        const String ScreenNamesMessage =

            "Enter a set of Twitter usernames separated with spaces, commas or"
            + " returns."
            ;

        if ( !ValidateRequiredTextBox(txbScreenNames, ScreenNamesMessage,
            out sScreenNames) )
        {
            return (false);
        }

        String [] asScreenNames =
            StringUtil.SplitOnCommonDelimiters(sScreenNames);

        if (asScreenNames.Length > MaximumScreenNames)
        {
            return (OnInvalidTextBox(txbScreenNames, String.Format(

                "The maximum number of usernames is {0}."
                ,
                MaximumScreenNames
                ) ) );
        }

        foreach (String sScreenName in asScreenNames)
        {
            if (sScreenName.Length > 15)
            {
                return ( OnInvalidTextBox(txbScreenNames,

                    String.Format(

                        "The username \"{0}\" is too long.  Twitter"
                        + " usernames can't be longer than 15"
                        + " characters."
                        ,
                        sScreenName
                        ) ) );
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetNetworkType()
    //
    /// <summary>
    /// Gets the network type selected by the user.
    /// </summary>
    ///
    /// <returns>
    /// The network type selected by the user.
    /// </returns>
    //*************************************************************************

    protected TwitterUsersNetworkAnalyzer.NetworkType
    GetNetworkType()
    {
        AssertValid();

        if (radNetworkTypeBasic.Checked)
        {
            return (TwitterUsersNetworkAnalyzer.NetworkType.Basic);
        }

        return (TwitterUsersNetworkAnalyzer.NetworkType.BasicPlusFollows);
    }

    //*************************************************************************
    //  Method: SetNetworkType()
    //
    /// <summary>
    /// Sets the network type controls.
    /// </summary>
    ///
    /// <param name="eNetworkType">
    /// The network type to set.
    /// </param>
    //*************************************************************************

    protected void
    SetNetworkType
    (
        TwitterUsersNetworkAnalyzer.NetworkType eNetworkType
    )
    {
        AssertValid();

        switch (eNetworkType)
        {
            case TwitterUsersNetworkAnalyzer.NetworkType.Basic:

                radNetworkTypeBasic.Checked = true;
                break;

            case TwitterUsersNetworkAnalyzer.NetworkType.BasicPlusFollows:

                radNetworkTypeBasicPlusFollows.Checked = true;
                break;

             default:

                Debug.Assert(false);
                break;
        }
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

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterUsersNetworkAnalyzer);

        ( (TwitterUsersNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_bUseListName, m_sListName,
                StringUtil.SplitOnCommonDelimiters(m_sScreenNames),
                m_eNetworkType, m_bLimitToSpecifiedUsers,
                m_iMaximumStatusesPerUser, m_bExpandStatusUrls);
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

        Boolean bUseListName = radUseListName.Checked;
        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        txbListName.Enabled = bUseListName;
        EnableControls(!bUseListName, txbScreenNames, lblScreenNamesHelp);
        EnableControls(!bIsBusy, pnlUserInputs, btnOK);

        this.UseWaitCursor = bIsBusy;
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

        String sMessage;
        TextBox oTextBoxToFocus;

        if (m_bUseListName)
        {
            sMessage = "There is no such Twitter List.";
            oTextBoxToFocus = txbListName;
        }
        else
        {
            sMessage =
                "Either there are no such users, or they have all protected"
                + " their Twitter accounts.";

            oTextBoxToFocus = txbScreenNames;
        }

        this.ShowInformation(sMessage);
        oTextBoxToFocus.Focus();
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
        //   Basic.png
        //   BasicPlusFollows.png
        //   BasicPlusFollowsLimitToSpecifiedUsers.png
        //   BasicLimitToSpecifiedUsers.png

        String sResourceName = String.Format(

            "Images.TwitterUsersNetworkPreview.{0}{1}.png"
            ,
            GetNetworkType(),  // Sample text: "Basic"

            chkLimitToSpecifiedUsers.Checked ?
                "LimitToSpecifiedUsers" : String.Empty
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

        // m_bUseListName
        // m_sScreenNames
        // m_sListName
        // m_eNetworkType
        // m_bExpandStatusUrls
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of screen names to allow.

    protected const Int32 MaximumScreenNames = 10000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// true if the users were specified as a Twitter List name, false if they
    /// were specified as a set of screen names.

    protected static Boolean m_bUseListName = false;

    /// One or more Twitter screen names delimited with spaces, commas or
    /// newlines if m_bUseListName is false; unused otherwise.

    protected static String m_sScreenNames = "bob nora dan";

    /// Twitter List name if m_bUseListName is true; unused otherwise.

    protected static String m_sListName = "bob/bobs";

    ///  Specifies the type of Twitter users network to get.

    protected static TwitterUsersNetworkAnalyzer.NetworkType m_eNetworkType;

    /// true if only the specified users should be included.

    protected static Boolean m_bLimitToSpecifiedUsers = true;

    /// Maximum number of recent statuses to request for each specified user.

    protected static Int32 m_iMaximumStatusesPerUser = 200;

    /// true to expand the URLs in the statuses.

    protected static Boolean m_bExpandStatusUrls = true;
}

}
