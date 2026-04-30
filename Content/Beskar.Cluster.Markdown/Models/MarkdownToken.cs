using System.Runtime.InteropServices;
using Beskar.Cluster.Markdown.Enums;

namespace Beskar.Cluster.Markdown.Models;

[StructLayout(LayoutKind.Auto)]
public readonly record struct MarkdownToken(
   MarkdownTokenType Type, RawMarkdownPosition ValuePostion);