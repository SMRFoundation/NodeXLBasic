# Complete NodeXL Release History

+1.0.1.388+  (2017-10-03)
* **NodeXL Pro:**
	* Export to Polinode feature uses the new API v.2 and fixed data type for numeric attributes.

+1.0.1.387+  (2017-09-27)
* **NodeXL Pro:**
	* New "Network Top Items" metric extends the "Twitter Search Network Top Items" metric to any kind of network.
	* Improved Graph Gallery reports for Facebook networks.

+1.0.1.386+  (2017-08-10)
* **NodeXL Pro:**
	* Fixed Export to Graph Gallery for Facebook Page-likes-Page edges.
	* Fixed guest uploads to Graph Gallery.

+1.0.1.385+  (2017-08-03)
* **NodeXL Pro:**
	* Added new "Page-like-Page" edge in Facebook Fan Page importer.
	* Added self-loop edges for posts/authors in Facebook Fan Page/Group importer.
	* Fixed UI in Facebook Fan Page/Group importer.
	* Fixed bug in Twitter importer.

+1.0.1.384+  (2017-06-16)
* **NodeXL Pro:**
	* Fixed "Operation has timed out" error when exporting large graphs to Polinode.

+1.0.1.383+  (2017-05-25)
* **NodeXL Pro:**
	* Added ability to import posts only from the Fan page owner or from other users too to the Facebook fan page importer.

+1.0.1.381+  (2017-05-23)
* **NodeXL Pro:**
	* Fixed bug for different regional settings than en-US

+1.0.1.381+  (2017-05-22)
* **NodeXL Pro:**
	* Added "Relationship creator" and comment column for Facebook post-post networks

+1.0.1.380+  (2017-05-02)
* **NodeXL Pro:**
	* Fixed TreeLayout algorithm for Paths.
	* Fixed Connected Components (Tarjan) algorithm StackOverflow exception when recursion too deep.
	* Updated Graph Pane default content.
* **NodeXL Basic**
	* Updated Graph Pane default content.

+1.0.1.379+  (2017-04-18)
* **NodeXL Pro:**
	* Fixes to Export to Polinode.

+1.0.1.378+  (2017-03-27)
* **NodeXL Pro:**
	* Fix for the TimeSeries not working when used in AutomationTools.

+1.0.1.376+  (2017-03-23)
* **NodeXL Pro:**
	* New "Path Sequence" column in the Paths worksheet which can be used as an X-coordinate for the nodes which will draw them in a tree-like layout.

+1.0.1.375+  (2017-03-16)
* **NodeXL Pro:**
	* New "Media in Tweet" and "Tweet Image File" columns for the Twitter networks, which hold the media URL (if exists).
	* New "Autoscale" feature which tries to automatically scale the graph depending on the number of vertices.
	* Bug fixes in Facebook login.
	* Bug fixes in "Export to PowerPoint".

+1.0.1.374+  (2017-03-07)
* **NodeXL Pro:**
	* New "Export to Polinode" feature.

+1.0.1.373+  (2017-02-22)
* **NodeXL Pro:**
	* Bug fixes.
	* Changes in the PowerPoint format.

+1.0.1.372+  (2017-02-15)
* **NodeXL Pro:**
	* Bug fixes.
	* Changed description to count only tweets from the initial data set to the time range calculation.

+1.0.1.371+  (2017-02-14)
* **NodeXL Pro:**
	* Added Prepare Data -> Find and Import Missing Tweets feature to import "reply-to" tweets which do not get imported by the search API.

+1.0.1.370+  (2017-02-06)
* **NodeXL Pro:**
	* Added "Path ID" and "Generation ID" to "Paths" worksheet to ease paths plotting. 

+1.0.1.369+  (2017-02-02)
* **NodeXL Pro:**
	* Big fixes in Paths generation.

+1.0.1.368+  (2017-01-30)
* **NodeXL Pro:**
	* Big fixes in "Export to PowerPoint" feature.

+1.0.1.367+  (2017-01-26)
* **NodeXL Pro:**
	* Added new "Export to PowerPoint" feature.

+1.0.1.366+  (2017-01-05)
* **NodeXL Pro:**
	* Fixed bug in "Paths" feature when names contain special characters.

+1.0.1.365+  (2016-12-29)
* **NodeXL Pro:**
	* Added new "Paths" feature which can be found under "Graph Metrics".

+1.0.1.364+  (2016-12-15)
* **NodeXL Pro:**
	* Performance improvement for "Export to Graph Gallery" feature.
	* Added "Extended Analysis" in Twitter importers.
	* Fixed duplicate "Follows" edges in Twitter User Importer.
	* Fixed edge type naming in Facebook importers.

+1.0.1.363+  (2016-11-24)
* **NodeXL Pro:**
	* Group information gets copied to "Vertices" and "Edges" sheets.
	* Added new "Count Edges by Type" feature in "Overall Metrics".
	* Added new Desktop shortcut to open multiple NodeXL instances.

+1.0.1.362+  (2016-10-29)
* **NodeXL Pro:**
	* Updated Facebook login dialog to the latest version (2.8).
	* Added the post creation date in the vertices worksheet when creating Post-Post network.

+1.0.1.361+  (2016-10-28)
* **NodeXL Pro:**
	* Added "NodeXL" to the chart title in Time Series worksheet.
	* Added zoom and scale NumericUpDown controls next to the sliders in the Document Actions.
* **NodeXL Basic:**
	* Added zoom and scale NumericUpDown controls next to the sliders in the Document Actions.


+1.0.1.360+  (2016-10-07)
* **NodeXL Pro:**
	* Bug fixes in the Facebook fan page importer.
	* Increased recent statuses limit to the Twitter User Network importer.
	* "Expand URLs" feature default to on in Twitter User Network Importer
	* Added ability to limit or not the number of friends/followers to request in both Twitter importers.

+1.0.1.359+  (2016-09-19)
* **NodeXL Pro:**
	* Bug fixes in the Twitter importers.

+1.0.1.358+  (2016-09-16)
* **NodeXL Pro:**
	* Added new edge attributes for the Twitter importers.
		* Favorited
		* Favorite Count
		* In-Reply-To User ID
		* Is Quote Status
		* Language
		* Possibly Sensitive
		* Quoted Status ID
		* Retweeted
		* Retweet Count
		* Retweet ID
		* Source
		* Truncated
		* Unified Twitter ID
		* Place Bounding Box
		* Place Country
		* Place Country Code
		* Place Full Name
		* Place ID
		* Place Name
		* Place Type
		* Place URL

+1.0.1.357+  (2016-09-09)
* **NodeXL Pro:**
	* Changed attribute types when exporting to GraphML:
		* Changed Degree, In-Degree, Out-Degree to int.
		* Changed centralities to double.

+1.0.1.356+  (2016-09-03)
* **NodeXL Pro:**
	* Improved Facebook Importer:
		* Updated to GraphAPI 2.7.
		* Reactions.
		* New share edge.
		* New user tagged edge.
		* Changed retry mechanism when an API limit is hit.
	* Bug fixes.

+1.0.1.355+  (2016-07-30)
* **NodeXL Pro:**
	* Switched to .NET Framework 4.5.
	* Fixed the date/time format for the Facebook importer.
	* Fixed the retry mechanism when the application/user is rate limited by Facebook
	* A NodeXL shortcut is added to the Desktop when installing.

+1.0.1.354+  (2016-06-08)
* **NodeXL Pro:**
	* Fixed bugs with Facebook importer

+1.0.1.353+  (2016-05-19)
* **NodeXL Pro:**
	* Added Graph Gallery support for Instagram Importer
	* Fixed bugs from version 352.

+1.0.1.352+  (2016-05-18)
* **NodeXL Pro:**
	* Fixed bugs from version 351.

+1.0.1.351+  (2016-05-18)
* **NodeXL Pro:**
	* Added gallery handlers for Instagram networks.
	* Added link to third-party Instagram importer [http://snatools.com/](http://snatools.com/).

+1.0.1.350+  (2016-01-26)
* **NodeXL Pro:**
	* Added Sentiment analysis.
	* Fixed Time Series bug in Office 2010.
	* Bug fixes when opening an already saved workbook.
* **NodeXL Basic:**
	* Added Sentiment analysis and Edge creation by content similarity features.
	* Bug fixes.

+1.0.1.349+  (2016-01-12)
* **NodeXL Pro:**
	* Added Time Series in Graph Metrics.

+1.0.1.348+  (2015-12-24)
* **NodeXL Pro:**
	* Small bug fixes
	* Added NodeXL Pro End User License Agreement to the "About " menu.

+1.0.1.347+  (2015-12-17)
* **NodeXL Pro:**
	* New Code Signing Certificate used for higher security.

+1.0.1.346+  (2015-12-07)
* **NodeXL Pro:**
	* Fixed YouTube Importers.
	* Fixed dependency problems fro .NET Framework 4.0
* **NodeXL Basic:**
	* New code signing certificate

+1.0.1.345+  (2015-11-30)
* **NodeXL Pro:**
	* Fixed YouTube Importers.
	* Added a Post-Deployment action to clear the ClickOnce cache when the application is uninstalled. This should fix the problem when switching from NodeXL Basic to NodeXL Pro and vice-versa.
* **NodeXL Basic:**
	* Added a Post-Deployment action to clear the ClickOnce cache when the application is uninstalled. This should fix the problem when switching from NodeXL Basic to NodeXL Pro and vice-versa.

+1.0.1.344+  (2015-10-23)
* **NodeXL Pro:**
	* New splash screen takes less time to close and shows information about the installed license.
	* Removed usage of ICMP to check for internet connection. This protocol is mainly blocked by firewalls so a lot of users reported the "You have reached the maximum allowed number of offline opens" error.
	* When the licenses is invalid or not found, a dialog box asking the user to choose a license will appear instead of just the error message.
* **NodeXL Basic:**
	* Removed usage of ICMP to check for internet connection.

+1.0.1.343+  (2015-10-12)
* As of this version NodeXL will be offered in two versions:
	* **NodeXL Pro** [http://www.smrfoundation.org/donation-guidance-how-to-support-the-social-media-research-foundation/](http://www.smrfoundation.org/donation-guidance-how-to-support-the-social-media-research-foundation/).
	* **NodeXL Basic** [ https://nodexl.codeplex.com/releases/view/117659]( https://nodexl.codeplex.com/releases/view/117659)

+1.0.1.342+  (2015-09-11)
* Bug and performance fixes.

+1.0.1.341+  (2015-08-05)
* Fixes in the edge creation of Facebook Fan Page/ Group importer. Post-Post networks now reflect posts rather than post authors. Removed usernames in "Comment" column for a better and non-biased content analysis.
* More "Export Options..." that will be included in the graph description. Now you can add your Logo and a URL to every network you export to Graph Gallery. For every entry in the description text you can attach a label and an action.

+1.0.1.340+  (2015-07-16)
* Added tooltips in the description for the overall metrics.

+1.0.1.339+  (2015-07-10)
* Fixed description bug in the automation.

+1.0.1.338+  (2015-07-09)
* New graph description for Twitter networks which  enable users to interact with the report on NodeXL Graph Gallery [ http://nodexlgraphgallery.org/Pages/Default.aspx]( http://nodexlgraphgallery.org/Pages/Default.aspx).
* Added "Export Options.." dialog so the user can add their own hashtag and URL to the "Smart Tweet" feature in NodeXL Graph Gallery [ http://nodexlgraphgallery.org/Pages/Default.aspx]( http://nodexlgraphgallery.org/Pages/Default.aspx).
* Enhanced splash screen with bug fixes.

+1.0.1.337+  (2015-06-15)
* Social Network Importer integrated into NodeXL.
* Minor changes in "Help" menu.
* Added splash screen when NodeXL is opened. It will close after 20 seconds or if the user hits "Esc" button or clicks "Cancel".

+1.0.1.336+  (2015-05-02)
* New feature: Limit edge creation by shared content similarity feature to isolate vertices. This feature is available in Graph Metrics dialog box.
* Changed the maximum file size allowed to be exported to Graph Gallery.

+1.0.1.335+  (2015-02-16)
* New feature: Edge creation by shared content similarity. You can have NodeXL create an edge based on the similarity of the content used by two vertices. This feature is available in Graph Metrics dialog box.
* Changed default options in "Import from Twitter Search Network".
* Removed "Import from NodeXL Graph Server" option. The graph data provider is now listed as a third party importer which can be downloaded [here](https://graphserverimporter.codeplex.com/).

+1.0.1.334+  (2014-10-29)
* Bug fix: If Twitter returned an empty stream the importer would fail with "Received an unexpected EOF or 0 bytes from the transport stream" error.
* Changed default options in "Import from Graph Server".

+1.0.1.333+  (2014-09-04)
* Bug fix, for programmers only: If the NodeXL solution folder was in a subfolder that included a space, as in "C:\Users\Ralph\Documents\Visual Studio 2013\Projects\NodeXL", for example, you would get two build errors that included the text "The command xcopy C:\Users\Ralph... exited with code 4."

+1.0.1.332+  (2014-08-03)
* Bug fix: If you attempted to import a network from Twitter or the NodeXL Graph Server and the network included a person with an attribute ("Location" or "Description", for example) that started with an equal sign, you would get an error message that included the text {""[COMException](COMException):"} Exception from HRESULT: 0x800A03EC."  This fix also applies to all text attributes imported from third-party importers.
* If a network or Twitter server glitch caused truncated text to be sent to NodeXL while a Twitter network was being imported, you would get an error message that included the text "{"[ArgumentException](ArgumentException):"} Unterminated string passed in."  Now, NodeXL attempts to get the text again.
* Bug fix: If you attempted to import a network from the Graph Server and the import took longer than two minutes, you would get the message "An unexpected problem occurred."  (This is a server-side change only.  You don't need a new version of NodeXL to make the problem go away.)
* Bug fix: If you specified a non-existent Twitter List in the Import from Twitter Users Network dialog box, you would get the message "There is no Twitter user with that username" instead of the correct message, which is "There is no such Twitter List."
* Bug fix: If you attempted to import a Flickr network, you would get a message that included the text "The remote server returned an error: (403) Forbidden."

+1.0.1.331+  (2014-06-25)
* If you select the "Basic network plus friends and followers" option in the Import from Twitter Users Network dialog box, each user's 2,000 newest friends and followers are imported.  That number used to be 1,000.
* When you import a network from the NodeXL Graph Server, you now specify a start date and either an end date or a maximum number of tweets working back in time from the start date.  Previously, it worked _forward_ in time from the start date.

+1.0.1.330+  (2014-06-24)
* Bug fix: If you attempted to import a network from the NodeXL Graph Server and your computer had .NET Framework 4.0 but not .NET Framework 4.5, you would get a message that included the text "InvalidDataContractException...Type 'System.Threading.Tasks.Task'...cannot be serialized."  (Note: NodeXL requires .NET Framework 4.0.  It does _not_ require .NET Framework 4.5.)

+1.0.1.329+  (2014-06-23)
* When you import a network from the NodeXL Graph Server, you now specify a start date and either an end date or a maximum number of tweets.

+1.0.1.328+  (2014-06-04)
* You can now specify different fonts for edge labels, vertex labels and group labels. Go to Graph Options, Other, Labels and click one of the three "Font" buttons. (In earlier versions, a single "Font" button set the font for both edge and vertex labels, and the font for group labels couldn't be changed.)
* Some options have been removed from the Import from Twitter Search Network dialog box to simplify it. You no longer have to check "add a Tweet column to the Edges worksheet", for example, because NodeXL will now automatically add a Tweet column. The only options remaining are those that significantly slow down the network, and those remain unchecked by default.
* For users of the advanced Network Server program in the NodeXL Automation Tools: In the configuration file for getting a Twitter search network, you no longer have to specify any of the following values in the "WhatToInclude" section, because they are now included by default: Statuses, Statistics, RepliesToEdges, MentionsEdges, and NonRepliesToNonMentionsEdges.  If you have old configuration files that specify these values, you do not have to edit them; the obsolete values will just be ignored.
* Also for users of the advanced Network Server program in the NodeXL Automation Tools: When the NodeXL Excel Template automatically updates itself with version 1.0.1.328, you will need to manually update the NodeXL Automation Tools to version 1.0.1.328.  It's available under "Other Downloads" on the CodePlex Downloads tab at http://nodexl.codeplex.com/releases.

+1.0.1.327+  (2014-05-27)
* In the Twitter Users Network (NodeXL, Data, Import, From Twitter Users Network), you can now specify how many recent tweets to analyze per user.  The available range is 1 to 200.
* In the Network Server program, the network configuration file for a Graph Server Twitter search network now specifies a "days including today" number instead of a start date and an end date.

+1.0.1.326+  (2014-05-19)
* Fixed NetworkServerStarter bug that arose when a network configuration file contained spaces.

+1.0.1.325+  (2014-05-19)
* Fixed build problem that required the solution to be built twice.

+1.0.1.324+  (2014-05-19)
* The NodeXL Network Server command-line program now has an option for getting a Twitter search network from the NodeXL Graph Server.
* The NodeXL Network Server command-line program will no longer create NodeXL workbooks; it will only create GraphML files. You can create NodeXL workbooks from GraphML files using the GraphML File Processor program in the NodeXL Automation Tools release.
* Bug fix: If you showed images in your graph, and there were thousands of them, and you set their size to anything other than their actual size, then NodeXL would behave badly when you attempted to show the graph.  Symptoms could include garbled, unresponsive windows and error messages that included the word "OutOfMemoryException."

+1.0.1.323+  (2014-04-28)
* The Twitter Users Network (NodeXL, Data, Import, From Twitter Users Network) now allows you to limit the network to the specified users. Previously, you would also get all the users who were replied to or mentioned by the specified users (and optionally all the friends and followers of the specified users), which often resulted in a huge network.

+1.0.1.322+  (2014-04-16)
* Bug fix: The suggested description in the Export to NodeXL Graph Gallery dialog box wasn't including the entire graph summary.

+1.0.1.321+  (2014-04-15)
* When you use NodeXL, Analysis, Subgraph Images, the Subgraph column now gets inserted into the second column of the Vertices worksheet instead of getting appended to the right edge of the worksheet.
* The suggested title in the Export to NodeXL Graph Gallery dialog box has been changed.  Ditto for the suggested subject in the Export to Email dialog box.

+1.0.1.320+  (2014-03-17)
* The Twitter User's Network and Twitter List Network (on the NodeXL, Data, Import menu) have been replaced by a new Twitter Users Network.  (The plural "Users" indicates that you can import information about multiple users.)  With the new network, you enter either a set of usernames or the name of a Twitter List, and NodeXL will analyze those users' 100 most recent tweets.  You can also import those users' friends and followers, but that option is highlighted with a warning that it can take a long time to do so.

+1.0.1.319+  (2014-02-21)
* Imported Twitter networks now have an "in-reply-to tweet ID" column.
* If you import a network from the NodeXL Graph Server (NodeXL, Data, Import, From NodeXL Graph Server), the graph summary (NodeXL, Graph, Summary) now specifies how many Twitter users there are in the network.
* When you import a Twitter Search Network (NodeXL, Data, Import, From Twitter Search Network), the graph description now specifies that some of the Twitter users in the network may have only been replied to or mentioned, as opposed to having tweeted the search term themselves.
* The maximum length of the search term in the Import from NodeXL Graph Server dialog box has been increased.

+1.0.1.318+  (2014-02-09)
* When you lay out each of the graph's groups in its own box, you can now select how the boxes are laid out.  Go to NodeXL, Graph, Layout, Layout Options in the Excel ribbon.  (Thanks to Cody Dunne for this feature.)
* The Check for Updates item has been removed from the Excel ribbon.  NodeXL now automatically checks for updates once a day.
* Bug fix: When grouping the graph's vertices by motif (NodeXL, Analysis, Groups, Group by Motif), the motif algorithm sometimes failed to find the largest motifs.  (Also Cody Dunne.)

+1.0.1.317+  (2014-01-27, private release)
* Once this release is installed, NodeXL will automatically update itself when a new release is available.  You will no longer have to manually download and install new releases.
* This release and those that follow will all be referred to as "NodeXL Excel Template 2014."  New releases will continue to have version numbers, but the numbers will be less important in light of the new auto-update feature.
* If you use third-party graph data importers, such as the Social Network Importer for NodeXL, note that the folder where the importers are stored has changed.  See "Using third-party graph data importers in NodeXL Excel Template 2014" at http://nodexl.codeplex.com/discussions/522826.
* If you use the NodeXL Network Server, an advanced command-line program that downloads a network from Twitter and stores the network on disk in several file formats, note that the program is no longer a part of NodeXL Excel Template.  See "Using the NodeXL Network Server command-line program with NodeXL Excel Template 2014" at http://nodexl.codeplex.com/discussions/522830.
* When a Twitter network is imported, the hashtags in the "Hashtags in Tweet" (or "Hashtags in Latest Tweet") column are now all in lower case.
* Bug fix: In some versions of Excel, the popup menu that appears when you right-click a cell in the Edges, Vertices and Groups worksheets did not include custom NodeXL menu items.
* Bug fix, for programmers only: If the NodeXL class libraries were used in an application that targeted .NET 4.0, the shaded rectangles behind vertex label annotations were drawn in the wrong place.
* Bug fix, for programmers only: You could not build the Release version of the NodeXL solution without first building the Debug version.

+1.0.1.251+  (2014-01-14)
* Bug fix: If you attempted to import a Twitter network on January 14, 2014 or later, you would get an error message that included the text "The remote server returned an error: (403) Forbidden."
* It now takes significantly less time to import a graph from the NodeXL Graph Server.  (This is a server-side change, so you don't need version 1.0.1.251 to notice the difference.)
* Graphs imported from the NodeXL Graph Server now include vertices for people who were replied to or mentioned by the people who tweeted the specified term but who didn't tweet the term themselves.  (Note 1: Collection of the additional vertices started on 2013-08-21.  Networks that span earlier dates _might_ include some additional vertices if they happen to already be in the collection database.  Note 2: This is a server-side change, so you don't need version 1.0.1.251 to notice the difference.)

 [Go to Page 2 >>](CompleteReleaseHistoryPage2)
