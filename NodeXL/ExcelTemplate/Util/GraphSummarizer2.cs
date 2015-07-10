using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Common;
using Smrf.XmlLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace Smrf.NodeXL.ExcelTemplate
{
    public static class GraphSummarizer2 : Object
    {

        public static Boolean
        TrySummarizeGraph
        (
            Microsoft.Office.Interop.Excel.Workbook workbook,
            out String graphSummary,
            out String xmlGraphSummary
        )
        {
            Debug.Assert(workbook != null);

            PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(workbook);

            String graphSource, graphTerm;

            if (TryGetGraphSourceTerm(oPerWorkbookSettings, out graphSource, out graphTerm))
            {
                if (graphSource.Contains("Twitter"))
                {
                    new TwitterGraphSummarizer(workbook, oPerWorkbookSettings,
                                        graphSource, graphTerm)
                                        .Summarize(out graphSummary, out xmlGraphSummary);

                    return (true);
                }
            }

            //In case this is not a network for which 
            //we have a summarizer, use the general summarizer
            GraphSummarizer.TrySummarizeGraph(workbook, out xmlGraphSummary);
            graphSummary = xmlGraphSummary;

            return (true);
        }

        public static String
        SummarizeGraph
        (
            Microsoft.Office.Interop.Excel.Workbook workbook
        )
        {
            Debug.Assert(workbook != null);

            PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(workbook);

            String graphSource, graphTerm;

            String graphSummary, xmlGraphSummary;

            if (TryGetGraphSourceTerm(oPerWorkbookSettings, out graphSource, out graphTerm))
            {
                if (graphSource.Contains("Twitter"))
                {
                    new TwitterGraphSummarizer(workbook, oPerWorkbookSettings,
                                        graphSource, graphTerm)
                                        .Summarize(out graphSummary, out xmlGraphSummary);

                    //This method is used by the automation
                    //just return the XML document to upload
                    return (xmlGraphSummary);
                }
            }

            //In case this is not a network for which 
            //we have a summarizer, use the general summarizer
            GraphSummarizer.TrySummarizeGraph(workbook, out xmlGraphSummary);


            return (xmlGraphSummary);
        }

        private static Boolean
        TryGetGraphSourceTerm
        (
            PerWorkbookSettings perWorkbookSettings,
            out String graphSource,
            out String graphTerm
        )
        {
            perWorkbookSettings.GraphHistory.TryGetValue(GraphHistoryKeys.GraphTerm, out graphTerm);

            return (perWorkbookSettings.GraphHistory.TryGetValue(
                        GraphHistoryKeys.GraphSource, out graphSource)
                        );

        }
    }
}
