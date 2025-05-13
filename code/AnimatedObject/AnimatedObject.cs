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
    namespace AnimatedObject
    {
        class App
        {
            public static void Run()
            {
                // Create an hourglass from lattices
                Lattice lat = new();
                lat.AddBeam(Vector3.Zero,  2* Vector3.UnitZ, 0.3f, 1f,false);
                lat.AddBeam(Vector3.Zero, -2* Vector3.UnitZ, 0.3f, 1f,false);

                // change color and add to viewer, using a specific viewer group
                Library.oViewer().SetGroupMaterial(99, "00FF00AA", 0.5f, 0.5f);
                Library.oViewer().Add(new Voxels(lat), 99);

                // Create an animation action to rotate the object
                Viewer.AnimGroupMatrixRotate oAction = new( Library.oViewer(), 
                                                            99,                 // viewer group
                                                            Matrix4x4.Identity, // start matrix
                                                            Vector3.UnitX,      // around which axis?
                                                            180);               // how many degrees?

                // Now create an animation that executes that action
                Animation oAnim
                        = new Animation(    oAction,                    // What action?
                                            5.0f,                       // How many seconds?
                                            Animation.EType.Repeat,     // repeat
                                            Easing.EEasing.SINE_INOUT); // Ease in and out

                // Add the animation to the viewer, now it will run
                Library.oViewer().AddAnimation(oAnim);

                // Simulate work - place your actual work here
                Thread.Sleep(15 * 1000); // Replace with check whether your work is done

                // Stop the animations and mark the object with a new color
                // you probably want to remove the placeholder and instead show your
                // result - you can do that with  Library.oViewer().RemoveAllObjects();
               
                Library.oViewer().RemoveAllAnimations();
                Library.oViewer().SetGroupMaterial(99, "FF0000", 0.5f, 0.5f);
            }
        }
    }
}
