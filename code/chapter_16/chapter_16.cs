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
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using PicoGK;

namespace Coding4Engineers
{
    namespace Chapter16
    {
        public class QuadSubdivision
        {
            public static void RunSimple()
            {
                Vector3[] avecQuad = 
                {
                    new Vector3(-50.0f, -50.0f, 0.0f), // Bottom-left corner
                    new Vector3( 50.0f, -50.0f, 0.0f), // Bottom-right corner
                    new Vector3( 50.0f,  50.0f, 0.0f), // Top-right corner
                    new Vector3(-50.0f,  50.0f, 0.0f)  // Top-left corner
                };

                Vector3[,] avecGrid = avecSubdivideQuad(  20,20, 
                                                          avecQuad[0],
                                                          avecQuad[1],
                                                          avecQuad[2],
                                                          avecQuad[3]);

                Mesh msh = new();
                msh.AddQuad(    avecQuad[0],
                                avecQuad[1],
                                avecQuad[2],
                                avecQuad[3]);

                Library.oViewer().SetGroupMaterial(1, "AAAA", 0.5f, 0.5f);
                Library.oViewer().Add(msh, 1);

                for (int i=0; i<avecGrid.GetLength(0);i++)
                {
                    for (int j=0; j<avecGrid.GetLength(1);j++)
                    {
                        PolyLine poly = new("00FF00");
                        poly.nAddVertex(avecGrid[i,j]);
                        poly.AddCross();
                        Library.oViewer().Add(poly);
                    }
                }
            }

            public static void RunGauss()
            {
                Vector3[] avecQuad = 
                {
                    new Vector3(-50.0f, -50.0f, 0.0f), // Bottom-left corner
                    new Vector3( 50.0f, -50.0f, 0.0f), // Bottom-right corner
                    new Vector3( 50.0f,  50.0f, 0.0f), // Top-right corner
                    new Vector3(-50.0f,  50.0f, 0.0f)  // Top-left corner
                };

                Mesh msh = new();
                msh.AddQuad(    avecQuad[0],
                                avecQuad[1],
                                avecQuad[2],
                                avecQuad[3]);

                Vector3 vecNormal = Vector3.Normalize(
                                            Vector3.Cross(  avecQuad[1] - avecQuad[0], 
                                                            avecQuad[2] - avecQuad[0]));

                Library.oViewer().SetGroupMaterial(1, "22", 0.1f, 0.5f);
                Library.oViewer().Add(msh, 1);

                Vector3[,] avecGrid = avecSubdivideQuad(  30,30, 
                                                          avecQuad[0],
                                                          avecQuad[1],
                                                          avecQuad[2],
                                                          avecQuad[3]);
                
                ModulateGridGaussian(ref avecGrid, vecNormal);

                for (int i=0; i<avecGrid.GetLength(0);i++)
                {
                    PolyLine polyEdge = new("9999FF");
                    for (int j=0; j<avecGrid.GetLength(1);j++)
                    {
                        polyEdge.nAddVertex(avecGrid[i,j]);
                    }

                    Library.oViewer().Add(polyEdge);
                }

                for (int j=0; j<avecGrid.GetLength(1);j++)
                {
                    PolyLine polyEdge = new("99FF99");
                    for (int i=0; i<avecGrid.GetLength(0);i++)
                    {
                        polyEdge.nAddVertex(avecGrid[i,j]);
                    }

                    Library.oViewer().Add(polyEdge);
                }

                Mesh mshSub = new();
                int [,] anGrid = anStoreGridVertices(ref mshSub, avecGrid);
                AddGridQuadsToMesh(ref mshSub, anGrid);

                
                Library.oViewer().SetGroupMaterial(2, "FFAAAADD", 0.5f, 0.5f);
                Library.oViewer().Add(mshSub,2);
            }

            public static void RunImage()
            {
                Vector3[] avecQuad = 
                {
                    new Vector3(-50.0f, -50.0f, 0.0f), // Bottom-left corner
                    new Vector3( 50.0f, -50.0f, 0.0f), // Bottom-right corner
                    new Vector3( 50.0f,  50.0f, 0.0f), // Top-right corner
                    new Vector3(-50.0f,  50.0f, 0.0f)  // Top-left corner
                };

                Mesh msh = new();
                msh.AddQuad(    avecQuad[0],
                                avecQuad[1],
                                avecQuad[2],
                                avecQuad[3]);

                string strImagePath = Path.Combine(Utils.strProjectRootFolder(), "Assets/Fractal.tga");
                TgaIo.LoadTga(strImagePath, out Image img);

                Vector3[,] avecGrid = avecSubdivideQuad(  img.nWidth,img.nHeight, 
                                                          avecQuad[0],
                                                          avecQuad[1],
                                                          avecQuad[2],
                                                          avecQuad[3]);

                Vector3 vecNormal = Vector3.Normalize(
                                            Vector3.Cross(  avecQuad[1] - avecQuad[0], 
                                                            avecQuad[2] - avecQuad[0]));
                
                ModulateWithImage(ref avecGrid, in img, vecNormal);

                Mesh mshSub = new();
                int [,] anGrid = anStoreGridVertices(ref mshSub, avecGrid);
                AddGridQuadsToMesh(ref mshSub, anGrid);

                
                Library.oViewer().SetGroupMaterial(2, "fb9607", 0.9f, 0.2f);
                Library.oViewer().Add(mshSub,2);
            }

        
            public static Vector3[,] avecSubdivideQuad( int nSubDivX,
                            	                        int nSubDivY,
                                                        Vector3 vecA, 
                                                        Vector3 vecB, 
                                                        Vector3 vecC, 
                                                        Vector3 vecD)
            {
                Vector3 [,] avecResult = new Vector3 [nSubDivX,nSubDivY];

                for (int i = 0; i < nSubDivX; i++)
                {
                    for (int j = 0; j < nSubDivY; j++)
                    {
                        // Calculate normalized interpolation factors
                        float fTX = (float)i / (nSubDivX - 1); // Horizontal interpolation (0 to 1)
                        float fTY = (float)j / (nSubDivY - 1); // Vertical interpolation (0 to 1)

                        // Perform bilinear interpolation
                        // Interpolate along the top and bottom edges
                        Vector3 vecTopEdge      = Vector3.Lerp(vecA, vecB, fTX);
                        Vector3 vecBottomEdge   = Vector3.Lerp(vecD, vecC, fTX);

                        // Interpolate between the top and bottom edges
                        avecResult[i, j] = Vector3.Lerp(vecTopEdge, vecBottomEdge, fTY);
                    }
                }

                return avecResult;
            }

            public static void ModulateGridGaussian(    ref Vector3 [,] avecGrid,
                                                        Vector3 vecNormal)
            {
                int nSizeX = avecGrid.GetLength(0);
                int nSizeY = avecGrid.GetLength(1);

                // Intentionally not modulating the edges, to keep them as-is
                // so we are starting a 1 (not 0) and ending one before the actual end
                for (int x = 1; x < nSizeX-1; x++)
                {
                    for (int y = 1; y < nSizeY-1; y++)
                    {
                        // Calculate normalized interpolation factors
                        float fTX = (float)x / (nSizeX - 1); // Horizontal interpolation (0 to 1)
                        float fTY = (float)y / (nSizeY - 1); // Vertical interpolation (0 to 1)

                        avecGrid[x,y] += vecNormal * fGaussDistribution2D(fTX,fTY) * 50;
                    }
                }
            }

            public static float fGaussDistribution2D(   float x, 
                                                        float y, 
                                                        float fSigmaX = 0.15f, 
                                                        float fSigmaY = 0.15f)
            {
                float fExpX = float.Pow(x - 0.5f, 2) / (2 * float.Pow(fSigmaX, 2));
                float fExpY = float.Pow(y - 0.5f, 2) / (2 * float.Pow(fSigmaY, 2));
                return float.Exp(-(fExpX + fExpY));
            }

            public static int[,] anStoreGridVertices(ref Mesh msh, Vector3[,] avecGrid)
            {
                int[,] anVertexIndices = new int[avecGrid.GetLength(0), avecGrid.GetLength(1)];
                for (int x = 0; x < avecGrid.GetLength(0); x++)
                {
                    for (int y = 0; y < avecGrid.GetLength(1); y++)
                    {
                        anVertexIndices[x,y] = msh.nAddVertex(avecGrid[x,y]);
                    }
                }
                return anVertexIndices;
            }

            public static void AddGridQuadsToMesh(ref Mesh msh, int[,] anVertex)
            {
                for (int x = 0; x < anVertex.GetLength(0)-1; x++)
                {
                    for (int y = 0; y < anVertex.GetLength(1)-1; y++)
                    {
                        int n0 = anVertex[x, y];
                        int n1 = anVertex[x, y + 1]; 
                        int n2 = anVertex[x + 1, y + 1];
                        int n3 = anVertex[x + 1, y];
                        msh.AddQuad(n0, n3, n2, n1);
                    }
                }
            }

            public static void ModulateWithImage(   ref Vector3 [,] avecGrid,
                                                    in Image img,
                                                    Vector3 vecNormal)
            {
                int nSizeX = avecGrid.GetLength(0);
                int nSizeY = avecGrid.GetLength(1);

                // Intentionally not modulating the edges, to keep them as-is
                // so we are starting a 1 (not 0) and ending one before the actual end
                for (int x = 1; x < nSizeX-1; x++)
                {
                    for (int y = 1; y < nSizeY-1; y++)
                    {
                        // Calculate normalized interpolation factors
                        float fTX = 1.0f - ((float)x / (nSizeX - 1)); // Horizontal interpolation (0 to 1)
                        float fTY = (float)y / (nSizeY - 1); // Vertical interpolation (0 to 1)

                        avecGrid[x,y] += vecNormal * img.fValue(    (int) (img.nWidth * fTX + 0.5f),
                                                                    (int) (img.nHeight *fTY + 0.5f)) * 8f;
                    }
                }
            }
        }
    }
}
