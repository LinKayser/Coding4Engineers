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

namespace Coding4Engineers.Chapter21
{
    namespace v1
    {
        public class SomeClass
        {
            public Vector3 vecOrigin()
            {
                return m_vecOrigin;
            }

            public void SetOrigin(Vector3 vec)
            {
                m_vecOrigin = vec;
            }
        
            // private variable
            Vector3 m_vecOrigin = Vector3.Zero;
        }
    }

    namespace v2
    {
        public class SomeClass
        {
            public Vector3 vecOrigin()
            {
                return m_vecOrigin;
            }

            public void SetOriginAndRecompute(Vector3 vec)
            {
                if (vec != m_vecOrigin)
                {
                    m_vecOrigin = vec;
                    Recompute();
                }
            }
        
            void Recompute()
            {
                // ...
            }

            // private variable
            Vector3 m_vecOrigin = Vector3.Zero;
        }
    }

    namespace v3
    {
        public class SomeClass
        {
            public Vector3 vecOrigin()
            {
                return m_vecOrigin;
            }

            public void SetOrigin(Vector3 vec)
            {
                if (vec != m_vecOrigin)
                {
                    m_vecOrigin = vec;
                    m_bRecomputeRequired = true;
                }
            }
        
            // private variable
            Vector3 m_vecOrigin = Vector3.Zero;
            bool m_bRecomputeRequired = true;
        }
    }

    namespace v4
    {
        public class SomeClass
        {
            public static void DoSomething()
            {
                SomeClass oClass = new();
                oClass.vecOrigin = Vector3.UnitX;

                Vector3 vecValue = oClass.vecOrigin;
                Console.WriteLine($"Value= {vecValue}");
            }
            public Vector3 vecOrigin
            {
                get
                {
                    return m_vecOrigin;
                }

                set
                {
                    if (value != m_vecOrigin)
                    {
                        m_vecOrigin = value;
                        m_bRecomputeRequired = true;
                    }
                }    
            }
        
            // private variable
            Vector3 m_vecOrigin = Vector3.Zero;
            bool m_bRecomputeRequired = true;
        }
    }
    

    namespace v5
    {
        public class State
        {

        }
        public class Universe
        {
            public State oQuantumStateOfTheUniverse
            {
                get => oCalculateTheQuantumStateOfTheUniverse();
            }
        
            State oCalculateTheQuantumStateOfTheUniverse()
            {
                // Run for at least 13.8 billion years
                return new();
            }
        }
    }

    namespace v6
    {
        public class AClass
        {
            
            static void DoSomething()
            {
                float [] af = new float[100];
                Console.WriteLine(af.Length);

                LinkedList<int> ai = new();
                ai.AddLast(10);
                Console.WriteLine(ai.Count());

                int nListCount = ai.Count();
                for (int n=0; n<nListCount; n++)
                {
                    // Do something
                }

                for (int n=0; n<af.Length; n++)
                {
                    // Do something
                }
            }
        }
    }

    namespace v7
    {
        public class SomeClass
        {
            public Vector3 vecOrigin {get;set;} = Vector3.Zero;
        }
    }
    
}