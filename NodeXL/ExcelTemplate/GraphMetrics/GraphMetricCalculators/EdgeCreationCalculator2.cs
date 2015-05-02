using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using Microsoft.Office.Interop.Excel;

namespace Smrf.NodeXL.ExcelTemplate
{

    //*****************************************************************************
    //  Class: EdgeCreationCalculator2
    //
    /// <summary>
    /// Create edges based on the similarity of the content used by two vertices.
    /// </summary>
    ///
    /// <remarks>
    /// This graph metric calculator differs from most other calculators in that it
    /// reads an arbitrary column in the Excel workbook.  The other calculators
    /// look only at how the graph's vertices are connected to each other.
    /// Therefore, there is no corresponding lower-level EdgeCreationCalculator class
    /// in the <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the Edge Creation
    /// metrics cannot be calculated outside of this ExcelTemplate project.
    /// 
    /// This class is not able to use GraphMetricColumnOrdered, GraphMetricColumnWithID
    /// to insert columns because the new edges did not exist in the table
    /// (i.e. no RowID for reference).
    /// This class uses the "workbook" parameter and ExcelTableUtil method 
    /// to delete and add rows into the Edges table.
    /// 
    /// This class uses "word" to describe shared content. It uses 
    /// <see cref="CalculateSimilarity" /> to calculate the similarity between 
    /// specified text column used by two vertices. 
    /// </remarks>
    /// 
    //*****************************************************************************   
    public class EdgeCreationCalculator2 : GraphMetricCalculatorBase2
    {
        //*************************************************************************
        //  Constructor: EdgeCreationCalculator2()
        //
        /// <summary>
        /// Create edges based on the similarity of the content used by two vertices.
        /// </summary>
        /// 
        /// <param name="workbook">
        /// This class directly deletes and adds rows into the Edges table.
        /// It needs the workbook parameter to get and set the table values.
        /// </param>
        /// 
        /// <remarks>
        /// This class calculates similarity between content in text column on vertices' table . 
        /// The text column is assigned by the user. User can also assign a threshold.
        /// Similarity = # shared words / MAX [# words in Vertex1's Text, # words in Vertex2's Text]
        /// </remarks>
        /// 
        /// 
        //*************************************************************************
        public EdgeCreationCalculator2
        (
            Microsoft.Office.Interop.Excel.Workbook workbook
        )
        {
            oWorkbook = workbook;
            AssertValid();
        }

        //*************************************************************************
        //  Property: HandlesDuplicateEdges
        //
        /// <summary>
        /// Gets a flag indicating whether duplicate edges are properly handled.
        /// </summary>
        ///
        /// <value>
        /// true if the graph metric calculator handles duplicate edges, false if
        /// duplicate edges should be removed from the graph before the
        /// calculator's <see cref="TryCalculateGraphMetrics" /> method is called.
        /// </value>
        //*************************************************************************
        public override Boolean
        HandlesDuplicateEdges
        {
            get
            {
                AssertValid();

                //Remove Duplicate edges if graph is not directed.

                return (false);
            }
        }

        //*************************************************************************
        //  Method: TryCalculateGraphMetrics()
        //
        /// <summary>
        /// Attempts to calculate a set of one or more related metrics.
        /// </summary>
        ///
        /// <param name="graph">
        /// The graph to calculate metrics for.  The graph may contain duplicate
        /// edges and self-loops.
        /// </param>
        ///
        /// <param name="calculateGraphMetricsContext">
        /// Provides access to objects needed for calculating graph metrics.
        /// </param>
        ///
        /// <param name="graphMetricColumns">
        /// Where an array of GraphMetricColumn objects gets stored if true is
        /// returned, one for each related metric calculated by this method.
        /// </param>
        ///
        /// <returns>
        /// true if the graph metrics were calculated or don't need to be
        /// calculated, false if the user wants to cancel.
        /// </returns>
        ///
        /// <remarks>
        /// This method periodically checks BackgroundWorker.<see
        /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
        /// immediately returns false.
        ///
        /// <para>
        /// It also periodically reports progress by calling the
        /// BackgroundWorker.<see
        /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
        /// userState argument is a <see cref="GraphMetricProgress" /> object.
        /// </para>
        ///
        /// <para>
        /// Calculated metrics for hidden rows are ignored by the caller, because
        /// Excel misbehaves when values are written to hidden cells.
        /// </para>
        ///
        /// </remarks>
        //*************************************************************************
        public override Boolean
        TryCalculateGraphMetrics
        (
            IGraph graph,
            CalculateGraphMetricsContext calculateGraphMetricsContext,
            out GraphMetricColumn[] graphMetricColumns
        )
        {
            Debug.Assert(graph != null);
            Debug.Assert(calculateGraphMetricsContext != null);
            AssertValid();

            graphMetricColumns = new GraphMetricColumn[0];
            
            EdgeCreationUserSettings oEdgeCreationUserSettings =
                calculateGraphMetricsContext.GraphMetricUserSettings
                .EdgeCreationUserSettings;

            if (
                !calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
                    GraphMetrics.EdgeCreation)
                ||
                String.IsNullOrEmpty(oEdgeCreationUserSettings.TextColumnName)
                )
            {
                return (true);
            }

            //User input variables
            double dThreshold = oEdgeCreationUserSettings.Threshold / 100.000;
            int iEdgeLimit = oEdgeCreationUserSettings.EdgeLimit;
            string sTextColumnName = oEdgeCreationUserSettings.TextColumnName;
            Boolean bLimitToIsolate = oEdgeCreationUserSettings.LimitToIsolate;

            //Declare pair of Vertices 
            IVertexCollection cFirstVertexCollection = graph.Vertices;
            IVertexCollection cSecondVertexCollection = graph.Vertices;
            
            //Graph's directness
            Boolean bGraphIsDirected =
            (graph.Directedness == GraphDirectedness.Directed);

            //Count #Edges been added
            Int32 iNewEdgesCount = 0;

            //Objects for Workbook
            ListObject oTable;
            ListColumn oColumn;
            

            //Need to Remove Edges with Relationship = Shared Content: sTextColumnName
            //first, so this won't create duplicate edges if users run this metric more than once
            ICollection<String> valuesToRemove = new String[]{"Shared Content: "+sTextColumnName};
            
            if (ExcelTableUtil.TryGetTable(oWorkbook,
                    WorksheetNames.Edges, TableNames.Edges, out oTable))
            {
                ExcelUtil.ActivateWorksheet(oTable);

                // Add a Shared Content column to the Edges worksheet.
                ExcelTableUtil.TryGetOrAddTableColumn(oTable,
                    EdgeTableColumnNames.SharedContent, ExcelTableUtil.AutoColumnWidth,
                    null, out oColumn);
                
                //Clear duplicate Shared Content Edges
                //i.e. Edges with Relationship = Shared Content: sTextColumnName
                ExcelTableUtil.RemoveTableRowsByStringColumnValues(oWorkbook,
                    WorksheetNames.Edges, TableNames.Edges,
                    "Relationship", valuesToRemove);
            }


            //Compare each pair of vertices, calculate similarity, and create new edges
            foreach (IVertex oFirstVertex in cFirstVertexCollection)
            {
                if(TryCreateEdges(oFirstVertex,bLimitToIsolate))
                {

                    foreach (IVertex oSecondVertex in cSecondVertexCollection)
                    {
                        String sFirstVertexName = oFirstVertex.Name;
                        String sSecondVertexName=oSecondVertex.Name;
                   
                        if(sFirstVertexName!=sSecondVertexName)
                        {
                            //Get text in sTextColumnName
                            Object oFirstTextAsObject;
                            Object oSecondTextAsObject;
                            if (oFirstVertex.TryGetValue(sTextColumnName, typeof(String), out oFirstTextAsObject) &&
                                oSecondVertex.TryGetValue(sTextColumnName, typeof(String), out oSecondTextAsObject))
                            {
                                String sFirstText = (String)oFirstTextAsObject;
                                String sSecondText = (String)oSecondTextAsObject;
                                
                                //Calcaulte similarity
                                String sSimilarityOutput = CalculateSimilarity(sFirstText, sSecondText);
                                String[] saSimilarityOutputArray=sSimilarityOutput.Split(',');
                                double dSimilarity = Convert.ToDouble(saSimilarityOutputArray[0]);
                                
                                //Add new edges if similarity is equal or more than threshold
                                if(dSimilarity>=dThreshold)
                                {
                                    //If more than iEdgeLimit edges been added, don't add new edges 
                                    if (iNewEdgesCount <= iEdgeLimit) 
                                    {
                                        String sNewEdgeOutput= sFirstVertexName + "," + sSecondVertexName + "," + sSimilarityOutput;
                                        
                                        IEdge oNewEdge = new Edge(oFirstVertex, oSecondVertex, bGraphIsDirected);
                                        oNewEdge.SetValue(ReservedMetadataKeys.EdgeWeight, dSimilarity);

                                        if (!graph.Edges.Contains(oNewEdge))
                                        {
                                            //Add Edge to the graph
                                            oNewEdge = graph.Edges.Add(oFirstVertex, oSecondVertex, bGraphIsDirected);
                                            
                                            String[] saSimilarityOutput = sSimilarityOutput.Split(',');
                                            
                                            //Add new edge to the Edges table
                                            ExcelTableUtil.AddTableRow(oTable,
                                                EdgeTableColumnNames.Vertex1Name, sFirstVertexName,
                                                EdgeTableColumnNames.Vertex2Name,sSecondVertexName,
                                                EdgeTableColumnNames.SharedContent,saSimilarityOutput[1],
                                                "Relationship", "Shared Content: " + sTextColumnName,
                                                "Edge Weight", saSimilarityOutput[0]);
                                            
                                            iNewEdgesCount++;                                        
                                        } 
                                    }
                                }
                            }
                        }                    
                    }//end foreach

                }
                 
            }
            
            return (true);
        }

        //*************************************************************************
        //  Method: TryCreateEdges()
        //
        /// <summary>
        /// Check if the program should create new edges for the vertex.
        /// 
        /// If Limit to Isolate is NOT chekced, return true.
        /// 
        /// If Limit to Isolate is checked, check if the vertex 
        /// has only one edge and the edge is a self-loop edge; if yes, return true.
        /// 
        /// Else, return false.
        /// 
        /// </summary>
        /// <param name="oVertex"></param>
        /// <param name="bLimitToIsolate"></param>
        /// <returns></returns>
        //*************************************************************************
        protected Boolean
        TryCreateEdges
        (
            IVertex oVertex,
            Boolean bLimitToIsolate
        )
        {
            if (!bLimitToIsolate)
            {
                return true;
            }
            else {
                IEdge[] edgeCollection = oVertex.IncidentEdges.ToArray();
                //ICollection<IEdge> edgeCollection = oVertex.IncidentEdges;
                if (edgeCollection.Length==1)
                {
                    if (edgeCollection[0].IsSelfLoop)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }   
            }                     
        }

        //*************************************************************************
        //  Method: CalculateSimilarity()
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFirstVertexText">
        /// The string of text from the text column of the first vertex
        /// </param>
        /// <param name="sSecondVertexText">
        /// The string of text from the text column of the second vertex
        /// </param>
        /// <returns></returns>
        //*************************************************************************
        protected String
        CalculateSimilarity
        (
            String sFirstVertexText,
            String sSecondVertexText
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(sFirstVertexText));
            Debug.Assert(!String.IsNullOrEmpty(sSecondVertexText));

            double dSimilarity=0;
            String sSharedContent="";
            Int32 iFirstVertexWordCount=0;
            Int32 iSecondVertexWordCount=0;
            String sSharedOutput="";

            if(!String.IsNullOrEmpty(sFirstVertexText)&&!String.IsNullOrEmpty(sSecondVertexText))
            {
                String[] asFirstVertexWords = StringUtil.SplitOnCommonDelimiters(sFirstVertexText);
                String[] asSecondVertexWords = StringUtil.SplitOnCommonDelimiters(sSecondVertexText);
                iFirstVertexWordCount = asFirstVertexWords.Length;
                iSecondVertexWordCount = asSecondVertexWords.Length;

                //Compare each word in the text string
                foreach (String sFirstVertexWord in asFirstVertexWords)
                {
                    if (!String.IsNullOrEmpty(sFirstVertexWord)) 
                    {
                        foreach (String sSecondVertexWord in asSecondVertexWords)
                        {
                            if (sFirstVertexWord == sSecondVertexWord)
                            {
                                sSharedContent = sSharedContent + sFirstVertexWord + " ";
                            }
                        }                    
                    }
                }
                sSharedContent = sSharedContent.Trim();

                //Calcaulte similarity
                if (!String.IsNullOrEmpty(sSharedContent))
                {
                    String[] saSharedContent = StringUtil.SplitOnSpaces(sSharedContent);

                    //Similarity = # Shared Words / MAX [# Words in FirstVertexText, # Words in SecondVertexText]
                    if (iFirstVertexWordCount >= iSecondVertexWordCount)
                    {                       
                        dSimilarity = Math.Round((((double)saSharedContent.Length / iFirstVertexWordCount)), 3);
                    }
                    else
                    {
                        dSimilarity = Math.Round(((double)saSharedContent.Length / iSecondVertexWordCount), 3);
                    }
                }
                sSharedOutput = dSimilarity.ToString() + "," + sSharedContent;
            }

            return sSharedOutput;
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
        //  Protected fields
        //*************************************************************************

        /// <summary>
        /// Workbook
        /// </summary>
        protected Microsoft.Office.Interop.Excel.Workbook oWorkbook;
    }

    
}
