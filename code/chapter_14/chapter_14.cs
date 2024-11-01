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
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using PicoGK;

namespace Coding4Engineers
{
    namespace Chapter14
    {
        public class PressureVessel
        {
            public static void Run()
            {
                Lattice latVesselVolume = new();
                latVesselVolume.AddBeam(Vector3.Zero, new(100,0,0), 20, 20);
                
                Lattice latPipeVolume = new();
                latPipeVolume.AddBeam(Vector3.Zero, new(-51,0,0), 10, 10, false);

                Voxels voxFluidVolume   = new Voxels(latVesselVolume) 
                                            + new Voxels(latPipeVolume);

                Voxels voxVessel = voxFluidVolume.voxOffset(2) - voxFluidVolume;

                BBox3 oBox = voxVessel.oCalculateBoundingBox();
                oBox.vecMin.X = -50;

                voxVessel.Trim(oBox);
                
                //Library.oViewer().SetGroupMaterial(1, "0000FFAA", 0.5f, 0.5f);
                Library.oViewer().SetGroupMaterial(2, "AA", 1.0f, 0.2f);
                
                //Library.oViewer().Add(voxFluidVolume, 1);
                Library.oViewer().Add(voxVessel, 2);
            }
        }

        public class Manifold
        {
            public static void Run()
            {
                // Let's create a lattice that represents the outflow pipe
                Lattice latOutflowVolume = new();
                latOutflowVolume.AddBeam(Vector3.Zero, new(0,0,100), 7, 7);
                
                // Let's create a lattice that represents the four inflow pipes
                Lattice latInflowVolume = new();
                latInflowVolume.AddBeam(Vector3.Zero, new( 15,0, -51), 7, 7);
                latInflowVolume.AddBeam(Vector3.Zero, new(-15,0, -51), 7, 7);
                latInflowVolume.AddBeam(Vector3.Zero, new(0,-15, -51), 7, 7);
                latInflowVolume.AddBeam(Vector3.Zero, new(0, 15, -51), 7, 7);

                // Combine both to create the fluid volume
                Voxels voxFluidVolume   = new Voxels(latOutflowVolume) 
                                            + new Voxels(latInflowVolume);

                Library.oViewer().SetGroupMaterial(1, "0000FF", 0f, .5f);
                Library.oViewer().Add(voxFluidVolume, 1);

                // Let's create the actual vessel by offsetting the fluid volume
                Voxels voxVessel = voxFluidVolume.voxOffset(1);

                BBox3 oBounds = voxVessel.oCalculateBoundingBox();

                // Fill gaps in triangular structure from 0 downwards
                voxVessel.ProjectZSlice(0, oBounds.vecMin.Z);

                // This box is used to trim away the shell at top and bottom
                BBox3 oBoxTrim = oBounds;
                oBoxTrim.vecMin.Z = -50;
                oBoxTrim.vecMax.Z = 100;

                // Create a flange box from the bounding box, with 5mm height
                BBox3 oBoxFlange = oBoxTrim;
                oBoxFlange.vecMax.Z = oBoxFlange.vecMin.Z + 5;

                // Add flange to vessel
                voxVessel += new Voxels(Utils.mshCreateCube(oBoxFlange));

                // Finally subtract the fluid volume to generate the piping
                voxVessel -= voxFluidVolume;

                // And cut off to open the ends
                voxVessel.Trim(oBoxTrim);
                
                Library.oViewer().SetGroupMaterial(2, "44EE", 0, 1f);
                Library.oViewer().Add(voxVessel, 2);
            }
        }

        public class LatticePainting
        {
            public static void Run()
            {
                Lattice latPaint = new();

                Vector3 vecPos = Vector3.Zero;
                for (float f=0; f<float.Pi*4; f+=0.01f)
                {
                    vecPos.X += Library.fVoxelSizeMM;
                    vecPos.Y = float.Sin(f) * 20f;
                    vecPos.Z = float.Cos(f) * 50f;
                    latPaint.AddSphere(vecPos, 5);
                }

                Library.oViewer().SetGroupMaterial(1, "FF0000", .1f, 0.5f);
                Library.oViewer().Add(new Voxels(latPaint), 1);
            }
        }

        public class LatticeMap
        {
            public static void Run()
            {
                string strImagePath = Path.Combine(Utils.strProjectRootFolder(), "chapter_14/PicoGK.tga");
                TgaIo.LoadTga(strImagePath, out Image img);

                Lattice latPaint = new();

                float fWidth    = img.nWidth;
                float fHeight   = img.nHeight;

                for (float x=0; x < fWidth; x+=0.25f)
                {
                    for (float y=0; y < fHeight; y+=0.25f)
                    {
                        Vector3 vec = new(  (fWidth-x) / 4f, 
                                            (1f - img.fValue((int) x, (int) y)) / 5f, 
                                            (fHeight-y) / 4f);

                        latPaint.AddSphere(vec, .5f);
                    }
                }

                Library.oViewer().SetGroupMaterial(1, "8e500d", .7f, 0.5f);
                Library.oViewer().Add(new Voxels(latPaint), 1);
            }
        }
    }
}
