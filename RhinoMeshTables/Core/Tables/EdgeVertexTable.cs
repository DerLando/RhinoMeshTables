﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public class EdgeVertexTable : TableBase
    {
        private VertexEdge[] _edges;
        private Vertex[] _vertices;
        private Dictionary<VertexIndex, List<EdgeIndex>> _ve_dict;

        public EdgeVertexTable(VertexEdge[] edges, Vertex[] vertices)
        {
            _edges = edges;
            _vertices = vertices;

            _ve_dict = CreateVertexEdgeDict();
        }

        private Dictionary<VertexIndex, List<EdgeIndex>> CreateVertexEdgeDict()
        {
            var ve_dict = new Dictionary<VertexIndex, List<EdgeIndex>>();
            for (int i = 0; i < _edges.Length; i++)
            {
                var e_index = new EdgeIndex((uint) i);
                for (int j = 0; j < 2; j++)
                {
                    var v_index = _edges[i].VertexIndices[j];
                    if (ve_dict.ContainsKey(v_index))
                    {
                        ve_dict[v_index].Add(e_index);
                    }
                    else
                    {
                        ve_dict[v_index] = new List<EdgeIndex> {e_index};
                    }
                }
            }

            return ve_dict;
        }

        #region Edge getters

        private EdgeIndex[] GetEdgeIndices(VertexIndex key)
        {
            return _ve_dict[key].ToArray();
        }

        private VertexEdge[] GetEdges(VertexIndex key)
        {
            return (from index in GetEdgeIndices(key) select _edges[index.Value]).ToArray();
        }

        public VertexEdge[] this[VertexIndex key]
        {
            get => GetEdges(key);
        }

        #endregion

        #region Vertex getters

        private VertexIndex[] GetVertexIndices(EdgeIndex key)
        {
            return _edges[key.Value].VertexIndices;
        }

        private Vertex[] GetVertices(EdgeIndex key)
        {
            return (from index in GetVertexIndices(key) select _vertices[index.Value]).ToArray();
        }

        public Vertex[] this[EdgeIndex key]
        {
            get => GetVertices(key);
        }

        #endregion

        public int EdgeCount => _edges.Length;
        public int VertexCount => _vertices.Length;
    }
}