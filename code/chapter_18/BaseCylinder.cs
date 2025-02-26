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

using PicoGK;
using System.Numerics;

namespace Coding4Engineers.Chapter18
{   
    class BaseCylinder
    {
        /// <summary>
        /// Constructs a cylinder with the specified radius and height
        /// </summary>
        /// <param name="frm">Position and orientation</param>
        /// <param name="fRadius">Radius of the cylinder</param>
        /// <param name="fHeight">Height of the cylinder</param>
        public BaseCylinder(    LocalFrame frm,
                                float fRadius,
                                float fHeight) : this(  frm, 
                                                        new Circle(fRadius), 
                                                        new Circle(fRadius), 
                                                        fHeight)
        {
        }

        public BaseCylinder(    LocalFrame frm, 
                                INormalizedContour2d oEdgeBottom, 
                                INormalizedContour2d oEdgeTop,
                                float fHeight)
        {
            m_frm       = frm;
            m_oEdgeBtm  = oEdgeBottom;
            m_oEdgeTop  = oEdgeTop;
            m_fHeight   = fHeight;   
        }

        public void SetSurfaceModulation(   ISurfaceModulation xMod, 
                                            float fHeight)
        {
            if (fHeight < 0)
                throw new ArgumentOutOfRangeException("Modulation height cannot be negative");

            m_xModulation       = xMod;
            m_fModulationHeight = fHeight;
        }

        public Mesh mshConstruct()
        {
            // Move the edges to top and bottom of the cylinder, respectively 
            LocalFrame frmTop = m_frm.frmTranslatedZ( m_fHeight/2);
            LocalFrame frmBtm = m_frm.frmTranslatedZ(-m_fHeight/2);

            // Position the 2D contours in space
            OrientedContour oTop = new(frmTop, m_oEdgeTop);
            OrientedContour oBtm = new(frmBtm, m_oEdgeBtm);

            // Calibrate fT, so the two contours start at the same position in space

            float fBtmCorr      = 0f;
            float fMinDist      = float.MaxValue;
            
            oBtm.PtAtT( 0f, 
                        out Vector3 vecBtmStart,
                        out _);

            // Adaptively calculate subdivisions from voxel size
            int nSubU = (int) float.Max(    m_oEdgeBtm.fLength / Library.fVoxelSizeMM,
                                            m_oEdgeTop.fLength / Library.fVoxelSizeMM);
        
            int nSubV = (int) (m_fHeight / Library.fVoxelSizeMM);

            nSubU += 2; // at least 2 subdivisions
            nSubV += 2; // at least 2 subdivisions

            for (int n=0; n < nSubU; n++)
            {
                float fU = (float) n / nSubU;

                oTop.PtAtT(fU, out Vector3 vec, out _);

                float fDist = Vector3.Distance(vecBtmStart, vec);

                if (fDist < fMinDist)
                {
                    fMinDist = fDist;
                    fBtmCorr = fU;
                }
            }

            Mesh msh = new();

            Vector3 vecBtmCenter = frmBtm.vecToWorld(Vector2.Zero);
            Vector3 vecTopCenter = frmTop.vecToWorld(Vector2.Zero);

            int nTopCenter = msh.nAddVertex(vecTopCenter);
            int nBtmCenter = msh.nAddVertex(vecBtmCenter);
            
            // Vertex index storage for the top and bottom edges
            int [] anVBtm  = anGenerateEdgeVertices(ref msh, oBtm, oTop, 0f, nSubU, fBtmCorr);
            int [] anVTop  = anGenerateEdgeVertices(ref msh, oBtm, oTop, 1f, nSubU, fBtmCorr);
            
            // Generate top and bottom mesh
            for (int u=0; u<nSubU; u++)
            {
                // Add bottom triangle
                msh.nAddTriangle(   anVBtm[(u+1) % nSubU],
                                    anVBtm[u], 
                                    nBtmCenter);

                // Add top triangle
                msh.nAddTriangle(   anVTop[u], 
                                    anVTop[(u+1) % nSubU], 
                                    nTopCenter);
            }

            int [] anPrvEdge = anVBtm;

            for (int v=1; v<=nSubV; v++)
            {
                int [] anCurEdge = (v==nSubV) ?
                                            anVTop : 
                                            anGenerateEdgeVertices( ref msh, 
                                                                    oBtm, oTop, 
                                                                    v/(float)nSubV, 
                                                                    nSubU, 
                                                                    fBtmCorr);
                for (int u=0; u<nSubU; u++)
                {
                    msh.AddQuad(    anPrvEdge[u],
                                    anPrvEdge[(u+1) % nSubU],
                                    anCurEdge[(u+1) % nSubU],
                                    anCurEdge[u]);
                } 

                anPrvEdge = anCurEdge;
            }
            
            return msh;
        }

        LocalFrame          m_frm;

        INormalizedContour2d m_oEdgeBtm;
        INormalizedContour2d m_oEdgeTop;

        float   m_fHeight;
        int     m_nUSubDiv = 0;
        int     m_nVSubDiv = 0;

        ISurfaceModulation  m_xModulation       = new SurfaceModulationNoop();
        float               m_fModulationHeight = 0f;

        private int [] anGenerateEdgeVertices(  ref Mesh msh,
                                                OrientedContour oBtm,
                                                OrientedContour oTop,
                                                float fV,
                                                int nSubDiv,
                                                float fBtmCorr)
        {
            int [] anIndices = new int[nSubDiv];

            // Generate the vertex indices for the specified V location

            for (int u=0; u<nSubDiv; u++)
            {
                float fUTop = u / (float) nSubDiv;
                float fUBtm = fUTop - fBtmCorr;
                if (fUBtm < 0f)
                    fUBtm += 1f;

                oBtm.PtAtT( in  fUBtm,
                            out Vector3 vecPtBtm,
                            out Vector3 vecNoBtm);

                oTop.PtAtT( in  fUTop,
                            out Vector3 vecPtTop,
                            out Vector3 vecNoTop);

                
                Vector3 vecCurPt = new Vector3(     float.Lerp(vecPtBtm.X, vecPtTop.X, fV),
                                                    float.Lerp(vecPtBtm.Y, vecPtTop.Y, fV),
                                                    float.Lerp(vecPtBtm.Z, vecPtTop.Z, fV));

                Vector3 vecCurNo = Vector3.Normalize(new Vector3(   float.Lerp(vecNoBtm.X, vecNoTop.X, fV),
                                                                    float.Lerp(vecNoBtm.Y, vecNoTop.Y, fV),
                                                                    float.Lerp(vecNoBtm.Z, vecNoTop.Z, fV)));

                float fU = u / (float) nSubDiv;

                float fOffset = m_xModulation.fOffset(fU, fV) * m_fModulationHeight;
                anIndices[u] = msh.nAddVertex(vecCurPt + vecCurNo * fOffset);
            }

            return anIndices;
        }
    }
}
