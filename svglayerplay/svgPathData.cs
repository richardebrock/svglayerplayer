using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace svglayerplay
{
    class svgPathData
    {
        String[] m_slPathData = null;
        int m_nIndex = 0;
        public void SetData(String strData)
        {
            String strPathData = strData.Replace(',', ' ');
            m_slPathData = strPathData.Split(' ');
            m_nIndex = 0;
        }
        public Boolean GetNextItem(ref String strItem)
        {
            while (m_nIndex < m_slPathData.Length)
            {
                strItem = m_slPathData[m_nIndex];
                m_nIndex++;
                if (strItem.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public double GetNextDouble()
        {
            if (m_nIndex < m_slPathData.Length)
            {
                String strItem = "";
                try
                {
                    if (GetNextItem(ref strItem))
                    {
                        return Convert.ToDouble(strItem);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
            return 0f;
        }
        public int GetNextInt()
        {
            if (m_nIndex < m_slPathData.Length)
            {
                String strItem = "";
                try
                {
                    if (GetNextItem(ref strItem))
                    {
                        return Convert.ToInt32(strItem);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
            return 0;
        }
    }
}
