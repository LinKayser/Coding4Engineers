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
using PicoGK.Shapes;

namespace Coding4Engineers.Chapter23
{
    public class Naca4Contour : IContour2d, ContourSampler2d.ISampleable
    {
        public Naca4Contour(    string strCode,
                                float fChordLength)
        {
            if (strCode.Length != 4)
                throw new ArgumentOutOfRangeException($"NACA 4 Code must be 4 characters ({strCode})");

            m_fMaxCamber    = float.Parse(strCode[0].ToString()) / 100f;
            m_fCamberPos    = float.Parse(strCode[1].ToString()) / 10f;
            m_fThickness    = float.Parse(strCode.Substring(2, 2)) / 100f;

            m_fChordLength  = fChordLength;

            m_oSampler = new(this);
        }

        public float fLength => m_oSampler.fTotalLength;

        public Vector2 vecPtAtT(float fT)
        {
            return vecPtAtTLinear(m_oSampler.fArcTFromLinearT(fT));
        }

        public Vector2 vecPtAtTLinear(float fT)
        {
            bool bUpper = fT <= 0.5f;

            float x = bUpper ? (1f - 2f * fT) : (2f * fT - 1f);

            float m = m_fMaxCamber;
            float p = m_fCamberPos;
            float t = m_fThickness;

            float yt = t /  0.2f * ( 0.2969f * float.Sqrt(x) -
                                     0.1260f * x -
                                     0.3516f * x * x +
                                     0.2843f * x * x * x -
                                     0.1015f * x * x * x * x);

            float yc;
            float dyc_dx;

            if (x < p)
            {
                yc = (m / (p * p)) * (2f * p * x - x * x);
                dyc_dx = (m / (p * p)) * (2f * p - 2f * x);
            }
            else
            {
                yc = (m / ((1f - p) * (1f - p))) * (1f - 2f * p + 2f * p * x - x * x);
                dyc_dx = (m / ((1f - p) * (1f - p))) * (2f * p - 2f * x);
            }

            float theta = float.Atan(dyc_dx);

            float xu = x - yt * float.Sin(theta);
            float yu = yc + yt * float.Cos(theta);
            float xl = x + yt * float.Sin(theta);
            float yl = yc - yt * float.Cos(theta);

            Vector2 vecPt = bUpper ? new Vector2(xu, yu) : new Vector2(xl, yl);
            
            // NACA defines leading edge at origin
            // geometrically, we want the origin
            // at the center
            vecPt.X -= 0.5f;

            // Now scale from normalized 0..1 pos to actual 
            // position of the wing profile for that chord length
            vecPt *= m_fChordLength;

            return vecPt;
        }

        ContourSampler2d m_oSampler;

        readonly float m_fChordLength;
        readonly float m_fMaxCamber;
        readonly float m_fCamberPos;
        readonly float m_fThickness;
    }
}