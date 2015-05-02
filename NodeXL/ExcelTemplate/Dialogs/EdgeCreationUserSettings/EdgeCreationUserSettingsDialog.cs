

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeCreationUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="EdgeCreationUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="EdgeCreationUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class EdgeCreationUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: EdgeCreationUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeCreationUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="edgeCreationUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public EdgeCreationUserSettingsDialog
    (
        EdgeCreationUserSettings edgeCreationUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(edgeCreationUserSettings != null);
        Debug.Assert(workbook != null);

        m_oEdgeCreationUserSettings = edgeCreationUserSettings;

        InitializeComponent();

        // Populate the column name ComboBoxes with column names from the
        // Vertex Worksheet.

        ListObject oTable;


        if ( ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oTable) )
        {
            cbxVertexColumn.PopulateWithTableColumnNames(oTable);
            
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oEdgeCreationUserSettingsDialogUserSettings =
            new EdgeCreationUserSettingsDialogUserSettings(this);

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
        String sTextColumnName;
        

        if (bFromControls)
        {
            

            ComboBox oComboBoxToValidate = (ComboBox)cbxVertexColumn;

            if ( !ValidateRequiredComboBox(oComboBoxToValidate,
                "Enter or select the column containing the text.",
                out sTextColumnName) )
            {
                return (false);
            }

           

            m_oEdgeCreationUserSettings.TextColumnName = sTextColumnName;
            m_oEdgeCreationUserSettings.Threshold = Convert.ToInt32(nudThreshold.Value);
            m_oEdgeCreationUserSettings.EdgeLimit = Convert.ToInt32(nudEdgeLimit.Value);
            m_oEdgeCreationUserSettings.LimitToIsolate = chkLimitToIsolate.Checked;
            
        }
        else
        {
            sTextColumnName = m_oEdgeCreationUserSettings.TextColumnName;
            
            nudThreshold.Value = m_oEdgeCreationUserSettings.Threshold;
            nudEdgeLimit.Value = m_oEdgeCreationUserSettings.EdgeLimit;
            chkLimitToIsolate.Checked = m_oEdgeCreationUserSettings.LimitToIsolate;
          
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
        System.EventArgs e
    )
    {
        if ( DoDataExchange(true) )
        {
            DialogResult = DialogResult.OK;
            this.Close();
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

        Debug.Assert(m_oEdgeCreationUserSettings != null);
        Debug.Assert(m_oEdgeCreationUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being edited.

    protected EdgeCreationUserSettings m_oEdgeCreationUserSettings;

    /// User settings for this dialog.

    protected EdgeCreationUserSettingsDialogUserSettings
        m_oEdgeCreationUserSettingsDialogUserSettings;

    
}


//*****************************************************************************
//  Class: EdgeCreationUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="EdgeCreationUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("EdgeCreationUserSettingsDialog") ]

public class EdgeCreationUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: EdgeCreationUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeCreationUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public EdgeCreationUserSettingsDialogUserSettings
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

    // (None.)
}
}
