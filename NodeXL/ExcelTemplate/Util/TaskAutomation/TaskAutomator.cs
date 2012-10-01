
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;
using Smrf.DateTimeLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TaskAutomator
//
/// <summary>
/// Runs multiple tasks on one workbook or a folder full of workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="AutomateOneWorkbook" /> or <see
/// cref="AutomateOneWorkbookIndirect" /> to run a specified set of tasks on
/// one NodeXL workbook, or <see cref="AutomateFolder" /> to run them on every
/// unopened NodeXL workbook in a folder.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class TaskAutomator : Object
{
    //*************************************************************************
    //  Method: AutomateOneWorkbook()
    //
    /// <summary>
    /// Immediately runs a specified set of tasks on one NodeXL workbook, given
    /// a <see cref="ThisWorkbook" /> object.
    /// </summary>
    ///
    /// <param name="thisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="tasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="folderToSaveWorkbookTo">
    /// If the <see cref="AutomationTasks.SaveWorkbookIfNeverSaved" /> flag is
    /// specified and the workbook has never been saved, the workbook is saved
    /// to a new file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    ///
    /// <param name="ribbon">
    /// The workbook's Ribbon.
    /// </param>
    ///
    /// <remarks>
    /// If a <see cref="ThisWorkbook" /> object isn't available, use <see
    /// cref="AutomateOneWorkbookIndirect" /> instead.
    /// </remarks>
    ///
    /// <exception cref="System.ArgumentException">
    /// Occurs when <paramref name="tasksToRun" /> includes <see
    /// cref="AutomationTasks.SaveGraphImageFile" /> without also including
    /// both <see cref="AutomationTasks.ReadWorkbook" /> and <see
    /// cref="AutomationTasks.SaveWorkbookIfNeverSaved" />.
    /// </exception>
    //*************************************************************************

    public static void
    AutomateOneWorkbook
    (
        ThisWorkbook thisWorkbook,
        AutomationTasks tasksToRun,
        String folderToSaveWorkbookTo,
        Ribbon ribbon
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(ribbon != null);

        CheckTasksToRunArgument(tasksToRun);

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            thisWorkbook.InnerObject;

        if
        (
            (
                ShouldRunTask(tasksToRun, AutomationTasks.MergeDuplicateEdges)
                &&
                !TryMergeDuplicateEdges(thisWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.CalculateClusters)
                &&
                !TryCalculateClusters(thisWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun,
                    AutomationTasks.CalculateGraphMetrics)
                &&
                !TryCalculateGraphMetrics(oWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.AutoFillWorkbook)
                &&
                !TryAutoFillWorkbook(oWorkbook, ribbon)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.CreateSubgraphImages)
                &&
                !TryCreateSubgraphImages(thisWorkbook)
            )
        )
        {
            return;
        }

        RunReadWorkbookTasks(thisWorkbook, tasksToRun, folderToSaveWorkbookTo,
            ribbon);
    }

    //*************************************************************************
    //  Method: AutomateFolder()
    //
    /// <summary>
    /// Runs a specified set of tasks on every unopened NodeXL workbook in a
    /// folder.
    /// </summary>
    ///
    /// <param name="folderToAutomate">
    /// Path to the folder to automate.
    /// </param>
    ///
    /// <param name="workbookSettings">
    /// The workbook settings to use for each NodeXL workbook.  These should
    /// come from <see cref="PerWorkbookSettings.WorkbookSettings" />.
    /// </param>
    ///
    /// <remarks>
    /// The workbook settings specified by <paramref name="workbookSettings" />
    /// get stored in each NodeXL workbook.
    /// </remarks>
    //*************************************************************************

    public static void
    AutomateFolder
    (
        String folderToAutomate,
        String workbookSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(folderToAutomate) );
        Debug.Assert( !String.IsNullOrEmpty(workbookSettings) );

        foreach ( String sFileName in Directory.GetFiles(folderToAutomate,
            "*.xlsx") )
        {
            String sFilePath = Path.Combine(folderToAutomate, sFileName);

            try
            {
                if (!NodeXLWorkbookUtil.FileIsNodeXLWorkbook(sFilePath))
                {
                    continue;
                }
            }
            catch (IOException)
            {
                // Skip any workbooks that are already open, or that have any
                // other problems that prevent them from being opened.

                continue;
            }

            AutomateOneWorkbookIndirect(sFilePath, workbookSettings);
        }
    }

    //*************************************************************************
    //  Method: AutomateOneWorkbookIndirect()
    //
    /// <summary>
    /// Indirectly runs a specified set of tasks on one NodeXL workbook, given
    /// a path to the workbook.
    /// </summary>
    ///
    /// <param name="nodeXLWorkbookFilePath">
    /// Path to the NodeXL workbook to automate.
    /// </param>
    ///
    /// <param name="workbookSettings">
    /// The workbook settings to use for each NodeXL workbook.  These should
    /// come from <see cref="PerWorkbookSettings.WorkbookSettings" />.
    /// </param>
    ///
    /// <remarks>
    /// This method stores an "automate tasks on open" flag in the workbook,
    /// indicating that task automation should be run on it the next time it's
    /// opened, then opens it.
    ///
    /// <para>
    /// The specified workbook settings also get stored in the workbook.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutomateOneWorkbookIndirect
    (
        String nodeXLWorkbookFilePath,
        String workbookSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(nodeXLWorkbookFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(workbookSettings) );

        // Ideally, the Excel API would be used here to open the workbook
        // and run the AutomateOneWorkbook() method on it.  Two things
        // make that impossible:
        //
        //   1. When you open a workbook using
        //      Application.Workbooks.Open(), you get only a native Excel
        //      workbook, not an "extended" ThisWorkbook object or its
        //      associated Ribbon object.  AutomateOneWorkbook() requires
        //      a Ribbon object.
        //
        //      Although a GetVstoObject() extension method is available to
        //      convert a native Excel workbook to an extended workbook,
        //      that method doesn't work on a native workbook opened via
        //      the Excel API -- it always returns null.
        //
        //      It might be possible to refactor AutomateOneWorkbook() to
        //      require only a native workbook.  However, problem 2 would
        //      still make things impossible...
        //
        //   2. If this method is being run from a modal dialog, which it
        //      is (see AutomateTasksDialog), then code in the workbook
        //      that needs to be automated doesn't run until the modal
        //      dialog closes.
        //      
        // The following code works around these problems.

        Microsoft.Office.Interop.Excel.Application oExcelApplication =
            null;

        ExcelApplicationKiller oExcelApplicationKiller = null;

        try
        {
            // Use a new Application object for each workbook.  If the same
            // Application object is reused, the memory used by each
            // workbook is never released and the machine will eventually
            // run out of memory.

            oExcelApplication =
                new Microsoft.Office.Interop.Excel.Application();

            if (oExcelApplication == null)
            {
                throw new Exception("Excel couldn't be opened.");
            }

            // ExcelApplicationKiller requires that the application be
            // visible.

            oExcelApplication.Visible = true;

            oExcelApplicationKiller = new ExcelApplicationKiller(
                oExcelApplication);

            // Store an "automate tasks on open" flag in the workbook,
            // indicating that task automation should be run on it the next
            // time it's opened.  This can be done via the Excel API.

            Microsoft.Office.Interop.Excel.Workbook oWorkbookToAutomate =
                ExcelUtil.OpenWorkbook(nodeXLWorkbookFilePath,
                oExcelApplication);

            PerWorkbookSettings oPerWorkbookSettings =
                new PerWorkbookSettings(oWorkbookToAutomate);

            oPerWorkbookSettings.WorkbookSettings = workbookSettings;
            oPerWorkbookSettings.AutomateTasksOnOpen = true;
            oWorkbookToAutomate.Save();
            oWorkbookToAutomate.Close(false, Missing.Value, Missing.Value);
            oExcelApplication.Quit();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }
        finally
        {
            // Quitting the Excel application does not remove it from
            // memory.  Kill its process.

            oExcelApplicationKiller.KillExcelApplication();
            oExcelApplication = null;
            oExcelApplicationKiller = null;
        }

        try
        {
            // Now open the workbook in another instance of Excel, which
            // bypasses problem 2.  Code in the workbook's Ribbon will
            // detect the flag's presence, run task automation on it, close
            // the workbook, and close the other instance of Excel.

            OpenWorkbookToAutomate(nodeXLWorkbookFilePath);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }
    }

    //*************************************************************************
    //  Method: OpenWorkbookToAutomate()
    //
    /// <summary>
    /// Opens a workbook that has its AutomateTasksOnOpen flag set to true,
    /// then waits for the workbook to close.
    /// </summary>
    ///
    /// <param name="workbookPath">
    /// Path to the workbook to open.
    /// </param>
    //*************************************************************************

    public static void
    OpenWorkbookToAutomate
    (
        String workbookPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(workbookPath) );

        Process oProcess = Process.Start("Excel.exe",
            "\"" + workbookPath + "\"");

        oProcess.WaitForExit();
        oProcess.Close();
    }

    //*************************************************************************
    //  Method: CheckTasksToRunArgument
    //
    /// <summary>
    /// Checks a "tasks to run" argument for validity.
    /// </summary>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to check.
    /// </param>
    ///
    /// <remarks>
    /// An ArgumentException is thrown if the tasks are invalid.
    /// </remarks>
    //*************************************************************************

    private static void
    CheckTasksToRunArgument
    (
        AutomationTasks eTasksToRun
    )
    {
        if ( ShouldRunTask(eTasksToRun, AutomationTasks.SaveGraphImageFile) )
        {
            if
            (
                !ShouldRunTask(eTasksToRun,
                    AutomationTasks.ReadWorkbook)
                ||
                !ShouldRunTask(eTasksToRun,
                    AutomationTasks.SaveWorkbookIfNeverSaved)
            )
            {
                throw new ArgumentException("Invalid AutomationTasks flags.");
            }
        }
    }

    //*************************************************************************
    //  Method: ShouldRunTask
    //
    /// <summary>
    /// Gets a flag specifying whether a task should be run.
    /// </summary>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="eTaskToTest">
    /// The task to be tested.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="eTaskToTest" /> should be run.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ShouldRunTask
    (
        AutomationTasks eTasksToRun,
        AutomationTasks eTaskToTest
    )
    {
        return ( (eTasksToRun & eTaskToTest) != 0 );
    }

    //*************************************************************************
    //  Method: TryMergeDuplicateEdges()
    //
    /// <summary>
    /// Attempts to merge duplicate edges.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryMergeDuplicateEdges
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        return ( RunEditableCommand(
            new RunMergeDuplicateEdgesCommandEventArgs(false),
            oThisWorkbook) );
    }

    //*************************************************************************
    //  Method: TryCalculateClusters()
    //
    /// <summary>
    /// Attempts to calculate clusters.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCalculateClusters
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        return ( RunEditableCommand(
            new RunGroupByClusterCommandEventArgs(false), oThisWorkbook) );
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate graph metrics.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCalculateGraphMetrics
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        // In this case, clicking the corresponding Ribbon button opens a
        // GraphMetricsDialog, which allows the user to edit the graph metric
        // settings before calculating the graph metrics.  The actual
        // calculations are done by CalculateGraphMetricsDialog, so just use
        // that dialog directly.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(null, oWorkbook);

        if (oCalculateGraphMetricsDialog.ShowDialog() != DialogResult.OK)
        {
            return (false);
        }

        if ( ( new GraphMetricUserSettings() ).ShouldCalculateGraphMetrics(
            GraphMetrics.TopNBy) )
        {
            // See the comments in GraphMetricsDialog for details on how
            // top-N-by metrics must be calculated after the other metrics
            // are calculated.

            TopNByMetricCalculator2 oTopNByMetricCalculator2 =
                new TopNByMetricCalculator2();

            oCalculateGraphMetricsDialog = new CalculateGraphMetricsDialog(
                null, oWorkbook,
                new IGraphMetricCalculator2 [] {oTopNByMetricCalculator2},
                null, true);

            if (oCalculateGraphMetricsDialog.ShowDialog() != DialogResult.OK)
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAutoFillWorkbook()
    //
    /// <summary>
    /// Attempts to autofill the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="oRibbon">
    /// The workbook's Ribbon.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryAutoFillWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Ribbon oRibbon
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oRibbon != null);

        // In this case, clicking the corresponding Ribbon button opens an
        // AutoFillWorkbookDialog, which allows the user to edit the autofill
        // settings before autofilling the workbook.  The actual autofilling is
        // done by WorkbookAutoFiller, so just use that class directly.

        try
        {
            WorkbookAutoFiller.AutoFillWorkbook(
                oWorkbook, new AutoFillUserSettings() );

            oRibbon.OnWorkbookAutoFilled(false);

            return (true);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryCreateSubgraphImages()
    //
    /// <summary>
    /// Attempts to create subgraph images.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCreateSubgraphImages
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        oThisWorkbook.CreateSubgraphImages(
            CreateSubgraphImagesDialog.DialogMode.Automate);

        return (true);
    }

    //*************************************************************************
    //  Method: RunReadWorkbookTasks()
    //
    /// <summary>
    /// Runs tasks related to reading the workbook.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="sFolderToSaveWorkbookTo">
    /// If the <see cref="AutomationTasks.SaveWorkbookIfNeverSaved" /> flag is
    /// specified and the workbook has never been saved, the workbook is saved
    /// to a new file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    ///
    /// <param name="oRibbon">
    /// The workbook's Ribbon.
    /// </param>
    //*************************************************************************

    private static void
    RunReadWorkbookTasks
    (
        ThisWorkbook oThisWorkbook,
        AutomationTasks eTasksToRun,
        String sFolderToSaveWorkbookTo,
        Ribbon oRibbon
    )
    {
        Debug.Assert(oThisWorkbook != null);
        Debug.Assert(oRibbon != null);

        Boolean bReadWorkbook = ShouldRunTask(
            eTasksToRun, AutomationTasks.ReadWorkbook);

        Boolean bSaveWorkbookIfNeverSaved = ShouldRunTask(
            eTasksToRun, AutomationTasks.SaveWorkbookIfNeverSaved);

        Boolean bSaveGraphImageFile = ShouldRunTask(
            eTasksToRun, AutomationTasks.SaveGraphImageFile);

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            oThisWorkbook.InnerObject;

        if (bReadWorkbook)
        {
            // If the vertex X and Y columns were autofilled, the layout type
            // was set to LayoutType.Null.  This will cause
            // TaskPane.ReadWorkbook() to display a warning.  Temporarily turn
            // the warning off.

            Boolean bLayoutTypeIsNullNotificationsWereEnabled =
                EnableLayoutTypeIsNullNotifications(false);

            if (bSaveWorkbookIfNeverSaved || bSaveGraphImageFile)
            {
                // These tasks need to wait until the workbook is read and the
                // graph is laid out.

                EventHandler<GraphLaidOutEventArgs> oGraphLaidOutEventHandler =
                    null;

                oGraphLaidOutEventHandler =
                    delegate(Object sender, GraphLaidOutEventArgs e)
                {
                    // This delegate remains forever, even when the dialog
                    // class is destroyed.  Prevent it from being called again.

                    oThisWorkbook.GraphLaidOut -= oGraphLaidOutEventHandler;

                    if (bSaveWorkbookIfNeverSaved)
                    {
                        SaveWorkbookIfNeverSaved(oWorkbook,
                            sFolderToSaveWorkbookTo);
                    }

                    if (bSaveGraphImageFile)
                    {
                        Debug.Assert( !String.IsNullOrEmpty(
                            oThisWorkbook.Path) );

                        SaveGraphImageFile(e.NodeXLControl, e.LegendControls,
                            oThisWorkbook.FullName);
                    }
                };

                oThisWorkbook.GraphLaidOut += oGraphLaidOutEventHandler;
            }

            // Read the workbook and lay out the graph.

            oRibbon.OnReadWorkbookClick();

            EnableLayoutTypeIsNullNotifications(
                bLayoutTypeIsNullNotificationsWereEnabled);
        }
        else
        {
            if (bSaveWorkbookIfNeverSaved)
            {
                SaveWorkbookIfNeverSaved(oWorkbook, sFolderToSaveWorkbookTo);
            }
        }
    }

    //*************************************************************************
    //  Method: SaveWorkbookIfNeverSaved()
    //
    /// <summary>
    /// Saves the workbook if it has never been saved.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook to save.
    /// </param>
    ///
    /// <param name="sFolderToSaveWorkbookTo">
    /// If the workbook has never been saved, the workbook is saved to a new
    /// file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    //*************************************************************************

    private static void
    SaveWorkbookIfNeverSaved
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sFolderToSaveWorkbookTo
    )
    {
        Debug.Assert(oWorkbook != null);

        // The Workbook.Path is an empty string until the workbook is saved.

        if ( String.IsNullOrEmpty(oWorkbook.Path) )
        {
            if ( String.IsNullOrEmpty(sFolderToSaveWorkbookTo) )
            {
                sFolderToSaveWorkbookTo = Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments);
            }

            String sFileNameNoExtension;
            
            // Use a suggested file name if available; otherwise use a file
            // name based on the current time.

            if ( ( new PerWorkbookSettings(oWorkbook) ).GraphHistory
                .TryGetValue(
                    GraphHistoryKeys.ImportSuggestedFileNameNoExtension,
                    out sFileNameNoExtension) )
            {
                if (sFileNameNoExtension.Length >
                    MaximumImportSuggestedFileNameNoExtension)
                {
                    sFileNameNoExtension = sFileNameNoExtension.Substring(
                        0, MaximumImportSuggestedFileNameNoExtension);
                }

                sFileNameNoExtension = FileUtil.ReplaceIllegalFileNameChars(
                    sFileNameNoExtension, " ");
            }
            else
            {
                sFileNameNoExtension =
                    DateTimeUtil2.ToCultureInvariantFileName(DateTime.Now)
                    + " NodeXL";
            }

            ExcelUtil.SaveWorkbookAs(oWorkbook,
                Path.Combine(sFolderToSaveWorkbookTo, sFileNameNoExtension) );
        }
    }

    //*************************************************************************
    //  Method: SaveGraphImageFile()
    //
    /// <summary>
    /// Save an image of the graph to a file.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// The control that displays the graph.
    /// </param>
    ///
    /// <param name="oLegendControls">
    /// Zero or more legend controls associated with <paramref
    /// name="oNodeXLControl" />.
    /// </param>
    ///
    /// <param name="sWorkbookFilePath">
    /// Path to the workbook file.  Sample: "C:\Workbooks\TheWorkbook.xlsx".
    /// </param>
    ///
    /// <remarks>
    /// The settings stored in the AutomatedGraphImageUserSettings class are
    /// used to save the image.
    /// </remarks>
    //*************************************************************************

    private static void
    SaveGraphImageFile
    (
        NodeXLControl oNodeXLControl,
        IEnumerable<LegendControlBase> oLegendControls,
        String sWorkbookFilePath
    )
    {
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(oLegendControls != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorkbookFilePath) );

        AutomatedGraphImageUserSettings oAutomatedGraphImageUserSettings =
            new AutomatedGraphImageUserSettings();

        System.Drawing.Size oImageSizePx =
            oAutomatedGraphImageUserSettings.ImageSizePx;

        Int32 iWidth = oImageSizePx.Width;
        Int32 iHeight = oImageSizePx.Height;

        Boolean bIncludeHeader = oAutomatedGraphImageUserSettings.IncludeHeader;
        Boolean bIncludeFooter = oAutomatedGraphImageUserSettings.IncludeFooter;

        Debug.Assert(!bIncludeHeader ||
            oAutomatedGraphImageUserSettings.HeaderText != null);

        Debug.Assert(!bIncludeFooter ||
            oAutomatedGraphImageUserSettings.FooterText != null);

        GraphImageCompositor oGraphImageCompositor =
            new GraphImageCompositor(oNodeXLControl);

        UIElement oCompositeElement = oGraphImageCompositor.Composite(
            iWidth, iHeight,
            bIncludeHeader ? oAutomatedGraphImageUserSettings.HeaderText : null,
            bIncludeFooter ? oAutomatedGraphImageUserSettings.FooterText : null,
            oAutomatedGraphImageUserSettings.HeaderFooterFont, oLegendControls
            );

        System.Drawing.Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(
            oCompositeElement, iWidth, iHeight);

        ImageFormat eImageFormat =
            oAutomatedGraphImageUserSettings.ImageFormat;

        String sImageFilePath = Path.ChangeExtension( sWorkbookFilePath,
            SaveableImageFormats.GetFileExtension(eImageFormat) );

        try
        {
            oBitmap.Save(sImageFilePath, eImageFormat);
        }
        catch (System.Runtime.InteropServices.ExternalException)
        {
            // When an image file already exists and is read-only, an
            // ExternalException is thrown.
            //
            // Note that this method is called from the
            // ThisWorkbook.GraphLaidOut event handler, so this exception can't
            // be handled by a TaskAutomator.AutomateOneWorkbook() exception
            // handler.

            FormUtil.ShowWarning( String.Format(
                "The image file \"{0}\" couldn't be saved.  Does a read-only"
                + " file with the same name already exist?"
                ,
                sImageFilePath
                ) );
        }
        finally
        {
            oBitmap.Dispose();

            oGraphImageCompositor.RestoreNodeXLControl();
        }
    }

    //*************************************************************************
    //  Method: RunEditableCommand()
    //
    /// <summary>
    /// Runs a command whose event arguments are derived from <see
    /// cref="RunEditableCommandEventArgs" />.
    /// </summary>
    ///
    /// <param name="oRunEditableCommandEventArgs">
    /// Provides information for the command.
    /// </param>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if the command was successfully run.
    /// </returns>
    //*************************************************************************

    private static Boolean
    RunEditableCommand
    (
        RunEditableCommandEventArgs oRunEditableCommandEventArgs,
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oRunEditableCommandEventArgs != null);
        Debug.Assert(oThisWorkbook != null);

        // Arbitrarily use the workbook as the sender here.  TaskAutomator
        // can't be used, because it's static.

        CommandDispatcher.SendCommand(oThisWorkbook,
            oRunEditableCommandEventArgs);

        return (oRunEditableCommandEventArgs.CommandSuccessfullyRun);
    }

    //*************************************************************************
    //  Method: EnableLayoutTypeIsNullNotifications()
    //
    /// <summary>
    /// Enables or disables notifications for "layout type is null."
    /// </summary>
    ///
    /// <param name="bNewValue">
    /// true to enable the notifications, false to disable them.
    /// </param>
    ///
    /// <returns>
    /// true if the notifications were enabled before this method was called.
    /// </returns>
    //*************************************************************************

    private static Boolean
    EnableLayoutTypeIsNullNotifications
    (
        Boolean bNewValue
    )
    {
        NotificationUserSettings oNotificationUserSettings =
            new NotificationUserSettings();

        Boolean bOldValue = oNotificationUserSettings.LayoutTypeIsNull;

        if (bNewValue != bOldValue)
        {
            oNotificationUserSettings.LayoutTypeIsNull = bNewValue;
            oNotificationUserSettings.Save();
        }

        return (bOldValue);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    // Maximum length of the file name suggested when a graph is imported.

    private const Int32 MaximumImportSuggestedFileNameNoExtension = 80;
}

}
