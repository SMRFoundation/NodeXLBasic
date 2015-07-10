
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
    //*****************************************************************************
    //  Class: ExportDataUserSettings
    //
    /// <summary>
    /// Stores the user's settings for exporting data from the workbook.
    /// </summary>
    //*****************************************************************************

    [SettingsGroupNameAttribute("ExportDataUserSettings")]

    public class ExportDataUserSettings : NodeXLApplicationSettingsBase
    {
        //*************************************************************************
        //  Constructor: IExportDataUserSettings()
        //
        /// <summary>
        /// Initializes a new instance of the ExportDataUserSettings class.
        /// </summary>
        //*************************************************************************

        public ExportDataUserSettings()
        {
            // (Do nothing.)

            AssertValid();
        }

        //*************************************************************************
        //  Property: Hashtag
        //
        /// <summary>
        /// Gets or sets the user's hashtag to be included in the tweet
        /// text in GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's hashtag.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        Hashtag
        {
            get
            {
                AssertValid();

                return ((String)this[HashtagKey]);
            }

            set
            {
                this[HashtagKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: URL
        //
        /// <summary>
        /// Gets or sets the user's URL to be included in the tweet
        /// text in GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's URL.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        URL
        {
            get
            {
                AssertValid();

                return ((String)this[URLKey]);
            }

            set
            {
                this[URLKey] = value;

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
        //  Protected constants
        //*************************************************************************

        /// Name of the settings key for the Hashtag property.

        protected const String HashtagKey =
            "Hashtag";

        /// Name of the settings key for the URL property.

        protected const String URLKey =
            "URL";

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // (None.)
    }

}
