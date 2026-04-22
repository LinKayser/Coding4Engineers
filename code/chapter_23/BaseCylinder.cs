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
using PicoGK.Shapes;
using System.Numerics;

namespace Coding4Engineers.Chapter23
{   
    class BaseCylinder
    {
        /// <summary>
        /// Constructs a cylinder with the specified radius and height
        /// </summary>
        /// <param name="frm">Position and orientation</param>
        /// <param name="fRadius">Radius of the cylinder</param>
        /// <param name="fHeight">Height of the cylinder</param>
        public BaseCylinder(    Frame3d frm,
                                float fRadius,
                                float fHeight) : this(  frm, 
                                                        new Circle(fRadius), 
                                                        new Circle(fRadius), 
                                                        fHeight)
        {
        }

        public BaseCylinder(    Frame3d frm, 
                                IContour2d oEdgeBottom, 
                                IContour2d oEdgeTop,
                                float fHeight)
        {
            m_frm       = frm;
            m_oEdgeBtm  = oEdgeBottom;
            m_oEdgeTop  = oEdgeTop;
            m_fHeight   = fHeight;   
        }

        public Mesh mshConstruct()
        {
            // Move the edges to top and bottom of the cylinder, respectively 
            Frame3d frmTop = m_frm.frmMovedLocalZ(m_fHeight/2);
            Frame3d frmBtm = m_frm.frmMovedLocalZ(-m_fHeight/2);

            //VisualizeContours.VizTwoConnected(Library.oViewer(), "#00FF00", frmTop, m_oEdgeTop, frmBtm, m_oEdgeBtm);

            // Position the 2D contours in space
            OrientedContour oTop = new(m_oEdgeTop, frmTop);
            OrientedContour oBtm = new(m_oEdgeBtm, frmBtm);

            // Adaptively calculate subdivisions from voxel size
            int nSubU = (int) float.Max(    m_oEdgeBtm.fLength / Library.fVoxelSizeMM,
                                            m_oEdgeTop.fLength / Library.fVoxelSizeMM);
        
            int nSubV = (int) (m_fHeight / Library.fVoxelSizeMM);

            nSubU += 2; // at least 2 subdivisions
            nSubV += 2; // at least 2 subdivisions

            Mesh msh = new();

            Vector3 vecBtmCenter = frmBtm.vecPtToWorld(Vector3.Zero);
            Vector3 vecTopCenter = frmTop.vecPtToWorld(Vector3.Zero);

            int nTopCenter = msh.nAddVertex(vecTopCenter);
            int nBtmCenter = msh.nAddVertex(vecBtmCenter);
            
            // Vertex index storage for the top and bottom edges
            int [] anVBtm  = anGenerateEdgeVertices(ref msh, oBtm, oTop, 0f, nSubU, 0);
            int [] anVTop  = anGenerateEdgeVertices(ref msh, oBtm, oTop, 1f, nSubU, 0);
            
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
                                                                    0);
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

        Frame3d m_frm;

        IContour2d m_oEdgeBtm;
        IContour2d m_oEdgeTop;

        float   m_fHeight;

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

                oBtm.PtAtT( fUBtm,
                            out Vector3 vecPtBtm,
                            out Vector3 vecNoBtm);

                oTop.PtAtT( fUTop,
                            out Vector3 vecPtTop,
                            out Vector3 vecNoTop);

                
                Vector3 vecCurPt = new Vector3(     float.Lerp(vecPtBtm.X, vecPtTop.X, fV),
                                                    float.Lerp(vecPtBtm.Y, vecPtTop.Y, fV),
                                                    float.Lerp(vecPtBtm.Z, vecPtTop.Z, fV));

                Vector3 vecCurNo = Vector3.Normalize(new Vector3(   float.Lerp(vecNoBtm.X, vecNoTop.X, fV),
                                                                    float.Lerp(vecNoBtm.Y, vecNoTop.Y, fV),
                                                                    float.Lerp(vecNoBtm.Z, vecNoTop.Z, fV)));

                float fU = u / (float) nSubDiv;

                anIndices[u] = msh.nAddVertex(vecCurPt + vecCurNo);
            }

            return anIndices;
        }
    }
}
