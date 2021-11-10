using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 座標ログをcsv出力
public class WritePosition : MonoBehaviour
{
    GameObject piece; // 可動ピース
    ProcessPiece piecePPscr; // 可動ピースのProcessPiece.cs
    SetPosition SPscr;
    SetRandom SRscr;
    private Vector3 firstPos; // 可動ピースの初期位置(SPscr)
    private Vector3 goalPos; // 可動ピースの目標位置

    // シーン初め
    void Start()
    {
        this.SRscr = this.GetComponent<SetRandom>();
        int PieceNo = SRscr.SelectNo;
        this.piece = transform.GetChild(PieceNo).gameObject;
        this.piecePPscr = piece.GetComponent<ProcessPiece>();

        this.SPscr = this.GetComponent<SetPosition>();
        this.firstPos = SPscr.firstPos;
        this.goalPos = new Vector3(SPscr.piecePosX[PieceNo], SPscr.piecePosY[PieceNo], 0);
    }

    private bool isDrag; // ドラッグ中かどうか
    private float timeElapsed;
    public float timeOut; // サンプリング間隔
    public float dis; // 始筆と終筆距離

    // 毎フレーム
    void Update()
    {
        this.isDrag = piecePPscr.isDrag;
        timeElapsed += Time.deltaTime;

        if(isDrag == true) { // ドラッグ中
            float disFirst = (piece.transform.position - firstPos).sqrMagnitude;
            float disGoal = (piece.transform.position - goalPos).sqrMagnitude;
            // サンプリング間隔
            if(timeElapsed >= timeOut) {
                WriteCSV("position.csv");

                if(disFirst < dis * dis){ // 始め
                    WriteCSV("position_first.csv");
                }
                else if(disGoal < dis * dis){ // 終わり
                    WriteCSV("position_goal.csv");
                }
                else { // 途中
                    WriteCSV("position_middle.csv");
                }

                timeElapsed = 0.0f;
            }
        }
    }

    private void WriteCSV(string fileName)
    {
        string dir = "C:/Users/jun12/Desktop/kenkyu/PuzzleCaptcha/csv/";
        StreamWriter swLEyeLog; // 全体
        FileInfo fiLEyeLog = new FileInfo(dir + fileName);

        swLEyeLog = fiLEyeLog.AppendText();
        swLEyeLog.Write(Time.time); swLEyeLog.Write(", ");
        swLEyeLog.Write(piece.transform.position.x); swLEyeLog.Write(", ");
        swLEyeLog.WriteLine(piece.transform.position.y);

        swLEyeLog.Flush();
        swLEyeLog.Close();
    }
}
