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
    namespace Chapter15
    {
        public class BoxShape
        {
            public static void Run()
            {
                Vector3[] avec = 
                {
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(10.0f, 0.0f, 0.0f),
                    new Vector3(10.0f, 10.0f, 0.0f),
                    new Vector3(0.0f, 10.0f, 0.0f),
                    new Vector3(0.0f, 0.0f, 15.0f),
                    new Vector3(10.0f, 0.0f, 15.0f),
                    new Vector3(10.0f, 10.0f, 15.0f),
                    new Vector3(0.0f, 10.0f, 15.0f)
                };

                Mesh msh = new();
                msh.AddVertices(avec, out int[] anV);

                msh.AddQuad(anV[3], anV[2], anV[1], anV[0]);
                msh.AddQuad(anV[4], anV[5], anV[6], anV[7]);
                msh.AddQuad(anV[1], anV[5], anV[4], anV[0]);
                msh.AddQuad(anV[7], anV[6], anV[2], anV[3]);
                msh.AddQuad(anV[4], anV[7], anV[3], anV[0]);
                msh.AddQuad(anV[2], anV[6], anV[5], anV[1]);
      
                PolyLine oLine = new("FF0000");
                oLine.nAddVertex(avec[0]);
                oLine.nAddVertex(avec[3]);
                oLine.AddArrow();
                oLine.nAddVertex(avec[2]);
                oLine.AddArrow();
                oLine.nAddVertex(avec[1]);
                oLine.AddArrow();
                oLine.nAddVertex(avec[0]);
                oLine.AddArrow();

                Library.oViewer().SetGroupMaterial(1, "AA77", 0.5f, 0.5f);
                Library.oViewer().Add(oLine,0);
                Library.oViewer().Add(msh,1);
            }
        }

        public class BoxPyramid
        {
            public static void Run()
            {
                Vector3[] avec = 
                {
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(10.0f, 0.0f, 0.0f),
                    new Vector3(10.0f, 10.0f, 0.0f),
                    new Vector3(0.0f, 10.0f, 0.0f),
                    new Vector3(0.0f, 0.0f, 15.0f),
                    new Vector3(10.0f, 0.0f, 15.0f),
                    new Vector3(10.0f, 10.0f, 15.0f),
                    new Vector3(0.0f, 10.0f, 15.0f)
                };

                Mesh msh = new();
                msh.AddVertices(avec, out int[] anV);

                msh.AddQuad(anV[3], anV[2], anV[1], anV[0]);
                //msh.AddQuad(anV[4], anV[5], anV[6], anV[7]); // skip top
                msh.AddQuad(anV[1], anV[5], anV[4], anV[0]);
                msh.AddQuad(anV[7], anV[6], anV[2], anV[3]);
                msh.AddQuad(anV[4], anV[7], anV[3], anV[0]);
                msh.AddQuad(anV[2], anV[6], anV[5], anV[1]);

                Vector3 vecMidPoint = new
                (
                    (avec[4].X + avec[5].X + avec[6].X + avec[7].X) / 4,
                    (avec[4].Y + avec[5].Y + avec[6].Y + avec[7].Y) / 4,
                    (avec[4].Z + avec[5].Z + avec[6].Z + avec[7].Z) / 4
                );

                vecMidPoint.Z += 5;

                int nMidVertex = msh.nAddVertex(vecMidPoint);

                msh.nAddTriangle(anV[4], anV[5], nMidVertex);
                msh.nAddTriangle(anV[5], anV[6], nMidVertex);
                msh.nAddTriangle(anV[6], anV[7], nMidVertex);
                msh.nAddTriangle(anV[7], anV[4], nMidVertex);

                Library.oViewer().SetGroupMaterial(1, "AA5555", 0.5f, 0.8f);
                Library.oViewer().Add(msh,1);
            }
        }

        public class BaseBox
        {
            public BaseBox()
            {
            }

            public BaseBox(Vector3 vecSize)
            {
                matTransform = Matrix4x4.CreateScale(vecSize / 2);
            }

            public Mesh mshConstruct()
            {
                Vector3[] avec = 
                {
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3( 1.0f, -1.0f, -1.0f),
                    new Vector3( 1.0f,  1.0f, -1.0f),
                    new Vector3(-1.0f,  1.0f, -1.0f),
                    new Vector3(-1.0f, -1.0f,  1.0f),
                    new Vector3( 1.0f, -1.0f,  1.0f),
                    new Vector3( 1.0f,  1.0f,  1.0f),
                    new Vector3(-1.0f,  1.0f,  1.0f)
                };

                Mesh msh = new();
                int[] anV = new int[avec.Count()];
                
                int n=0;
                foreach (Vector3 vec in avec)
                {
                    anV[n] = msh.nAddVertex(Vector3.Transform(vec, matTransform));
                    n++;
                }

                msh.AddQuad(anV[3], anV[2], anV[1], anV[0]);
                msh.AddQuad(anV[4], anV[5], anV[6], anV[7]);
                msh.AddQuad(anV[1], anV[5], anV[4], anV[0]);
                msh.AddQuad(anV[7], anV[6], anV[2], anV[3]);
                msh.AddQuad(anV[4], anV[7], anV[3], anV[0]);
                msh.AddQuad(anV[2], anV[6], anV[5], anV[1]);

                return msh;
            }

            public Matrix4x4 matTransform {get;set;}
        }

        public class ShowTransformedBox
        {
            public static void Run()
            {
                BaseBox oBox = new(new Vector3(10,10,15));

                Library.oViewer().Add(oBox.mshConstruct());
            }
        }

        public class ShowBoxesInCircles
        {
            public static void Run()
            {
                BaseBox oBox = new(new Vector3(10,10,15));

                int nNumber     = 30;
                float fRadius   = 80;
                float fAngle    = 2 * float.Pi / nNumber;
                Vector3 vecP    = Vector3.Zero;
                Matrix4x4 matO  = oBox.matTransform;

                for (int n=0; n<nNumber; n++)
                {
                    vecP.X = fRadius * float.Sin(fAngle * n);
                    vecP.Y = fRadius * float.Cos(fAngle * n);

                    Vector3 vecLookAtPoint = new Vector3(0,0,100);
                    Vector3 vecDir = Vector3.Normalize(vecLookAtPoint - vecP);
                    
                    Matrix4x4 matWorld = Matrix4x4.CreateWorld( position:   vecP, 
                                                                forward:    vecDir, 
                                                                up:         Vector3.UnitZ);
                    
                    oBox.matTransform = matO * matWorld;
                    Library.oViewer().Add(oBox.mshConstruct());
                }
            }
        }
    }
}
