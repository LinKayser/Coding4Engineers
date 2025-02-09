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
        public class BaseBox
        {
            public BaseBox()
            {
            }

            public Mesh mshConstruct(   Surface.IModulation xFront,
                                        Surface.IModulation xBack,
                                        Surface.IModulation xTop,
                                        Surface.IModulation xBottom,
                                        Surface.IModulation xLeft,
                                        Surface.IModulation xRight,
                                        int nSubDivU = 0,
                                        int nSubDivV = 0)
            {
                List<Vector3> avec = new()
                {
                    new Vector3( 1, -1, -1), // 0: Front Bottom-Right
                    new Vector3(-1, -1, -1), // 1: Front Bottom-Left
                    new Vector3(-1, -1,  1), // 2: Front Top-Left
                    new Vector3( 1, -1,  1), // 3: Front Top-Right
                    new Vector3(-1,  1, -1), // 4: Back Bottom-Right
                    new Vector3( 1,  1, -1), // 5: Back Bottom-Left
                    new Vector3( 1,  1,  1), // 6: Back Top-Left
                    new Vector3(-1,  1,  1), // 7: Back Top-Right
                };

                Face oFront     = new(avec[0], avec[1], avec[2], avec[3]); 
                Face oBack      = new(avec[4], avec[5], avec[6], avec[7]);
                Face oRight     = new(avec[5], avec[0], avec[3], avec[6]);
                Face oLeft      = new(avec[1], avec[4], avec[7], avec[2]);
                Face oTop       = new(avec[7], avec[6], avec[3], avec[2]);
                Face oBottom    = new(avec[1], avec[0], avec[5], avec[4]);

                /*
                    oFront. Visualize(Library.oViewer(), true, true);
                    oBack.  Visualize(Library.oViewer(), true);
                    oRight. Visualize(Library.oViewer(), true);
                    oLeft.  Visualize(Library.oViewer(), true);
                    oTop.   Visualize(Library.oViewer(), true);
                    oBottom.Visualize(Library.oViewer(), true);
                */

                Mesh msh = new();
        
                Grid oGridFront     = new(oFront,   msh, nSubDivU, nSubDivV, xFront);
                Grid oGridBack      = new(oBack,    msh, nSubDivU, nSubDivV, xBack);
                Grid oGridRight     = new(oRight,   msh, nSubDivU, nSubDivV, xRight);
                Grid oGridLeft      = new(oLeft,    msh, nSubDivU, nSubDivV, xLeft);
                Grid oGridTop       = new(oTop,     msh, nSubDivU, nSubDivV, xTop);
                Grid oGridBottom    = new(oBottom,  msh, nSubDivU, nSubDivV, xBottom);

                oGridFront. ReplaceRightEdge(oGridRight.    anLeftEdge());
                oGridLeft.  ReplaceRightEdge(oGridFront.    anLeftEdge());
                oGridBack.  ReplaceRightEdge(oGridLeft.     anLeftEdge());
                oGridRight. ReplaceRightEdge(oGridBack.     anLeftEdge());

                oGridTop.ReplaceTopEdge     (oGridFront.    anTopEdge());
                oGridTop.ReplaceRightEdge   (oGridLeft.     anTopEdge());
                oGridTop.ReplaceBottomEdge  (oGridBack.     anTopEdge());
                oGridTop.ReplaceLeftEdge    (oGridRight.    anTopEdge());

                oGridBottom.ReplaceTopEdge     (oGridBack.  anBottomEdge());
                oGridBottom.ReplaceRightEdge   (oGridLeft.  anBottomEdge());
                oGridBottom.ReplaceBottomEdge  (oGridFront. anBottomEdge());
                oGridBottom.ReplaceLeftEdge    (oGridRight. anBottomEdge());

                oGridFront  .Construct();
                oGridBack   .Construct();
                oGridRight  .Construct();
                oGridLeft   .Construct();
                oGridTop    .Construct();
                oGridBottom .Construct();
                
                return msh;
            }
        }
    }
}