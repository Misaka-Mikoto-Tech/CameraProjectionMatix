using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class CameraDemo : MonoBehaviour
{
    GameObject gameobject;

    public Camera cam;
    public Transform[] startPoints;
    public Transform[] projectionPoints;
    // Start is called before the first frame update

    public Transform[] drawCubePoints;


    public Transform startPos;
    public Transform endPos;

    private void Start()
    {
        //cam.fieldOfView = 90;
        List<Vector3> lists = GetPosLocation();
        for (int i = 0; i < 8; i++)
        {
            startPoints[i].position = lists[i];
            Vector3 targetPos = GetCameraPos(lists[i]);
          
            Vector4 resultProjection = GetProjectionPos(GetCameraPos(lists[i]));
            if(resultProjection.w!=0)
            {
                projectionPoints[i].position = resultProjection / resultProjection.w;
            }

        }
    }
    private void Update()
    {
        DrawLine(drawCubePoints);
        Vector4 resultProjection = GetProjectionPos(GetCameraPos(startPos.position));
        float value = resultProjection.w > 0 ? resultProjection.w : -resultProjection.w;
        if(value!=0)
        {

            endPos.position = new Vector3(resultProjection.x / value, resultProjection.y / value, resultProjection.z / value);
                Debug.Log(resultProjection);
        }
    }


    void DrawLine(Transform[] points)
    {
        for (int i = 1; i < points.Length; i++)
        {
            Debug.DrawLine(points[i].position,  points[i - 1].position);

        }
    }
    List<Vector3> GetPosLocation()
    {
        List<Vector3> backList=new List<Vector3>();
        for (int z = 0; z < 2; z++)
        {
            for (int i = -1; i < 2; i += 2)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    Vector3 pos = GetPos(z, new Vector2Int(i, j) , cam);
                    backList.Add(pos);
                }
            }
        }
        return backList;
    }
    Vector3 GetPos(int lenType,Vector2Int dirType,Camera cam)
    {
        Vector3 cPos = cam.transform.position;
        //根据FOV得到水平与垂直角度一般对应的弧度
        float vecAngle = (cam.fieldOfView*Mathf.PI)/360;
        float horAngle = Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect) * Mathf.PI/360;        
        float zoffset  = lenType == 0 ? cam.nearClipPlane : cam.farClipPlane;
        float vecOffset = zoffset * Mathf.Tan(vecAngle);
        float horOffset = zoffset * Mathf.Tan(horAngle);
        Vector3 offsetV3 = new Vector3(horOffset * dirType.x, vecOffset * dirType.y, zoffset);
        return cPos + offsetV3;
    }

    Vector4 GetCameraPos(Vector3 startPos)
    {
        return cam.worldToCameraMatrix * new Vector4(startPos.x, startPos.y, startPos.z, 1);

    }

    Vector4 GetProjectionPos(Vector4 cameraSpacePos)
    {
        // Debug.Log(cameraSpacePos);
        return cam.projectionMatrix * cameraSpacePos;

    }


    private void OnDrawGizmos()
    {
        //相机投影矩阵
        Matrix4x4 mat = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, 0, cam.aspect);
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        //点位绘制
        Gizmos.matrix = mat;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, 0, 50));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, 50, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, new Vector3(50, 0, 0));
        //视锥



    }

  


}

