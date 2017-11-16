# Complete NodeXL Release History, Page 2

 [<< Go to Page 1](CompleteReleaseHistory)

+1.0.1.250+  (2013-08-19)
* Checking "Expand URLs in tweets" in the Import from NodeXL Graph Server dialog box no longer slows the import to a crawl.  It can still take a long time to get the network if it has thousands of edges and vertices, but expanding URLs is now much faster.  (This is a server-side change, so you don't actually need version 1.0.1.250 to notice the difference.)
* Bug fix: When it looked for user names in tweets, NodeXL was treating underscores as discardable punctuation.  This resulted in @john_doe being interpreted as @john.

+1.0.1.249+  (2013-08-14)
* Bug fix: If your workbook had a file name that contained certain non-English characters (Korean characters, for example) and you attempted to export the workbook via NodeXL, Data, Export, To Email, you would get a message saying "A problem occurred while sending the email."

+1.0.1.248+  (2013-08-09)
* If you import a network from the NodeXL Graph Server, the graph summary (NodeXL, Graph, Summary) now includes more information, including the date range of the tweets in the network.
* The Data Import section of the Graph Summary for all Twitter, YouTube and Flickr network is now broken into paragraphs for clarity.
* When it looks for user names in tweets, NodeXL now ignores most punctuation.  For example, the user name "john" in the tweet "Hello,@john" wasn't being detected, because it was preceded by a comma instead of a space.
* NodeXL will now pause for an extra 15 seconds when Twitter rate limits are reached.  This is an attempt to work around a problem where Twitter occasionally refuses to provide more information even after NodeXL pauses for the time specified by Twitter.  The symptom of that problem is a message that includes the text "A likely cause is that you have made too many Twitter requests in the last 15 minutes."  (This might not fix the problem, the cause of which is unknown.)

+1.0.1.247+  (2013-08-02)
* If you import a Twitter search network and tell NodeXL to calculate Twitter search network top items (NodeXL, Analysis, Graph Metrics), then the following columns now get added to the Vertices worksheet: Top URLs in Tweet by Count, Top URLs in Tweet by Salience, Top Domains in Tweet by Count, Top Domains in Tweet by Salience, Top Hashtags in Tweet by Count, Top Hashtags in Tweet by Salience, Top Words in Tweet by Count, Top Words in Tweet by Salience, Top Word Pairs in Tweet by Count, Top Word Pairs in Tweet by Salience.
* When importing a Twitter network, if Twitter unexpectedly closes the network connection, then NodeXL will now try to reestablish the connection.  (The symptom of this problem is a message that includes the text "{"[IOException](IOException): Unable to read data from the transport connection: The connection was closed.""})

+1.0.1.246+  (2013-07-19)
* An importer for a new Graph Server has been added.  The Graph Server is currently private.
* Bug fix: If you used NodeXL, Analysis, Subgraph Images and one of your vertices had a name that was "reserved" by Windows (con, lpt, nul, and several others), you would get an error message that included the text {""[ExternalException](ExternalException): A generic error occurred in GDI+""}.  Note that subgraph image file names now always start with "Img-", as in "Img-con.png".

+1.0.1.245+  (2013-06-19)
* This release wraps up a set of enhancements and bug fixes that were previously included in a series of experimental private releases.  All changes since the last public release (1.0.1.238) have to do with importing Twitter networks.
* An experiment involving "shared term" edges in the Twitter search network is still in progress, and those edges are not included in this release.  (They were not included in version 1.0.1.244, either.)
* This release is functionally identical to version 1.0.1.244.

+1.0.1.244+  (2013-06-17, private release)
* In the Twitter search network, vertices are now added to the graph for people who were replied to or mentioned by the people who actually tweeted the specified term.
* Bug fix: In the Twitter networks, if you checked "Expand URLs in tweets" and a malformed URL was provided by Twitter, you would get a "Partial Network" message and then the graph would have no edges.  The Partial Network details included the text {""[UriFormatException](UriFormatException):"} Invalid URI: The format of the URI could not be determined".  Now, NodeXL doesn't attempt to expand the bad URL.
* Bug fix: When getting a Twitter search network, you would sometimes get a "Partial Network" message with details that included the text {""[KeyNotFoundException](KeyNotFoundException): The given key was not present in the dictionary.""}  (This was caused by Twitter giving NodeXL a tweet that included no information about the tweeter.  NodeXL now ignores such tweets.)

+1.0.1.243+  (2013-05-19, private release)
* The Twitter Search Network now uses experimental shared term thresholds.  This is part of an effort to obtain meaningful edges without running into Twitter's new rate limits. 

+1.0.1.242+  (2013-05-01, private release)
* The shared word user threshold can now be specified in the network configuration file used by the Network Server program.  (For advanced users of private releases only.)
* Bug fix: If you specified shared hashtag, URL, word, or word pair edge relationships in the Import from Twitter Search Network dialog box, the graph summary (NodeXL, Graph, Summary) did not mention the fact that the graph included those edges.  (Private releases only.)

+1.0.1.241+  (2013-04-25, private release)
* Bug fix: If you imported a Twitter network that included the empty hyperlink "http://" somewhere in the network's text, Excel would refuse to save the workbook.  The error message started with "Errors were detected while saving..."  This is an Excel bug that will also arise if you type "http://" directly into a cell.  This change does _not_ fix the latter problem.

+1.0.1.240+  (2013-04-24, private release)
* If you check "add statistic columns to the Vertices worksheet" in the Import from Twitter Search Network dialog box, no additional time will be required to get the network.  This used to require additional Twitter requests, and it was marked as "slower" in the dialog box.

+1.0.1.239+  (2013-04-08, private release)
* Same as 1.0.1.238, but with additional edge options.

+1.0.1.238+  (2013-04-08)
* Bug fix: In versions 1.0.1.236 and 237, if you entered a space (or a colon, or a few other special characters) into the "Add a vertex for each..." textbox on the Import from Twitter Search Network dialog box, you would get a message that included the text, "The Twitter Web service refused to provide the requested information."

+1.0.1.237+  (2013-03-25, private release)
* Same as 1.0.1.236, but with additional edge options.

+1.0.1.236+  (2013-03-25)
* There are some major changes in NodeXL's Twitter importers.  See [Changes in NodeXL's Twitter Importers](http://nodexl.codeplex.com/discussions/437841) for more information.
* When you use dynamic filters (NodeXL, Analysis, Dynamic Filters), filtering an edge will automatically filter its adjacent vertices if the filtering causes those vertices to become isolates.
* If you set the Visibility of vertex X to "Skip" and vertex X has adjacent vertices that are connected only to vertex X, then those adjacent vertices are now skipped as well.  Previously, the adjacent vertices would remain in the graph as isolates.
* For programmers: The NodeXL source code is now available on the CodePlex "Source Code" tab.  The source code will no longer be provided as separate releases on the Downloads tab.

+1.0.1.234+  (2013-01-25, private release)
* There are some new edge options in the Import from Twitter Search Network dialog box.

+1.0.1.233+  (2013-01-19, private release)
* In the Export to Email dialog box, (NodeXL, Data, Export, To Email), there is a new button for inserting a sample message.
* When you import a set of GraphML files into a set of new NodeXL workbooks (NodeXL, Data, Import, From GraphML Files), a bad GraphML file will no longer stop NodeXL in its tracks.  Any bad GraphML files are now reported after all the good GraphML files are imported.
* There are some new edge options in the Import from Twitter Search Network dialog box.
* Bug fix: If you entered an Image File value on the Vertices worksheet that included an invalid character, such as a less-than sign, you would get an error message that included the text "{"[ArgumentException](ArgumentException): Illegal characters in path"}".  Now there will be no error message, and instead an image of a red X will be shown in the graph pane.
* Bug fix: If you automated a workbook (NodeXL, Graph, Automate), checked "save workbook to a new file if it has never been saved," and the specified folder to save the workbook to didn't exist, you would get an error message that included the text "{"[COMException](COMException): Microsoft Office Excel cannot access the file..."}".  Now you get a message telling you how to fix the problem.
* Bug fix, for programmers only: If you colored vertices by using the System.Windows.Media.Color type for ReservedMetadataKeys.PerColor values, you created groups, and then you collapsed a group, you would get an ArgumentException with the message "The value with the key "~PDColor" is of type System.Windows.Media.Color.  The expected type is System.Drawing.Color."  This did not occur if you used the System.Drawing.Color type.

+1.0.1.230+  (2012-12-07)
* If you import a Twitter search network (NodeXL, Data, Import, From Twitter Search Network), the graph summary (NodeXL, Graph, Summary) now includes the actual number of Twitter users in the network, not just the maximum number.
* If you autofill the group label column (NodeXL, Visual Properties, Autofill Columns), you can now tell NodeXL to prepend the group name to the group label.

+1.0.1.229+  (2012-11-25)
* The maximum Size value in the Vertices worksheet has been increased from 100 to 1,000, so NodeXL will make the vertices in the graph pane much larger if you tell it to.  
* When you use what is commonly called "group in a box" (NodeXL, Graph, Layout, Layout Options, "Lay out each of the graph's groups in its own box), the boxes now have less contrast with the graph background, so they don't stand out as much.
* If you export the graph to email (NodeXL, Data, Export, To Email), you can now include HTML markup in the email's message, and you can place a graph summary or an image of the graph anywhere in the message.

+1.0.1.228+  (2012-11-19, private release)
* NodeXL will now group the graph's vertices by _motif_.  Motifs are a new way to reduce the visual complexity of your graph. With motif simplification, common repeating network motifs are replaced with easily understandable glyphs that require less space, are easier to understand, and reveal hidden relationships.  Go to NodeXL, Analysis, Groups, Group by Motif.  (This work was done by Cody Dunne and Ben Shneiderman at the University of Maryland.)
* Bug fix: If you used the NodeXL Network Server program to import a Twitter network, the graph summary did not include any data import details.  The data import details are now _always_ included when the NodeXL Network Server program is used.

+1.0.1.227+  (2012-11-13, private release)
* The way the Size column on the Vertices worksheet affects the actual size of Image vertices in the graph pane has been changed.  The range of actual Image vertex sizes was too large before; now it has been compressed into something more sensible.
* If you automate the graph (NodeXL, Graph, Automate), there is a new option for automatically exporting the graph to the [NodeXL Graph Gallery](http://www.nodexlgraphgallery.org).
* If you automate the graph, there is a new option for automatically exporting the graph to email.
* If you export the graph to email, the email is now in HTML format and includes an embedded image of the graph.
* You can now enter a longer description when exporting a graph to the NodeXL Graph Gallery or to email.
* The order of items in the graph summary (NodeXL, Graph, Summary) has been changed: The "Groups" item now comes immediately after the "Graph type" item.
* The order of items in the Twitter Search Network Top Items worksheet (and in the graph summary) has been changed: The "Top Replied-To" and "Top Mentioned" items now come immediately before the "Top Tweeters."
* Each column in the Twitter Search Network Top Items worksheet is now only as wide as the longest column name in the column.  The column used to be as wide as the longest _cell contents_ in the column, which made the column too wide when the cells contained URLs, for example.
* Bug fix: If you entered an invalid email address in the Export to Email dialog box, you would get an ugly error message that included the text "FormatException."  You now get a message that makes sense.
* For programmers: Several benign build warnings involving XML documentation have been fixed.  (These warnings showed up only in Visual Studio 2010, not 2008.)

+1.0.1.226+  (2012-10-16)
* You can now email your graph directly from NodeXL.  Your email can include the NodeXL workbook and the graph data as GraphML.  Go to NodeXL, Data, Export, To Email.
* If you export a large graph to the NodeXL Graph Gallery (NodeXL, Data, Export, To NodeXL Graph Gallery) and you include the graph data as GraphML, the export is now much faster.  (The GraphML is now zipped before it is exported).
* You can now export your graph to a GEXF file.  (GEXF is a file format used by the [Gephi](http://gephi.org/) program.)  Go to NodeXL, Data, Export, To GEXF file in the Excel ribbon.
* The "Import from YouTube Video Network" feature (NodeXL, Data, Import) no longer offers an "add an edge for each pair of videos tagged with the same keyword" option, because YouTube will no longer provide programs like NodeXL with keyword information.  It has been replaced with an "add an edge for each pair of videos that have the same _category_" option.
* The "Import from YouTube User's Network" feature no longer offers an "add a vertex for each friend of the user" option.  YouTube will no longer provide this information.
* If you group the graph's vertices by motif, you can now group by clique motifs in addition to fan and D-parallel motifs.  (This work was done by Cody Dunne at the University of Maryland.)
* If you use the Network Server program to import networks into GraphML files, and you then import the GraphML files into NodeXL workbooks using NodeXL, Data, Import, From GraphML File or From GraphML Files, the data import details ("The graph represents a network of up to 100 Twitter users...") are now preserved.
* For programmers: The NodeXL source code is now available on the CodePlex "Source Code" tab.  The source code will no longer be provided as separate releases on the Downloads tab.

+1.0.1.225+  (2012-09-02, private release)
* Bug fix: When opening an older NodeXL workbook with a newer version of NodeXL, it was possible to get the message "Invalid AutomationTasks flags."

+1.0.1.224+  (2012-08-31)
* The way the Size column on the Vertices worksheet affects the actual size of vertices in the graph pane has been changed.  The Size values now determine the _area_ of the vertices instead of the radius of the vertices, and the range of actual vertex sizes has been compressed into something more sensible.
* If you import a Twitter search network (NodeXL, Data, Import, From Twitter Search Network), your graph has groups, and you calculate Twitter search network top items (NodeXL, Analysis, Graph Metrics), the following additional columns now get added to the Groups worksheet: Top Replied-To in Tweet, Top Mentioned in Tweet, Top URLs in Tweet, Top Domains in Tweet, Top Words in Tweet, Top Word Pairs in Tweet, and Top Tweeters.
* If your graph has groups, you tell NodeXL to lay out the groups in boxes (NodeXL, Graph, Layout, Layout Options), and you combine intergroup edges, the maximum width of the combined intergroup edges is now three times what it was before.
* If you calculate word and word pair metrics (NodeXL, Analysis, Graph Metrics), the order of the word pairs is now considered, so that "brown fox" and "fox brown" are counted as separate word pairs.  They used to be counted as one pair.
* If you calculate word and word pair metrics, NodeXL now calculates something called "mutual information" for the word pairs.  It's described in the Graph Metrics dialog box.

+1.0.1.223+  (2012-08-23, private release)
* If you import a Twitter, Flickr or YouTube network (NodeXL, Data, Import) and then automate the workbook (NodeXL, Graph, Automate), and you tell NodeXL to save the workbook if it has never been saved before, the file name NodeXL uses now contains more information about the import: "2012-08-15 11-29-48 NodeXL Twitter Search London Olympics.xlsx", for example.
* If you import a Twitter search network and you have opted to have NodeXL add a description of the imported data to the graph summary (NodeXL, Data, Import, Import Options), then the graph summary will now state the tweet period with greater precision: "The tweets were made over the 9-hour, 12-minute period from...", for example.
* If you import a Twitter network and you tell NodeXL to add a Tweet or Latest Tweet column, a new Domains in Tweet or Domains in Latest Tweet column also gets added.
* If you tell NodeXL to calculate Twitter search network top item metrics (NodeXL, Analysis, Graph Metrics), the Twitter Search Network Top Items worksheet will now include the top domains in the tweets.
* If you import a Twitter network and you tell NodeXL to expand the URLs in the tweets or latest tweets, the URLs are now expanded twice if necessary--once to expand the "http://t.co/..." URL, and again if that expanded URL is itself a shortened URL, such as "http://bit.ly/...". This significantly increases the import time.  {"[This change was a mistake and was reversed in version 1.0.1.224.](This-change-was-a-mistake-and-was-reversed-in-version-1.0.1.224.)"}
* For programmers only: The Smrf.NodeXL.Algorithms.ClusterCalculator class will now calculate clusters by clique, via the SNAP library.

+1.0.1.221+  (2012-08-13, private release)
* If you automate the workbook (NodeXL, Graph, Automate), there is a new option in the Automate dialog box to have NodeXL save the workbook if it has never been saved before.  The file name includes import information if you imported a network from Twitter, Flickr or YouTube, or just the date and time otherwise.
* You can now group vertices by D-parallel motifs, not just two-parallel motifs.
* Collapsed D-parallel motifs now look like tapered diamonds instead of crescents.

+1.0.1.220+  (2012-08-01, private release)
* If you export many graph images to the NodeXL Graph Gallery (NodeXL, Data, Export, To NodeXL Graph Gallery), you can now specify that all your exported images be the same size.
* When you calculate word and word pair metrics (NodeXL, Analysis, Graph Metrics) and you select the "count by group" option, the results now include metrics for the entire graph as well as for each group.
* When you calculate Twitter search network top item metrics, the results now include the top words and top word pairs in the tweets.
* For programmers using the NodeXL Class Libraries: You can now control the alpha (opacity) of the rectangles that are drawn behind label annotations to make them more legible.  See the new NodeXLControl.GraphDrawer.VertexDrawer.BackgroundAlpha property.  (You can set this to zero as a temporary workaround for the .NET Framework version problem discussed at http://nodexl.codeplex.com/discussions/389327.)

+1.0.1.219+  (2012-07-25)
* The Graph Metrics dialog box (NodeXL, Analysis, Graph Metrics) has been redesigned to make it easier to see the description of each metric, and to make it clear that some metrics are valid only for directed or undirected graphs.
* When it calculates overall metrics, NodeXL now includes a vertex reciprocation metric and an edge reciprocation metric for the graph as a whole.  The two new metrics are explained in the Graph Metrics dialog box.
* Also in the Graph Metrics dialog box, you can now tell NodeXL to calculate a reciprocation metric for each of the graph's vertices.
* You now have more flexibility when dealing with duplicate edges.  You can tell NodeXL to  count duplicate edges, merge duplicate edges, or both; and you can specify a third column to use to determine whether two edges are duplicates--the Relationship column in a Twitter network, for example.  Go to NodeXL, Data, Prepare Data, Count and Merge Duplicate Edges.
* If you import a YouTube video network (NodeXL, Data, Import, From YouTube Video Network), the videos' authors are now included on the Vertices worksheet.

+1.0.1.218+  (2012-07-11)
* If you import from a Twitter network (NodeXL, Data, Import, From Twitter...) and you add a Tweet or Latest Tweet column, you can now tell NodeXL to expand any shortened URLs it finds in the tweets: http://bit.ly/LfZMMG will get expanded to https://nodexlgraphgallery.org/Pages/Graph.aspx?graphID=735, for example.  Expanding URLs can add a significant amount of time to the import.
* If you import from a Twitter search network and you calculate graph metrics for Twitter search network top items (NodeXL, Analysis, Graph Metrics), the graph metrics now appear in the graph summary (NodeXL, Graph, Summary).
* NodeXL will now count words in a text column in addition to word pairs, and it now calculates a "salience" metric for each word and word pair.  Go to NodeXL, Analysis, Graph Metrics; check "words and word pairs" in the Graph Metrics dialog box; and click the options button to select a text column and other options.

+1.0.1.217+  (2012-06-27, private release)
* You can now edit the visual properties of an entire set of edges all at once.  Select the edges, then right-click the graph pane and use the Edit Selected Edge Properties item on the right-click menu.
* When you edit the visual properties of an entire set of vertices (Edit Selected Vertex Properties on the graph pane's right-click menu), you can now edit the label, label fill color, and tooltip for the vertices.  Those were missing from the list of editable properties before.
* If you tell NodeXL to lay out the graph's groups in boxes and you choose to use the Grid layout for groups that don't have many edges (NodeXL, Graph, Layout, Layout Options), NodeXL now excludes self-loops when it counts the edges in a group.
* Bug fix: If you attempted to export a large graph to the NodeXL Graph Gallery (NodeXL, Data, Export, To NodeXL Graph Gallery) and you included the workbook and GraphML, you could get the message "The graph is too large to export" or "A problem occurred within the NodeXL Graph Gallery."

+1.0.1.216+ (2012-06-18, private release)
* When you group the graph's vertices (NodeXL, Analysis, Groups, Group by...), NodeXL now sorts the groups by descending vertex count.
* When you group the graph's vertices by cluster (NodeXL, Analysis, Groups, Group by Cluster), you can now tell NodeXL to put all neighborless vertices into one group.
* You can now tell NodeXL to automate the graph immediately after you import data into the workbook.  Go to NodeXL, Data, Import, Import Options.
* Bug fix: If you attempted to export a large graph to the NodeXL Graph Gallery (NodeXL, Export, To NodeXL Graph Gallery) and the network connection to the Gallery was slow, you could get the message "The NodeXL Graph Gallery couldn't be reached.  Try again later."

+1.0.1.215+ (2012-06-11)
* You can now hide and skip individual groups.  When a group is hidden, space is reserved in the graph pane for the group's vertices, but the vertices and their edges are hidden. When a group is skipped, its vertices and edges are completely ignored when the graph is shown.  See the new Visibility column on the Groups worksheet.
* If your graph has collapsed groups, NodeXL now remembers their locations.  Previously, the locations would be lost if you performed certain operations in the graph pane or you saved and reopened the workbook.
* If you lay out the graph's groups in boxes (NodeXL, Graph, Layout, Layout Options) and your groups have labels, you can now specify where the labels should go within the boxes.  Go to Graph Options, Other tab, Labels, Group Box Labels, Position.
* When you import a network from Twitter, YouTube, Flickr or another source (NodeXL, Data, Import), any imported cells that contain an URL are now clickable links.
* The "Top URLs in Tweet" cells on the Twitter Search Network Top Items worksheet are now clickable links.
* When you import a very large network from Twitter, YouTube or Flickr, you are now less likely to encounter an "OutOfMemoryException" error message during the import.  (The error message included the text "at System.Xml.XmlNode.get_OuterXml()".)  You can still run out of memory later, however, depending on what you do with the graph in NodeXL and how much memory your computer has.
* Bug fix: If you attempted to import a Twitter list network (NodeXL, Data, Import, From Twitter List Network) and the list name included hyphens (verified/world-leaders, for example), you would get the message "There is no such Twitter List."

+1.0.1.214+ (2012-05-15, private release)
* The graph summary (NodeXL, Graph, Summary) now includes the NodeXL version number.
* Bug fix: If you used NodeXL, Show/Hide, Workbook Columns, Show All, you would see a "Collapsed Properties" column on the Groups worksheet that is meant for NodeXL's use only and should not be visible.
* Possible bug fix: If you schedule multiple, overlapping runs of the NodeXL Network Server program, you can get a message at random times that includes the text "{"[ConfigurationErrorsException](ConfigurationErrorsException)"}: Failed to save settings: The configuration file has been saved by another program...at Smrf.NodeXL.ExcelTemplate.TaskAutomator.AutomateOneWorkbook()".  This message should now appear less frequently, if at all.
* Possible bug fix: With the same kind of overlapping runs, you could get a message that included the text "{"[UnauthorizedAccessException](UnauthorizedAccessException)"}: Access to the path is denied...at System.IO.Path.GetTempFileName()".
* Possible bug fix: With the same kind of overlapping runs, you could get a message that included the text "{"[COMException](COMException)"}: Exception from HRESULT: 0x800AC472...at Microsoft.Office.Interop.Excel._Application.Intersect()...".
+1.0.1.213+ (2012-05-13, private release)
* Bug fix, third attempt: In some cases, you could get an "Assertion Failed" error message that included the text "at Axis.OnRender."

+1.0.1.212+ (2012-05-12, private release)
* When importing a Twitter, Flickr or YouTube network (NodeXL, Data, Import), you can now specify any number for the "Limit to" field.  You used to be restricted to a limited set of drop-down choices.
* You can now enter much longer search text in the Import from Twitter Search Network dialog box.
* Bug fix, second attempt: In some cases, you could get an "Assertion Failed" error message that included the text "at Axis.OnRender."

+1.0.1.211+ (2012-05-09, private release)
* The Graph Options and Autofill Columns dialog boxes now remember which tab in the dialog was last selected.
* Some diagnostics were added to help track down an "Assertion Failed" error message that includes the text "at Axis.OnRender."

+1.0.1.210+ (2012-05-07)
* NodeXL will now count repeated pairs of words it finds in a text column on the Edges or Vertices worksheet.  You can use this to analyze tweets in a Twitter network, for example.  Go to NodeXL, Analysis, Graph Metrics and look for "word pairs."
* When you use Dynamic Filters (NodeXL, Analysis, Dynamic Filters) and you are filtering on a column that has empty cells, then by default the edges or vertices that have those empty cells are no longer filtered out of the graph pane.  You can control this behavior with a new checkbox in the Dynamic Filters dialog box.
* The graph summary for a Twitter search network (NodeXL, Graph, Summary) now includes the number of days over which the network's tweets were made.
* When NodeXL calculates graph metrics for a Twitter search network (NodeXL, Analysis, Graph Metrics), it now adds a "Top Hashtags in Tweet" column to the Groups worksheet.
* Bug fix: If you used "group-in-a-box" (NodeXL, Graph, Layout, "lay out each of the graph's groups in its own box..."), your groups had labels, and you saved an image of the graph to a file (right-click graph pane, Save Image to File), the group labels did not get scaled with the rest of the graph.
* Bug fix: If you laid out the graph using the Fruchterman-Reingold layout, for example, and then refreshed the graph using the None layout, the Graph Summary would say that the graph was laid out with the None layout.  It now says that it was laid out with Fruchterman-Reingold and ignores the subsequent None refresh.
* Bug fix: In some cases, you could get an "Assertion Failed" error message that included the text "at Axis.OnRender."

+1.0.1.209+ (2012-04-17)
* When you import a Twitter network (NodeXL, Data, Import, From Twitter...), the URLs that are optionally included in the workbook are now the full, original URLs that the Twitter user entered, instead of the shortened "http://t.co" URLs that were included in previous releases.
* If Twitter refuses to provide more information even after NodeXL pauses for "rate limiting," you will now be given the option to import the partial network that was obtained at that point.  Previously, the partial network was discarded and all your time was wasted.

+1.0.1.208+ (2012-04-05, private release)
* The top items for a Twitter search network (NodeXL, Data, Import, From Twitter Search Network) are no longer available at NodeXL, Graph, Summary.  Instead, go to NodeXL, Analysis, Graph Metrics and check the "Twitter search network top items" checkbox in the Graph Metrics dialog box.  The top items will appear in a set of tables on a new worksheet.

+1.0.1.207+ (2012-03-28, private release)
* If you import a Twitter search network (NodeXL, Data, Import, From Twitter Search Network) and you check the "Add a Tweet column to the Edges worksheet" option, then NodeXL will find the top 10 URLs, hashtags, replies-to and mentions in the entire graph, as well as in each of the graph's 10 largest groups, assuming that you have created groups.  This information is available at NodeXL, Graph, Summary.
* When you group the graph's vertices by motif, the collapsed motifs are now larger.

+1.0.1.206+ (2012-03-23, private release)
* When you autofill a color column (NodeXL, Visual Properties, Autofill Columns), NodeXL now uses a color range of orange to blue instead of red to green.
* If the Color column on the Vertices worksheet has been autofilled and the graph has one or more collapsed fan motifs, the average of the autofilled colors of the fan vertices now determines the color of just the "fan" part of the collapsed fan motif.  This color used to be applied to the entire collapsed fan motif.
* Bug fix: Grouping the graph's vertices by two-parallel motif could sometimes result in two groups that contained the same vertices.

+1.0.1.205+ (2012-03-14, private release)
* NodeXL will now calculate a new readability metric called "graph pane coverage," which is the fraction of the graph pane that is covered by vertices and edges.

+1.0.1.204+ (2012-03-11, private release)
* The glyph for a collapsed two-parallel motif is now a crescent instead of a diamond.
* Bug fix: If two vertices were in the exact same location, and an edge between them had a label, and the edge was curved, then showing the graph would pop up an "Assertion Failed" dialog box that included the text "at EdgeDrawer.DrawBezierLabel(DrawingContext oDrawingContext, ...".

+1.0.1.203+ (2012-03-05)
* Bug fix: If your graph had groups, and you laid out each of the graph's groups in its own box, and you saved an image of the graph with dimensions that differed from the size of the graph pane, then the group boxes would appear in the wrong places in the saved image.
* Bug fix: Top Item metrics weren't working properly when a workbook was automated (NodeXL, Graph, Automate).

+1.0.1.202+ (2012-03-05)
* You can now have NodeXL list the top vertices ranked by the numbers in a column of your choice.  For example, you can list the top 10 vertices ranked by closeness centrality.  The top vertices appear in a Top Items worksheet, and they get included in the graph summary (NodeXL, Graph, Graph Summary).  For more information, go to NodeXL, Analysis, Graph Metrics and click the Details link next to the Top Items checkbox.
* The graph summary now includes overall graph metrics, if they have been calculated.
* When you import from a Twitter search network (NodeXL, Data, Import, From Twitter Search Network), the graph summary now includes the dates of the earliest and latest tweets in the network.  Note: You must check all three tweet-related options under "Add an edge for each" in the Import from Twitter Search Network dialog box if you want this date range included in the graph summary.  Those options are checked by default.
* For application programmers only:  When laying out a graph synchronously, the layout will now use the value of the LayoutStyle property on the layout object.  That includes UseGroups (group-in-a-box) and UseBinning (move the graph's smaller connected components to the bottom of the graph rectangle).  Prior to this change, the LayoutStyle property was used only when laying out the graph asynchronously.

+1.0.1.201+ (2012-02-14)
* To make it easier to document your graphs, NodeXL will now provide a summary of how a graph was created.  Go to NodeXL, Graph, Summary in the Ribbon.
* When you imported a Twitter network (NodeXL, Data, Import, From Twitter...) and you added edges for "replies-to" and "mentions" relationships, a "replies-to" was also considered a "mentions."  (The thinking was that if Bob replied to Mary, Bob was also mentioning Mary.)  This has been changed so that a "replies-to" is no longer considered a "mentions."
* Bug fix: When you save an image of the graph (right-click the graph pane, select Save Image to File) and the image isn't the same size as the graph pane, the image now looks much more like the graph in the graph pane.  Before this change, the graph's vertices and edges weren't getting properly scaled.  (This was fixed once before, in version 1,0.1.194, but then accidentally broken again in version 1.0.1.196.)
* Bug fix: The name "VOSON" in the NodeXL, Data, Import, Get Third-Party Graph Data Importers menu item was misspelled as "Voson."

+1.0.1.200+ (2012-01-31)
* You can now toggle the selection state of a group of vertices by dragging a box around them in the graph pane while holding down the Ctrl key.
* If you export a graph to the NodeXL Graph Gallery (NodeXL, Data, Export, To NodeXL Graph Gallery), you can now export the NodeXL workbook to the Gallery along with the graph's image.  Others will then be able to download your workbook or the workbook's options from the Gallery.
* If you export a graph to the NodeXL Graph Gallery, the graph image that appears in the Gallery is now much larger, enabling others to see more details in the graph.
* Bug fix: If you exported a NodeXL options file (NodeXL, Options, Export) from a computer that used one language (English, for example) and then imported the options file on a computer that used a different language (Portuguese, for example), some color and font information was getting lost in the translation.
* Bug fix: If you clicked a link in the NodeXL splash screen that appears when a workbook is opened, the link would always open in Internet Explorer, even if your default browser was set to something else.

+1.0.1.199+ (2012-01-12, private release) 
* If you group the graph's vertices by motif (NodeXL, Analysis, Groups, Group by Motif), the groups are now given names that describe the motif.
* If you group the graph's vertices by motif, the collapsed motif groups now have tooltips that summarize the motifs' contents.
* If you group the graph's vertices by motif AND autofill the Color column on the Vertices worksheet, a collapsed motif group is now colored using the average color of the motif's vertices.
* If you group the graph's vertices by two-parallel motif AND autofill the Color or Width column on the Edges worksheet, the edges incident to the diamonds for the collapsed motifs are now colored and sized using the average color and size of the motif's edges.
* If you group the graph's vertices by fan motif, the fan arcs for the collapsed motifs are now scaled in such a way that the smallest fan motif in the graph has the smallest possible arc, the largest fan motif in the graph has the largest possible arc, and the other fan motifs are scaled between these limits.
* If you group the graph's vertices by two-parallel motif, the diamonds for the collapsed motifs are now scaled in such a way that the smallest two-parallel motif in the graph has the smallest possible diamond, the largest two-parallel motif in the graph has the largest possible diamond, and the other two-parallel motifs are scaled between these limits.

+1.0.1.198+ (2012-01-04, private release) 
* Bug fix: If you manually added vertices to a group (NodeXL, Analysis, Groups, Add Selected Vertices to Group), some unwanted text was inserted into the Shape column on the Vertices worksheet.
* Bug fix: If you grouped the graph's vertices by fan motif (NodeXL, Analysis, Groups, Group by Motif), the shape of the fan's head vertex was modified.  The head vertex now retains its original shape.
* Bug fix: If you grouped the graph's vertices by two-parallel motif and two or more two-parallel motifs shared the same anchor vertex, NodeXL failed to find all the two-parallel motifs.
* Bug fix: If you calculated group metrics (NodeXL, Analysis, Graph Metrics) and not all of the graph's vertices were part of a group, you would get a message that included the text "{"[KeyNotFoundException](KeyNotFoundException)"}: The given key was not present in the dictionary."

+1.0.1.197+  (2011-12-21, private release)
* You can now group the graph's vertices by motif.  Go to NodeXL, Analysis, Groups, Group by Motif in the Excel ribbon.

+1.0.1.196+  (2011-12-06)
* If your graph has groups, the graph's "modularity" is now calculated as a part of overall graph metrics (NodeXL, Analysis, Graph Metrics).  Modularity is a measure of the quality of the grouping, where a high-quality grouping has many edges within the groups and few edges between the groups. 
* Bug fix: When an image of the graph was saved to an XPS file, the graph was saved in bitmap format instead of a scalable vector format.  The XPS file is now scalable.
* Bug fix: If you duplicated worksheet rows via copy/paste and then performed certain actions, including exporting the graph to a file, you could get an error message that included the text "{"[ArgumentException](ArgumentException)"}: An item with the same key has already been added."

+1.0.1.195+  (2011-11-23)
* You can now export graphs to the NodeXL Graph Gallery ([http://www.nodexlgraphgallery.org](http://www.nodexlgraphgallery.org)) using your own account, which means that you can edit or delete the graph later, if you need to.  Go to NodeXL, Data, Export, To NodeXL Graph Gallery.
* Bug fix: When showing images imported from Twitter (NodeXL, Data, Import, From Twitter...), an occasional corrupted image would result in a red "X" being shown.  NodeXL now ignores the corruption and shows the image anyway.  (The corruption usually isn't visible.)
* Bug fix, for programmers only: You can now build the NodeXL source code when it's placed in a folder whose name includes spaces, such as "C:\Documents and Settings\Ralph\Desktop\NodeXL."  This used to raise a build error in the NodeXLExcelTemplate project.

+1.0.1.194+  (2011-11-09)
* When you save an image of the graph (right-click the graph pane, select Save Image to File) and the image isn't the same size as the graph pane, the image now looks much more like the graph in the graph pane.  Before this change, the graph's vertices and edges weren't getting properly scaled.  {"[This was accidentally broken again in version 1.0.1.196, then fixed once and for all in 1.0.1.201.](This-was-accidentally-broken-again-in-version-1.0.1.196,-then-fixed-once-and-for-all-in-1.0.1.201.)"}
* There is a new option in NodeXL, Analysis, Graph Metrics for calculating edge reciprocation.  (In a directed graph, an edge between vertices A and B is reciprocated if the graph also has an edge between B and A.)  This adds a "Reciprocated?" column to the Edges worksheet.

+1.0.1.193+  (2011-10-27)
* You can now add drop shadows and "glow" effects to the graph's vertices.  Go to Graph Options at the top of the graph pane and see the Effects settings on the Vertices tab.  Note that use of these effects slows down interaction with the graph, especially when the graph has many vertices.
* When you bundle edges, you can now specify how tightly to bundle them.  Go to Graph Options and see the Curvature settings on the Edges tab.
* The Fruchterman-Reingold layout algorithm now uses an optional Edge Weight column on the Edges worksheet to determine the attractive forces between vertices.  An edge with a larger Edge Weight exerts a greater attractive force on its vertices.  Edge Weight values should be greater than zero.  A value of zero or less is ignored and 1.0 is used instead.
* If you lay out each group in its own box and you combine intergroup edges (NodeXL, Graph, Layout, Layout Options), the optional Edge Weight column is now used to determine the width of the combined edges.

+1.0.1.192+  (2011-10-18, alpha release)
* **NodeXL's program folder has changed.**  Most people won't notice the difference.  However, if you use a third-party graph data importer, or you use Task Scheduler to run the NodeXL Network Server command-line program on a periodic basis, or you install NodeXL for multiple users, then you need to take additional steps after installing this version of NodeXL.  Please see [NodeXL's Program Folder Has Changed](http://nodexl.codeplex.com/discussions/276351).
* When you calculate group metrics (NodeXL, Analysis, Graph Metrics), a Group Edges worksheet now gets added to the workbook.  This worksheet lists the number of edges in each group and the number of edges between pairs of groups.
* When you lay out each group in its own box (NodeXL, Graph, Layout, Layout Options) and your groups have labels, the labels no longer change size as you adjust the Scale slider at the top of the graph pane.
* When you lay out each group in its own box and your groups have labels, you can now select the color and opacity of the labels.  Go to Graph Options at the top of the graph pane and select Labels in the Graph Options dialog box.
* If you open an older NodeXL workbook that doesn't have a Label column on the Groups worksheet, one will automatically be added.

+1.0.1.179+  (2011-09-28)
* Groups can now have labels, which appear when you either collapse a group (NodeXL, Analysis, Groups) or choose to lay out each group in its own box (NodeXL, Graph, Layout, Layout Options).  Group labels are specified in the new Label column on the Groups worksheet.  You can autofill the Label column using NodeXL, Visual Properties, Autofill Columns.
* If you lay out each group in its own box, you can now "combine" intergroup edges to reduce clutter.  All edges between two groups get combined into a single edge whose width reflects the number of edges that have been combined.  Go to NodeXL, Graph, Layout, Layout Options and set "Intergroup edges" to "Combined."
* When importing a YouTube video network (NodeXL, Data, Import, From YouTube Video Network), the following columns are now added to the Edges worksheet: Shared Tag, Shared Commenter and Shared Video Responder.
* Bug fix: If you attempted to import a graph from Flickr, Twitter or YouTube and your computer network used a proxy server that required authentication, you would get a message that included the text "The remote server returned an error: (407) Proxy Authentication Required."
* Bug fix: If you attempted to import a graph from Facebook and someone in your network included a very long quote in her profile, a message would appear that included the text "Exception from HRESULT: 0x800A03EC".

+1.0.1.178+  (2011-09-22, private release)
* The internal algorithm for bundling edges was improved.

+1.0.1.177+  (2011-09-14)
* There is a new "toggle selection" item on the graph pane's right-click menu.  It selects all unselected vertices and edges, and deselects those that are selected.
* The Autofill Columns feature (NodeXL, Visual Properties, Autofill Columns) will now autofill the Style column on the Edges worksheet.
* The Autofill Columns dialog box has been reorganized to make it easier to use.
* A new "edge bundling" option reduces visual clutter by bundling edges that are near each other, analogous to the way electrical cables are bundled to reduce tangling.  Go to Graph Options at the top of the graph pane and select Bundled for the Curvature option.  (Bundling edges significantly slows down many tasks when used with large graphs.)
* A new "Get Third-Party Graph Data Importers" item on the NodeXL, Data, Import menu takes you to a page where you can get third-party software that will import graph data from other types of networks.  (The page is at http://nodexl.codeplex.com/wikipage?title=Third-Party%20NodeXL%20Graph%20Data%20Importers.)
* If you import a large graph using one of the NodeXL, Data, Import menu items, NodeXL now turns off Excel's "wrap text" feature before importing the graph.  (It will warn you first.)  Text wrapping dramatically slows down the insertion of large amounts of text in an Excel workbook.
* For clarity, the NodeXL, Analysis, Groups, Group by Cluster menu item is now called Group by Cluster Algorithm.  It does the same thing as before.
* For convenience, the cluster algorithm can now be set from the Automate dialog box (NodeXL, Graph, Automate).
* Bug fix: If you imported a graph using one of the NodeXL, Data, Import menu items and the imported graph was smaller than what was in the workbook before, you would end up with leftover, unused "row stripes" in the Edges and Vertices worksheets.
* Bug fix: In the Twitter Search network (NodeXL, Data, Import, From Twitter Search Network), if you checked only the "Tweet that is not a replies-to or mentions" edge option, you would get edges that were actually replies-to or mentions.  That option _did_ work properly if you also checked the "Replies-to" and "Mentions" options, but it gave inaccurate results if it was the only checked option.
* Bug fix: If you specified a layout order using the Layout Order column on the vertices worksheet, _and_ you grouped the graph's vertices, _and_ you collapsed one or more groups, you could get an "Assertion Failed" message that included the text "at ArgumentChecker.ThrowArgumentException."
* Bug fix: The arrow key shortcuts in the graph pane stopped working in the previous version.  (The arrow keys can be used to move the selected vertices.  Combine them with the Shift key to move longer distances.)

+1.0.1.176+  (2011-08-31)
* You can now select whether vertex labels wrap, and you can set the width of the wrapping.  Click Graph Options at the top of the graph pane, then click Labels.
* If you create a graph that you would like to share with others, you can now do so by exporting the graph to the new NodeXL Graph Gallery website (http://www.nodexlgraphgallery.org).  Go to NodeXL, Data, Export, To NodeXL Graph Gallery in the ribbon.
* When importing from a Twitter network (NodeXL, Data, Import), a Location column is now added to the Vertices worksheet.  (For the Twitter List and Search networks, you have to check "add statistic columns to the Vertices worksheet" to get the new column.)  The Location is the user's city, country, or whatever he has specified in his Twitter profile.
* Bug fix: If you set the Shape of a vertex to Sphere and then tried to filter the vertex out of the graph using Dynamic Filters (NodeXL, Analysis, Dynamic Filters), a "ghost" of the vertex always remained.

+1.0.1.175+  (2011-08-17)
* If you make the graph's edges curved (in Graph Options at the top of the graph pane), you can now set the degree of curvature.
* The help system (NodeXL, Help, Help) now has Search and Index tabs.
* Bug fix: In some cases, the graph legend (NodeXL, Show/Hide, Graph Elements, Legend) had scroll bars that weren't needed.
* Bug fix: When you dragged a vertex, it could end up underneath other vertices.  (A dragged vertex is a selected vertex, and selected vertices are always supposed to be on top of other vertices.)
* Bug fix: The translucent background rectangles that are used to add contrast to vertex labels were visible even when the vertices were filtered out of the graph using Dynamic Filters (NodeXL, Analysis, Dynamic Filters).
* Bug fix: If you entered an upper-case username in the Import from Twitter User's Network dialog box (NodeXL, Data, Import, From Twitter User's Network), you could get an error message that included the text "{"[Argument Exception](Argument-Exception)"}: An item with the same key has already been added."

+1.0.1.174+  (2011-08-03)
* The graph's edges can now be curved instead of straight.  Go to Graph Options at the top of the graph pane and check "make edges curved."  (If your graph has many edges and you use edge labels, curved edges will slow down some graph operations.)

+1.0.1.173+  (2011-07-20)
* The Layout Order column on the Vertices worksheet now controls the stacking order of overlapping vertices as well as the layout order.  (You can show the Layout Order column by going to NodeXL, Show/Hide, Workbook Columns in the ribbon and checking Layout.)
* The workbook column group options (NodeXL, Show/Hide, Workbook Columns) are now treated like all of NodeXL's other options, and can be exported, imported and reset using the commands in NodeXL, Options.
* The default edge color for new graphs is now Gray.  If you have already changed the default to something else, you won't notice the change.
* Some topics in the help system (NodeXL, Help, Help) have been updated.
* Bug fix: If Excel's "auto table expansion" feature is turned off (round Office Button, Excel Options, Proofing, AutoCorrect Options, AutoFormat As You Type, Include new rows and columns in table), NodeXL won't work properly.  NodeXL now offers to turn the feature on for you.  (One symptom you can get when the feature is turned off is an error message that includes the word "IndexOutOfRangeException" when attempting to use a NodeXL, Data, Import menu item.)
* Bug fix: If your graph had groups, you laid them out in boxes (NodeXL, Graph, Layout, Layout Options) and you either saved the graph image to a file or copied it to the clipboard via the graph pane's right-click menu, the group boxes in the graph pane could end up in the wrong places.

+1.0.1.172+  (2011-07-06)
* NodeXL now includes a help system.  Go to NodeXL, Help, Help in the Excel Ribbon.
* Vertex label annotations now have background shading.  This makes it easier to read the labels when the graph is crowded.

+1.0.1.171+  (2011-06-22)
* A new button in the Ribbon resets all the workbook's options.  Go to NodeXL, Options, Reset All.
* The Twitter search network (NodeXL, Data, Import, From Twitter Search Network) works differently.  It used to import only one tweet per person; now it imports multiple tweets per person if that person has repeatedly tweeted the specified search term.  Also, the tweets themselves are now imported into the Edges worksheet instead of the Vertices worksheet, and each vertex is given a tooltip that consists of the person's username followed by his most recent tweet that contains the search term.
* The NodeXL Network Server, which is a console program that can download Twitter networks on a scheduled basis, can now download Twitter list networks.  It used to be restricted to search and user networks.  See the SampleNetworkConfiguration.xml file in NodeXL's program folder for details.
* Bug fix: If you used a logarithmic mapping to autofill a column (NodeXL, Visual Properties, Autofill Columns, Options) and then showed a legend for that column in the graph pane (NodeXL, Show/Hide, Graph Elements, Legend), the number range shown in the legend was incorrect.
* Bug fix: If you used a logarithmic mapping to autofill the X and Y columns and then showed the axes in the graph pane (NodeXL, Show/Hide, Graph Elements, Axes), the axis scales were linear instead of logarithmic.
* Bug fix: The maximum length of a Flickr screen name that you can specify in NodeXL, Data, Import, From Flickr User's Network has been increased from 20 to 254.

+1.0.1.170+  (2011-06-08)
* If you save the graph image to a file and you include an image header or footer  (right-click graph pane, Save Image to File, Image Options), the header and footer text will now wrap to multiple lines if necessary.  The text used to get truncated to one line.
* You can now specify the font to use in graph image headers and footers.
* If the graph legend is showing (NodeXL, Show/Hide, Graph Elements, Legend), the legend is now included if you save the graph image to a file.
* The options for clearing workbook columns in the Autofill Columns dialog box (NodeXL, Visual Properties, Autofill Columns) have been simplified.
* Bug fix: If you filled in the Layout Order column on the Vertices worksheet, laid out the graph's groups in boxes, and told NodeXL to use the Grid layout for groups that don't have many edges (NodeXL, Graph, Layout, Layout Options), the Layout Order values were not used within the grids.
* Bug fix: If you unchecked NodeXL, Data, Import, Clear NodeXL Workbook First AND imported multiple networks into the workbook AND one of the networks included an attribute that started with an equal sign, you would get an error message that started with "{"[COMException](COMException)"}: Exception from HRESULT: 0x800A03EC."

+1.0.1.169+  (2011-05-17)
* If your graph has groups and you choose to lay out the groups in their own boxes (NodeXL, Graph, Layout, Layout Options), you can now opt to have NodeXL use a Grid layout for groups that don't have many edges.
* A few items on the NodeXL, Analysis, Groups menu have been renamed for consistency.
* The colors that NodeXL uses for groups have been modified to produce a better-looking graph.
* The toolbar at the top of the graph pane now has buttons that lock and unlock the selected vertices.  Locked vertices do not move when the graph is refreshed or laid out again.
* A new keyboard shortcut is available in the graph pane: Press Ctrl+R to show (or refresh) the graph.  (Keyboard shortcuts can be used after you click in the graph pane.  If you want to know what keyboard shortcuts are available, right-click the graph pane.)
* Bug fix: If you ran NodeXL on a computer that used the default Windows text size (Control Panel, Display in Windows 7), changed the Windows text size to something larger, and then ran NodeXL again, NodeXL's dialog boxes would be the wrong size.  The same bug would occur if you changed the Windows text size in the other direction.
* Bug fix: Importing a Twitter network that included a corrupted image would cause an error whose error message included the text "{"[FileFormatException](FileFormatException)"}: The image format is unrecognized."
* Bug fix, for programmers only: If you built NodeXL in Visual Studio 2010, you would get a number of warnings having to do with malformed XML comments.  (There are still problems with building NodeXL in Visual Studio 2010.  See "How to build NodeXL?" at http://nodexl.codeplex.com/discussions/250507 for now.)

+1.0.1.167+  (2011-05-04)
* If your graph has groups and you choose to lay out the groups in their own boxes (NodeXL, Graph, Layout, Layout Options), you can now specify the width of the box outlines.
* When you select an edge, its width no longer changes.  NodeXL used to use the same width for all selected edges, even if the edges had varying widths when unselected.
* When a graph has groups, you now have more control over how the groups are shown. Go to NodeXL, Analysis, Groups, Group Options.
* The NodeXL, Show/Hide, Graph Elements, Groups menu item has been replaced with a checkbox in the Group Options dialog box.
* Menu items for selecting, expanding, collapsing and removing groups are now available in the menu that appears when you right-click the graph pane. (These are just shortcuts for the same menu items that are available in the Ribbon at NodeXL, Analysis, Groups.)
* Hidden edges and vertices (those that have their Visibility cells set to Hide) can no longer be selected in the graph pane.
* Edges and vertices that have been filtered (NodeXL, Analysis, Dynamic Filters) can no longer be selected in the graph pane.
* When importing a Twitter list network (NodeXL, Import, From Twitter List Network), you can now enter up to 10,000 usernames.  The maximum used to be 500.
* Bug fix: When exporting the graph to a UCINET file (NodeXL, Data, Export, To UCINET Full Matrix DL File), isolated vertices didn't get exported.
* Bug fix: When exporting the graph to a new matrix workbook (NodeXL, Data, Export, To New Matrix Workbook), isolated vertices didn't get exported.
* Bug fix: When importing a graph from a matrix workbook (NodeXL, Data, Import, From Open Matrix Workbook), isolated vertices didn't get imported.

+1.0.1.166+  (2011-04-20)
* After you click in the graph pane, a number of keyboard shortcuts are now available.  {"Press Ctrl+A to select all vertices and edges, Ctrl+V to select all vertices, Ctrl+E to select all edges, Ctrl+D to deselect everything, Ctrl+P to edit the properties of the selected vertices, Ctrl+C to save the graph image to the Windows clipboard, Ctrl+I to save the graph image to a file, an arrow key to move the selected vertices a small distance, and Shift+arrow key to move the selected vertices a large distance."}  (If you forget a shortcut, most of them are listed in the graph pane's right-click menu.)
* NodeXL no longer uses the color yellow when you tell it to create groups.  Yellow was too hard to see against a white background.
* When you save an image of the graph by right-clicking the graph pane and selecting "Save Image to File," the default file format is now PNG instead of BMP.  PNG offers the best compromise between image quality and file size.
* You can now autofill the "Collapsed?" column on the Groups worksheet.  (Go to NodeXL, Visual Properties, Autofill Columns.)
* The legend in the graph pane (NodeXL, Show/Hide, Graph Elements, Legend) has been reformatted to make it more legible.
* The NodeXL Network Server console program now lets you specify a NodeXL options file to use when a network is saved to a NodeXL workbook.  See the NodeXLOptionsFile topic in the SampleNetworkConfiguration.xml file for details.
* Bug fix: The Scale setting at the top of the graph pane was not exported or imported when you used the NodeXL, Options, Export and Import buttons in the Ribbon.

+1.0.1.165+  (2011-04-05, private release)
* Each NodeXL workbook now has its own set of options.  The options are stored right in the workbook, so if you send a workbook to someone else, she'll be using the same set of options that you did.   ("Options" are the selections you make in NodeXL's dialog boxes, in the NodeXL tab in the Excel Ribbon, and in the toolbar at the top of the graph pane.)
* If you like the options you've selected in a workbook and you want those options to be used for all new NodeXL workbooks, use NodeXL, Options, Use Current for New in the Ribbon.
* You can export a workbook's options to a separate "options file" that you can send to another NodeXL user or use yourself for other NodeXL workbooks.  Use NodeXL, Options, Export.
* You can import an options file into a workbook using NodeXL, Options, Import.  (Known bug, will be fixed in next release: The setting for the Scale slider at the top of the graph pane does not get imported.)
* The old "Options" button at the top of the graph pane is now called "Graph Options."
* There is no longer a Background button in NodeXL, Visual Attributes.  The graph's background color and image are now both set via Graph Options.
* When you group the graph's vertices by vertex attribute and the column's values are categories (NodeXL, Analysis, Groups, Group by Vertex Attribute), the category names are now used for the group names.  Previously, the groups were given the generic names "G1," "G2," and so on.
* When you create subgraph images (NodeXL, Analysis, Subgraph Images), the images now have a fixed margin.  The margin used to vary when you changed the layout margin (NodeXL, Graph, Layout, Layout Options, Margin).
* A new "Discussions" button in the NodeXL, Help ribbon tab will take you to the NodeXL Discussions list, where you can ask questions and read comments about NodeXL.

+1.0.1.164+  (2011-03-17)
* Bug fix: If you attempted to install version 1.0.1.162 on a computer that already had version 1.0.1.161 installed on it, the Setup program would pop up the message, "Error 1001. The installation of the ClickOnce solution failed with exit code -400."

+1.0.1.162+  (2011-03-16)
* When your graph has groups (NodeXL, Analysis, Groups), you can now tell NodeXL to lay out each group in its own box.  Go to NodeXL, Graph, Layout, Layout Options and select "Lay out each of the graph's groups in its own box and sort the boxes by group size."  You can also tell NodeXL to show each box's outline and hide the intergroup edges.
* The maximum number of dynamic filters per table (NodeXL, Analysis, Dynamic Filters) has been increased from 20 to 120.
* The Twitter-related dialog boxes have been updated to reflect the fact that Twitter will no longer "whitelist" people who need a lot of Twitter data.
* The order of the tasks within NodeXL, Graph, Automate has been changed: the "find clusters" option now precedes the "graph metrics" option.  This allows group metrics to be calculated for the found clusters.
* Bug fix: When autofilling columns (NodeXL, Visual Properties, Autofill Columns), you could get an error message that included the text "{"[ExternalException](ExternalException)"}: Requested Clipboard operation did not succeed."
* Bug fix (for programmers only): SubgraphCalculator.GetSubgraphAsNewGraph() failed to set the Name property of the edges in the new graph.

+1.0.1.161+  (2011-02-09)
* An additional type of Twitter network can now be imported.  When you use the new NodeXL, Data, Import, From Twitter List Network item in the Ribbon, you can view the relationships among a set of Twitter users that you specify, or among the users in a Twitter List.  (A Twitter List is a group of Twitter users that any Twitter user can create.  They are explained at http://support.twitter.com/forums/10711/entries/76460.)
* When importing a Twitter network, if you check "Add a Tweet {"[or Latest Tweet](or-Latest-Tweet)(or-Latest-Tweet)"} column to the Vertices worksheet," you will also get a "Hashtags in Tweet {"[or Latest Tweet](or-Latest-Tweet)(or-Latest-Tweet)"}" column.  The new column contains any hashtags the tweeter included in his tweet.
* When importing a Flickr, Twitter or YouTube network, the imported column headers now get word-wrapped to make them easier to read.
* Bug fix: When importing a Twitter network, the "URLs in Latest Tweet" and "URLs in Tweet" columns used a comma to separate multiple URLs -- "http://url1,http://url2", for example.  A space is now used instead.  (The comma is a valid URL character; the space isn't.)
* Bug fix: When calculating overall metrics on an older workbook (NodeXL, Analysis, Graph Metrics), you would get the message "This operation is not allowed.  The operation is attempting to shift cells in a table on your worksheet."

+1.0.1.160+  (2011-01-25)
* It now takes significantly less time to import a 1.5-level Twitter user's network (NodeXL, Data, Import, From Twitter User's Network).  In one test, a 1.5-level network that used to take an average of 202 seconds now takes 38 seconds.  (The speedup does _not_ extend to 1.0 or 2.0-level networks.)
* When importing a Twitter user's network, a Web column is now added to the Vertices worksheet.  This contains the URL of the Web page the user has specified for himself.  The column also gets added for the Twitter search network (NodeXL, Data, Import, From Twitter Search Network), provided that "Add statistic columns to the Vertices worksheet" is checked.
* When importing a Twitter user's network, if you check "Add a Latest Tweet column to the Vertices worksheet," you will also get an "URLs in Latest Tweet" column.  The new column contains any URLs the tweeter included in her tweet.  You can get a similar column when importing a Twitter search network.
* When importing a graph from a Pajek file (NodeXL, Data, Import, From Pajek File), the edge weights in the file now get imported.  They were previously ignored.
* When calculating readability metrics, you can now select which metrics to calculate.  Also, you can optionally have the metrics recalculate automatically whenever a vertex is moved in the graph pane or the graph is laid out again.  {"[Note: The readability metrics are temporarily hidden.  We are still working on this feature.](Note_-The-readability-metrics-are-temporarily-hidden.--We-are-still-working-on-this-feature.)"}
* Bug fix: It was possible to open multiple Autofill Columns dialog boxes at the same time.

+1.0.1.159+  (2010-12-19)
* A new NodeXL, Help, Sample Workbook item in the Ribbon opens a sample workbook prepopulated with graph data.  New users are also asked if they would like to see the sample workbook the first time they open NodeXL.
* The new readability metrics added to earlier private releases have been hidden while we expand the feature.
* Bug fix: When calculating readability metrics (NodeXL, Graph, Layout, Calculate Readability Metrics), edges that overlapped other edges were left with an empty Edge Crossing Readability cell in the Edges worksheet.
* Bug fix: When calculating readability metrics for a graph with circular vertices that did not overlap, the Vertex Overlap on the Overall Metrics worksheet approached but did not equal the correct value of 1.0.

+1.0.1.158+  (2010-12-13, private release)
* This release introduces "readability metrics," which attempt to quantify the readability of the graph by counting undesired edge crossings and measuring undesired vertex overlap.  These new metrics are based on work done by Cody Dunne and Ben Shneiderman at the University of Maryland.  The metrics can be calculated with the NodeXL, Graph, Layout, Calculate Readability Metrics item in the ribbon, and also from the Layout drop-down at the top of the graph pane.  Readability metrics for the graph as a whole appear in a new table on the Overall Metrics worksheet, and individual per-edge and per-vertex readability metrics appear on the Edges and Vertices worksheet.
* The Windows Start Menu item for NodeXL has been changed slightly, from "Excel Template" to "NodeXL Excel Template."  Now, when you search for NodeXL in the Start Menu, the template appears at the top of the search results.
* Bug fix: On the Groups worksheet, you can no longer select Image or Label for Vertex Shape.
* Bug fix: In some cases, the Color Options dialog box (NodeXL, Visual Properties, Autofill Columns, Edge Color, Edge Color Options, for example) was missing its OK and Cancel buttons because of an improperly sized window.  (The temporary workaround was to use the Enter key for OK and the Escape key for Cancel.)

+1.0.1.157+  (2010-12-01, private release)
* The Import from Open Edge Workbook feature on the NodeXL, Data menu has been replaced with Import from Open Workbook.  In the revised feature, you can import columns from another workbook into both the Edges and Vertices worksheets in the NodeXL workbook.  You used to be able to import columns only into the Edges worksheet.

+1.0.1.156+  (2010-11-23)
* You can now create a workbook that aggregates the overall metrics (edge counts, vertex counts, connected component counts, etc.) for a folder full of NodeXL workbooks.  Use NodeXL, Analysis, Graph Metrics, Aggregate Overall Metrics.
* If you check "Add a Tweet column to the Vertices worksheet" in NodeXL, Data, Import, From Twitter Search Network, the tweeter's geographical coordinates will be added to the Vertices worksheet when they are available.
* Similarly, if you check "Add a Latest Tweet column to the Vertices worksheet" in NodeXL, Data, Import, From Twitter User's Network, the tweeter's geographical coordinates will be added to the Vertices worksheet when they are available.
* Bug fix: When automating a folder containing many NodeXL workbooks (NodeXL, Graph, Automate) or importing a folder full of GraphML files into a set of new NodeXL workbooks (NodeXL, Import, From GraphML Files), the computer could run out of memory.  The symptoms could vary, but the error message might include the text "OutOfMemoryException," or "Not enough quota is available to process this command."

+1.0.1.155+  (2010-11-04, private release)
* You can now import a folder full of GraphML files into a set of new NodeXL workbooks.  Go to NodeXL, Import, From GraphML Files in the Excel ribbon.

+1.0.1.154+  (2010-11-02)
* You can now autofill a color column with unique colors that correspond to categories specified in another column.  Go to NodeXL, Visual Properties, Autofill Columns, Vertex Color Options (for example), and specify "The source column's values are categories."
* You now have more flexibility when autofilling the Edge Visibility, Vertex Visibility, and Vertex Shape columns.  For example, you can now say "if the source column number is greater than 0, then set the vertex shape to Solid Square; otherwise, set it to Circle."  The "otherwise" part, which is optional, is what's new.
* You can now autofill the Label Position column on the Vertices worksheet.
* Edge labels are no longer the same color as their edges.  To set the default edge label color, go to Options, Labels, Text Color.  To set the color of an individual edge label, use the new Label Text Color column on the Edges worksheet.
* A new Label Font Size column on the Edges worksheet lets you change the font size for an individual edge label.  (The default font size is still set using Options, Labels, Font.)
* Bug fix: If the Edges or Vertices worksheet had a numeric column whose column name included a left bracket, right bracket, pound sign, or single quotation mark, and you attempted to use Dynamic Filters (NodeXL, Analysis, Dynamic Filters), you would get an error message that included the text "{"[COMException](COMException)"}: Exception from HRESULT: 0x800A03EC".
* Bug fix: If you selected a label font that couldn't be used by NodeXL in Options, Labels, Font, your font selection would be silently ignored.  You are now asked to select a different font.  (Due to a bug outside of NodeXL's control, unusable fonts can't be removed from the font selection list.)

+1.0.1.153+  (2010-10-20)
* You can now add a header and footer to a graph image that you save to a file.  Right-click the graph pane, then select Save Image to File, Image Options.
* The Label Position column on the Vertices worksheet now has a "Nowhere" option.  This prevents the vertex's Label from being shown as an annotation and is a non-destructive alternative to clearing the vertex's Label cell.
* When importing a Twitter network (NodeXL, Data, Import), multi-line tweets no longer increase the row height in the Vertices worksheet.
* For programmers: When NodeXL exports a graph to GraphML, all "key" nodes now precede the "graph" node.  They used to follow the "graph" node, which caused problems with some XML parsers.
* Bug fix: On some computers, the NodeXL setup program would stop with the message "NodeXL Template Setup Starter has stopped working."
* Bug fix: If you used Import from Twitter User's Network, set "Levels to include" to 1.5 or 2.0, unchecked "Limit to...people," selected "I have a Twitter account," and your Twitter account had low rate limits, then you would not get a complete network.

+1.0.1.151+  (2010-10-06)
* Bug fix: If you calculated certain graph metrics using NodeXL, Analysis, Graph Metrics, you would get an error message that included the text "{"[Win32Exception](Win32Exception)"}: The application has failed to start because its side-by-side configuration is incorrect."

+1.0.1.150+  (2010-10-06)
* The Groups worksheet is now formatted like the Edges and Vertices worksheets, with color-coded column groups that can be collapsed and expanded using the NodeXL, Show/Hide, Workbook Columns menu in the Excel ribbon.  (The Groups worksheet formatting is available in new NodeXL workbooks.  If you open an older NodeXL workbook, the Groups worksheet formatting may look a bit odd.)
* You can now calculate graph metrics for groups.  They are calculated for each group as if the vertices and edges outside the group do not exist, and they get inserted into the Groups worksheet.  To calculate graph metrics for groups, go to NodeXL, Analysis, Graph Metrics and check "Group metrics."
* Administrators can now install NodeXL for multiple users by simply selecting an "Everyone" option in the NodeXL setup program.  There are a few things to know if you select the "Everyone" option -- in particular, you should first uninstall any older version that you installed for "Just me," including versions prior to 1.0.1.150, which were always "Just me."  See [How to install NodeXL for multiple users](http://nodexl.codeplex.com/Thread/View.aspx?ThreadId=228638) for details.
* Bug fix: The average geodesic distance on the Overall Metrics worksheet is now a decimal number.  It used to get truncated to a whole number.
* Bug fix: If you added a column to the Edges or Vertices worksheet that had spaces at the beginning or end of the column name and then clicked NodeXL, Analysis, Dynamic Filters, you would get an error message.  The message included the text "{"[ArgumentException](ArgumentException)"}: An item with the same key has already been added."

+1.0.1.137+  (2010-09-24)
* Bug fix: When importing a Twitter network (NodeXL, Data, Import), selecting the "I have a Twitter account, but I have not yet authorized NodeXL to use my account" option could, under some conditions, lead to an error message that started with "Could not find a part of the path..."

+1.0.1.136+  (2010-09-22)
* You now have more control over vertex groups.  In the NodeXL, Analysis, Groups menu, there are many new items: Collapse All Groups, Expand All Groups, Select Groups Containing Selected Vertices, Select All Groups, Remove Selected Vertices from Groups, Add Selected Vertices to Group, Remove Selected Groups, and Remove All Groups.
* The plus sign that distinguishes a collapsed group from a normal vertex is now shown in the group's center rather than next to the group, where it was easily obscured.
* You no longer have to select the Groups worksheet before you can collapse or expand groups.
* The pop-up comments on the Groups and Group Vertices worksheets have been updated to note that these worksheets shouldn't be edited in most cases.  Instead, groups should be created and managed via the NodeXL, Analysis, Groups menu.
* Bug fix: If you ran the NodeXL Network Server console program, saved the graph to a NodeXL workbook, and automated the NodeXL workbook, you could get a message that included the text "{"[FileName](FileName)"}.xlsx is locked for editing by 'another user'." 

+1.0.1.135+  (2010-09-21, alpha release)
* Bug fix: Attempting to show certain images obtained from a Twitter analysis could lead to a message that started with "{"[IOException](IOException)"}: Cannot read from the stream." 
* Bug fix: Some of the selection-related items in the graph pane's right-click menu (Select Adjacent Vertices, for example) did not always work as expected.
* Most items on the Groups menu, which is under development, are temporarily grayed out pending testing.

+1.0.1.134+  (2010-09-09, alpha release)
* Bug fix: Attempting to show certain images obtained from a Twitter analysis could lead to a message that started with "{"[ArgumentException](ArgumentException)"}: Value does not fall within the expected range."
* Bug fix: Inserting subgraph images into the workbook (NodeXL, Analysis, Subgraph Images) could lead to a message that started with "System.IO.IOException: The directory is not empty."
* Bug fix: When running multiple, simultaneous copies of the NodeXL Network Server console program, you could get a message that included the text "The process cannot access the file 'C:\Users\...\user.config' because it is being used by another process."
* Bug fix: When running the NodeXL Network Server console program, an invisible copy of Excel could sometimes be left running after the NodeXL Network Server program closed.
* Please Note: If you are already a NodeXL user, the setup program for this release will reset your custom settings, such as those you set in the Options dialog box accessible from the graph pane -- black disks for vertices, for example.  This is a one-time incident; future releases will retain your custom settings, just as previous releases have.

+1.0.1.132+  (2010-09-06)
* After you group the graph's vertices (NodeXL, Analysis, Groups), you can now select all the vertices in a group.  Go to the Groups worksheet and click on a group name.
* Once a group is selected, you can collapse it into a single vertex.  Go to NodeXL, Analysis, Groups, Collapse Group.  You can expand it again using Expand Group.
* The Groups worksheet now includes a column that tells you how many vertices are in the group.
* Bug fix: The NodeXL, Help, Check for Updates feature stopped working in version 1.0.1.131.
* Bug fix: If you clicked NodeXL, Graph, Show Graph while editing a worksheet cell, you would get a message that started with "Unable to set the Hidden property of the Range class."

+1.0.1.131+  (2010-08-19)
* This version introduces the concept of "vertex groups," or "groups" for short.  A group is a set of related vertices.  All vertices in a group are shown with the same shape and color.  Clusters are an example of groups.
* The worksheets that used to be called "Clusters" and "Cluster Vertices" are now called "Groups" and "Group Vertices."
* The NodeXL, Analysis, Find Clusters button in the ribbon has been moved to a new NodeXL, Analysis, Groups menu.
* You can now group vertices by connected components, meaning that each group of interconnected vertices will have the same shape and color.  Go to NodeXL, Analysis, Groups, Find Connected Components.
* You can now group vertices using the values in a column on the Vertices worksheet -- all vertices with degree greater than 100 in one group, all vertices with degree greater than 50 in another, for example.
* If you open an older NodeXL workbook in this new version of NodeXL, the Clusters and Cluster Vertices worksheets will be automatically renamed.
* _You cannot open a new NodeXL workbook in an older version of NodeXL._  If you attempt to do so, you will get a message that starts with "This document might not function as expected because the following control is missing: Clusters."

+1.0.1.130+  (2010-08-04)
* When calculating graph metrics (NodeXL, Analysis, Graph Metrics), duplicate edges are now skipped to avoid invalid metrics.  In previous versions, metrics that couldn't be accurately calculated because the graph contained duplicate edges were highlighted in red.
* The setting of the Scale slider at the top of the graph pane is now saved when the workbook is closed.
* The responsiveness of the Scale slider has been improved when used on larger graphs.
* When editing vertex properties directly in the graph pane (right-click graph pane, Edit Selected Vertex Properties), you can now set the vertex visibility to any value.  You were previously restricted to "Skip" and "Hide."
* If you reach Twitter rate limits while importing a Twitter network (NodeXL, Import), NodeXL now pauses for about an hour until the rate limits reset themselves.
* If you are using the NodeXL Network Server console program, the program now provides continuous updates about what it is doing: "Getting people followed by Bob," and so on.
* Bug fix: If you attempted to calculate overall metrics on a graph that had vertices but no edges, you would get an "Assertion Failed" message that started with "at OverallMetricCalculator.CalculateGeodesicDistances."

+1.0.1.129+  (2010-07-21)
* When automating an entire set of workbooks (NodeXL, Graph, Automate), you can now show the graph and save an image of the graph for each workbook.
* If you are using the NodeXL Network Server console program to create NodeXL workbooks, you can now tell the Server to automate each workbook as it is created.  For more details, search for the word "automate" in the SampleNetworkConfiguration.xml file in the NodeXL program folder.
* When you import a Twitter network (NodeXL, Import), all Twitter date columns are now in a format that Excel understands.  That means that you can now filter edges and vertices by date range using either Excel's column filters or NodeXL's Dynamic Filters feature.
* All Twitter date column headers now include "UTC" to clarify the dates' time zones.
* The Twitter column named "Replies To or Mentions Date" is now named "Relationship Date (UTC)."  For followed or following relationships, the Relationship Date is now set to the current date; it was left blank before.
* Bug fix: If you double-clicked a vertex in the graph pane to expand the selection outward, not all of the selected vertices would get selected in the Vertices worksheet.

+1.0.1.128+  (2010-07-08)
* You can now perform a number of common tasks -- merging duplicate edges, calculating graph metrics, autofilling columns, creating subgraph images, finding clusters, and showing the graph -- with a single button click.  You can do this on either a single workbook or an entire set of workbooks.  Go to NodeXL, Graph, Automate in the Excel ribbon.
* If you have multiple workbooks open and you change NodeXL options in one of them, the changes are immediately made available in the other workbooks.  In previous versions, each workbook would take a static "snapshot" of your options when the workbook was opened, which could lead to confusing behavior.


+1.0.1.127+  (2010-06-23)
* For people importing Twitter networks (NodeXL, Data, Import):  You no longer enter your own screen name or password into the Twitter dialog boxes.  Instead, NodeXL will take you to the Twitter Web site, where you can authorize NodeXL to use your Twitter account without having to tell NodeXL anything about your account.  This one-time authorization, which you can revoke, is a security measure introduced by Twitter to protect your account.
* For the same group of people:  It now pays to authorize NodeXL to use your Twitter account, even if you have not been "whitelisted" by Twitter, because Twitter's rate limits are higher for authorized accounts.  (They are still _much_ higher for whitelisted, authorized accounts.)
* For people using the NodeXL Network Server, which is a command line program that can be scheduled to periodically import Twitter networks:  Your screen name and password no longer have to be included in the network configuration file.  Instead, if you want NodeXL to use your Twitter account, you must use the NodeXL Excel Template to authorize NodeXL to do so.  This authorization is used by both the Excel Template and the Network Server, and needs to be done just once.
* Bug fix: Certain sequences of actions could lead to an "Assertion Failed" error message when opening the Dynamic Filters dialog box.  The error message details included the text "at TaskPane.ReadDynamicFilterColumn."
* Bug fix: When running in 64-bit Office 2010, the menu items for Twitter, YouTube, and Flickr did not appear in the NodeXL, Data, Import menu.
* Bug fix: When running in 64-bit Office 2010, some of the graph metrics (NodeXL, Analysis, Graph Metrics) didn't work.  You would get an error message that included the text "The executable that calculates SNAP graph metrics can't be found."

+1.0.1.126+  (2010-06-09)
* Bug fix: When attempting to use the NodeXL Excel Template on computers configured for Spanish-Paraguay (and some other languages as well), you would get an error message whose details included the text "The property 'LabelUserSettings' could not be created from it's default value. Error message: Text "Microsoft Sans Serif, 8.25pt" cannot be parsed."

+1.0.1.125+  (2010-06-08)
* When NodeXL fills a column with colors (NodeXL, Analysis, Find Clusters; NodeXL, Visual Properties, Autofill Columns), it now uses names for common colors -- "Blue" instead of "0, 0, 255", for example.
* This release includes a new program for advanced users called the NodeXL Network Server.  It does the same thing as the Twitter network import features in the NodeXL Excel Template (NodeXL, Data, Import), but it is meant to be run from a Windows command line and can be scheduled to run periodically using the Windows Task Scheduler.  For more details, search for the file "NodeXLNetworkServerFAQ.docx" after running Setup.exe.
* Bug fix: In version 1.0.1.124, which was never a default release, the Scale slider in the graph pane didn't work properly.
* Bug fix: When importing from a Twitter network, if someone's tweet started with an equal sign, as in _=:-) hello!_, an error would occur.  The error's details included the text "COMException" and "at Microsoft.Office.Interop.Excel.Range.set_Value."

+1.0.1.124+  (2010-05-26)
* When importing from a Twitter Network (NodeXL, Data, Import), if you opt to add edges for "replies to" or "mentions" relationships, a new "Replies To or Mentions Date" column will get added to the Edges worksheet.
* Bug fix: When importing a GraphML file created by yED (NodeXL, Data Import, From GraphML File), you could get the following message: "The file could not be opened. Details: An XML node with the name "default" is missing a required descendant node whose value must be a non-empty String."
* Bug fix, for programmers only: If you used the NodeXLControl in your own application, and you called the DrawGraphAsync() method with a layOutGraphFirst argument of false, the DrawingGraph and DrawGraphCompleted events did not fire.  {"[This change was reverted in version 1.0.1.25 and the events were renamed to LayingOutGraph and GraphLaidOut.](This-change-was-reverted-in-version-1.0.1.25-and-the-events-were-renamed-to-LayingOutGraph-and-GraphLaidOut.)"}

+1.0.1.123+  (2010-05-11)
* The Vertex 1 and Vertex 2 columns on the Edges worksheet are now frozen, meaning that they remain visible even if you scroll the worksheet to the right.  Ditto for the Vertex column on the Vertices worksheet.  (To unfreeze them, use View, Freeze Panes, Unfreeze Panes in the Excel Ribbon.)
* The worksheet columns for labels and tooltips are now shown by default.  You used to have to unhide them using NodeXL, Show/Hide, Workbook Columns.
* The time required to lay out graphs using Fruchterman-Reingold (NodeXL, Graph, Layout) has been significantly reduced.  The improvement varies with the graph, but in most cases the layout time is half of what it was.
* Repeatedly double-clicking a vertex now expands the selection outward.  The first double-click selects the vertex's adjacent vertices, the second double-click adds the vertices adjacent to the adjacent vertices, and so on.
* If you tell NodeXL to move the graph's smaller components into boxes at the bottom of the graph (NodeXL, Graph, Layout, Layout Options), you can now specify the box size and the maximum size of the components to move.
* Bug fix: When calculating all graph metrics using a French version of Windows (and some other language versions as well), you would get an "Assertion Failed" dialog box that started with the text "at BrandesFastCentralityCalculator.TryCalculateGraphMetricsCore()."
* Bug fix: When importing a GraphML file using a French version of Windows (and some other language versions as well), you could get an error message that went something like "The GraphML-attribute value ... is not of the specified type."

+1.0.1.122+  (2010-04-28)
* The calculations for the betweenness, closeness, and eigenvector centralities (NodeXL, Analysis, Graph Metrics in the Excel ribbon) are now significantly faster, due to NodeXL's use of Jure Leskovec's SNAP graph library.  (For more information about SNAP, go to http://snap.stanford.edu/index.html.)
* The centrality numbers differ from the number calculated by previous versions of NodeXL, due to SNAP's use of different centrality algorithms.
* PageRank has been added to the list of available graph metrics.
* When you tell NodeXL to find clusters of vertices (NodeXL, Analysis, Find Clusters), you now can choose among three cluster-detection algorithms.  Click the down-arrow next to the Find Clusters button to select an algorithm.
* The default setting for vertex label position is now Bottom Center instead of Top Right.  (You can change the default label position by going to Options in the graph pane and clicking Labels.  You can change the label position for an individual vertex with the Label Position column on the Vertices worksheet.  To show that column, use NodeXL, Show/Hide, Workbook Columns, Labels.)
* When you register, you'll now be asked to fill out a short survey.  Registration, which is optional, can be done either while running NodeXL's setup program or by going to NodeXL, Help, Register in the Excel ribbon after NodeXL is installed.
* Bug fix: When analyzing large Twitter networks (NodeXL, Data, Import, From Twitter...) with the Replies To and Mentions options checked, the analysis could take a very long time and appeared to be stalled.  It was actually taking its sweet time searching for Twitter screen names within tweets.  That search is now several orders of magnitude faster.
* Bug fix: There were rounding errors in the Opacity setting in the Vertex Properties dialog box.  (This dialog box, which can be reached by right-clicking the graph pane and selecting Edit Selected Vertex Properties, lets you set the color, shape, and other properties of one or more vertices directly from the graph pane.)
* Bug fix: If you used the yEd Graph Editor to create a graph containing a nested graph, exported the parent graph to GraphML, and attempted to import the GraphML into NodeXL, you would get a message saying something like 'An "edge" XML node references the node id "n0::n0", for which there is no corresponding "node" XML node.'  Now, NodeXL will successfully import the GraphML, although the nested graph will be replaced by a single vertex.

+1.0.1.121+  (2010-04-12)
* You can now export a NodeXL workbook to a GraphML file.  Go to NodeXL, Data, Export, To GraphML File.  All columns in the Edges and Vertices worksheet get exported, including columns you've added yourself.
* You can now tidy up a graph by "snapping" the vertices to the nearest locations on a grid.  In the graph pane, click the down-arrow to the right of Lay Out Again, then select Snap Vertices to Grid.  There is also a Set Grid Size item on the same menu.
* When importing an edge list from another workbook (NodeXL, Data, Import, From Open Edge Workbook), you can now select or deselect all the columns at once instead of having to check or uncheck them one by one.
* The Harel-Koren layout algorithm is memory intensive and can cause NodeXL to run out of memory when a very large graph is laid out.  When this occurs, a clearer error message now appears explaining what you can do to try to work around the problem.  The previous error message was written in techno-babble.
* Bug fix: When attempting to import a GraphML file created by the yEd program (NodeXL, Data, Import, From GraphML File), an error message popped up.  The message was 'An XML node with the name "key" is missing a required descendant node whose value must be a non-empty String.  The XPath is "@attr.name".'
* Bug fix: The Pajek program was unable to read the complete Pajek file exported by NodeXL (NodeXL, Data, Export, To Pajek File). 

+1.0.1.120+  (2010-03-23)
* The Scale slider in the graph pane's toolbar has been flipped horizontally to make it more intuitive.  When the slider is moved all the way to the right, which is its default position, the graph's vertices are shown at normal size.  As you move the slider to the left, the vertices shrink without changing location within the graph.
* The graph pane has a new Lay Out Selected Vertices Again, Within Bounds menu item.  Click the down-arrow to the right of Lay Out Again to get to it.  This menu item lays out the selected vertices again, but limits the layout to the rectangle defined by the outermost selected vertices.
* When the graph's smaller components are put at the bottom of the graph (NodeXL, Graph, Layout, Layout Options, Put the Graph's Smaller Components at the Bottom of the Graph), the components are now sorted by the Layout Order column on the Vertices worksheet.  (Use NodeXL, Show/Hide, Workbook Columns, Layout to show the Layout Order column.)  The components are sorted first by vertex count, and then by  the smallest Layout Order value within the component.
* Bug fix: When "Put the Graph's Smaller Components at the Bottom of the Graph" was checked and the Layout was set to None, clicking Refresh Graph caused the graph's smaller components to move.  Now, when the Layout is None, no vertices move no matter what.
* Bug fix: Vertex tooltips in the graph pane would change size as you adjusted the Zoom and Scale sliders.
* Bug fix: The graph's background image (NodeXL, Visual Properties, Background) would change size as you adjusted the Scale slider.
* Bug fix: On Vista and XP computers, detaching the graph pane from the Excel window (which you can do by dragging the graph pane's title bar) and then right-clicking the graph pane popped up a menu that behaved erratically.  If you clicked on the Save Image to File menu item, for example, the expected submenu items did not appear and a cell in Excel started flashing.
* Bug fix: If you autofilled some columns (NodeXL, Visual Properties, Autofill Columns) and displayed the autofill results in the graph legend (NodeXL, Show/Hide, Graph Elements, Legend), the autofill results would disappear from the legend when you clicked Refresh Graph.
* Bug fix: In version 1.0.1.117 (which was never made the default NodeXL download but was left as an Alpha release), you would get a "..was signed by an untrusted publisher, and as such cannot be installed automatically" error when attempting to run the Setup program on Vista and Windows 7 computers with Window's User Account Control (UAC) turned on.  You could avoid the error by right-clicking the Setup program and selecting Run as Administrator.  This is no longer necessary.

+1.0.1.117+  (2010-03-10, Alpha Release Only)
* You can now specify edge styles such as solid, dash, dot, dash-dot, and so on.  Use the new Style column on the Edges worksheet.
* There is a new set of buttons at the top of the graph pane to assist in selecting vertices, zooming the graph, and moving around the zoomed-in graph.  Hover the mouse over the buttons for information on how they can be used.
* When autofilling a Label or Tooltip column (NodeXL, Visual Properties, Autofill Columns), the autofill is now "displayed text only."  Prior to this change, if you autofilled Tooltip using Betweenness Centrality as a source column, for example, the raw centrality numbers (0.932765432) were autofilled into Tooltip as numbers, even if the Betweenness Centrality column displayed only the first three decimal places (0.933).  Now, 0.933 will be autofilled into Tooltip, and it will be as text, not a number.
* Bug fix: Workbooks created on one computer could not be reliably opened on another -- you could get a "There was an error during installation" message when attempting to open such a workbook.  Now, new workbooks can be shared between any computers running this version of NodeXL or later.  IMPORTANT NOTE: Workbooks created with older versions of NodeXL will still have this problem.  If you run into this problem when attempting to open an older workbook, you can fix it by going to NodeXL, Import, From NodeXL Workbook Created On Another Computer.

+1.0.1.113+  (2010-02-23)
* The Layout Options dialog box, where you set the graph's margin and a few other options, is no longer available from Options in the graph pane.  Instead, go to NodeXL, Graph, Layout, Layout Options.  (You can also click the layout drop-down in the graph pane and select Layout Options.)
* The "Binned" option under NodeXL, Graph, Layout has been removed.  In its place, you can now "bin" the graph using any of the other layouts.  Go to NodeXL, Graph, Layout, Layout Options and check "Put the graph's smaller components at the bottom of the graph."  (We're no longer using the word "binned."  We're searching for a better term.)
* There is a new Lay Out Visible Vertices Again item in the drop-down menu for the Lay Out Again button in the graph pane.  This item is also available when you right-click the graph pane.  Laying out visible vertices will leave untouched those vertices that have their Visibility set to Hide, and vertices that have been filtered out using dynamic filters (NodeXL, Analysis, Dynamic Filters).
* When you autofill a workbook using NodeXL, Visual Properties, Autofill Columns, your autofill settings now get stored within the workbook.  This allows you to see how the workbook was autofilled when you reopen it later.
* Double-clicking a vertex in the graph pane will now select the vertex and its adjacent vertices.  (You can also select a vertex's adjacent vertices by right-clicking the vertex and selecting Select Adjacent Vertices.)
* New NodeXL users can once again register to be on the NodeXL mailing list.  (Registration was turned off in the last several releases.)  Go to NodeXL, Help, Register.  You can also register while installing NodeXL.

+1.0.1.112+  (2010-02-09)
* The setup program has been modified to allow NodeXL to be installed on 64-bit versions of Office 2010 Beta.  Note that NodeXL has received only minimal testing with Office 2010 Beta and is not guaranteed to work with all Beta editions and configurations.

+1.0.1.111+  (2010-02-09)
* The Dynamic Filters dialog box (NodeXL, Analysis, Dynamic Filters) now includes a histogram for each Excel column that can be used for filtering.  The histogram shows the distribution of the values in the column.  (The histograms are available only in workbooks created from the latest NodeXLGraph template.  If you open an older NodeXL workbook, histograms will not be available.)

+1.0.1.110+  (2010-02-03)
* The Overall Metrics worksheet now includes more information about the degree, in-degree, out-degree, betweenness centrality, closeness centrality, eigenvector centrality, and clustering coefficient metrics when those metrics are computed.  The additional information includes the minimum, maximum, average, and median metric values, and a histogram showing the metric value distribution.  (The histograms are available only in workbooks created from the latest NodeXLGraph template.  If you open an older NodeXL workbook, histograms will not be available.)
* The "Convert Old Workbook" item on the NodeXL, Data, Import menu in the Ribbon is now called "Import from NodeXL Workbook Created on Another Computer."  This menu item can be used to work around the following problem: NodeXL workbooks created on a 64-bit Windows computer cannot be opened directly in Excel on a 32-bit Windows computer, and vice-versa.  (If you attempt to do so, you will get an error message whose details include "could not find a part of the path.")
* A Clear All Worksheet Columns Now button has been added to the Autofill Columns dialog box (NodeXL, Visual Properties, Autofill).  Also, you can now clear an individual worksheet column by clicking a button in the dialog box's Options column.
* Bug fix: On large-font machines, the buttons at the bottom of the Autofill Columns dialog box didn't fit within the dialog box.
* Bug fix: In some circumstances, vertices were drawn below the bottom of the graph pane and were impossible to see.  One such circumstance was when the selection was exported to a new workbook (NodeXL, Data, Export, Selection to New NodeXL Workbook).  The graph pane in the new workbook acted as if it were taller than its real height, leading to vertices dropping off the bottom.

+1.0.1.109+  (2010-01-26)
* Overall Metrics (NodeXL, Analysis, Graph Metrics, Miscellaneous, Overall Metrics) now include the graph's maximum geodesic distance (also known as graph diameter) and average geodesic distance.
* Overall Metrics now also include the number of connected components in the graph, the number of connected components that have only one vertex, the maximum number of vertices in a connected component, and the maximum number of edges in a connected component.
* If you are using the graph legend (NodeXL, Show/Hide, Graph Elements, Legend) to show visual properties that have been autofilled from numeric source columns (NodeXL, Visual Properties, Autofill Columns), you can now control the number of decimal places that are shown in the legend.  To do so, format the source column in the worksheet as Numeric (Home, Cells, Format, Format Cells) and set Decimal Places to your desired value.
* The setup program has been modified to allow NodeXL to be installed on computers that have 32-bit Office 2010 Beta.  It has been tested only with the "Microsoft Office Professional Plus 2010 Beta" edition and is not guaranteed to work with all Beta editions and configurations.
*  The Twitter search network (NodeXL, Data, Import, From Twitter Search Network) has been modified to accommodate a January 18 change in the Twitter search API.  The change involved what happens when Twitter rate limiting kicks in.
* Bug fix: On Windows XP machines that have Windows Desktop Search 4.0 installed, subject text searches did not work when importing from an email network (NodeXL, Data, Import, From Email Network).  If you checked "Subject Includes text" and specified subject text, no emails would be found.
* Bug fix: When exporting the selection to a new workbook (NodeXL, Data, Export, Selection to New Workbook), the new workbook sometimes contained duplicate edges and vertices, depending on how the edges and vertices in the original workbook were selected.
* Bug fix: When using Autofill Columns, if you autofilled a color column from a source column that contained all zeros, you would get an "ArgumentOutOfRangeException" message that started with "ColorGradientMapper.Initialize: dMaxColorMetric must be > dMinColorMetric."
* Bug fix: If you used an Excel table filter (the down-arrows within the table column headers) to filter out the first row of the Edges or Vertices worksheet and then clicked Show Workbook, an "Assertion Failed" dialog box would pop up with a message that started with "at ExcelUtil.TryGetNonEmptyRange(Range range, Range& usedRange)."

+1.0.1.108+  (2010/01/14)
* There is a new "Binned" option in the list of layout algorithms (NodeXL, Graph, Layout in the Excel ribbon).  The Binned layout places the graph's smaller connected components (those with one, two, or three vertices) at the bottom of the graph pane, then lays out the rest of the vertices in the remaining space.

+1.0.1.107+  (2010/01/06, Private Release)
* Added an "Auto Layout on Open" row to the hidden PerWorkbookSettings table.  When the NodeXL workbook is being filled in by an external program, this allows the program to set the layout algorithm and force the workbook to be automatically read into the graph.

+1.0.1.106+  (2010/01/06)
* You can now add a background image to the graph.  Go to NodeXL, Visual Properties, Background in the Excel ribbon to specify the image.  (Known issue: When the scale of the graph is changed using the Scale slider in the graph pane, the scale of the background image changes as well.)
* You can now set the graph background color for the current workbook only, and the color will get stored within the workbook.  To set the background color for the current workbook, go to NodeXL, Visual Properties, Background.  To set the default background color for new workbooks, go to Options in the graph pane.
* Bug fix: In the Flickr user's network (NodeXL, Data, Import, From Flickr User's Network), if you check the "Add a vertex for each person who commented on the user's photos" option, the commenters will now appear in the Vertex 1 column instead of the Vertex 2 column.  In previous versions, the commenter edges were pointing in the wrong direction.
* Bug fix: In the Flickr user's network, the Comment Time (UTC) column had erratic formatting on computers using the Portuguese language, and probably other languages as well.
* Bug fix: In the YouTube user's network (NodeXL, Data, Import, From YouTube User's Network), the "Joined YouTube Date" column in the Vertices worksheet could be off by one day, and it wasn't clear which time zone the date corresponded to.  The column has been renamed "Joined YouTube Date (UTC)."
* Bug fix: In the YouTube video network (NodeXL, Data, Import, From YouTube Video Network), the "Created Date" column in the Vertices worksheet could be off by one day, and it wasn't clear which time zone the date corresponded to.  The column has been renamed "Created Date (UTC)."
* Bug fix: On computers that use a "DPI scaling" other than 96 DPI (on Vista, for example, go to Control Panel, Personalization, Adjust font size (DPI), Larger Scale), images would always get scaled, even if you went to Options in NodeXL's graph pane and set Size (images) to "Actual size."

+1.0.1.105+  (2009/12/16)
* The list of available layouts (NodeXL, Graph, Layout) now includes Polar Absolute.  This differs from the older Polar layout (which is still available) in that the Polar R values are in absolute units of 1/96 inch, and there are no value limits.  In the older Polar layout, Polar R ranges from 0.0 to 1.0, where 1.0 represents one-half the graph pane's width or height, whichever is smaller.
* The maximum Size value on the Vertices worksheet has been increased from 10 to 100.  A Size value of 10 results in the same size vertex as before, so you won't notice this change unless you specify values greater than 10.
* Bug fix: When switching windows while a long-running operation was in progress, and error message starting with "{"[COMException](COMException)"}: Exception from HRESULT: 0x800AC472" would occasionally pop up.

+1.0.1.104+  (2009/12/09)
* The way Twitter, Flickr, and YouTube request errors are dealt with has been overhauled.  NodeXL used to deal with such an error by retrying the request several times, with pauses between retries.  If the retries didn't work, NodeXL would stop the import process and ask whether you wanted to import the partial network obtained before the request failed.  Now, NodeXL does not stop.  Instead, it attempts to get the rest of the network, and at the end of the process it tells you how many requests failed.  Thus, you will always get a network (unless the very first request fails), and it is up to you to decide whether the number of failures is acceptable.
* In the Flickr user's network (NodeXL, Data, Import, From Flickr User's Network), checking "add user information to the Vertices worksheet" now populates the Image File column with a photo of each user.
* In the Twitter search network (NodeXL, Data, Import, From Twitter Search Network), checking "Add a Tweet column to the Vertices worksheet" now also adds a Tweet Date column.
* Similarly, in the Twitter user network (NodeXL, Data, Import, From Twitter User's Network), checking "Add a Latest Tweet column to the Vertices worksheet" now also adds a Latest Tweet Date column.
* You can now fill the Edge Label column using Autofill Columns.  (Go to NodeXL, Visual Properties.)

+1.0.1.103+  (2009/12/01)
* You can now import a network of Flickr users.  Go to NodeXL, Data, Import, From Flickr User's Network.
* Some improvements were made to the way Twitter, YouTube, and Flickr server errors are handled.
* Bug fix: When using Autofill Columns (NodeXL, Visual Properties, Autofill Columns) to fill one or more edge columns but zero vertex columns, the graph legend (NodeXL, Show/Hide, Graph Elements, Legend) would show a "Vertex Properties" column header instead of the correct "Edge Properties."

+1.0.1.102+  (2009/11/26)
* The feature that imports a network of related Flickr tags has been improved.  (It's at NodeXL, Data, Import, From Flickr Related Tags Network.)  You can now select the number of network levels to include, an optional sample image file can be included for each tag, and the dialog now provides feedback as it requests the various parts of the network from Flickr.

+1.0.1.101+  (2009/11/24)
* Bug fix: When importing a network of YouTube videos (NodeXL, Data, Import, From YouTube Video Network), you could sometimes get an error message that started with "Namespace prefix 'g' is not defined."

+1.0.1.100+  (2009/11/18)
* You can now import a network of YouTube videos.  Go to NodeXL, Data, Import, From YouTube Video Network.
* When importing a YouTube user's network (NodeXL, Data, Import, From YouTube User's Network), "subscribes to" relationships are now differentiated in the Relationship column on the Edges worksheet.  Such relationships are now marked as "subscribes to channel," "subscribes to favorites," or "subscribes to playlist."
* Bug fix: If a problem occurred while getting a Twitter or YouTube user's network and you decided to import the partial network that had already been obtained, you could get this error message: An "edge" XML node references the node id "...", for which there is no corresponding "node" XML node.'
* Bug fix: The Twitter Search Network feature was failing to recognize a reply-to or mentions if the replied-to or mentioned screen name was followed by a colon, as in "@johndoe: Hello".
* Bug fix: The Twitter Search Network feature was not recognizing a reply-to as a mentions.  Twitter defines a "mentions" as "any Twitter update that contains @username in the body of the tweet."  Therefore, NodeXL now considers a reply-to to be both a reply-to and a mentions.

+1.0.1.99+  (2009/11/14, private release)
* Added a workaround for the following error encountered on at least one computer when attempting to get a Twitter network: "The request was aborted: The request was canceled."

+1.0.1.98+  (2009/11/12)
* When computing graph metrics (NodeXL, Analysis, Graph Metrics), betweenness and closeness centralities are now computed simultaneously instead of sequentially, saving significant time when analyzing a large graph.
* You can now import a network of YouTube users.  Go to NodeXL, Data, Import, From YouTube User's Network.
* If a problem is encountered while importing a Twitter network (NodeXL, Data, Import, From Twitter Search Network and From Twitter User Network), you are now given the option of importing the partial network that was obtained before the problem occurred.  In previous versions, any glitch in the connection to Twitter would result in a failure and you would have to start the import again.
* Additional information on Twitter users is now provided.  For the Twitter User Network, the following columns are now added to the Vertices worksheet: Description, Favorites, Time Zone, Time Zone UTC Offset (Seconds), Joined Twitter Date.  For the Twitter Search Network, these columns will be added only if "Add statistic columns to the Vertices worksheet (slower)" is checked.
* When importing an email network (NodeXL, Data, Import, From Email Network), you can now filter on the subject text.

+1.0.1.97+  (2009/10/28)
* Vertices with the shape "Label" can now be resized using the Size column on the Vertices worksheet.  Setting the Size column changes the height of the font, and the overall vertex size adjusts accordingly.  The default font height is set using Options, Labels in the graph pane.
* You can now set the position of vertex labels that are shown as annotations.  To set the default position, use Options, Labels in the graph pane.  To set positions on a per-vertex basis, use the new Label Position column on the Vertices worksheet.  (The new column is hidden by default, along with the other label columns.  To show the label columns, go to NodeXL, Show/Hide, Workbook Columns in the Excel ribbon and check the Labels menu item.)
* You can now truncate long vertex and edge labels in the graph pane.  Use Options, Labels.
* Vertex and edge labels can now be hidden.  In the Excel ribbon, use NodeXL, Show/Hide, Graph Elements, Vertex Labels and Edge Labels.
* The drop-down lists in the NodeXL workbook have been simplified for readability and ease of typing.  In the Shape column on the Vertices worksheet, for example, you can now select or type "Circle" instead of the old "Circle (1)".  For backward compatibility, the old options will still be recognized when you read a workbook created with an older version of NodeXL.
* The color of the border drawn around images can now be set using the Color column on the Vertices worksheet.  The border also uses any specified Opacity value.
* The Image and Label shapes have been added to the NodeXL, Visual Properties, Vertex Shape menu in the Excel ribbon.  They were accidentally omitted in the previous release.
* Bug fix: Custom colors created in the Options dialog box were not retained, and custom colors created from within the workbook weren't available from Options.  Now, a custom color created anywhere will be available anywhere else in the same workbook.  (Custom colors are not shared _between_ workbooks, however.)
* Bug fix, redux: Dynamically filtering vertices shown as images sometimes left "shadows" of the image outlines on the screen.  The alleged fix in version 1.0.1.96 didn't work on all machines.  This fix should always work, so long as the filter opacity in the Dynamic Filters dialog box is set to 0%.  (If the opacity is greater than 0%, you might still see an outline shadow.)

+1.0.1.96+  (2009/10/14)
* Using images for vertices has been simplified.  To show a vertex as an image, set the Shape cell on the Vertices worksheet to Image, then fill in the new Image File cell.  Image IDs have been eliminated and the Images worksheet no longer exists.
* Vertex labels now work differently.  Instead of "Primary Label" and "Secondary Label" columns, there is now a single "Label" column, and the list of vertex shapes now includes "Label."  To show a vertex as a box containing text, set the Shape cell on the Vertices worksheet to Label, then fill in the Label cell with the text.  To "annotate" another vertex shape with text, set the Shape to something else and fill in the Label cell with the annotation text.  You cannot annotate a Label shape.
* The "What to Show" column on the Vertices worksheet has been removed.  It is no longer needed, because every vertex now has a shape and you control what is shown (image, label, or another shape) via the Shape column.
* The "Primary Label Fill Color" column on the Vertices worksheet is now "Label Fill Color."
* The part of NodeXL that communicates with the Twitter Web service has been updated to accommodate Twitter API changes that go into effect on October 26, 2009.  Older versions of NodeXL will not work properly with Twitter starting on that date.
* Although importing from Twitter provided URLs to the Twitter users' photos, the photos were difficult to actually get into the graph.  Now if you want to show the photos, just set the Shape column on the Vertices worksheet to Image after importing from Twitter.
* Bug fix: If you used "near:" or "within:" when importing from a Twitter search network, you would get an incorrect error message concerning rate limiting.  Now, you will get a message telling you that Twitter doesn't allow "near:" or "within:" to be used when searching from an outside program, such as NodeXL.
* Bug fix: Non-English words were not handled properly when importing from a Twitter search network.  This caused a "There are no people in that network" message to be displayed when searching for tweets that contained Korean words, for example.
* Bug fix: If a Twitter user's list of people he is following or are following him included someone who was "suspended" by Twitter, NodeXL sometimes didn't get the complete list.
* Bug fix: Dynamically filtering vertices shown as images sometimes left "shadows" of the image outlines on the screen.

+1.0.1.95+  (2009/09/28)
* When an email network is analyzed (NodeXL, Data, Import, From Email Network), the resulting graph is now directed.  This means that the relationships (John,Mary) and (Mary,John) are no longer combined into a single edge with an edge weight of 2; instead, they are considered unique edges.

+1.0.1.94+  (2009/09/28)
* When importing a Twitter search network (NodeXL, Data, Import, From Twitter Search Network), you can now add a Tweet column to the Vertices worksheet.
* When importing a Twitter user network (NodeXL, Data, Import, From Twitter User Network), you now have some options for how edges get added to the graph.  You can add an edge for each followed/following relationship (which was always done in previous versions), each "replies-to" relationship in the people's latest tweets, and each "mentions" relationship in the latest tweets.
* Ditto for the Twitter search network, although in this case the "replies-to" and "mentions" relationships apply to the tweets that satisfied the search criteria, which aren't necessarily the people's latest tweets.
* Twitter search networks and Twitter user networks now add an Image URL column to the Vertices worksheet.  The images are those of the people who wrote the tweets.
* Bug fix: In the Import from Twitter Search Network dialog box, the word "latest" was removed from the text, "Search for people whose latest tweets contain..."  Reason: The tweets returned by the search aren't necessarily the people's latest tweets.
* Bug fix: When importing a Twitter search network, a tweet posted by a person with "protected" status in Twitter would bring the search to a halt with a "There is no Twitter user with that screen name" error.  Now, such people are skipped.
* Bug fix: Using any of the NodeXL, Data, Import items in the ribbon failed to clear the Images worksheet before importing the new data.

+1.0.1.93+ (2009/9/18)

* You can now import a Twitter network of users who have tweeted a specified term.  For example, you can create a graph with a vertex for each person who has included the hashtag "#chi2010" in a tweet, with an edge between the people who follow each other.  In the Excel Ribbon, go to NodeXL, Data, Import, From Twitter Search Network.
* When including an image in a graph, you can now specify an URL to an image on the Internet.
* When an image file isn't available, an error message is no longer displayed.  Instead, a small red X is shown in the graph in place of the missing image.
* Images can now be resized using the Size column on the Vertices worksheet.  There are new options in the Options dialog box for setting the default image size.  (Known bug: Changing the default image size doesn't update the graph pane until the workbook is refreshed.)
* When importing from a Twitter user's network (NodeXL, Data, Import, From Twitter User's Network), a Relationship column is added to the Edges worksheet.  This gets set to Followed or Follower.
* The Twitter dialog boxes now provide feedback on what they're doing as they assemble the requested network.
* Bug fix: Attempting to get a Twitter user network that included someone who "protected" her Twitter identity would cause a failure.  Now, that user is skipped and the rest of the network is obtained.
* Bug fix: There was a rounding error with very small numbers (on the order of 1.0E-22) that could cause some vertices or edges to always be filtered out by Dynamic Filters, even if the filters were reset to their entire range.

+1.0.1.92+ (2009/9/4)

* In the Autofill Columns dialog box (NodeXL, Visual Properties, Autofill Columns), you can now specify a logarithmic mapping instead of a linear mapping when autofilling Edge Color, Edge Width, Edge Opacity, Vertex Color, Vertex Size, Vertex Opacity, Vertex Primary Label Fill Color, Vertex Layout Order, Vertex X, Vertex Y, Vertex R, or Vertex Polar Angle.  Click one of the Options buttons, then check "use a logarithmic mapping."
* You can now import a network of Flickr tags related to a specified tag.  Use NodeXL, Import, From Network of Related Flickr Tags.  You will need what Flickr calls an "API key."  There is a link in the Import dialog box for requesting a key from Flickr.
* The Twitter import feature has been expanded.  (It's at NodeXL, Import, From Twitter User's Network.)  You can now import the network of people followed by a user, people following a user, or both.  There is a new option for selecting a 1, 1.5, or 2-level network, and you can limit the network to a specified number of people.  New columns are added to the Vertices worksheet: Followed, Followers, Tweets, and (optionally) Latest Tweet.  Also, you can right-click a vertex in the graph pane and select the new Open Twitter Page for This Person menu item.
* Bug fix: When using dynamic filters, a filtered edge's label obscured what was under it even though the edge itself was hidden.
* For programmers only: The IGraphDataProvider interface used by data provider plug-ins for the Excel Template has changed.  The GetGraphData() method is now called TryGetGraphData() and it now returns a Boolean.  (The original design failed to accommodate failures while getting graph data.)

+1.0.1.91+ (2009/8/19)

* You can now label edges by filling in a new Label column on the Edges worksheet.  The Label column is hidden by default.  To make it visible, use NodeXL, Show/Hide, Workbook Columns, Labels in the Excel Ribbon.
* You can now import a graph from a GraphML file.  GraphML is an XML-based file format used by a variety of graph applications and libraries, including Pajek, "R," and JUNG.  NodeXL will import all edge and vertex attributes in a GraphML file, including those that correspond to standard NodeXL columns such as Edge Color and Vertex Size.  Use NodeXL, Data, Import, From GraphML File.
* Importing from a Twitter network (NodeXL, Data, Import, From Twitter Network) is now more reliable, thanks to automatic retries that will occur if a request to Twitter fails.
* Of possible interest to developers: NodeXL now supports custom "plug-in" .NET assemblies that will import graphs from custom data sources.  For details, see the IGraphDataProvider Interface topic in the NodeXLApi.chm help file and the sample implementation in SampleGraphDataProvider.cs.
* The graph legend is now more compact.  (To show the legend, use NodeXL, Show/Hide, Graph Elements, Legend.)
* Bug fix: When importing from an open edge workbook (NodeXL, Data, Import, From Open Edge Workbook), columns in the open edge workbook that have the same name as standard NodeXL columns will be copied to those NodeXL columns.  Before, a column named "Color" was copied to "Color 2," for example.
* Bug fix: Your settings, such as those entered in the Options dialog box, are now stored in a single file in your local Windows profile.  Before, each named NodeXL workbook got its own settings file.

+1.0.1.90+ (2009/7/24)

* When you import data into the workbook (NodeXL, Data, Import), you now have the option to append the imported data to the workbook's contents instead of clearing the workbook first.  Check or uncheck the NodeXL, Data, Import, Clear NodeXL Workbook First checkbox to control this.
* The Closeness Centrality graph metric (NodeXL, Analysis, Graph Metrics) is now computed much more quickly.  For example, with a graph containing about 1,000 vertices and 1,000 edges, the computation time went from 31 seconds to 3 seconds, and with a larger graph containing 5,000 vertices and 8,000 edges, the time went from 63 minutes to 2 minutes.
* The graph legend is now hidden by default.
* Your settings for showing or hiding the graph legend and axes (NodeXL, Show/Hide, Graph Elements) are now saved along with the rest of your settings.
* If you autofill the X and Y columns in the Vertices worksheet (NodeXL, Visual Properties, Autofill Columns), the Locked column is no longer automatically set to Yes.  Instead, the Layout (NodeXL, Graph, Layout) is set to None, which achieves the same effect but is easier to undo.  If you no longer want the autofilled X and Y values, just set the Layout to something else.
* In the options dialog boxes within the Autofill Columns dialog box, there is now a "Swap" button that will quickly swap the colors or numbers you are autofilling.
* You can now change the font used for the graph axes.  In the graph pane, go to Options, Axis Font.
* The Auto Refresh checkbox that used to be in the NodeXL, Visual Properties Ribbon group is now in the Options dialog box, reachable from the graph pane.
* Bug fix: In the Import From Twitter Network feature, the screen name and password were not being correctly sent to Twitter.  This caused Twitter rate limiting to kick in even if your rate limit had been lifted by Twitter.

+1.0.1.89+ (2009/7/9)

* There are now X and Y axes in the graph pane.  To show them, check the NodeXL, Show/Hide, Graph Elements, Axes checkbox in the Excel Ribbon.  If you autofill the X and Y columns in the Vertices worksheet (NodeXL, Visual Properties, Autofill Columns), the axes will show the range of autofilled values.  Otherwise, the axes simply show NodeXL's full range of coordinate values (0 to 9,999).
* You can now export the NodeXL workbook's edges to a UCINET file.  The file format is what UCINET calls "full matrix DL."  Go to NodeXL, Data, Export, To UCINET Full Matrix DL File in the Ribbon.
* You can also import a UCINET full matrix DL file.  Go to NodeXL, Data, Import, From UCINET Full Matrix DL File in the Ribbon.  If you have a file in a different UCINET format, you will need to use the UCINET program to convert it to full matrix DL.  Click on the "What if my file is not in full matrix DL format?" link in the Import dialog box for instructions.
* You can now export the NodeXL workbook's edges to a Pajek file.  Go to NodeXL, Data, Export, To Pajek File in the Ribbon.  The vertex coordinates are exported, but no other edge or vertex attributes are.

+1.0.1.88+ (2009/6/18)

* A new graph layout algorithm is available.  In the Ribbon, select NodeXL, Graph, Layout, Harel-Koren Fast Multiscale.
* You can now save a graph image in a vector graphics format, in addition to the raster formats that have always been available.  Right-click the graph pane; select Save Image to File, Save Image; and select XPS in Save As Type.  XPS is Microsoft's vector file format, similar in concept to PDF.  (If you need a graph image in PDF format, save the image as an XPS, then use a third-party application to convert the XPS to a PDF.  Adobe Acrobat Pro can be used for this, and there are several free alternatives as well.)
* In the Import from Email Network dialog box (NodeXL, Data, Import, From Email Network), any email addresses you enter are now ORed together.  They were ANDed before.  This is explained in a new About Email Addresses link in the dialog box.
* In the Autofill Columns dialog box, the Vertex column now appears at the top of the lists for the Vertex Primary Label, Vertex Secondary Label, and Vertex ToolTip drop-downs.  It used to be at the bottom.
* Bug fix: Locked vertices are now honored when using the Sugiyama graph layout algorithm.
* Bug fix: The color dialog box (NodeXL, Visual Properties, Color and elsewhere) now remembers any custom colors you define within one Excel session.  (The dialog box's custom colors are lost when you close the workbook, however.)
* Bug fix: If you change the default font via the Options button in the graph pane, you can now use italic and bold fonts.  The italic and bold options were ignored before.
* Bug fix: When saving a graph image or copying it to the clipboard, the image may not have been properly centered, depending on how the graph was zoomed and dragged.
* Bug fix: When analyzing email, an "{"[InvalidCastException](InvalidCastException)"}: Row handle is invalid" error could occur if your mailbox contained mail with unrecognized To, From, CC, or BCC fields.  Such emails are now skipped.

+1.0.1.86+ (2009/6/4)

* There is now a legend at the bottom of the graph pane.  The legend displays any dynamic filters that are in effect, along with a description of any visual properties that have been set via Schemes or Autofill Columns.  To hide the legend, uncheck the NodeXL, Show/Hide, Graph Legend checkbox in the Ribbon.
* For table columns that were formatted as Text to prevent Excel from performing unwanted conversions, the Text formatting is now extended below the table to include the entire worksheet column.
* Additional columns are now formatted as Text: Cluster on the Clusters worksheet, and Cluster and Vertex on the Cluster Vertices worksheet.
* Several default settings for AutoFill columns were changed.  Outliers are no longer ignored, minimum Opacity is now 10 instead of 1, and minimum Vertex Size is now 1.5 instead of 1.0.
* Bug fix: The Schemes dialog box no longer closes after it displays an error message.
* Bug fix: Opening the Options dialog box could change the selected Layout under certain conditions.
* Bug fix: Running Find Clusters when there was a vertex named "5-1", for example, resulted in a vertex named "5/1/2009" being inserted into the Cluster Vertices worksheet, which broke the Find Clusters feature.  Excel has now been told in no uncertain terms that vertex names are _not_ to be converted to something else.

+1.0.1.85+ (2009/5/21)

* The Visual Properties group in the Node XL Ribbon tab has been expanded.  New buttons let you easily set a visual property, such as edge width or vertex color, and have the graph automatically refreshed.  If you have a large graph and automatically refreshing it takes a long time, you can turn off the automatic refreshes.
* Turning off automatic refreshes also affects the Schemes and Autofill Columns features.
* The Edit Selected Vertex Attributes feature (available when right-clicking a vertex in the graph pane) is now called Edit Selected Vertex Properties.
* The Import Open Edge Workbook dialog box (NodeXL, Data, Import, From Open Edge Workbook) now automatically selects the first two columns in the selected workbook.
* Workbook columns formatted as Text, such as the vertex names, now include a comment in the column header explaining that the format must be changed to General if you want to use Excel formulas in the column.  (Why are some columns formatted as Text?  To avoid unwanted automatic conversions performed by Excel on General cells.  Enter "1-1" in a General cell, for example, and it will turn into something like "Jan 1.")
* Bug fix: The Autofill Columns feature wouldn't autofill the vertex Radius column in old workbooks.  ("Radius" was changed to "Size" in version 1.0.1.81.)  Now, an old Radius column is changed to Size when the workbook is opened.

+1.0.1.84+ (2009/4/30)
* The Read Workbook button in the graph pane is now called Show Graph.  You can show or refresh the graph by clicking either Show Graph button -- the one in the graph pane or the one in the NodeXL Ribbon (NodeXL, Graph, Show Graph).
* When you apply a visual property scheme (NodeXL, Visual Properties, Schemes), the graph is automatically refreshed.
* When you autofill one or more columns (NodeXL, Visual Properties, Autofill Columns), the graph is automatically refreshed.
* You can now delete the subgraph image thumbnails created by NodeXL, Analysis, Subgraph Images.  Click the down-arrow on the Subgraph Images button and click Delete Thumbnails.
* Image file paths specified in the Images worksheet can now be either relative to the workbook's folder ("Images\Image.jpg") or absolute ("C:\MyImages\Image.jpg").  Prior to this change, an absolute path was required.
* Bug fix: Computing graph metrics on an empty workbook caused an error.  The error message started with "Assertion Failed at <SplitRange>d 0.MoveNext."
* Bug fix: Hiding the Other Columns column group (NodeXL, Show/Hide, Workbook Columns) didn't hide subgraph image thumbnails.
* Bug fix: Using one of the NodeXL, Data, Import items didn't delete subgraph image thumbnails.

+1.0.1.81+ (2009/4/24)
* The worksheet columns are now arranged in groups that can be hidden.  In the NodeXL Ribbon tab, go to Show/Hide, Workbook Columns.
* The column headers are now frozen and will remain in place as you scroll the worksheet.
* Insertion of your own columns within a NodeXL table is no longer recommended.  Add them to the right end of the table instead.  (Hover over "Add Your Own Columns Here" for details.)
* You can now apply a "scheme" of visual properties to your graph in one step.  For example, the Weighted Graph Scheme applies edge widths based on an edge weight column you specify.
* The Radius column on the Vertices worksheet is now called Size.  NodeXL will still read the Radius column in old workbooks, however.
* In the Autofill Columns dialog box, right-clicking Vertex X or Vertex Y and selecting "Clear Vertex X (or Y) Worksheet Column Now" will also clear the Locked column.  (Bet you didn't know you could do that, did you?)
* Bug fix: If you manually hid an Excel column and clicked Read Workbook, an error occurred.  The error message started with "{"[IndexOutOfRangeException](IndexOutOfRangeException)"}: Index was outside the bounds of the array."
* Bug fix: One of the images in the setup program was incorrect.
* New known bug: If you create subgraph images (NodeXL, Analysis, Subgraph Images) and then hide the "Other Columns" column group (NodeXL, Show/Hide, Workbook Columns), the Subgraph column on the Vertices worksheet will be hidden but the images won't.

+1.0.1.80+ (2009/4/5)
* There are no new features in this release, but it does introduce the first phase of the user interface redesign we're working on.   Please see [NodeXL Redesign](NodeXL-Redesign) for more information.

+1.0.1.79+ (2009/3/22)
* You can now import a graph that is stored  as an adjacency matrix in another Excel workbook.  In the Excel Ribbon, go to NodeXL, Graph, Import, From Open Matrix Workbook.
* You can also export a NodeXL graph to a new Excel workbook as an adjacency matrix.  Go to NodeXL, Graph, Export, To New Matrix Workbook.
* The term "Tie Strength" has been replaced with "Edge Weight."
* Bug fix: Certain sequences could lead to an assertion.  The assertion message started with "at ExcelUtil.TryGetSelectedTableRange(..."  Here is one such sequence: Use Excel's table filtering to filter the Edges table, select the Vertices worksheet, and click Read Worksheet.  Another such sequence: Filter the Edges table, select the Vertices worksheet, and click Calculate Graph Metrics.
* Bug fix: Vertex tooltips in the graph pane, which can be added to the graph with the Tooltips column in the Vertices worksheet, were partially obscured by the mouse cursor.

+1.0.1.78+ (2009/3/10)
* You can now read the workbook for the first time even if the Layout Type is set to None.  You'll still be notified that the results won't be what you want if the graph has never been laid out, but you won't be prevented from reading the workbook, which is what happened in the previous release.  You can turn off the notification if you don't want it.

+1.0.1.77+ (2009/3/8)
* The Autofill Columns dialog box, which is accessible via NodeXL, Graph, Autofill Columns, is now modeless.  That means it can stay on the screen while you work elsewhere within Excel.  Also, the specified columns get autofilled when you click a new Autofill button in the dialog box, not when you click Read Workbook in the graph pane, and a new right-click menu on the worksheet column names within the dialog box lets you immediately clear a column in the worksheet and perform other tasks.
* There is a new "None" option in the Layout Type drop-down in the graph pane.  This lets you edit vertex and edge attributes in the workbook, then read the workbook again without affecting the layout.
* There is a new Polar option in the Layout Type drop-down, and new Polar R and Polar Angle columns in the Vertices worksheet.  These let you place your vertices within a polar coordinate space.  Hover over the new column headers for details.
* A new Layout Order column in the Vertices worksheet lets you specify the vertex order when using the Circle, Spiral, Sine Wave, and Grid layouts.
* When you save a graph image to a file using the right-click menu in the graph pane, you can now set the image size.  Previously the saved image was always the same size as the graph pane, which is still an option.
* Vertex degree graph metrics no longer get inserted into the Edges worksheet.
* Bug fix: Using dynamic filters on a column that had an "#N/A" value caused an exception.  The exception message started with "{"[InvalidCastException](InvalidCastException)"}: Specified cast is not valid."
* Bug fix: When NodeXL inserted a column into a worksheet, the widths of the columns to the right were messed up.
* Bug fix: An extraneous input message was removed from the first empty cell in row 2 of the Vertices worksheet.
+1.0.1.76+ (2009/2/18)
* For new users, there are now no graph metrics selected by default, and you will get a message explaining how to select them the first time you attempt to calculate graph metrics.  This is to make the Select Graph Metrics dialog box more discoverable.
* In the Select Graph Metrics dialog box, some metrics are now marked as "slow."
* Each graph metric in the Select Graph Metrics dialog box now has a Details link that explains the metric and how it is calculated.
* The graph metrics are now divided into groups.
* Graph metrics now get inserted near the left edge of the worksheets.  They used to get appended to the right edge.
* For application developers, graph metrics are now available in a separate assembly.  They used to be part of the Excel Template code.
* Bug fix: Saving the graph image to the clipboard or a file via the right-click menu in the graph pane didn't work properly if the scale of the graph was changed.
* Bug fix: If an edge or vertex color was specified in the workbook, the opacity specified in the Options dialog was ignored.
* Bug fix: If a graph had a self-loop (an edge connecting a vertex to itself), switching to and from another workbook caused an error.  (The first line of the error message was "at WpfGraphicsUtil.GetFarthestRectangleEdge...")
+1.0.1.74+ (2009/2/13)
* Eigenvector centrality was added to the list of available graph metrics, which is accessible by going to NodeXL, Analysis in the Excel ribbon and clicking the down-arrow to the right of the Calculate Graph Metrics button.  Eigenvector centrality is defined at [http://en.wikipedia.org/wiki/Eigenvector_centrality#eigenvector_centrality](http://en.wikipedia.org/wiki/Eigenvector_centrality#eigenvector_centrality).  The accelerated power method is used to obtain the dominant eigenvector.
* Closeness centrality was also added to the list.  The closeness centrality of a vertex is the mean geodesic distance (shortest path) between it and all other vertices reachable from it.
+1.0.1.73+ (2009/2/10)
* Bug fix: With regional options in Windows set to Slovak (and some other languages as well), selecting vertices in the graph pane caused an exception.  The exception message started with "{"[COMException](COMException)"}: Exception from HRESULT: 0x800A03EC."
* The default strength of the repulsive force between vertices in the Layout Options dialog box (accessible from the Options dialog box) is now 3.0. (It was 6.0.)
+1.0.1.72+  (2009/2/8)
* Bug fix: If you attempted to open the NodeXL template on a machine whose regional options specified a comma instead of a period for the decimal character, an exception would occur.  The exception message was "The property 'LayoutUserSettings' could not be created from it's {"[sic](sic)"} default value. Error message: Input string was not in a correct format."
* The mouse wheel now zooms the graph more quickly.
+1.0.1.71+  (2009/2/3)
* You can now zoom into the graph and set the size of the canvas the graph is drawn on.  Click the "About Zoom and Scale" link in the graph pane for details.
* You can now move a group of vertices by selecting them and dragging any one of them with the mouse.  You used to be able to move only one vertex at a time.
* The Layout Type that you select in the graph pane is now saved in your options.
* The default dynamic filter opacity in the Options dialog box is now 0.  (It was 0.5.)  This will hide dynamically filtered vertices and edges.
* The default strength of the repulsive force between vertices in the Layout Options dialog box (accessible from the Options dialog box) is now 6.0.  (It was 1.0.)
* Bug fix: The Create Subgraph Images feature was always using the Fruchterman Reingold layout instead of the Layout Type selected in the graph pane.
* Bug fix: Directed edges that ended on an image didn't have an arrowhead.
* Bug fix: The Merge Duplicate Edges feature raised an assertion if a worksheet other than the Edges worksheet was active.
+1.0.1.68+  (2009/1/2)
* There are new options for controlling how the Fruchterman-Reingold layout algorithm works.  Click Options in the graph pane, then click the Layout button.
* Editing the settings in the Options dialog box no longer results in the graph being laid out again.  The edited settings still get applied to the graph when you click OK, but the vertices don't move.
+1.0.1.67+  (2008/12/28)
* You can now control the opacity of dynamically-filtered vertices and edges.  Click Options in the graph pane, then modify the Dynamic Filter Opacity setting.
+1.0.1.66+  (2008/12/17)
* Bug fix: If you used the Microsoft NodeXL, Excel Template shortcut on the Start menu, the shortcut would look for the NodeXL setup files and fail if they had been removed.
* Bug fix: A few remnants of our old product name were removed.
+1.0.1.65+  (2008/12/16)
* Bug fix: If you used the Vertex Shape option in AutoFill Columns to set the vertex shape to Solid Square, Solid Diamond, or Solid Triangle, you got an error when attempting to read the workbook into the graph.
* Bug fix: In AutoFill Columns, numbers you entered retained only one decimal place of precision.
* Bug fix: The dynamic filter for Clustering Coefficient didn't include any decimal places, so you could set the filter range only to 0 or 1.  This was true for some other dynamic filters as well.
* Bug fix: All references to nodexl@microsoft.com, which was an email address that didn't exist, were replaced with links to the CodePlex discussion page.
* When you register, the NodeXL version and current time are now registered along with your email address.
+1.0.1.64+  (2008/12/13)
* The first version with our new name, NodeXL.  We used to be called .NetMap.