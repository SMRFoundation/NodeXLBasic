﻿

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: FacebookGraphDataProvider
    //
    /// <summary>
    /// Gets the network of Facebook friends.
    /// </summary>
    ///
    /// <remarks>
    /// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
    /// that describes a network of Facebook freinds.
    /// </remarks>
    //*****************************************************************************

    public class FacebookFanPageGraphDataProvider : GraphDataProviderBase
    {
        //*************************************************************************
        //  Constructor: FacebookGraphDataProvider()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="FacebookGraphDataProvider" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookFanPageGraphDataProvider()
            :
            base(GraphDataProviderName,
                "Test Facebook")
        {
            // (Do nothing.)

            AssertValid();
        }

        public FacebookFanPageDialog a
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        //*************************************************************************
        //  Method: CreateDialog()
        //
        /// <summary>
        /// Creates a dialog for getting graph data.
        /// </summary>
        ///
        /// <returns>
        /// A dialog derived from GraphDataProviderDialogBase.
        /// </returns>
        //*************************************************************************

        protected override GraphDataProviderDialogBase
        CreateDialog()
        {
            AssertValid();

            return (new FacebookFanPageDialog());
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

        /// Value of the Name property.

        public const String GraphDataProviderName =
            "Facebook Fan Page Network";


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // (None.)
    }

}