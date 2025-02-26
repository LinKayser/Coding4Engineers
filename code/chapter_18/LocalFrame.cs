//
// SPDX-License-Identifier: CC0-1.0
//
// This example code file is released to the public under Creative Commons CC0.
// See https://creativecommons.org/publicdomain/zero/1.0/legalcode
//
// To the extent possible under law, the author has waived all copyright and
// related or neighboring rights to this example code file.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System.Diagnostics;
using System.Numerics;

namespace Coding4Engineers.Chapter18
{
    /// <summary>
    /// The LocalFrame object encapsulates a local reference 
    /// coordinate system
    /// </summary>
    public class LocalFrame
    {
        /// <summary>
        /// Constant for non-transformed world coordinates
        /// Origin at 0/0/0 and X,Y,Z axes
        /// </summary>
        public static LocalFrame frmWorld = new(Vector3.Zero);

        /// <summary>
        /// Creates a local frame at the specified
        /// position, with X,Y,Z aligned with world
        /// axes
        /// </summary>
        /// <param name="vecPos">Origin of frame</param>
        public LocalFrame(Vector3 vecPos)
        {
            m_vecPosition   = vecPos;
            m_vecLocalZ     = Vector3.UnitZ;
            m_vecLocalX     = Vector3.UnitX;
            m_vecLocalY     = Vector3.UnitY;

            Debug.Assert(vecLy == Vector3.Cross(vecLz, vecLx));
        }

        /// <summary>
        /// Creates a copy of the specified frame
        /// </summary>
        /// <param name="frmFrame">Frame to copy from</param>
        public LocalFrame(LocalFrame frm)
        {
            m_vecPosition   = frm.vecPos;
            m_vecLocalZ     = frm.vecLz;
            m_vecLocalX     = frm.vecLx;
            m_vecLocalY     = frm.vecLy;

            Debug.Assert(vecLy == Vector3.Cross(vecLz, vecLx));
        }

        /// <summary>
        /// Creates a local frame with the same
        /// coordinates as the specified frame
        /// but at a new position
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="vecNewPos"></param>
        public LocalFrame(  LocalFrame frm,
                            Vector3 vecNewPos)
        {
            m_vecPosition   = vecNewPos;
            m_vecLocalZ     = frm.vecLz;
            m_vecLocalX     = frm.vecLx;
            m_vecLocalY     = frm.vecLy;
        }

        /// <summary>
        /// Create a local frame at the specified
        /// position with the specified Z and X axis directions
        /// The Y is calculated using the right hand rule
        /// </summary>
        /// <param name="vecPos"></param>
        /// <param name="vecLocalZ"></param>
        /// <param name="vecLocalX"></param>
        public LocalFrame(  Vector3 vecPos,
                            Vector3 vecLocalZ,
                            Vector3 vecLocalX)
        {
            m_vecPosition   = vecPos;
            m_vecLocalZ     = Vector3.Normalize(vecLocalZ);
            m_vecLocalX     = Vector3.Normalize(vecLocalX);
            m_vecLocalY     = Vector3.Cross(m_vecLocalZ, m_vecLocalX);
        }

        /// <summary>
        /// Create a LocalFrame that is moved by the specified distance
        /// along the local Z axis
        /// </summary>
        /// <param name="fDistance"></param>
        /// <returns></returns>
        public LocalFrame frmTranslatedZ(float fDistance)
        {
           return new(  vecPos + vecLz * fDistance,
                        vecLz,
                        vecLx);   
        }

        /// <summary>
        /// Transforms a point defined in the local frame
        /// coordinate system into the world coordinate system
        /// </summary>
        /// <param name="vecLocal"></param>
        /// <returns></returns>
        public Vector3 vecToWorld(Vector3 vecPoint)
        {
            return  vecPoint.X * vecLx + 
                    vecPoint.Y * vecLy + 
                    vecPoint.Z * vecLz + 
                    vecPos;
        }

        /// <summary>
        /// Transforms a 3D direction defined in the local frame
        /// coordinate system into the world coordinate system
        /// </summary>
        /// <param name="vecDir"></param>
        /// <returns></returns>
        public Vector3 vecDirToWorld(Vector3 vecDir)
        {
            return Vector3.Normalize(   vecDir.X * vecLx + 
                                        vecDir.Y * vecLy +
                                        vecDir.Z * vecLz);
        }

        /// <summary>
        /// Transforms a 2D point defined in the local frame
        /// coordinate system into the world coordinate system
        /// </summary>
        /// <param name="vecLocal"></param>
        /// <returns></returns>
        public Vector3 vecToWorld(Vector2 vecPoint)
        {
            return  vecPoint.X * vecLx + 
                    vecPoint.Y * vecLy + 
                    vecPos;
        }

        /// <summary>
        /// Transforms a 2D point defined in the local frame
        /// coordinate system into the world coordinate system
        /// </summary>
        /// <param name="vecLocal"></param>
        /// <returns></returns>
        public Vector3 vecDirToWorld(Vector2 vecDir)
        {
            return Vector3.Normalize(vecDir.X * vecLx + vecDir.Y * vecLy);
        }
        
        public Vector3 vecPos   => m_vecPosition;
        public Vector3 vecLx    => m_vecLocalX;
        public Vector3 vecLy    => m_vecLocalY;
        public Vector3 vecLz    => m_vecLocalZ;

        readonly Vector3 m_vecPosition;
        readonly Vector3 m_vecLocalX;
        readonly Vector3 m_vecLocalY;
        readonly Vector3 m_vecLocalZ;
    }
}