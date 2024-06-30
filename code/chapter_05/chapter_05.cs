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
    namespace Chapter05
    {
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
        
        public class TeslaRoadster : Car
        {
            public TeslaRoadster(   string  strName         = "Unnamed Tesla Roadster",
                                    uint    nPercentFull    = 0) 
                : base(strName)
            {
                m_fBatteryLevel    = nPercentFull / 100.0f;
            }

            public void Charge()
            {
                m_fBatteryLevel     = 1;
                m_nChargingCycles   = m_nChargingCycles + 1;
            }

            public uint nBatteryLevelPercent()
            {
                return Convert.ToUInt32(m_fBatteryLevel * 100.0f);
            }

            public uint nBatteryHealthPercent()
            {
                // Calculate an abstract "health percentage"
                float fHealth = 1.0f - (float) m_nChargingCycles / 10000.0f;
                return Convert.ToUInt32(fHealth * 100.0f);
            }

            public override void PrepareForDriving()
            {
                Charge();
            }

            float   m_fBatteryLevel;
            uint    m_nChargingCycles = 0;
        }   
    }
}
