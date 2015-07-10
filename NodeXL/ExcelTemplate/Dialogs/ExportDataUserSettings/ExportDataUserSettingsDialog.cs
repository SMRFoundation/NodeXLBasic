using Smrf.AppLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Smrf.NodeXL.ExcelTemplate
{
    public partial class ExportDataUserSettingsDialog : ExcelTemplateForm
    {
        public ExportDataUserSettingsDialog
        (
            ExportDataUserSettings exportDataUserSettings,
            ThisWorkbook thisWorkbook
        )
        {
            Debug.Assert(exportDataUserSettings != null);
            exportDataUserSettings.AssertValid();
            Debug.Assert(thisWorkbook != null);

            m_oExportDataUserSettings = exportDataUserSettings;            
            m_oThisWorkbook = thisWorkbook;

            // Instantiate an object that saves and retrieves the position of this
            // dialog.  Note that the object automatically saves the settings when
            // the form closes.

            m_oExportDataUserSettingsDialogUserSettings =
                new ExportDataUserSettingsDialogUserSettings(this);

            InitializeComponent();
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

                m_oExportDataUserSettings.Hashtag =
                    this.txbHashtag.Text;

                m_oExportDataUserSettings.URL =
                    this.txbURL.Text;
            }
            else
            {
                this.txbHashtag.Text =
                    m_oExportDataUserSettings.Hashtag;

                this.txbURL.Text =
                    m_oExportDataUserSettings.URL;
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
            AssertValid();

            if (DoDataExchange(true))
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

            Debug.Assert(m_oExportDataUserSettings != null);            
            Debug.Assert(m_oThisWorkbook != null);
            Debug.Assert(m_oExportDataUserSettingsDialogUserSettings != null);
        }


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// Object whose properties are being edited.

        protected ExportDataUserSettings m_oExportDataUserSettings;
                
        /// Workbook containing the graph contents.

        protected ThisWorkbook m_oThisWorkbook;

        /// User settings for this dialog.

        protected ExportDataUserSettingsDialogUserSettings
            m_oExportDataUserSettingsDialogUserSettings;
    }

    //*****************************************************************************
    //  Class: ExportDataUserSettingsDialogUserSettings
    //
    /// <summary>
    /// Stores the user's settings for the <see
    /// cref="ExportDataUserSettingsDialog" />.
    /// </summary>
    ///
    /// <remarks>
    /// The user settings include the form size and location.
    /// </remarks>
    //*****************************************************************************

    [SettingsGroupNameAttribute("ExportDataUserSettingsDialog")]

    public class ExportDataUserSettingsDialogUserSettings : FormSettings
    {
        //*************************************************************************
        //  Constructor: ExportDataUserSettingsDialogUserSettings()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ExportDataUserSettingsDialogUserSettings" /> class.
        /// </summary>
        ///
        /// <param name="oForm">
        /// The form to save settings for.
        /// </param>
        //*************************************************************************

        public ExportDataUserSettingsDialogUserSettings
        (
            Form oForm
        )
            : base(oForm, true)
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
