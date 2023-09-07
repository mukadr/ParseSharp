using System;

namespace ParseSharp;

public static partial class Parser
{
    public static Parser<string> Match(char c) =>
        new(input => input.Match(c, c));

    public static Parser<string> Match(char first, char last) =>
        new(input => input.Match(first, last));

    public static Parser<string> Match(string s, StringComparison comparisonType = StringComparison.Ordinal) =>
        new(input => input.Match(s, comparisonType));
}