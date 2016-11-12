using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svglayerplay
{
    class svgPathCommand
    {
        public String m_strCommand;
        int m_nCoordCount = 0;
        int m_nDataCount = 0;
        float[] m_dData = new float[8];
        float[] m_dX = new float[8];
        float[] m_dY = new float[8];
        public void AddCoords(float dX, float dY)
        {
            m_dX[m_nCoordCount] = dX;
            m_dY[m_nCoordCount] = dY;
            m_nCoordCount++;
        }
        public void AddData(float d)
        {
            m_dData[m_nDataCount] = d;
            m_nDataCount++;
        }
        public void GetCoord(int nIndex, ref float x, ref float y)
        {
            if (nIndex < m_nCoordCount)
            {
                x = m_dX[nIndex];
                y = m_dY[nIndex];
            }
        }
        public float GetData(int nIndex)
        {
            if (nIndex < m_nDataCount) return m_dData[nIndex];
            return 0;
        }
    }
}
