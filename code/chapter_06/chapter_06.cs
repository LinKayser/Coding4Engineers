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
    namespace Chapter06
    {
        public interface IPoweredObject
        {
            void TurnOn();

            void TurnOff();
        }

        public class Toaster : IPoweredObject
        {
            public void TurnOn()
            {
                // Do something
            }

            public void TurnOff()
            {
                // Do something
            }
        }

        public abstract class Car
        {
            public Car(string strName = "Unnamed Car")
            {
                m_strName = strName;
            }

            public abstract void PrepareForDriving();

            public string strName()
            {
                return m_strName;
            }

            string m_strName;
        }

        public class ElectricCar : Car, IPoweredObject
        {
            public ElectricCar( string strName = "Unnamed Electric Car",
                                uint nPercentFull = 0)
                : base(strName)
            {
                m_fBatteryLevel = nPercentFull / 100.0f;
            }

            public void TurnOn()
            {
                m_bRunning = true;
            }

            public void TurnOff()
            {
                m_bRunning = false;
            }

            public void Charge()
            {
                TurnOff();
                m_fBatteryLevel = 1;
                m_nChargingCycles = m_nChargingCycles + 1;
            }

            public uint nBatteryLevelPercent()
            {
                return Convert.ToUInt32(m_fBatteryLevel * 100.0f);
            }

            public uint nBatteryHealthPercent()
            {
                // Calculate an abstract "health percentage"
                float fHealth = 1.0f - (float)m_nChargingCycles / 10000.0f;
                return Convert.ToUInt32(fHealth * 100.0f);
            }

            public override void PrepareForDriving()
            {
                Charge();
                TurnOn();
            }

            bool m_bRunning = false;
            float m_fBatteryLevel;
            uint m_nChargingCycles = 0;
        }

        public class TeslaRoadster : ElectricCar
        {
            public TeslaRoadster(   string strName = "Unnamed Tesla Roadster",
                                    uint nPercentFull = 0)
            : base(strName, nPercentFull)
            {

            }
        }
    }
}
