using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.Models
{
    public class ParentCloneGraph
    {
        public Dictionary<string, DatGameInfo> Games { get; set; } = new();
        public Dictionary<string, List<DatGameInfo>> ClonesByParent { get; set; } = new();
    
    public static ParentCloneGraph BuildParentCloneGraph(List<DatGameInfo> games)
        {
            var graph = new ParentCloneGraph();

            foreach (var g in games)
                graph.Games[g.Name] = g;

            foreach (var g in games)
            {
                if (!string.IsNullOrEmpty(g.CloneOf))
                {
                    if (!graph.ClonesByParent.ContainsKey(g.CloneOf))
                        graph.ClonesByParent[g.CloneOf] = new List<DatGameInfo>();

                    graph.ClonesByParent[g.CloneOf].Add(g);
                }
            }

            return graph;
        }

    }
}