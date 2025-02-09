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
        public class Grid
        {
            public Grid(    Face oFace,
                            Mesh mshVertexStorage,
                            int nSubdivU,
                            int nSubdivV,
                            Surface.IModulation xModulation)
            {
                // Needs to have at least two edges
                if (nSubdivU < 2)
                    nSubdivU = 2;

                if (nSubdivV < 2)
                    nSubdivV = 2;

                m_msh = mshVertexStorage;
                m_anGrid = new int [nSubdivU,nSubdivV];

                for (int u = 0; u < nSubdivU; u++)
                {
                    for (int v = 0; v < nSubdivV; v++)
                    {
                        // Calculate normalized interpolation factors
                        float fTU = (float)u / (nSubdivU - 1); // Horizontal interpolation (0 to 1)
                        float fTV = (float)v / (nSubdivV - 1); // Vertical interpolation (0 to 1)

                        // Perform bilinear interpolation
                        // Interpolate along the top and bottom edges
                        Vector3 vecBottomEdge      = Vector3.Lerp(  oFace.vecBL, 
                                                                    oFace.vecBR,
                                                                    fTU);

                        Vector3 vecTopEdge   = Vector3.Lerp(    oFace.vecTL, 
                                                                oFace.vecTR, 
                                                                fTU);

                        
                        Vector3 vecInterpolated = Vector3.Lerp(vecBottomEdge, vecTopEdge, fTV);
                        vecInterpolated += xModulation.fHeight(new(fTU, fTV)) * oFace.vecN;

                         // Interpolate between the top and bottom edges
                        m_anGrid[u, v] = mshVertexStorage.nAddVertex(vecInterpolated);
                    }
                }
            }

            public int [] anBottomEdge()
            {
                // U direction
                int [] anEdge = new int [m_anGrid.GetLength(0)];

                for (int u=0;u<m_anGrid.GetLength(0);u++)
                {
                    anEdge[u] = m_anGrid[m_anGrid.GetLength(0)-u-1, 0];
                }

                return anEdge;
            }

            public int [] anTopEdge()
            {
                // U direction
                int [] anEdge = new int [m_anGrid.GetLength(0)];

                for (int u=0;u<m_anGrid.GetLength(0);u++)
                {
                    anEdge[u] = m_anGrid[u, m_anGrid.GetLength(1)-1];
                }

                return anEdge;
            }

            public int [] anRightEdge()
            {
                // U direction
                int [] anEdge = new int [m_anGrid.GetLength(1)];

                for (int v=0;v<m_anGrid.GetLength(1);v++)
                {
                    anEdge[v] = m_anGrid[   m_anGrid.GetLength(0)-1,
                                            v];
                }

                return anEdge;
            }

            public int [] anLeftEdge()
            {
                // U direction
                int [] anEdge = new int [m_anGrid.GetLength(1)];

                for (int v=0;v<m_anGrid.GetLength(1);v++)
                {
                    anEdge[v] = m_anGrid[   0,
                                            v];
                }

                return anEdge;
            }

            public void ReplaceTopEdge(int [] anEdge)
            {
                if (anEdge.Length != m_anGrid.GetLength(0))
                    throw new IndexOutOfRangeException();

                for (int u=0;u<m_anGrid.GetLength(0);u++)
                {
                     m_anGrid[  m_anGrid.GetLength(0)-u-1,
                                m_anGrid.GetLength(1)-1] = anEdge[u];
                }
            }

            public void ReplaceBottomEdge(int [] anEdge)
            {
                if (anEdge.Length != m_anGrid.GetLength(0))
                    throw new IndexOutOfRangeException();

                for (int u=0;u<m_anGrid.GetLength(0);u++)
                {
                     m_anGrid[u,0] = anEdge[u];
                }
            }

            public void ReplaceRightEdge(int [] anEdge)
            {
                if (anEdge.Length != m_anGrid.GetLength(1))
                    throw new IndexOutOfRangeException();

                for (int v=0;v<m_anGrid.GetLength(1);v++)
                {
                     m_anGrid[m_anGrid.GetLength(0)-1,v] = anEdge[v];
                }
            }

            public void ReplaceLeftEdge(int [] anEdge)
            {
                if (anEdge.Length != m_anGrid.GetLength(1))
                    throw new IndexOutOfRangeException();

                for (int v=0;v<m_anGrid.GetLength(1);v++)
                {
                     m_anGrid[0,m_anGrid.GetLength(1)-v-1] = anEdge[v];
                }
            }

            public void Construct()
            {
                for (int u = 0; u < m_anGrid.GetLength(0)-1; u++)
                {
                    for (int v = 0; v < m_anGrid.GetLength(1)-1;v++)
                    {
                        int n0 = m_anGrid[u, v];
                        int n1 = m_anGrid[u, v + 1]; 
                        int n2 = m_anGrid[u + 1, v + 1];
                        int n3 = m_anGrid[u + 1, v];

                        m_msh.AddQuad(n3, n2, n1, n0);
                    }
                }
            }

            int [,] m_anGrid;
            Mesh    m_msh;
        }
    }
}