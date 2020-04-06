using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    /// <summary>
    /// Table which can be queried for relations between edges and faces
    /// </summary>
    public class EdgeFaceTable : TableBase
    {
        private Edge[] _edges;
        private Face[] _faces;
        private Dictionary<FaceIndex, List<EdgeIndex>> _fe_dict;

        public EdgeFaceTable(Edge[] edges, Face[] faces)
        {
            _edges = edges;
            _faces = faces;

            _fe_dict = CreateFaceEdgeDict();
        }

        private Dictionary<FaceIndex, List<EdgeIndex>> CreateFaceEdgeDict()
        {
            var fe_dict = new Dictionary<FaceIndex, List<EdgeIndex>>();

            for (int i = 0; i < _edges.Length; i++)
            {
                var e_index = new EdgeIndex((uint) i);
                for (int j = 0; j < _edges[i].FaceCount; j++)
                {
                    // FaceCount is 1 for naked edges and 2 for all other
                    var f_index = _edges[i].FaceIndices[j];
                    if (fe_dict.ContainsKey(f_index))
                    {
                        fe_dict[f_index].Add(e_index);
                    }
                    else
                    {
                        fe_dict[f_index] = new List<EdgeIndex> {e_index};
                    }
                }
            }

            return fe_dict;
        }

        #region Edge getters

        public EdgeIndex[] GetFaceIndices(FaceIndex key)
        {
            return _fe_dict[key].ToArray();
        }

        private Edge[] GetFaces(FaceIndex key)
        {
            return (from index in GetFaceIndices(key) select _edges[index.Value]).ToArray();
        }

        public Edge[] this[FaceIndex key]
        {
            get => GetFaces(key);
        }

        #endregion

        #region Face getters

        private FaceIndex[] GetFaceIndices(EdgeIndex key)
        {
            return _edges[key.Value].FaceIndices;
        }

        private Face[] GetFaces(EdgeIndex key)
        {
            return (from index in GetFaceIndices(key) select _faces[index.Value]).ToArray();
        }

        public Face[] this[EdgeIndex key]
        {
            get => GetFaces(key);
        }

        #endregion

        public int EdgeCount => _edges.Length;
        public int FaceCount => _faces.Length;
    }
}
