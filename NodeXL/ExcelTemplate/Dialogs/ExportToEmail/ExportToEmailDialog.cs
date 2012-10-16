

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Net.Mail;
using Smrf.AppLib;
using Smrf.NodeXL.Visualization.Wpf;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToEmailDialog
//
/// <summary>
/// Exports the graph to one or more email addresses.
/// </summary>
///
/// <remarks>
/// Most of the work is done by an internal <see cref="EmailExporter" />
/// object.
/// </remarks>
//*****************************************************************************

public partial class ExportToEmailDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ExportToEmailDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportToEmailDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    //*************************************************************************

    public ExportToEmailDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);

        m_oWorkbook = workbook;
        m_oNodeXLControl = nodeXLControl;

        InitializeComponent();

        usrExportedFilesDescription.Workbook = workbook;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oExportToEmailDialogUserSettings =
            new ExportToEmailDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
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

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            String sToAddresses, sFromAddress, sSmtpHost, sSmtpUserName,
                sSmtpPassword;

            Int32 iSmtpPort;

            if (
                !ValidateRequiredTextBox(txbToAddresses,

                    "Enter one or more \"to\" email addresses, separated with"
                    + " spaces, commas or returns.",

                    out sToAddresses)
                ||
                !ValidateRequiredTextBox(txbFromAddress,
                    "Enter a \"from\" email address.",
                    out sFromAddress)
                ||
                !this.usrExportedFilesDescription.Validate()
                ||
                !ValidateRequiredTextBox(txbSmtpHost,

                    "Enter the name of an SMTP server, such as"
                        + " smtp.gmail.com.",

                    out sSmtpHost)
                ||
                !ValidateInt32TextBox(txbSmtpPort, 1, 65535,
                    "Enter the SMTP port number.",
                    out iSmtpPort)
                ||
                !ValidateRequiredTextBox(txbSmtpUserName,
                    "Enter the user name for an account on the SMTP server.",
                    out sSmtpUserName)
                ||
                !ValidateRequiredTextBox(txbSmtpPassword,
                    "Enter the password for an account on the SMTP server.",
                    out sSmtpPassword)
                ||
                !usrExportedFiles.Validate()
                )
            {
                return (false);
            }

            m_oExportToEmailDialogUserSettings.SpaceDelimitedToAddresses =
                String.Join( " ", StringUtil.SplitOnCommonDelimiters(
                    sToAddresses) );

            m_oExportToEmailDialogUserSettings.FromAddress = sFromAddress;

            m_oExportToEmailDialogUserSettings.Subject =
                usrExportedFilesDescription.Title;

            m_oExportToEmailDialogUserSettings.MessageBody =
                usrExportedFilesDescription.Description;

            m_oExportToEmailDialogUserSettings.SmtpHost = sSmtpHost;
            m_oExportToEmailDialogUserSettings.SmtpPort = iSmtpPort;

            m_oExportToEmailDialogUserSettings.UseSslForSmtp =
                chkUseSslForSmtp.Checked;

            m_oExportToEmailDialogUserSettings.SmtpUserName = sSmtpUserName;
            m_sSmtpPassword = sSmtpPassword;

            m_oExportToEmailDialogUserSettings.ExportWorkbookAndSettings =
                usrExportedFiles.ExportWorkbookAndSettings;

            m_oExportToEmailDialogUserSettings.ExportGraphML =
                usrExportedFiles.ExportGraphML;

            m_oExportToEmailDialogUserSettings.UseFixedAspectRatio =
                usrExportedFiles.UseFixedAspectRatio;
        }
        else
        {
            txbToAddresses.Text =
                m_oExportToEmailDialogUserSettings.SpaceDelimitedToAddresses;

            txbFromAddress.Text =
                m_oExportToEmailDialogUserSettings.FromAddress;

            usrExportedFilesDescription.Title =
                m_oExportToEmailDialogUserSettings.Subject;

            usrExportedFilesDescription.Description =
                m_oExportToEmailDialogUserSettings.MessageBody;

            txbSmtpHost.Text = m_oExportToEmailDialogUserSettings.SmtpHost;

            txbSmtpPort.Text =
                m_oExportToEmailDialogUserSettings.SmtpPort.ToString(
                    CultureInfo.InvariantCulture);

            chkUseSslForSmtp.Checked =
                m_oExportToEmailDialogUserSettings.UseSslForSmtp;

            txbSmtpUserName.Text =
                m_oExportToEmailDialogUserSettings.SmtpUserName;

            txbSmtpPassword.Text = m_sSmtpPassword;

            usrExportedFiles.ExportWorkbookAndSettings =
                m_oExportToEmailDialogUserSettings.ExportWorkbookAndSettings;

            usrExportedFiles.ExportGraphML =
                m_oExportToEmailDialogUserSettings.ExportGraphML;

            usrExportedFiles.UseFixedAspectRatio = 
                m_oExportToEmailDialogUserSettings.UseFixedAspectRatio;
        }

        return (true);
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

    private void
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.Cursor = Cursors.WaitCursor;

        try
        {
            ( new EmailExporter() ).ExportToEmail(
                m_oWorkbook,
                m_oNodeXLControl,

                m_oExportToEmailDialogUserSettings
                    .SpaceDelimitedToAddresses.Split(' '),

                m_oExportToEmailDialogUserSettings.FromAddress,
                m_oExportToEmailDialogUserSettings.Subject,
                m_oExportToEmailDialogUserSettings.MessageBody,
                m_oExportToEmailDialogUserSettings.SmtpHost,
                m_oExportToEmailDialogUserSettings.SmtpPort,
                m_oExportToEmailDialogUserSettings.UseSslForSmtp,
                m_oExportToEmailDialogUserSettings.SmtpUserName,
                m_sSmtpPassword,
                m_oExportToEmailDialogUserSettings.ExportWorkbookAndSettings,
                m_oExportToEmailDialogUserSettings.ExportGraphML,
                m_oExportToEmailDialogUserSettings.UseFixedAspectRatio
                );
        }
        catch (SmtpException oSmtpException)
        {
            this.ShowWarning(
                "A problem occurred while sending the email.  Details:"
                + "\r\n\r\n"
                + oSmtpException
                );

            return;
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
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

        Debug.Assert(m_oExportToEmailDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oNodeXLControl != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ExportToEmailDialogUserSettings
        m_oExportToEmailDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// NodeXLControl containing the graph.

    protected NodeXLControl m_oNodeXLControl;

    /// This is static so that the password is retained while Excel is open.
    /// The password isn't saved in the user's settings.

    protected static String m_sSmtpPassword = String.Empty;
}


//*****************************************************************************
//  Class: ExportToEmailDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="ExportToEmailDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToEmailDialog") ]

public class ExportToEmailDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ExportToEmailDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToEmailDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ExportToEmailDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: SpaceDelimitedToAddresses
    //
    /// <summary>
    /// Gets or sets a space-delimited set of "to" email addresses.
    /// </summary>
    ///
    /// <value>
    /// A space-delimited set of "to" email addresses.  The default value is
    /// String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SpaceDelimitedToAddresses
    {
        get
        {
            AssertValid();

            return ( (String)this[SpaceDelimitedToAddressesKey] );
        }

        set
        {
            this[SpaceDelimitedToAddressesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FromAddress
    //
    /// <summary>
    /// Gets or sets the "from" email address.
    /// </summary>
    ///
    /// <value>
    /// The "from" email address.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    FromAddress
    {
        get
        {
            AssertValid();

            return ( (String)this[FromAddressKey] );
        }

        set
        {
            this[FromAddressKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Subject
    //
    /// <summary>
    /// Gets or sets the email subject.
    /// </summary>
    ///
    /// <value>
    /// The email subject.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Subject
    {
        get
        {
            AssertValid();

            return ( (String)this[SubjectKey] );
        }

        set
        {
            this[SubjectKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MessageBody
    //
    /// <summary>
    /// Gets or sets the email's message body.
    /// </summary>
    ///
    /// <value>
    /// The message body subject.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    MessageBody
    {
        get
        {
            AssertValid();

            return ( (String)this[MessageBodyKey] );
        }

        set
        {
            this[MessageBodyKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpHost
    //
    /// <summary>
    /// Gets or sets the SMTP host name.
    /// </summary>
    ///
    /// <value>
    /// The SMTP host name.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SmtpHost
    {
        get
        {
            AssertValid();

            return ( (String)this[SmtpHostKey] );
        }

        set
        {
            this[SmtpHostKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpPort
    //
    /// <summary>
    /// Gets or sets the SMTP port.
    /// </summary>
    ///
    /// <value>
    /// The SMTP port.  The default value is 587.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("587") ]

    public Int32
    SmtpPort
    {
        get
        {
            AssertValid();

            return ( (Int32)this[SmtpPortKey] );
        }

        set
        {
            this[SmtpPortKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseSslForSmtp
    //
    /// <summary>
    /// Gets or sets a flag specifying whether SSL should be used.
    /// </summary>
    ///
    /// <value>
    /// true to use SSL.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    UseSslForSmtp
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseSslForSmtpKey] );
        }

        set
        {
            this[UseSslForSmtpKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpUserName
    //
    /// <summary>
    /// Gets or sets the SMTP user name.
    /// </summary>
    ///
    /// <value>
    /// The SMTP user name.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SmtpUserName
    {
        get
        {
            AssertValid();

            return ( (String)this[SmtpUserNameKey] );
        }

        set
        {
            this[SmtpUserNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ExportWorkbookAndSettings
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the workbook and its settings
    /// should be exported.
    /// </summary>
    ///
    /// <value>
    /// true to export the workbook and its settings.  The default value is
    /// false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ExportWorkbookAndSettings
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ExportWorkbookAndSettingsKey] );
        }

        set
        {
            this[ExportWorkbookAndSettingsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ExportGraphML
    //
    /// <summary>
    /// Gets or sets a flag indicating whether GraphML should be exported.
    /// </summary>
    ///
    /// <value>
    /// true to export GraphML.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ExportGraphML
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ExportGraphMLKey] );
        }

        set
        {
            this[ExportGraphMLKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseFixedAspectRatio
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the exported image should have
    /// a fixed aspect ratio.
    /// </summary>
    ///
    /// <value>
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseFixedAspectRatio
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseFixedAspectRatioKey] );
        }

        set
        {
            this[UseFixedAspectRatioKey] = value;

            AssertValid();
        }
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the settings key for the SpaceDelimitedToAddresses property.

    protected const String SpaceDelimitedToAddressesKey =
        "SpaceDelimitedToAddresses";

    /// Name of the settings key for the FromAddress property.

    protected const String FromAddressKey =
        "FromAddress";

    /// Name of the settings key for the Subject property.

    protected const String SubjectKey =
        "Subject";

    /// Name of the settings key for the MessageBody property.

    protected const String MessageBodyKey =
        "MessageBody";

    /// Name of the settings key for the SmtpHost property.

    protected const String SmtpHostKey =
        "SmtpHost";

    /// Name of the settings key for the SmtpPort property.

    protected const String SmtpPortKey =
        "SmtpPort";

    /// Name of the settings key for the UseSslForSmtp property.

    protected const String UseSslForSmtpKey =
        "UseSslForSmtp";

    /// Name of the settings key for the SmtpUserName property.

    protected const String SmtpUserNameKey =
        "SmtpUserName";

    /// Name of the settings key for the ExportWorkbookAndSettings property.

    protected const String ExportWorkbookAndSettingsKey =
        "ExportWorkbookAndSettings";

    /// Name of the settings key for the ExportGraphML property.

    protected const String ExportGraphMLKey =
        "ExportGraphML";

    /// Name of the settings key for the UseFixedAspectRatio property.

    protected const String UseFixedAspectRatioKey =
        "UseFixedAspectRatio";
}
}
