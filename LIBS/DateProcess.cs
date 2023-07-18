using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathNet.Numerics;
namespace LIBS.Windows
{
    internal class DateProcess
    {
        #region 基线校正
        /// <summary>
        /// 窗口移动最小二乘拟合基线校正
        /// </summary>
        /// <param name="wavelen">波长</param>
        /// <param name="intensity">强度</param>
        /// <param name="n">窗口点数，应为奇数</param>
        /// <param name="t">多项式阶数</param>
        /// <param name="t2">拟合基线的多项式阶数</param>
        /// <param name="r">导数阈值</param>
        /// <param name="cy">基线</param>
        /// <param name="result">基线校正后强度</param>
        public void baselineCorrection(double[] wavelen, double[] intensity, int n, int t, int t2, double r, out double[] cy, out double[] result)
        {
            #region 去除峰值
            //double[,] a = new double[n, t + 1];
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j <= t; j++)
            //    {
            //        a[i, j] = Math.Pow(Convert.ToDouble(i - (n - 1) / 2), Convert.ToDouble(j));
            //    }
            //}
            //double[,] zhuanzhia;
            //inverseMatrix(a, out zhuanzhia);
            //double[,] chenga1;
            //multiplyMatrix(zhuanzhia, a, out chenga1);
            //double[,] nia;
            //oppMatrix(chenga1, out nia);
            //double[,] c;
            //multiplyMatrix(nia, zhuanzhia, out c);

            //double[] cc = new double[c.GetLength(1)];
            //for (int i = 0; i < c.GetLength(1); i++)
            //{
            //    cc[i] = c[1, i];
            //}


            int m = intensity.Length;
            //int a1 = (n - 1) / 2 + 1;
            //int b1 = m - (n - 1) / 2;
            //double[] xnew = new double[m];
            //for (int i = 0; i < xnew.Length; i++)
            //{
            //    xnew[i] = 0;
            //}
            //for (int j = a1; j <= b1; j++)
            //{
            //    int aa = j - (n - 1) / 2;
            //    int bb = j + (n - 1) / 2;
            //    for (int k = aa - 1; k <= bb - 1; k++)
            //    {
            //        xnew[j - 1] += intensity[k] * cc[k - aa + 1];
            //    }
            //}

            //List<int> indexlist = new List<int>();
            //for (int i = 3; i <= m - 2; i++)//选出导数小于阈值的强度序列
            //{
            //    //if (Math.Abs(xnew[i - 1]) >= r && Math.Abs(xnew[i - 2]) >= r && Math.Abs(xnew[i - 3]) >= r && Math.Abs(xnew[i]) >= r && Math.Abs(xnew[i + 1]) >= r)
            //    if (Math.Abs(xnew[i - 1]) <= r && Math.Abs(xnew[i - 2]) <= r && Math.Abs(xnew[i - 3]) <= r && Math.Abs(xnew[i]) <= r && Math.Abs(xnew[i + 1]) <= r)
            //    {
            //        indexlist.Add(i - 1);
            //    }
            //}
            #endregion
            List<int> indexlist = new List<int>();
            int step = 5;
            for (int i = step; i < intensity.Length - step; i++)
            {
                for (int j = 1; j <= step; j++)
                {
                    if(intensity[i]<=intensity[i-j]|| intensity[i] <= intensity[i + j])
                    {
                        indexlist.Add(i);
                    }
                }
            } 


            double[] l0 = new double[indexlist.Count];
            double[] l1 = new double[indexlist.Count];
            for (int i = 0; i < indexlist.Count; i++)
            {
                l0[i] = wavelen[indexlist[i]];
                l1[i] = intensity[indexlist[i]];
            }

            double[] xishu = curveFitting(l0, l1, t2);

            double[,] xx = new double[m, t2 + 1];
            for (int i = 0; i < m; i++)//拟合一个和原数据等长的基线
            {
                for (int j = 0; j <= t2; j++)
                {
                    xx[i, j] = Math.Pow(wavelen[i], Convert.ToDouble(j));
                }
            }

            cy = new double[m];
            multiplyMatrixToArray(xx, xishu, out cy);

            result = new double[intensity.Length];
            for (int i = 0; i < intensity.Length; i++)
            {
                result[i] = intensity[i] - cy[i];
            }
        }
        /// <summary>
        /// 矩阵的转置
        /// </summary>
        /// <param name="a">原始矩阵</param>
        /// <param name="b">转置后的矩阵</param>
        private void inverseMatrix(double[,] a, out double[,] b)
        {
            b = new double[a.GetLength(1), a.GetLength(0)];
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    b[i, j] = a[j, i];
                }
            }
        }

        /// <summary>
        /// 矩阵的乘
        /// </summary>
        private void multiplyMatrix(double[,] a, double[,] b, out double[,] c)
        {
            c = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    c[i, j] = 0;
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
        }

        /// <summary>
        /// 矩阵的逆
        /// </summary>
        /// <param name="a">矩阵</param>
        /// <param name="b">逆矩阵</param>
        private void oppMatrix(double[,] a, out double[,] b)
        {
            double X = surplusMatrix(a);
            X = 1 / X;
            double[,] B = new double[a.GetLength(0), a.GetLength(1)];
            double[,] SP = new double[a.GetLength(0), a.GetLength(1)];
            double[,] AB = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    for (int m = 0; m < a.GetLength(0); m++)
                    {
                        for (int n = 0; n < a.GetLength(1); n++)
                        { B[m, n] = a[m, n]; }
                    }

                    for (int x = 0; x < a.GetLength(1); x++)
                    { B[i, x] = 0; }
                    for (int y = 0; y < a.GetLength(0); y++)
                    { B[y, j] = 0; }
                    B[i, j] = 1;
                    SP[i, j] = surplusMatrix(B);
                    AB[i, j] = X * SP[i, j];
                }
            }
            double[,] tmp;
            inverseMatrix(AB, out tmp);
            b = new double[tmp.GetLength(0), tmp.GetLength(1)];
            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                for (int j = 0; j < tmp.GetLength(1); j++)
                {
                    b[i, j] = tmp[i, j];
                }
            }
        }

        /// <summary>
        /// 曲线拟合
        /// </summary>
        /// <param name="wavelen">波长</param>
        /// <param name="intensity">强度</param>
        /// <param name="order">阶数</param>
        /// <returns>系数数组</returns>
        public double[] curveFitting(double[] wavelen, double[] intensity, int order)
        {
            double[,] tmp = new double[wavelen.Length, order + 1];
            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                for (int j = 0; j <= order; j++)
                {
                    tmp[i, j] = Math.Pow(wavelen[i], Convert.ToDouble(j));
                }
            }

            double[,] zhuanzhi;
            inverseMatrix(tmp, out zhuanzhi);
            double[,] cheng1;
            multiplyMatrix(zhuanzhi, tmp, out cheng1);
            double[,] ni;
            oppMatrix(cheng1, out ni);
            double[,] cheng2;
            multiplyMatrix(ni, zhuanzhi, out cheng2);
            double[] cheng3;
            multiplyMatrixToArray(cheng2, intensity, out cheng3);

            return cheng3;
        }

        /// <summary>
        /// 矩阵乘一维数组
        /// </summary>
        /// <param name="a">矩阵</param>
        /// <param name="b">一维数组</param>
        /// <param name="c">输出数组</param>
        private void multiplyMatrixToArray(double[,] a, double[] b, out double[] c)
        {
            c = new double[a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                c[i] = 0;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    c[i] += a[i, j] * b[j];
                }
            }
        }

        /// <summary>
        /// 矩阵的行列式的值
        /// </summary>
        /// <param name="a">矩阵</param>
        /// <returns>行列式的值</returns>
        private double surplusMatrix(double[,] a)
        {
            int i, j, k, p, r, m, n;
            m = a.GetLength(0);
            n = a.GetLength(1);
            double X, temp = 1, temp1 = 1, s = 0, s1 = 0;

            if (n == 2)
            {
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        if ((i + j) % 2 > 0)
                        { temp1 *= a[i, j]; }
                        else
                        { temp *= a[i, j]; }
                    }
                }
                X = temp - temp1;
            }
            else
            {
                for (k = 0; k < n; k++)
                {
                    for (i = 0, j = k; i < m && j < n; i++, j++)
                    {
                        temp *= a[i, j];
                    }
                    if (m - i > 0)
                    {
                        for (p = m - i, r = m - 1; p > 0; p--, r--)
                        {
                            temp *= a[r, p - 1];
                        }
                    }
                    s += temp;
                    temp = 1;
                }
                for (k = n - 1; k >= 0; k--)
                {
                    for (i = 0, j = k; i < m && j >= 0; i++, j--)
                    {
                        temp1 *= a[i, j];
                    }
                    if (m - i > 0)
                    {
                        for (p = m - 1, r = i; r < m; p--, r++)
                        {
                            temp1 *= a[r, p];
                        }
                    }
                    s1 += temp1;
                    temp1 = 1;
                }
                X = s - s1;
            }
            return X;
        }
        #endregion

        #region 光谱滤噪
        /// <summary>
        /// 软阈值滤噪
        /// </summary>
        /// <param name="intensity">强度</param>
        /// <param name="type">小波名</param>
        /// <returns>滤噪后强度</returns>
        public double[] IDWT(double[] intensity, string type)
        {
            double[] wd1 = new double[intensity.Length];

            //--------------------------------选择小波基类型--------------------------------
            #region
            double[] p = null;
            switch (type)
            {
                case "hear":
                    //选择haar小波基
                    p = new double[] { 0.70710678118655, 0.70710678118655 };
                    break;
                case "db2":
                    //选择db2小波基
                    p = new double[] { 0.48296291314453, 0.83651630373780, 0.22414386804201, -0.12940952255126 };
                    break;
                case "db3":
                    //选择db3小波基
                    p = new double[] { 0.33267055295008, 0.80689150931109, 0.45987750211849, -0.13501102001025, -0.08544127388203, 0.03522629188571 };
                    break;
                case "db4":
                    //选择db4小波基
                    p = new double[] { 0.23037781330889, 0.71484657055291, 0.63088076792986, -0.02798376941686, -0.18703481171909, 0.03084138183556, 0.03288301166689, -0.01059740178507 };
                    break;
                case "db5":
                    //选择db5小波基
                    p = new double[] { 0.16010239797419, 0.60382926979719, 0.72430852843778, 0.13842814590132, -0.24229488706638, -0.03224486958464, 0.07757149384005, -0.00624149021280, -0.01258075199908, 0.00333572528547 };
                    break;
                default:
                    //选择db6小波基
                    p = new double[] { 0.11154074335011, 0.49462389039845, 0.75113390802110, 0.31525035170920, -0.22626469396544, -0.12976686756726, 0.09750160558732, 0.02752286553031, -0.03158203931849, 0.00055384220116, 0.00477725751195, -0.00107730108531 };
                    break;
            }
            #endregion

            #region
            //选择haar小波基
            //double[] p = new double[] { 0.70710678118655, 0.70710678118655 };
            //选择db2小波基
            //double[] p = new double[] { 0.48296291314453, 0.83651630373780, 0.22414386804201, -0.12940952255126 };
            //选择db3小波基
            //double[] p =new double[] { 0.33267055295008, 0.80689150931109, 0.45987750211849, -0.13501102001025, -0.08544127388203, 0.03522629188571 };
            //选择db4小波基
            //double[] p =new double[] { 0.23037781330889, 0.71484657055291, 0.63088076792986, -0.02798376941686, -0.18703481171909, 0.03084138183556, 0.03288301166689, -0.01059740178507 };
            //选择db5小波基
            //double[] p = new double[] { 0.16010239797419, 0.60382926979719, 0.72430852843778, 0.13842814590132, -0.24229488706638, -0.03224486958464, 0.07757149384005, -0.00624149021280, -0.01258075199908, 0.00333572528547 };
            //选择db6小波基
            //double[] p = new double[] { 0.11154074335011, 0.49462389039845, 0.75113390802110, 0.31525035170920, -0.22626469396544, -0.12976686756726, 0.09750160558732, 0.02752286553031, -0.03158203931849, 0.00055384220116, 0.00477725751195, -0.00107730108531 };

            //MATLAB中的小波基系数
            //double[] p = new double[] { 0.0788712160014344, 0.349751907037568, 0.531131879941213, 0.222915661465051, -0.159993299445874, -0.0917590320300334, 0.0689440464871973, 0.0194616048539635, -0.0223318741654753, 0.000391625576034686, 0.00337803118150508, -0.000761766902583715 };
            //double[] p = new double[] { 0.000630961046000000, -0.00115222485200000, -0.00519452402600000, 0.0113624592440000, 0.0188672353780000, -0.0574642344290000, -0.0396526485170000, 0.293667390895000, 0.553126452562000, 0.307157326198000, -0.0471127388650000, -0.0680381270510000, 0.0278136401530000, 0.0177358374380000, -0.0107563185170000, -0.00400101288600000, 0.00265266594600000, 0.000895594529000000, -0.000416500571000000, -0.000183829769000000, 4.40803540000000e-05, 2.20828570000000e-05, -2.30494200000000e-06, -1.26217500000000e-06};
            //double[] p = new double[] {-0.00551593375468968, 0.00124996104639266, 0.0316252813299409, -0.0148918756492222, -0.0513624849309040, 0.238952185666053, 0.556946391963959, 0.347228986478351, -0.0341615607932361, -0.0834316077058440, 0.00246830618592038, 0.0108923501632796};

            //sym6
            //double[] p = new double[] { -0.00551593375468968,	0.00124996104639266,0.0316252813299409,	-0.0148918756492222,-0.0513624849309040,0.238952185666053,0.556946391963959,0.347228986478351,-0.0341615607932361,-0.0834316077058440,0.00246830618592038,0.0108923501632796};
            #endregion

            //--------------------------------高通滤波器系数--------------------------------
            double[] q = new double[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                q[i] = Math.Pow(-1, i) * p[p.Length - 1 - i];
            }

            for (int i = 0; i < intensity.Length; i++)
            {
                wd1[i] = intensity[i];
            }

            //--------------------------------五级分解--------------------------------
            #region
            //分解一级的
            int temp11;
            int sclLen11 = wd1.Length;
            int pLen11 = p.Length;
            double[] scl11 = new double[sclLen11 / 2];
            double[] wvl11 = new double[sclLen11 / 2];

            double[] sw11 = new double[sclLen11 / 2];

            for (int i = 0; i < sclLen11 / 2; i++)
            {
                scl11[i] = 0.0;
                wvl11[i] = 0.0;
                for (int j = 0; j < pLen11; j++)
                {
                    temp11 = (j + i * 2) % sclLen11;
                    scl11[i] += p[j] * wd1[temp11];
                    wvl11[i] += q[j] * wd1[temp11];
                }
            }

            //软阈值去噪1

            //double thresholding1 = 0.3936 + 0.1829 * (Math.Log(sclLen11) / Math.Log(2))+2;
            double thresholding1 = Math.Sqrt(2 * Math.Log(sclLen11));

            for (int i = 0; i < sclLen11 / 2; i++)
            {
                if (wvl11[i] >= thresholding1)//高频信号高于阈值
                {
                    sw11[i] = wvl11[i] - thresholding1;
                }

                else
                {
                    if (wvl11[i] <= -thresholding1)
                        sw11[i] = wvl11[i] + thresholding1;
                    else
                        sw11[i] = 0;
                }

            }

            //分解二级的
            int temp22;
            int sclLen22 = scl11.Length;
            int pLen22 = p.Length;
            double[] scl22 = new double[sclLen22 / 2];
            double[] wvl22 = new double[sclLen22 / 2];

            double[] sw22 = new double[sclLen22 / 2];

            for (int i = 0; i < sclLen22 / 2; i++)
            {
                scl22[i] = 0.0;
                wvl22[i] = 0.0;
                for (int j = 0; j < pLen22; j++)
                {
                    temp22 = (j + i * 2) % sclLen22;
                    scl22[i] += p[j] * scl11[temp22];
                    wvl22[i] += q[j] * scl11[temp22];
                }
            }

            //软阈值去噪2
            //double thresholding2 = 0.3936 + 0.1829 * (Math.Log(sclLen22) / Math.Log(2))+2;
            double thresholding2 = Math.Sqrt(2 * Math.Log(sclLen22));

            for (int i = 0; i < sclLen22 / 2; i++)
            {
                if (wvl22[i] >= thresholding2)
                {
                    sw22[i] = wvl22[i] - thresholding2;
                }

                else
                {
                    if (wvl22[i] <= -thresholding2)
                        sw22[i] = wvl22[i] + thresholding2;
                    else
                        sw22[i] = 0;
                }
            }

            //分解三级的
            int temp33;
            int sclLen33 = scl22.Length;
            int pLen33 = p.Length;
            double[] scl33 = new double[sclLen33 / 2];
            double[] wvl33 = new double[sclLen33 / 2];

            double[] sw33 = new double[sclLen33 / 2];

            for (int i = 0; i < sclLen33 / 2; i++)
            {
                scl33[i] = 0.0;
                wvl33[i] = 0.0;
                for (int j = 0; j < pLen33; j++)
                {
                    temp33 = (j + i * 2) % sclLen33;
                    scl33[i] += p[j] * scl22[temp33];
                    wvl33[i] += q[j] * scl22[temp33];
                }
            }

            //软阈值去噪3
            //double thresholding3 = 0.3936 + 0.1829 * (Math.Log(sclLen33) / Math.Log(2))+2;
            double thresholding3 = Math.Sqrt(2 * Math.Log(sclLen33));

            for (int i = 0; i < sclLen33 / 2; i++)
            {
                if (wvl33[i] >= thresholding3)
                {
                    sw33[i] = wvl33[i] - thresholding3;
                }

                else
                {
                    if (wvl33[i] <= -thresholding3)
                        sw33[i] = wvl33[i] + thresholding3;
                    else
                        sw33[i] = 0;
                }
            }

            //分解四级的
            int temp44;
            int sclLen44 = scl33.Length;
            int pLen44 = p.Length;
            double[] scl44 = new double[sclLen44 / 2];
            double[] wvl44 = new double[sclLen44 / 2];

            double[] sw44 = new double[sclLen44 / 2];

            for (int i = 0; i < sclLen44 / 2; i++)
            {
                scl44[i] = 0.0;
                wvl44[i] = 0.0;
                for (int j = 0; j < pLen44; j++)
                {
                    temp44 = (j + i * 2) % sclLen44;
                    scl44[i] += p[j] * scl33[temp44];
                    wvl44[i] += q[j] * scl33[temp44];
                }
            }

            //软阈值去噪4
            //double thresholding4 = 0.3936 + 0.1829 * (Math.Log(sclLen44) / Math.Log(2))+2;
            double thresholding4 = Math.Sqrt(2 * Math.Log(sclLen44));


            for (int i = 0; i < sclLen44 / 2; i++)
            {
                if (wvl44[i] >= thresholding4)
                {
                    sw44[i] = wvl44[i] - thresholding4;
                }

                else
                {
                    if (wvl44[i] <= -thresholding4)
                        sw44[i] = wvl44[i] + thresholding4;
                    else
                        sw44[i] = 0;
                }
            }

            //分解五级的
            int temp55;
            int sclLen55 = scl44.Length;
            int pLen55 = p.Length;
            double[] scl55 = new double[sclLen55 / 2];
            double[] wvl55 = new double[sclLen55 / 2];

            double[] sw55 = new double[sclLen55 / 2];

            for (int i = 0; i < sclLen55 / 2; i++)
            {
                scl55[i] = 0.0;
                wvl55[i] = 0.0;
                for (int j = 0; j < pLen55; j++)
                {
                    temp55 = (j + i * 2) % sclLen55;
                    scl55[i] += p[j] * scl44[temp55];
                    wvl55[i] += q[j] * scl44[temp55];
                }
            }

            //软阈值去噪5
            //double thresholding5 = 0.3936 + 0.1829 * (Math.Log(sclLen55) / Math.Log(2))+2;
            double thresholding5 = Math.Sqrt(2 * Math.Log(sclLen55));

            for (int i = 0; i < sclLen55 / 2; i++)
            {
                if (wvl55[i] >= thresholding5)
                {
                    sw55[i] = wvl55[i] - thresholding5;
                }

                else
                {
                    if (wvl55[i] <= -thresholding5)
                        sw55[i] = wvl55[i] + thresholding5;
                    else
                        sw55[i] = 0;
                }
            }
            #endregion

            //--------------------------------五级合成--------------------------------
            #region
            //合成第五级
            int temp50;
            int sclLen50 = scl55.Length;
            int pLen50 = p.Length;
            double[] scl50 = new double[sclLen50 * 2];

            for (int i = 0; i < sclLen50; i++)
            {
                scl50[2 * i + 1] = 0.0;
                scl50[2 * i] = 0.0;
                for (int j = 0; j < pLen50 / 2; j++)
                {
                    temp50 = (i - j + sclLen50) % sclLen50;
                    scl50[2 * i + 1] += p[2 * j + 1] * scl55[temp50] + q[2 * j + 1] * sw55[temp50];
                    scl50[2 * i] += p[2 * j] * scl55[temp50] + q[2 * j] * sw55[temp50];
                }
            }

            //合成第四级
            int temp40;
            int sclLen40 = scl44.Length;
            int pLen40 = p.Length;
            double[] scl40 = new double[sclLen40 * 2];

            for (int i = 0; i < sclLen40; i++)
            {
                scl40[2 * i + 1] = 0.0;
                scl40[2 * i] = 0.0;
                for (int j = 0; j < pLen40 / 2; j++)
                {
                    temp40 = (i - j + sclLen40) % sclLen40;
                    scl40[2 * i + 1] += p[2 * j + 1] * scl44[temp40] + q[2 * j + 1] * sw44[temp40];
                    scl40[2 * i] += p[2 * j] * scl44[temp40] + q[2 * j] * sw44[temp40];
                }
            }

            //合成第三级
            int temp41;
            int sclLen41 = scl33.Length;
            int pLen41 = p.Length;
            double[] scl41 = new double[sclLen41 * 2];

            for (int i = 0; i < sclLen41; i++)
            {
                scl41[2 * i + 1] = 0.0;
                scl41[2 * i] = 0.0;
                for (int j = 0; j < pLen41 / 2; j++)
                {
                    temp41 = (i - j + sclLen41) % sclLen41;
                    scl41[2 * i + 1] += p[2 * j + 1] * scl33[temp41] + q[2 * j + 1] * sw33[temp41];
                    scl41[2 * i] += p[2 * j] * scl33[temp41] + q[2 * j] * sw33[temp41];
                }
            }

            //合成第二级
            int temp42;
            int sclLen42 = scl41.Length;
            int pLen42 = p.Length;
            double[] scl42 = new double[sclLen42 * 2];

            for (int i = 0; i < sclLen42; i++)
            {
                scl42[2 * i + 1] = 0.0;
                scl42[2 * i] = 0.0;
                for (int j = 0; j < pLen42 / 2; j++)
                {
                    temp42 = (i - j + sclLen42) % sclLen42;
                    scl42[2 * i + 1] += p[2 * j + 1] * scl41[temp42] + q[2 * j + 1] * sw22[temp42];
                    scl42[2 * i] += p[2 * j] * scl41[temp42] + q[2 * j] * sw22[temp42];
                }
            }

            //合成第一级
            int temp43;
            int sclLen43 = scl42.Length;
            int pLen43 = p.Length;
            double[] scl43 = new double[sclLen43 * 2];//经过处理的最终数据

            for (int i = 0; i < sclLen43; i++)
            {
                scl43[2 * i + 1] = 0.0;
                scl43[2 * i] = 0.0;
                for (int j = 0; j < pLen43 / 2; j++)
                {
                    temp43 = (i - j + sclLen43) % sclLen43;
                    scl43[2 * i + 1] += p[2 * j + 1] * scl42[temp43] + q[2 * j + 1] * sw11[temp43];
                    scl43[2 * i] += p[2 * j] * scl42[temp43] + q[2 * j] * sw11[temp43];
                }
            }
            #endregion

            return scl43;
        }
        /// <summary>
        /// 移动窗口均值滤波器
        /// </summary>
        /// <param name="intensity">强度数组</param>
        /// <param name="window">窗口长度</param>
        /// <returns>滤噪完的数组</returns>
        public double[] wave_filiter(List<double> intensity, int window)
        {
            double[] outputarry = new double[intensity.Count];
            for (int i = 0; i < intensity.Count; i++)
            {
                int windowStart = Math.Max(i - window / 2, 0);
                int windowEnd = Math.Min(i + window / 2, intensity.Count - 1);
                double windowSum = 0;
                for (int j = windowStart; j <= windowEnd; j++)
                {
                    windowSum += intensity[j];
                }
                outputarry[i] = windowSum / (windowEnd - windowStart + 1);
            }
            return outputarry;
        }
        #endregion

        #region 定性分析
        /// <summary>
        /// 定性分析
        /// </summary>
        /// <param name="wavelen">波长</param>
        /// <param name="intensity">强度</param>
        /// <param name="minWave">最小值</param>
        /// <param name="maxWave">最大波长</param>
        /// <param name="minIntensity">最低波长值</param>
        /// <param name="stepLen">窗口步长</param>
        /// <param name="NIST">NIST原子谱线数据库</param>
        /// <param name="position">波峰位置</param>
        /// <param name="eleTypeList">类型</param>
        /// <param name="eleNameList">元素名</param>
        /// <param name="eleIntensity">强度值</param>
        public void qualitativeAnalysis(double[] wavelen, double[] intensity, double minWave, double maxWave, double minIntensity, int stepLen, string[,] NIST, out int[] position, out string[] eleTypeList, out string[] eleNameList, out double[] eleIntensity)
        {
            int top, bottom, max = 0;
            List<int> indexList = new List<int>();//存储识别的峰的索引
            List<string> elementName = new List<string>();//存储元素谱线类型
            List<string> listName = new List<string>();//存储元素名
            List<double> listIntensity = new List<double>();//存储特征峰强度

            for (int i = 1; i <= wavelen.Length; i++)
            {
                if (i - stepLen <= 1)
                {
                    top = 0;//范围内最左边的值
                }
                else
                {
                    top = i - stepLen - 1;
                }

                if (i + stepLen > wavelen.Length)
                {
                    bottom = wavelen.Length - 1;//范围内最右边的值
                }
                else
                {
                    bottom = i + stepLen - 1;
                }

                int n = 0;
                int length = bottom - top + 1;
                int[] xyz = new int[length];
                for (int j = top; j <= bottom; j++)
                {
                    if (intensity[j] < intensity[i - 1])
                    {
                        xyz[n] = 0;
                    }
                    else
                    {
                        xyz[n] = 1;
                    }
                    n++;
                }
                if (getSum(xyz, length) == 1)
                {
                    max = i - 1;//找到极大值

                    if ((wavelen[max] >= minWave) & (wavelen[max] <= maxWave) & (intensity[max] >= minIntensity))
                    {
                        List<double> minDele = new List<double>();//存储波长和标准数据的差值
                        List<string> tmpEle = new List<string>();//存储差值在阈值范围内的元素谱线类型
                        List<string> tmpList = new List<string>();//存储差值在阈值范围内的元素名
                        for (int k = 0; k < NIST.GetLength(0); k++)
                        {
                            if (NIST[k, 1] != "NaN" & NIST[k, 1] != "")
                            {
                                double tmp = 0.0;
                                try
                                {
                                    tmp = Convert.ToDouble(NIST[k, 1]);
                                }
                                catch
                                {
                                    MessageBox.Show("数据类型错误！");
                                    break;
                                }
                                if (Math.Abs(tmp - wavelen[max]) < 0.08)
                                {
                                    minDele.Add(Math.Abs(tmp - wavelen[max]));
                                    tmpEle.Add(NIST[k, 0]);
                                    tmpList.Add(NIST[k, 2]);
                                }
                                if (k == (NIST.GetLength(0) - 1))
                                {
                                    if (minDele.Count > 0)
                                    {
                                        indexList.Add(max);
                                        string eleName, listEleName;
                                        getElement(minDele, tmpEle, tmpList, out eleName, out listEleName);//一个峰匹配多个元素时，取最接近的元素
                                        elementName.Add(eleName);//取得的最接近元素的谱线类型
                                        listName.Add(listEleName);//取得的最接近元素的元素名
                                        listIntensity.Add(intensity[max]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            position = new int[indexList.Count];
            for (int j = 0; j < position.Length; j++)
            {
                position[j] = indexList[j];
            }
            eleTypeList = new string[elementName.Count];
            for (int k = 0; k < eleTypeList.Length; k++)
            {
                eleTypeList[k] = elementName[k];
            }
            eleNameList = new string[listName.Count];
            for (int w = 0; w < eleNameList.Length; w++)
            {
                eleNameList[w] = listName[w];
            }
            //eleNameList = eleNameList.Distinct().ToArray();//去掉重复元素名
            eleIntensity = new double[listIntensity.Count];
            for (int x = 0; x < eleIntensity.Length; x++)
            {
                eleIntensity[x] = listIntensity[x];
            }
        }
        /// <summary>
        /// 数组求和
        /// </summary>
        /// <param name="bf"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private int getSum(int[] bf, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += bf[i];
            }

            return sum;
        }
        /// <summary>
        /// 取最接近的元素
        /// </summary>
        /// <param name="dValue">差值列表</param>
        /// <param name="eleName">有罗马数字元素名列表</param>
        /// <param name="listName">无罗马数字元素名列表</param>
        /// <param name="elementName">取到的有罗马数字元素名列表</param>
        /// <param name="listName">取到的无罗马数字元素名列表</param>
        private void getElement(List<double> dValue, List<string> eleName, List<string> listName, out string elementName, out string listEleName)
        {
            elementName = eleName[0];
            listEleName = listName[0];

            if (dValue.Count > 1)
            {
                double minDele = Convert.ToDouble(dValue[0]);
                for (int i = 1; i < dValue.Count; i++)
                {
                    if (Convert.ToDouble(dValue[i]) < minDele)
                    {
                        elementName = eleName[i];
                        listEleName = listName[i];
                        minDele = Convert.ToDouble(dValue[i]);
                    }
                }
            }
        }

        #endregion

        #region   基线校正2采用Savitzky-Golay卷积平滑
        //此方法计算滤波器系数并将其应用于输入数组
        public double[] FilterArray(double[] inputArray, int windowSize, int polyOrder)
        {
            int fl = windowSize / 2;
            int length = inputArray.Length;
            var result = new double[length];
            var coefficients = ComputeCoefficients(windowSize, polyOrder);

            for (int i = 0; i < length; i++)
            {
                int windowStart = i - fl;
                int windowEnd = i + fl;

                //模式："最近"
                if (windowStart < 0)
                    windowStart = 0;
                if (windowEnd >= length)
                    windowEnd = length - 1;

                double filteredValue = 0;

                for (int j = windowStart, k = 0; j <= windowEnd; j++, k++)
                    filteredValue += inputArray[j] * coefficients[k];

                result[i] = filteredValue;
            }

            return result;
        }

        // 此方法用于计算 Savitzky-Golay 滤波器的系数
        private double[] ComputeCoefficients(int windowSize, int polynomialOrder)
        {
            var m = (windowSize - 1) / 2;
            var coefficients = new double[windowSize];

            for (int u = -m; u <= m; u++)
            {
                double sum = 0;
                for (int n = 0; n <= polynomialOrder; n++)
                {
                    var a = (double)Factorial(n) / (Factorial(u + m - n) * Factorial(n) * Factorial(m - n));
                    var b = Math.Pow(2 * m - 2 * n, u);
                    var factor = a * b;
                    sum += factor;
                }
                coefficients[u + m] = sum / Factorial(m);
            }

            var sumCoefficients = coefficients.Sum();
            return coefficients.Select(x => x / sumCoefficients).ToArray();
        }

        // 此方法用于计算阶乘
        private int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
        /// <summary>
        /// Savitzky-Golay 滤波的多项式基线校正
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="count"></param>
        /// <param name="n"></param>
        /// <param name="windowSize"></param>
        /// <param name="polyOrder"></param>
        /// <returns></returns>
        public double[] BaselineCorrect2(double[] wave, double[] count, int n, int windowSize, int polyOrder)
        {
            double[] y_filter=new double[count.Length];
            for (int i = 0; i < count.Length; i++)
            {
                y_filter[i]=count[i];
            }
            //y_filter = FilterArray(count, windowSize, polyOrder);
            double[] y_uniform = new double[y_filter.Length];
            for (int i = 0; i < y_filter.Length; i++)
            {
                y_uniform[i] = (y_filter[i] - y_filter.Min()) / (y_filter.Max() - y_filter.Min());
            }
            var p = Fit.Polynomial(wave, y_uniform, n);
            double[] yFit = new double[wave.Length], r0 = new double[wave.Length];
            for (int i = 0; i < wave.Length; i++)
            {
                yFit[i] = Polynomial.Evaluate(wave[i], p);
            }
            //计算拟合值
            for (int i = 0; i < wave.Length; i++)
            {
                r0[i] = y_uniform[i] - yFit[i];
            }
            //计算残差
            double dev0 = cancha(r0);
            //峰值消除
            List<double> x_remove0 = new List<double>();
            List<double> y_remove0 = new List<double>();
            for (int i = 0; i < y_uniform.Length; i++)
            {
                if (y_uniform[i] < yFit[i])
                {
                    y_remove0.Add(y_uniform[i]);
                    x_remove0.Add(wave[i]);
                }
            }
            int j = 0;
            double[] p2 = new double[n + 1];
            bool judge = true;
            List<double> dev = new List<double>();
            while (judge)
            {
                double[] p1 = Fit.Polynomial(x_remove0.ToArray(), y_remove0.ToArray(), n);
                p2 = p1;
                double[] yFit1 = new double[x_remove0.Count];
                double[] r1 = new double[x_remove0.Count];
                for (int i = 0; i < yFit1.Length; i++)
                {
                    yFit1[i] = Polynomial.Evaluate(x_remove0.ToArray()[i], p1);
                }

                for (int i = 0; i < yFit1.Length; i++)
                {
                    r1[i] = y_remove0[i] - yFit1[i];
                }
                double dev1 = cancha(r1);
                dev.Add(dev1);
                if (j == 0)
                {
                    judge = Math.Abs((dev[j] - dev0) / dev[j]) > 0.05;
                }
                else
                {
                    judge = Math.Abs((dev[j] - dev[j - 1]) / dev[j]) > 0.05;
                }
                //光谱重建
                for (int i = 0; i < y_remove0.Count; i++)
                {
                    if (y_remove0[i] <= yFit1[i])
                    {
                        y_remove0[i] = yFit1[i];
                    }
                }
                j++;
            }
            double[] baseline = new double[wave.Length];
            double[] baseline_correction = new double[wave.Length];
            for (int i = 0; i < wave.Length; i++)
            {
                baseline[i] = Polynomial.Evaluate(wave[i], p2);
            }
            for (int i = 0; i < baseline.Length; i++)
            {
                baseline_correction[i] = y_uniform[i] - baseline[i];
            }
            double[] recover1 = new double[wave.Length];
            double[] recover2 = new double[wave.Length];
            for (int i = 0; i < wave.Length; i++)
            {
                recover1[i] = baseline[i] * (y_filter.Max() - y_filter.Min()) + y_filter.Min();
                recover2[i] = baseline_correction[i] * (y_filter.Max() - y_filter.Min()) + y_filter.Min();
            }
            return recover2;
        }
        //计算残差
        private double cancha(double[] inputarray)
        {
            double mean = inputarray.Average();
            double sum = 0;
            double[] vs = new double[inputarray.Length];
            for (int i = 0; i < inputarray.Length; i++)
            {
                vs[i] = Math.Pow(inputarray[i] - mean, 2);
            }
            sum = vs.Sum();
            return Math.Sqrt(sum / inputarray.Length);
        }

        #endregion
    }
}
