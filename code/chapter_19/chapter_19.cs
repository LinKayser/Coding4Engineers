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

namespace Coding4Engineers.Chapter19
{
    public class ImplicitSphere : IBoundedImplicit
    {
        public ImplicitSphere(float fRadius)
        {
            m_fRadius = fRadius;
        }
        public float fSignedDistance(in Vector3 vec)
        {
            return  float.Sqrt( vec.X * vec.X +
                                vec.Y * vec.Y +
                                vec.Z * vec.Z)
                     - m_fRadius;
        }

        BBox3 IBoundedImplicit.oBounds => new (     new(-m_fRadius,-m_fRadius,-m_fRadius),
                                                    new( m_fRadius, m_fRadius, m_fRadius));

        float m_fRadius;
    }

    public class ImplicitGyroid : IImplicit
    {
        public ImplicitGyroid(  float fUnitSize,
                                float fThickness)
        {
            m_fUnitSize     = fUnitSize;
            m_fThickness    = fThickness;
        }
        public float fSignedDistance(in Vector3 vec)
        {
            // Scale coordinates to match unit cell size (period becomes m_fUnitSize)
            float k = 2.0f * float.Pi / m_fUnitSize;
            float sx = k * vec.X;
            float sy = k * vec.Y;
            float sz = k * vec.Z;

            // Gyroid implicit function: sin(x)*cos(y) + sin(y)*cos(z) + sin(z)*cos(x)
            float f =   float.Sin(sx) * float.Cos(sy) +
                        float.Sin(sy) * float.Cos(sz) +
                        float.Sin(sz) * float.Cos(sx);

            // SDF: distance from surface with thickness m_fThickness
            return float.Abs(f) - (m_fThickness / 2.0f);
        }

        float m_fUnitSize;
        float m_fThickness;
    }

    public static class Demo
    {
        public static void RunSphere()
        {
            Voxels vox = new(new ImplicitSphere(3));

            Library.oViewer().SetGroupMaterial(1, "fb9696", 0.9f, 0.2f);
            Library.oViewer().Add(vox, 1);
        }

        public static void RunGyroid()
        {
            Lattice lat = new();

            lat.AddBeam(    Vector3.Zero,
                            new(6,0,8),
                            2,
                            6);

            Voxels vox = new(lat);

            Voxels voxSmaller = vox.voxOffset(-0.3f);
            vox -= voxSmaller; // Create shell

            ImplicitGyroid oGyroid = new(2, .5f);
            voxSmaller.IntersectImplicit(oGyroid);

            vox += voxSmaller;

            Library.oViewer().SetGroupMaterial(1, "fb96FF33", 0.9f, 0.2f);
            Library.oViewer().Add(vox, 1);
        }
    }
}
