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

namespace Coding4Engineers.Chapter19
{
    namespace v1
    {
        public class SomeClass
        {
            public SomeClass(Vector3 vecOrg)
            {
                m_vecOrigin = vecOrg;
            }
            
            public SomeClass()
            {
                m_vecOrigin = Vector3.Zero;
            }
        
            Vector3 m_vecOrigin;
        }
    }

    namespace v2
    {
        public class SomeClass
        {
            public SomeClass(Vector3? vecOrg=null)
            {
                if (vecOrg is null)
                    m_vecOrigin = Vector3.Zero;
                else
                    m_vecOrigin = vecOrg.Value;
            }

            Vector3 m_vecOrigin;
        }
    }

    namespace v3
    {
        public class SomeClass
        {
            public SomeClass(Vector3? vecOrg=null)
            {
                m_vecOrigin = vecOrg is null ? Vector3.Zero : vecOrg.Value;
            }

            Vector3 m_vecOrigin;
        }
    }

    namespace v4
    {
        public class SomeClass
        {
            public SomeClass(Vector3? vecOrg=null)
            {
                m_vecOrigin = vecOrg ?? Vector3.Zero;
            }

            Vector3 m_vecOrigin;
        }
    }
    

    namespace v1
    {
        public class AClass
        {
            public static int nNumber()
            {
                return 42;
            }

            public static int nAdd(int n1, int n2)
            {
                return n1 + n2;
            }
        }
    }

    namespace v2
    {
        public class AClass
        {
            public static int nNumber() => 42;
            
            public static int nAdd(int n1, int n2) => n1 + n2;
        }
    }

    namespace v3
    {
        public class AClass
        {
            public AClass(Vector3 vecValue)
            {
                m_vecValue = vecValue;
            }

            Vector3 m_vecValue;
        }
    }

    namespace v4
    {
        public class AClass
        {
            public AClass(Vector3 vecValue)
            {
                m_vecValue = vecValue;
            }

            public Vector3 vecVal()
            {
                return m_vecValue;
            }

            Vector3 m_vecValue;
        }
    }

    namespace v5
    {
        public class AClass
        {
            public AClass(Vector3 vecValue)
            {
                m_vecValue = vecValue;
            }

            public Vector3 vecVal() => m_vecValue;

            Vector3 m_vecValue;
        }
    }

    namespace v6
    {
        public class AClass
        {
            public AClass(Vector3 vecValue)
            {
                m_vecValue = vecValue;
            }

            public Vector3 vecVal => m_vecValue;

            Vector3 m_vecValue;
        }
    }
    
}