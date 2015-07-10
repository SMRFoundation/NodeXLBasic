using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.ExcelTemplate
{
    //*****************************************************************************
    //  Class: TwitterSearchNetworkTopItemsReader
    //
    /// <summary>
    /// Class that knows how to read the the "top items in the tweets within a
    /// Twitter search network" from a NodeXL workbook.
    /// </summary>
    ///
    /// <remarks>
    /// Call <see cref="TopMetricsReaderBase.TryReadMetrics" /> to attempt to read
    /// the top Twitter items.
    ///
    /// <para>
    /// This class does not calculate the top items.  Instead, it reads the top
    /// items that were calculated by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" /> and written to the
    /// workbook.
    /// </para>
    ///
    /// </remarks>
    //*****************************************************************************
    public class TopMetricsWithCountReader
    {
        //*************************************************************************
        //  Constructor: TwitterSearchNetworkTopItemsReader()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="TwitterSearchNetworkTopItemsReader" /> class.
        /// </summary>
        //*************************************************************************

        public TopMetricsWithCountReader
        (
            String worksheetName,
            String tableNameRoot
        )        
        {
            m_sWorksheetName = worksheetName;
            m_sTableNameRoot = tableNameRoot;

            AssertValid();
        }

        //*************************************************************************
        //  Method: TryReadMetrics()
        //
        /// <summary>
        /// Attempts to read the top metrics from a workbook.
        /// </summary>
        ///
        /// <param name="workbook">
        /// Workbook containing the graph data.
        /// </param>
        ///
        /// <param name="topMetrics">
        /// Where a summary of the top metrics gets stored if true is returned.
        /// Gets set to an empty string if false is returned.
        /// </param>
        ///
        /// <returns>
        /// true if successful.
        /// </returns>
        ///
        /// <remarks>
        /// This method attempts to read the top metrics that have already been
        /// calculated and written to one or more tables in a worksheet in a NodeXL
        /// workbook.  If it successfully reads them, a summary of them gets stored
        /// at <paramref name="topMetrics" /> and true is returned.  false is
        /// returned otherwise.
        /// </remarks>
        //*************************************************************************

        public Boolean
        TryReadMetrics
        (
            Microsoft.Office.Interop.Excel.Workbook workbook,
            out String topMetrics
        )
        {
            Debug.Assert(workbook != null);
            AssertValid();

            Worksheet oWorksheet;
            StringBuilder oTopMetrics = new StringBuilder();

            if (ExcelUtil.TryGetWorksheet(workbook, m_sWorksheetName,
                out oWorksheet))
            {
                foreach (ListObject oTopMetricsTable in oWorksheet.ListObjects)
                {
                    if (oTopMetricsTable.Name.StartsWith(m_sTableNameRoot))
                    {
                        ReadTopColumns(oTopMetricsTable, oTopMetrics);
                    }
                }
            }

            topMetrics = oTopMetrics.ToString();
            return (topMetrics.Length > 0);
        }

        //*************************************************************************
        //  Method: ReadTopColumns()
        //
        /// <summary>
        /// Reads the "Top" columns in a table.
        /// </summary>
        ///
        /// <param name="oTopMetricsTable">
        /// The table to read the "Top" columns from.
        /// </param>
        ///
        /// <param name="oTopMetrics">
        /// Where the column contents get appended.
        /// </param>
        ///
        /// <remarks>
        /// This method provides a text representation of top items stored in a
        /// table.  It is meant for use with a table that has one or more columns
        /// whose column headers start with "Top " ("Top URLs", for example).  For
        /// each such column, this method appends the column header to <paramref
        /// name="oTopMetrics" />, then appends the contents of each cell in the
        /// column.
        /// </remarks>
        //*************************************************************************

        protected void
        ReadTopColumns
        (
            ListObject oTopMetricsTable,
            StringBuilder oTopMetrics
        )
        {
            Debug.Assert(oTopMetricsTable != null);
            Debug.Assert(oTopMetrics != null);

            String sTopColumnHeader = null;
            String sCountColumnHeader = null;

            foreach (ListColumn oColumn in oTopMetricsTable.ListColumns)
            {
                String sColumnHeader = oColumn.Name;

                if (sColumnHeader.StartsWith("Top "))
                {
                    sTopColumnHeader = sColumnHeader;                    
                }
                else if (sColumnHeader.Contains("Count"))
                {
                    sCountColumnHeader = sColumnHeader;
                }

                if(!String.IsNullOrEmpty(sTopColumnHeader) &&
                    !String.IsNullOrEmpty(sCountColumnHeader))
                {
                    ReadTopColumn(oTopMetricsTable, sTopColumnHeader,
                                    sCountColumnHeader, oTopMetrics);
                    sTopColumnHeader = sCountColumnHeader = null;                    
                }
            }
        }

        //*************************************************************************
        //  Method: ReadTopColumn()
        //
        /// <summary>
        /// Reads one "Top" column in a table.
        /// </summary>
        ///
        /// <param name="oTopMetricsTable">
        /// The table to read the "Top" column from.
        /// </param>
        ///
        /// <param name="sColumnHeader">
        /// Header text for the column to read.
        /// </param>
        ///
        /// <param name="oTopMetrics">
        /// Where the column contents get appended.
        /// </param>
        ///
        /// <remarks>
        /// This method appends the column header to <paramref
        /// name="oTopMetrics" />, then appends the contents of each cell in the
        /// column.
        /// </remarks>
        //*************************************************************************

        protected void
        ReadTopColumn
        (
            ListObject oTopMetricsTable,
            String sTopColumnHeader,
            String sCountColumnHeader,
            StringBuilder oTopMetrics
        )
        {
            Debug.Assert(oTopMetricsTable != null);
            Debug.Assert(!String.IsNullOrEmpty(sTopColumnHeader));
            Debug.Assert(!String.IsNullOrEmpty(sCountColumnHeader));
            Debug.Assert(oTopMetrics != null);

            StringBuilder oTopColumn = new StringBuilder();
            Boolean bColumnIsEmpty = true;

            oTopColumn.Append(sTopColumnHeader);
            oTopColumn.Append(':');

            ExcelTableReader oExcelTableReader =
                new ExcelTableReader(oTopMetricsTable);

            foreach (ExcelTableReader.ExcelTableRow oRow in
                oExcelTableReader.GetRows())
            {
                String sTopItemName;
                String sCountItemName;

                if (oRow.TryGetNonEmptyStringFromCell(sCountColumnHeader,
                    out sCountItemName))
                {
                    //StringUtil.AppendAfterEmptyLine(oTopColumn, sItemName);
                    bColumnIsEmpty = false;
                }

                if (oRow.TryGetNonEmptyStringFromCell(sTopColumnHeader,
                    out sTopItemName))
                {
                    StringUtil.AppendAfterEmptyLine(oTopColumn, 
                        String.Format("[{0}]\t{1}", sCountItemName, sTopItemName));
                    bColumnIsEmpty = false;
                }
            }

            if (!bColumnIsEmpty)
            {
                StringUtil.AppendSectionSeparator(oTopMetrics);
                oTopMetrics.Append(oTopColumn.ToString());
            }
        }

        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        [Conditional("DEBUG")]

        public new void
        AssertValid()
        {
            Debug.Assert(!String.IsNullOrEmpty(m_sWorksheetName));
            Debug.Assert(!String.IsNullOrEmpty(m_sTableNameRoot));
        }

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// Name of the worksheet containing the top metrics tables.

        protected String m_sWorksheetName;

        /// Root of the name of each top metric table on the worksheet.

        protected String m_sTableNameRoot;
    
    }
}
