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

namespace Coding4Engineers.Chapter22
{
    public class TestValueType
    {
        public static void Run()
        {
            // check out reference
            float fValue = 10;
            
            DontChangeVariable(fValue);
            Console.WriteLine(fValue);
            // f is still 10

            ChangeVariable(ref fValue);
            Console.WriteLine(fValue);
            // f is changed
        }

        static void DontChangeVariable(float f)
        {
            f = 5;
        }

        static void ChangeVariable(ref float f)
        {
            f = 5;
        }
    }

    public class TestRefType
    {
        interface IMyContainer
        {
            float fValue {get;}
        }
        class MyContainer : IMyContainer
        {
            public MyContainer(float f=0)
            {
                m_fValue = f;
            }
            public float fValue
            {
                get => m_fValue;
                set => m_fValue = value;
            }

            float m_fValue;
        }

        public static void Run()
        {
            // check out reference
            MyContainer oContainer = new();
            oContainer.fValue = 10;
            
            DontChangeVariableButActuallyItChanges(oContainer);
            Console.WriteLine(oContainer.fValue);
        }

        static void DontChangeVariableButActuallyItChanges(MyContainer oCon)
        {
            oCon.fValue = 5;
        }

        static void CannotChangeReference(IMyContainer xCon)
        {
            // This doesn't work because IMyContainer has no setter
            // xCon.fValue = 5;

            // Make a copy
            MyContainer oCon = new(xCon.fValue);
            oCon.fValue = 5;
        }
    }

    public class TestCustomValue
    {
        public struct MyValue
        {
            public float fValue
            {
                get => m_fValue;
                set => m_fValue = value;
            }
        
            float   m_fValue;
        }

        public static void Run()
        {
            // check out reference
            MyValue oValue = new();
            oValue.fValue = 10;
            
            DontChangeVariable(oValue);
            Console.WriteLine(oValue.fValue);
            // Will be 10, because it's a value type

            ChangeVariable(ref oValue);
            Console.WriteLine(oValue.fValue);
            // Will be 5, because we explictly passed it by reference
        }

        static void DontChangeVariable(MyValue oVal)
        {
            oVal.fValue = 5;
        }

        static void ChangeVariable(ref MyValue oVal)
        {
            oVal.fValue = 5;
        }
    }
    
}