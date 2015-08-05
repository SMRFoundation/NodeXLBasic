
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
        //  Property: BrandLogo
        //
        /// <summary>
        /// Gets or sets the user's BrandLogo file path to be included
        /// in GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's brand logo file path.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        BrandLogo
        {
            get
            {
                AssertValid();

                return ((String)this[BrandLogoKey]);
            }

            set
            {
                this[BrandLogoKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: BrandURL
        //
        /// <summary>
        /// Gets or sets the user's Brand URL to be included in 
        /// GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's brand URL.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        BrandURL
        {
            get
            {
                AssertValid();

                return ((String)this[BrandURLKey]);
            }

            set
            {
                this[BrandURLKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: ActionLabel
        //
        /// <summary>
        /// Gets or sets the user's action label be included in 
        /// every section in GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's action label.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        ActionLabel
        {
            get
            {
                AssertValid();

                return ((String)this[ActionLabelKey]);
            }

            set
            {
                this[ActionLabelKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: ActionURL
        //
        /// <summary>
        /// Gets or sets the user's action URL to be included in 
        /// every section in GraphDescriptionDocument.
        /// </summary>
        ///
        /// <value>
        /// The user's action URL.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        ActionURL
        {
            get
            {
                AssertValid();

                return ((String)this[ActionURLKey]);
            }

            set
            {
                this[ActionURLKey] = value;

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

        /// Name of the settings key for the BrandLogo property.

        protected const String BrandLogoKey =
            "BrandLogo";

        /// Name of the settings key for the BrandURL property.

        protected const String BrandURLKey =
            "BrandURL";

        /// Name of the settings key for the ActionLabel property.

        protected const String ActionLabelKey =
            "ActionLabel";

        /// Name of the settings key for the ActionURL property.

        protected const String ActionURLKey =
            "ActionURL";

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // (None.)
    }

}
