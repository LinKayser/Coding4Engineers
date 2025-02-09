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
using PicoGK;

namespace Coding4Engineers
{
    namespace Chapter17
    {
        public class Face
        {
            public Face(	Vector3 vecBottomRight,
             		        Vector3 vecBottomLeft,
             		        Vector3 vecTopLeft,
                            Vector3 vecTopRight)
            {
                m_vecBottomLeft     = vecBottomLeft;
                m_vecBottomRight    = vecBottomRight;
                m_vecTopRight       = vecTopRight;
                m_vecTopLeft        = vecTopLeft;

                Vector3 u = vecBottomRight - vecBottomLeft;
                Vector3 v = vecTopRight - vecBottomRight;
                
                m_vecNormal = Vector3.Normalize(Vector3.Cross(u, v));
            }

            public void AddTo(Mesh msh, float fDistance = 0.0f)
            {
                msh.AddQuad(    m_vecBottomLeft + m_vecNormal * fDistance, 
                                m_vecBottomRight + m_vecNormal * fDistance, 
                                m_vecTopRight + m_vecNormal * fDistance, 
                                m_vecTopLeft + m_vecNormal * fDistance);
            }

            public void Visualize(  Viewer oViewer,
                                    bool bShowEdges = false,
                                    bool bHighlight = false,
                                    float fDistance = .3f)
            {
                Mesh msh = new();
                AddTo(msh, fDistance);

                ColorFloat clrFace = bHighlight ? new ("FF000033") :  new ("3333");
                int nGroup = bHighlight ? 999 : 1000;

                oViewer.SetGroupMaterial(nGroup, clrFace, 0.5f, 0.5f);
                oViewer.Add(msh, nGroup);

                if (bShowEdges)
                {
                    PolyLine oBottom = new("000000");
                    oBottom.nAddVertex(m_vecBottomRight + m_vecNormal * fDistance);
                    oBottom.nAddVertex(m_vecBottomLeft + m_vecNormal * fDistance);
                    oBottom.AddArrow(0.1f);
                    oViewer.Add(oBottom);

                    PolyLine oLeft = new("FF0000");
                    oLeft.nAddVertex(m_vecBottomLeft + m_vecNormal * fDistance);
                    oLeft.nAddVertex(m_vecTopLeft + m_vecNormal * fDistance);
                    oLeft.AddArrow(0.1f);
                    oViewer.Add(oLeft);

                    PolyLine oTop = new("0000FF");
                    oTop.nAddVertex(m_vecTopLeft + m_vecNormal * fDistance);
                    oTop.nAddVertex(m_vecTopRight + m_vecNormal * fDistance);
                    oTop.AddArrow(0.1f);
                    oViewer.Add(oTop);

                    PolyLine oRight = new("00FF00");
                    oRight.nAddVertex(m_vecTopRight + m_vecNormal * fDistance);
                    oRight.nAddVertex(m_vecBottomRight + m_vecNormal * fDistance);
                    oRight.AddArrow(0.1f);
                    oViewer.Add(oRight);

                    PolyLine oNormal = new("AAAA00");
                    oNormal.nAddVertex((m_vecBottomLeft + m_vecTopRight) / 2);
                    oNormal.nAddVertex((m_vecBottomLeft + m_vecTopRight) / 2 + m_vecNormal);
                    oNormal.AddArrow(0.1f);
                    oViewer.Add(oNormal);
                }
            }

            public Vector3 vecBL => m_vecBottomLeft;
            public Vector3 vecBR => m_vecBottomRight;
            public Vector3 vecTR => m_vecTopRight;
            public Vector3 vecTL => m_vecTopLeft;
            public Vector3 vecN => m_vecNormal;

            Vector3 m_vecBottomLeft;
            Vector3 m_vecBottomRight;
            Vector3 m_vecTopRight;
            Vector3 m_vecTopLeft;
            Vector3 m_vecNormal;
        }
    }
}