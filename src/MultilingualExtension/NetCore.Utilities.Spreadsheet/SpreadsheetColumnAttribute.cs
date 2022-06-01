﻿using System;

namespace ICG.NetCore.Utilities.Spreadsheet;

/// <summary>
///     Controls how a property is mapped to a spreadsheet column
/// </summary>
public class SpreadsheetColumnAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new
    /// </summary>
    /// <param name="displayName">
    ///     Sets the display name of the column. If not provided, will fall back on the DisplayName
    ///     attribute.
    /// </param>
    /// <param name="width">Sets the width of the column</param>
    /// <param name="ignore">If true, the column will be excluded from the spreadsheet</param>
    /// <param name="format">Sets the format of the column data</param>
    public SpreadsheetColumnAttribute(string displayName = null, float width = 0, bool ignore = false,
        string format = null)
    {
        DisplayName = displayName;
        Width = width;
        Ignore = ignore;
        Format = format;
    }

    /// <summary>
    ///     A custom name to use for the header when exported
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    ///     A custom width for the cell when exported
    /// </summary>
    /// <remarks>
    ///     If not set will default to Excel default of 10 if not auto-sized
    /// </remarks>
    public float Width { get; }

    /// <summary>
    ///     Should this column be ignored from export
    /// </summary>
    public bool Ignore { get; }

    /// <summary>
    ///     A custom format for te column
    /// </summary>
    public string Format { get; }
}