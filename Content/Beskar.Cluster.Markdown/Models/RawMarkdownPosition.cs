using System.Runtime.InteropServices;

namespace Beskar.Cluster.Markdown.Models;

[StructLayout(LayoutKind.Auto)]
public readonly record struct RawMarkdownPosition(
   int Index, int Length);