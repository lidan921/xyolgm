using UnityEngine;
using System.Collections;

public class FingerEvent : MonoBehaviour {

    public static FingerEvent Instance;

    /// <summary>
    /// 玩家点击地面委托
    /// </summary>
    public System.Action OnPlayerClick;

    /// <summary>
    /// 手指方向枚举
    /// </summary>
    public enum FingerDir
    {
        Left,
        Right,
        Up,
        Down
    }

    /// <summary>
    /// 滑动委托
    /// </summary>
    public System.Action<FingerDir> OnFingerDrag;

    public enum ZoomType
    {
        In,
        Out
    }


    public System.Action<ZoomType> OnZoom;


    private Vector2 m_TempFinger1pos;
    private Vector2 m_TempFinger2pos;

    private Vector2 m_OldFinger1pos;
    private Vector2 m_OldFinger2pos;


    /// <summary>
    /// 手指上一个位置
    /// </summary>
    private Vector2 m_01dFingerPos;

    /// <summary>
    /// 手指滑动方向
    /// </summary>
    private Vector2 m_Dir;

    /// <summary>
    /// 上一操作
    /// </summary>
    private int m_prevFinger = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (OnZoom != null)
            {
                OnZoom(ZoomType.Out);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (OnZoom != null)
            {
                OnZoom(ZoomType.In);
            }
        }
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                m_TempFinger1pos = Input.GetTouch(0).position;
                m_TempFinger2pos = Input.GetTouch(1).position;

                if (Vector2.Distance(m_TempFinger1pos, m_OldFinger2pos) < Vector2.Distance(m_TempFinger1pos, m_TempFinger2pos))
                {
                    if (OnZoom != null)
                    {
                        OnZoom(ZoomType.In);
                    }
                }
                else
                {
                    if (OnZoom != null)
                    {
                        OnZoom(ZoomType.Out);
                    }
                }
               
            }
        }
#endif


    }


    void OnEnable()
    {
    	//启动时调用，这里开始注册手势操作的事件。
    	
    	//按下事件： OnFingerDown就是按下事件监听的方法，这个名子可以由你来自定义。方法只能在本类中监听。下面所有的事件都一样！！！
        FingerGestures.OnFingerDown += OnFingerDown;
        //抬起事件
		FingerGestures.OnFingerUp += OnFingerUp;
	    //开始拖动事件
	    FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
        //拖动中事件...
        FingerGestures.OnFingerDragMove += OnFingerDragMove;
        //拖动结束事件
        FingerGestures.OnFingerDragEnd += OnFingerDragEnd; 

		//按下事件后调用一下三个方法
		FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;
	
	
		
    }

    void OnDisable()
    {
    	//关闭时调用，这里销毁手势操作的事件
    	//和上面一样
        FingerGestures.OnFingerDown -= OnFingerDown;
		FingerGestures.OnFingerUp -= OnFingerUp;
		FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= OnFingerDragEnd; 

		FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;

    }

    //按下时调用
    void OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        m_prevFinger = 1;
    }
	
	//抬起时调用
	void OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
        if (m_prevFinger == 1)
        {
            m_prevFinger = -1;

            if (OnPlayerClick != null)
            {
                OnPlayerClick();
            }
        }
	}
	
	//开始滑动
	void OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {

        m_prevFinger = 2;
        m_01dFingerPos = fingerPos;
    }

	//滑动结束
	void OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
	{
        m_prevFinger = 4;
	}
	//滑动中
    void OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        //if (UIViewUtil.Instance.OpenWindowCount>0)
        //{
        //    return;
        //}

        m_prevFinger = 3;
        Debug.Log("滑动中....");
        m_Dir = fingerPos - m_01dFingerPos;
        if (m_Dir.y < m_Dir.x && m_Dir.y > -m_Dir.x)
        {
            //向右
            Debug.Log("向右");
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Right);
            }
        }
        else if (m_Dir.y > m_Dir.x && m_Dir.y < -m_Dir.x)
        {
            //向左
            Debug.Log("向左");
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Left);
            }
        }
        else if (m_Dir.y > m_Dir.x && m_Dir.y > -m_Dir.x)
        {
            //向上
            Debug.Log("向上");
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Up);
            }
        }
        else
        {
            //向下
            Debug.Log("向下");
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Down);
            }
        }


    }

	
	//按下事件开始后调用，包括 开始 结束 持续中状态只到下次事件开始！
	void OnFingerStationaryBegin( int fingerIndex, Vector2 fingerPos )
	{
		
	}
	
	

}
