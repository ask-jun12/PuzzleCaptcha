using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// csv出力
public class WriteCsv : MonoBehaviour
{
    GameObject piece; // 可動ピース
    ProcessPiece piecePPscr; // 可動ピースのProcessPiece.cs
    SetPosition SPscr;
    SetRandom SRscr;
    private Vector3 startPos; // 可動ピースの初期位置(SPscr)
    private Vector3 goalPos; // 可動ピースの目標位置
    private Vector3 prevPos; // 一つ前の位置
    private string directory = "C:/Users/jun12/Desktop/kenkyu/PuzzleCaptcha/csv/";
    private string fileNamePos = "position.csv";
    private string fileNameVel = "velocity.csv";

    // シーン初め
    void Start()
    {
        this.SRscr = this.GetComponent<SetRandom>();
        int PieceNo = SRscr.SelectNo;
        this.piece = transform.GetChild(PieceNo).gameObject;
        this.piecePPscr = piece.GetComponent<ProcessPiece>();

        this.SPscr = this.GetComponent<SetPosition>();
        this.startPos = SPscr.startPos;
        this.goalPos = new Vector3(SPscr.piecePosX[PieceNo], SPscr.piecePosY[PieceNo], 0);

        this.prevPos = SPscr.startPos;
    }

    private bool isDrag; // ドラッグ中かどうか
    private float timeElapsed;
    public float timeOut; // サンプリング間隔
    private float timeAcc = 0; // サンプリング時のtimeElapsedを累積

    // 毎フレーム
    void Update()
    {
        this.isDrag = piecePPscr.isDrag;
        this.timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeOut) { // サンプリング間隔
            if(isDrag == true) { // ドラッグ中
                this.timeAcc += timeElapsed;
                WritePos();
                writeVelocity();
            }
            timeElapsed = 0.0f;
        }
    }

    // 座標ログを出力
    private void WritePos()
    {
        StreamWriter swLEyeLog;
        FileInfo fiLEyeLog = new FileInfo(this.directory + this.fileNamePos);
        swLEyeLog = fiLEyeLog.AppendText();
        swLEyeLog.Write(this.timeAcc); swLEyeLog.Write(", ");
        swLEyeLog.Write(piece.transform.position.x); swLEyeLog.Write(", ");
        swLEyeLog.WriteLine(piece.transform.position.y);
        swLEyeLog.Flush();
        swLEyeLog.Close();
    }

    private Vector3 nowPos;

    // 速度を出力
    private void writeVelocity()
    {
        this.nowPos = piece.transform.position;
        float dis = Mathf.Abs(Vector3.Distance(this.prevPos, this.nowPos));
        float velocity = dis / this.timeElapsed;

        StreamWriter swLEyeLog;
        FileInfo fiLEyeLog = new FileInfo(this.directory + this.fileNameVel);
        swLEyeLog = fiLEyeLog.AppendText();
        swLEyeLog.Write(this.timeAcc); swLEyeLog.Write(", ");
        swLEyeLog.WriteLine(velocity);
        swLEyeLog.Flush();
        swLEyeLog.Close();

        this.prevPos = this.nowPos;
    }
}
