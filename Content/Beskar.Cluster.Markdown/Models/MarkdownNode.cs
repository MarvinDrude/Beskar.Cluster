using System.Runtime.InteropServices;
using Beskar.Cluster.Markdown.Enums;

namespace Beskar.Cluster.Markdown.Models;

[StructLayout(LayoutKind.Auto)]
public struct MarkdownNode
{
   public MarkdownTokenType Type;
   public RawMarkdownPosition Position;
   
   public int FirstChildIndex;
   public int NextSiblingIndex;
}