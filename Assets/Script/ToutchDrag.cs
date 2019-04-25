using UnityEngine;

public class ToutchDrag : MonoBehaviour
{
    #region PRIVATE_VARIABLES
    bool isDrag = false, firstDrag;
    float distance = 1f;
    float sensitive = 3f;
    Vector2 oldPosition1, oldPosition2;
    Vector3 dragPos;
    Transform hitTransfrom;
    Quaternion dragRotato;
    #endregion
    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        DragByX();
        DragByY();
        ScaleObj();
    }

    #endregion
    #region PRIVATE_METHODS

    #region FreeDrag
    /// <summary>
    /// 自由旋转
    /// </summary>
    void FreeDrag()
    {
        if (Input.GetMouseButton(0) && isDrag)
        {
            //isDrag = false;
            if (firstDrag)
            {
                transform.position = dragPos;
                transform.rotation = dragRotato;
                firstDrag = false;
            }

            float v = Input.GetAxis("Mouse Y");
            float h = Input.GetAxis("Mouse X");
            transform.Rotate(new Vector3(v * sensitive, -h * sensitive, 0f), Space.World);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            dragPos = transform.position;
            dragRotato = transform.rotation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            firstDrag = true;
        }
    }
    #endregion


    /// <summary>
    /// 按Y轴旋转
    /// </summary>
    void DragByY()
    {
#if !UNITY_EDITOR
        if(Input.GetTouch(0).phase == TouchPhase.Moved)
#endif
        {
            float h = Input.GetAxis("Mouse X");
            transform.Rotate(new Vector3(0, 1, 0), -h * sensitive,Space.World);
        }

    }

    /// <summary>
    /// 按X轴旋转
    /// </summary>
    void DragByX()
    {
#if !UNITY_EDITOR
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
#endif
        {
            float h = Input.GetAxis("Mouse Y");
            transform.Rotate(new Vector3(1, 0, 0), h * sensitive, Space.World);

            //transform.Rotate(new Vector3(h * sensitive, 0f, 0f), Space.World);
        }
    }
    /// <summary>
    /// 缩小和放大物体
    /// </summary>
    void ScaleObj()
    {
#if !UNITY_EDITOR
        //判断触摸数量为多点触摸
        if (Input.touchCount > 1)
        {
            //前两只手指触摸类型都为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                //计算出当前两点触摸点的位置
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;
                //函数返回真为放大，返回假为缩小
                if (IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    //放大系数超过3以后不允许继续放大
                    
                        if (distance < 4)
                        {
                            distance += Time.deltaTime * 10f;
                        }
                }
                else
                {
                    //缩小系数返回18.5后不允许继续缩小
                    if (distance > 1)
                    {
                        distance -= Time.deltaTime * 10f;
                    }
                }

                transform.localScale = new Vector3(distance, distance, distance);
                //备份上一次触摸点的位置，用于对比
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
#else
        //鼠标滚轮向上滑动
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance < 2)
            {
                distance += Time.deltaTime * 10f;
            }
        }
        //鼠标滚轮乡下滑动
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance > 1)
            {
                distance -= Time.deltaTime * 10f;
            }
        }
        this.transform.localScale = new Vector3(distance, distance,distance);

#endif

    }
    //函数返回真为放大，返回假为缩小
    bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        var leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        var leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            //放大手势
            return true;
        }
        else
        {
            //缩小手势
            return false;
        }
    }

    /// <summary>
    /// 允许对物体进行旋转和放大操作
    /// </summary>
    /// <param name="value"></param>
    public void EnableDrag(bool value)
    {
        isDrag = value;
    }
#endregion
}
