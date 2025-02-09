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

namespace Coding4Engineers
{
    namespace Chapter17
    {
        public static class Demo
        {
            public static void Run()
            {
                string strImagePath = Path.Combine(Utils.strProjectRootFolder(), "Assets/Fractal.tga");
                TgaIo.LoadTga(strImagePath, out Image img);

                Surface.IModulation xImg    = new Surface.ModulationImage(img);
                Surface.IModulation xTop    = new Surface.ModulationTrans(xImg, 0.02f);
                Surface.IModulation xBottom = new Surface.ModulationGauss();
                Surface.IModulation xLeft   = new Surface.ModulationRandom(0.001f);
                Surface.IModulation xRight  = new Surface.ModulationNoop();
                Surface.IModulation xBack   = xBottom;
                Surface.IModulation xFront  = xBottom;

                BaseBox oBox = new();
                Mesh msh = oBox.mshConstruct(   xTop:       xTop,
                                                xBottom:    xBottom,
                                                xFront:     xFront,
                                                xBack:      xBack,
                                                xLeft:      xLeft,
                                                xRight:     xRight,
                                                nSubDivU:   500, 
                                                nSubDivV:   500);

                Library.oViewer().SetGroupMaterial(1, "fb9607", 0.9f, 0.2f);
                Library.oViewer().Add(msh, 1);
            }
        }
    }
}
