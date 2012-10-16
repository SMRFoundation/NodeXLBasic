
using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMLFilesImporter
//
/// <summary>
/// Imports a set of GraphML files into a set of new NodeXL workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="ImportGraphMLFilesAsync" /> to import the GraphML files.
/// Call <see cref="CancelAsync" /> to stop the import.  Handle the <see
/// cref="ImportationProgressChanged" /> and <see
/// cref="ImportationCompleted" /> events to monitor the progress and
/// completion of the importation.
/// </remarks>
//*****************************************************************************

public class GraphMLFilesImporter : Object
{
    //*************************************************************************
    //  Constructor: GraphMLFilesImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLFilesImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLFilesImporter()
    {
        m_oBackgroundWorker = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: ImportGraphMLFilesAsync()
    //
    /// <summary>
    /// Asynchronously imports a set of GraphML files into a set of new NodeXL
    /// workbooks.
    /// </summary>
    ///
    /// <param name="sourceFolderPath">
    /// Folder containing GraphML files.
    /// </param>
    ///
    /// <param name="destinationFolderPath">
    /// Folder where the new NodeXL workbooks will be saved.
    /// </param>
    ///
    /// <remarks>
    /// When importation completes, the <see cref="ImportationCompleted" />
    /// event fires.
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ImportGraphMLFilesAsync
    (
        String sourceFolderPath,
        String destinationFolderPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sourceFolderPath) );
        Debug.Assert( Directory.Exists(sourceFolderPath) );
        Debug.Assert( !String.IsNullOrEmpty(destinationFolderPath) );
        Debug.Assert( Directory.Exists(destinationFolderPath) );
        AssertValid();

        if (this.IsBusy)
        {
            throw new InvalidOperationException(
                "GraphMLFilesImporter.ImportGraphMLFilesAsync: An asynchronous"
                + " operation is already in progress."
                );
        }

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs =
            new ImportGraphMLFilesAsyncArgs();

        oImportGraphMLFilesAsyncArgs.SourceFolderPath = sourceFolderPath;

        oImportGraphMLFilesAsyncArgs.DestinationFolderPath =
            destinationFolderPath;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerReportsProgress = true;
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oImportGraphMLFilesAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the importation started by <see
    /// cref="ImportGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the importation cancels, the <see cref="ImportationCompleted" />
    /// event fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" />
    /// property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: ImportationProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the importation started by <see
    /// cref="ImportGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// String describing the progress.  The String is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler ImportationProgressChanged;


    //*************************************************************************
    //  Event: ImportationCompleted
    //
    /// <summary>
    /// Occurs when the importation started by <see
    /// cref="ImportGraphMLFilesAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler ImportationCompleted;


    //*************************************************************************
    //  Method: ImportGraphMLFilesInternal()
    //
    /// <summary>
    /// Imports a set of GraphML files into a set of new NodeXL workbooks.
    /// </summary>
    ///
    /// <param name="oImportGraphMLFilesAsyncArgs">
    /// Contains the arguments needed to asynchronously import GraphML files.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    protected void
    ImportGraphMLFilesInternal
    (
        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oImportGraphMLFilesAsyncArgs != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oDoWorkEventArgs != null);
        AssertValid();

        Int32 iGraphMLFiles = 0;

        foreach ( String sGraphMLFilePath in Directory.GetFiles(
            oImportGraphMLFilesAsyncArgs.SourceFolderPath, "*.graphml") )
        {
            if (oBackgroundWorker.CancellationPending)
            {
                oDoWorkEventArgs.Cancel = true;
                return;
            }

            String sGraphMLFileName = Path.GetFileName(sGraphMLFilePath);

            oBackgroundWorker.ReportProgress(0,
                String.Format(
                    "Importing \"{0}\"."
                    ,
                    sGraphMLFileName
                ) );

            XmlDocument oGraphMLDocument = new XmlDocument();

            try
            {
                oGraphMLDocument.Load(sGraphMLFilePath);
            }
            catch (XmlException oXmlException)
            {
                throw new XmlException( String.Format(

                    "The file \"{0}\" does not contain valid XML."
                    ,
                    sGraphMLFilePath
                    ),

                    oXmlException
                    );
            }

            String sNodeXLWorkbookPath = Path.Combine(
                oImportGraphMLFilesAsyncArgs.DestinationFolderPath,
                sGraphMLFileName
                );

            sNodeXLWorkbookPath = Path.ChangeExtension(sNodeXLWorkbookPath,
                ".xlsx");

            try
            {
                GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                    oGraphMLDocument, sGraphMLFilePath, sNodeXLWorkbookPath,
                    null, false);
            }
            catch (XmlException oXmlException)
            {
                // SaveGraphToNodeXLWorkbook() throws an XmlException when
                // invalid GraphML is detected, but it doesn't know the name of
                // the file the GraphML came from.  Refine the exception
                // message.

                throw new XmlException( String.Format(

                    "The file \"{0}\" does not contain valid GraphML."
                    + "  Details:"
                    + "\r\n\r\n"
                    + "{1}"
                    ,
                    sGraphMLFilePath,
                    oXmlException.Message
                    ),

                    oXmlException
                    );
            }

            iGraphMLFiles++;
        }

        oBackgroundWorker.ReportProgress(0,
            String.Format(
                "Done.  GraphML files imported: {0}."
                ,
                iGraphMLFiles
            ) );
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);
        AssertValid();

        Debug.Assert(e.Argument is ImportGraphMLFilesAsyncArgs);

        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs =
            (ImportGraphMLFilesAsyncArgs)e.Argument;

        ImportGraphMLFilesInternal(oImportGraphMLFilesAsyncArgs,
            m_oBackgroundWorker, e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler oImportationProgressChanged =
            this.ImportationProgressChanged;

        if (oImportationProgressChanged != null)
        {
            oImportationProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oImportationCompleted =
            this.ImportationCompleted;

        if (oImportationCompleted != null)
        {
            oImportationCompleted(this, e);
        }

        m_oBackgroundWorker = null;
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
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous importation.  null if an asynchronous importation
    /// is not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: ImportGraphMLFilesAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously import GraphML files.
    /// </summary>
    //*************************************************************************

    protected class ImportGraphMLFilesAsyncArgs
    {
        ///
        public String SourceFolderPath;
        ///
        public String DestinationFolderPath;
    };
}

}
