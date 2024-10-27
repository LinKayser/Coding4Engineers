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
    namespace Chapter13
    {
        public class MakeBeams
        {
            public static void Run()
            {
                Lattice lat = new();

                lat.AddBeam(Vector3.Zero, new(100,0,0), 10, 10);
                lat.AddBeam(new(100,0,0), new(100,100,0), 10, 10);
                lat.AddBeam(new(100,100,0), new(100,100,100), 10, 10);

                BBox3 oBox      = new(new(0,-12,-12), new(112,112,100));

                Voxels voxInside    = new(lat);
                Voxels voxOutside   = voxInside.voxOffset(2);
                Voxels voxPipe      =   (voxOutside - voxInside)
                                        & new Voxels(Utils.mshCreateCube(oBox));

                Library.oViewer().SetGroupMaterial(1, "0000FFAA", 0.5f, 0.5f);
                Library.oViewer().SetGroupMaterial(2, "AA", 1.0f, 0.2f);
                
                Library.oViewer().Add(voxInside, 1);
                Library.oViewer().Add(voxOutside, 2);

                Thread.Sleep(1000);
                Library.oViewer().RemoveAllObjects();
                Library.oViewer().Add(voxPipe, 2);
            }

            public static void RunMatrices()
            {
                Matrix4x4 matMoved = Matrix4x4.CreateTranslation(new Vector3(50,0,0));

                Vector3 vecOrigin = Vector3.Zero;
                Vector3 vec100mmX = new(100,0,0);

                Vector3 vecOriginT = Vector3.Transform(vecOrigin, matMoved);
                Vector3 vec100mmXT = Vector3.Transform(vec100mmX, matMoved);

                Console.WriteLine($"Origin is at {vecOriginT}");
                Console.WriteLine($"100mm is at {vec100mmXT}");

                Plane plane         = new Plane(Vector3.UnitX, 0);
                Matrix4x4 matPlane  = Matrix4x4.CreateReflection(plane);

                Vector3 vecOriginP = Vector3.Transform(vecOrigin, matPlane);
                Vector3 vec100mmXP = Vector3.Transform(vec100mmX, matPlane);

                Console.WriteLine($"Origin is at {vecOriginP}");
                Console.WriteLine($"100mm is at {vec100mmXP}");

                Quaternion quatZ    = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, float.Pi / 2);
                Matrix4x4 matQuatZ  = Matrix4x4.CreateFromQuaternion(quatZ);

                Vector3 vecOriginQZ = Vector3.Transform(vecOrigin, matQuatZ);
                Vector3 vec100mmXQZ = Vector3.Transform(vec100mmX, matQuatZ);

                Console.WriteLine($"Origin is at {vecOriginQZ}");
                Console.WriteLine($"100mm is at {vec100mmXQZ}");
            }
        }
    }
}
