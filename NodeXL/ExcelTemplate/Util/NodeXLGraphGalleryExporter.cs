
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLGraphGalleryExporter
//
/// <summary>
/// Exports a graph to the NodeXL Graph Gallery.
/// </summary>
///
/// <remarks>
/// Call <see cref="ExportToNodeXLGraphGallery" /> to export a graph to the
/// NodeXL Graph Gallery website.
/// </remarks>
//*****************************************************************************

public class NodeXLGraphGalleryExporter : Object
{
    //*************************************************************************
    //  Constructor: NodeXLGraphGalleryExporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLGraphGalleryExporter" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLGraphGalleryExporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportToNodeXLGraphGallery()
    //
    /// <summary>
    /// Exports a graph to the NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  The control's ActualWidth and
    /// ActualHeight properties must be at least MinimumNodeXLControlWidth and
    /// MinimumNodeXLControlHeight, respectively.
    /// </param>
    ///
    /// <param name="title">
    /// The graph's title.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="description">
    /// The graph's description.  Can be null or empty.
    /// </param>
    ///
    /// <param name="spaceDelimitedTags">
    /// The graph's space-delimited tags.  Can be null or empty.
    /// </param>
    ///
    /// <param name="author">
    /// The graph's author.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="password">
    /// The password for <paramref name="author" /> if using credentials, or
    /// null if not.
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
    ///
    /// <exception cref="GraphTooLargeException">
    /// Thrown when the graph is too large to export.
    /// </exception>
    //*************************************************************************

    public void
    ExportToNodeXLGraphGallery
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        String title,
        String description,
        String spaceDelimitedTags,
        String author,
        String password,
        Boolean exportWorkbookAndSettings,
        Boolean exportGraphML,
        Boolean useFixedAspectRatio
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(nodeXLControl.ActualWidth >= MinimumNodeXLControlWidth);
        Debug.Assert(nodeXLControl.ActualHeight >= MinimumNodeXLControlHeight);
        Debug.Assert( !String.IsNullOrEmpty(title) );
        Debug.Assert( !String.IsNullOrEmpty(author) );
        AssertValid();

        Byte [] abtWorkbook = null;
        String sWorkbookSettings = null;

        if (exportWorkbookAndSettings)
        {
            abtWorkbook = ReadWorkbook(workbook);
            sWorkbookSettings = GetWorkbookSettings(workbook);
        }

        String sGraphML = null;

        if (exportGraphML)
        {
            sGraphML = GetGraphML(workbook);
        }

        Size oFullSizeImageSizePx = GetFullSizeImageSizePx(nodeXLControl,
            useFixedAspectRatio);

        Image oFullSizeImage = nodeXLControl.CopyGraphToBitmap(
            oFullSizeImageSizePx.Width, oFullSizeImageSizePx.Height);

        Size oThumbnailImageSizePx = GetThumbnailImageSizePx(
            oFullSizeImageSizePx);

        Image oThumbnail = oFullSizeImage.GetThumbnailImage(
            oThumbnailImageSizePx.Width, oThumbnailImageSizePx.Height,
            () => {return false;}, IntPtr.Zero);

        NodeXLGraphGalleryServiceClient oClient =
            new NodeXLGraphGalleryServiceClient(GetWcfServiceBinding(),
                new EndpointAddress(
                    ProjectInformation.NodeXLGraphGalleryWcfServiceUrl)
                );

        oClient.Endpoint.Binding.SendTimeout =
            new TimeSpan(0, SendTimeoutMinutes, 0);

        try
        {
            oClient.AddGraph3(title, author, password, description,
                spaceDelimitedTags, ImageToBytes(oFullSizeImage),
                ImageToBytes(oThumbnail), abtWorkbook, sWorkbookSettings,
                sGraphML);
        }
        catch (ProtocolException oProtocolException)
        {
            // The following text search detects an exception thrown by the WCF
            // service when the exported byte count exceeds the maximum that
            // can be handled by the WCF service.
            //
            // This isn't a very robust test.  Is there a better way to do
            // it?  An earlier version attempted to calculate the number of
            // bytes that would be exported before attempting to export them,
            // but it didn't seem to be doing so accurately.
            //
            // See the MaximumBytes constant for more details.

            if (oProtocolException.Message.IndexOf("(400) Bad Request") >= 0)
            {
                throw new GraphTooLargeException();
            }

            throw (oProtocolException);
        }
        finally
        {
            oFullSizeImage.Dispose();
            oThumbnail.Dispose();
        }
    }

    //*************************************************************************
    //  Method: GetFullSizeImageSizePx()
    //
    /// <summary>
    /// Gets the size to use for the full-size image.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="bUseFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <returns>
    /// The size to use for the full-size image, in pixels, as a Size.
    /// </returns>
    //*************************************************************************

    protected Size
    GetFullSizeImageSizePx
    (
        NodeXLControl oNodeXLControl,
        Boolean bUseFixedAspectRatio
    )
    {
        Debug.Assert(oNodeXLControl != null);
        AssertValid();

        Debug.Assert(oNodeXLControl.ActualHeight > 0);

        Double dAspectRatio = bUseFixedAspectRatio ? FixedAspectRatio :
            (oNodeXLControl.ActualWidth / oNodeXLControl.ActualHeight);

        Debug.Assert(dAspectRatio > 0);

        return ( new Size(
            FullSizeImageWidthPx,

            (Int32)Math.Round( (Double)FullSizeImageWidthPx / dAspectRatio )
            ) );
    }

    //*************************************************************************
    //  Method: GetThumbnailImageSizePx()
    //
    /// <summary>
    /// Gets the size to use for the thumbnail image.
    /// </summary>
    ///
    /// <param name="oFullSizeImageSizePx">
    /// The size used for the full-size image, in pixels.
    /// </param>
    ///
    /// <returns>
    /// The size to use for the thumbnail image, in pixels, as a Size.
    /// </returns>
    //*************************************************************************

    protected Size
    GetThumbnailImageSizePx
    (
        Size oFullSizeImageSizePx
    )
    {
        AssertValid();

        Debug.Assert(oFullSizeImageSizePx.Height > 0);

        Double dFullSizeImageAspectRatio = (Double)oFullSizeImageSizePx.Width /
            (Double)oFullSizeImageSizePx.Height;

        if (dFullSizeImageAspectRatio >=
            (Double)ThumbnailImageWidthIfWiderPx /
            (Double)ThumbnailImageHeightIfTallerPx)
        {
            Debug.Assert(dFullSizeImageAspectRatio > 0);

            return ( new Size(
                ThumbnailImageWidthIfWiderPx,

                (Int32)Math.Ceiling( (Double)ThumbnailImageWidthIfWiderPx /
                    dFullSizeImageAspectRatio )
                ) );
        }
        else
        {
            return ( new Size(
                (Int32)Math.Ceiling( (Double)ThumbnailImageHeightIfTallerPx *
                    dFullSizeImageAspectRatio),

                ThumbnailImageHeightIfTallerPx
                ) );
        }
    }

    //*************************************************************************
    //  Method: ImageToBytes()
    //
    /// <summary>
    /// Gets an Image's data bytes.
    /// </summary>
    ///
    /// <param name="oImage">
    /// Image to get the data bytes from.
    /// </param>
    ///
    /// <returns>
    /// The image data as an array of bytes, in PNG format.
    /// </returns>
    ///
    /// <remarks>
    /// The Image is not disposed by this method.
    /// </remarks>
    //*************************************************************************

    protected Byte []
    ImageToBytes
    (
        Image oImage
    )
    {
        Debug.Assert(oImage != null);
        AssertValid();

        using ( MemoryStream oMemoryStream = new MemoryStream() )
        {
            oImage.Save(oMemoryStream, ImageFormat.Png);

            return ( oMemoryStream.ToArray() );
        }
    }

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
    /// Reads the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The workbook contents.
    /// </returns>
    //*************************************************************************

    protected Byte []
    ReadWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // Save the workbook to a temporary file, then read the temporary file.

        String sTempFilePath = Path.GetTempFileName();

        try
        {
            oWorkbook.SaveCopyAs(sTempFilePath);

            return ( FileUtil.ReadBinaryFile(sTempFilePath) );
        }
        finally
        {
            if ( File.Exists(sTempFilePath) )
            {
                File.Delete(sTempFilePath);
            }
        }
    }

    //*************************************************************************
    //  Method: GetWorkbookSettings()
    //
    /// <summary>
    /// Gets the workbook settings from a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The workbook settings, as an XML string.
    /// </returns>
    //*************************************************************************

    protected String
    GetWorkbookSettings
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // The workbook settings are stored within the workbook.  Retrieve
        // them.

        return (new PerWorkbookSettings(oWorkbook).WorkbookSettings);
    }

    //*************************************************************************
    //  Method: GetGraphML()
    //
    /// <summary>
    /// Gets the graph as GraphML.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The graph as GraphML.
    /// </returns>
    //*************************************************************************

    protected String
    GetGraphML
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        String sGraphML = null;

        // The graph owned by the NodeXLControl can't be used, because it
        // doesn't include all the edge and vertex column data needed for
        // GraphML.  Instead, read the graph from the workbook and include all
        // the necessary data.

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadAllEdgeAndVertexColumns = true;

        WorkbookReader oWorkbookReader = new WorkbookReader();

        IGraph oGraphForGraphML = oWorkbookReader.ReadWorkbook(
            oWorkbook, oReadWorkbookContext);

        GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();

        using ( MemoryStream oMemoryStream = new MemoryStream() )
        {
            oGraphMLGraphAdapter.SaveGraph(oGraphForGraphML, oMemoryStream);
            oMemoryStream.Position = 0;

            using ( StreamReader oStreamReader =
                new StreamReader(oMemoryStream) )
            {
                sGraphML = oStreamReader.ReadToEnd();
            }
        }

        return (sGraphML);
    }

    //*************************************************************************
    //  Method: GetWcfServiceBinding()
    //
    /// <summary>
    /// Returns the binding to use for communicating with the NodeXL Graph
    /// Gallery's WCF service.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="BasicHttpBinding" /> object.
    /// </returns>
    //*************************************************************************

    protected BasicHttpBinding
    GetWcfServiceBinding()
    {
        // Communicate over SSL.

        BasicHttpBinding oBasicHttpBinding =
            new BasicHttpBinding(BasicHttpSecurityMode.Transport);

        // These settings were determined by running the svcutil.exe utility
        // on the NodeXLGraphGalleryService.svc file and looking at the config
        // file generated by the utility.

        oBasicHttpBinding.Name = "BasicHttpBinding_INodeXLGraphGalleryService";
        oBasicHttpBinding.ReceiveTimeout = new TimeSpan(0, 1, 0);
        oBasicHttpBinding.MaxBufferSize = MaximumBytes;
        oBasicHttpBinding.MaxReceivedMessageSize = MaximumBytes;
        oBasicHttpBinding.TransferMode = TransferMode.Buffered;

        XmlDictionaryReaderQuotas oReaderQuotas =
            new XmlDictionaryReaderQuotas();

        oReaderQuotas.MaxArrayLength = MaximumBytes;
        oBasicHttpBinding.ReaderQuotas = oReaderQuotas;

        return (oBasicHttpBinding);
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
    //  Public constants
    //*************************************************************************

    /// Minimum actual width and height of the NodeXLControl required by this
    /// class, in WPF units.  These were chosen arbitrarily.

    public const Double MinimumNodeXLControlWidth = 100;
    ///
    public const Double MinimumNodeXLControlHeight = 100;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Width of the full-size image, in pixels.  This is constrained by the
    /// width of the page that displays the graph on the NodeXL Graph Gallery
    /// site.

    protected const Int32 FullSizeImageWidthPx = 950;

    /// Aspect ratio to use for the full-size image, if a fixed aspect ratio is
    /// specified.

    protected const Double FixedAspectRatio = 950.0 / 688.0;

    /// Width of the thumbnail image if the full-size image is wider than it is
    /// tall, in pixels.  This is constrained by the width of the page that
    /// displays thumbnails on the NodeXL Graph Gallery site.

    protected const Int32 ThumbnailImageWidthIfWiderPx = 175;

    /// Height of the thumbnail image if the full-size image is taller than it
    /// is wide, in pixels.  This is constrained by the maximum desired table
    /// row height on the page that displays thumbnails on the NodeXL Graph
    /// Gallery site.

    protected const Int32 ThumbnailImageHeightIfTallerPx = 125;

    /// Maximum number of bytes that can be exported to the WCF service.  This
    /// is the value to use for length-related parameters in the WCF binding
    /// object.  If this maximum is exceeded, a
    /// System.ServiceModel.CommunicationException will occur when attempting
    /// to export a graph.
    ///
    /// This must be the same value that is specified many times in the
    /// Web.config file in the NodeXLGraphGaller\WcfService project.  Search
    /// for the following text in the Web.config file to find the value:
    //
    //     maxRequestLength
    //     maxReceivedMessageSize
    //     maxArrayLength
    //     maxStringContentLength

    protected const Int32 MaximumBytes = 50000000;

    /// Number of minutes to wait while sending a graph.  If it takes longer
    /// than this to send the graph, a TimeoutException is thrown on the client
    /// end.

    protected const Int32 SendTimeoutMinutes = 20;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
