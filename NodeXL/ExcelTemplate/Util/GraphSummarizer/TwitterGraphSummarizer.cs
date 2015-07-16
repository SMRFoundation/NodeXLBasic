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
    public class TwitterGraphSummarizer
    {
        public TwitterGraphSummarizer
        (
            Microsoft.Office.Interop.Excel.Workbook workbook,
            PerWorkbookSettings perWorkbookSettings,
            String graphSource,
            String graphTerm        
        )
        {
            this._workbook = workbook;
            this._perWorkbookSettings = perWorkbookSettings;

            _twitterGraphDescription = new TwitterGraphDescription(graphSource, graphTerm);
        }

        public void
        Summarize
        (
            out String graphSummary,
            out String xmlGraphSummary
        )
        {
            StringBuilder oStringBuilder = new StringBuilder();

            AppendGraphHistoryValues(oStringBuilder,
                GraphHistoryKeys.ImportDescription,
                GraphHistoryKeys.GraphDirectedness,
                GraphHistoryKeys.GroupingDescription,
                GraphHistoryKeys.LayoutAlgorithm
                );

            AddDescription(oStringBuilder.ToString());
            AddSections();

            xmlGraphSummary = this._twitterGraphDescription.ToString();
            graphSummary = this._twitterGraphDescription.ToDisplayableString();
        }

        private void
        AddDescription
        (
            String description            
        )
        {
            this._twitterGraphDescription.AppendDescriptionXmlNode(description);
        }

        private void
        AddShortDescription
        (
            String shortDescription
        )
        {
            this._twitterGraphDescription.AppendShortDescriptionXmlNode(shortDescription);
        }

        private void
        AddSections
        (
            
        )
        {
            XmlNode oSections = this._twitterGraphDescription.AppendSectionsXmlNode();

            //Add Overall Metrics section
            AddOverallMetrics(oSections);
            //Add TopNByMetrics
            AddTopNByMetrics(oSections);
            //Add Top Metrics
            AddTopMetrics(oSections);            
        }

        private void
        AddOverallMetrics
        (
            XmlNode oSections            
        )
        {
            XmlNode oSection = this._twitterGraphDescription.AppendSectionXmlNode(oSections, "OverallMetrics", "Overall Graph Metrics");

            OverallMetrics oOverallMetrics;

            if ((new OverallMetricsReader()).TryReadMetrics(
                this._workbook, out oOverallMetrics))
            {
                XmlNode oElement, oActions;

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                    OverallMetricNames.Vertices, oOverallMetrics.Vertices.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.Vertices);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                    OverallMetricNames.UniqueEdges, oOverallMetrics.UniqueEdges.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.UniqueEdges);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.EdgesWithDuplicates, oOverallMetrics.EdgesWithDuplicates.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.EdgesWithDuplicates);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.TotalEdges, oOverallMetrics.TotalEdges.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.TotalEdges);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.SelfLoops, oOverallMetrics.SelfLoops.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.SelfLoops);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.ReciprocatedVertexPairRatio,
                     NullableToOverallMetricValue<Double>(oOverallMetrics.ReciprocatedVertexPairRatio)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.ReciprocatedVertexPairRatio);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.ReciprocatedEdgeRatio,
                     NullableToOverallMetricValue<Double>(oOverallMetrics.ReciprocatedEdgeRatio)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.ReciprocatedEdgeRatio);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.ConnectedComponents, oOverallMetrics.ConnectedComponents.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.ConnectedComponents);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.SingleVertexConnectedComponents, oOverallMetrics.SingleVertexConnectedComponents.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.SingleVertexConnectedComponents);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.MaximumConnectedComponentVertices, oOverallMetrics.MaximumConnectedComponentVertices.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.MaximumConnectedComponentVertices);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.MaximumConnectedComponentEdges, oOverallMetrics.MaximumConnectedComponentEdges.ToString());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.MaximumConnectedComponentEdges);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.MaximumGeodesicDistance,
                     NullableToOverallMetricValue<Int32>(oOverallMetrics.MaximumGeodesicDistance)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.MaximumGeodesicDistance);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.AverageGeodesicDistance,
                     NullableToOverallMetricValue<Double>(oOverallMetrics.AverageGeodesicDistance)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.AverageGeodesicDistance);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.GraphDensity,
                     NullableToOverallMetricValue<Double>(oOverallMetrics.GraphDensity)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.GraphDensity);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.Modularity,
                     NullableToOverallMetricValue<Double>(oOverallMetrics.Modularity)
                     );
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.Modularity);

                oElement = this._twitterGraphDescription.AppendKeyValueElementXmlNode(oSection,
                     OverallMetricNames.NodeXLVersion, AssemblyUtil2.GetFileVersion());
                oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                this._twitterGraphDescription.AppendActionXmlNode(oActions, "tooltip",
                    OverallMetricTooltips.NodeXLVersion);
            }
        }

        private void
        AddTopNByMetrics
        (
            XmlNode oSections
        )
        {
            XmlNode oSection = null;

            String sTopNByMetrics;

            (new TopNByMetricsReader()).TryReadMetrics(
                this._workbook, out sTopNByMetrics);

            String[] oItems = sTopNByMetrics.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<String, Dictionary<String,String>> oVertexData = GetDataFromVertexName(oItems);

            foreach (String sItem in oItems)
            {
                if (!sItem.StartsWith("Top "))
                {
                    XmlNode oElement = this._twitterGraphDescription.AppendValueElementXmlNode(oSection, sItem);
                    XmlNode oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                    this._twitterGraphDescription.AppendActionXmlNode(oActions, "image", oVertexData[sItem][_imageUriKey]);
                    this._twitterGraphDescription.AppendActionXmlNode(oActions, "tweet", BuildTweetUrl(oVertexData[sItem], sItem));
                    this._twitterGraphDescription.AppendActionXmlNode(oActions, "follow", BuildFollowUrl(sItem));
                    this._twitterGraphDescription.AppendActionXmlNode(oActions, "profile", BuildProfileUrl(sItem));
                }
                else
                {
                    oSection = this._twitterGraphDescription.AppendSectionXmlNode(oSections, "TopNByMetrics", sItem);
                }
            }
        }

        private void
        AddTopMetrics
        (
            XmlNode oSections
        )
        {
            String sTwitterSearchNetworkTopItems;

            (new TwitterSearchNetworkTopItemsReader()).TryReadMetrics(
                this._workbook, out sTwitterSearchNetworkTopItems);

            String[] oItems = sTwitterSearchNetworkTopItems.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for(Int32 i=0;i<oItems.Length;i++){
                if(oItems[i].StartsWith("Top ")){
                    //It is a new section
                    AddTopMetricSection(oSections, oItems, i);
                }
            }
        }

        private void
        AddTopMetricSection
        (
            XmlNode sections,
            String[] items,
            Int32 sectionIndex
        )
        {
            String sectionValue = items[sectionIndex].Replace(" ", "");
            XmlNode oSection = this._twitterGraphDescription.AppendSectionXmlNode(sections,
                                         sectionValue, items[sectionIndex]);

            for (Int32 i = sectionIndex + 1; i < items.Length && !items[i].StartsWith("Top "); i++)
            {
                XmlNode oElement = this._twitterGraphDescription.AppendValueElementXmlNode(oSection, items[i]);
                XmlNode oActions = this._twitterGraphDescription.AppendActionsXmlNode(oElement);
                //Split item
                String[] parts = items[i].Split('\t');
                AddTopMetricsAction(oActions, items[sectionIndex], parts[1]);
                
            }
        }

        private void
        AddTopMetricsAction
        (
            XmlNode actions,
            String sectionHeader,
            String item
        )
        {
            if (sectionHeader.Contains(" URLs "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "hyperlink", item);
            }
            else if (sectionHeader.Contains(" Domains "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "hyperlink", "http://"+item);
            }
            else if (sectionHeader.Contains(" Hashtags "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _searchURL + "%23"+ item);
            }
            else if (sectionHeader.Contains(" Words "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _searchURL + item);
            }
            else if (sectionHeader.Contains(" Word Pairs "))
            {
                String[] parts = item.Split(',');
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _searchURL + parts[0] +"%20AND%20"+parts[1]);
            }
            else if (sectionHeader.Contains(" Replied-To "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _followURL + item);
            }
            else if (sectionHeader.Contains(" Mentioned "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _followURL + item);
            }
            else if (sectionHeader.Contains(" Tweeters "))
            {
                this._twitterGraphDescription.AppendActionXmlNode(actions, "search", _followURL + item);
            }
        }
        

        private Dictionary<String, Dictionary<String, String>>
        GetDataFromVertexName
        (
            String[] oVertices
        )
        {

            ListObject oVertexTable;
            Dictionary<String, Dictionary<String, String>> oVertexData = 
                new Dictionary<String, Dictionary<String,String>>();

            if (ExcelTableUtil.TryGetTable(this._workbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oVertexTable))
            {
                ExcelHiddenColumns oHiddenColumns =
                ExcelColumnHider.ShowHiddenColumns(oVertexTable);

                ExcelTableReader oExcelTableReader =
                        new ExcelTableReader(oVertexTable);

                foreach (ExcelTableReader.ExcelTableRow oRow in
                                    oExcelTableReader.GetRows())
                {
                    String sVertexName;

                    if (oVertexData.Count == oVertices.Length)
                    {
                        break;
                    }

                    if (oRow.TryGetNonEmptyStringFromCell(VertexTableColumnNames.VertexName,
                        out sVertexName))
                    {                        

                        if (oVertices.Contains(sVertexName))
                        {
                            Dictionary<String, String> oData = new Dictionary<String, String>();
                            String sImageUri, sTopHashtags, sTopWords;                            
                            if (oRow.TryGetNonEmptyStringFromCell(VertexTableColumnNames.ImageUri,
                                out sImageUri))
                            {
                                oData.Add(_imageUriKey, sImageUri);                                
                            }

                            if (oRow.TryGetNonEmptyStringFromCell(VertexTableColumnNames.TopHashtagsInTweetBySalience,
                                out sTopHashtags))
                            {
                                oData.Add(_topHashtagsKey, String.Join(" #", sTopHashtags.Split(' ').Take(2)));
                            }

                            if (oRow.TryGetNonEmptyStringFromCell(VertexTableColumnNames.TopWordsInTweetBySalience,
                                out sTopWords))
                            {
                                oData.Add(_topWordsKey, String.Join(" ", sTopWords.Split(' ').Take(2)));
                            }

                            oVertexData.Add(sVertexName, oData);
                        }
                    }
                }

                ExcelColumnHider.RestoreHiddenColumns(oVertexTable,
                    oHiddenColumns);
            }

            return oVertexData;
        }

        //*************************************************************************
        //  Method: AppendGraphHistoryValues()
        //
        /// <summary>
        /// Appends graph history values to the graph history if the values are
        /// available.
        /// </summary>
        ///
        /// <param name="oGraphHistory">
        /// Stores attributes that describe how the graph was created.
        /// </param>
        ///
        /// <param name="oStringBuilder">
        /// Object used to build the graph history.
        /// </param>
        ///
        /// <param name="asGraphHistoryKeys">
        /// Array of the keys for the values to append.
        /// </param>
        //*************************************************************************

        private void
        AppendGraphHistoryValues
        (            
            StringBuilder oStringBuilder,
            params String[] asGraphHistoryKeys
        )
        {
            Debug.Assert(this._perWorkbookSettings.GraphHistory != null);
            Debug.Assert(oStringBuilder != null);
            Debug.Assert(asGraphHistoryKeys != null);

            String sGraphHistoryValue;

            foreach (String sGraphHistoryKey in asGraphHistoryKeys)
            {
                if (this._perWorkbookSettings.GraphHistory.TryGetValue(sGraphHistoryKey,
                    out sGraphHistoryValue))
                {
                    StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
                        sGraphHistoryValue);
                }
            }
        }

        private  String
        BuildTweetUrl
        (
            Dictionary<String, String> userData,
            String userName
        )
        {
            String url = _tweetURL + "@" + userName;

            if (userData.ContainsKey(_topHashtagsKey))
            {
                url += " #" + userData[_topHashtagsKey];
            }
            if (userData.ContainsKey(_topWordsKey))
            {
                url += " " + userData[_topWordsKey];
            }

            ExportDataUserSettings oExportDataUserSettings =
                new ExportDataUserSettings();

            url += " " + oExportDataUserSettings.Hashtag;
            url += " " + oExportDataUserSettings.URL;

            return (url);
        }

        private String
        BuildFollowUrl
        (
            String userName
        )
        {
            return (String.Format("{0}{1}", _followURL, userName));
        }

        private String
        BuildProfileUrl
        (
            String userName
        )
        {
            return (String.Format("{0}{1}", _followURL, userName));
        }

        //*************************************************************************
        //  Method: NullableToOverallMetricValue()
        //
        /// <summary>
        /// Converts a Nullable overall metric to a string value to be used in a
        /// graph summary.
        /// </summary>
        ///
        /// <param name="oNullable">
        /// The Nullable to convert.
        /// </param>
        ///
        /// <returns>
        /// If the Nullable has a value, the value in string form is returned.
        /// Otherwise, a "not applicable" string is returned.
        /// </returns>
        //*************************************************************************

        private String
        NullableToOverallMetricValue<T>
        (
            Nullable<T> oNullable
        )
        where T : struct
        {
            if (oNullable.HasValue)
            {
                return (oNullable.Value.ToString());
            }

            return (GraphMetricCalculatorBase.NotApplicableMessage);
        }

        

        private Microsoft.Office.Interop.Excel.Workbook _workbook;
        private PerWorkbookSettings _perWorkbookSettings;
        private TwitterGraphDescription _twitterGraphDescription;

        private static readonly String _followURL = "https://twitter.com/";

        private static readonly String _tweetURL = "https://twitter.com/intent/tweet?text=";

        private static readonly String _searchURL = "https://twitter.com/search/?q=";

        private static readonly String _imageUriKey = "ImageUri";

        private static readonly String _topHashtagsKey = "TopHashtags";

        private static readonly String _topWordsKey = "TopWords";
    }
}
