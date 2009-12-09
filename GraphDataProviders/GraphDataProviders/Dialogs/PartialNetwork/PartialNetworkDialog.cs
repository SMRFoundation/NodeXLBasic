﻿

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: PartialNetworkDialog
//
/// <summary>
/// Asks the user whether he wants to import the partial network that was
/// obtained.
/// </summary>
///
/// <remarks>
/// When a <see cref="PartialNetworkException" /> is thrown, show this dialog.
/// The dialog asks the user whether he wants to import the partial network
/// that was obtained.  If he does, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Yes.  Otherwise, it returns DialogResult.No.
/// </remarks>
//*****************************************************************************

public partial class PartialNetworkDialog : FormPlus
{
    //*************************************************************************
    //  Constructor: PartialNetworkDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="PartialNetworkDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PartialNetworkDialog" /> class with a <see
    /// cref="PartialNetworkException" />.
    /// </summary>
    ///
    /// <param name="partialNetworkException">
    /// The <see cref="PartialNetworkException" /> that was thrown.
    /// </param>
    ///
    /// <param name="lastUnexpectedExceptionMessage">
    /// The most recent unexpected exception (after retries) that occurred
    /// while getting the network, converted to a message.
    /// </param>
    //*************************************************************************

    public PartialNetworkDialog
    (
        PartialNetworkException partialNetworkException,
        String lastUnexpectedExceptionMessage
    )
    : this()
    {
        m_oPartialNetworkException = partialNetworkException;
        m_sLastUnexpectedExceptionMessage = lastUnexpectedExceptionMessage;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: PartialNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PartialNetworkDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public PartialNetworkDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Method: lnkDetails_Click()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkDetails LinkLabel.
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
    lnkDetails_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        RequestStatistics oRequestStatistics =
            m_oPartialNetworkException.RequestStatistics;

        Int32 iUnexpectedExceptions = oRequestStatistics.UnexpectedExceptions;

        const String Int32Format = "N0";

        this.ShowInformation( String.Format(

            "Getting a network can involve many information requests to a Web"
            + " service.  In this case, {0} requests were made and {1} of them"
            + " {2} unsuccessful."
            + "\r\n\r\n"
            + "(Note that unsuccessful requests might have led to additional"
            + " requests if they had succeeded, so it is not possible to"
            + " calculate the percentage of the complete network that was"
            + " actually obtained.)"
            + "\r\n\r\n"
            + "Here are details for the most recent unsuccessful request:"
            + "\r\n\r\n"
            + "{3}"
            ,
            (oRequestStatistics.SuccessfulRequests +
                iUnexpectedExceptions).ToString(Int32Format),

            iUnexpectedExceptions.ToString(Int32Format),
            (iUnexpectedExceptions == 1) ? "was" : "were",
            m_sLastUnexpectedExceptionMessage
            ) );
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

        Debug.Assert(m_oPartialNetworkException != null);

        Debug.Assert( !String.IsNullOrEmpty(
            m_sLastUnexpectedExceptionMessage) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The PartialNetworkException that was thrown.

    protected PartialNetworkException m_oPartialNetworkException;

    /// The most recent unexpected exception (after retries) that occurred
    /// while getting the network, converted to a message.

    protected String m_sLastUnexpectedExceptionMessage;
}
}
