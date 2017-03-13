using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point2AVGLine : MonoBehaviour {

	///<summary>
	///raycastに失敗した場合の値は必ず100にしておく
	///返される配列は一番目が傾き二番目が切片
	///</summary>

	public static float[] point2line(Vector3[] vec){

				// 戻り値
        float[] rtn = {0, 0};
        // 平均
        float aveX = 0;
        float aveZ = 0;
        // 偏差
        float devX = 0;
        float devZ = 0;

        // 共分散
        float CovXZ = 0;
        // 分散
        float varianceX = 0;
        float varianceZ = 0;

        // 標本数
        int count = 0;

        for (int i = 0; i < vec.Length; i++)
        {
            // 交点が取得できているもののみ取得する
            if(vec[i].x != 100)
            {
                aveX += vec[i].x;
                aveZ += vec[i].z;
                count++;
            }
        }
        aveX /= count;
        aveZ /= count;

        for (int i = 0; i < count; i++)
        {
            devX = vec[i].x - aveX;
            devZ = vec[i].z - aveZ;
            CovXZ += devX * devZ;
            varianceX += devX * devX;
            varianceZ += devZ * devZ;
        }
        // 傾き
        rtn[0] = (float)(CovXZ / varianceX);
        // 切片
        rtn[1] = aveZ - rtn[0] * aveX;

        return rtn;

	}

}
