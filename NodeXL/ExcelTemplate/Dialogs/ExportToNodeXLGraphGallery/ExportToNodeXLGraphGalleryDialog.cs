

using System;
using System.Configuration;
using System.Windows.Forms;
using System.ServiceModel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToNodeXLGraphGalleryDialog
//
/// <summary>
/// Exports the graph to the NodeXL Graph Gallery.
/// </summary>
///
/// <remarks>
/// Most of the work is done by an internal <see
/// cref="NodeXLGraphGalleryExporter" /> object.
/// </remarks>
//*****************************************************************************

public partial class ExportToNodeXLGraphGalleryDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ExportToNodeXLGraphGalleryDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToNodeXLGraphGalleryDialog" /> class.
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

    public ExportToNodeXLGraphGalleryDialog
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

        lnkNodeXLGraphGallery.Tag = ProjectInformation.NodeXLGraphGalleryUrl;

        usrExportedFilesDescription.Workbook = workbook;

        lnkCreateAccount.Tag =
            ProjectInformation.NodeXLGraphGalleryCreateAccountUrl;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oExportToNodeXLGraphGalleryDialogUserSettings =
            new ExportToNodeXLGraphGalleryDialogUserSettings(this);

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
            if ( !usrExportedFilesDescription.Validate() )
            {
                return (false);
            }

            m_oExportToNodeXLGraphGalleryDialogUserSettings.UseCredentials =
                radUseCredentials.Checked;

            String sAuthor;

            if (m_oExportToNodeXLGraphGalleryDialogUserSettings.UseCredentials)
            {
                if (
                    !ValidateRequiredTextBox(txbAuthor,
                        "Enter a user name.", out sAuthor)
                    ||
                    !ValidateRequiredTextBox(txbPassword,
                        "Enter a password.", out m_sPassword)
                    )
                {
                    return (false);
                }
            }
            else
            {
                if (
                    !ValidateRequiredTextBox(txbGuestName,
                        "Enter a guest name.", out sAuthor)
                    )
                {
                    return (false);
                }
            }

            if ( !usrExportedFiles.Validate() )
            {
                return (false);
            }

            m_oExportToNodeXLGraphGalleryDialogUserSettings.Title =
                usrExportedFilesDescription.Title;

            m_oExportToNodeXLGraphGalleryDialogUserSettings.Description =
                usrExportedFilesDescription.Description;

            m_oExportToNodeXLGraphGalleryDialogUserSettings.SpaceDelimitedTags =
                txbSpaceDelimitedTags.Text.Trim();

            m_oExportToNodeXLGraphGalleryDialogUserSettings.
                ExportWorkbookAndSettings =
                usrExportedFiles.ExportWorkbookAndSettings;

            m_oExportToNodeXLGraphGalleryDialogUserSettings.ExportGraphML =
                usrExportedFiles.ExportGraphML;

            m_oExportToNodeXLGraphGalleryDialogUserSettings
                .UseFixedAspectRatio = usrExportedFiles.UseFixedAspectRatio;

            m_oExportToNodeXLGraphGalleryDialogUserSettings.Author = sAuthor;
        }
        else
        {
            usrExportedFilesDescription.Title =
                m_oExportToNodeXLGraphGalleryDialogUserSettings.Title;

            usrExportedFilesDescription.Description =
                m_oExportToNodeXLGraphGalleryDialogUserSettings.Description;

            txbSpaceDelimitedTags.Text =
                m_oExportToNodeXLGraphGalleryDialogUserSettings.
                SpaceDelimitedTags;

            usrExportedFiles.ExportWorkbookAndSettings =
                m_oExportToNodeXLGraphGalleryDialogUserSettings.
                ExportWorkbookAndSettings;

            usrExportedFiles.ExportGraphML =
                m_oExportToNodeXLGraphGalleryDialogUserSettings.ExportGraphML;

            usrExportedFiles.UseFixedAspectRatio = 
                m_oExportToNodeXLGraphGalleryDialogUserSettings
                .UseFixedAspectRatio;

            if (m_oExportToNodeXLGraphGalleryDialogUserSettings.UseCredentials)
            {
                radUseCredentials.Checked = true;

                txbAuthor.Text =
                    m_oExportToNodeXLGraphGalleryDialogUserSettings.Author;

                txbPassword.Text = m_sPassword;
            }
            else
            {
                radAsGuest.Checked = true;

                txbGuestName.Text =
                    m_oExportToNodeXLGraphGalleryDialogUserSettings.Author;
            }

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        EnableControls(radAsGuest.Checked,
            lblGuestName, txbGuestName);

        EnableControls(radUseCredentials.Checked,
            lblAuthor, txbAuthor, lblPassword, txbPassword);
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

    private void
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

        const String NotReachableMessage =
            "The NodeXL Graph Gallery couldn't be reached.  ";

        const String TryAgainMessage = "Try again later.";

        NodeXLGraphGalleryExporter oNodeXLGraphGalleryExporter =
            new NodeXLGraphGalleryExporter();

        this.Cursor = Cursors.WaitCursor;

        try
        {
            oNodeXLGraphGalleryExporter.ExportToNodeXLGraphGallery(

                m_oWorkbook, m_oNodeXLControl,
                m_oExportToNodeXLGraphGalleryDialogUserSettings.Title,
                m_oExportToNodeXLGraphGalleryDialogUserSettings.Description,

                m_oExportToNodeXLGraphGalleryDialogUserSettings.
                    SpaceDelimitedTags,

                m_oExportToNodeXLGraphGalleryDialogUserSettings.Author,

                m_oExportToNodeXLGraphGalleryDialogUserSettings.UseCredentials
                    ? m_sPassword : null,

                m_oExportToNodeXLGraphGalleryDialogUserSettings.
                    ExportWorkbookAndSettings,

                m_oExportToNodeXLGraphGalleryDialogUserSettings.ExportGraphML,

                m_oExportToNodeXLGraphGalleryDialogUserSettings
                    .UseFixedAspectRatio
                );
        }
        catch (GraphTooLargeException)
        {
            this.ShowWarning(
                "The graph is too large to export.  Uncheck the"
                + " \"Also export the workbook and its options\" or"
                + " \"Also export the graph data as GraphML\" checkboxes and"
                + " try again."
                );

            return;
        }
        catch (FaultException<String> oFaultException)
        {
            // The GraphDataSource.AddGraph() method throws a
            // FaultException<String> with a friendly error message when the
            // graph can't be added for a known reason, such as an invalid
            // author.

            this.ShowWarning(oFaultException.Detail);
            txbAuthor.Focus();
            return;
        }
        catch (FaultException)
        {
            // This is an unexpected error.

            this.ShowWarning(
                "A problem occurred within the NodeXL Graph Gallery.  "
                + TryAgainMessage);
        }
        catch (TimeoutException)
        {
            this.ShowWarning(
                NotReachableMessage + TryAgainMessage);
        }
        catch (CommunicationException)
        {
            this.ShowWarning(
                NotReachableMessage + TryAgainMessage);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
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

        Debug.Assert(m_oExportToNodeXLGraphGalleryDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oNodeXLControl != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ExportToNodeXLGraphGalleryDialogUserSettings
        m_oExportToNodeXLGraphGalleryDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// NodeXLControl containing the graph.

    protected NodeXLControl m_oNodeXLControl;

    /// This is static so that the password is retained while Excel is open.
    /// The password isn't saved in the user's settings.

    protected static String m_sPassword = String.Empty;
}


//*****************************************************************************
//  Class: ExportToNodeXLGraphGalleryDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ExportToNodeXLGraphGalleryDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToNodeXLGraphGalleryDialog") ]

public class ExportToNodeXLGraphGalleryDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ExportToNodeXLGraphGalleryDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToNodeXLGraphGalleryDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ExportToNodeXLGraphGalleryDialogUserSettings
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
    //  Property: Title
    //
    /// <summary>
    /// Gets or sets the graph's title.
    /// </summary>
    ///
    /// <value>
    /// The graph's title.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Title
    {
        get
        {
            AssertValid();

            return ( (String)this[TitleKey] );
        }

        set
        {
            this[TitleKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets or sets the graph's description.
    /// </summary>
    ///
    /// <value>
    /// The graph's description.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Description
    {
        get
        {
            AssertValid();

            return ( (String)this[DescriptionKey] );
        }

        set
        {
            this[DescriptionKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SpaceDelimitedTags
    //
    /// <summary>
    /// Gets or sets the graph's space-delimited tags.
    /// </summary>
    ///
    /// <value>
    /// The graph's space-delimited tags.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SpaceDelimitedTags
    {
        get
        {
            AssertValid();

            return ( (String)this[SpaceDelimitedTagsKey] );
        }

        set
        {
            this[SpaceDelimitedTagsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Author
    //
    /// <summary>
    /// Gets or sets the graph's author.
    /// </summary>
    ///
    /// <value>
    /// The graph's author.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Author
    {
        get
        {
            AssertValid();

            return ( (String)this[AuthorKey] );
        }

        set
        {
            this[AuthorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseCredentials
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the user has credentials for the
    /// NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <value>
    /// true if the user has credentials.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseCredentials
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseCredentialsKey] );
        }

        set
        {
            this[UseCredentialsKey] = value;

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

    /// Name of the settings key for the Title property.

    protected const String TitleKey =
        "Title";

    /// Name of the settings key for the Description property.

    protected const String DescriptionKey =
        "Description";

    /// Name of the settings key for the SpaceDelimitedTags property.

    protected const String SpaceDelimitedTagsKey =
        "SpaceDelimitedTags";

    /// Name of the settings key for the Author property.

    protected const String AuthorKey =
        "Author";

    /// Name of the settings key for the ExportWorkbookAndSettings property.

    protected const String ExportWorkbookAndSettingsKey =
        "ExportWorkbookAndSettings";

    /// Name of the settings key for the ExportGraphML property.

    protected const String ExportGraphMLKey =
        "ExportGraphML";

    /// Name of the settings key for the UseFixedAspectRatio property.

    protected const String UseFixedAspectRatioKey =
        "UseFixedAspectRatio";

    /// Name of the settings key for the UseCredentials property.

    protected const String UseCredentialsKey =
        "UseCredentials";
}
}
