

using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterUsersNetworkGraphDataProvider
//
/// <summary>
/// Gets a network that shows the connections between a set of Twitter users
/// specified by either a Twitter List name or a set of Twitter screen names.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphDataAsTemporaryFile" /> to
/// get the network as a temporary GraphML file.
/// </remarks>
//*****************************************************************************

public class TwitterUsersNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: TwitterUsersNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterUsersNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterUsersNetworkGraphDataProvider()
    :
    base(GraphDataProviderName, "get the network of specified Twitter users")
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CreateDialog()
    //
    /// <summary>
    /// Creates a dialog for getting graph data.
    /// </summary>
    ///
    /// <returns>
    /// A dialog derived from GraphDataProviderDialogBase.
    /// </returns>
    //*************************************************************************

    protected override GraphDataProviderDialogBase
    CreateDialog()
    {
        AssertValid();

        return ( new TwitterGetUsersNetworkDialog() );
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
    //  Public constants
    //*************************************************************************

    /// Value of the Name property.

    public const String GraphDataProviderName =
        "Twitter Users Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
