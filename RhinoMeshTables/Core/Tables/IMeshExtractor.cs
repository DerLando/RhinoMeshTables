using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public interface IMeshExtractor<out T>
    {
        T Mesh { get; }
        Face[] ExtractFaces();
        Vertex[] ExtractVertices();
        Edge[] ExtractEdges();
    }
}
