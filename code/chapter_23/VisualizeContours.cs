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
using PicoGK.Shapes;

namespace Coding4Engineers.Chapter23
{
    public static class VisualizeContours
    {
        public static void Viz( Viewer oViewer,
                                ColorFloat clr,
                                Frame3d frm,
                                IContour2d xContour,
                                float fStep = 0.01f)
        {
            float fT = 0f;

            PolyLine poly = new(clr);

            while (true)
            {
                xContour.PtAtT(fT <= 1 ? fT : 0, out Vector2 vecPt, out Vector2 vecNormal);
            
                Vector3 vecPoint = frm.vecPtToWorld(vecPt);
                poly.nAddVertex(vecPoint);;
                poly.AddArrow(4);

                PolyLine polyNormal = new(clr);
                polyNormal.nAddVertex(vecPoint);
                polyNormal.nAddVertex(vecPoint + frm.vecDirToWorld(vecNormal) * 8);
                polyNormal.AddArrow(2);
                oViewer.Add(polyNormal);

                fT += fStep;

                if (fT > (1.0f + fStep))
                    break;
            }

            oViewer.Add(poly);

            PolyLine polyCenter = new(clr);
            polyCenter.nAddVertex(frm.vecPtToWorld(Vector2.Zero));
            polyCenter.AddCross(5);
            oViewer.Add(polyCenter);
        }
        public static void VizTwoConnected( Viewer oViewer,
                                            ColorFloat clr,
                                            Frame3d frm1,
                                            IContour2d x1,
                                            Frame3d frm2,
                                            IContour2d x2,
                                            float fStep = 0.01f)
        {
            Viz(oViewer, clr, frm1, x1, fStep);
            Viz(oViewer, clr, frm2, x2, fStep);

            float fT = 0f;
            while (fT <= 1)
            {
                x1.PtAtT(fT, out Vector2 vecPt1, out _);
                x2.PtAtT(fT, out Vector2 vecPt2, out _);
                
                PolyLine poly = new(clr);
                poly.nAddVertex(frm1.vecPtToWorld(vecPt1));
                poly.nAddVertex(frm2.vecPtToWorld(vecPt2));
                oViewer.Add(poly);
                fT += fStep;
            }
        }

        public static void VizMshPointCloud(    Viewer oViewer,
                                                ColorFloat clr,
                                                Mesh msh)
        {
            for (int n=0; n<msh.nVertexCount(); n++)
            {
                PolyLine poly = new(clr);
                poly.nAddVertex(msh.vecVertexAt(n));
                poly.AddCross(5);
                oViewer.Add(poly);
            }
        }
    }
}
