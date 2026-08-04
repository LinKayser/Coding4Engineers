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

namespace Coding4Engineers.Chapter25
{
    namespace V1
    {
        public readonly struct Angle
        {
            public Angle(float fAngleInRad)
            {
                m_fAngleInRad = fAngleInRad;    
            }

            public float fAngleInRad => m_fAngleInRad;

            readonly float m_fAngleInRad;
        }

        public static class UseAngle
        {
            public static void Use()
            {
                Angle angle = new(Rad.TwoPi / 2f);

                float fSin = float.Sin(angle.fAngleInRad);
            }
        }
    }

    namespace V2
    {
        public readonly struct Angle
        {
            public static Angle oFromRad(float fAngleRad)
            {
                return new Angle(fAngleRad);
            }

            public static Angle oFromDeg(float fAngleDeg)
            {
                return new Angle(Rad.TwoPi * fAngleDeg / 360f);
            }

            public float fAngleInRad => m_fAngleInRad;

            public static implicit operator float(Angle oAngle)
            {
                return oAngle.m_fAngleInRad;
            }

            Angle(float fAngleInRad)
            {
                m_fAngleInRad = fAngleInRad;    
            }

            readonly float m_fAngleInRad;
        }

        public static class UseAngle
        {
            public static void Use()
            {
                Angle angle1 = Angle.oFromRad(Rad.TwoPi / 2f);
                Angle angle2 = Angle.oFromDeg(180f);

                float fSin = float.Sin(angle1);
            }
        }
    }

    namespace V3
    {
        public readonly struct Rad
        {
            public const float TwoPi = float.Tau;

            public Rad(float fAngleInRad)
            {
                m_fRad = fAngleInRad;    
            }

            public static Rad rFromDeg(float fAngleDeg)
            {
                return new Rad(Rad.TwoPi * fAngleDeg / 360f);
            }

            public float fRad => m_fRad;

            public float fDeg => m_fRad * 360f / TwoPi;

            public static implicit operator float(Rad rAngle)
            {
                return rAngle.m_fRad;
            }

            public static explicit operator Rad(float fRad)
            {
                return new Rad(fRad);
            }

            readonly float m_fRad;
        }

        public static class UseAngle
        {
            public static void Use()
            {
                Rad r1 = new(Rad.TwoPi / 2f);
                Rad rOriginal = Rad.rFromDeg(30f);
                
                float fSin = float.Sin(rOriginal);
                Rad rRecovered = new(float.Asin(fSin));
            }
        }
    }
    
}