﻿//
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

namespace Coding4Engineers.Chapter18
{
    public static class Demo
    {
        public static void Run()
        {
            BaseCylinder oCyl = new(    LocalFrame.frmWorld, 
                                        new Circle(10), 
                                        new Ellipse(20f,10f),
                                        20f);

            oCyl.SetSurfaceModulation(new SurfaceModulationSineWaveUV(30,2), 1f);

            Mesh msh = oCyl.mshConstruct();

            Library.oViewer().SetGroupMaterial(1, "fb9696", 0.9f, 0.2f);
            Library.oViewer().Add(msh, 1);
        }
    }
}
