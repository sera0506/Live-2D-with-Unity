/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System.Collections;
using UnityEngine;

namespace live2d.framework
{
    public class L2DMatrix44
    {
        protected float[] tr = new float[16];// 行列データ

        public L2DMatrix44()
        {
            identity();
        }

        public L2DMatrix44(float[] f)
        {
            identity();
            setMatrix(f);
        }

        public void identity()
        {
            for (int i = 0; i < 16; i++) tr[i] = ((i % 5) == 0) ? 1 : 0;
        }


        /*
         * 行列配列を取得
         * @return
         */
        public float[] getArray()
        {
            return tr;
        }


        /*
         * 行列配列のコピーを取得
         * @return
         */
        public float[] getCopyMatrix()
        {
            return (float[])(tr.Clone());	// 配列のコピーを返す
        }


        /*
         * 行列を設定
         * 長さ16の配列でないときは何もしない
         * @param tr
         */
        public void setMatrix(float[] tr)
        {
            // 配列の長さが異なる場合は何もしない
            if (tr == null || this.tr.Length != tr.Length) return;
            for (int i = 0; i < 16; i++) this.tr[i] = tr[i];
        }


        public float getScaleX()
        {
            return tr[0];
        }


        public float getScaleY()
        {
            return tr[5];
        }


        /*
         * xの値を現在の行列で計算する。
         * @param src
         * @return
         */
        public float transformX(float src)
        {
            return tr[0] * src + tr[12];
        }


        /*
         * yの値を現在の行列で計算する。
         * @param src
         * @return
         */
        public float transformY(float src)
        {
            return tr[5] * src + tr[13];
        }


        /*
         * xの値を現在の行列で逆計算する。
         * @param src
         * @return
         */
        public float invertTransformX(float src)
        {
            return (src - tr[12]) / tr[0];
        }


        /*
         * yの値を現在の行列で逆計算する。
         * @param src
         * @return
         */
        public float invertTransformY(float src)
        {
            return (src - tr[13]) / tr[5];
        }


        /*
         * 受け取った2つの行列の掛け算を行う。
         * @param a 入力1
         * @param b 入力2
         * @param dst 出力
         */
        protected static void mul(float[] a, float[] b, float[] dst)
        {
            float[] c = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int n = 4;
            int i, j, k;

            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (k = 0; k < n; k++)
                    {
                        c[i + j * 4] += a[i + k * 4] * b[k + j * 4];
                    }
                }
            }

            for (i = 0; i < 16; i++)
            {
                dst[i] = c[i];
            }
        }

        protected static void mul(L2DMatrix44 a, L2DMatrix44 b, L2DMatrix44 dst)
        {
            mul(a.tr, b.tr, dst.tr);
        }

        /*
         * 移動量の計算。
         * @param shiftX
         * @param shiftY
         */
        public void multTranslate(float shiftX, float shiftY)
        {
            float[] tr1 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, shiftX, shiftY, 0, 1 };
            mul(tr1, tr, tr);
        }


        public void translate(float x, float y)
        {
            tr[12] = x;
            tr[13] = y;
        }


        public void multTranslate(float shiftX, float shiftY, float shiftZ)
        {
            float[] tr1 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, shiftX, shiftY, shiftZ, 1 };
            mul(tr1, tr, tr);
        }


        public void translate(float x, float y, float z)
        {
            tr[12] = x;
            tr[13] = y;
            tr[14] = z;
        }


        public void translateX(float x)
        {
            tr[12] = x;
        }


        public void translateY(float y)
        {
            tr[13] = y;
        }


        /*
         * 回転角の計算。
         * @param sin
         * @param cos
         */
        public void multRotateX(float sin, float cos)
        {
            float[] tr1 = { 1, 0, 0, 0, 0, cos, sin, 0, 0, -sin, cos, 0, 0, 0, 0, 1 };
            mul(tr1, tr, tr);
        }


        public void multRotateY(float sin, float cos)
        {
            float[] tr1 = { cos, 0, -sin, 0, 0, 1, 0, 0, sin, 0, cos, 0, 0, 0, 0, 1 };
            mul(tr1, tr, tr);
        }


        public void multRotateZ(float sin, float cos)
        {
            float[] tr1 = { cos, sin, 0, 0, -sin, cos, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
            mul(tr1, tr, tr);
        }


        /*
         * 拡大率の計算。
         * @param scaleX
         * @param scaleY
         */
        public void multScale(float scaleX, float scaleY)
        {
            float[] tr1 = { scaleX, 0, 0, 0, 0, scaleY, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
            mul(tr1, tr, tr);
        }


        public void scale(float scaleX, float scaleY)
        {
            tr[0] = scaleX;
            tr[5] = scaleY;
        }


        public void multScale(float scaleX, float scaleY, float scaleZ)
        {
            float[] tr1 = { scaleX, 0, 0, 0, 0, scaleY, 0, 0, 0, 0, scaleZ, 0, 0, 0, 0, 1 };
            mul(tr1, tr, tr);
        }


        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            tr[0] = scaleX;
            tr[5] = scaleY;
            tr[10] = scaleZ;
        }

        /*
	     * 任意の軸で回転。（度数で角度を指定）
	     * 
	     * 
	     * 単位ベクトルを入れる！！！
	     * 
	     */
        public L2DMatrix44 multRotate(float Q_deg, float[] v)
        {
            mul(this,createRotate(Q_deg, v[0], v[1], v[2]),this);
            return this;
        }

        public L2DMatrix44 multRotate(float Q_deg, Vector3 v)
        {
            mul(this, createRotate(Q_deg, v[0], v[1], v[2]), this);
            return this;
        }

        /*
         * 任意の軸で回転。（度数で角度を指定）
         * 
         * 単位ベクトルを入れる！！！
         * 
         */
        public static L2DMatrix44 createRotate(float Q_deg, float vx, float vy, float vz)
        {
            float xx = vx * vx;
            float yy = vy * vy;
            float zz = vz * vz;

            float len = xx + yy + zz;//二乗距離
            //if (!UtMath.equals(len, 1, UtMath.GOSA))
            //{
                // throw new RuntimeException(tr("DESI-0013") + vx + " , " + vy + " , " + vz + " // len=" + len);	
                // tr("Illegal State : 必ず回転軸は単位ベクトルである必要があります ")
            //}

            float DEG2RAD = Mathf.PI / 180.0f;
            Q_deg *= DEG2RAD;

            float s, c;

            s = Mathf.Sin(Q_deg);
            c = Mathf.Cos(Q_deg);

            return new L2DMatrix44(new float[]{
				xx *(1-c)+c  		, vx*vy*(1-c)+vz*s 	, vz*vx*(1-c)-vy*s 	, 0 ,
				vx*vy*(1-c)-vz*s 	, yy *(1-c)+c		, vy*vz*(1-c)+vx*s 	, 0 ,//見た目上は転置されている
				vz*vx*(1-c)+vy*s	, vy*vz*(1-c)-vx*s	, zz *(1-c)+c		, 0 ,
				 0 , 0 , 0 , 1 ,
		});
        }

        public override string ToString()
        {
            string ret="";
            for (int i = 0; i < 4 ; i++) {
			    for (int j = 0; j < 4 ; j++) {
                    ret+=string.Format("{0:F4} ",tr[i + (j<<2)])  ;
			    }
                ret+="\n";
		    }
            return ret;
        }
    }
}