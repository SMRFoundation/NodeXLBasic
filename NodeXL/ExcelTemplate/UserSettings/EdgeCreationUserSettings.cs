using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{

    //*****************************************************************************
    //  Class: EdgeCreationUserSettings
    //
    /// <summary>
    /// Stores the user's settings for creating edges based on shared content similarity.
    /// </summary>
    //*****************************************************************************

[TypeConverterAttribute(typeof(EdgeCreationUserSettingsTypeConverter))]

    public class EdgeCreationUserSettings : NodeXLApplicationSettingsBase
    {
        //*************************************************************************
        //  Constructor: EdgeCreationUserSettings()
        //
        /// <summary>
        /// Initializes a new instance of the EdgeCreationUserSettings class.
        /// </summary>
        //*************************************************************************

    public EdgeCreationUserSettings()
        {
            // (Do nothing.)

            AssertValid();
        }

        

        //*************************************************************************
        //  Property: TextColumnName
        //
        /// <summary>
        /// Gets or sets the name of the text column.
        /// </summary>
        ///
        /// <value>
        /// The name of the text column.  The default value is String.Empty.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]

        public String
        TextColumnName
        {
            get
            {
                AssertValid();

                return ((String)this[TextColumnNameKey]);
            }

            set
            {
                this[TextColumnNameKey] = value;

                AssertValid();
            }
        }

        

        //*************************************************************************
        //  Property: Threshold
        //
        /// <summary>
        /// Gets or sets the Threshold for shared content.
        /// </summary>
        ///
        /// <value>
        /// The threshold used to determine whether to create an edge based on the shared content.
        /// The default value is 75(%).
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]

        [DefaultSettingValueAttribute("75")]

        public Int32
        Threshold
        {
            get
            {
                AssertValid();

                return ((Int32)this[ThresholdKey]);
            }

            set
            {
                this[ThresholdKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: EdgeLimit
        //
        /// <summary>
        /// Gets or sets the maximum number of edges created.
        /// </summary>
        ///
        /// <value>
        /// The number used to determine how many edges to be created.
        /// The default and the maximum value is 10,000.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]

        [DefaultSettingValueAttribute("10000")]

        public Int32
        EdgeLimit
        {
            get
            {
                AssertValid();

                return ((Int32)this[EdgeLimitKey]);
            }

            set
            {
                this[EdgeLimitKey] = value;

                AssertValid();
            }
        }

        //*************************************************************************
        //  Property: LimitToIsolate
        //
        /// <summary>
        /// Gets or sets a flag indicating whether only create edges 
        /// for isolated vertex.
        /// </summary>
        ///
        /// <value>
        /// true if only create edges for isolated vertex, 
        /// false if create edges for all vertex.  
        /// The default value is false.
        /// </value>
        //*************************************************************************

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("false")]

        public Boolean
        LimitToIsolate
        {
            get
            {
                AssertValid();

                return ((Boolean)this[LimitToIsolateKey]);
            }

            set
            {
                this[LimitToIsolateKey] = value;

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
        //  Public constants
        //*************************************************************************

        // These are public to allow them to be shared with
        // EdgeCreationUserSettingsTypeConverter.  There is no point in having that
        // class define its own keys.

       

        /// Name of the settings key for the TextColumnName property.

        public const String TextColumnNameKey =
            "TextColumnName";

        /// Name of the settings key for the Threshold property.

        public const String ThresholdKey =
            "Threshold";

        /// Name of the settings key for the EdgeLimit property.

        public const String EdgeLimitKey =
            "EdgeLimit";

        /// Name of the settings key for the LimitToIsolate property.

        public const String LimitToIsolateKey =
            "LimitToIsolate";

       


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // (None.)
    }


//*****************************************************************************
//  Class: EdgeCreationUserSettingsTypeConverter
//
/// <summary>
/// Converts a <see cref="EdgeCreationUserSettings" /> object to and from a
/// String.
/// </summary>
/// 
/// <remarks>
/// The <see cref="GraphMetricUserSettings.EdgeCreationUserSettings" /> property
/// is of type <see cref="EdgeCreationUserSettings" />.  The application settings
/// architecture requires a type converter for such a nested type.
/// </remarks>
//*****************************************************************************

public class EdgeCreationUserSettingsTypeConverter :
    UserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: EdgeCreationUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeCreationUserSettingsTypeConverter" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeCreationUserSettingsTypeConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertTo()
    //
    /// <summary>
    /// Converts the given value object to the specified type, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If null is passed, the current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <param name="destinationType">
    /// The Type to convert the value parameter to. 
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertTo
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value,
        Type destinationType
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is EdgeCreationUserSettings);
        Debug.Assert(destinationType == typeof(String));
        AssertValid();

        // Note:
        //
        // Earlier user settings type converter classes used a string of
        // ordered, tab-delimited values to persist the user settings.  That
        // was a brittle solution.  Newer classes, including this one, use an
        // unordered dictionary.

        EdgeCreationUserSettings oEdgeCreationUserSettings =
            (EdgeCreationUserSettings)value;

        PersistableStringDictionary oDictionary =
            new PersistableStringDictionary();

        
        oDictionary.Add(EdgeCreationUserSettings.TextColumnNameKey,
            oEdgeCreationUserSettings.TextColumnName);

        oDictionary.Add(EdgeCreationUserSettings.ThresholdKey,
            oEdgeCreationUserSettings.Threshold);

        oDictionary.Add(EdgeCreationUserSettings.EdgeLimitKey,
            oEdgeCreationUserSettings.EdgeLimit);

        oDictionary.Add(EdgeCreationUserSettings.LimitToIsolateKey,
            oEdgeCreationUserSettings.LimitToIsolate);

        return (oDictionary.ToString());
    }

    //*************************************************************************
    //  Method: ConvertFrom()
    //
    /// <summary>
    /// Converts the given object to the type of this converter, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If nullNothingnullptra null reference is passed, the
    /// current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertFrom
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is String);
        AssertValid();

        PersistableStringDictionary oDictionary =
            PersistableStringDictionary.FromString((String)value);

        EdgeCreationUserSettings oEdgeCreationUserSettings =
            new EdgeCreationUserSettings();

        Boolean bValue;
        String sValue;
        Int32 iValueThreshold;
        Int32 iValueEdgeLimit;

        if (oDictionary.TryGetValue(
            EdgeCreationUserSettings.TextColumnNameKey, out sValue))
        {
            oEdgeCreationUserSettings.TextColumnName = sValue;
        }

        if (oDictionary.TryGetValue(
            EdgeCreationUserSettings.ThresholdKey, out iValueThreshold))
        {
            oEdgeCreationUserSettings.Threshold = iValueThreshold;
        }

        if (oDictionary.TryGetValue(
            EdgeCreationUserSettings.EdgeLimitKey, out iValueEdgeLimit))
        {
            oEdgeCreationUserSettings.EdgeLimit = iValueEdgeLimit;
        }

        if (oDictionary.TryGetValue(
            EdgeCreationUserSettings.LimitToIsolateKey, out bValue))
        {
            oEdgeCreationUserSettings.LimitToIsolate = bValue;
        }

        return (oEdgeCreationUserSettings);
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
