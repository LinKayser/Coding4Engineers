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
    namespace Chapter09
    {
        namespace Fixtures
        {
            public class App
            {
                public static void Run()
                {
                    BasePlate oBase = new();

                    Mesh mshSmall = Mesh.mshFromStlFile(Path.Combine(
                                            Utils.strPicoGKSourceCodeFolder(),
                                            "Examples/Testfiles/Teapot.stl"));

                    Mesh mshObject = mshSmall.mshCreateTransformed(new(6, 6, 6), Vector3.Zero);

                    FixtureObject oObject = new(    mshObject,
                                                    10,
                                                    15,
                                                    3,
                                                    20);

                    FixtureMaker oMaker = new(oBase, oObject);

                    oMaker.Run();
                }
            }

            /// <summary>
            /// BasePlate object that implements the base plate
            /// </summary>
            public class BasePlate
            {
                // nothing here yet
            }

            /// <summary>
            /// The FixtureObject holds the object to be fixed and various
            /// parameters that are necessary to describe the fixture
            /// </summary>
            public class FixtureObject
            {
                public FixtureObject(  Mesh msh,
                                       float fObjectBottomMM,
                                       float fSleeveMM,
                                       float fWallMM,
                                       float fFlangeMM)
                {
                    if (fObjectBottomMM <= 0)
                        throw new Exception("Object cannot be placed below build plate");

                    BBox3 oObjectBounds = msh.oBoundingBox();

                    Vector3 vecOffset = new Vector3( -oObjectBounds.vecCenter().X,
                                                     -oObjectBounds.vecCenter().Y,
                                                     -oObjectBounds.vecMin.Z + fObjectBottomMM);

                    m_voxObject = new Voxels(msh.mshCreateTransformed(Vector3.One, vecOffset));

                    m_fObjectBottom = fObjectBottomMM;
                    m_fSleeve = fSleeveMM;
                    m_fWall = fWallMM;
                    m_fFlange = fFlangeMM;
                }

                public Voxels voxObject()
                {
                    return m_voxObject;
                }

                public float fWallMM()
                {
                    return m_fWall;
                }

                public float fSleeveMM()
                {
                    return m_fSleeve;
                }

                public float fFlangeHeightMM()
                {
                    return m_fObjectBottom;
                }

                public float fFlangeWidthMM()
                {
                    return m_fFlange;
                }

                Voxels m_voxObject;
                float m_fWall;
                float m_fSleeve;
                float m_fObjectBottom;
                float m_fFlange;
            }

            /// <summary>
            /// The FixtureMaker builds a fixture from a base plate and object
            /// </summary>
            public class FixtureMaker
            {
                public FixtureMaker(  BasePlate oPlate,
                                      FixtureObject oObject)
                {
                    m_oPlate = oPlate;
                    m_oObject = oObject;
                }

                public void Run()
                {
                    Voxels voxFixture = new(m_oObject.voxObject());
                    voxFixture.Offset(m_oObject.fWallMM());
                    voxFixture.ProjectZSlice(m_oObject.fFlangeHeightMM() + m_oObject.fSleeveMM(), 0);

                    BBox3 oFixtureBounds = voxFixture.mshAsMesh().oBoundingBox();
                    oFixtureBounds.vecMin.Z = 0;
                    oFixtureBounds.vecMax.Z = m_oObject.fFlangeHeightMM() + m_oObject.fSleeveMM();

                    Mesh mshIntersect = Utils.mshCreateCube(oFixtureBounds);
                    voxFixture.BoolIntersect(new Voxels(mshIntersect));

                    // Flange

                    Voxels voxFlange = new(voxFixture);
                    voxFlange.Offset(m_oObject.fFlangeWidthMM());

                    BBox3 oFlangeBounds = voxFlange.mshAsMesh().oBoundingBox();
                    oFlangeBounds.vecMin.Z = 0;
                    oFlangeBounds.vecMax.Z = m_oObject.fFlangeHeightMM();

                    Mesh mshIntersectFlange = Utils.mshCreateCube(oFlangeBounds);
                    voxFlange.BoolIntersect(new Voxels(mshIntersectFlange));

                    voxFixture.BoolAdd(voxFlange);
                    voxFixture.BoolSubtract(m_oObject.voxObject());

                    Library.oViewer().SetGroupMaterial(0, "da9c6b", 0.3f, 0.7f);
                    Library.oViewer().SetGroupMaterial(1, "FF0000BB", 0.5f, 0.5f);

                    Library.oViewer().Add(m_oObject.voxObject(), 1);
                    Library.oViewer().Add(voxFixture, 0);
                }

                BasePlate m_oPlate;
                FixtureObject m_oObject;
            }
        }
    }
}
