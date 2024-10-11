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
    namespace Chapter10
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
                                                    15,
                                                    20,
                                                    5,
                                                    25);

                    Fixture oFixture = new( oBase, 
                                            oObject, 
                                            new ProgressReporterActive());

                    oFixture.voxAsVoxels().mshAsMesh().SaveToStlFile(Path.Combine(  Utils.strDocumentsFolder(), 
                                                                                    "Fixture.stl"));
                }
            }

            public abstract class ProgressReporter
            {
                public abstract void AddObject( Voxels vox,	
                                                int iGroupID = 0);
                
                public abstract void AddObject( Mesh msh, 	
                                                int iGroupID = 0);
                
                public abstract void SetGroupMaterial(  int        iID, 
                                                        ColorFloat clr, 
                                                        float      fMetallic, 
                                                        float      fRoughness);

                public abstract void ReportTask(string strTask);
            }

            public class ProgressReporterSilent : ProgressReporter
            {
                public override void AddObject( Voxels vox,	
                                                int iGroupID = 0)
                {}
                
                public override void AddObject( Mesh msh, 	
                                                int iGroupID = 0)
                {}
                
                public override void SetGroupMaterial(  int        iID, 
                                                        ColorFloat clr, 
                                                        float      fMetallic, 
                                                        float      fRoughness)
                {}

                public override void ReportTask(string strTask)
                {}
            }

            public class ProgressReporterActive : ProgressReporter
            {
                public override void AddObject( Voxels vox,	
                                                int iGroupID = 0)
                {
                    Library.oViewer().Add(vox, iGroupID);
                }
                
                public override void AddObject( Mesh msh, 	
                                                int iGroupID = 0)
                {
                    Library.oViewer().Add(msh, iGroupID);
                }
                
                public override void SetGroupMaterial(  int        iID, 
                                                        ColorFloat clr, 
                                                        float      fMetallic, 
                                                        float      fRoughness)
                {
                    Library.oViewer().SetGroupMaterial( iID, 
                                                        clr, 
                                                        fMetallic, 
                                                        fRoughness);
                }

                public override void ReportTask(string strTask)
                {
                    Library.Log(strTask);
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
            /// The Fixture class creates fixture from a base plate and object
            /// </summary>
            public class Fixture
            {
                public Fixture( BasePlate           oPlate,
                                FixtureObject       oObject,
                                ProgressReporter    oProgress)
                {
                    oProgress.ReportTask("Creating a new fixture");
                    
                    m_voxFixture = new(oObject.voxObject());

                    oProgress.ReportTask("Creating the sleeve");
                    
                    m_voxFixture.Offset(oObject.fWallMM());
                    m_voxFixture.ProjectZSlice(oObject.fFlangeHeightMM() + oObject.fSleeveMM(), 0);

                    BBox3 oFixtureBounds = m_voxFixture.mshAsMesh().oBoundingBox();
                    oFixtureBounds.vecMin.Z = 0;
                    oFixtureBounds.vecMax.Z = oObject.fFlangeHeightMM() + oObject.fSleeveMM();

                    Mesh mshIntersect = Utils.mshCreateCube(oFixtureBounds);
                    m_voxFixture.BoolIntersect(new Voxels(mshIntersect));

                    oProgress.ReportTask("Building the flange");

                    Voxels voxFlange = new(m_voxFixture);
                    voxFlange.Offset(oObject.fFlangeWidthMM());

                    BBox3 oFlangeBounds = voxFlange.mshAsMesh().oBoundingBox();
                    oFlangeBounds.vecMin.Z = 0;
                    oFlangeBounds.vecMax.Z = oObject.fFlangeHeightMM();

                    Mesh mshIntersectFlange = Utils.mshCreateCube(oFlangeBounds);
                    voxFlange.BoolIntersect(new Voxels(mshIntersectFlange));

                    m_voxFixture.BoolAdd(voxFlange);

                    // Let's make sure the object is removable
                    // by projecting the slices of the object upwards
                    Voxels voxObjectRemovable = new(oObject.voxObject());
                    BBox3 oObjectBounds = voxObjectRemovable.mshAsMesh().oBoundingBox();
                    voxObjectRemovable.ProjectZSlice(   oObjectBounds.vecMin.Z, 
                                                        oObjectBounds.vecMax.Z);
                    
                    m_voxFixture.BoolSubtract(voxObjectRemovable);

                    oProgress.SetGroupMaterial(0, "da9c6b", 0.3f, 0.7f);
                    oProgress.SetGroupMaterial(1, "FF0000BB", 0.5f, 0.5f);
                    oProgress.SetGroupMaterial(2, "0000FF33", 0.5f, 0.5f);

                    oProgress.AddObject(oObject.voxObject(), 1);
                     oProgress.AddObject(voxObjectRemovable, 2);
                    oProgress.AddObject(m_voxFixture, 0);
                }

                public Voxels voxAsVoxels()
                {
                    return m_voxFixture;
                }

                Voxels m_voxFixture;
            }
        }
    }
}
