using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // ヘッダーの設定
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private int _hp;
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private bool _bJump;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dのコンポーネント取得
        _rigid = GetComponent<Rigidbody2D>();
        _bJump = false;
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        Debug.Log(_hp);
        _LookMoveDirec();
    }

    // 移動処理
    private void _Move()
    {
        // X軸の移動
        _rigid.velocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.velocity.y);
        // 歩く動作のbool変更
        _anim.SetBool("Walk", _inputDirection.x != 0);
    }

    // 進行方向でプレイヤーの向きを決める
    private void _LookMoveDirec()
    {
        // 右へ進んでいる時
        if (_inputDirection.x > 0.0f)
        {
            // eulerAnglesはオブジェクトの角度をオイラー角(回転)で指定
            transform.eulerAngles = Vector3.zero;
        }
        // 左へ進んでいる時
        else
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    // 移動した入力情報を代入する
    public void OnMove(InputAction.CallbackContext context)
    {
        // vector2型に変換
        _inputDirection = context.ReadValue<Vector2>();
    }

    // 当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面
        if (collision.gameObject.tag == "Floor")
        {
            _bJump = false;
            _anim.SetBool("Jump", _bJump);
        }

        // 敵
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
            // layerの名前を取得
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            // コルーチンで関数の実行
            StartCoroutine(_Damage());
        }
    }

    // 敵を倒す処理
    private void _HitEnemy(GameObject enemy)
    {
        // 縦の大きさの半分
        float halfScaleY = transform.lossyScale.y / 2.0f;
        // 敵の半分の大きさ
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;

        // 敵を踏める条件
        if (transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        // ダメージを受ける
        else
        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
        }
    }

    // 敵にダメージを受けた後、無敵時間の付与　(コルーチン)
    IEnumerator _Damage()
    {
        // 初期値：白
        Color color = _spriteRenderer.color;
        for (int i = 0; i < _damageTime; i++)
        {
            // 処理を一度中断
            yield return new WaitForSeconds(_flashTime);
            // float型の引数：赤、緑、青、透明度
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            // 処理を一度中断
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        // 色を元に戻す
        _spriteRenderer.color = color;
        // layerをdefaultに戻す
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    // 敵を倒した時
    private void _Dead()
    {
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Jumpアクション
    public void OnJump(InputAction.CallbackContext context)
    {
        // jumpキーではない場合
        if (!context.performed || _bJump) return;

        // 上方向に力を加える
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);  // ForceMode2D.Impulseは初速が早く、徐々に減速させる
        _bJump = true;
        _anim.SetBool("Jump", _bJump);
    }

    // ダメージ処理
    public void Damage(int damage)
    {
        // 0と比べて大きい方の数値を_hpに代入
        _hp = Mathf.Max(_hp - damage, 0);
        _Dead();
    }

    // 体力の取得
    public int GetHP()
    {
        return _hp;
    }
}
