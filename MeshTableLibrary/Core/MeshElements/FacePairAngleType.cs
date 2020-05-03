using System;
using System.Collections.Generic;
using System.Text;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// Different possible angles that can be defined between two faces
    /// </summary>
    public enum FacePairAngleType
    {
        /// <summary>
        /// The normals of the two faces form a sharp angle,
        /// if a ball would lie on the edge between the two faces,
        /// it would roll off
        /// </summary>
        Hill,

        /// <summary>
        /// The normals of the two faces are parallel,
        /// if a ball would lie on the edge between the two faces,
        /// it would not move
        /// </summary>
        Saddle,

        /// <summary>
        /// The normals of the two faces form a dull angle
        /// if a ball would be placed anywhere on one of the two faces,
        /// if would roll towards the shared edge
        /// </summary>
        Valley
    }
}
