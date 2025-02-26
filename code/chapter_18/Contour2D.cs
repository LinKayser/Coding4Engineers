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

using System.Numerics;

namespace Coding4Engineers.Chapter18
{
    /// <summary>
    /// Interface to represent a normalized closed contour
    /// which travels from 0..1
    /// Postion at 0 and position at 1 are identical
    /// The path needs to be centered around the coordinate 0/0
    /// The contour must be in counter-clockwise winding order
    /// </summary>
    public interface INormalizedContour2d
    {
        public void PtAtT(  in  float t,
                            out Vector2 vecPt,
                            out Vector2 vecNormal);

        public float fLength {get;}
    }

    /// <summary>
    /// Encapsulates a 2D contour and positions it in space using a LocalFrame
    /// </summary>
    public class OrientedContour
    {
        /// <summary>
        /// Construct an oriented contour in space from a 2D contour
        /// </summary>
        /// <param name="xContour">2D contour</param>
        /// <param name="frm">Frame to use</param>
        public OrientedContour( LocalFrame frm,
                                INormalizedContour2d xContour)
        {
            m_xContour  = xContour;
            m_frm       = frm;
        }

        /// <summary>
        /// Returns the point and normal at the specified T (0..1)
        /// </summary>
        /// <param name="t">Position on contour 0..1 (0 and 1 point to the same position)</param>
        /// <param name="vecPt">Position on the contour</param>
        /// <param name="vecNormal">Outwards pointing normal</param>
        public void PtAtT(  in  float t,
                            out Vector3 vecPt,
                            out Vector3 vecNormal)
        {
            m_xContour.PtAtT(t, out Vector2 vecPt2d, out Vector2 vecN2d);
            vecPt       = m_frm.vecToWorld(vecPt2d);
            vecNormal   = m_frm.vecDirToWorld(vecN2d);
        }

        /// <summary>
        /// Total length of the path
        /// </summary>
        public float fLength => m_xContour.fLength;
        
        readonly LocalFrame             m_frm;
        readonly INormalizedContour2d   m_xContour;
    }

    /// <summary>
    /// Class to represent an circle as a normalized path/contour
    /// </summary>
    public class Circle : INormalizedContour2d
    {
        public Circle(float fR)
        {
            m_fR = fR;
        }

        public float fLength => float.Pi * 2 * m_fR;

        public void PtAtT(in float t, out Vector2 vecPt, out Vector2 vecNormal)
        {
            float fAngle = float.Clamp(t, 0, 1) * float.Pi * 2;
            
            vecPt =  new (  float.Cos(fAngle) * fR,
                            float.Sin(fAngle) * fR);

            vecNormal = Vector2.Normalize(vecPt);
        }

        public float            fR      => m_fR;

        readonly float          m_fR;
    }

    /// <summary>
    /// Class to represent an ellipse as a normalized path/contour
    /// </summary>
    public class Ellipse : INormalizedContour2d
    {
        /// <summary>
        /// Constructor using axis A vector and axis B length
        /// </summary>
        /// <param name="vecAxisA">Direction and length of axis A</param>
        /// <param name="fLengthB">Length of axis B (perpendicular to A)</param>
        /// <exception cref="ArgumentException"></exception>
        public Ellipse(Vector2 vecAxisA, float fLengthB)
        {
            float a = vecAxisA.Length();
            if (a == 0) throw new ArgumentException("Major axis cannot be zero.");
            
            float phi = (float)Math.Atan2(vecAxisA.Y, vecAxisA.X);
            m_fA = a;
            m_fB = fLengthB;
            m_fCosPhi = (float)Math.Cos(phi);
            m_fSinPhi = (float)Math.Sin(phi);
        }

        /// <summary>
        /// Constructor using semi-major axis length, semi-minor axis length, and rotation angle
        /// </summary>
        /// <param name="a">Length of axis A</param>
        /// <param name="b">Length of axis B</param>
        /// <param name="fAngle">Rotation angle in radians</param>
        public Ellipse(float a, float b, float fAngle=0)
        {
            m_fA = a;
            m_fB = b;
            m_fCosPhi = float.Cos(fAngle);
            m_fSinPhi = float.Sin(fAngle);
        }

        /// <summary>
        /// Approximated length of the ellipse
        /// </summary>
        public float fLength =>     float.Pi * (3 * (m_fA + m_fB) 
                                    - float.Sqrt((3 * m_fA + m_fB) 
                                    * (m_fA + 3 * m_fB)));

        public void PtAtT(in float t, out Vector2 vecPt, out Vector2 vecNormal)
        {
            float theta = float.Clamp(t, 0f, 1f) * 2f * float.Pi;
            float x = m_fA * float.Cos(theta);
            float y = m_fB * float.Sin(theta);
            
            // Position (same as before)
            vecPt = new Vector2( x * m_fCosPhi - y * m_fSinPhi, 
                                x * m_fSinPhi + y * m_fCosPhi);

            // Tangent components before rotation
            float txPre = -m_fA * float.Sin(theta); // dx/dθ
            float tyPre = m_fB * float.Cos(theta);  // dy/dθ

            // Rotate tangent
            float tx = txPre * m_fCosPhi - tyPre * m_fSinPhi;
            float ty = txPre * m_fSinPhi + tyPre * m_fCosPhi;

            // Normal (perpendicular to tangent, outward)
            vecNormal = -Vector2.Normalize(new Vector2(-ty, tx));
        }

        readonly float m_fA;       // axis A length
        readonly float m_fB;       // axis B length
        readonly float m_fCosPhi;  // Precomputed cos(phi)
        readonly float m_fSinPhi;  // Precomputed sin(phi)
    }
}