using System;
using System.Collections.Generic;

namespace Transforms
{
    /// <summary>
    /// Static Class containing all 'Transform' functions. Arguments named 't' should all be within the range of 0-1.
    /// <para> The argument 't' is the variable value that you input into the equation, 't' being the interpolating value. </para>
    /// </summary>
    public static class Transform
    {

        #region Simple

        /// <summary> Returns the result of Sin(t), normalized such that one wave period occurs over an increment of 1. </summary>
        /// <param name="t"> Input value, used like interpolation, should be between 0-1. </param>
        /// <param name="xOffset"> X offset for sine wave. </param>
        /// <param name="magnitude"> The most extreme y-value deviation from the midline that this function will return. </param>
        /// <param name="midline"> The 'y-value' for the center between the peaks and troughs of the wave. </param>
        public static float Sine(float t, float xOffset = 0, float magnitude = 0.5f, float midline = 0.5f)
        {
            // 'sine' produces the result of Math.Sin, which (by default) is normalized to the 0-1 domain/range.
            // https://www.desmos.com/calculator/xp5uwiwywl

            // magnitude 0.5, center @ y=0.5, period = 1
            /* sample values:
             
            [0.00 ,   0.50],
            [0.25 ,     1.00],
            [0.50 ,   0.50],
            [0.75 , 0.00],
            [1.00 ,   0.50]
             
            */
            double twoPi = Math.PI * 2.0;
            double ret = (Math.Sin(twoPi * (t + xOffset)) * magnitude) + midline;
            return (float)ret;
        }

        /// <summary> Flips given 't' value. </summary>
        /// <param name="t"> Value to flip. </param>
        public static float Flip(float t) { return 1.0f - t; }

        /// <summary> Squares given 't' value. </summary>
        /// <param name="t"> Value to square. </param>
        public static float Square(float t) { return t * t; }

        /// <summary> Returns a mix of values 'a' and 'b', with the given mult applied to 'b', and (1 - mult) applied to 'a'. </summary>
        /// <param name="a"> The first input value. </param>
        /// <param name="b"> The second input value. </param>
        /// <param name="bMult"> The value to multiply 'b' by. Must be within the 0-1 range. </param>
        /// <returns></returns>
        public static float Mix(float a, float b, float bMult)
        {
            if (bMult > 1.0f)
                bMult = 1.0f;
            else if (bMult < 0.0f)
                bMult = 0.0f;

            return ((1.0f - bMult) * a) + (bMult * b);
        }

        /// <summary> Returns a value which is a mix of [t] between [a] and [b].  </summary>
        /// <param name="a"> The value to return when 't' equals 0. </param>
        /// <param name="b"> The value to return when 't' equals 1. </param>
        /// <param name="t"> Value to fade from 'a' to 'b' with. </param>
        public static float Crossfade(float a, float b, float t) { return Mix(a, b, t); }

        /// <summary> Multiplies 't' by the given value 'a'. </summary>
        /// <param name="t"> The value to multiply by 'a'. </param>
        /// <param name="a"> The value to multiply 't' by. </param>
        public static float Scale(float t, float a) { return t * a; }



        /// <summary> Multiplies value 't' by itself 'x' times. </summary>
        /// <param name="t"> The value to be raised to a power. </param>
        /// <param name="x"> The power to raise 't' to. </param>
        /// <returns></returns>
        public static float Power(float t, int x) // can be somewhat easily optimised
        {
            if (x == 0)
                return 1;

            float tRet = t;

            while (x > 1)
            {
                tRet *= t;
                x--;
            }

            return tRet;
        }

        /// <summary> Returns the absolute value of the given float. </summary>
        /// <param name="a"> Float to get the absolute value of. </param>
        public static float Abs(float a)
        {
            if (a < 0)
                a *= -1;

            return a;
        }


        /// <summary> If 't' is less than 'x', 't' is made equal to 'x'. </summary>
        /// <param name="t"> Value to not allow to be less than 'x'. </param>
        /// <param name="x"> Value which 't' is not allowed to be less than. </param>
        public static float Min(float t, float x)
        {
            if (t < x)
                return x;
            else
                return t;
        }

        /// <summary> If 't' is greater than 'x', 't' is made equal to 'x' </summary>
        /// <param name="t"> Value to not allow to be greater than 'x'. </param>
        /// <param name="x"> Value which 't' is not allowed to be greater than. </param>
        public static float Max(float t, float x)
        {
            if (t > x)
                return x;
            else
                return t;
        }

        /// <summary> Uses a known linear equation to get a value. Returns 'f(x) = b + m*x' </summary>
        /// <param name="x"> The 'x-value' to input into the equation. </param>
        /// <param name="m"> The 'slope' of the equation. </param>
        /// <param name="b"> The 'y-intercept' of the equation. </param>
        public static float LinEq(float x, float m, float b) { return (m * x) + b; }

        /// <summary> Returns an interpolated value between 'min' and 'max' </summary>
        /// <param name="t"> Value to interpolate with. </param>
        /// <param name="min"> Value to return when 't' equals 0. </param>
        /// <param name="max"> Value to return when 't' equals 1. </param>
        public static float Lerp(float t, float min, float max)
        { return min + ((max - min) * t); }

        /// <summary> Returns a value which represents the fraction between [min] and [max] that [a] is. </summary>
        /// <param name="a"> Value between [min] and [max]. Note: if [a]&gt;[max], ret will be &gt;[1]. If [a]&lt;[min], ret will be &lt;[0]. </param>
        /// <param name="min"> The minimum expected value for [a] to be. </param>
        /// <param name="max"> The maximum expected value for [a] to be. </param>
        public static float InverseLinInterpolate(float a, float min, float max)
        {
            // when min == max, any value of 'a' will give the same result.
            if (min == max)
            {
                return max;
            }

            float a2 = a - min;
            float max2 = max - min;
            return a2 / max2;

            #region Old Code

            // step 1: normalize values to 0(min-min)->X(max-min)
            //float a2   = a   - min;
            //float min2 = min - min;
            //float max2 = max - min;

            //return a2 / max2;

            // step 2: determine result
            //if (a2 < min2)
            //{
            //    // ret < 0

            //    float ret = Abs(a2) / max2;
            //    return -1.0f * ret;
            //}
            //else if (a2 > max2)
            //{
            //    // ret > 1
            //    return 1;
            //}
            //else
            //{
            // min <= [ret]
            // which is okay because method is okay with returning values greater than 1
            //return a2 / max2;
            //}

            #endregion
        }


        #endregion

        private static float _floatPowCommon(float t, float pow, Func<float, int, float> func)
        {
            var bPow = new BetterPow(pow);

            return Mix(func(t, bPow.PowWhole), func(t, bPow.PowWhole + 1), bPow.PowDecimal);
        }

        /// <summary> Returns a value which increases slowly at low values of 't', and more rapidly as 't' approaches 1. </summary>
        /// <param name="t"> Value to apply 'SmoothStart' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStartX(float t, float pow) { return _floatPowCommon(t, pow, SmoothStartX); }

        /// <summary> Returns a value which increases slowly at low values of 't', and more rapidly as 't' approaches 1. </summary>
        /// <param name="t"> Value to apply 'SmoothStart' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStartX(float t, int pow) { return Power(t, pow); }



        /// <summary> Returns a value which increases rapidly at low values of 't', but more slowly as 't' approaches 1. </summary>
        /// <param name="t"> Value to apply 'SmoothStop' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStopX(float t, float pow) { return _floatPowCommon(t, pow, SmoothStopX); }

        /// <summary> Returns a value which increases rapidly at low values of 't', but more slowly as 't' approaches 1. </summary>
        /// <param name="t"> Value to apply 'SmoothStop' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStopX(float t, int pow) { return Flip(Power(Flip(t), pow)); }



        /// <summary> Returns a value which increases slowly when 't' is near 0 and 1, but increases rapidly when 't' is near 0.5. </summary>
        /// <param name="t"> Value to apply 'SmoothStep' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStepX(float t, float pow) { return _floatPowCommon(t, pow, SmoothStepX); }

        /// <summary> Returns a value which increases slowly when 't' is near 0 and 1, but increases rapidly when 't' is near 0.5. </summary>
        /// <param name="t"> Value to apply 'SmoothStep' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float SmoothStepX(float t, int pow) { return Crossfade(SmoothStartX(t, pow), SmoothStopX(t, pow), t); }


        /// <summary> Returns a value which equals 0 when [t] equals 0 or 1, but returns 1 when [t] equals 0.5. </summary>
        /// <param name="t"> Value to apply 'Arch' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float ArchX(float t, float pow) { return _floatPowCommon(t, pow, ArchX); }

        /// <summary> Returns a value which equals 0 when [t] equals 0 or 1, but returns 1 when [t] equals 0.5. </summary>
        /// <param name="t"> Value to apply 'Arch' function to. </param>
        /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
        public static float ArchX(float t, int pow) { return Power(4 * t * Flip(t), pow); }




        /// <summary> Detemines a linear equation which touches points (x,y) and (1,1) and plugs in 't' as the 'x-value' to get the result. </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        public static float LinEqGivenPoint_ToOne(float t, (float X, float Y) p)
        {
            // needs to go from point p.X,p.Y (assuming within 0-1 range) thru 1,1

            // slope is (1-y)/(1-x)
            // y-intercept is (y - mx) (with y and x in this case being coordinates of one of the points)
            // so equation to get Y at point T given line between x,y and 1,1 is
            // ret =      m  *  (t) + b
            // ret = ((1-y)/(1-x))(t) + (1 - ((1-y)/(1-x))*1)

            float m = (1 - p.Y) / (1 - p.X);
            float b = (1 - m);
            return LinEq(t, m, b);
        }

        /// <summary> Detemines a linear equation which touches points (0,0) and (p.X,p.Y) and plugs in 't' to get the result. </summary>
        /// <param name="t"> The interpolant value to use between the point (0,0) and the given point. </param>
        /// <param name="point"> The other point to use when calculating the line from (0,0). </param>
        public static float LinEqGivenPoint_FromZero(float t, (float X, float Y) point)
        {
            return LinEqGivenPoints(t, (0, 0), point);
        }



        /// <summary> Determines a linear equation which touches points (x1,y1) and (x2,y2) and plugs in 't' to get the result. Works outside the 0-1 domain. </summary>
        /// <param name="t"> Value to plug into equation as 'x'. </param>
        /// <param name="pointA"> One of the two points which the equation passes through. </param>
        /// <param name="pointB"> The other of the two points which the equation passes through. </param>
        /// <returns></returns>
        public static float LinEqGivenPoints(float t, (float X, float Y) pointA, (float X, float Y) pointB)
        {
            // slope is (y2-y1)/(x2-x1)
            // y-intercept is (y - m*x) (with y and x in this case being coordinates of one of the points)
            // so equation to get Y at point T given line between x,y and 1,1 is
            // ret =       m   *  (t) +            b
            // ret = ((y2-y1)/(x2-x1))(t) + (y2 - ((y2-y1)/(x2-x1))*x2)

            float m = (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
            float b = (pointA.Y - (m * pointA.X));
            return LinEq(t, m, b);
        }


        #region Bezier

        public static List<int> GetPascalRow(int rowIndex)
        {
            #region Diagram

            //================================================================================
            // rowIndex 0 =                            {  1  }
            // rowIndex 1 =                         {  1  ,  1  }
            // rowIndex 2 =                      {  1  ,  2  ,  1  }
            // rowIndex 3 =                   {  1  ,  3  ,  3  ,  1  }
            // rowIndex 4 =                {  1  ,  4  ,  6  ,  4  ,  1  }
            // rowIndex 5 =             {  1  ,  5  , 10  ,  10 ,  5  ,  1  }
            // rowIndex 5 =          {  1  ,  6  , 15  , 20  ,  15 ,  6  ,  1  }
            // rowIndex 7 =       {  1  ,  7  , 21  , 35  ,  35 ,  21 ,  7  ,  1  }
            // rowIndex 8 =    {  1  ,  8  , 28  , 56  , 70  ,  56 ,  28 ,  8  ,  1  }
            // rowIndex 9 = {  1  ,  9  , 36  , 84  , 126 , 126 ,  84 ,  36 ,  9  ,  1  }
            //================================================================================
            // rowIndex 0 (1) = {  1  }
            // rowIndex 1 (2) = {  1  ,  1  }
            // rowIndex 2 (3) = {  1  ,  2  ,  1  }
            // rowIndex 3 (4) = {  1  ,  3  ,  3  ,  1  }
            // rowIndex 4 (5) = {  1  ,  4  ,  6  ,  4  ,   1 }
            // rowIndex 5 (6) = {  1  ,  5  , 10  , 10  ,   5 ,   1 }
            // rowIndex 6 (7) = {  1  ,  6  , 15  , 20  ,  15 ,   6 ,  1  }
            // rowIndex 7 (8) = {  1  ,  7  , 21  , 35  ,  35 ,  21 ,  7  ,  1  }
            // rowIndex 8 (9) = {  1  ,  8  , 28  , 56  ,  70 ,  56 , 28  ,  8  ,  1  }
            // rowIndex 9 (10)= {  1  ,  9  , 36  , 84  , 126 , 126 , 84  , 36  ,  9  ,  1  }
            //================================================================================

            #endregion

            #region Code

            if (rowIndex < 0) return new List<int> { };
            else if (rowIndex == 0) return new List<int> { 1 };
            else
            {
                var prevRow = GetPascalRow(rowIndex - 1);
                var retRow = new List<int> { };

                for (int i = 0; i <= rowIndex; i++)
                {
                    int add;

                    if (i == 0 || i == rowIndex)
                        add = 1;
                    else
                        add = prevRow[i] + prevRow[i - 1];

                    retRow.Add(add);
                }

                return retRow;
            }

            #endregion
        }


        /// <summary> Returns the corresponding value of the normalized Bezier curve supplied via [vals] at x=[t].
        /// <para> This method returns a 'normalized' value, meaning that when [t]=0, method returns 0, and when [t]=1, returns 1. </para></summary>
        /// <param name="t"> Value to use as 'x' to get corresponding Bezier curve value. </param>
        /// <param name="mids"> Collection of evenly spaced points between [0,1], exclusive, to form the Bezier curve from. </param>
        public static float NormalizedBezierX(float t, List<float> mids)
        {
            #region example data:

            #region mids.count = 0

            // (midsfull makes everything line up better)
            // mids =     {...... ......}
            // midsfull = { [0]0 , [1]1 }
            // pascal   = { [0]1 , [1]1 }

            // numMidsFull == 2 == numPascal

            // ex: if (i == 1)
            // ret += pas[i] * midsFull[i] * Power(s, (numMidsFull - 1) - i) * Power(t, i)
            // ret += pas[1] * midsFull[1] * Power(s, (2 - 1) - 1)           * Power(t, 1)
            // ret += 1      * 1           * s^0                             * t^1


            // ret += 1 * 0 * 1 * 1 ( when i == 0)
            // ret += 1 * 1 * 1 * 1 ( when i == 1)

            // result is linear

            // full breakdown follows

            // i = 0 | 1  * 0  * 0^0 * 1^1
            // i = 1 | 1  * 1  * 1^1 * 0^0

            #endregion


            #region mids.count = 4

            // (midsfull makes everything line up better)
            // mids =     {......  [0]0.5 , [1]1.0 , [2]0.7 , [3]0.2  ......}
            // midsfull = { [0]0 , [1]0.5 , [2]1.0 , [3]0.7 , [4]0.2 , [5]1 }
            // pascal   = { [0]1 , [1]5   , [2]10  , [3]10  , [4]5   , [5]1 }

            // numMidsFull == 6 == numPascal

            // ex: if (i == 3)
            // ret += pas[i] * midsFull[i] * Power(s, (numMidsFull - 1) - i) * Power(t, i)
            // ret += pas[3] * midsFull[3] * Power(s, (6 - 1) - 3)           * Power(t, 3)
            // ret += 10     * 0.7         * s^2                             * t^3

            // full breakdown follows

            // i = 0 | 1  * 0   * s^5 * t^0
            // i = 1 | 5  * 0.5 * s^4 * t^1
            // i = 2 | 10 * 1.0 * s^3 * t^2
            // i = 3 | 10 * 0.7 * s^2 * t^3
            // i = 4 | 5  * 0.2 * s^1 * t^4
            // i = 5 | 1  * 1   * s^0 * t^5

            #endregion


            #endregion

            // init 'midsfull' for easier programming
            var allVals = new List<float> { 0 };
            allVals.AddRange(mids); allVals.Add(1);

            return BezierX(t, allVals);
        }

        /// <summary> Returns the corresponding value of the Bezier curve supplied via [vals] at x=[t]. </summary>
        /// <param name="t"> Value to use as 'x' to get corresponding Bezier curve value. </param>
        /// <param name="vals"> Collection of values spaced evenly between [0,1] (inclusive) which the Bezier curve will be formed from. </param>
        public static float BezierX(float t, List<float> vals)
        {
            if (vals.Count <= 2)
                return Lerp(t, 0, 1);
            else
            {
                float s = 1 - t;
                int numVals = vals.Count;

                var pas = GetPascalRow(numVals - 1);

                float ret = 0;

                for (int i = 0; i < numVals; i++)
                {
                    ret += pas[i] * vals[i] * Power(s, (numVals - 1) - i) * Power(t, i);
                }

                return ret;
            }
        }




        #endregion


    }

    internal struct BetterPow
    {
        public BetterPow(float startPow)
        {
            PowWhole = (int)Math.Floor(startPow);
            PowDecimal = startPow - PowWhole;
        }

        public int PowWhole;
        public float PowDecimal;
    }


}
