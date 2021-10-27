using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ピースに関する処理
public class ProcessPiece : MonoBehaviour
{
    GameObject setrandom;
    GameObject pieces;
    SetRandom SRscr;
    SetPosition SPscr;
    public int PieceNo; // 各ピースのナンバー
    public int SelectNo; // ランダムに選択されたナンバー
    public float[] piecePosX = new float[48];
    public float[] piecePosY = new float[48];
    public Vector3 firstPos; // 可動ピースの初期位置
    private float span; // ピースがワープする距離

    // シーン初め
    void Start()
    {
        // SetRandomScr.csから変数SelectNoを取得
        this.setrandom = GameObject.Find ("SetRandom");
        this.SRscr = setrandom.GetComponent<SetRandom>();
        this.SelectNo = SRscr.SelectNo;

        // SetPosition.csから配列piecePosX・Yを取得
        // Vector3型firstPos・変数spanを取得
        this.pieces = GameObject.Find ("Pieces");
        this.SPscr = pieces.GetComponent<SetPosition>();
        this.piecePosX = SPscr.piecePosX;
        this.piecePosY = SPscr.piecePosY;
        this.firstPos = SPscr.firstPos;
        this.span = SPscr.span;

        // pieceのNo.を取得
        string num = Regex.Replace (transform.name, @"[^0-9]", "");
        this.PieceNo = int.Parse(num);

        // パズルのピースを設置
        if (this.PieceNo == this.SelectNo)
        {
            this.transform.position = firstPos;//
        }
        else
        {
            this.transform.position = new Vector3(this.piecePosX[this.PieceNo], this.piecePosY[this.PieceNo], 0);//
        }
    }

    // ドラッグ処理
    public void OnMouseDrag()
    {
        // ランダムに選択されたナンバーのピースのみ可動
        if (this.PieceNo == this.SelectNo)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 99; // 最前面へ
            Vector3 ObjPos = Input.mousePosition;
            ObjPos.z = 10f;
            transform.position = Camera.main.ScreenToWorldPoint(ObjPos);
        }
    }

    // ドロップ処理
    public void OnMouseUp() // +async
    {
        // ドロップ時のピースのｘｙ座標を取得
        float objx = this.transform.position.x;//
        float objy = this.transform.position.y;//

        // ポジションからのオフセット
        float leftx = this.piecePosX[this.SelectNo] - this.span;
        float rightx = this.piecePosX[this.SelectNo] + this.span;
        float upy = this.piecePosY[this.SelectNo] + this.span;
        float downy = this.piecePosY[this.SelectNo] - this.span;

        // ポジションからオフセット内にドロップされたかの判定
        if (leftx <= objx && objx <= rightx)
        {
            if (upy >= objy && objy >= downy)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 2; // 最背面へ
                this.transform.position = new Vector3(this.piecePosX[this.SelectNo], this.piecePosY[this.SelectNo], 0);//
                Debug.LogError("success");
                // await DelayMethod(); // 遅延
                // SceneManager.LoadScene("GameScene"); // 次のパズルへ
                Application.Quit();
            }
        }
        else
        {
            Debug.LogWarning("miss");
            // await DelayMethod(); // 遅延
            // SceneManager.LoadScene("GameScene"); // 次のパズルへ
            Application.Quit();
        }
    }

    // 遅延処理
    private async Task DelayMethod()
    {
        await Task.Delay(1000);
    }
}