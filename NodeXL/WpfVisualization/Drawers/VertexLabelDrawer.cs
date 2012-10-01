
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexLabelDrawer
//
/// <summary>
/// Draws a vertex label as an annotation.
/// </summary>
///
/// <remarks>
/// This class draws a label next to a vertex, as an annotation.  It does NOT
/// draw vertices that have the shape <see cref="VertexShape.Label" />.
/// </remarks>
//*****************************************************************************

public class VertexLabelDrawer : DrawerBase
{
    //*************************************************************************
    //  Constructor: VertexLabelDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexLabelDrawer" />
    /// class.
    /// </summary>
    ///
    /// <param name="labelPosition">
    /// The default position to use for vertex labels.
    /// </param>
    ///
    /// <param name="backgroundAlpha">
    /// The alpha of the label's background rectangle, as a Byte.
    /// </param>
    //*************************************************************************

    public VertexLabelDrawer
    (
        VertexLabelPosition labelPosition,
        Byte backgroundAlpha
    )
    {
        m_eLabelPosition = labelPosition;
        m_btBackgroundAlpha = backgroundAlpha;

        AssertValid();
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws a vertex label as an annotation.
    /// </summary>
    ///
    /// <summary>
    /// Draws a vertex label as an annotation at a position determined by the
    /// vertex's metadata.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Describes how the vertex was drawn.
    /// </param>
    ///
    /// <param name="vertexBounds">
    /// The vertex's bounding rectangle.
    /// </param>
    ///
    /// <param name="formattedText">
    /// The FormattedText object to use.  Several properties get changed by
    /// this method.
    /// </param>
    ///
    /// <param name="formattedTextColor">
    /// The color of the FormattedText's foreground brush.
    /// </param>
    //*************************************************************************

    public void
    DrawLabel
    (
        DrawingContext drawingContext,
        GraphDrawingContext graphDrawingContext,
        VertexDrawingHistory vertexDrawingHistory,
        Rect vertexBounds,
        FormattedText formattedText,
        Color formattedTextColor
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(vertexDrawingHistory != null);
        Debug.Assert(formattedText != null);
        AssertValid();

        DrawLabel(drawingContext, graphDrawingContext, vertexDrawingHistory,
            vertexBounds, GetLabelPosition(vertexDrawingHistory.Vertex),
            formattedText, formattedTextColor, true);
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws a vertex label as an annotation at a specified position.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Describes how the vertex was drawn.
    /// </param>
    ///
    /// <param name="vertexBounds">
    /// The vertex's bounding rectangle.
    /// </param>
    ///
    /// <param name="labelPosition">
    /// The label's position.
    /// </param>
    ///
    /// <param name="formattedText">
    /// The FormattedText object to use.  Several properties get changed by
    /// this method.
    /// </param>
    ///
    /// <param name="formattedTextColor">
    /// The color of the FormattedText's foreground brush.
    /// </param>
    ///
    /// <param name="drawBackground">
    /// If true, a background is drawn behind the label text to make it more
    /// legible.
    /// </param>
    //*************************************************************************

    public void
    DrawLabel
    (
        DrawingContext drawingContext,
        GraphDrawingContext graphDrawingContext,
        VertexDrawingHistory vertexDrawingHistory,
        Rect vertexBounds,
        VertexLabelPosition labelPosition,
        FormattedText formattedText,
        Color formattedTextColor,
        Boolean drawBackground
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(vertexDrawingHistory != null);
        Debug.Assert(formattedText != null);
        AssertValid();

        if (labelPosition == VertexLabelPosition.Nowhere)
        {
            return;
        }

        // The alignment needs to be set before the width and height of the
        // FormattedText are obtained.

        SetTextAlignment(formattedText, labelPosition);

        // You can't use FormattedText.Width to get the width of the actual
        // text when text wrapping is enabled (FormattedText.MaxTextWidth > 0).
        // Instead, use a method that takes wrapping into account.

        Double dLabelWidth = WpfGraphicsUtil.GetFormattedTextSize(
            formattedText).Width;

        Double dLabelHeight = formattedText.Height;

        Double dHalfVertexBoundsWidth = vertexBounds.Width / 2.0;
        Double dHalfVertexBoundsHeight = vertexBounds.Height / 2.0;
        Double dHalfLabelHeight = dLabelHeight / 2.0;
        Double dHalfLabelWidth = dLabelWidth / 2.0;
        Double dMaxTextWidth = formattedText.MaxTextWidth;
        Double dHalfMaxTextWidth = dMaxTextWidth / 2.0;

        // This is the point where the label will be drawn.  It initially
        // assumes a text height of zero with no margin, but that will be
        // adjusted within the switch statement below.

        Point oDraw = vertexDrawingHistory.GetLabelLocation(labelPosition);
        Double dDrawX = oDraw.X;
        Double dDrawY = oDraw.Y;

        // These are the bounds of the label text.

        Double dLabelBoundsLeft = 0;
        Double dLabelBoundsRight = 0;

        // This is the adjustment that needs to be made to the label's
        // x-coordinate to account for the way FormattedText draws centered and
        // right-justified text.
        //
        // When wrapping is turned off (FormattedText.MaxTextWidth = 0) and
        // FormattedText.TextAlignment = TextAlignment.Center, for example,
        // the text gets centered horizontally at the specified drawing point.
        // When wrapping is turned on, however (FormattedText.MaxTextWidth
        // > 0), the text gets centered horizontally at the point halfway
        // between the specified drawing point and the MaxTextWidth value.

        Double dXAdjustment = 0;

        switch (labelPosition)
        {
            case VertexLabelPosition.TopLeft:

                dXAdjustment = -dMaxTextWidth;
                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.TopCenter:

                dXAdjustment = -dHalfMaxTextWidth;
                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.TopRight:

                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.MiddleLeft:

                dXAdjustment = -dMaxTextWidth;
                dDrawX -= HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.MiddleCenter:

                dXAdjustment = -dHalfMaxTextWidth;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.MiddleRight:

                dDrawX += HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.BottomLeft:

                dXAdjustment = -dMaxTextWidth;
                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.BottomCenter:

                dXAdjustment = -dHalfMaxTextWidth;
                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.BottomRight:

                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.Nowhere:

                // (This was handled at the top of the method.)

            default:

                Debug.Assert(false);
                break;
        }

        // Don't let the text exceed the bounds of the graph rectangle.

        Double dLabelBoundsTop = dDrawY;
        Double dLabelBoundsBottom = dDrawY + dLabelHeight;

        Rect oGraphRectangleMinusMargin =
            graphDrawingContext.GraphRectangleMinusMargin;

        dDrawX += Math.Max(0,
            oGraphRectangleMinusMargin.Left - dLabelBoundsLeft);

        dDrawX -= Math.Max(0,
            dLabelBoundsRight - oGraphRectangleMinusMargin.Right);

        dDrawY += Math.Max(0,
            oGraphRectangleMinusMargin.Top - dLabelBoundsTop);

        dDrawY -= Math.Max(0,
            dLabelBoundsBottom - oGraphRectangleMinusMargin.Bottom);

        Point oTextOrigin = new Point(dDrawX + dXAdjustment, dDrawY);

        if (drawBackground)
        {
            DrawLabelBackground(drawingContext, graphDrawingContext,
                formattedText, formattedTextColor, m_btBackgroundAlpha,
                oTextOrigin);
        }

        drawingContext.DrawText(formattedText, oTextOrigin);
    }

    //*************************************************************************
    //  Method: GetLabelPosition()
    //
    /// <summary>
    /// Gets the position of a vertex label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the label position for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's label position.
    /// </returns>
    //*************************************************************************

    protected VertexLabelPosition
    GetLabelPosition
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default position.

        VertexLabelPosition eLabelPosition = m_eLabelPosition;

        // Check for a per-vertex label position.

        Object oPerVertexLabelPositionAsObject;

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexLabelPosition,
            typeof(VertexLabelPosition), out oPerVertexLabelPositionAsObject) )
        {
            eLabelPosition =
                (VertexLabelPosition)oPerVertexLabelPositionAsObject;
        }

        return (eLabelPosition);
    }

    //*************************************************************************
    //  Method: SetTextAlignment()
    //
    /// <summary>
    /// Sets the TextAlignment property on a FormattedText object.
    /// </summary>
    ///
    /// <param name="oFormattedText">
    /// The FormattedText object to set the TextAlignment property on.
    /// </param>
    ///
    /// <param name="eLabelPosition">
    /// The label's position.  Can't be VertexLabelPosition.Nowhere.
    /// </param>
    //*************************************************************************

    protected void
    SetTextAlignment
    (
        FormattedText oFormattedText,
        VertexLabelPosition eLabelPosition
    )
    {
        Debug.Assert(oFormattedText != null);
        Debug.Assert(eLabelPosition != VertexLabelPosition.Nowhere);
        AssertValid();

        TextAlignment eTextAlignment = TextAlignment.Left;

        switch (eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:
            case VertexLabelPosition.MiddleLeft:
            case VertexLabelPosition.BottomLeft:

                eTextAlignment = TextAlignment.Right;
                break;

            case VertexLabelPosition.TopCenter:
            case VertexLabelPosition.MiddleCenter:
            case VertexLabelPosition.BottomCenter:

                eTextAlignment = TextAlignment.Center;
                break;

            case VertexLabelPosition.TopRight:
            case VertexLabelPosition.MiddleRight:
            case VertexLabelPosition.BottomRight:

                // eTextAlignment = TextAlignment.Left;
                break;

            case VertexLabelPosition.Nowhere:
            default:

                Debug.Assert(false);
                break;
        }

        oFormattedText.TextAlignment = eTextAlignment;
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

        // m_btBackgroundAlpha
        // m_eLabelPosition
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Horizontal margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double HorizontalMargin = 3;

    /// Vertical margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double VerticalMargin = 2;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Alpha of the label's background rectangle.

    protected Byte m_btBackgroundAlpha;

    /// Default position of vertex labels drawn as annotations.

    protected VertexLabelPosition m_eLabelPosition;
}
}
