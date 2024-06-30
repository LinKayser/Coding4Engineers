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
    namespace Chapter08
    {
        class App
        {
            public static void Run()
            {
                Lattice lat         = new();     
                Vector3 vecPrevious = new(0,0,0);
                Random  oRand       = new();

                for (int n=0; n<100; n++)
                {
                    Vector3 vecNew = new(   oRand.NextSingle() * 100,
                                            oRand.NextSingle() * 100,
                                            oRand.NextSingle() * 100);
                        
                    lat.AddBeam(    vecPrevious,
                                    vecNew,
                                    1, 1, true);

                    vecPrevious = vecNew;
                }

                Voxels voxLat = new(lat);

                Lattice latSphere = new();
                latSphere.AddSphere(new(50,50,50), 40);
                Voxels voxSphere = new(latSphere);

                voxLat.BoolIntersect(voxSphere);

                Library.oViewer().Add(voxLat);
            }
        }
    }
}
