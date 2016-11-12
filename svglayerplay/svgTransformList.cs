using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace svglayerplay
{
    class svgTransformList
    {
        public List<Matrix> m_list = new List<Matrix>();
        /// <summary>
        /// Interprets a transform string and adds a new transform the the list of transforms.
        /// </summary>
        /// <param name="strTransform">SVG transform attribute.</param>
        /// <returns>void</returns>
        public void Add(String strTransform)
        {
            try
            {
                //extract the transform type
                String[] slParts = strTransform.Split('(');
                String strTransformType = slParts[0];
                String[] slTransformData = slParts[1].Substring(0, slParts[1].IndexOf(")")).Split(',');
                float[] dTransformData = new float[slTransformData.Length];
                //extract the transform values
                for (int a = 0; a < slTransformData.Length; a++)
                {
                    dTransformData[a] = (float)Convert.ToDouble(slTransformData[a]);
                }
                //interpret the transform type
                if (strTransformType == "matrix")
                {
                    Matrix matrix = new Matrix(dTransformData[0], dTransformData[1], dTransformData[2], dTransformData[3], dTransformData[4], dTransformData[5]);
                    m_list.Add(matrix);
                }
                else if (strTransformType == "translate")
                {
                    Matrix matrix = new Matrix();
                    matrix.Translate(dTransformData[0], dTransformData[1]);
                    m_list.Add(matrix);
                }
                else if (strTransformType == "scale")
                {
                    Matrix matrix = new Matrix();
                    matrix.Scale(dTransformData[0], dTransformData[1]);
                    m_list.Add(matrix);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Removes the latest transform from the list
        /// </summary>
        /// <returns>void</returns>
        public void Remove()
        {
            if (m_list.Count > 0)
            {
                m_list.RemoveAt(m_list.Count - 1);
            }
        }
        /// <summary>
        /// Combines all the current transforms into a Matrix that can then be applied to a path.
        /// </summary>
        /// <returns>A new Matrix containing all the current transforms.</returns>
        public Matrix GetMatrix()
        {
            if (m_list.Count == 0) return null;
            Matrix matrix = new Matrix();
            for (int x = 0; x < m_list.Count; x++)
            {
                matrix.Multiply(m_list[x]);
            }
            return matrix;
        }
    }
}
