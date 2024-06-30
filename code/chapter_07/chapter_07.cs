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

namespace Coding4Engineers
{
    namespace Chapter07
    {
        public class Airplane
        {
            Wing m_oWingLeft        = new(Wing.ESide.Left);
            Wing m_oWingRight       = new(Wing.ESide.Right);
            Fuselage m_oFuselage    = new();
            Tail m_oTail            = new();
        }

        public class Wing
        {
            public enum ESide {Left, Right};
        
            public Wing(ESide eSide)
            {
                m_eSide = eSide;	
            }
        
            Engine  m_oEngine = new();
            ESide   m_eSide;
        }

        public class Fuselage
        {
            // We will figure it out
        }

        public class Tail
        {
            // We will figure it out
        }

        public class Engine
        {
            // Some complex stuff here
        }
    }
}
