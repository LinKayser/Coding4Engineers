using System.Numerics;
using PicoGK;

namespace Coding4Engineers
{
    namespace Chapter17
    {
        namespace Surface
        {
            public interface IModulation
            {
                /// <summary>
                /// Abstracts the modulation of a surface
                /// </summary>
                /// <param name="vecUV">Local x/y coordinate of the surface, 0..1</param>
                /// <returns>Absolute height of the modulated surface</returns>
                float fHeight(Vector2 vecUV);
            }

            /// <summary>
            /// A surface modulation that does nothing
            /// </summary>
            public class ModulationNoop : IModulation
            {
                public float fHeight(Vector2 _) => 0;
            }

            /// <summary>
            /// A surface modulation that does nothing
            /// </summary>
            public class ModulationRandom : IModulation
            {
                public ModulationRandom(float fHeight)
                {
                    m_fHeight = fHeight;
                }

                public float fHeight(Vector2 _)
                {
                    return m_rnd.NextSingle() * m_fHeight;
                }

                Random  m_rnd = new();
                float   m_fHeight;
            }

            public class ModulationGauss : IModulation
            {
                public ModulationGauss( float fSigmaU = 0.15f, 
                                        float fSigmaV = 0.15f)
                {
                    m_fSigmaU = fSigmaU;
                    m_fSigmaV = fSigmaV;
                }

                public float fHeight(Vector2 vecUV)
                {
                    float fExpU = float.Pow(vecUV.X - 0.5f, 2) / (2 * float.Pow(m_fSigmaU, 2));
                    float fExpV = float.Pow(vecUV.Y - 0.5f, 2) / (2 * float.Pow(m_fSigmaV, 2));
                    return float.Exp(-(fExpU + fExpV));
                }

                float m_fSigmaU;
                float m_fSigmaV;
            }

            /// <summary>
            /// Reads the depth information from a supplied image
            /// </summary>
            public class ModulationImage : IModulation
            {
                public ModulationImage(Image img)
                {
                    m_img       = img;
                }

                public float fHeight(Vector2 vecUV)
                {
                    int nX = m_img.nWidth - (int) float.Round(vecUV.X * m_img.nWidth);
                    int nY = (int) float.Round(vecUV.Y * m_img.nHeight);

                    return m_img.fValue(nX, nY);
                }

                Image m_img;
            }

            public class ModulationTrans : IModulation
            {
                public ModulationTrans(   IModulation xModulation,
                                          float   fScale    = 1,
                                          float   fOffset   = 0,
                                          bool    bFlipU    = false,
                                          bool    bFlipV    = false)
                {
                    m_xModulation   = xModulation;
                    m_fScale        = fScale;
                    m_fOffset       = fOffset;
                    m_bFlipU        = bFlipU;
                    m_bFlipV        = bFlipV;
                }

                public float fHeight(Vector2 vecUV)
                {
                    if (m_bFlipU)
                        vecUV.X = 1.0f - vecUV.X;

                    if (m_bFlipV)
                        vecUV.Y = 1.0f - vecUV.Y;

                    return m_xModulation.fHeight(vecUV) * m_fScale + m_fOffset;
                }

                IModulation m_xModulation;
                float       m_fScale;
                float       m_fOffset;
                bool        m_bFlipU;
                bool        m_bFlipV;
            }
        }     
    }
}