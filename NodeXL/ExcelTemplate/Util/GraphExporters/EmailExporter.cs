
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Text;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EmailExporter
//
/// <summary>
/// Exports a graph to one or more email recipients.
/// </summary>
///
/// <remarks>
/// Call <see cref="ExportToEmail" /> to export a graph to one or more email
/// recipients.
/// </remarks>
//*****************************************************************************

public class EmailExporter : Object
{
    //*************************************************************************
    //  Constructor: EmailExporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailExporter" /> class.
    /// </summary>
    //*************************************************************************

    public EmailExporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportToEmail()
    //
    /// <summary>
    /// Exports a graph to one or more email recipients.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  The control's ActualWidth and
    /// ActualHeight properties must be at least <see
    /// cref="GraphExporterUtil.MinimumNodeXLControlWidth" /> and <see 
    /// cref="GraphExporterUtil.MinimumNodeXLControlHeight" />, respectively.
    /// </param>
    ///
    /// <param name="toAddresses">
    /// Array of one or more email addresses to export the graph to.
    /// </param>
    ///
    /// <param name="fromAddress">
    /// "From" email address.
    /// </param>
    ///
    /// <param name="subject">
    /// Email subject line.
    /// </param>
    ///
    /// <param name="messageBody">
    /// Email message body.  Can be empty or null.
    /// </param>
    ///
    /// <param name="smtpHost">
    /// Name of the SMTP server to use.
    /// </param>
    ///
    /// <param name="smtpPort">
    /// SMTP port to use.
    /// </param>
    ///
    /// <param name="useSslForSmtp">
    /// true to use SSL.
    /// </param>
    ///
    /// <param name="smtpUserName">
    /// The user name for the SMTP account.
    /// </param>
    ///
    /// <param name="smtpPassword">
    /// The password for the SMTP account.
    /// </param>
    ///
    /// <param name="exportWorkbookAndSettings">
    /// true to export the workbook and its settings.
    /// </param>
    ///
    /// <param name="exportGraphML">
    /// true to export the graph's data as GraphML.
    /// </param>
    ///
    /// <param name="useFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    //*************************************************************************

    public void
    ExportToEmail
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        String [] toAddresses,
        String fromAddress,
        String subject,
        String messageBody,
        String smtpHost,
        Int32 smtpPort,
        Boolean useSslForSmtp,
        String smtpUserName,
        String smtpPassword,
        Boolean exportWorkbookAndSettings,
        Boolean exportGraphML,
        Boolean useFixedAspectRatio
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);

        Debug.Assert(nodeXLControl.ActualWidth >=
            GraphExporterUtil.MinimumNodeXLControlWidth);

        Debug.Assert(nodeXLControl.ActualHeight >=
            GraphExporterUtil.MinimumNodeXLControlHeight);

        Debug.Assert(toAddresses != null);
        Debug.Assert(toAddresses.Length > 0);
        Debug.Assert( !String.IsNullOrEmpty(fromAddress) );
        Debug.Assert( !String.IsNullOrEmpty(subject) );
        Debug.Assert( !String.IsNullOrEmpty(smtpHost) );
        Debug.Assert(smtpPort > 0);
        Debug.Assert( !String.IsNullOrEmpty(smtpUserName) );
        Debug.Assert( !String.IsNullOrEmpty(smtpPassword) );
        AssertValid();

        SmtpClient oSmtpClient = GetSmtpClient(smtpHost, smtpPort,
            useSslForSmtp, smtpUserName, smtpPassword);

        MailMessage oMailMessage = GetMailMessage(workbook, nodeXLControl,
            toAddresses, fromAddress, subject, messageBody,
            exportWorkbookAndSettings, exportGraphML, useFixedAspectRatio);

        oSmtpClient.Send(oMailMessage);
    }

    //*************************************************************************
    //  Method: GetSmtpClient()
    //
    /// <summary>
    /// Gets an SmtpClient object to use to send the email.
    /// </summary>
    ///
    /// <param name="sSmtpHost">
    /// Name of the SMTP server to use.
    /// </param>
    ///
    /// <param name="iSmtpPort">
    /// SMTP port to use.
    /// </param>
    ///
    /// <param name="bUseSslForSmtp">
    /// true to use SSL.
    /// </param>
    ///
    /// <param name="sSmtpUserName">
    /// The user name for the SMTP account.
    /// </param>
    ///
    /// <param name="sSmtpPassword">
    /// The password for the SMTP account.
    /// </param>
    ///
    /// <returns>
    /// A new SmtpClient object.
    /// </returns>
    //*************************************************************************

    protected SmtpClient
    GetSmtpClient
    (
        String sSmtpHost,
        Int32 iSmtpPort,
        Boolean bUseSslForSmtp,
        String sSmtpUserName,
        String sSmtpPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSmtpHost) );
        Debug.Assert(iSmtpPort > 0);
        Debug.Assert( !String.IsNullOrEmpty(sSmtpUserName) );
        Debug.Assert( !String.IsNullOrEmpty(sSmtpPassword) );
        AssertValid();

        SmtpClient oSmtpClient = new SmtpClient();

        oSmtpClient.Host = sSmtpHost;
        oSmtpClient.Port = iSmtpPort;
        oSmtpClient.EnableSsl = bUseSslForSmtp;
        oSmtpClient.Timeout = SendTimeoutMs;

        oSmtpClient.Credentials = new NetworkCredential(sSmtpUserName,
            sSmtpPassword);

        return (oSmtpClient);
    }

    //*************************************************************************
    //  Method: GetMailMessage()
    //
    /// <summary>
    /// Gets a MailMessage object to send.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="asToAddresses">
    /// Array of one or more email addresses to export the graph to.
    /// </param>
    ///
    /// <param name="sFromAddress">
    /// "From" email address.
    /// </param>
    ///
    /// <param name="sSubject">
    /// Email subject line.
    /// </param>
    ///
    /// <param name="sMessageBody">
    /// Email message body.  Can be empty or null.
    /// </param>
    ///
    /// <param name="bExportWorkbookAndSettings">
    /// true to export the workbook and its settings.
    /// </param>
    ///
    /// <param name="bExportGraphML">
    /// true to export the graph's data as GraphML.
    /// </param>
    ///
    /// <param name="bUseFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <returns>
    /// A new MailMessage object.
    /// </returns>
    //*************************************************************************

    protected MailMessage
    GetMailMessage
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl,
        String [] asToAddresses,
        String sFromAddress,
        String sSubject,
        String sMessageBody,
        Boolean bExportWorkbookAndSettings,
        Boolean bExportGraphML,
        Boolean bUseFixedAspectRatio
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);

        Debug.Assert(oNodeXLControl.ActualWidth >=
            GraphExporterUtil.MinimumNodeXLControlWidth);

        Debug.Assert(oNodeXLControl.ActualHeight >=
            GraphExporterUtil.MinimumNodeXLControlHeight);

        Debug.Assert(asToAddresses != null);
        Debug.Assert(asToAddresses.Length > 0);
        Debug.Assert( !String.IsNullOrEmpty(sFromAddress) );
        Debug.Assert( !String.IsNullOrEmpty(sSubject) );
        AssertValid();

        MailMessage oMailMessage = new MailMessage();

        oMailMessage.Subject = sSubject;
        oMailMessage.Body = sMessageBody;
        SetAddresses(oMailMessage, asToAddresses, sFromAddress);

        AddAttachmentsAndAlternateHtml(oMailMessage, oWorkbook, oNodeXLControl,
            bExportWorkbookAndSettings, bExportGraphML, bUseFixedAspectRatio);

        return (oMailMessage);
    }

    //*************************************************************************
    //  Method: SetAddresses()
    //
    /// <summary>
    /// Sets the addresses on a MailMessage object.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to set the addresses on.
    /// </param>
    ///
    /// <param name="asToAddresses">
    /// Array of one or more email addresses to export the graph to.
    /// </param>
    ///
    /// <param name="sFromAddress">
    /// "From" email address.
    /// </param>
    //*************************************************************************

    protected void
    SetAddresses
    (
        MailMessage oMailMessage,
        String [] asToAddresses,
        String sFromAddress
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert(asToAddresses != null);
        Debug.Assert(asToAddresses.Length > 0);
        Debug.Assert( !String.IsNullOrEmpty(sFromAddress) );
        AssertValid();

        try
        {
            foreach (String sToAddress in asToAddresses)
            {
                oMailMessage.To.Add(sToAddress);
            }
        }
        catch (FormatException)
        {
            throw new EmailAddressFormatException(EmailAddressType.To);
        }

        try
        {
            oMailMessage.From = new MailAddress(sFromAddress);
        }
        catch (FormatException)
        {
            throw new EmailAddressFormatException(EmailAddressType.From);
        }
    }

    //*************************************************************************
    //  Method: AddAttachmentsAndAlternateHtml()
    //
    /// <summary>
    /// Adds attachments and an alternate HTML view to a MailMessage.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to add attachments and HTML to.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="bExportWorkbookAndSettings">
    /// true to export the workbook and its settings.
    /// </param>
    ///
    /// <param name="bExportGraphML">
    /// true to export the graph's data as GraphML.
    /// </param>
    ///
    /// <param name="bUseFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    //*************************************************************************

    protected void
    AddAttachmentsAndAlternateHtml
    (
        MailMessage oMailMessage,
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl,
        Boolean bExportWorkbookAndSettings,
        Boolean bExportGraphML,
        Boolean bUseFixedAspectRatio
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);

        Debug.Assert(oNodeXLControl.ActualWidth >=
            GraphExporterUtil.MinimumNodeXLControlWidth);

        Debug.Assert(oNodeXLControl.ActualHeight >=
            GraphExporterUtil.MinimumNodeXLControlHeight);

        AssertValid();

        String sSuggestedFileNameNoExtension =
            GetSuggestedFileNameNoExtension(oWorkbook);

        String sGraphMLFileNameNoExtension =
            sSuggestedFileNameNoExtension + "-GraphML";

        Byte [] abtFullSizeImage, abtThumbnail, abtWorkbookContents,
            abtGraphMLZipped;

        String sWorkbookSettings;

        GraphExporterUtil.GetDataToExport(oWorkbook, oNodeXLControl,
            bExportWorkbookAndSettings, bExportGraphML,
            Path.ChangeExtension(sGraphMLFileNameNoExtension, "xml"),
            bUseFixedAspectRatio, out abtFullSizeImage, out abtThumbnail,
            out abtWorkbookContents, out sWorkbookSettings,
            out abtGraphMLZipped);

        AddAttachments(oMailMessage, sSuggestedFileNameNoExtension,
            sGraphMLFileNameNoExtension, abtFullSizeImage, abtWorkbookContents,
            sWorkbookSettings, abtGraphMLZipped);

        AddAlternateHtml(oMailMessage, abtFullSizeImage);
    }

    //*************************************************************************
    //  Method: AddAttachments()
    //
    /// <summary>
    /// Adds attachments to a MailMessage.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to add attachments to.
    /// </param>
    ///
    /// <param name="sSuggestedFileNameNoExtension">
    /// The suggested file name for each attachment, without an extension.
    /// </param>
    ///
    /// <param name="sGraphMLFileNameNoExtension">
    /// The file name for the GraphML, without an extension.
    /// </param>
    ///
    /// <param name="abtFullSizeImage">
    /// The full-size PNG image.
    /// </param>
    ///
    /// <param name="abtWorkbookContents">
    /// The workbook contents.  Can be null.
    /// </param>
    ///
    /// <param name="sWorkbookSettings">
    /// The workbook settings.  Can be null or empty.
    /// </param>
    ///
    /// <param name="abtGraphMLZipped">
    /// The GraphML.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    AddAttachments
    (
        MailMessage oMailMessage,
        String sSuggestedFileNameNoExtension,
        String sGraphMLFileNameNoExtension,
        Byte [] abtFullSizeImage,
        Byte [] abtWorkbookContents,
        String sWorkbookSettings,
        Byte [] abtGraphMLZipped
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert( !String.IsNullOrEmpty(sSuggestedFileNameNoExtension) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLFileNameNoExtension) );
        AssertValid();

        AddAttachment(oMailMessage, abtFullSizeImage, PngContentType,
            sSuggestedFileNameNoExtension, "png");

        if (abtWorkbookContents != null)
        {
            AddAttachment(oMailMessage, abtWorkbookContents,
                "application/vnd.ms-excel", sSuggestedFileNameNoExtension,
                "xlsx");
        }

        if ( !String.IsNullOrEmpty(sWorkbookSettings) )
        {
            AddAttachment(oMailMessage, sWorkbookSettings, XmlContentType,
                sSuggestedFileNameNoExtension, "NodeXLOptions");
        }

        if (abtGraphMLZipped != null)
        {
            AddAttachment(oMailMessage, abtGraphMLZipped, "application/zip",
                sGraphMLFileNameNoExtension, "zip");
        }
    }

    //*************************************************************************
    //  Method: AddAlternateHtml()
    //
    /// <summary>
    /// Adds an alternate HTML view to a MailMessage.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to add alternate HTML to.  If the MessageBody property is
    /// set, this is used as the HTML's text.
    /// </param>
    ///
    /// <param name="abtFullSizeImage">
    /// The full-size PNG image.
    /// </param>
    //*************************************************************************

    protected void
    AddAlternateHtml
    (
        MailMessage oMailMessage,
        Byte [] abtFullSizeImage
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert(abtFullSizeImage != null);
        AssertValid();

        // Always include the full-size image.

        String sHtml =
            "<html>"
            +     "<head>"
            +     "</head>"
            +     "<body>"
            +         "<div style=\"margin-bottom:1em;\">"
            +             "<img src=cid:fullSizeImage"
            +                 " style=\"border:1px solid #000000;\" />"
            +         "</div>"
            ; 

        // Add the plain-text message body if there is one.

        String sMessageBody = oMailMessage.Body;

        if ( !String.IsNullOrEmpty(sMessageBody) )
        {
            sHtml +=
                      "<div>"
                +          sMessageBody.Replace("\r\n", "<br />")
                +     "</div>"
                ;
        }

        sHtml +=
                  "</body>"
            + "</html>"
            ;

        LinkedResource oFullSizeImageResource = new LinkedResource(
            new MemoryStream(abtFullSizeImage), PngContentType);

        oFullSizeImageResource.ContentId = "fullSizeImage";

        AlternateView oHtmlView = AlternateView.CreateAlternateViewFromString(
            sHtml, null, "text/html");

        oHtmlView.LinkedResources.Add(oFullSizeImageResource);

        oMailMessage.AlternateViews.Add(oHtmlView);
    }

    //*************************************************************************
    //  Method: AddAttachment()
    //
    /// <summary>
    /// Adds a string attachment to a MailMessage.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to add attachments to.
    /// </param>
    ///
    /// <param name="sAttachmentContents">
    /// The attachment contents, as a String.
    /// </param>
    ///
    /// <param name="sContentType">
    /// The content type.
    /// </param>
    ///
    /// <param name="sSuggestedFileNameNoExtension">
    /// The suggested file name, without an extension.
    /// </param>
    ///
    /// <param name="sSuggestedExtension">
    /// The suggested extension.
    /// </param>
    //*************************************************************************

    protected void
    AddAttachment
    (
        MailMessage oMailMessage,
        String sAttachmentContents,
        String sContentType,
        String sSuggestedFileNameNoExtension,
        String sSuggestedExtension
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttachmentContents) );
        Debug.Assert( !String.IsNullOrEmpty(sContentType) );
        Debug.Assert( !String.IsNullOrEmpty(sSuggestedFileNameNoExtension) );
        Debug.Assert( !String.IsNullOrEmpty(sSuggestedExtension) );
        AssertValid();

        AddAttachment(oMailMessage,
            Encoding.UTF8.GetBytes(sAttachmentContents), sContentType,
            sSuggestedFileNameNoExtension, sSuggestedExtension);
    }

    //*************************************************************************
    //  Method: AddAttachment()
    //
    /// <summary>
    /// Adds a binary attachment to a MailMessage.
    /// </summary>
    ///
    /// <param name="oMailMessage">
    /// The message to add attachments to.
    /// </param>
    ///
    /// <param name="abtAttachmentContents">
    /// The attachment contents, as an array of bytes.
    /// </param>
    ///
    /// <param name="sContentType">
    /// The content type.
    /// </param>
    ///
    /// <param name="sSuggestedFileNameNoExtension">
    /// The suggested file name, without an extension.
    /// </param>
    ///
    /// <param name="sSuggestedExtension">
    /// The suggested extension.
    /// </param>
    //*************************************************************************

    protected void
    AddAttachment
    (
        MailMessage oMailMessage,
        Byte [] abtAttachmentContents,
        String sContentType,
        String sSuggestedFileNameNoExtension,
        String sSuggestedExtension
    )
    {
        Debug.Assert(oMailMessage != null);
        Debug.Assert(abtAttachmentContents != null);
        Debug.Assert( !String.IsNullOrEmpty(sContentType) );
        Debug.Assert( !String.IsNullOrEmpty(sSuggestedFileNameNoExtension) );
        Debug.Assert( !String.IsNullOrEmpty(sSuggestedExtension) );
        AssertValid();

        Attachment oAttachment = new Attachment(
            new MemoryStream(abtAttachmentContents),
            new ContentType(sContentType)
            );

        oAttachment.ContentDisposition.FileName = Path.ChangeExtension(
            sSuggestedFileNameNoExtension, sSuggestedExtension);

        oMailMessage.Attachments.Add(oAttachment);
    }

    //*************************************************************************
    //  Method: GetSuggestedFileNameNoExtension()
    //
    /// <summary>
    /// Gets a suggested file name for each of the message's attachments.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The suggested file name, with no extension.
    /// </returns>
    //*************************************************************************

    protected String
    GetSuggestedFileNameNoExtension
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // If the workbook has been saved, used the file name.

        if ( !String.IsNullOrEmpty(oWorkbook.Path) )
        {
            return ( Path.GetFileNameWithoutExtension(oWorkbook.Name) );
        }

        return ("NodeXLGraph");
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Send timeout, in milliseconds.

    protected const Int32 SendTimeoutMs = 5 * 60 * 1000;

    /// XML content type.

    protected const String XmlContentType = "text/xml";

    /// PNG content type.

    protected const String PngContentType = "image/png";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
