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
    public static class App
    {
        public static void Run()
        {
            float fWingSpan = 11000; //mm
            
            Frame3d frmRight = Frame3d.frmFromZX(   new Vector3(0,  fWingSpan / 4, 0),  
                                                    Vector3.UnitY, 
                                                    Vector3.UnitZ);

            IContour2d xRoot  = new Naca4Contour("2412", 1570);
            IContour2d xTip   = new Naca4Contour("0009", 830);
            
            BaseCylinder oWingR = new(  frmRight, 
                                        xRoot, 
                                        xTip,
                                        fWingSpan / 2);

            Frame3d frmLeft  = Frame3d.frmFromZX(   new Vector3(0, -fWingSpan / 4, 0),  
                                                    Vector3.UnitY, 
                                                    Vector3.UnitZ);

            BaseCylinder oWingL = new(  frmLeft, 
                                        xTip,
                                        xRoot,
                                        fWingSpan / 2);

            Mesh mshR = oWingR.mshConstruct();
            Mesh mshL = oWingL.mshConstruct();
            
            Library.oViewer().SetGroupMaterial(1, "#994444EE", 0.1f, 0.9f);
            Library.oViewer().Add(mshR, 1);
            Library.oViewer().Add(mshL, 1);

            Frame3d frmViz = frmRight.frmMovedLocalZ(fWingSpan / 4);

            VisualizeContours.Viz(Library.oViewer(), "#00FF", frmViz, xTip, 0.008f);
        }
    }
}
