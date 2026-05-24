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
using PicoGK.Numerics;

namespace Coding4Engineers.Chapter24
{ 
    public class PlayGround
    {
        public static void ShowCartesian()
        {
            Library.oViewer().AddCross(Vector3.Zero);

            Library.oViewer().AddArrow(Vector3.Zero, new(10,0,0), "#FF0000");
            Library.oViewer().AddArrow(new(10,0,0), new(10,10,0), "#009900");
            Library.oViewer().AddArrow(new(0,0,0), new(10,10,0), "#494cf2");
        }

        public static void ShowPolar()
        {
            Polar coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian().vecAsVector3());

            Polar co = new();
            co.R = 10;
            
            float fPhiTarget = (2 * float.Pi) * 2f/3f; // two thirds around the circle

            Library.oViewer().AddArrow(coZero.vecAsCartesian().vecAsVector3(), co.vecAsCartesian().vecAsVector3(), "#FF0000");

            PolyLine oPoly = new("#009900");

            int nSteps = 100;
            float fStep = fPhiTarget / nSteps;
            
            for (int n=0; n<nSteps; n++)
            {
                co.Phi = n * fStep;
                oPoly.nAddVertex(co.vecAsCartesian().vecAsVector3());
            }

            oPoly.AddArrow();
            Library.oViewer().Add(oPoly);
        }

        public static void PolarCircle()
        {
            Polar coZero = new();

            Vector2 vec2 = coZero.vecAsCartesian();
            Vector3 vec3 = vec2.vecAsVector3();
            Library.oViewer().AddCross(vec3);

            Polar co = new();
            co.R = 10;

            int     nSteps  = 20;
            float   fStep   = float.Pi * 2 / nSteps;
            
            for (int n=0; n<nSteps;n++)
            {
                co.Phi = n * fStep;
                Library.oViewer().AddCross(co.vecAsCartesian().vecAsVector3());
            }
        }

        public static void ShowCylindrical()
        {
            Cylindrical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            Vector3 vecFrom = coZero.vecAsCartesian();
            coZero.Z = 10;

            Library.oViewer().AddArrow(vecFrom, coZero.vecAsCartesian(), "#494cf2");

            Cylindrical co = new();
            co.Z = coZero.Z;
            co.R = 10;
            
            float fPhiTarget = (2 * float.Pi) * 2f/3f; // two thirds around the circle

            Library.oViewer().AddArrow(coZero.vecAsCartesian(), co.vecAsCartesian(), "#FF0000");

            PolyLine oPoly = new("#009900");

            int nSteps = 100;
            float fStep = fPhiTarget / nSteps;
            
            for (int n=0; n<nSteps; n++)
            {
                co.Phi = n * fStep;
                oPoly.nAddVertex(co.vecAsCartesian());
            }

            oPoly.AddArrow();
            Library.oViewer().Add(oPoly);    
        }

        public static void CylindricalCircle()
        {
            Cylindrical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            Cylindrical co = new();
            co.R = 10;
            
            int     nSteps  = 40;
            float   fStep   = float.Pi * 2 / nSteps;
            
            for (int n=0; n<nSteps;n++)
            {
                co.Phi = n * fStep;
                Library.oViewer().AddCross(co.vecAsCartesian());
            }
        }

        public static void CylindricalSpiral()
        {
            Cylindrical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            Cylindrical co = new();
            co.R = 10;
            
            int     nSteps  = 40;
            float   fStep   = float.Pi * 2 / nSteps;
            float   fZStep  = 20f / nSteps;
            
            for (int n=0; n<nSteps;n++)
            {
                co.Phi      = n * fStep;
                coZero.Z    = n * fZStep;
                co.Z        = coZero.Z;
                
                Library.oViewer().AddArrow(coZero.vecAsCartesian(), co.vecAsCartesian());
            }
        }

        public static void ShowSpherical()
        {
            Spherical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            for (float fT = 0.1f; fT < float.Pi *.8f; fT+=0.2f)
            {
                
            }

            float fThetaTarget  = float.Pi / 2; // half-way down from the "North Pole" -> Equator
            float fPhiTarget    = (2 * float.Pi) * 2f/3f; // two thirds around the circle
            int nSteps = 100;

            Spherical co = new();
            co.R = 10;

            Library.oViewer().AddArrow(coZero.vecAsCartesian(), co.vecAsCartesian(), "#FF0000");

            float fThetaStep = fThetaTarget / (nSteps-1);
            PolyLine polyTheta = new("#494cf2");
            
            for (int n=0; n<nSteps; n++)
            {
                co.Theta = n * fThetaStep;
                polyTheta.nAddVertex(co.vecAsCartesian());
            }

            polyTheta.AddArrow();
            Library.oViewer().Add(polyTheta);

            PolyLine polyPhi = new("#009900");

            float fPhiStep = fPhiTarget / nSteps;
            
            for (int n=0; n<nSteps; n++)
            {
                co.Phi = n * fPhiStep;
                polyPhi.nAddVertex(co.vecAsCartesian());
            }

            polyPhi.AddArrow();
            Library.oViewer().Add(polyPhi);    
        }

        public static void ShowConcentric()
        {
            Spherical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            float fPhiTarget    = (2 * float.Pi) * 9f/10f; // nine tenths around the circle
            int nSteps = 100;

            Spherical co = new();
            co.R = 30;

            for (co.Theta = 0.05f; co.Theta < float.Pi *.7f; co.Theta +=0.2f)
            {
                PolyLine polyPhi = new("#009900");

                float fPhiStep = fPhiTarget / nSteps;
            
                for (int n=0; n<nSteps; n++)
                {
                    co.Phi = n * fPhiStep;
                    polyPhi.nAddVertex(co.vecAsCartesian());
                }

                polyPhi.AddArrow();
                Library.oViewer().Add(polyPhi);    
            }
        }

        public static void ShowRadial()
        {
            Spherical coZero = new();
            Library.oViewer().AddCross(coZero.vecAsCartesian());

            float fThetaTarget    = (float.Pi) * 7f/10f; // seven tenths of the half circle
            int nSteps = 100;

            Spherical co = new();
            co.R = 30;

            for (co.Phi = 0.05f; co.Phi < 2*float.Pi; co.Phi +=0.2f)
            {
                PolyLine polyTheta = new("#494cf2");

                float fThetaStep = fThetaTarget / (nSteps-1);
            
                for (int n=0; n<nSteps; n++)
                {
                    co.Theta = n * fThetaStep;
                    polyTheta.nAddVertex(co.vecAsCartesian());
                }

                polyTheta.AddArrow();
                Library.oViewer().Add(polyTheta);    
            }
        }

        public static void Spherical()
        {
            Spherical co = new();
            
            co.R = 1;
            Console.WriteLine($"Example 1: Spherical={co} - Cartesian={co.vecAsCartesian()}");

            co.Theta = float.Pi / 2;
            Console.WriteLine($"Example 2: Spherical={co} - Cartesian={co.vecAsCartesian()}");

            co.Phi = float.Pi;
            Console.WriteLine($"Example 3: Spherical={co} - Cartesian={co.vecAsCartesian()}");

            co.Theta = float.Pi;
            Console.WriteLine($"Example 4: Spherical={co} - Cartesian={co.vecAsCartesian()}");
        }

        public static void ObjectsAlongACircle()
        {
            Library.oViewer().SetGroupMaterial(0, "#e1188a", 0.8f, 0.4f);

            Polar co = new();
            
            co.R = 30;
            
            int nSteps = 10;
            float fStep = float.Pi*2 / nSteps;
            
            for (int n=0; n<nSteps; n++)
            {
                co.Phi = n * fStep;
                Vector3 vecPos = co.vecAsCartesian().vecAsVector3();
                Library.oViewer().Add(Voxels.voxSphere(vecPos, 2));
            }
        }

        public static void AlignedObjectsAlongACircle()
        {
            Library.oViewer().SetGroupMaterial(0, "#18e1ab", 0.8f, 0.4f);

            Polar co = new();
            co.R = 30;
            
            int nSteps = 10;
            float fStep = float.Pi*2 / nSteps;
            
            for (int n=0; n<nSteps; n++)
            {
                co.Phi = n * fStep;
                Vector3 vecPos = co.vecAsCartesian().vecAsVector3();

                Polar coOuter = co;
                coOuter.R += 10;
                Vector3 vecOuter = coOuter.vecAsCartesian().vecAsVector3();

                Library.oViewer().Add(Voxels.voxLatticeBeam(vecPos, 2, vecOuter, 1));
            }
        }

        public static void AlignedObjectsAroundSphere()
        {
            Library.oViewer().SetGroupMaterial(0, "#e15418", 0.8f, 0.4f);

            Spherical co = new();
            co.R = 20;
            
            int nThetaSteps = 5;
            float fThetaStep = float.Pi / (nThetaSteps-1);

            int nPhiSteps = 10;
            float fPhiStep = 2*float.Pi / nPhiSteps;
            
            for (int nTheta=0; nTheta<nThetaSteps; nTheta++)
            {
                co.Theta = nTheta * fThetaStep;

                for (int nPhi=0; nPhi <nPhiSteps; nPhi++)
                {
                    co.Phi          = nPhi * fPhiStep;
                    Vector3 vecPos  = co.vecAsCartesian();

                    Spherical coOuter = co;
                    coOuter.R += 10;
                    Vector3 vecOuter = coOuter.vecAsCartesian();

                    Library.oViewer().Add(Voxels.voxLatticeBeam(vecPos, 1, vecOuter, .5f));
                }
            }
        }

        public static void HedgeHog()
        {
            Library.oViewer().SetGroupMaterial(0, "#d0cd21", 0.8f, 0.1f);
            Spherical co = new();
            co.R = 60;
            
            int nThetaSteps = 30;
            float fThetaStep = float.Pi / (nThetaSteps-1);

            int nPhiSteps = 60;
            float fPhiStep = 2*float.Pi / nPhiSteps;
            
            for (int nTheta=0; nTheta<nThetaSteps; nTheta++)
            {
                co.Theta = nTheta * fThetaStep;
                
                for (int nPhi=0; nPhi <nPhiSteps; nPhi++)
                {
                    co.Phi          = nPhi * fPhiStep;
                    Vector3 vecPos  = co.vecAsCartesian();

                    Spherical coOuter = co;
                    coOuter.R += 40;
                    Vector3 vecOuter = coOuter.vecAsCartesian();

                    Library.oViewer().Add(Voxels.voxLatticeBeam(vecPos, 2, vecOuter, 1));
                }
            }
        }

        public static void AlignedObjectsAroundCylinder()
        {
            Library.oViewer().SetGroupMaterial(0, "#2190d0", 0.8f, 0.7f);

            Cylindrical co = new();
            co.R = 15;
            
            int nSteps = 15;
            float fPhiStep = 2*float.Pi / nSteps;

            // Distance (on the circle), between each object: 2πR
            float fZStep = fPhiStep * co.R;

            for (int nZ=0; nZ<nSteps; nZ++)
            {
                co.Z = nZ * fZStep;

                for (int nPhi=0; nPhi <nSteps; nPhi++)
                {
                    co.Phi          = nPhi * fPhiStep;
                    Vector3 vecPos  = co.vecAsCartesian();

                    Cylindrical coOuter = co;
                    coOuter.R += 10;
                    Vector3 vecOuter = coOuter.vecAsCartesian();

                    Library.oViewer().Add(Voxels.voxLatticeBeam(vecPos, 1, vecOuter, .5f));
                }
            }
        }

        public static void AroundInTwoWays()
        {
            Polar co = new(1,0); // Radius 1, Phi 0

            Console.WriteLine("Positive (Counter-Clockwise)");

            co.Phi=Rad.TwoPi * -0.00f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * 0.25f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * 0.50f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * 0.75f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            Console.WriteLine("Negative (Clockwise)");

            co.Phi=Rad.TwoPi * -0.00f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * -0.25f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * -0.50f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            co.Phi=Rad.TwoPi * -0.75f;
            Console.WriteLine($"{co} -> {co.vecAsCartesian()}");

            Console.WriteLine("Cartesian Conversion");
            Vector2 vec;
            
            vec = Vector2.UnitX;
            co    = new(vec);
            Console.WriteLine($"{vec} -> {co}");

            vec = Vector2.UnitY;
            co    = new(vec);
            Console.WriteLine($"{vec} -> {co}");

            vec = -Vector2.UnitX;
            co    = new(vec);
            Console.WriteLine($"{vec} -> {co}");

            vec = -Vector2.UnitY;
            co    = new(vec);
            Console.WriteLine($"{vec} -> {co}");
        }

        public static void CartesianToOthers()
        {
            TestPolar(Vector2.UnitX);
            TestPolar(Vector2.UnitY);
            TestPolar(-Vector2.UnitX);
            TestPolar(-Vector2.UnitY);
            TestPolar(new(2,2));

            TestCylindrical(Vector3.UnitX);
            TestCylindrical(Vector3.UnitY);
            TestCylindrical(Vector3.UnitZ);
            TestCylindrical(-Vector3.UnitX);
            TestCylindrical(-Vector3.UnitY);
            TestCylindrical(-Vector3.UnitZ);
            TestCylindrical(new(2,2,2));

            TestSpherical(Vector3.UnitX);
            TestSpherical(Vector3.UnitY);
            TestSpherical(Vector3.UnitZ);
            TestSpherical(-Vector3.UnitX);
            TestSpherical(-Vector3.UnitY);
            TestSpherical(-Vector3.UnitZ);
            TestSpherical(new(2,2,2));
        }

        static void TestPolar(Vector2 vec)
        {
            Polar coPol = new(vec);
            Vector2 vecConverted = coPol.vecAsCartesian();

            Console.WriteLine($"Vector2: {vec} -> Polar: {coPol} -> Cartesian: {vecConverted}");

            if (vecConverted.bAlmostEqual(vec))
                Console.WriteLine($"Success");
            else
                Console.WriteLine($"Failed");
        }

        static void TestCylindrical(Vector3 vec)
        {
            Cylindrical coCyl = new(vec);
            Vector3 vecConverted = coCyl.vecAsCartesian();

            Console.WriteLine($"Vector3: {vec} -> Cylindrical: {coCyl} -> Cartesian: {vecConverted}");

            if (vecConverted.bAlmostEqual(vec))
                Console.WriteLine($"Success");
            else
                Console.WriteLine($"Failed");
        }

        static void TestSpherical(Vector3 vec)
        {
            Spherical coSph = new(vec);
            Vector3 vecConverted = coSph.vecAsCartesian();

            Console.WriteLine($"Vector3: {vec} -> Spherical: {coSph} -> Cartesian: {vecConverted}");

            if (vecConverted.bAlmostEqual(vec))
                Console.WriteLine($"Success");
            else
                Console.WriteLine($"Failed");
        }
    }
}