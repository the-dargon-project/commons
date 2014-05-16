using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty
{
    public class Matrix
    {
        public static Matrix Identity
        {
            get
            {
                return new Matrix(
                    new double [][]{
                        new double[]{1, 0, 0, 0},
                        new double[]{0, 1, 0, 0},
                        new double[]{0, 0, 1, 0},
                        new double[]{0, 0, 0, 1}
                    }
                );
            }
        }
        //-------------------
        public double[][] Content { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Matrix(double[][] content)
        {
            this.Content = content;
            if (content == null) throw new Exception("Content of matrix create was null");

            int columns = content[0].Length;
            for (int i = 1; i < content.Length; i++)
            {
                if (content[i].Length != columns) throw new Exception("Matrix wasn't a perfect C*R dimension");
            }
            Rows = content.Length;
            Columns = content[0].Length;
        }

        public static Matrix CreateTranslationMatrix(double x, double y, double z)
        {
            return new Matrix(
                new double[][]{
               new double[]{1, 0, 0, 0}, 
               new double[]{0, 1, 0, 0},
               new double[]{0, 0, 1, 0},
               new double[]{x, y, z, 1}
                }
            );
        }
        public static Matrix CreateScaleMatrix(double s)
        {
            return new Matrix(
                new double[][]{
               new double[]{s, 0, 0, 0}, 
               new double[]{0, s, 0, 0},
               new double[]{0, 0, s, 0},
               new double[]{0, 0, 0, 1}
                }
            );
        }
        public static Matrix CreateScaleMatrix(double sX, double sY, double sZ)
        {
            return new Matrix(
                new double[][]{
               new double[]{sX, 0, 0, 0}, 
               new double[]{ 0,sY, 0, 0},
               new double[]{ 0, 0,sZ, 0},
               new double[]{ 0, 0, 0, 1}
                }
            );
        }

       public static Matrix operator *(Matrix a, Matrix b)
       {
          if (a.Columns != b.Rows) throw new Exception("invalid dimension exception");
          int dimau = a.Rows;
          int dimav = a.Columns;
          int dimbu = a.Rows;
          int dimbv = a.Columns;
          double[][] final = new double[dimau][]; //now we fill from left to right... scanline style
          //alert(dimau + " " + dimav + " , " + dimbu + " " + dimbv);
          for (int _maty = 0; _maty < dimau; _maty++)
          {
             final[_maty] = new double[dimbv];
             for (int _matx = 0; _matx < dimbv; _matx++)
             {
                //Now we gather the array that we will need...
                double finalCell = 0;
                for (int _mati = 0; _mati < dimbu; _mati++)
                {
                   //alert(a[_maty][_mati] +"x"+ b[_mati][_matx] +"="+a[_maty][_mati] * b[_mati][_matx]);
                   finalCell += a.Content[_maty][_mati] * b.Content[_mati][_matx];
                }
                final[_maty][_matx] = finalCell;
             }
          }
          return new Matrix(final);
       }
    }
}
