
using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SubgraphImageColumnPopulator
//
/// <summary>
/// Populates the subgraph image column on the vertex table with images that
/// have been stored in a temporary folder.
/// </summary>
///
/// <remarks>
/// Call <see cref="PopulateSubgraphImageColumn" /> to populate the subgraph
/// image column.  Call <see cref="ShowOrHideSubgraphImages" /> to hide or show
/// the subgraph images.  Call <see cref="DeleteSubgraphImages" /> to delete
/// the subgraph images.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class SubgraphImageColumnPopulator : Object
{
    //*************************************************************************
    //  Method: PopulateSubgraphImageColumn()
    //
    /// <summary>
    /// Populates the subgraph image column on the vertex table with images
    /// that have been stored in a temporary folder.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertex table.
    /// </param>
    ///
    /// <param name="temporarySubgraphImages">
    /// Contains information about the subgraph images that should be inserted.
    /// </param>
    ///
    /// <remarks>
    /// If a subgraph image column doesn't already exist, this method inserts
    /// it into the vertex table.  It then populates the column with the
    /// temporary images specified by <paramref
    /// name="temporarySubgraphImages" />, and deletes the temporary folder
    /// containing the subgraph images.
    ///
    /// <para>
    /// The images are shown by default.  Call <see
    /// cref="ShowOrHideSubgraphImages" /> to hide or reshow them.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    PopulateSubgraphImageColumn
    (
        Workbook workbook,
        TemporaryImages temporarySubgraphImages
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(temporarySubgraphImages != null);

        Boolean bOriginalScreenUpdating = workbook.Application.ScreenUpdating;
        workbook.Application.ScreenUpdating = false;

        try
        {
            PopulateSubgraphImageColumnInternal(workbook,
                temporarySubgraphImages);
        }
        finally
        {
            workbook.Application.ScreenUpdating = bOriginalScreenUpdating;
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideSubgraphImages()
    //
    /// <summary>
    /// Shows or hides the subgraph images.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the subgraph images.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the images, false to hide them.
    /// </param>
    ///
    /// <remarks>
    /// This methods shows or hides the subgraph images without affecting the
    /// visibility of the subgraph image column itself.
    /// </remarks>
    //*************************************************************************

    public static void
    ShowOrHideSubgraphImages
    (
        Workbook workbook,
        Boolean show
    )
    {
        Debug.Assert(workbook != null);

        MsoTriState eVisible = show ?
            MsoTriState.msoTrue : MsoTriState.msoFalse; 

        foreach ( Microsoft.Office.Interop.Excel.Shape oSubgraphImage in
            EnumerateSubgraphImages(workbook) )
        {
            oSubgraphImage.Visible = eVisible;
        }
    }

    //*************************************************************************
    //  Method: DeleteSubgraphImages()
    //
    /// <summary>
    /// Deletes any subgraph images.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the subgraph images.
    /// </param>
    //*************************************************************************

    public static void
    DeleteSubgraphImages
    (
        Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        foreach ( Microsoft.Office.Interop.Excel.Shape oSubgraphImage in
            EnumerateSubgraphImages(workbook) )
        {
            oSubgraphImage.Delete();
        }
    }

    //*************************************************************************
    //  Method: PopulateSubgraphImageColumnInternal()
    //
    /// <summary>
    /// Populates the subgraph image column on the vertex table with images
    /// that have been stored in a temporary folder.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the vertex table.
    /// </param>
    ///
    /// <param name="oTemporarySubgraphImages">
    /// Contains information about the subgraph images that should be inserted.
    /// </param>
    ///
    /// <remarks>
    /// If a subgraph image column doesn't already exist, this method inserts
    /// it into the vertex table.  It then populates the column with the
    /// temporary images specified by <paramref
    /// name="oTemporarySubgraphImages" />, and deletes the temporary folder
    /// containing the subgraph images.
    ///
    /// <para>
    /// The images are shown by default.  Call <see
    /// cref="ShowOrHideSubgraphImages" /> to hide or reshow them.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    private static void
    PopulateSubgraphImageColumnInternal
    (
        Workbook oWorkbook,
        TemporaryImages oTemporarySubgraphImages
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oTemporarySubgraphImages != null);

        ListObject oVertexTable;
        Range oVisibleNameColumnData, oVisibleSubgraphImageColumnData;
        String sTemporaryImageFolder = oTemporarySubgraphImages.Folder;

        if (
            // If the vertex table, the name column data, or the image column
            // data aren't available, nothing can be done.

            !TryGetVertexTableAndVisibleColumnData(oWorkbook, out oVertexTable,
                out oVisibleNameColumnData,
                out oVisibleSubgraphImageColumnData)

            // If no temporary images were created, nothing more needs to be
            // done.

            ||
            sTemporaryImageFolder == null
            )

        {
            return;
        }

        Int32 iAreas = oVisibleNameColumnData.Areas.Count;

        if (iAreas != oVisibleSubgraphImageColumnData.Areas.Count)
        {
            return;
        }

        SizeF oSubgraphImageSizePt = GetSubgraphImageSizePt(
            oTemporarySubgraphImages.ImageSizePx, oWorkbook);

        // Get any old images in the image column as a dictionary.  This
        // significantly speeds up the deletion of the old images, because
        // Excel doesn't have to do a linear search on Shape.Name as each image
        // is deleted by PopulateAreaWithSubgraphImages().

        Debug.Assert(oVertexTable.Parent is Worksheet);

        Dictionary<String, Microsoft.Office.Interop.Excel.Shape>
            oOldSubgraphImages = GetSubgraphImageDictionary(
                (Worksheet)oVertexTable.Parent);

        for (Int32 iArea = 1; iArea <= iAreas; iArea++)
        {
            PopulateAreaWithSubgraphImages(
                oVisibleNameColumnData.Areas[iArea],
                oVisibleSubgraphImageColumnData.Areas[iArea],
                oSubgraphImageSizePt, oOldSubgraphImages,
                oTemporarySubgraphImages);
        }

        DeleteTemporaryImageFolder(sTemporaryImageFolder);
    }

    //*************************************************************************
    //  Method: TryGetVertexTableAndVisibleColumnData()
    //
    /// <summary>
    /// Attempts to get the vertex table and the visible ranges for the name
    /// and subgraph image columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the vertex table.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Where the vertex table gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oVisibleNameColumnData">
    /// Where the visible range for the name column gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="oVisibleSubgraphImageColumnData">
    /// Where the visible range for the subgraph image column gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetVertexTableAndVisibleColumnData
    (
        Workbook oWorkbook,
        out ListObject oVertexTable,
        out Range oVisibleNameColumnData,
        out Range oVisibleSubgraphImageColumnData
    )
    {
        Debug.Assert(oWorkbook != null);

        oVertexTable = null;
        oVisibleNameColumnData = null;
        oVisibleSubgraphImageColumnData = null;

        Range oNameColumnData, oSubgraphImageColumnData;

        return (

            // If the vertex table, the name column data, or the image column
            // data aren't available, nothing can be done.

            ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
                TableNames.Vertices, out oVertexTable) 
            &&
            ExcelTableUtil.TryGetTableColumnData(oVertexTable,
                VertexTableColumnNames.VertexName, out oNameColumnData)
            &&
            TryGetSubgraphImageColumnData(oVertexTable,
                out oSubgraphImageColumnData)


            // Reduce the name and image column data to visible areas only.

            &&
            ExcelUtil.TryGetVisibleRange(oNameColumnData,
                out oVisibleNameColumnData)
            &&
            ExcelUtil.TryGetVisibleRange(oSubgraphImageColumnData,
                out oVisibleSubgraphImageColumnData)
            );
    }

    //*************************************************************************
    //  Method: TryGetSubgraphImageColumnData()
    //
    /// <summary>
    /// Gets the subgraph image column.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// Table containing the subgraph image column.
    /// </param>
    ///
    /// <param name="oSubgraphImageColumnData">
    /// Where the subgraph image column data gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If a subgraph image column doesn't already exist, this method inserts
    /// it into the vertex table.
    /// </remarks>
    //*************************************************************************

    private static Boolean
    TryGetSubgraphImageColumnData
    (
        ListObject oVertexTable,
        out Range oSubgraphImageColumnData
    )
    {
        Debug.Assert(oVertexTable != null);

        oSubgraphImageColumnData = null;

        ListColumn oSubgraphImageColumn;

        if (
            ExcelTableUtil.TryGetOrInsertTableColumn(oVertexTable,
                VertexTableColumnNames.SubgraphImage,
                OneBasedSubgraphImageColumnIndex,
                SubgraphImageColumnWidthChars, null, out oSubgraphImageColumn)
            &&
            ExcelTableUtil.TryGetTableColumnData(oSubgraphImageColumn,
                out oSubgraphImageColumnData)
            )
        {
            ShiftColumnGroupNames(oVertexTable);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: ShiftColumnGroupNames()
    //
    /// <summary>
    /// Shifts the column group names in the first row of the worksheet one
    /// cell to the right if necessary.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// Table containing the subgraph image column.
    /// </param>
    ///
    /// <remarks>
    /// Inserting the subgraph image column did not insert an empty cell into
    /// row 1 of the worksheet, the row that contains column group names
    /// ("Visual Properties", "Labels", etc.).  The result is that the
    /// "Subgraph" header in cell B2 ends up below the "Visual Properties"
    /// group name in cell B1.  This method inserts an empty cell into B1,
    /// which causes "Visual Properties" to be in cell C1, right above "Color"
    /// in cell C2, where it belongs.
    /// </remarks>
    //*************************************************************************

    private static void
    ShiftColumnGroupNames
    (
        ListObject oVertexTable
    )
    {
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oVertexTable.Parent is Worksheet);

        Worksheet oWorksheet = (Worksheet)oVertexTable.Parent;
        Range oRange = ExcelUtil.GetRange(oWorksheet, "B1");
        Object oValue = ExcelUtil.GetRangeValues(oRange)[1, 1];

        if (oValue is String && (String)oValue == "Visual Properties")
        {
            // Note: Don't try to use Range.Insert() here.  That raises a
            // COMException if the table has hidden rows.  Selecting the range
            // and then inserting at the selection works.

            oRange.Select();

            oWorksheet.Application.Selection.Insert(
                XlInsertShiftDirection.xlShiftToRight,
                XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        }
    }

    //*************************************************************************
    //  Method: PopulateAreaWithSubgraphImages()
    //
    /// <summary>
    /// Populates one area of the image column with subgraph images.
    /// </summary>
    ///
    /// <param name="oNameColumnArea">
    /// Area from the name column.
    /// </param>
    ///
    /// <param name="oSubgraphImageColumnArea">
    /// Corresponding area from the subgraph image column.
    /// </param>
    ///
    /// <param name="oSubgraphImageSizePt">
    /// Size of each subgraph image, in points.
    /// </param>
    ///
    /// <param name="oOldSubgraphImages">
    /// A dictionary of zero or more key/value pairs.  The key is the
    /// Shape.Name of an old image in the subgraph image column and the value
    /// is the image, as a Shape.
    /// </param>
    ///
    /// <param name="oTemporarySubgraphImages">
    /// Contains information about the subgraph images that should be inserted.
    /// </param>
    ///
    /// <remarks>
    /// This method populates <paramref name="oSubgraphImageColumnArea" /> with
    /// the temporary subgraph images specified by <paramref
    /// name="oTemporarySubgraphImages" />.
    /// </remarks>
    //*************************************************************************

    private static void
    PopulateAreaWithSubgraphImages
    (
        Range oNameColumnArea,
        Range oSubgraphImageColumnArea,
        SizeF oSubgraphImageSizePt,

        Dictionary<String, Microsoft.Office.Interop.Excel.Shape>
            oOldSubgraphImages,

        TemporaryImages oTemporarySubgraphImages
    )
    {
        Debug.Assert(oNameColumnArea != null);
        Debug.Assert(oSubgraphImageColumnArea != null);
        Debug.Assert(oOldSubgraphImages != null);
        Debug.Assert(oTemporarySubgraphImages != null);

        // Gather some required information.

        Int32 iRows = oNameColumnArea.Rows.Count;

        Debug.Assert(iRows == oSubgraphImageColumnArea.Rows.Count);

        Debug.Assert(oNameColumnArea.Parent is Worksheet);

        Worksheet oWorksheet = (Worksheet)oNameColumnArea.Parent;

        Microsoft.Office.Interop.Excel.Shapes oShapes = oWorksheet.Shapes;

        Object [,] aoNameValues = ExcelUtil.GetRangeValues(oNameColumnArea);

        Dictionary<String, String> oFileNames =
            oTemporarySubgraphImages.FileNames;

        // Set the row heights to fit the images.

        oNameColumnArea.RowHeight =
            oSubgraphImageSizePt.Height + 2 * SubgraphImageMarginPt;

        // Get the first cell in the subgraph image column.

        Range oSubgraphImageCell = (Range)oSubgraphImageColumnArea.Cells[1, 1];

        // Loop through the area's rows.

        for (Int32 iRow = 1; iRow <= iRows; iRow++)
        {
            String sName, sFileName;

            // Check whether the row's name cell has a corresponding file name
            // in the dictionary.

            if (
                ExcelUtil.TryGetNonEmptyStringFromCell(aoNameValues, iRow, 1,
                    out sName)
                &&
                oFileNames.TryGetValue(sName, out sFileName)
                )
            {
                // Give the picture a name that can be recognized by
                // GetSubgraphImageDictionary().

                String sPictureName =
                    VertexTableColumnNames.SubgraphImage + "-" + sName;

                Microsoft.Office.Interop.Excel.Shape oPicture;

                // If an old version of the picture remains from a previous
                // call to this method, delete it.

                if ( oOldSubgraphImages.TryGetValue(sPictureName,
                    out oPicture) )
                {
                    oPicture.Delete();
                }

                String sFileNameWithPath = Path.Combine(
                    oTemporarySubgraphImages.Folder, sFileName);

                oPicture = oShapes.AddPicture(sFileNameWithPath,
                    MsoTriState.msoFalse, MsoTriState.msoCTrue,

                    (Single)(Double)oSubgraphImageCell.Left
                        + SubgraphImageMarginPt,

                    (Single)(Double)oSubgraphImageCell.Top
                        + SubgraphImageMarginPt,

                    oSubgraphImageSizePt.Width,
                    oSubgraphImageSizePt.Height
                    );

                oPicture.Name = sPictureName;
            }

            // Move down one cell in the image column.

            oSubgraphImageCell = oSubgraphImageCell.get_Offset(1, 0);
        }
    }

    //*************************************************************************
    //  Method: EnumerateSubgraphImages()
    //
    /// <summary>
    /// Enumerates the images in the subgraph image column.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the subgraph images.
    /// </param>
    ///
    /// <returns>
    /// The enumerated images.
    /// </returns>
    //*************************************************************************

    private static IEnumerable<Microsoft.Office.Interop.Excel.Shape>
    EnumerateSubgraphImages
    (
        Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        Worksheet oWorksheet;

        if ( ExcelUtil.TryGetWorksheet(workbook, WorksheetNames.Vertices,
            out oWorksheet) )
        {
            foreach (Microsoft.Office.Interop.Excel.Shape oSubgraphImage in
                GetSubgraphImageDictionary(oWorksheet).Values)
            {
                yield return (oSubgraphImage);
            }
        }
    }

    //*************************************************************************
    //  Method: GetSubgraphImageDictionary()
    //
    /// <summary>
    /// Gets a dictionary of any subgraph images that were inserted by this
    /// class.
    /// </summary>
    ///
    /// <param name="oWorksheet">
    /// Worksheet containing the subgraph images.
    /// </param>
    ///
    /// <returns>
    /// A dictionary of zero or more key/value pairs.  The key is the
    /// Shape.Name of an image in the subgraph image column and the value is
    /// the image, as a Shape.
    /// </returns>
    //*************************************************************************

    private static Dictionary<String, Microsoft.Office.Interop.Excel.Shape>
    GetSubgraphImageDictionary
    (
        Worksheet oWorksheet
    )
    {
        Debug.Assert(oWorksheet != null);

        Dictionary<String, Microsoft.Office.Interop.Excel.Shape>
            oSubgraphImagesInColumn =
                new Dictionary<String, Microsoft.Office.Interop.Excel.Shape>();

        foreach (Microsoft.Office.Interop.Excel.Shape oShape in
            oWorksheet.Shapes)
        {
            // The names of the images added by this class start with the
            // subgraph image column name.

            String sShapeName = oShape.Name;

            if ( sShapeName != null &&
                sShapeName.StartsWith(VertexTableColumnNames.SubgraphImage) )
            {
                oSubgraphImagesInColumn[sShapeName] = oShape;
            }
        }

        return (oSubgraphImagesInColumn);
    }

    //*************************************************************************
    //  Method: GetSubgraphImageSizePt()
    //
    /// <summary>
    /// Gets the size of each subgraph image, in points.
    /// </summary>
    ///
    /// <param name="iSubgraphImageSizePx">
    /// The size of each subgraph image, in pixels.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the table.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="iSubgraphImageSizePx" /> converted to points.
    /// </returns>
    //*************************************************************************

    private static SizeF
    GetSubgraphImageSizePt
    (
        Size iSubgraphImageSizePx,
        Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        // There doesn't seem to be a direct way to get the screen resolution
        // from Excel or from .NET.  As a workaround, leveredge the fact that 
        // a new bitmap is given a resolution equal to the screen resolution.
        //
        // Because this method is called one time only, this workaround is
        // acceptable.

        Bitmap oBitmap = new Bitmap(10, 10);

        Single fPxPerInchX = oBitmap.HorizontalResolution;
        Single fPxPerInchY = oBitmap.VerticalResolution;

        oBitmap.Dispose();

        const Single PointsPerInch = 72;

        return ( new SizeF(
            (Single)iSubgraphImageSizePx.Width * PointsPerInch / fPxPerInchX,
            (Single)iSubgraphImageSizePx.Height * PointsPerInch / fPxPerInchY
            ) );
    }

    //*************************************************************************
    //  Method: DeleteTemporaryImageFolder()
    //
    /// <summary>
    /// Deletes the entire temporary image folder.
    /// </summary>
    ///
    /// <param name="sTemporaryImageFolder">
    /// Full path to the temporary folder.
    /// </param>
    //*************************************************************************

    private static void
    DeleteTemporaryImageFolder
    (
        String sTemporaryImageFolder
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTemporaryImageFolder) );

        try
        {
            Directory.Delete(sTemporaryImageFolder, true);
        }
        catch (IOException)
        {
            // A user reported the following exception thrown from the above
            // Directory.Delete() call:
            //
            // "System.IO.IOException: The directory is not empty.:
            //
            // Others have reported this happenning at random times.  For
            // example:
            //
            // http://forums.asp.net/p/1114215/1722498.aspx
            //
            // I have also seen it happen from the command line outside of
            // .NET.  When it occurs, the directory IS empty but cannot be
            // accessed in any way.  The directory disappears when the machine
            // is rebooted.
            //
            // I can't figure out the cause or a fix.  Ignore the problem,
            // which seems to be benign.
        }
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// One-based index where the subgraph image column should be inserted.

    private const Int32 OneBasedSubgraphImageColumnIndex = 2;

    /// Width of the subgraph image column, in characters.

    private const Single SubgraphImageColumnWidthChars = 11;

    /// Margin between the subgraph image and the subgraph image cell, in
    /// points.

    private const Single SubgraphImageMarginPt = 2;
}

}
